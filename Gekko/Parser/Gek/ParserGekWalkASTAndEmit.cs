using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Text;

namespace Gekko.Parser.Gek
{
    
    public class GekkoSB
    {
        private StringBuilder storage = null;
        
        public GekkoSB()
        {
            //this.storage = new StringBuilder();
            this.storage = null;
        }

        public bool IsNull() {
            if (this.storage == null) return true;
            return false;
        }   

        public override string ToString()
        {
            if (this.storage == null)
            {
                return null;
            }
            return this.storage.ToString();
        }

        public bool Contains(string s)
        {
            if (this.storage == null) return false;
            return this.storage.ToString().Contains(s);
        }

        public GekkoSB A(string s)
        {
            if (this.storage == null) this.storage = new StringBuilder();                
            this.storage.Append(s);
            return this;
        }        

        public GekkoSB A(int i)
        {
            if (this.storage == null) this.storage = new StringBuilder();
            this.storage.Append(i.ToString());
            return this;
        }

        public GekkoSB A(GekkoSB sb)
        {
            if (this.storage == null) this.storage = new StringBuilder();                
            this.storage.Append(sb.storage);
            return this;
        }

        public void CA(string s)
        {
            if (this.storage == null) this.storage = new StringBuilder();                
            this.storage.Clear();
            this.storage.Append(s);
        }

        public void CA(GekkoSB sb)
        {
            if (this.storage == null) this.storage = new StringBuilder();                
            this.storage.Clear();
            this.storage.Append(sb.storage);
        }

        public GekkoSB End()
        {
            if (this.storage == null) throw new GekkoException();
            this.storage.Append(";" + G.NL);
            return this;
        }

        public void Prepend(string s)
        {
            if (s == null || s.Trim() == "") return;
            if (this.storage == null) this.storage = new StringBuilder();            
            this.storage.Insert(0, s);
        }
        
        public void Replace(string s1, string s2)
        {
            this.storage.Replace(s1, s2);
        }

        public void LoopSmplCode(string s)
        {
            this.A(Globals.startGekkoSmplIteratorCode);
            this.A(s).End();            
            this.A(Globals.endGekkoSmplIteratorCode);
            this.Replace(Globals.gekkoSmplIteratorName, (++Globals.counter).ToString());
        }
    }

    
    public enum EOptionType
    {
        String,
        Val,
        Date
    }

    public class ParserGekWalkASTAndEmit
    {   

        public enum ELastCommand
        {
            Unknown,
            Genr,
            Val,
            String,
            Date
        }

        public static string GetStringFromIdent(ASTNode node)
        {
            if (node.Text != "ASTIDENT")
            {
                new Error("#8937524309");
                //throw new GekkoException();
            }
            return node[0].Text;
        }

        public static void FindFunctionsUsedInGekkoCode(ASTNode node, Dictionary<string, int> functions)
        {
            //asdfg added [0]
            //if (node.Text == "ASTFUNCTION" || node.Text == "ASTFUNCTION_Q")
            if (node[0].Text == "ASTFUNCTION" || node[0].Text == "ASTFUNCTION_Q")
            {                
                string functionName = GetFunctionName(node);
                if (!functions.ContainsKey(functionName)) functions.Add(functionName, 1);  //1 just arbitrary            
            }
            foreach (ASTNode child in node.ChildrenIterator())
            {
                FindFunctionsUsedInGekkoCode(child, functions);                
            }
        }

        public static void WalkASTToCheckSumFunction(ASTNode node, int[] found)
        {
            switch (node.Text)
            {
                case "ASTINDEXER":  //indexer
                    {
                        //ASTINDEXERELEMENT, x[#i] or x['a', #i]
                        string s = null;
                        for (int i = 0; i < node.ChildrenCount(); i++)
                        {
                            try
                            {
                                s = node[i][0][0][0][0].Text;
                            }
                            catch { };                            
                        }
                        if (s == "ASTHASH") found[0] = 1;
                    }
                    break;
                case "ASTCURLY":  //indexer, x{#i}
                    {                        
                        string s = null;
                        try
                        {
                            s = node[0][0][0][0].Text;
                        }
                        catch { };
                        if (s == "ASTHASH") found[0] = 1;
                    }
                    break;
            }
            if (found[0] == 1) return;
            foreach (ASTNode child in node.ChildrenIterator())
            {
                WalkASTToCheckSumFunction(child, found);                
            }
        }

        public static void WalkASTAndEmitUnfold(ASTNode node, int depth)
        {
            //before subnodes
            //locates x[#m] or x[#m+...] or x[#m-...], or same for curlies         
            //The #m is assigned to sum() or unfold() or to PRTELEMENT (last one will be converted to unfold()).
            //Also in x[#m] = y[#m] + ... , the #m is assigned to ASTASSIGNMENT
            //also locates ... $ (#m in ...)
            // ---> these sets are assigned to a "parent", either sum() or unfold() function.
            //      sets that are in conditions like "a.val = 15" will later on point to their "parent", 
            //      so that they are treated like elements and not lists when the code i rum.

            //Also locates listfiles via ASTBANKVARNAME2. For instance #(listfile m) or #(listfile {'m'})
            //the former will work with sum(), unfold() etc.

            WalkAstAndEmitUnfoldBefore(node);

            foreach (ASTNode child in node.ChildrenIterator())
            {
                WalkASTAndEmitUnfold(child, depth + 1);
            }
            //after subnodes

            WalkAstAndEmitUnfoldAfter(node);
        }

        private static void WalkAstAndEmitUnfoldAfter(ASTNode node)
        {
            if (node.freeIndexedLists != null && node.freeIndexedLists.Count > 0)
            {

                if (node.Text == "ASTASSIGNMENT" || node.Text == "ASTEVAL")  //#p24234oi34
                {
                    //augment SearchUpwardsInTree2() in normal Walker so that it also checks
                    //if it is a left-side looper
                    //what about ser x[#i] = 1 + sum(#i, x[#i]) + x[#i], sum should fail. must use alias. or allow it??
                    //int i = node.freeIndexedListsLeftSide.Count;

                    //ok to just reassign, we are not going to add to any of these dictionaries anyway

                    if (node.listLoopAnchor == null) node.listLoopAnchor = new GekkoDictionary<string, TwoStrings>(StringComparer.OrdinalIgnoreCase);
                    foreach (string s in node.freeIndexedLists.Keys)
                    {
                        //these are for an unfold function
                        node.listLoopAnchor.Add(s, new TwoStrings(Globals.listLoopInternalName + s + ++Globals.counter, "unfold"));
                    }
                }
                else if (node.Text == "ASTPRTELEMENT")
                {
                    if (node[0].Text == "ASTEXPRESSION")
                    {

                        List<string> xx = new List<string>();
                        foreach (string ss in node.freeIndexedLists.Keys)
                        {
                            xx.Add(ss);
                        }
                        xx.Sort(StringComparer.OrdinalIgnoreCase);

                        //here: unfolding 1 list (#m)

                        //    ASTPRTELEMENT
                        // 0    ASTEXPRESSION
                        // 1      ASTFUNCTION    <----------- our insert begins here
                        // 1a       ASTPLACEHOLDER    asdfg new node
                        // 2          ASTIDENT                          
                        // 3            unfold
                        // 2a         ASTPLACEHOLDER  <-- normally contains an ident with library name
                        // 3a       ASTSPECIALARGS
                        // 4        ASTBANKVARNAME         <-- if there are > 1 lists, an LISTDEF and LISTDEFITEM node is inserted here, and the ASTBANKVARNAME nodes are subnodes
                        // 5          ASTPLACEHOLDER
                        // 6          ASTVARNAME
                        // 7            ASTPLACEHOLDER
                        // 8              ASTHASH
                        // 9            ASTPLACEHOLDER
                        //10              ASTNAME
                        //11                ASTIDENT
                        //12                  m
                        //13            ASTPLACEHOLDER
                        //        AST ----------------> orignial code after ASTEXPRESSION comes here

                        ASTNode n0 = new ASTNode("ASTEXPRESSION", true);
                        ASTNode n1 = new ASTNode("ASTFUNCTION", true);
                        //asdfg new node:
                        ASTNode n1a = new ASTNode("ASTPLACEHOLDER", true);
                        ASTNode n2 = new ASTNode("ASTIDENT", true);
                        //asdfg new node:
                        ASTNode n2a = new ASTNode("ASTPLACEHOLDER", true);
                        ASTNode n3 = new ASTNode("unfold", true);
                        ASTNode n3a = new ASTNode("ASTSPECIALARGSDEF", true);

                        n0.Add(n1);
                        if (false)
                        {
                            n1.Add(n2);
                        }
                        else
                        {
                            //asdfg rewiring
                            n1.Add(n1a);
                            n1a.Add(n2);
                            n1a.Add(n2a);
                        }
                        n2.Add(n3);
                        n1.Add(n3a);

                        ASTNode list = new ASTNode("ASTLISTDEF", true);

                        for (int i = 0; i < xx.Count; i++)
                        {
                            ASTNode listDefItem = new ASTNode("LISTDEFITEM", true);  //ZXCVB
                            ASTNode n4 = new ASTNode("ASTBANKVARNAME", true);
                            ASTNode n5 = new ASTNode("ASTPLACEHOLDER", true);
                            ASTNode n6 = new ASTNode("ASTVARNAME", true);
                            ASTNode n7 = new ASTNode("ASTPLACEHOLDER", true);
                            ASTNode n8 = new ASTNode("ASTHASH", true);
                            ASTNode n9 = new ASTNode("ASTPLACEHOLDER", true);
                            ASTNode n10 = new ASTNode("ASTNAME", true);
                            ASTNode n11 = new ASTNode("ASTIDENT", true);
                            ASTNode n12 = new ASTNode(xx[i], true);
                            ASTNode n13 = new ASTNode("ASTPLACEHOLDER", true);
                            listDefItem.Add(n4); //ZXCVB
                            n4.Add(n5);
                            n4.Add(n6);
                            n6.Add(n7);
                            n7.Add(n8);
                            n6.Add(n9);
                            n9.Add(n10);
                            n10.Add(n11);
                            n11.Add(n12);
                            n6.Add(n13);
                            if (xx.Count == 1)
                            {
                                n1.Add(n4);  //ZXCVB
                            }
                            else
                            {
                                list.Add(listDefItem); //ZXCVB
                            }
                        }

                        if (xx.Count > 1)
                        {
                            n1.Add(list);
                        }

                        n1.Add(node[0][0]);  //original code goes to arg 2 of unfold function: unfold(#m, ...[here]...)
                        node.RemoveLast();
                        node.Add(n0);
                    }
                }
            }
        }

        private static void WalkAstAndEmitUnfoldBefore(ASTNode node)
        {
            if (node.Text == "ASTBANKVARNAME2")
            {
                //handles a listfile

                ASTNode name = node[1][1][0];
                ASTNode placeholder = node[1][1];

                //#895943275
                if (name[0].Text == "ASTIDENT")
                {
                    // ASTBANKVARNAME
                    //   ASTPLACEHOLDER
                    //   ASTVARNAME
                    //     ASTPLACEHOLDER
                    //       ASTHASH
                    //     ASTPLACEHOLDER
                    //       ASTNAME ---> name
                    //         ASTIDENT
                    //           i ------------>  changed
                    //     ASTPLACEHOLDER


                    name[0][0].Text = Globals.listfile + "___" + name[0][0].Text;
                }
                else
                {

                    // ASTBANKVARNAME
                    //   ASTPLACEHOLDER
                    //   ASTVARNAME
                    //     ASTPLACEHOLDER
                    //       ASTHASH
                    //     ASTPLACEHOLDER
                    //       ASTNAME ----------> [1][1][0], name, here an ASTCNAME is inserted with 'listfile' as first node
                    //         ASTCURLY
                    //           ASTSTRINGINQUOTES
                    //             'i' 
                    //     ASTPLACEHOLDER

                    // becomes...

                    // ASTBANKVARNAME
                    //   ASTPLACEHOLDER
                    //   ASTVARNAME
                    //     ASTPLACEHOLDER
                    //       ASTHASH
                    //     ASTPLACEHOLDER
                    //       ASTCNAME   ----------> here an ASTCNAME is inserted with 'listfile' as first node
                    //         ASTNAME  --> extraName
                    //           ASTIDENT
                    //             listfile_
                    //         ASTNAME -----------> name, is detached from ASTPLACEHOLDER above and attached here
                    //           ASTCURLY
                    //             ASTSTRINGINQUOTES
                    //               'i' 
                    //     ASTPLACEHOLDER


                    ASTNode cname = new ASTNode("ASTCNAME", true);
                    ASTNode extraname = new ASTNode("ASTNAME", true);
                    extraname.Add(new ASTNode("ASTIDENT", true));
                    extraname[0].Add(new ASTNode(Globals.listfile + "___"));
                    cname.Add(extraname);
                    cname.Add(name);
                    //name.Parent[0] = cname;
                    placeholder.RemoveLast();
                    placeholder.Add(cname);

                }
                node.Text = "ASTBANKVARNAME";
            }
            else if (node.Text == "ASTBANKVARNAME")  //p24234oi33
            {
                //a variable name like x or x!q or %x or #x
                string s = GetSimpleName(node);
                if (s != null && s[0] == Globals.symbolCollection)
                {
                    //here we are sure the variable name starts with '#'
                    string listnameWithoutSigil = s.Substring(1);
                    bool isInSpecialOperator = node.Parent.Text == "ASTCOMPARE" && node.Number == 1 && node.Parent[0][0].Text == "ASTIFOPERATOR7";
                    if (node.Parent.Text == "ASTINDEXERELEMENT" || node.Parent.Text == "ASTCURLY" || (node.Parent.Text == "ASTCOMPARE2" && node.Number == 1) || ((node.Parent.Text == "ASTPLUS" || node.Parent.Text == "ASTMINUS") && (node.Parent.Parent.Text == "ASTINDEXERELEMENT" || node.Parent.Parent.Text == "ASTCURLY")) || isInSpecialOperator)
                    {
                        //ASTPLUS/MINUS: see also #980752345
                        //Checks that #i is one of these:
                        //#i is inside a x[#i]
                        //#i is inside a x{#i} -- could also be stand alone {#i}                        
                        //#i is a x[#i+1] or x[#i-1] or the like
                        //#i is a x{#i+1} or x{#i-1} or the like
                        //#i is a #i2[#i] conditional
                        //#i is used like $(#i in #i2)
                        //
                        //Now look to see if #i is controlled by a sum(#i, ...). Also checks for unfold(#i, ...) but that should not be possible unless the user really types it explicitly
                        //If it is not controlled by sum(#i, ...), the following takes place if the #i is one of these:
                        //- appears on LHS
                        //- is a prt/plot/sheet element
                        //If so, freeIndexedLists will get augmented by #i. For LHS this means unrolling eqs, and for PRT etc. it means several columns.

                        //To handle PRT sum({#i}) or PRT sum(x{#i}), we could look at the first argument and check whether it is
                        //either #i OR #i $ ... OR (#i, #j).

                        //Regarding uncontrolled sets.
                        //If the set is inside a normal sum() function, it does not count as uncontrolled.
                        //A normal sum() function is a non-controlled sum function.
                        //A controlled sum() function is a sum() with exactly 2 argumetns, where
                        //  the first argument is either #i OR #i $ ... OR (#i, #j).
                                                                                                                                                   

                        if (true)
                        {
                            //This is almost completely identical to the code here: #lakhf8akjaf

                            bool isControlled = false;
                            bool isInsideNonControlledSumFunction = false;

                            ASTNode node2 = node.Parent.Parent;

                            while (true)
                            {
                                if (node2 == null || node2.Text == null) break;                                
                                int numberOfArguments = node2.ChildrenCount() - 2;
                                if (IsSumOrUnfoldFunction(node2))  //children = 4 --> has 2 "real" arguments
                                {
                                    if (numberOfArguments == 2)
                                    {
                                        if (node2[2].Text == "ASTBANKVARNAME")
                                        {
                                            string s2 = GetSimpleName(node2[2]);  //arg number 1
                                            if (G.Equal(s, s2))
                                            {
                                                isControlled = true; //no more search, #i in x[#i] in sum(#i, x[#i]) is controlled from a sum() function
                                            }
                                            if (string.IsNullOrEmpty(s2) || s2[0] != Globals.symbolCollection)
                                            {
                                                isInsideNonControlledSumFunction = true;
                                            }
                                        }
                                        else if (node2[2].Text == "ASTDOLLAR")
                                        {
                                            string s2 = GetSimpleName(node2[2][0]); //in arg number 1
                                            if (G.Equal(s, s2))
                                            {
                                                isControlled = true; //no more search, #i in x[#i] in sum(#i $ ..., x[#i]) is controlled from a sum() function
                                            }
                                            if (string.IsNullOrEmpty(s2) || s2[0] != Globals.symbolCollection)
                                            {
                                                isInsideNonControlledSumFunction = true;
                                            }
                                        }
                                        else if (node2[2].Text == "ASTLISTDEF")
                                        {
                                            foreach (ASTNode node3 in node2[2].ChildrenIterator())
                                            {
                                                string s2 = GetSimpleName(node3[0]);  //ZXCVB. //inside parenthesis in arg number 1
                                                if (G.Equal(s, s2))
                                                {
                                                    isControlled = true; //no more search, #i in x[#i] in sum((#i, #j), x[#i]) is controlled from a sum() function
                                                }
                                                if (string.IsNullOrEmpty(s2) || s2[0] != Globals.symbolCollection)
                                                {
                                                    isInsideNonControlledSumFunction = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        isInsideNonControlledSumFunction = true;
                                    }
                                }
                                else if (node2.Text == "ASTPRTELEMENT" || node2.Text == "ASTLEFTSIDE" || node2.Text == "ASTEVAL" || (node2.Text == "ASTASSIGNMENT" && G.Equal(node2[3].Text, "VAR_KDUSJFLQO2")))  //Note: we cannot have both of these in the same tree, they are always separate
                                {
                                    //The #i in x[#i] or similar does not seem to be controlled from an outer sum(#i, ...) function
                                    //Here we check if it is inside a more normal sum() function like for instance sum({#i}) or sum(x{#i}).
                                    //  if so, we do not put the #i on freeIndexedLists. Because in PRT/PLOT/SHEET, this would produce columns and be wrong. 

                                    ASTNode tmp = node2;
                                    if (node2.Text == "ASTLEFTSIDE")
                                    {
                                        tmp = node2.Parent;
                                        if (tmp.Text != "ASTASSIGNMENT")
                                        {
                                            new Error("Internal error #32468353233");  //see #32468353233
                                        }
                                    }

                                    if (!isControlled && !isInsideNonControlledSumFunction)
                                    {
                                        if (tmp.freeIndexedLists == null) tmp.freeIndexedLists = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                                        if (!tmp.freeIndexedLists.ContainsKey(listnameWithoutSigil)) tmp.freeIndexedLists.Add(listnameWithoutSigil, null);
                                    }
                                }

                                node2 = node2.Parent;
                            }
                        Label: node2 = node2;
                        }
                    
                    }
                }
            }
        }

        private static bool IsSumOrUnfoldFunction(ASTNode node2)
        {
            return (node2.Text == "ASTFUNCTION" || node2.Text == "ASTFUNCTION_Q") && (G.Equal(node2[0][0][0].Text, "sum") || G.Equal(node2[0][0][0].Text, "unfold"));
        }

        public static void WalkASTSimple(ASTNode node, int depth, ref int line)
        {
            if (line > 0) return;
            if (node != null)
            {
                if (node.Line > 0)
                {
                    line = node.Line;
                    return;
                }
                foreach (ASTNode child in node.ChildrenIterator())
                {
                    WalkASTSimple(child, depth + 1, ref line);
                }
            }
        }        

        public static void WalkASTAndEmit(ASTNode node, int absoluteDepth, int relativeDepth, string textInput, W w, P p)
        {

            if (node.Parent != null)
            {
                string s = null;
                node.commandLinesCounter = node.Parent.commandLinesCounter + s;  //default, may be overridden if new command is encountered.               
            }

            if (absoluteDepth == 1 && w.libraryName != null)
            {
                //a command at the first indentation level
                if (node.Text == "ASTFUNCTIONDEF2" || node.Text == "ASTPROCEDUREDEF")
                {
                    //good
                }
                else
                {
                    using (Error error = new Error())
                    {
                        error.MainAdd("You can only put functions and procedures inside libraries, not normal statements.");
                    }
                }
            }

            //HACK #438543
            if (node.Text == "ASTIFSTATEMENTS" || node.Text == "ASTELSESTATEMENTS" || node.Text == "ASTFUNCTIONDEFCODE" || node.Text == "ASTPROCEDUREDEFCODE")
            {
                relativeDepth = 0;  //new indentation level, used to know what is a command and what is stuff deeper down the tree
            }

            if (relativeDepth == 1)
            {
                //NEW COMMAND encountered
                //Corresponds to a new statement at current "indentation" level
                w.wh = new WalkHelper();
                w.wh.currentCommand = node.Text;  //if for instance a GENR statement, this field will be 'ASTGENR'
                w.commandLinesCounter++;  //becomes 0 first time here (starts at -1)
                node.commandLinesCounter = w.commandLinesCounter.ToString(); //used for the O(node)-method, so that o0, o1, o2 numbers do not suddenly change after for instance a FOR.
                w.expressionCounter = -1;  //for labels in PRT elements
            }

            WalkASTAndEmitBefore(node, w);

            foreach (ASTNode child in node.ChildrenIterator())
            {
                WalkASTAndEmit(child, absoluteDepth + 1, relativeDepth + 1, textInput, w, p);
            }

            //In general, we have this pattern
            //
            //   ASTCOPY (or other node)
            //     ASTOPT_
            //       ASTOPT_STRING_AS
            //       ASTOPT_DATE_END
            //       ASTOPT_DATE_START
            //   
            //The code below is to avoid a lot of repetitive coding regarding options to commands
            //
            // !!! probably not much used anymore, after option code has been changed (also in ANTLR)
            //
            //#astopt
            if (node.Text != null && node.Text.StartsWith("ASTOPT_STRING_"))
            {
                string s2 = node.Text.Substring(14);
                if (node.ChildrenCount() == 0) node.Code.A("o" + Num(node) + ".opt_" + s2.ToLower() + " = `yes`;" + G.NL);  //Using PRT<rows> instead of the more explicit PRT<rows=yes>.
                else
                {
                    node.Code.A("o" + Num(node) + ".opt_" + s2.ToLower() + " = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                }
            }
            else if (node.Text != null && node.Text.StartsWith("ASTOPT_VAL_"))
            {
                string s2 = node.Text.Substring(11);
                if (node.ChildrenCount() == 0) throw new GekkoException();
                else node.Code.A("o" + Num(node) + ".opt_" + s2.ToLower() + " = O.ConvertToVal(" + node[0].Code + ");" + G.NL);
            }
            else if (node.Text != null && node.Text.StartsWith("ASTOPT_DATE_"))
            {
                string s2 = node.Text.Substring(12);
                if (node.ChildrenCount() == 0) throw new GekkoException();
                else node.Code.A("o" + Num(node) + ".opt_" + s2.ToLower() + " = O.ConvertToDate(" + node[0].Code + ", O.GetDateChoices.Strict);" + G.NL);
            }
            else if (node.Text != null && node.Text.StartsWith("ASTOPT_LIST_"))
            {
                string s2 = node.Text.Substring(12);
                if (node.ChildrenCount() == 0) throw new GekkoException();
                else node.Code.A("o" + Num(node) + ".opt_" + s2.ToLower() + " = O.GetList(" + node[0].Code + ");" + G.NL);
            }
            else if (node.Text != null && node.Text.StartsWith("ASTOPT_VAR_"))
            {
                string s2 = node.Text.Substring(11);
                if (node.ChildrenCount() == 0) throw new GekkoException();
                else node.Code.A("o" + Num(node) + ".opt_" + s2.ToLower() + " = " + node[0].Code + ";" + G.NL);
            }
            else
            {
                //After sub-nodes  
                WalkASTAndEmitAfter(node, w);                
            }  //end of switch on node.Text AFTER sub-nodes are done

            bool isGoto = false;
            if (w.wh != null && w.wh.isGotoOrTarget) isGoto = true;

            if (!isGoto)
            {

                if (relativeDepth == 1)
                {
                    //#982375: if it is 0, walk the sub-tree to see...                  

                    int line = node.Line;

                    if (node.Text == "ASTIF" || node.Text == "ASTIFOLD")
                    {
                        line = GetLineRecursive(node[0]);
                    }
                    else if (node.Text == "ASTFOR")
                    {
                        line = GetLineRecursive(node[0]);
                    }

                    //HACK #438543
                    if (Globals.special.ContainsKey(node.Text))
                    {
                        //do nothing
                        string putInBefore = G.NL + "p.SetText(@`¤" + line + "`);" + G.NL + w.wh?.localFuncsCode?.ToString() + G.NL;
                        node.Code.Prepend(putInBefore);
                    }
                    else
                    {
                        //#2384328423                        
                        string putInBefore = G.NL + Globals.splitStart + Num(node) + G.NL + "p.SetText(@`¤" + line + "`); " + Globals.gekkoSmplInitCommand + G.NL + w.wh?.localFuncsCode?.ToString() + G.NL;
                        node.Code.Prepend(putInBefore);
                    }

                    //HACK #438543: a hack on this hack...! To avoid it is getting printed > 1 time for the same statement                    
                    w.wh.localFuncsCode = null;
                }
            }

            if (node != null && absoluteDepth == 0)
            {
                //returning from the whole tree                
                //node.Code.A(Globals.splitSTART);
                foreach (ASTNode child in node.ChildrenIterator())
                {
                    node.Code.A(child.Code + G.NL);
                }
                //node.Code.A(Globals.splitSTOP);
            }
            if (relativeDepth == 1)
            {
                node.Code.A(G.NL + Globals.splitEnd + Num(node) + G.NL);
            }
        }

        private static void WalkASTAndEmitBefore(ASTNode node, W w)
        {
            //Before sub-nodes
            switch (node.Text)
            {

                case "ASTSERIESLHS":
                    {
                        w.wh.seriesHelper = WalkHelper.seriesType.SeriesLhs;
                    }
                    break;
                case "ASTSERIESRHS":
                    {
                        w.wh.seriesHelper = WalkHelper.seriesType.SeriesRhs;
                    }
                    break;
                case "ASTGENR":
                    {
                        //For now we just say that old GENR is all rhs (there will be no loop [#i] on lhs anyway!)
                        w.wh.seriesHelper = WalkHelper.seriesType.SeriesRhs;
                    }
                    break;
                case "ASTFOR":
                    {
                        List<string> varnames = GetForLoopVariables(node);  //TODO: more than 1...

                        foreach (string varname in varnames)
                        {
                            if (node.forLoopAnchor == null) node.forLoopAnchor = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            node.forLoopAnchor.Add(varname, Globals.forLoopName + ++Globals.counter);
                            if (node.forLoop == null) node.forLoop = new List<Tuple<string, string>>();
                            node.forLoop.Add(new Tuple<string, string>("IVariable", Globals.forLoopName + Globals.counter));
                        }
                    }
                    break;
                case "ASTFUNCTIONDEF2":
                case "ASTPROCEDUREDEF":

                    {
                        if (SearchUpwardsInTree8(node) != null)
                        {
                            new Error("Function definition inside function definition is not allowed");
                            throw new GekkoException();
                        }
                        string returnType = node[0].Text;
                        string functionName = GetFunctionName2(node);  //node[1]

                        if (node[2][0] != null && node[2][0].Text == "ASTSPECIALARGSDEF")  //f(<date %t1, date %t2>)
                        {
                            foreach (ASTNode child in node[2][0].ChildrenIterator())
                            {
                                FunctionHelper4(node, functionName, child);
                            }
                        }
                        else
                        {
                            if (node.functionDef == null) node.functionDef = new List<ArgHelper>();
                            node.functionDef.Add(new ArgHelper("date", Globals.functionArgName + ++Globals.counter, null, null));
                            node.functionDef.Add(new ArgHelper("date", Globals.functionArgName + ++Globals.counter, null, null));
                        }

                        foreach (ASTNode child in node[2].ChildrenIterator())
                        {
                            if (child.Text == "ASTSPECIALARGSDEF")
                            {
                                continue;  //<date %t1, date %t2> has been done above
                            }
                            //here we need to take into account default values, like function 
                            FunctionHelper4(node, functionName, child);
                        }
                        string ftype = null;
                        if (node.Text == "ASTPROCEDUREDEF") ftype = "void";
                        else ftype = returnType.ToLower();
                        node.functionType = ftype;

                    }
                    break;
                case "ASTFUNCTION_Q":
                case "ASTFUNCTION":  //kind of like ASTFUNCTIONDEF, but the difference is that these sum() functions may be nested, so the nodes themselves need to keep the anchor info
                    {
                        string functionName = GetFunctionName(node);
                        string[] listNames = IsGamsSumFunctionOrUnfoldFunction(node, functionName);

                        if (listNames != null && listNames.Length > 0 && listNames[0] != null)
                        {
                            if (node.listLoopAnchor == null) node.listLoopAnchor = new GekkoDictionary<string, TwoStrings>(StringComparer.OrdinalIgnoreCase);
                            foreach (string s in listNames)
                            {
                                if (node.listLoopAnchor.ContainsKey(s))
                                {
                                    new Error("The list " + Globals.symbolCollection + s + " is used several times for multidimensional looping in sum() or unfold() function");
                                    //throw new GekkoException();
                                }
                                node.listLoopAnchor.Add(s, new TwoStrings(Globals.listLoopInternalName + s + ++Globals.counter, functionName));
                            }
                        }
                    }
                    break;
                case "ASTLEFTSIDE":
                    {
                        node.ivTempVarName = "ivTmpvar" + ++Globals.counter;  //we use the counter value to hook up the rhs with the lhs                        
                    }
                    break;
                case "ASTMAPDEF":
                    {
                        node.mapTempVarName = "mapTmpvar" + ++Globals.counter;  //we use the counter value to hook up in a map def
                    }
                    break;
            }
        }

        private static void WalkASTAndEmitAfter(ASTNode node, W w)
        {
            switch (node.Text)
            {
                case "+":
                    {
                        node.Code.CA("O.Add(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "-":
                    {
                        node.Code.CA("O.Subtract(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "*":
                    {
                        node.Code.CA("O.Multiply(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "/":
                    {
                        node.Code.CA("O.Divide(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "ASTPOW":
                    {
                        node.Code.CA("O.Power(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;

                case "ASTLISTAND":
                    {
                        node.Code.CA("O.Intersect(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "ASTLISTOR":
                    {
                        node.Code.CA("O.Union(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "ASTPLUS":
                    {
                        node.Code.CA("O.Add(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "ASTMINUS":
                    {
                        node.Code.CA("O.Subtract(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;

                case "ASTSTAR":
                    {
                        node.Code.CA("O.Multiply(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "ASTREPSTAR":
                    {
                        node.Code.CA("Globals.scalarStringStar");
                    }
                    break;
                case "ASTDIV":
                    {
                        node.Code.CA("O.Divide(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "ASTPOWER":
                    {
                        node.Code.CA("O.Power(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "ASTXLINE":
                    {
                        node.Code.CA("new ScalarString(`xline`)");
                    }
                    break;
                case "ASTYLINE":
                    {
                        node.Code.CA("new ScalarString(`yline`)");
                    }
                    break;
                case "ASTLINESPOINTS":
                    {
                        node.Code.CA("new ScalarString(`linespoints`)");
                    }
                    break;
                case "ASTBOXES":
                    {
                        node.Code.CA("new ScalarString(`boxes`)");
                    }
                    break;
                case "ASTLINES":
                    {
                        node.Code.CA("new ScalarString(`lines`)");
                    }
                    break;
                case "ASTFILLEDCURVES":
                    {
                        node.Code.CA("new ScalarString(`filledcurves`)");
                    }
                    break;
                case "ASTSTEPS":
                    {
                        node.Code.CA("new ScalarString(`steps`)");
                    }
                    break;
                case "ASTPOINTS":
                    {
                        node.Code.CA("new ScalarString(`points`)");
                    }
                    break;
                case "ASTDOTS":
                    {
                        node.Code.CA("new ScalarString(`dots`)");
                    }
                    break;
                case "ASTIMPULSES":
                    {
                        node.Code.CA("new ScalarString(`impulses`)");
                    }
                    break;


                case "ASTAPPEND":
                    {
                        node.Code.CA("new ScalarString(`append`)");
                    }
                    break;
                case "ASTAT":
                    {
                        node.Code.CA("new ScalarString(`Ref:`)");
                    }
                    break;
                case "ASTSTARS":
                    {
                        node.Code.CA("new ScalarString(`**`)");
                    }
                    break;
                case "ASTTRIPLESTARS":
                    {
                        node.Code.CA("new ScalarString(`***`)");
                    }
                    break;
                case "ASTCOLON":
                    {
                        node.Code.CA("new ScalarString(`:`)");
                    }
                    break;
                case "ASTL0":
                    {

                        node.Code.CA("new ScalarString(`[`)");

                        if (true)
                        {
                            for (int i = 0; i < node.ChildrenCount(); i++)
                            {
                                for (int j = 0; j < node[i].ChildrenCount(); j++)
                                {
                                    node.Code.A(".Concat(null, ").A(node[i][j].Code).A(")");
                                }
                                if (i < node.ChildrenCount() - 1) node.Code.A(".Concat(null, new ScalarString(`, `))");  //a blank here, for prettier output and also suits keys in DECOMP dictionary etc.
                            }
                            node.Code.A(".Concat(null, new ScalarString(`]`))");
                        }
                    }
                    break;

                case "ASTSEQ7":
                    {

                        string s = null;
                        bool isFirst = true;
                        for (int i = 0; i < 3; i++)
                        {
                            if (node[i] != null)
                            {
                                for (int j = 0; j < node[i].ChildrenCount(); j++)
                                {
                                    string ss1 = null;
                                    string ss2 = null;
                                    if (!isFirst)
                                    {
                                        ss1 = null;
                                        if (true)
                                        {
                                            ss1 = ".Concat(null, ";
                                        }
                                        ss2 = ")";
                                    }
                                    s += ss1 + node[i][j].Code.ToString() + ss2;

                                    isFirst = false;
                                }
                            }
                        }

                        node.Code.A(s);


                    }
                    break;

                case "ASTS":
                    {
                        node.Code.A(AddOperator(Globals.operator_r, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTSN":  //rn
                    {
                        node.Code.A(AddOperator(Globals.operator_rn, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTSD":
                    {
                        node.Code.A(AddOperator(Globals.operator_rd, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTSDP":
                    {
                        node.Code.A(AddOperator(Globals.operator_rdp, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTSP":
                    {
                        node.Code.A(AddOperator(Globals.operator_rp, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;

                case "ASTCLEAR":
                    {
                        node.Code.A("O.Clear o" + Num(node) + " = new O.Clear();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;");
                        if (node[1].ChildrenCount() > 0)
                        {
                            node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        }
                        GetCodeFromAllChildren(node, node[0]);  //options
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTLOCK":
                    {
                        node.Code.A("O.Lock o" + Num(node) + " = new O.Lock();" + G.NL);
                        //node.Code.A("o" + Num(node) + ".p = p;");
                        node.Code.A("o" + Num(node) + ".bank = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTUNLOCK":
                    {
                        node.Code.A("O.Unlock o" + Num(node) + " = new O.Unlock();" + G.NL);
                        //node.Code.A("o" + Num(node) + ".p = p;");
                        node.Code.A("o" + Num(node) + ".bank = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTCLONE":
                    {
                        node.Code.A("O.Clone o" + Num(node) + " = new O.Clone();" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTCLEAR2":
                    {
                        node.Code.A("O.Clear2 o" + Num(node) + " = new O.Clear2();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;");
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTCLOSE":
                    {
                        node.Code.A("O.Close o" + Num(node) + " = new O.Close();" + G.NL);
                        node.Code.A("o" + Num(node) + ".listItems = " + node[0].Code + ";" + G.NL);
                        GetCodeFromAllChildren(node, node[1]);  //options                      
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTLIBRARYCLOSE":
                    {
                        node.Code.A("O.LibraryClose o" + Num(node) + " = new O.LibraryClose();" + G.NL);
                        node.Code.A("o" + Num(node) + ".listItems = " + node[0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTLIBRARYCLEAR":
                    {
                        node.Code.A("O.LibraryClear o" + Num(node) + " = new O.LibraryClear();" + G.NL);
                        node.Code.A("o" + Num(node) + ".listItems = " + node[0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTRESET":
                    {
                        node.Code.A("O.Reset o" + Num(node) + " = new O.Reset();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;");
                        node.Code.A("o" + Num(node) + ".Exe(" + Globals.smpl + ");" + G.NL);
                    }
                    break;
                case "ASTRESTART":
                    {
                        node.Code.A("O.Restart o" + Num(node) + " = new O.Restart();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;");
                        node.Code.A("o" + Num(node) + ".Exe(" + Globals.smpl + ");" + G.NL);
                    }
                    break;

                case "ASTCLOSESTAR":
                    {
                        node.Code.A("O.Close o" + Num(node) + " = new O.Close();" + G.NL);
                        node.Code.A("o" + Num(node) + ".name = `*`;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTCLS":
                    {
                        node.Code.A("O.Cls(``);"); //main window
                        node.Code.A("O.Cls(`output`);");  //output window
                    }
                    break;
                case "ASTCOLLAPSE":
                    {
                        node.Code.A("O.Collapse o" + Num(node) + " = new O.Collapse();" + G.NL);
                        node.Code.A("o" + Num(node) + ".lhs = " + node[0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".rhs = " + node[1].Code + ";" + G.NL);
                        string type = "null";
                        if (node[2].ChildrenCount() > 0) type = "O.ConvertToString(" + node[2][0].Code.ToString() + ")";
                        node.Code.A("o" + Num(node) + ".type = " + type + ";" + G.NL);
                        GetCodeFromAllChildren(node, node[3]);  //options
                        node.Code.A("o" + Num(node) + ".p = p;");
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTINTERPOLATE":
                    {
                        node.Code.A("O.Interpolate o" + Num(node) + " = new O.Interpolate();" + G.NL);
                        node.Code.A("o" + Num(node) + ".lhs = " + node[0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".rhs = " + node[1].Code + ";" + G.NL);
                        string type = "null";
                        if (node.ChildrenCount() >= 3) type = "O.ConvertToString(" + node[2].Code.ToString() + ")";
                        node.Code.A("o" + Num(node) + ".type = " + type + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;");
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTDOC":
                    {
                        node.Code.A("O.Doc o" + Num(node) + " = new O.Doc();" + G.NL);
                        if (node[0] != null) GetCodeFromAllChildren(node, node[0]);
                        if (node[1][0] != null) node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        if (node[2] != null) GetCodeFromAllChildren(node, node[2]);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        break;
                    }
                    break;
                case "ASTANALYZE":
                    {

                        node.Code.A("O.Analyze o" + Num(node) + " = new O.Analyze();" + G.NL);

                        GetCodeFromAllChildren(node, node[0]);

                        node.Code.A("o" + Num(node) + ".x = new List<IVariable>();" + G.NL);
                        for (int i = 1; i < node.ChildrenCount(); i++)
                        {
                            node.Code.A("o" + Num(node) + ".x.Add(" + node[i][0].Code + ");" + G.NL);
                        }

                        node.Code.A("o" + Num(node) + ".expressionsText = new List<string>();" + G.NL);
                        for (int i = 1; i < node.ChildrenCount(); i++)
                        {
                            node.Code.A("o" + Num(node) + ".expressionsText.Add(@`" + node[i].specialExpressionAndLabelInfo[1] + "`);" + G.NL);
                        }

                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTOLS":
                    {
                        node.Code.A("O.Ols o" + Num(node) + " = new O.Ols();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);
                        if (!node[1].Code.IsNull()) node.Code.A("o" + Num(node) + ".name = " + node[1].Code).End();
                        if (node[2].ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".impose = " + node[2][0].Code).End();

                        node.Code.A("o" + Num(node) + ".expressions = new List<IVariable>();" + G.NL);
                        for (int i = 3; i < node.ChildrenCount(); i++)
                        {
                            node.Code.A("o" + Num(node) + ".expressions.Add(" + node[i][0].Code + ");" + G.NL);
                        }

                        node.Code.A("o" + Num(node) + ".expressionsText = new List<string>();" + G.NL);
                        for (int i = 3; i < node.ChildrenCount(); i++)
                        {
                            node.Code.A("o" + Num(node) + ".expressionsText.Add(@`" + node[i].specialExpressionAndLabelInfo[1] + "`);" + G.NL);
                        }

                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTACCEPT":
                    {
                        node.Code.A("O.Accept o" + Num(node) + " = new O.Accept();" + G.NL);
                        node.Code.A("o" + Num(node) + ".type = @`" + node[0].Text + "`;" + G.NL);
                        node.Code.A("o" + Num(node) + ".name = " + node[1].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".message = " + node[2].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTCOPY":
                    {
                        node.Code.A("O.Copy o" + Num(node) + " = new O.Copy();" + G.NL);

                        if (node[0][0] != null) node.Code.A("o" + Num(node) + ".type = @`" + node[0][0].Text + "`;");

                        GetCodeFromAllChildren(node, node[1]);
                        node.Code.A("o" + Num(node) + ".names1 = " + node[2].Code + ";" + G.NL);
                        if (node[3] != null) node.Code.A("o" + Num(node) + ".names2 = " + node[3].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTDELETE":
                    {
                        node.Code.A("O.Delete o" + Num(node) + " = new O.Delete();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);  //options
                        if (node[1][0] != null) node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        break;
                    }
                    break;

                case "ASTCREATE":
                    {
                        node.Code.A("O.Create o" + Num(node) + " = new O.Create();" + G.NL);
                        node.Code.A("o" + Num(node) + ".names = " + node[0][0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        break;
                    }
                    break;
                case "ASTLOCAL":
                    {
                        node.Code.A("O.Local o" + Num(node) + " = new O.Local();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);  //options
                        if (node[1][0] != null) node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        break;
                    }
                    break;
                case "ASTGLOBAL":
                    {
                        node.Code.A("O.Global o" + Num(node) + " = new O.Global();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);  //options
                        if (node[1][0] != null) node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        break;
                    }
                    break;

                case "ASTENDO":
                case "ASTEXO":
                    {

                        string a = node[1][0].AlternativeCode.ToString();

                        string gt = "gt" + ++Globals.counter;
                        string la = "la" + ++Globals.counter;
                        string l = "l" + ++Globals.counter;
                        string helper = "helper" + ++Globals.counter;

                        node.Code.A("GekkoTimes " + gt + " = null; " + G.NL);

                        string s2 = null;
                        if (node[0][0] != null)
                        {
                            s2 = node[0][0].Code.ToString();
                        }
                        if (s2 != null)
                        {
                            node.Code.A(gt + " = " + s2 + "; " + G.NL);
                        }

                        node.Code.A("List<O.HandleEndoHelper> " + la + " = new List<O.HandleEndoHelper>()" + "; " + G.NL);
                        for (int ii = 0 + 1; ii < node.ChildrenCount(); ii++)
                        {

                            node.Code.A("O.HandleEndoHelper " + helper + "" + ii + " = new O.HandleEndoHelper();" + G.NL);
                            string s = null;
                            if (node[ii][1] != null)
                            {
                                s = node[ii][1].Code.ToString();
                            }

                            if (s != null)
                            {
                                node.Code.A(helper + ii + ".local = " + s + ";" + G.NL);
                            }

                            if (node[ii][0].Text == "ASTDOTORINDEXER")
                            {

                                if (!node[ii][0][0].Code.ToString().StartsWith("O.Lookup("))
                                {
                                    new Error("Internal error #09875209835");
                                    //throw new GekkoException();
                                }


                                node.Code.A("List<IVariable> " + l + ii + " = new List<IVariable>();" + G.NL);

                                for (int i = 0; i < node[ii][0][1].ChildrenCount(); i++)
                                {
                                    node.Code.A(l + ii + ".Add(" + node[ii][0][1][i].Code + ");" + G.NL);
                                }

                                node.Code.A(helper + ii + ".varname = " + "O.NameLookup(" + node[ii][0][0].Code.ToString().Substring("O.Lookup(".Length) + ";" + G.NL);
                                node.Code.A(helper + ii + ".indices = " + l + ii + ";" + G.NL);

                            }
                            else if (node[ii][0].Text == "ASTBANKVARNAME")
                            {
                                node.Code.A(helper + ii + ".varname = " + "O.NameLookup(" + node[ii][0].Code.ToString().Substring("O.Lookup(".Length) + ";" + G.NL);
                            }
                            else
                            {
                                new Error("Expected a variable with or without []-indexes");
                                //throw new GekkoException();
                            }
                            node.Code.A(la + ".Add(" + helper + "" + ii + ");" + G.NL);

                        }
                        node.Code.A("O.HandleEndoExo(" + gt + ", " + la + ", " + (node.Text == "ASTENDO").ToString().ToLower() + ");" + G.NL);

                    }
                    break;
                case "ASTENDOQUESTION":
                    {
                        node.Code.A("O.Endo o" + Num(node) + " = new O.Endo();" + G.NL);
                        node.Code.A("o" + Num(node) + ".question = true;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;

                case "ASTEXOQUESTION":
                    {
                        node.Code.A("O.Exo o" + Num(node) + " = new O.Exo();" + G.NL);
                        node.Code.A("o" + Num(node) + ".question = true;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTCUT":
                    {
                        node.Code.A("O.Cut();" + G.NL);
                    }
                    break;
                case "ASTEXIT":  // <command>
                    {
                        node.Code.A("O.Exit();" + G.NL);
                        //#9807235423 return problem                            
                        node.Code.A("return;" + G.NL);  //probably superfluous
                    }
                    break;

                case "ASTSTOP":
                    {
                        node.Code.A("O.StopHelper(smpl, p);" + G.NL);
                    }
                    break;

                case "ASTREPLACE":
                    {

                    }
                    break;
                case "ASTFINDMISSINGDATA": // <command> <period>
                    {
                        node.Code.A("O.Findmissingdata o" + Num(node) + " = new O.Findmissingdata();" + G.NL);
                        if (node[1][0] != null) node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTHDG":
                    node.Code.A("O.Hdg(O.ConvertToString(" + node[0].Code + "));");
                    break;
                case "ASTTELL":
                    {
                        string s = "false";
                        string ss = "new ScalarString(``)";
                        if (node.ChildrenCount() > 1)
                        {
                            s = "true";
                        }
                        if (node[0].ChildrenCount() == 1)
                        {
                            ss = node[0][0].Code.ToString();
                        }
                        node.Code.A("O.Tell(O.ConvertToString(" + ss + "), " + s + ");");
                    }
                    break;
                case "ASTSYS":
                    {
                        node.Code.A("O.Sys o" + Num(node) + " = new O.Sys();" + G.NL);
                        if (node[0] != null) node.Code.A("o" + Num(node) + ".s = " + node[0].Code + ";" + G.NL);
                        if (node[1] != null) node.Code.A(node[1].Code);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTHELP":
                    {
                        string code = "null";
                        if (node.ChildrenCount() > 0) code = "O.ConvertToString(" + node[0].Code + ")";
                        node.Code.A("O.Help(" + code + ");" + G.NL);
                    }
                    break;
                case "ASTCREATEQUESTION":
                    {
                        node.Code.A("O.Create o" + Num(node) + " = new O.Create();" + G.NL);
                        node.Code.A("o" + Num(node) + ".question = true;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        break;
                    }
                case "ASTCHECKOFF":
                    {
                        node.Code.A("O.Checkoff o" + Num(node) + " = new O.Checkoff();" + G.NL);
                        if (node.ChildrenCount() == 0)
                        {
                            node.Code.A("o" + Num(node) + ".type = `clear`;");
                        }
                        else if (node.ChildrenCount() == 1 && node[0].Text == "?")
                        {
                            node.Code.A("o" + Num(node) + ".type = `?`;");
                        }
                        else
                        {
                            //node.Code.A(node[0].Code);
                            node.Code.A("o" + Num(node) + ".names = " + node[0].Code + ";" + G.NL);
                        }
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        break;
                    }
                case "ASTCURLY":
                    {
                        string s = node[0].Code.ToString();
                        ASTNode child = node;

                        string internalName = null;
                        string internalFunction = null;


                        if (child[0].Text == "ASTPLUS" || child[0].Text == "ASTMINUS")
                        {
                            //See also #980752345
                            string listName = GetSimpleHashName(child[0][0]);
                            if (listName != null)
                            {
                                TwoStrings two = SearchUpwardsInTree2(node, listName);
                                if (two != null)
                                {
                                    internalName = two.s1;
                                    internalFunction = two.s2;
                                }
                            }

                            if (internalName != null)
                            {
                                if (child[0].Text == "ASTPLUS")
                                {
                                    s = "O.AddSpecial(" + Globals.smpl + ", " + internalName + ", " + child[0][1].Code + ", false)";
                                }
                                else
                                {
                                    s = "O.AddSpecial(" + Globals.smpl + ", " + internalName + ", " + child[0][1].Code + ", true)";
                                }
                            }
                            else
                            {
                                //keep the s from above
                            }

                        }
                        else
                        {
                            string listName = GetSimpleHashName(node[0]);

                            if (listName != null)
                            {
                                TwoStrings two = SearchUpwardsInTree2(node, listName);
                                if (two != null)
                                {
                                    internalName = two.s1;
                                    internalFunction = two.s2;
                                }
                            }
                            if (internalName != null) s = internalName;
                        }

                        if ((w.wh.currentCommand == "ASTPRT" || w.wh.currentCommand == "ASTDISP") && !SearchUpwardsInTree5(node.Parent))
                        {
                            bool xxx = ReportHelperIsSum(internalName, internalFunction); //a controlled #x, bounded by sum

                            //only for PRT-type or DISP, and only if the {} is not inside [] or {}.
                            if (node.specialExpressionAndLabelInfo != null && !xxx)
                            {
                                node.Code.CA(Globals.reportLabel1 + "" + s + ", `" + ReportLabelHelper(node) + "`" + Globals.reportLabel2);
                            }
                            else
                            {
                                node.Code.CA(s);
                            }
                        }
                        else
                        {
                            node.Code.CA(s);
                        }

                    }
                    break;

                case "ASTEXPRESSION":
                    {
                        node.Code.CA(node[0].Code);
                    }
                    break;
                case "ASTD":
                    {
                        node.Code.A(AddOperator("d", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTDP":
                    {
                        node.Code.A(AddOperator("dp", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;

                case "ASTDATE2":
                    {
                        //TODO:
                        //TODO:
                        //TODO: FromStringToDate() depends upon current freq -- fix that!
                        //TODO:
                        //TODO:
                        node.Code.A("new ScalarDate(GekkoTime.FromStringToGekkoTime(`" + node[0].Text + "`))");
                    }
                    break;
                case "ASTDATES":
                case "ASTDATES_BLOCK":
                case "ASTDATES_TYPE2":
                    {
                        //what about G.GetStart/EndDate()??
                        //We use  O.GetDateChoices.FlexibleStart and  O.GetDateChoices.FlexibleEnd
                        //These can transform an integer into a quarterly or monthly frequency.
                        //This is only legal/possible for two such dates (from/to).

                        string ss1 = node.GetChildCode(0).ToString();
                        string s1 = null;
                        if (ss1 == null)
                        {
                            if (node.Text == "ASTDATES_TYPE2") s1 = "GekkoTime.tNull";
                            else if (node.Text == "ASTDATES_BLOCK") s1 = "";
                            else s1 = "Globals.globalPeriodStart";
                        }
                        else s1 = "O.ConvertToDate(" + ss1 + ", O.GetDateChoices.FlexibleStart);" + G.NL;

                        string ss2 = node.GetChildCode(1).ToString();
                        string s2 = null;
                        if (ss2 == null)
                        {
                            if (node.Text == "ASTDATES_TYPE2") s2 = "GekkoTime.tNull";
                            else if (node.Text == "ASTDATES_BLOCK") s2 = "";
                            else s2 = "Globals.globalPeriodEnd";
                        }
                        else s2 = "O.ConvertToDate(" + ss2 + ", O.GetDateChoices.FlexibleEnd);" + G.NL;

                        if (node.Text == "ASTDATES_BLOCK")
                        {
                            node.Code.A(s1).A(Globals.blockHelper).A(s2); //will be splitted up later on
                        }
                        else
                        {
                            if (node?.Parent?.Parent?.Parent?.Text == "ASTASSIGNMENT")
                            {
                                //assignment is a bit special
                                node.Code.A("" + Globals.smpl + ".t0 = ").A(s1).A(";").A(G.NL);
                                node.Code.A("" + Globals.smpl + ".t1 = ").A(s1).A(";").A(G.NL);
                                node.Code.A("" + Globals.smpl + ".t2 = ").A(s2).A(";").A(G.NL);
                                node.Code.A("" + Globals.smpl + ".t3 = ").A(s2).A(";").A(G.NL);
                            }
                            else
                            {
                                if (node?.Parent?.Parent?.Text == "ASTOLS")  //so that the expressions get the right " + Globals.smpl + " period
                                {
                                    node.Code.A("" + Globals.smpl + ".t0 = ").A(s1).A(";").A(G.NL);
                                    node.Code.A("" + Globals.smpl + ".t1 = ").A(s1).A(";").A(G.NL);
                                    node.Code.A("" + Globals.smpl + ".t2 = ").A(s2).A(";").A(G.NL);
                                    node.Code.A("" + Globals.smpl + ".t3 = ").A(s2).A(";").A(G.NL);
                                }

                                node.Code.A("o").A(Num(node)).A(".t1 = ").A(s1).A(";").A(G.NL);
                                node.Code.A("o").A(Num(node)).A(".t2 = ").A(s2).A(";").A(G.NL);
                            }
                        }
                    }
                    break;
                case "ASTDATES2":
                    {
                        string ss1 = node.GetChildCode(0).ToString();
                        string s1 = "O.ConvertToDate(" + ss1 + ", O.GetDateChoices.FlexibleStart)" + G.NL;
                        string ss2 = node.GetChildCode(1).ToString();
                        string s2 = "O.ConvertToDate(" + ss2 + ", O.GetDateChoices.FlexibleEnd)" + G.NL;
                        string s = "O.HandleDates(" + s1 + "," + s2 + " );";
                        node.Code.A(s);
                    }
                    break;


                case "ASTNUMBER":
                    {
                        if (node.ChildrenCount() > 1 && node[1].Text == "ASTNUMBERMINUS")
                        {
                            node.Code.A("O.Minus(" + node[0].Code + ")");
                        }
                        else
                        {
                            node.Code.A(node[0].Code);
                        }
                    }
                    break;
                case "ASTDOUBLE":
                    {
                        //TODO TODO TODO
                        //TODO TODO TODO
                        //TODO TODO TODO find out about GLUEDOT etc. --> how to replace it.
                        //TODO TODO TODO
                        //TODO TODO TODO

                        //TODO TODO TODO
                        //TODO TODO TODO
                        //TODO TODO TODO use the cache to avoid repetitions of numbers
                        //TODO TODO TODO Maybe not: this simplifies user defined functions
                        //TODO TODO TODO

                        string minus = HandleNegate(node);
                        string doubleWithNumber = "d" + ++Globals.counter;
                        string temp = node[0].Text.Replace(Globals.symbolGlueChar3, "") + "d";
                        if (temp.EndsWith(".d")) temp = temp.Substring(0, temp.Length - 2) + ".0d";
                        string s = "new ScalarVal(" + minus + temp + ")";
                        GetHeaderCs(w).AppendLine("public static readonly ScalarVal " + doubleWithNumber + " = " + s + ";");
                        node.Code.CA(doubleWithNumber); //no need for checking if it exists                      
                    }
                    break;
                case "ASTOR":
                    {

                        node.Code.A("O.LogicalOr(" + Globals.smpl + ", ");
                        node.Code.A(node[0].Code);
                        node.Code.A(", ");
                        node.Code.A(node[1].Code);
                        node.Code.A(")");
                    }
                    break;
                case "ASTAND":
                    {

                        node.Code.A("O.LogicalAnd(" + Globals.smpl + ", ");
                        node.Code.A(node[0].Code);
                        node.Code.A(", ");
                        node.Code.A(node[1].Code);
                        node.Code.A(")");

                    }
                    break;
                case "ASTNOT":
                    {

                        node.Code.A("O.LogicalNot(" + Globals.smpl + ", ");
                        node.Code.A(node[0].Code);
                        node.Code.A(")");
                    }
                    break;
                case "ASTCOMPARE2":
                    {

                        string indexes = null;

                        ASTNode child = node[1];

                        string listName = GetSimpleHashName(child);
                        string internalName = null;
                        string internalFunction = null;
                        if (listName != null)
                        {
                            TwoStrings two = SearchUpwardsInTree2(node, listName);
                            if (two != null)
                            {
                                internalName = two.s1;
                                internalFunction = two.s2;
                            }
                        }

                        if (internalName != null)
                        {
                            indexes += internalName;
                        }
                        else
                        {
                            indexes += node[1].Code.ToString();
                        }

                        bool xxx = ReportHelperIsSum(internalName, internalFunction); //a controlled #x, bounded by sum

                        if (((w.wh.currentCommand == "ASTPRT" || w.wh.currentCommand == "ASTDISP") && !SearchUpwardsInTree5(node.Parent)) && (!xxx))
                        {
                            //only for PRT-type or DISP, and only if the [] is not inside [] or {}.
                            node.Code.A("O.ListContains(" + node[0].Code + "," + Globals.reportLabel1 + indexes + ", `" + ReportLabelHelper(node) + "`" + Globals.reportLabel2 + ")");
                        }
                        else
                        {
                            node.Code.A("O.ListContains(" + node[0].Code + "," + indexes + ")");
                        }
                    }
                    break;
                case "ASTCOMPARE":
                    {
                        string op = node[0][0].Text;
                        string code1 = node[1].Code.ToString();
                        string code2 = node[2].Code.ToString();

                        //all comparisons, including "... in ..." can use controlled set
                        if (false)
                        {
                            code1 = MaybeControlledSet777(node[1], code1);
                            code2 = MaybeControlledSet777(node[2], code2);
                        }

                        if (op == "ASTIFOPERATOR4")  //"<"
                        {
                            node.Code.A("O.StrictlySmallerThan(" + Globals.smpl + ", " + code1 + "," + code2 + ")");
                        }
                        else if (op == "ASTIFOPERATOR6")  //"<="
                        {
                            node.Code.A("O.SmallerThanOrEqual(" + Globals.smpl + ", " + code1 + "," + code2 + ")");
                        }
                        else if (op == "ASTIFOPERATOR1") //"=="
                        {
                            node.Code.A("O.Equals(" + Globals.smpl + ", " + code1 + "," + code2 + ")");
                        }
                        else if (op == "ASTIFOPERATOR5")  //">="
                        {
                            node.Code.A("O.LargerThanOrEqual(" + Globals.smpl + ", " + code1 + "," + code2 + ")");
                        }
                        else if (op == "ASTIFOPERATOR3") //">"
                        {
                            node.Code.A("O.StrictlyLargerThan(" + Globals.smpl + ", " + code1 + "," + code2 + ")");
                        }
                        else if (op == "ASTIFOPERATOR2") //"<>"
                        {
                            node.Code.A("O.NonEquals(" + Globals.smpl + ", " + code1 + "," + code2 + ")");
                        }
                        else if (op == "ASTIFOPERATOR7") //"in"
                        {
                            code1 = MaybeControlledSet777(node[1], code1);
                            code2 = MaybeControlledSet777(node[2], code2);
                            node.Code.A("O.In(" + Globals.smpl + ", " + code1 + "," + code2 + ")");
                        }
                    }
                    break;

                case "ASTDOLLARCONDITIONAL":
                case "ASTDOLLARCONDITIONALVARIABLE":
                    {
                        GetCodeFromAllChildren(node);
                        break;
                    }
                case "ASTDOLLAR":
                    {

                        string s = node[0].Code.ToString();
                        if (node.Parent.Text == "ASTLEFTSIDE")
                        {
                            //This is a hack, but quite innocent.
                            //Problem is, we have this ASTDOLLAR, with ASTBANKVARNAME and ASTDOLLARCONDITIONAL as children. 
                            //Both of these produce an IVARIABLE, but ASTDOLLARCONDITIONAL is still empty when ASTBANKVARNAME is encountered. 
                            //A solution could be to walk ASTDOLLARCONDITIONAL manually, and do the DollarLookup() from ASTBANKVARNAME. 
                            //But the hack is not that bad.

                            if (node.Parent.Parent.Text == "ASTASSIGNMENT")
                            {
                                //must probably always be so
                                ASTNode gparent = node.Parent.Parent;
                                if (G.Equal(gparent[3].Text, "VAR_KDUSJFLQO2"))
                                {
                                    gparent.loopCodeCs = node[1].Code.ToString();
                                }
                            }

                            if (s.StartsWith("O.Lookup("))
                            {
                                node.Code.A("O.DollarLookup(" + node[1].Code.ToString() + ", " + s.Substring("O.Lookup(".Length));
                            }
                            else if (s.StartsWith("O.IndexerSetData("))
                            {
                                node.Code.A("O.DollarIndexerSetData(" + node[1].Code.ToString() + ", " + s.Substring("O.IndexerSetData(".Length));
                            }
                            else
                            {
                                new Error("Internal error related to $ on left-hand side");
                            }
                        }
                        else
                        {
                            //right-hand side, much easier
                            node.Code.A("O.Conditional1Of3(" + Globals.smpl + ", " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    }

                case Globals.symbolDollar:
                    {
                        //Codesplitting would interfere really bad here, so not allowed
                        //Conditional, for instance SERIES y = x $ (%d == 1);
                        //node.Code.A(Globals.splitSTOP);
                        node.Code.A("(" + node[1].Code + ")");
                        node.Code.A(" ? ");
                        node.Code.A("(");
                        node.Code.A(node[0].Code);
                        node.Code.A(")");
                        node.Code.A(" : ");
                        node.Code.A("(");
                        node.Code.A("new ScalarVal(0d)");
                        node.Code.A(")");
                        //node.Code.A(Globals.splitSTART);
                        break;
                    }

                case "ASTRETURN":
                    {
                        LinesForSpecialCommands(node);

                        string type = SearchUpwardsInTree8(node);

                        if (type == null)
                        {
                            //normal return from a command file (not inside function)

                            if (node.ChildrenCount() > 0)
                            {
                                new Error("Return of variable, but not inside function definition");
                            }
                            node.Code.A("return;" + G.NL);
                        }
                        else
                        {
                            //return from inside function)

                            if (G.Equal(type, "void"))
                            {
                                if (node.ChildrenCount() > 0)
                                {
                                    new Error("RETURN <variable> used, should be just RETURN with no variable");
                                }
                                node.Code.A("return null;" + G.NL);
                            }
                            else if (node.ChildrenCount() == 0)
                            {
                                //#9807235423 return problem, should it be return true?? C1(), C2(), ...
                                if (!G.Equal(type, "void"))
                                {
                                    new Error("RETURN with no variable used, should be RETURN <variable>");
                                }
                                node.Code.A("return;" + G.NL);  //probably the node[0].Code is always empty here (should be)
                            }
                            else
                            {
                                node.Code.A("return O.TypeCheck_" + type + "(" + node[0].Code + ", 0);" + G.NL);
                            }
                        }

                    }
                    break;
                case "ASTGOTO":
                    {
                        LinesForSpecialCommands(node);

                        node.Code.A("goto " + GetStringFromIdent(node[0]).ToLower().Trim() + ";" + G.NL);  //calls a C# label
                        w.wh.isGotoOrTarget = true;
                    }
                    break;
                case "ASTTARGET":  //AREMOS: target
                    {
                        LinesForSpecialCommands(node);
                        node.Code.A(GetStringFromIdent(node[0]).ToLower().Trim() + ":;" + G.NL);  //a C# label
                        w.wh.isGotoOrTarget = true;
                    }
                    break;

                case "ASTFOR":
                    {
                        LinesForSpecialCommands(node);

                        GetCodeFromAllChildren(node[1]);

                        List<string> varnames = GetForLoopVariables(node);

                        if (node[0].ChildrenCount() == 1)
                        {
                            O.ELoopType loopType = LoopType(node, 0);

                            string type = node[0][0][0][0].Text;

                            CheckTypeInFunctionDefProcedureDefForDef("for-loop", type, varnames[0]);

                            int i = 0;
                            string codeStart, codeEnd2, codeStep;
                            GetCodes(node, i, out codeStart, out codeEnd2, out codeStep);
                            string temp = "counter" + ++Globals.counter;
                            node.Code.A(node.forLoop[i].Item1 + " " + node.forLoop[i].Item2 + " = null").End();
                            node.Code.A("int " + temp + " = 0").End();
                            string loopType2 = "O.ELoopType." + loopType.ToString();
                            string y = "years" + ++Globals.counter;
                            node.Code.A("bool " + y + " = O.LoopYears(`" + type + "`, " + loopType2 + ", " + codeStart + ", " + codeEnd2 + "); " + "for (O.IterateStart(" + y + ", " + loopType2 + ", ref " + node.forLoop[i].Item2 + ", " + codeStart + "); O.IterateContinue(" + y + ", " + loopType2 + ", " + node.forLoop[i].Item2 + ", " + codeStart + ", " + codeEnd2 + ", " + codeStep + ", ref " + temp + "); O.IterateStep(" + y + ", " + loopType2 + ", ref " + node.forLoop[i].Item2 + ", " + codeStart + ", " + codeStep + ", " + temp + "))" + G.NL);
                            node.Code.A("{").End();
                            node.Code.A("O.TypeCheck_" + type.ToLower() + "(" + node.forLoop[i].Item2 + ", 0);" + G.NL);
                            node.Code.A(node[1].Code);
                            node.Code.A("}").End();
                        }
                        else
                        {
                            string listsname = "lists" + ++Globals.counter;
                            node.Code.A("List<List<IVariable>> " + listsname + " = new List<List<IVariable>>()").End();
                            for (int i = 0; i < node[0].ChildrenCount(); i++)
                            {
                                O.ELoopType loopType = LoopType(node, 0);
                                string codeStart, codeEnd2, codeStep;
                                GetCodes(node, i, out codeStart, out codeEnd2, out codeStep);
                                if (loopType == O.ELoopType.ForTo)
                                {
                                    new Error("You cannot use TO or STEP/BY in a parallel loop");
                                    //throw new GekkoException();
                                }
                                node.Code.A(listsname + ".Add(O.ConvertToList(" + codeStart + "))").End();
                            }
                            string maxname = "max" + ++Globals.counter;
                            node.Code.A("int " + maxname + " = O.ForListMax(" + listsname + ")").End();

                            string iname = "i" + ++Globals.counter;

                            node.Code.A("for (int " + iname + " = 0; " + iname + " < " + maxname + "; " + iname + " ++) {").End();

                            for (int i = 0; i < node[0].ChildrenCount(); i++)
                            {
                                node.Code.A("IVariable ").A(node.forLoop[i].Item2).A(" = ").A(listsname).A("[" + i + "]").A("[" + iname + "]").End();
                            }
                            node.Code.A(node[1].Code);

                            node.Code.A("}").End();

                            //parallel loop

                        }


                    }
                    break;

                case "ASTBLOCK":
                    {

                        string record = null;
                        string alter = null;
                        string play = null;

                        foreach (ASTNode child in node[0].ChildrenIterator())
                        {
                            if (child.Text == "ASTDATES_BLOCK")
                            {
                                //handle BLOCK time ...

                                string ss = child.Code.ToString();
                                if (ss != null)
                                {
                                    string[] sss = ss.Split(new string[] { Globals.blockHelper }, StringSplitOptions.None);
                                    int n = ++Globals.counter;
                                    record += "var record" + n + " = Globals.globalPeriodStart;" + G.NL;  //var record117 = Globals.globalPeriodStart
                                    alter += "Globals.globalPeriodStart = " + sss[0] + G.NL;               //Globals.globalPeriodStart = ...;
                                    play += "Globals.globalPeriodStart = record" + n + ";" + G.NL;        //Globals.globalPeriodStart = record117
                                    n = ++Globals.counter;
                                    record += "var record" + n + " = Globals.globalPeriodEnd;" + G.NL;
                                    alter += "Globals.globalPeriodEnd = " + sss[1] + G.NL;
                                    play += "Globals.globalPeriodEnd = record" + n + ";" + G.NL;
                                }
                                else
                                {
                                    new Error("Internal error related to BLOCK");
                                    //throw new GekkoException();
                                }
                            }
                            else if (child.Text == "ASTBLOCKOPTION")
                            {

                                Tuple<string, string> tup = HandleOptionAndBlock(child, true);

                                if (tup.Item1 == "Program.options.freq")
                                {
                                    //see also #89073589324, must also record global time settings, since these are implicitly altered when changing frequency
                                    int n = ++Globals.counter;
                                    record += "var record" + n + " = " + tup.Item1 + ";" + G.NL;  //var record117 = Program.options.freq;
                                    alter += tup.Item1 + " = " + tup.Item2 + ";" + G.NL;          //Program.options.freq = EFreq.Q;
                                    alter += "O.AdjustFreq();" + G.NL;                            //O.AdjustFreq();
                                    play += tup.Item1 + " = record" + n + ";" + G.NL;             //Program.options.freq = record117
                                                                                                  // global perStart
                                    n = ++Globals.counter;
                                    record += "var record" + n + " = Globals.globalPeriodStart;" + G.NL;
                                    play += "Globals.globalPeriodStart = record" + n + ";" + G.NL;
                                    // global perEnd
                                    n = ++Globals.counter;
                                    record += "var record" + n + " = Globals.globalPeriodEnd;" + G.NL;
                                    play += "Globals.globalPeriodEnd = record" + n + ";" + G.NL;
                                }
                                else
                                {
                                    int n = ++Globals.counter;
                                    record += "var record" + n + " = " + tup.Item1 + ";" + G.NL;  //var record117 = Program.options....;
                                    alter += tup.Item1 + " = " + tup.Item2 + ";" + G.NL;          //Program.options.... = ...;
                                    alter += "O.PrintOptions(`" + tup.Item1 + "`);" + G.NL;       //O.PrintOptions(...)
                                    alter += "O.HandleOptions(`" + tup.Item1 + "`, 1, p);" + G.NL;   //O.HandleOptions(...)
                                    play += tup.Item1 + " = record" + n + ";" + G.NL;             //Program.options.... = record117
                                    play += "O.HandleOptions(`" + tup.Item1 + "`, 2, p);" + G.NL;    //O.HandleOptions(...)
                                }
                            }
                            else
                            {
                                new Error("Internal error related to BLOCK");
                                //throw new GekkoException();
                            }
                        }
                        node.Code.A(record);
                        node.Code.A(alter);
                        GetCodeFromAllChildren(node, node[1][0]);  //code inside the block
                        node.Code.A(play);

                    }
                    break;

                case "ASTIF":
                case "ASTIFOLD":
                    {
                        LinesForSpecialCommands(node);
                        if (node.Text == "ASTIFOLD") node.Code.A("O.UseOldIf(true);" + G.NL);
                        node.Code.A("if(O.IsTrue(" + Globals.smpl + ", " + node[0].Code + ")) {");
                        GetCodeFromAllChildren(node, node[1][0]);
                        node.Code.A("}");
                        node.Code.A("else {");
                        GetCodeFromAllChildren(node, node[2][0]);
                        node.Code.A("}");
                        if (node.Text == "ASTIFOLD") node.Code.A("O.UseOldIf(false);" + G.NL);

                    }
                    break;
                case "ASTFUNCTIONDEF2":
                case "ASTPROCEDUREDEF":
                    {
                        StringBuilder sb = new StringBuilder();

                        int numberOfDates_constant = 2;

                        string returnTypeLower = node[0].Text.ToLower();
                        string functionNameLower = node[1][0].Text.ToLower();

                        string libraryName = "null";
                        if (w.libraryName != null) libraryName = "`" + w.libraryName + "`";

                        if (node.Text == "ASTPROCEDUREDEF")
                        {
                            functionNameLower = Globals.procedure + functionNameLower;
                        }

                        int numberOfParameters = node.functionDef.Count;

                        string internalName = "FunctionDef" + ++Globals.counter;

                        GetCodeFromAllChildren(node[3]);  //it is a placeholder node that does not get code

                        sb.AppendLine(internalName + "();" + G.NL);

                        string typeChecks = null;

                        int numberOfOptionalParameters = 0;
                        if (node.functionDef == null)
                        {
                            //do nothing
                        }
                        else
                        {
                            for (int i = 2; i < numberOfParameters; i++)
                            {
                                if (node[2]?[i - numberOfDates_constant]?[2]?[0] != null) node.functionDef[i].labelCode = node[2][i - numberOfDates_constant][2][0].Code.ToString();
                                if (node[2]?[i - numberOfDates_constant]?[3]?[0] != null) node.functionDef[i].defaultValueCode = node[2][i - numberOfDates_constant][3][0].Code.ToString();
                            }

                            for (int i = numberOfDates_constant; i < numberOfParameters; i++)
                            {
                                if (node.functionDef[i].defaultValueCode == null)
                                {
                                    if (numberOfOptionalParameters > 0)
                                    {
                                        new Error("Required parameters cannot be stated after optional parameters");
                                    }
                                }
                                else
                                {
                                    numberOfOptionalParameters++;
                                }
                            }

                            for (int i = 0; i < numberOfParameters; i++)
                            {

                                string f = "f1";
                                if (node.functionDef[i].type.ToLower() == "name") f = "f2";

                                if (i < 2)
                                {
                                    //special <%t1 %t2> arguments
                                    typeChecks += "IVariable " + node.functionDef[i].internalName + " = " + "O.TypeCheck_" + node.functionDef[i].type.ToLower() + "(" + node.functionDef[i].internalName + "_func, " + Globals.smpl + ", " + (i + 1) + ");" + G.NL;
                                }
                                else
                                {
                                    typeChecks += "IVariable " + node.functionDef[i].internalName + " = " + "O.TypeCheck_" + node.functionDef[i].type.ToLower() + "(" + node.functionDef[i].internalName + "_func." + f + "(" + Globals.smpl + ")" + ", " + (i + 1) + ");" + G.NL;
                                }

                            }
                        }

                        string qName = "q" + ++Globals.counter;

                        w.headerCs.AppendLine("public static void " + internalName + "() {" + G.NL);

                        //Version with all parameters, also optional parameters
                        w.headerCs.AppendLine("O.PrepareUfunction(" + numberOfParameters + ", `" + functionNameLower + "`);" + G.NL);
                        w.headerCs.AppendLine("O.Add" + numberOfParameters + Globals.functionSpecialName1 + "(" + libraryName + ", `" + functionNameLower + "`, (GekkoSmpl " + Globals.smpl + ", P p, bool " + qName + "" + GetParametersInAList(node, numberOfParameters, 0) + ") => " + G.NL);
                        w.headerCs.AppendLine(G.NL + "{ " + typeChecks + G.NL + LocalCode1(Num(node), functionNameLower, w.fileNameContainingParsedCode) + G.NL + node[3].Code.ToString() + G.NL + "return null; " + G.NL + LocalCode2(Num(node), functionNameLower) + "});" + G.NL);

                        //for instance, f(x1, x2, x3, x4=..., x5=...)
                        //here we have 5 parameters, of which 2 are optional
                        //Then we need to create all in all 2+1 functions:
                        //f(x1, x2, x3, x4, x5)
                        //f(x1, x2, x3, x4) -> with default for x5
                        //f(x1, x2, x3)     -> with default for x4 and x5
                        //The last two call the first one. If for instance f(..., ..., ...) exists already, it will
                        //be overwritten.

                        for (int i = 0; i < numberOfOptionalParameters; i++)  //i = 0, 1 in example
                        {
                            // numberOfParametersOverload = 4, 3 in the example
                            int numberOfParametersOverloadedVersion = numberOfParameters - i - 1;

                            // so first time, we cut off 1 optional parameter and use default for x5
                            // second time, we cut off 2 optional parameter2 and use default for x4 and x5

                            int numberOfParametersCutOff = numberOfParameters - numberOfParametersOverloadedVersion;

                            string questionsName = "questions" + ++Globals.counter;
                            string defaultValueCodesName = "defaultValueCodes" + ++Globals.counter;
                            string typesName = "types" + ++Globals.counter;
                            string labelCodesName = "labelCodes" + ++Globals.counter;
                            string promptResultsName = "promptResults" + ++Globals.counter;

                            string defaultValueCodes = null;
                            string labelCodes = null;
                            string types = null;
                            string questions = null;
                            string prompts = null;
                            string prompts2 = null;
                            for (int j = 0; j < numberOfParametersCutOff; j++)
                            {
                                //j will run 0 first time, and then 0, 1. 

                                //j=0 -->add 1
                                //j=1 -->add 0

                                int xxx = numberOfParameters - (numberOfParametersCutOff - j - 1) - 1;
                                string defaultValueCode = node.functionDef[xxx].defaultValueCode;
                                int jjj = numberOfParameters - i - 1 + j;

                                string defaultValueCode2 = node.functionDef[jjj].defaultValueCode;
                                string labelCode = node.functionDef[xxx].labelCode;
                                string type = node.functionDef[xxx].type;
                                string question = "false";
                                if (j > 0)
                                {
                                    defaultValueCodes += ", ";
                                    labelCodes += ", ";
                                    types += ", ";
                                    questions += ", ";
                                }

                                defaultValueCodes += defaultValueCode;
                                labelCodes += labelCode;
                                types += "`" + type.Replace("`", "\\`") + "`";
                                questions += qName;  //the q parameter tells if the is a question sign on the function

                                int n = ++Globals.counter;

                                if (G.Equal(type, "name"))
                                {
                                    prompts += ", new GekkoArg((spml" + n + ") => null, (spml" + n + ") => " + promptResultsName + "[" + j + "])";
                                    prompts2 += ", new GekkoArg((spml" + n + ") => null, (spml" + n + ") => " + defaultValueCode2 + ")";  //simple version, just normal overload
                                }
                                else
                                {
                                    prompts += ", new GekkoArg((spml" + n + ") => " + promptResultsName + "[" + j + "], (spml" + n + ") => null)";
                                    prompts2 += ", new GekkoArg((spml" + n + ") => " + defaultValueCode2 + ", (spml" + n + ") => null)";  //simple version, just normal overload
                                }
                            }

                            string defaultValues = null;

                            string libraryNameWhereTheFunctionIsCallingFrom = CallingLibraryHelper(w);

                            w.headerCs.AppendLine("O.PrepareUfunction(" + numberOfParametersOverloadedVersion + ", `" + functionNameLower + "`);" + G.NL);
                            w.headerCs.AppendLine("O.Add" + numberOfParametersOverloadedVersion + Globals.functionSpecialName1 + "(" + libraryName + ", `" + functionNameLower + "`, (GekkoSmpl " + Globals.smpl + ", P p, bool " + qName + "" + GetParametersInAList(node, numberOfParametersOverloadedVersion, 0) + ") => " + G.NL);
                            w.headerCs.AppendLine(G.NL + "{ " + G.NL);

                            w.headerCs.AppendLine("if(" + qName + ") {" + G.NL);
                            w.headerCs.AppendLine("List<bool> " + questionsName + " = new List<bool> { " + questions + " };");
                            w.headerCs.AppendLine("List<IVariable> " + defaultValueCodesName + " = new List<IVariable> { " + defaultValueCodes + " };");
                            w.headerCs.AppendLine("List<string> " + typesName + " = new List<string> { " + types + " };");
                            w.headerCs.AppendLine("List<IVariable> " + labelCodesName + " = new List<IVariable> { " + labelCodes + " };");
                            w.headerCs.AppendLine("List<IVariable> " + promptResultsName + " = O.Prompt(" + questionsName + ", " + defaultValueCodesName + ", " + typesName + ", " + labelCodesName + ");");
                            w.headerCs.AppendLine("return " + Globals.functionSpecialName2 + numberOfParameters + "(" + libraryNameWhereTheFunctionIsCallingFrom + ", " + libraryName + ", `" + functionNameLower + "`)(smpl, p, false " + GetParametersInAList(node, numberOfParametersOverloadedVersion, 1) + " " + prompts + ");");
                            w.headerCs.AppendLine("}" + G.NL);
                            w.headerCs.AppendLine("else" + G.NL);
                            w.headerCs.AppendLine("{" + G.NL);
                            w.headerCs.AppendLine("return " + Globals.functionSpecialName2 + numberOfParameters + "(" + libraryNameWhereTheFunctionIsCallingFrom + ", " + libraryName + ", `" + functionNameLower + "`)(smpl, p, false " + GetParametersInAList(node, numberOfParametersOverloadedVersion, 1) + " " + prompts2 + ");");
                            w.headerCs.AppendLine("}" + G.NL);

                            w.headerCs.AppendLine(G.NL + " return null; });" + G.NL);
                        }
                        w.headerCs.AppendLine("}" + G.NL);

                        node.Code.A(sb.ToString());

                    }
                    break;

                case "ASTLIBRARYNAME":
                    {
                        node.Code.A("O.ReplaceSlash(").A(node[0].Code).A(".Concat(" + Globals.smpl + ", O.scalarStringColon)").A(".Concat(" + Globals.smpl + ", ").A(node[1].Code).A("))");
                    }
                    break;

                case "ASTFILENAME":
                    {
                        //node.Code.CA(node[0].Code;
                        string s = null;
                        foreach (ASTNode child in node.ChildrenIterator())
                        {
                            if (child.IsFirstChild()) s = child.Code.ToString();
                            else s += ".Add(" + Globals.smpl + ", new ScalarString(`\\\\`)).Add(" + Globals.smpl + ", " + child.Code + ")";
                        }
                        node.Code.A("O.ReplaceSlash(" + s + ")");
                    }
                    break;
                case "ASTFILENAMEPART":
                    {
                        string s = null;
                        foreach (ASTNode child in node.ChildrenIterator())
                        {
                            if (child.IsFirstChild()) s = child.Code.ToString();
                            else s += ".Add(" + Globals.smpl + ", new ScalarString(`.`)).Add(" + Globals.smpl + ", " + child.Code + ")";
                        }
                        node.Code.A(s);
                    }
                    break;
                case "ASTFILENAMEFIRST1":
                    {
                        node.Code.CA(node[0].Code + ".Add(" + Globals.smpl + ", new ScalarString(`:\\\\`)).Add(" + Globals.smpl + ", " + node[1].Code + ")");
                    }
                    break;
                case "ASTFILENAMEFIRST2":
                case "ASTFILENAMEFIRST3":
                    {
                        node.Code.CA(node[0].Code);
                    }
                    break;
                case "ASTPARENTDIRECTORY":
                    {
                        node.Code.A("(new ScalarString(`..`))");
                    }
                    break;
                case "ASTFUNCTION_Q":
                case "ASTFUNCTIONNAKED_Q":
                case "ASTOBJECTFUNCTION_Q":
                case "ASTOBJECTFUNCTIONNAKED_Q":
                case "ASTPROCEDURE_Q":
                case "ASTFUNCTION":
                case "ASTFUNCTIONNAKED":
                case "ASTOBJECTFUNCTION":
                case "ASTOBJECTFUNCTIONNAKED":
                case "ASTPROCEDURE":
                    {
                        bool isQuestion = false;
                        if (node.Text.EndsWith("_Q"))
                        {
                            isQuestion = true;
                        }

                        bool isObjectFunction = false;
                        if (node.Text.Contains("OBJECTFUNCTION"))
                        {
                            isObjectFunction = true;
                        }

                        string functionName = GetFunctionName(node);
                        if (functionName == "null") functionName = "null2";  //cannot have the name Functions.null(...)
                        else if (functionName == "int") functionName = "int2";  //cannot have the name Functions.int(...)

                        string libraryNameWhereTheFunctionIsCallingFrom = CallingLibraryHelper(w);

                        bool hasLibrary; string libraryName;
                        LibraryHelper(node, out hasLibrary, out libraryName);

                        if (node.Text == "ASTPROCEDURE" || node.Text == "ASTPROCEDURE_Q")
                        {
                            functionName = Globals.procedure + functionName;
                        }

                        //will always be null for ASTOBJECTFUNCTION
                        string[] listNames = IsGamsSumFunctionOrUnfoldFunction(node, functionName);  //also checks that the name is "sum"

                        if (!hasLibrary && listNames != null && listNames.Length > 0 && listNames[0] != null)
                        {
                            //We do not expect this to be called with sum?(...), but it will work if so

                            //GAMS-like sum function, for instance sum(#i, x[#i]+1),
                            //or unfold()-function, for instance unfold(#i, x[#i]+1)

                            //string funcName = "localFunc" + ++Globals.counter;

                            string parentListLoopVars1 = "GekkoSmpl " + Globals.smpl + ", ";
                            string parentListLoopVars2 = Globals.smpl + ", ";

                            int parentListLoopCounter = 0;
                            ASTNode node2 = node;
                            while (true)
                            {
                                node2 = node2.Parent;
                                if (node2 == null) break;
                                if (node2.listLoopAnchor != null)
                                {
                                    foreach (KeyValuePair<string, TwoStrings> kvp in node2.listLoopAnchor)
                                    {
                                        parentListLoopVars1 += "IVariable " + kvp.Value.s1 + ", ";
                                        parentListLoopVars2 += kvp.Value.s1 + ", ";
                                        parentListLoopCounter++;
                                    }
                                }
                            }
                            if (parentListLoopVars1 != null) parentListLoopVars1 = parentListLoopVars1.Substring(0, parentListLoopVars1.Length - ", ".Length);
                            if (parentListLoopVars2 != null) parentListLoopVars2 = parentListLoopVars2.Substring(0, parentListLoopVars2.Length - ", ".Length);

                            string tempName = "temp" + ++Globals.counter;
                            string funcName = "func" + ++Globals.counter;

                            string listName = null;
                            if (node.listLoopAnchor == null)
                            {
                                new Error("Internal error #98973422");
                            }

                            //method def:

                            string iv = "GekkoSmpl, IVariable";
                            for (int i = 0; i < parentListLoopCounter; i++)
                            {
                                iv = iv + ", IVariable";
                            }

                            int smplCommandNumber = ++Globals.counter;

                            StringBuilder sb1 = new StringBuilder();

                            if (node.CodeSentFromSubTree != null)
                            {
                                sb1.Append(node.CodeSentFromSubTree);
                            }

                            sb1.AppendLine("Func<" + iv + "> " + funcName + " = (" + parentListLoopVars1 + ") => {");
                            //NOTE: local functions are in C#7, but to compile them .NET 4.6 is necessary. So use Func<> for now, small speed penalty.
                            //Hmm, tried this 27/4 2021, did not work...
                            //sb1.AppendLine(iv + " " + funcName + "(" + parentListLoopVars1 + ")" + " {");

                            if (node.localInsideLoopVariablesCs != null)
                            {
                                sb1.AppendLine(node.localInsideLoopVariablesCs);
                            }

                            if (G.Equal(functionName, "sum"))
                            {
                                sb1.AppendLine(GekkoSmplCommandHelper1(smplCommandNumber, "Sum"));
                                sb1.AppendLine("Series " + tempName + " = new Series(ESeriesType.Normal, Program.options.freq, null); " + tempName + ".SetZero(" + Globals.smpl + ");" + G.NL);
                            }
                            else
                            {
                                sb1.AppendLine(GekkoSmplCommandHelper1(smplCommandNumber, "Unfold"));
                                sb1.AppendLine("List " + tempName + " = new List();" + G.NL);
                            }

                            foreach (KeyValuePair<string, TwoStrings> kvp in node.listLoopAnchor)
                            {
                                string listname = Globals.symbolCollection + kvp.Key;  //add a # to the list ident, this is the way they are stored in node.functionDefAnchor
                                string internalName = SearchUpwardsInTree3(node, listname);
                                string s = SearchUpwardsInTree3(node, listname);

                                if (s == null)
                                {
                                    s = "O.Lookup(" + Globals.smpl + ", null, ((O.scalarStringHash).Add(" + Globals.smpl + ", (new ScalarString(" + Globals.QT + kvp.Key + Globals.QT + ")))), null, new  LookupSettings(), EVariableType.Var, null)";  //false is regarding isLeftSide, null regarding options
                                }

                                sb1.AppendLine("foreach (IVariable " + kvp.Value.s1 + " in new O.GekkoListIterator(" + s + ")) {");
                            }

                            if (G.Equal(functionName, "sum"))
                            {
                                string dollarCode = null;
                                if (node[2].Text == "ASTDOLLAR") dollarCode = node[2][1].Code.ToString();

                                if (!G.NullOrBlanks(dollarCode))
                                {
                                    string vName = "v" + ++Globals.counter;
                                    string s = O.Conditional3Of3(dollarCode, vName);
                                    sb1.AppendLine(s);
                                }

                                sb1.AppendLine(tempName + ".InjectAdd(" + Globals.smpl + ", " + node[3].Code.ToString() + ");" + G.NL);
                                sb1.AppendLine(Globals.labelCounter + "++;"); //not done for unfold. This means that only first item in loop(s) is recorded.
                            }
                            else
                            {
                                //unfold
                                sb1.AppendLine(tempName + ".Add(" + node[3].Code.ToString() + ");" + G.NL);
                            }

                            foreach (KeyValuePair<string, TwoStrings> kvp in node.listLoopAnchor)
                            {
                                sb1.AppendLine("}");
                            }

                            if (G.Equal(functionName, "sum"))
                            {
                                //after a sum(#i, ....) function, the labelCounter must be set to 0, if this sum() function is not inside another sum() function
                                bool b = SearchUpwardsInTree6(node.Parent);
                                if (!b)
                                {
                                    sb1.AppendLine(Globals.labelCounter + " = 0;");
                                }
                            }

                            sb1.AppendLine(GekkoSmplCommandHelper2(smplCommandNumber));  //resets command name to what it was previously
                            sb1.AppendLine("return " + tempName + ";" + G.NL);
                            sb1.AppendLine("};");  //method def, must end with ;

                            string smplLocal, s2_changes; ReplaceSmpl(sb1.ToString(), out smplLocal, out s2_changes);

                            if (w.wh.localFuncsCode == null) w.wh.localFuncsCode = new GekkoStringBuilder();
                            w.wh.localFuncsCode.AppendLine(s2_changes.ToString());

                            node.Code.A(funcName + "(" + parentListLoopVars2 + ")"); //functionname may be for instance temp27(smpl)

                        }
                        else
                        {
                            //Not a sum() or unfold() function that is going to be looped                                

                            string meta = null;
                            if (!hasLibrary && Globals.gekkoInbuiltFunctions.TryGetValue(functionName, out meta))
                            {
                                //Inbuilt function

                                string extra = null;
                                int lagIndex = -12345;
                                int lagIndexOffset = 0;
                                if (meta != null)
                                {
                                    meta = meta.ToLower().Replace(" ", "");
                                    if (meta.StartsWith("lag="))
                                    {
                                        meta = meta.Replace("lag=", "");
                                        if (G.IsInteger(meta))
                                        {
                                            //simple: lag=2                                                
                                            extra = "O.Smpl(" + Globals.smpl + ", " + -int.Parse(meta) + "), ";
                                        }
                                        else
                                        {
                                            //example: lag=[2] or lag=[2]-1
                                            string[] ss = meta.Split('-');
                                            if (ss.Length == 2)
                                            {
                                                lagIndexOffset = -int.Parse(ss[1]);
                                                lagIndex = int.Parse(ss[0].Substring(1, ss[0].Length - 2));
                                            }
                                            else
                                            {
                                                ss = meta.Split('+');
                                                if (ss.Length == 2)
                                                {
                                                    lagIndexOffset = int.Parse(ss[1]);
                                                    lagIndex = int.Parse(ss[0].Substring(1, ss[0].Length - 2));
                                                }
                                                else
                                                {
                                                    //no offset
                                                    lagIndexOffset = 0;
                                                    lagIndex = int.Parse(meta.Substring(1, meta.Length - 2));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //ignore
                                    }
                                }

                                if (lagIndex != -12345 && isObjectFunction)
                                {
                                    //if we compare lag(x, 3) and x.lag(3), the former would have the 
                                    //lag index at position 1, and the latter at position 0. Therefore,
                                    //we deduct 1 if it is an object function.
                                    lagIndex--;
                                }

                                List<string> args = new List<string>();

                                if (node[1].ChildrenCount() == 0)
                                {
                                    //args += ", null, null";
                                    args.Add("null");
                                    args.Add("null");
                                }
                                else
                                {
                                    for (int i = 0; i < node[1].ChildrenCount(); i++)
                                    {
                                        args.Add(node[1][i].Code.ToString());
                                    }
                                }

                                for (int i = 2; i < node.ChildrenCount(); i++)
                                {
                                    args.Add(node[i].Code.ToString());
                                }

                                if (lagIndex >= 3 && (lagIndex - 1 < node.ChildrenCount()))
                                {
                                    if (lagIndexOffset == 0)
                                    {
                                        extra = "O.Smpl(" + Globals.smpl + ", " + node[lagIndex - 1].Code + "), ";
                                    }
                                    else
                                    {
                                        extra = "O.Smpl(" + Globals.smpl + ", O.Add(" + Globals.smpl + ", " + node[lagIndex - 1].Code + ", new ScalarVal(" + lagIndexOffset + "d))), ";
                                    }
                                }

                                string aa1, aa2;
                                FunctionHelper10(args, out aa1, out aa2);

                                if (node.Text == "ASTOBJECTFUNCTION" || node.Text == "ASTOBJECTFUNCTION_Q")
                                {
                                    node.Code.A("Functions." + functionName + "(").A(extra + Globals.functionT1Cs + ", ").A(aa1).A(", " + Globals.objFunctionPlaceholder + "").A("" + aa2).A(")");
                                }
                                else if (node.Text == "ASTOBJECTFUNCTIONNAKED" || node.Text == "ASTOBJECTFUNCTIONNAKED_Q")
                                {
                                    //same as the other???                                        
                                    node.Code.A("Functions." + functionName + "(").A(extra + Globals.functionT1Cs + ", ").A(aa1).A(", " + Globals.objFunctionPlaceholder + "").A("" + aa2).A(")");
                                }
                                else
                                {
                                    node.Code.A("Functions." + functionName).A("(" + extra + Globals.functionT1Cs + ", ").A(aa1 + aa2).A(")");
                                }
                                if (node.Text == "ASTFUNCTIONNAKED" || node.Text == "ASTFUNCTIONNAKED_Q" || node.Text == "ASTOBJECTFUNCTIONNAKED" || node.Text == "ASTOBJECTFUNCTIONNAKED_Q")
                                {
                                    node.Code.A(";" + G.NL);
                                }
                            }
                            else
                            {
                                //User defined function or procedure

                                List<string> args = new List<string>();

                                if (node[1] == null || node[1].ChildrenCount() == 0)
                                {
                                    args.Add("null");
                                    args.Add("null");
                                }
                                else
                                {
                                    for (int i = 0; i < node[1].ChildrenCount(); i++)
                                    {
                                        FunctionHelper2(node[1], args, i);
                                    }
                                }

                                for (int i = 1; i < node.ChildrenCount(); i++)  //first item is function name
                                {
                                    if (node[i].Text == "ASTSPECIALARGS")
                                    {
                                        //just so that ASTSPECIALARGS is easy to find, else could loop from i = 2
                                        continue;
                                    }
                                    FunctionHelper2(node, args, i);
                                }

                                int numberOfArguments = args.Count;

                                //TODO TODO TODO
                                // the 'extra' parameter indicating lag to come
                                //

                                string aa1, aa2;
                                FunctionHelper10(args, out aa1, out aa2);

                                string q = "false";
                                if (isQuestion) q = "true";

                                if (node.Text == "ASTOBJECTFUNCTION" || node.Text == "ASTOBJECTFUNCTION_Q")
                                {
                                    node.Code.A(Globals.functionSpecialName2).A(numberOfArguments + 1).A("(").A(libraryNameWhereTheFunctionIsCallingFrom).A(", ").A(libraryName).A(", `").A(functionName).A("`)(" + Globals.functionTP1Cs + "").A(", " + q).A(", " + aa1).A(", " + Globals.objFunctionPlaceholder + "").A(aa2).A(")");
                                }
                                else if (node.Text == "ASTOBJECTFUNCTIONNAKED" || node.Text == "ASTOBJECTFUNCTIONNAKED_Q")
                                {
                                    node.Code.A(Globals.functionSpecialName2).A(numberOfArguments + 1).A("(").A(libraryNameWhereTheFunctionIsCallingFrom).A(", ").A(libraryName).A(", `").A(functionName + "").A("`)(" + Globals.functionTP1Cs + "").A(", " + q).A(", " + aa1).A(", " + Globals.objFunctionPlaceholder + "").A(aa2).A(")");
                                }
                                else
                                {
                                    node.Code.A(Globals.functionSpecialName2).A(numberOfArguments).A("(").A(libraryNameWhereTheFunctionIsCallingFrom).A(", ").A(libraryName).A(", `").A(functionName).A("`)(" + Globals.functionTP1Cs + "").A(", " + q).A(", " + aa1 + aa2).A(")");
                                }

                                if (node.Text == "ASTFUNCTIONNAKED" || node.Text == "ASTFUNCTIONNAKED_Q" || node.Text == "ASTOBJECTFUNCTIONNAKED" || node.Text == "ASTOBJECTFUNCTIONNAKED_Q" || node.Text == "ASTPROCEDURE" || node.Text == "ASTPROCEDURE_Q")
                                {
                                    node.Code.A(";" + G.NL);
                                }
                            }
                        }
                    }
                    break;

                case "ASTGENERIC1":
                    {
                        //This catches name:name or identDigit.                            
                        if (node.ChildrenCount() == 1)
                        {
                            //identDigit
                            node.Code.CA(node[0].Code);
                        }
                        else
                        {
                            //name:name
                            node.Code.CA("O.Add(" + Globals.smpl + ", " + node[0].Code + ", new ScalarString(`:`), " + node[1].Code + ")");
                        }
                    }
                    break;


                case "ASTMATRIXROW":
                    {
                        node.Code.CA("O.MatrixRow(");
                        for (int i = 0; i < node.ChildrenCount(); i++)
                        {
                            node.Code.A(node[i].Code);
                            if (i < node.ChildrenCount() - 1) node.Code.A(", ");
                        }
                        node.Code.A(")");
                    }
                    break;
                case "ASTMATRIXCOL":
                    {
                        node.Code.CA("O.MatrixCol(");
                        for (int i = 0; i < node.ChildrenCount(); i++)
                        {
                            node.Code.A(node[i].Code);
                            if (i < node.ChildrenCount() - 1) node.Code.A(", ");
                        }
                        node.Code.A(")");

                    }
                    break;

                case "ASTIDENT":
                    {
                        node.Code.CA("new ScalarString(`" + node[0].Text + "`)");  //problem is that we now allow VAL %v = 1, for instance. Here %v is not recursive.
                    }
                    break;

                case "ASTIDENTDIGIT":
                    {
                        if (node[0].Text == "ASTIDENT" || node[0].Text == "ASTNAME")
                        {
                            node.Code.CA(node[0].Code);
                        }
                        else
                        {
                            node.Code.CA("new ScalarString(`" + node[0].Text + "`)");
                        }
                    }
                    break;
                case "ASTDOWNLOAD":
                    {
                        node.Code.A("O.Download o" + Num(node) + " = new O.Download();" + G.NL);
                        string s = null;
                        //if (node[0].Text != null) s = "http://";
                        node.Code.A("o" + Num(node) + ".dbUrl = `" + s + "` + O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        node.Code.A(node[1].Code);  //fileName json
                        if (node[2] != null) node.Code.A(node[2].Code); //px file dump
                        if (node[3] != null) node.Code.A(node[3].Code); //options
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTEMPTYRANGEELEMENT":
                    {
                        node.Code.CA("null");
                    }
                    break;
                case "ASTINDEX":  //the INDEX command
                case "ASTCOUNT":  //the COUNT command
                    {
                        node.Code.A("O.Index o" + Num(node) + " = new O.Index();" + G.NL);
                        if (node[0][0] != null) node.Code.A(node[0][0].Code);  //options
                        if (node[1][0] != null) node.Code.A("o" + Num(node) + ".names2 = " + node[1][0].Code + ";" + G.NL);
                        if (node[2][0] != null) node.Code.A("o" + Num(node) + ".type = @`" + node[2][0].Text + "`;");
                        if (node[3] != null) node.Code.A("o" + Num(node) + ".names1 = " + node[3].Code + ";" + G.NL);
                        if (node.Text != "ASTINDEX") node.Code.A("o" + Num(node) + ".isCountCommand = true;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTPRT":
                    {
                        {
                            //PRT

                            //NOTE: This Func<> does not need to have a smpl argument, since it can not be used in 
                            //      a recursive way.
                            node.Code.A("Func<GraphHelper, string> print" + Num(node) + " = (gh) =>" + G.NL);
                            node.Code.A("{" + G.NL);  //start Action

                            node.Code.A("O.Prt o" + Num(node) + " = new O.Prt();" + G.NL);

                            node.Code.A("" + Globals.labelCounter + " = 0;");

                            node.Code.A("o" + Num(node) + ".guiGraphIsRefreshing = gh.isRefreshing;" + G.NL);
                            node.Code.A("o" + Num(node) + ".guiGraphOperator = gh.operator2;" + G.NL); //printCode is from the Func<> call, is null if PLOT window buttons are not clicked
                            node.Code.A("o" + Num(node) + ".guiGraphIsLogTransform = gh.isLogTransform;" + G.NL);

                            node.Code.A(node[0].Code);  //type (prt, plot, ...)                          
                            GetCodeFromAllChildren(node, node[1]);  //options
                            GetCodeFromAllChildren(node, node[2]);  //option         

                            node.Code.A(LocalCode3(Num(node)));

                            node.Code.A(node[3].Code);

                            //node.Code.A(LocalCode4(Num(node)));

                            node.Code.A("o" + Num(node) + ".printStorageAsFuncCounter = Globals.printStorageAsFunc.Count - 1;" + G.NL);

                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);

                            node.Code.A(LocalCode4(Num(node)));

                            node.Code.A("return o" + Num(node) + ".emfName;" + G.NL);

                            node.Code.A("};" + G.NL);  //end Action

                            node.Code.A("Globals.printStorageAsFunc.Add(Globals.printStorageAsFunc.Count, print" + Num(node) + "); " + G.NL);

                            node.Code.A("print" + Num(node) + "(new GraphHelper());" + G.NL); //end Action

                        }


                    }
                    break;
                case "ASTREBASE":  //the REBASE command
                    {
                        node.Code.A("O.Rebase o" + Num(node) + " = new O.Rebase();" + G.NL);
                        //node.Code.A(node[0].Code);

                        node.Code.A("o" + Num(node) + ".names = " + node[0].Code + ";" + G.NL);

                        if (node[1][0] != null)
                        {
                            node.Code.A("o" + Num(node) + ".t1 = O.ConvertToDate(" + node[1][0].Code + ", O.GetDateChoices.Strict);" + G.NL);
                        }
                        if (node[1][1] != null)
                        {
                            node.Code.A("o" + Num(node) + ".t2 = O.ConvertToDate(" + node[1][1].Code + ", O.GetDateChoices.Strict);" + G.NL);
                        }
                        if (node[2] != null) node.Code.A(node[2].Code);  //options
                        node.Code.A("o" + Num(node) + ".p = p;");
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTINDEXERALONE":
                    {
                        GetCodeFromAllChildren(node);
                    }
                    break;
                case "ASTDOTORINDEXER":
                    {
                        string ivTempVar = SearchUpwardsInTree4(node);  //checks if left-hand side

                        //isLhs is true if the indexer is on the left-hand side, and is the last indexer.
                        //For instance #m[2][3] = 'a' -----> here the [3] indexer will get "true"                            
                        //LIGHTFIXME, isRhs

                        bool reportInterior = ((w.wh.currentCommand == "ASTPRT" || w.wh.currentCommand == "ASTDISP") && !SearchUpwardsInTree5(node.Parent));
                        if (node[1].Text == "ASTDOT")
                        {
                            reportInterior = false;  //never for #x.??? type indexing
                        }

                        string indexes = null;
                        string indexesReport = null;  //DELETE THIS SOON!!! Seems it has no use.

                        List<string> ix = new List<string>(); for (int i = 0; i < node[1].ChildrenCount(); i++) ix.Add(null);
                        List<string> ixr = new List<string>(); for (int i = 0; i < node[1].ChildrenCount(); i++) ixr.Add(null);

                        for (int i = 0; i < node[1].ChildrenCount(); i++)
                        {
                            ASTNode child = node[1][i];

                            if (child[0].Text == "ASTPLUS" || child[0].Text == "ASTMINUS")
                            {
                                //See also #980752345
                                string listName = GetSimpleHashName(child[0][0]);
                                string internalName = null;
                                string internalFunction = null;
                                if (listName != null)
                                {
                                    TwoStrings two = SearchUpwardsInTree2(node, listName);
                                    if (two != null)
                                    {
                                        internalName = two.s1;
                                        internalFunction = two.s2;
                                    }
                                }

                                bool isSum = ReportHelperIsSum(internalName, internalFunction); //a controlled #x, bounded by sum

                                if (internalName != null)
                                {
                                    if (child[0].Text == "ASTPLUS")
                                    {
                                        string s = "O.AddSpecial(" + Globals.smpl + ", " + internalName + ", " + child[0][1].Code + ", false)";
                                        indexes += s;
                                        ix[i] = s;
                                        if (reportInterior && !isSum)
                                        {
                                            indexesReport += Globals.reportLabel1 + s + ", `" + ReportLabelHelper(child) + "`" + Globals.reportLabel2;
                                            ixr[i] = Globals.reportLabel1 + s + ", `" + ReportLabelHelper(child) + "`" + Globals.reportLabel2;
                                        }
                                    }
                                    else
                                    {
                                        string s = "O.AddSpecial(" + Globals.smpl + ", " + internalName + ", " + child[0][1].Code + ", true)";
                                        indexes += s;
                                        ix[i] = s;
                                        if (reportInterior && !isSum)
                                        {
                                            indexesReport += Globals.reportLabel1 + s + ", `" + ReportLabelHelper(child) + "`" + Globals.reportLabel2;
                                            ixr[i] = Globals.reportLabel1 + s + ", `" + ReportLabelHelper(child) + "`" + Globals.reportLabel2;
                                        }
                                    }
                                }
                                else
                                {
                                    string s = node[1][i].Code.ToString();
                                    indexes += s;
                                    ix[i] = s;
                                    if (reportInterior && !isSum)
                                    {
                                        indexesReport += Globals.reportLabel1 + s + ", `" + ReportLabelHelper(child) + "`" + Globals.reportLabel2;
                                        ixr[i] = Globals.reportLabel1 + s + ", `" + ReportLabelHelper(child) + "`" + Globals.reportLabel2;
                                    }
                                }
                                if (i < node[1].ChildrenCount() - 1)
                                {
                                    indexes += ", ";
                                    if (reportInterior && !isSum)
                                    {
                                        indexesReport += ", ";
                                    }
                                }
                            }
                            else
                            {
                                //not plus or minus in indexer
                                string listName = GetSimpleHashName(child[0]);
                                string internalName = null;
                                string internalFunction = null;
                                if (listName != null)
                                {
                                    TwoStrings two = SearchUpwardsInTree2(node, listName);
                                    if (two != null)
                                    {
                                        internalName = two.s1;
                                        internalFunction = two.s2;
                                    }
                                }

                                string s = node[1][i].Code.ToString();
                                if (internalName != null) s = internalName;

                                indexes += s;  //always done as fallback      
                                ix[i] = s;

                                if (reportInterior)
                                {
                                    bool isSum2 = ReportHelperIsSum(internalName, internalFunction);
                                    if (isSum2)
                                    {
                                        indexesReport += s;
                                        ixr[i] = s;
                                    }
                                    else
                                    {
                                        //only for PRT-type or DISP, and only if the [] is not inside [] or {}.

                                        string temp = ReportLabelHelper(child);
                                        if (temp != null)
                                        {
                                            indexesReport += Globals.reportLabel1 + s + ", `" + temp + "`" + Globals.reportLabel2;
                                            ixr[i] = Globals.reportLabel1 + s + ", `" + temp + "`" + Globals.reportLabel2;
                                        }
                                        else
                                        {
                                            indexesReport += s;
                                            ixr[i] = s;
                                        }
                                    }
                                }

                                if (i < node[1].ChildrenCount() - 1)
                                {
                                    indexes += ", ";
                                    if (reportInterior)
                                    {
                                        indexesReport += ", ";
                                    }
                                }
                            }
                            if (ixr[i] == null) ixr[i] = ix[i];
                        }  //end of i loop, for each child

                        string indexerType = "O.EIndexerType.None";
                        if (node[1].ChildrenCount() == 1)
                        {
                            //x[-1] or x[(-1)] or x[+1], but not x[1] x[0-1] or x[0+1].
                            //so it tests + or - in first pos after [, not counting blanks and parentheses
                            if (node[1].Text == "ASTDOT")
                            {
                                indexerType = "O.EIndexerType.Dot";
                            }
                            else if (node[1][0].Text == "ASTINDEXERELEMENTPLUS")
                            {
                                //cannot be so for ASTDOT
                                indexerType = "O.EIndexerType.IndexerLead";
                            }
                            else if (node[1][0][0].Text == "ASTNEGATE")
                            {
                                //cannot be so for ASTDOT
                                indexerType = "O.EIndexerType.IndexerLag";
                            }
                            else
                            {
                                //do nothing
                            }
                        }

                        if (ivTempVar == null)
                        {
                            //it is a right-hand side variable

                            if (indexesReport == null) indexesReport = indexes;

                            if (node[1][0].Text == "ASTOBJECTFUNCTION" || node[1][0].Text == "ASTOBJECTFUNCTION_Q" || node[1][0].Text == "ASTOBJECTFUNCTIONNAKED" || node[1][0].Text == "ASTOBJECTFUNCTIONNAKED_Q")
                            {
                                string functionNameLower = node[1][0][0][0][0].Text.ToLower();

                                bool isInbuilt = false;
                                string meta = null;
                                if (Globals.gekkoInbuiltFunctions.TryGetValue(functionNameLower, out meta)) isInbuilt = true;

                                string s = node[1][0].Code.ToString();
                                string[] ss = s.Split(new string[] { Globals.objFunctionPlaceholder }, StringSplitOptions.None);
                                if (ss.Length != 2)
                                {
                                    new Error("Unexpected function error");
                                }

                                string s2 = null;
                                if (!isInbuilt)
                                {
                                    string code = GetFuncArgumentCode(node, 0);
                                    s2 = s.Replace(Globals.objFunctionPlaceholder, code);
                                }
                                else
                                {
                                    s2 = s.Replace(Globals.objFunctionPlaceholder, node[0].Code.ToString());
                                }
                                node.Code.A(s2);
                            }
                            else
                            {
                                node.Code.A("O.Indexer(O.Indexer2(" + Globals.smpl + ", " + indexerType + "," + Stringlist.GetListWithCommas(ix) + "), " + Globals.smpl + ", " + indexerType + ", " + node[0].Code + ", " + Stringlist.GetListWithCommas(ixr) + ")");

                                //this alternative code is only done for x[a] type of variables, not x.f() etc.

                                if (node[0].AlternativeCode != null)
                                {
                                    node.AlternativeCode = new GekkoSB();
                                    node.AlternativeCode.A("(").A(node[0].AlternativeCode).A(")");
                                    for (int i = 0; i < node[1].ChildrenCount(); i++)
                                    {
                                        if (i == 0) node.AlternativeCode.A(".Add(" + Globals.smpl + ", new ScalarString(\"[\"))");
                                        node.AlternativeCode = node.AlternativeCode.A(".Add(" + Globals.smpl + ", ").A(node[1][i].Code).A(")");
                                        if (i < node[1].ChildrenCount() - 1) node.AlternativeCode.A(".Add(" + Globals.smpl + ", new ScalarString(\", \"))");
                                        if (i == node[1].ChildrenCount() - 1) node.AlternativeCode.A(".Add(" + Globals.smpl + ", new ScalarString(\"]\"))");
                                    }
                                }
                            }
                        }
                        else
                        {
                            //This only happens for x3 in x1.x2.x3.x4 = ..., the last indexer on the LHS

                            //for example:

                            //For instance: #m1.#m2.#m3.x4 = ...
                            //This will produce code similar to this (in reality longer, but same structure):
                            //---------------------------------------------
                            //IVariable FIND_M1 = O.Lookup(smpl, null, null, "#m1", null, null, new LookupSettings(O.ELookupType.LeftHandSide), EVariableType.Var, null);
                            //IVariable FIND_M2 = O.Indexer(null, smpl, O.EIndexerType.Dot, FIND_M1, (O.scalarStringHash).Add(smpl, (new ScalarString("m2"))));
                            //IVariable FIND_M3 = O.Indexer(null, smpl, O.EIndexerType.Dot, FIND_M2, (O.scalarStringHash).Add(smpl, (new ScalarString("m3"))));
                            //O.IndexerSetData(smpl, FIND_M3, ivTmpvar5, o0, (new ScalarString("x4")));
                            //---------------------------------------------
                            //So a lookup on the first container, then index and index, and finally indexersetdata()

                            node.Code.A("O.IndexerSetData(" + Globals.smpl + ", ").A(node[0].Code).A(",  ").A(ivTempVar).A(", ").A(OperatorHelper(node, -12345) + ", ").A(indexes).A(")");

                        }

                    }
                    break;
                case "ASTINDEXER":
                    {
                        //handled in ASTDOTORINDEXER, keep this note here to avoid confusion
                    }
                    break;
                case "ASTINDEXERELEMENTIDENT":
                    {
                        string s = node[0][0].Text;
                        node.Code.A("new ScalarString(\"" + s + "\")");

                    }
                    break;
                case "ASTINDEXERELEMENT":
                case "ASTINDEXERELEMENTPLUS":
                    {
                        if (node.ChildrenCount() == 1)
                        {
                            GetCodeFromAllChildren(node);
                        }
                        else
                        {
                            node.Code.A("(new Range(").A(node[0].Code).A(", ").A(node[1].Code).A("))");
                        }

                    }
                    break;

                case "ASTINTEGER":
                    {
                        string minus = HandleNegate(node);
                        byte b = 0;

                        if (minus == "")
                        {
                            if (node[0].Text.StartsWith("0"))
                            {
                                //#8952042732435
                                //x[-01] will not become x['-01']
                                //but x[01] will become x['01'], if x is an array-series. They way this is done is that
                                //the ScalarVal contains info on trailing zeroes, which can be used later on. This info
                                //is not stored in databanks or anything, and they will not survive addition, multiplication,
                                //etc.  
                                for (byte i = 0; i < node[0].Text.Length; i++)
                                {
                                    if (node[0].Text[i] != '0')
                                    {
                                        b = i;
                                        break;
                                    }
                                    if (i > 250)
                                    {
                                        new Error("Did not expect so many trailing zeroes: " + node[0].Text);
                                        //throw new GekkoException();
                                    }
                                }

                                if (b == 0)
                                {
                                    //for instance '000', has 2 trailing zeroes
                                    //if b == 0 here, all chars are '0'. So we just subtract 1 from length
                                    if (node[0].Text.Length > 250)
                                    {
                                        new Error("Did not expect so many trailing zeroes: " + node[0].Text);
                                        //throw new GekkoException();
                                    }
                                    b = (byte)(node[0].Text.Length - 1);
                                }


                            }
                        }
                        else
                        {
                            //do nothing, if it starts with a minus
                        }

                        string intWithNumber = "i" + ++Globals.counter;
                        string s = "new ScalarVal(" + minus + node[0].Text + "d, " + b + ")";
                        GetHeaderCs(w).AppendLine("public static readonly ScalarVal " + intWithNumber + " = " + s + ";");
                        node.Code.CA(intWithNumber);  //no need for checking if it exists

                    }
                    break;


                case "ASTLISTDEF":
                    {
                        node.Code.A("O.ListDefHelper(");
                        ListDefHelper(node);
                        node.Code.A(")");
                    }
                    break;

                case "ASTLISTFILE":
                    {
                        node.Code.A(node[0].Code);
                    }
                    break;
                case "ASTRANGEGENERAL":
                    {
                        node.Code.A("O.RangeGeneral(" + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "ASTWILDCARDWITHBANK":
                    {
                        string bankCs = node[0].Code.ToString();
                        if (true)
                        {
                            //wildcard   
                            if (bankCs == null || bankCs == "")
                            {
                                node.Code.CA(node[1].Code);
                            }
                            else
                            {
                                node.Code.CA("(" + bankCs + ").Add(" + Globals.smpl + ", new ScalarString(\":\")).Add(" + Globals.smpl + ", " + node[1].Code + ")");
                            }

                        }

                    }
                    break;
                case "ASTRANGEWITHBANK":
                    {
                        if (true)
                        {
                            node.Code.CA("(" + node[0].Code + ").Add(" + Globals.smpl + ", new ScalarString(\"..\")).Add(" + Globals.smpl + ", " + node[1].Code + ")");
                        }
                    }
                    break;


                case "ASTLISTITEMS":
                    {
                        node.Code.A("o" + Num(node) + ".listItems = new List<string>();" + G.NL);
                        GetCodeFromAllChildren(node);
                    }
                    break;


                case "ASTM":
                    {
                        node.Code.A(AddOperator("m", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTV":
                    {
                        node.Code.A(AddOperator("v", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTMEM":
                    {
                        node.Code.A("O.Mem(null);" + G.NL);
                    }
                    break;
                case "ASTMODEL":
                    {
                        node.Code.A("O.Model o" + Num(node) + " = new O.Model();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;");
                        GetCodeFromAllChildren(node); //gets filename
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTEDIT":
                    {
                        node.Code.A("O.Edit o" + Num(node) + " = new O.Edit();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;");
                        GetCodeFromAllChildren(node); //gets filename
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTXEDIT":
                    {
                        node.Code.A("O.XEdit o" + Num(node) + " = new O.XEdit();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;");
                        GetCodeFromAllChildren(node); //gets filename
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTMODE":
                    {
                        node.Code.A("O.Mode o" + Num(node) + " = new O.Mode();" + G.NL);
                        node.Code.A("o" + Num(node) + ".mode = @`" + node[0].Text + "`;");
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTMODEQUESTION":
                    {
                        node.Code.A("O.Mode.Q();" + G.NL);
                    }
                    break;
                case "ASTLIBRARYQUESTION":
                    {
                        if (node[0].ChildrenCount() == 0) node.Code.A("O.Library.Q();" + G.NL);
                        else
                        {
                            node.Code.A("O.Library.Q(" + node[0][0].Code + ");" + G.NL);
                        }
                    }
                    break;
                case "ASTTIMEQUESTION":
                    {
                        node.Code.A("O.Time.Q();" + G.NL);
                    }
                    break;
                case "ASTMP":
                    {
                        node.Code.A(AddOperator("mp", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;

                case "ASTN":
                    {
                        node.Code.A(AddOperator("n", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;

                case "ASTASSIGNMENT":
                    {
                        if (node[0].Text == "ASTASSIGNMENTQUESTION")
                        {
                            //val?, series?, etc.
                            string type = node[1].Text.ToLower();
                            node.Code.A("O.Mem(`" + type + "`);" + G.NL);
                        }
                        else
                        {
                            string number = "_" + ++Globals.counter;

                            string operatorType = "ASTPLACEHOLDER";
                            if (node[4] != null) operatorType = node[4].Text; //ASTHAT2, ASTPERCENT2, ASTPLUS, etc. (ASTPLACEHOLDER if none)

                            GekkoSB sb = new GekkoSB();

                            string ass = "O.Assignment o" + Num(node) + " = new O.Assignment();" + G.NL; //note: using a hack later on

                            if (node.Parent.Text == null || node.Parent.Text == "ASTFUNCTIONDEFCODE" || node.Parent.Text == "ASTPROCEDUREDEFCODE" || node.Parent.Text == "ASTMAPITEM")  //the last convers function/procedure, but also IF and FOR indentation
                            {
                                //only for the top-most node, that is, only 1 time
                                //not for assignments in maps, #m = (x = 5), where x = 5 is assigned.
                                sb.A(ass); //note: using a hack later on
                            }

                            if (node.specialExpressionAndLabelInfo != null)
                            {
                                //may be overwritten with explicit source
                                //maybe later on introduce a #calc list with such meta information
                                sb.A("o" + Num(node) + ".opt_source = @`<[code]>" + G.ReplaceGlueSymbols(node.specialExpressionAndLabelInfo[1]) + "`;" + G.NL);
                            }

                            string type = HandleVar(node[3].Text);  //2 is options   
                            GetCodeFromAllChildren(sb, node[2]);
                            if (G.Equal(type, "STRING2")) type = "string";
                            string ivTempVar = SearchUpwardsInTree4(node[0]);
                            if (ivTempVar == null)
                            {
                                new Error("Internal error #7698248427");
                                //throw new GekkoException();
                            }
                            string convertTo = null;

                            string temp = node[1].Code.ToString();

                            if (operatorType == "ASTHAT2")
                            {
                                sb.A("o" + Num(node) + ".opt_d = `yes`;" + G.NL);
                            }
                            else if (operatorType == "ASTPERCENT2")
                            {
                                sb.A("o" + Num(node) + ".opt_p = `yes`;" + G.NL);
                            }
                            else if (operatorType == "ASTHASH2")
                            {
                                sb.A("o" + Num(node) + ".opt_mp = `yes`;" + G.NL);
                            }

                            if (node[0].ChildrenCount() > 1)  //left-side function like pch(x) = 5
                            {
                                node.Code.A("o" + Num(node) + ".opt_lsfunc = `" + node[0][1][0].Text + "`;" + G.NL);
                            }

                            string methodName = null;
                            if (G.Equal(type, "VAR_KDUSJFLQO2"))
                            {
                                methodName = "Evalcode" + ++Globals.counter;
                                node.Code.A("var " + methodName + " = new List<Func<GekkoSmpl, IVariable>>();");
                            }

                            if (node.listLoopAnchor != null && node.listLoopAnchor.Count > 0)
                            {
                                foreach (KeyValuePair<string, TwoStrings> kvp in node.listLoopAnchor)
                                {
                                    node.Code.A("foreach (IVariable " + kvp.Value.s1 + " in new O.GekkoListIterator(O.Lookup(" + Globals.smpl + ", null, ((O.scalarStringHash).Add(" + Globals.smpl + ", (new ScalarString(`" + kvp.Key + "`)))), null, new  LookupSettings(), EVariableType.Var,     o" + Num(node) + "))) {" + G.NL);
                                }
                            }

                            //is set after any array-series loop, for we get this after the foreach (...)
                            node.Code.A("Globals.precedentsSeries = null;" + G.NL);  //wiping it out

                            if (G.Equal(type, "VAR_KDUSJFLQO2"))
                            {
                                //Special EVAL code

                                StringBuilder sb7 = new StringBuilder();

                                if (!G.NullOrBlanks(node.loopCodeCs))
                                {
                                    string vName = "v" + ++Globals.counter;
                                    string s = O.Conditional3Of3(node.loopCodeCs, vName);
                                    node.Code.A(s);
                                }

                                StringBuilder sb4 = new StringBuilder();
                                sb4.AppendLine("  " + methodName + ".Add((" + Globals.smpl + ") => { ");
                                sb4.AppendLine("return " + node[1].Code.ToString() + " ;");
                                sb4.AppendLine("  });");
                                string codeNew; string smplLocal; ReplaceSmpl(sb4.ToString(), out smplLocal, out codeNew);
                                node.Code.A(codeNew);
                            }
                            else
                            {
                                //normal assignment

                                GekkoSB sb1 = new GekkoSB();
                                GekkoSB sb2 = new GekkoSB();

                                sb1.A(OperatorHelper(null, -Globals.smplOffset)).End();
                                sb1.A("IVariable " + ivTempVar + " = ").A(temp).End();
                                sb1.A(OperatorHelper(null, Globals.smplOffset)).End();

                                sb2.A(sb1); //cloning
                                sb1.A(node[0].Code).End();  //simple Lookup() for sb1

                                //more complicated probing for sb2
                                sb2.A("if (" + ivTempVar + ".Type() != EVariableType.Series) return false;" + G.NL);
                                sb2.A("O.Dynamic1(" + Globals.smpl + ");" + G.NL);
                                sb2.A(node[0].Code).End();
                                sb2.A("return O.Dynamic2(" + Globals.smpl + ");" + G.NL);

                                node.Code.A("Action assign" + number + " = () => {" + G.NL);  //start of action
                                node.Code.A(sb1);
                                node.Code.A("};" + G.NL);  //end of action

                                node.Code.A("Func<bool> check" + number + " = () => {" + G.NL);  //start of action
                                node.Code.A(sb2);
                                node.Code.A("};" + G.NL);  //end of action

                                node.Code.A("O.RunAssigmentMaybeDynamic(" + Globals.smpl + ", assign" + number + ", check" + number + ", " + "o" + Num(node) + ");" + G.NL);

                            }

                            if (node.listLoopAnchor != null && node.listLoopAnchor.Count > 0)
                            {
                                foreach (KeyValuePair<string, TwoStrings> kvp in node.listLoopAnchor)
                                {
                                    node.Code.A("}" + G.NL);
                                }
                            }

                            if (G.Equal(type, "VAR_KDUSJFLQO2"))
                            {
                                node.Code.A("Globals.expressions = " + methodName + ";" + G.NL);
                            }

                            string localFuncCode = "";
                            if (w.wh.localFuncsCode != null) localFuncCode = w.wh.localFuncsCode.ToString();

                            // HACK HACK HACK
                            // HACK HACK HACK
                            // HACK HACK HACK
                            //#6473443634
                            //A hack: check that the assignment of the same object is not already there. The hack should be relatively safe.
                            if (sb.ToString().Contains(ass))  //can probably only contain one of these
                            {
                                localFuncCode = sb.ToString() + G.NL + localFuncCode.Replace(ass, "");
                            }
                            else
                            {
                                localFuncCode = sb.ToString() + G.NL + localFuncCode;
                            }

                            w.wh.localFuncsCode = new GekkoStringBuilder();
                            w.wh.localFuncsCode.Append(localFuncCode);
                        }
                    }
                    break;
                case "ASTPERCENT":
                    {
                        node.Code.A("O.scalarStringPercent");
                    }
                    break;
                case "ASTHASH":
                    {
                        node.Code.A("O.scalarStringHash");
                    }
                    break;
                case "ASTEXCLAMATION":
                    {
                        node.Code.A("O.scalarStringExclamation");
                    }
                    break;
                case "ASTMAPDEF":
                    {
                        string funcName = "MapDef_" + node.mapTempVarName;
                        string s2 = "Map " + node.mapTempVarName + " = new Map();" + G.NL;
                        foreach (ASTNode child in node.ChildrenIterator()) s2 += child.Code.ToString();
                        if (w.wh.localFuncsCode == null) w.wh.localFuncsCode = new GekkoStringBuilder();
                        string smplLocal, s2_changes; ReplaceSmpl(s2, out smplLocal, out s2_changes);
                        w.wh.localFuncsCode.AppendLine("Func<GekkoSmpl, Map> " + funcName + " = (" + smplLocal + ") => {" + G.NL + s2_changes + G.NL + "return " + node.mapTempVarName + ";" + G.NL + "};" + G.NL);
                        node.Code.A(funcName + "(" + Globals.smpl + ")");
                    }
                    break;
                case "ASTMAPITEM":
                    {
                        GetCodeFromAllChildren(node);
                    }
                    break;

                case "ASTNAKEDLISTMISS":
                    {
                        string name = node[0][0].Text;
                        if (G.Equal(name, "m") || G.Equal(name, "miss"))
                        {
                            node.Code.A("Globals.scalarValMissing, null");  //the null is: no rep
                        }
                    }
                    break;

                case "ASTNAKEDLISTITEM":
                    {
                        if (node.ChildrenCount() > 1)
                        {
                            node.Code.A(node[0].Code + ", " + node[1].Code);
                        }
                        else
                        {
                            node.Code.A(node[0].Code + ", " + "null");
                        }
                    }
                    break;

                case "ASTBANKVARNAMELIST":
                case "ASTNAKEDLIST":
                    {
                        string naked = "false";
                        if (node.Text == "ASTNAKEDLIST") naked = "true";
                        bool isFor = false;

                        if (node?.Parent?.Text == "ASTPLACEHOLDER" && node?.Parent?.Parent?.Text == "ASTFORTYPE2" && node?.Parent?.Parent?.Parent?.Text == "ASTPLACEHOLDER" && node?.Parent?.Parent?.Parent?.Parent?.Text == "ASTFOR")
                        {
                            //We need to check parents carefully, otherwise the INDEX in stuff like FOR string %i = ... ; INDEX * to #m; END; will think it is a FOR-statement list (where '#' is not allowed)
                            isFor = true;
                        }

                        string code = "O.FlattenIVariablesSeq(" + naked + ", new List(new List<IVariable> {";
                        if (isFor) code = "O.FlattenIVariablesSeqFor(" + naked + ", new List(new List<IVariable> {";

                        foreach (ASTNode child in node.ChildrenIterator())
                        {
                            string name = null;
                            if (child.Text == "ASTNAKEDLISTMISS" || child.Text == "ASTNAKEDLISTITEM" || child.Text == "ASTSEQ7" || child.Text == "ASTWILDCARDWITHBANK" || child.Text == "ASTRANGEWITHBANK" || child.Text == "ASTSEQITEMMINUS")
                            {
                                name = child.Code.ToString();
                            }
                            else
                            {
                                if (child.AlternativeCode == null)
                                {
                                    new Error("Name is expected. Use {...} to turn a string into a name.");
                                    //throw new GekkoException();
                                }
                                name = child.AlternativeCode.ToString();
                                if (name == null || name == "")
                                {
                                    new Error("Name is expected. Use {...} to turn a string into a name.");
                                    //throw new GekkoException();
                                }
                            }
                            code += name + ", ";
                        }
                        code = code.Substring(0, code.Length - ", ".Length);
                        code += "}))";
                        node.Code.CA(code);  //CA() overrides the Lookup(...) stuff that was made in subnodes
                    }
                    break;
                case "ASTFILENAMELIST":
                    {
                        string code = "O.FlattenIVariables(new List(new List<IVariable> {";
                        foreach (ASTNode child in node.ChildrenIterator())
                        {
                            string name = null;
                            name = child.Code.ToString();
                            code += name + ", ";
                        }
                        code = code.Substring(0, code.Length - ", ".Length);
                        code += "}))";
                        node.Code.CA(code);  //CA() overrides the Lookup(...) stuff that was made in subnodes
                    }
                    break;
                case "ASTBANKVARNAME":
                    {
                        //The structure is the following:
                        // b1:%x!q

                        //       ASTBANKVARNAME (child)
                        //         ASTPLACEHOLDER
                        //           ASTNAME
                        //             ASTIDENT
                        //               b1                <-- bankname
                        //         ASTVARNAME
                        //           ASTPLACEHOLDER
                        //             ASTPERCENT          <-- %-sigil
                        //           ASTPLACEHOLDER
                        //             ASTNAME
                        //               ASTIDENT
                        //                 x               <-- varname
                        //           ASTPLACEHOLDER
                        //             ASTNAME
                        //               ASTIDENT
                        //                 q               <--- freq indicator


                        /*

                       #m.#m2.#m3.z = 56
                                   astleftside
                                     / 
                                    /                                         
                             astdotori   
                               /     \
                              /       \
                         astdotori    astdot
                           /   \         \
                          /     \         \
                    astdotori   astdot     z --------> this is the assign
                     / \         \                 if it has one astdotorindexer above,
                    /   \         \                and then a astleftside, we are ok.
                   #m  astdot      #m3 --------------> but this is where the IndexerSetData
                         \                        is called, because it might be #m.#m2.x[2000],
                          \                       where ivtempvar != null 
                           #m2


ASTASSIGNMENT [0]
ASTLEFTSIDE [0]
  ASTDOTORINDEXER [0]
    ASTDOTORINDEXER [0]
      ASTDOTORINDEXER [0]
        ASTBANKVARNAME [0]
          ASTPLACEHOLDER [0]
          ASTVARNAME [0]
            ASTPLACEHOLDER [0]
              ASTHASH [0]
            ASTPLACEHOLDER [1]
              ASTNAME [1]
                ASTIDENT [1]
                  m [1]
            ASTPLACEHOLDER [0]
        ASTDOT [0]
          ASTVARNAME [0]
            ASTPLACEHOLDER [0]
              ASTHASH [0]
            ASTPLACEHOLDER [1]
              ASTNAME [1]
                ASTIDENT [1]
                  m2 [1]
            ASTPLACEHOLDER [0]
      ASTDOT [0]
        ASTVARNAME [0]
          ASTPLACEHOLDER [0]
            ASTHASH [0]
          ASTPLACEHOLDER [1]
            ASTNAME [1]
              ASTIDENT [1]
                m3 [1]
          ASTPLACEHOLDER [0]
    ASTDOT [0]
      ASTVARNAME [0]
        ASTPLACEHOLDER [0]
        ASTPLACEHOLDER [1]
          ASTNAME [1]
            ASTIDENT [1]
              z [1]
        ASTPLACEHOLDER [0]
ASTINTEGER [1]
  56 [1]
ASTPLACEHOLDER [0]
ASTPLACEHOLDER [0]
ASTPLACEHOLDER [0]
[0]
*/

                        //int leftSideType = 0;  //(0): none, (1): simple x = ..., (2): #m1.#m2.x = ... or #m[2][3] = ...
                        //bool isMapItem = (node?.Parent?.Text == "ASTASSIGNMENT" && node?.Parent?.Parent?.Text == "ASTMAPITEM") || (node?.Parent?.Parent.Text == "ASTASSIGNMENT" && node?.Parent?.Parent?.Parent.Text == "ASTMAPITEM");
                        bool isMapItem = SearchUpwardsInTree9(node);  //finds ASTMAPITEM and ASTFUNCTIONDEFCODE
                        bool isLeftSide = node?.Parent.Text == "ASTLEFTSIDE";

                        bool isLoneOnRightSide = node?.Parent.Text == "ASTASSIGNMENT";

                        string optionsString = "null";
                        if (!isMapItem && isLeftSide)
                        {
                            //four possibilities, y1 = x1 and #m = (y2 = x2)
                            //here, x1 and x2 have !isLeftSide
                            //for y2, isMapItem is true.
                            //only one left is thus y1, for which printcodes work.                                                                                            
                            optionsString = OperatorHelper(node, -12345);  //options like <p> or <keep> etc.
                        }

                        Tuple<bool, string> tuple = CheckIfLeftSide(node);  //In x[%s1, %s2][%date] = ... this will only be true for x, not for the other vars
                        bool isLeftSideVariable = tuple.Item1;
                        string type = tuple.Item2;

                        string lookupSettings = "new  LookupSettings()";
                        if (isLeftSideVariable) lookupSettings = "new  LookupSettings(O.ELookupType.LeftHandSide)";

                        string ivTempVar = SearchUpwardsInTree4(node);

                        string s = GetSimpleName(node);
                        string internalName = null;
                        if (s != null) internalName = SearchUpwardsInTree3(node, s);
                        bool functionHit = false;
                        if (internalName != null)
                        {
                            if (isLeftSideVariable && ivTempVar != null)
                            {
                                node.Code.CA(internalName + " = " + ivTempVar + ";" + G.NL);
                            }
                            else
                            {
                                node.Code.CA(internalName);
                            }
                            functionHit = true;
                        }

                        if (!functionHit)
                        {
                            string mapName = "null";
                            if (node.Parent.Text == "ASTLEFTSIDE" && node.Parent.Parent.Text == "ASTASSIGNMENT" && node.Parent.Parent.Parent.Text == "ASTMAPITEM" && node.Parent.Parent.Parent.Parent.Text == "ASTMAPDEF")
                            {
                                mapName = node.Parent.Parent.Parent.Parent.mapTempVarName;
                                if (mapName == null) throw new GekkoException();
                            }

                            if (ivTempVar == null) ivTempVar = "null";

                            //Check for simple variable like b:x!q or b:%s
                            //This is only for performance reasons, making lookup faster especially for VALs                            

                            //Check bank
                            string simpleBank = null;
                            if (node[0][0] == null)
                            {
                                simpleBank = "";  //signals that it does not exist, so treated as ok
                            }
                            else
                            {
                                //#746384984 merge these in a method
                                if (node[0][0].Text == "ASTNAME" && node[0][0].ChildrenCount() == 1)
                                {
                                    if (node[0][0][0].Text == "ASTIDENT")
                                    {
                                        simpleBank = node[0][0][0][0].Text;
                                    }
                                    else if (node[0][0][0].Text == "REF")
                                    {
                                        simpleBank = Globals.Ref;
                                    }
                                }
                            }

                            //Check sigil
                            string sigil = GetSigilAsString(node[1][0]);

                            //Check name
                            string simpleName = null;
                            //#746384984 merge these in a method
                            if (node[1][1][0].Text == "ASTNAME" && node[1][1][0].ChildrenCount() == 1 && node[1][1][0][0].Text == "ASTIDENT")
                            {
                                simpleName = node[1][1][0][0][0].Text;
                            }

                            //Check frequency
                            string simpleFreq = null;
                            if (node[1][2][0] == null)
                            {
                                simpleFreq = "";  //signals that it does not exist, so treated as ok
                            }
                            else
                            {
                                //#746384984 merge these in a method
                                if (node[1][2][0].Text == "ASTNAME" && node[1][2][0].ChildrenCount() == 1 && node[1][2][0][0].Text == "ASTIDENT")
                                {
                                    simpleFreq = node[1][2][0][0][0].Text;
                                }
                            }

                            string lookupCode = null;

                            if (simpleBank != null && simpleName != null && simpleFreq != null)
                            {
                                //Ok is simple stuff like b:ts!f, or b:%v
                                //We override anything in composed in sub-nodes...!
                                //We do this for speed, in all these simple cases, to avoid IVariables etc.

                                //For instance, b:x!q --> smpl, "b", "x", "q"
                                //For instance, b:%x --> smpl, "b", "%x", null
                                //For instance, x --> smpl, null, "x", null
                                //simpleName can never contain a '!', but if simpleFreq = null, a freq like "!a" will be added
                                //  when looking up.

                                string code = MaybeControlledSet(node);
                                if (code != null)
                                {
                                    //a controlled list, like y[#i] = x[#i] inside an implicit loop or sum loop over #i.
                                    node.Code.A(code);  //will be the C# name of the listitem
                                }
                                else
                                {

                                    string hasSigilText = "false";
                                    if (sigil != null) hasSigilText = "true";
                                    string simpleBankText777 = Globals.QT + simpleBank + Globals.QT;
                                    if (simpleBank == "") simpleBankText777 = "null";
                                    string simpleFreqText777 = Globals.QT + simpleFreq + Globals.QT;
                                    if (simpleFreq == "") simpleFreqText777 = "null";

                                    lookupCode = "O.Lookup(" + Globals.smpl + ", " + mapName + ", " + simpleBankText777 + ", " + Globals.QT + sigil + simpleName + Globals.QT + ", " + simpleFreqText777 + ", " + ivTempVar + ", " + lookupSettings + ", EVariableType." + type + ", " + optionsString + ")";

                                    node.AlternativeCode = new GekkoSB();
                                    string ss = sigil + simpleName;
                                    if (simpleBankText777 != "null") ss = simpleBank + Globals.symbolBankColon + ss;
                                    if (simpleFreqText777 != "null") ss = ss + Globals.freqIndicator + simpleFreq;
                                    node.AlternativeCode.A("new ScalarString(" + Globals.QT + ss + Globals.QT + ")");
                                }
                            }
                            else
                            {
                                //Complicated name, for instance
                                //{%s}, a%s, a{#m}, {%b}:a, b:a!{%f}, %(%s),  ... 

                                //#746384984 merge these in a method
                                string nameAndBankCode = null;
                                if (node[0][0] == null)
                                {
                                    //no bank indicator        
                                    nameAndBankCode = node[1].Code.ToString();
                                }
                                else
                                {
                                    //bank indicator  
                                    string bankNameCs = null;
                                    bankNameCs = node[0][0].Code.ToString();
                                    if (true)
                                    {
                                        nameAndBankCode = "(" + bankNameCs + ")" + ".Concat(" + Globals.smpl + ", O.scalarStringColon)" + ".Concat(" + Globals.smpl + ", " + node[1].Code + ")";
                                    }
                                }
                                lookupCode = "O.Lookup(" + Globals.smpl + ", " + mapName + ", " + nameAndBankCode + ", " + ivTempVar + ", " + lookupSettings + ", EVariableType." + type + ", " + optionsString + ")";

                                node.AlternativeCode = new GekkoSB();
                                node.AlternativeCode.A("" + nameAndBankCode + "");
                            }
                            node.Code.A(lookupCode);
                        }
                        break;
                    }

                case "ASTVARNAME":
                    {
                        bool s0 = false; if (node[0][0] != null) s0 = true;  //sigil % or #
                        bool s2 = false; if (node[2][0] != null) s2 = true;  //freq !

                        if (s0)
                        {
                            if (s2)
                            {
                                //%a!q, does not make sense...
                                if (true)
                                {
                                    node.Code.A("(" + node[0][0].Code + ")").A(".Concat(" + Globals.smpl + ", " + node[1][0].Code + ")").A(".Concat(" + Globals.smpl + ", O.scalarStringTilde)").A(".Concat(" + Globals.smpl + ", " + node[2][0].Code + ")");
                                }

                            }
                            else
                            {
                                //%a
                                if (true)
                                {
                                    node.Code.A("(" + node[0][0].Code + ")").A(".Concat(" + Globals.smpl + ", " + node[1][0].Code + ")");
                                }

                            }
                        }
                        else
                        {
                            if (s2)
                            {
                                //a!q
                                if (true)
                                {
                                    node.Code.A("(" + node[1][0].Code + ")").A(".Concat(" + Globals.smpl + ", O.scalarStringTilde)").A(".Concat(" + Globals.smpl + ", " + node[2][0].Code + ")");
                                }
                            }
                            else
                            {
                                //a
                                node.Code = node[1][0].Code;
                            }
                        }
                    }
                    break;
                case "ASTNAME":
                case "ASTCNAME":
                    {
                        if (node.ChildrenCount() > 0)
                        {
                            int counter = 0;
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                if (counter == 0)
                                {
                                    node.Code.A("(" + child.Code + ")");
                                }
                                else
                                {
                                    if (true)
                                    {
                                        node.Code.A(".Concat(" + Globals.smpl + ", " + child.Code + ")");
                                    }
                                }
                                counter++;
                            }
                            if (node.ChildrenCount() == 1 && node[0].Text == "ASTIDENT") node.nameSimpleIdent = node[0][0].Text;
                        }
                        else
                        {
                            //do nothing, probably we can never end here
                        }
                    }
                    break;

                case "ASTNO":
                    {
                        node.Code.CA("new ScalarString(`no`)");
                    }
                    break;
                case "ASTNEGATE":
                    {
                        node.Code.A("O.Negate(" + Globals.smpl + ", " + node.GetChildCode(0) + ")");
                    }
                    break;
                case "ASTSEQITEMMINUS":
                    {
                        node.Code.A("(").A("(new ScalarString(`-`)).Add(" + Globals.smpl + ", " + node[0].Code.ToString() + ")").A(")");
                    }
                    break;
                case "ASTOPEN":
                    {
                        //node.Code.A(Globals.clearTsCsCode + G.NL);
                        node.Code.A("O.Open o" + Num(node) + " = new O.Open();" + G.NL);
                        GetCodeFromAllChildren(node);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTLEFTSIDE":
                    {
                        node.Code.A(node[0].Code + G.NL);
                    }
                    break;
                case "ASTOPENHELPER":
                    {
                        node.Code.A("o" + Num(node) + ".openFileNames2 = " + node[0].Code + ";" + G.NL);
                        string as2 = "null";
                        if (node[1][0] != null)
                        {
                            as2 = node[1][0].Code.ToString();
                        }
                        node.Code.A("o" + Num(node) + ".openFileNamesAs2 = " + as2 + ";" + G.NL);
                    }
                    break;
                case "ASTOPT_":
                    {
                        //See the stuff just before this big switch statement, ASTOPT_STRING_xxx etc.
                        //Gets the stuff copied to this node, so we do not have to do this manually
                        GetCodeFromAllChildren(node);
                    }
                    break;
                case "ASTOPTION":
                    {
                        if (node[0].Text == "?")
                        {
                            node.Code.A("O.PrintOptions(null);");
                        }
                        else
                        {
                            Tuple<string, string> tup = HandleOptionAndBlock(node, false);
                            node.Code.A(tup.Item1 + " = " + tup.Item2 + ";" + G.NL);
                            node.Code.A("O.PrintOptions(`" + tup.Item1 + "`);" + G.NL);
                            node.Code.A("O.HandleOptions(`" + tup.Item1 + "`, 0, p);" + G.NL);
                        }

                    }
                    break;
                case "ASTOPT1":  //PRT-type statement
                case "ASTOPT2":  //PRT-type statement
                    {
                        //node.Code.A("codes.Add(`" + node[0].Text + "`);" + G.NL;
                        GetCodeFromAllChildren(node);
                    }
                    break;
                case "ASTOPN":
                    {
                        node.Code.A("`n`");
                    }
                    break;
                case "ASTOPD":
                    {
                        node.Code.A("`d`");
                    }
                    break;
                case "ASTOPP":
                    {
                        node.Code.A("`p`");
                    }
                    break;
                case "ASTOPM":
                    {
                        node.Code.A("`m`");
                    }
                    break;
                case "ASTOPMP":
                    {
                        node.Code.A("`mp`");
                    }
                    break;
                case "ASTOPQ":
                    {
                        node.Code.A("`q`");
                    }
                    break;
                case "ASTP":
                    {
                        node.Code.A(AddOperator("p", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;

                case "ASTL":
                    {
                        node.Code.A(AddOperator(Globals.operator_l, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTDL":
                    {
                        node.Code.A(AddOperator(Globals.operator_dl, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTSL":
                    {
                        node.Code.A(AddOperator(Globals.operator_rl, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTSDL":
                    {
                        node.Code.A(AddOperator(Globals.operator_rdl, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;


                case "ASTPCH":
                    {
                        node.Code.A(AddOperator("pch", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTABS":
                    {
                        node.Code.A(AddOperator("abs", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTDIF":
                    {
                        node.Code.A(AddOperator("dif", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTGDIF":
                    {
                        node.Code.A(AddOperator("gdif", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTLEV":
                    {
                        node.Code.A(AddOperator("lev", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;

                case "ASTNAMEHELPER":
                    {
                        if (node[0] != null)
                        {
                            node.Code.CA("o" + Num(node) + ".name = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        }
                    }
                    break;
                case "ASTIMPOSE":  //"impose = " in OLS
                    {
                        node.Code.A("o" + Num(node) + ".impose = " + node[0].Code + ";" + G.NL);
                    }
                    break;
                case "ASTPRTTYPE":
                    {
                        node.Code.A("o" + Num(node) + ".prtType = `" + node[0].Text + "`;" + G.NL);
                    }
                    break;
                case "ASTPRTOPTION":
                    {
                        node.Code.A(node[0].Code + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTOPTIONFIELD":
                    {
                        GetCodeFromAllChildren(node);
                    }
                    break;
                case "===ASTPRTROWS":  //TT: Wonder what this is: can probably be deleted, looks like a hack
                    {
                        node.Code.CA(node[0].Code);
                    }
                    break;
                case "ASTNEWTABLE":
                    {
                        node.Code.A("O.CreateNewTable(O.ConvertToString(" + node[0].Code + "));" + G.NL);
                    }
                    break;
                case "ASTTABLENEXT":
                    {
                        node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Next();");
                    }
                    break;
                case "ASTTABLEPRINT":
                    {
                        if (node.ChildrenCount() > 1)
                        {
                            node.Code.A("O.PrintTable(O.GetTable(O.ConvertToString(" + node[0].Code + ")), O.ConvertToString(" + node[1].Code + "));" + G.NL);
                        }
                        else
                        {
                            node.Code.A("O.PrintTable(O.GetTable(O.ConvertToString(" + node[0].Code + ")), null);" + G.NL);
                        }
                    }
                    break;
                case "ASTTABLESETVALUES":
                    {
                        node.Code.A("O.Table.SetValues o" + Num(node) + " = new O.Table.SetValues();" + G.NL);
                        node.Code.A("o" + Num(node) + ".name = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".col = O.ConvertToInt(" + node[1].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".t1 = O.ConvertToDate(" + node[2].Code + ", O.GetDateChoices.Strict);" + G.NL);
                        node.Code.A("o" + Num(node) + ".t2 = O.ConvertToDate(" + node[3].Code + ", O.GetDateChoices.Strict);" + G.NL);
                        node.Code.A("o" + Num(node) + ".operator2 = O.ConvertToString(" + node[5].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".scale = O.ConvertToVal(" + node[6].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".format = O.ConvertToString(" + node[7].Code + ");" + G.NL);

                        node.Code.A("try {").A(G.NL);
                        node.Code.A("O.isTableCall = true;").A(G.NL);
                        node.Code.A(node[4].Code);
                        node.Code.A("}").A(G.NL);
                        node.Code.A("finally {").A(G.NL);
                        node.Code.A("  O.isTableCall = false;").A(G.NL);
                        node.Code.A("}").A(G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;

                case "ASTTABLESETTOPBORDER":
                case "ASTTABLESETBOTTOMBORDER":
                    {
                        string name = node.GetChild(0).Text;
                        //CheckCurrow(node.GetChild(1).Text);                            
                        string ss = "Top";
                        if (node.Text == "ASTTABLESETBOTTOMBORDER") ss = "Bottom";
                        node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Set" + ss + "Border(O.ConvertToInt(" + node[2].Code + "), O.ConvertToInt(" + node[3].Code + "));" + G.NL);
                    }
                    break;

                case "ASTTABLESETLEFTBORDER":
                case "ASTTABLESETRIGHTBORDER":
                    {
                        bool color = false;
                        if (node.ChildrenCount() == 4)
                        {
                            color = true;
                        }
                        string ss = "Left";
                        if (node.Text == "ASTTABLESETRIGHTBORDER") ss = "Right";
                        if (color)
                        {
                            node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Set" + ss + "Border(O.ConvertToInt(" + node[2].Code + "), O.ConvertToInt(" + node[3].Code + "));" + G.NL);
                        }
                        else
                        {
                            node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Set" + ss + "Border(O.ConvertToInt(" + node[2].Code + "));" + G.NL);
                        }
                    }
                    break;

                case "ASTTABLEHIDELEFTBORDER":
                case "ASTTABLEHIDERIGHTBORDER":
                    {
                        string ss = "Left";
                        if (node.Text == "ASTTABLEHIDERIGHTBORDER") ss = "Right";
                        node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Hide" + ss + "Border(O.ConvertToInt(" + node[2].Code + "));" + G.NL);
                    }
                    break;
                case "ASTTABLESHOWBORDERS":
                    {
                        node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.ShowBorders();");
                    }
                    break;

                case "ASTTABLESETTEXT":
                    {
                        node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.SetText(O.ConvertToInt(" + node[2].Code + "), O.ConvertToString(" + node[3].Code + "));");
                    }
                    break;

                case "ASTTABLEALIGNLEFT":
                case "ASTTABLEALIGNCENTER":
                case "ASTTABLEALIGNRIGHT":
                    {
                        string type = "Left";
                        if (node.Text == "ASTTABLEALIGNCENTER") type = "Center";
                        else if (node.Text == "ASTTABLEALIGNRIGHT") type = "Right";
                        node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Align" + type + "(O.ConvertToInt(" + node[2].Code + "));");
                    }
                    break;
                case "ASTTABLEMERGECOLS":
                    {
                        node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.MergeCols(O.ConvertToInt(" + node[2].Code + "), O.ConvertToInt(" + node[3].Code + "));");
                    }
                    break;

                case "ASTTABLESETDATES":
                    {
                        node.Code.A("O.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.SetDates(O.ConvertToInt(" + node[2].Code + "), O.ConvertToDate(" + node[3].Code + ", O.GetDateChoices.Strict), O.ConvertToDate(" + node[4].Code + ", O.GetDateChoices.Strict));");
                    }
                    break;


                case "ASTTABLE":
                case "ASTMENUTABLE":
                    {
                        node.Code.A("O.Table.CallTableFile o" + Num(node) + " = new O.Table.CallTableFile();" + G.NL);
                        GetCodeFromAllChildren(node);
                        if (node.Text == "ASTMENUTABLE") node.Code.A("o" + Num(node) + ".menuTable = true;" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTLINETYPE":
                    {
                        node.Code.A("ope" + Num(node) + ".linetype = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTDASHTYPE":
                    {
                        node.Code.A("ope" + Num(node) + ".dashtype = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTLINEWIDTH":
                    {
                        node.Code.A("ope" + Num(node) + ".linewidth = O.ConvertToVal(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTLINECOLOR":
                    {
                        node.Code.A("ope" + Num(node) + ".linecolor = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTPOINTTYPE":
                    {
                        node.Code.A("ope" + Num(node) + ".pointtype = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTPOINTSIZE":
                    {
                        node.Code.A("ope" + Num(node) + ".pointsize = O.ConvertToVal(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTFILLSTYLE":
                    {
                        node.Code.A("ope" + Num(node) + ".fillstyle = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTY2":
                    {
                        node.Code.A("ope" + Num(node) + ".y2 = O.ConvertToString(`yes`);" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTDEC":
                    {
                        node.Code.A("ope" + Num(node) + ".dec = O.ConvertToInt(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTPRTELEMENTWIDTH":
                    {
                        node.Code.A("ope" + Num(node) + ".width = O.ConvertToInt(" + node[0].Code + ");" + G.NL);
                    }
                    break;

                case "ASTPRTELEMENTNDEC":
                    {
                        node.Code.A("ope" + Num(node) + ".ndec = O.ConvertToInt(" + node[0].Code + ");" + G.NL);
                    }
                    break;

                case "ASTPRTELEMENTNWIDTH":
                    {
                        node.Code.A("ope" + Num(node) + ".nwidth = O.ConvertToInt(" + node[0].Code + ");" + G.NL);
                    }
                    break;

                case "ASTPRTELEMENTPDEC":
                    {
                        node.Code.A("ope" + Num(node) + ".pdec = O.ConvertToInt(" + node[0].Code + ");" + G.NL);
                    }
                    break;

                case "ASTPRTELEMENTPWIDTH":
                    {
                        node.Code.A("ope" + Num(node) + ".pwidth = O.ConvertToInt(" + node[0].Code + ");" + G.NL);
                    }
                    break;

                case "ASTPRTTIMEFILTER":
                    {
                        node.Code.A("o").A(Num(node)).A(".timefilter = @`").A(node[0].Text).A("`;").A(G.NL);
                    }
                    break;

                case "ASTPRTELEMENT":
                case "ASTTABLESETVALUESELEMENT":
                    {

                        w.expressionCounter++;
                        node.Code.A("{" + G.NL);  //avoid scope collisions                            
                        if (node.Text == "ASTTABLESETVALUESELEMENT")
                        {
                            node.Code.A("List<int> bankNumbers = O.Prt.CreateBankHelper(1);" + G.NL);
                        }
                        else if (node.Text == "ASTOLSELEMENT")
                        {
                            node.Code.A("List<int> bankNumbers = O.Prt.CreateBankHelper(1);" + G.NL);
                        }
                        else  //prt
                        {
                            node.Code.A("List<int> bankNumbers = null;" + G.NL);
                        }

                        string givenLabel = null;
                        if (node.Text != "ASTTABLESETVALUESELEMENT")
                        {
                            if (node.specialExpressionAndLabelInfo[2] != "")
                            {
                                givenLabel = G.ReplaceGlueSymbols(node.specialExpressionAndLabelInfo[2]);
                                givenLabel = G.StripQuotes(givenLabel);
                                givenLabel = Globals.labelCheatString + givenLabel;
                            }
                            else givenLabel = G.ReplaceGlueSymbols(node.specialExpressionAndLabelInfo[1]);
                        }
                        node.Code.A("O.Prt.Element ope" + Num(node) + " = new O.Prt.Element();" + G.NL);  //this must be after the list start iterator code                                                       

                        string freelists = null;
                        if (node.freeIndexedLists != null && node.freeIndexedLists.Count > 0)
                        {
                            List<string> xx = new List<string>();
                            foreach (string ss in node.freeIndexedLists.Keys)
                            {
                                xx.Add(ss);
                            }
                            xx.Sort(StringComparer.OrdinalIgnoreCase);
                            foreach (string s in xx)
                            {
                                freelists += s + ", ";
                            }
                        }
                        if (freelists != null)
                        {
                            freelists = freelists.Substring(0, freelists.Length - ", ".Length);
                            freelists = Globals.freelists + freelists + Globals.freelists;
                        }

                        string label5 = "`" + freelists + ReportLabelHelper(node) + "`";
                        ASTNode labelInHyphens = node.GetChild("ASTGEKKOLABEL");
                        if (labelInHyphens != null)
                        {
                            label5 = "O.ConvertToString(" + labelInHyphens[0].Code + ")";
                        }
                        label5 = label5.Replace(G.NL, ""); //remove any newlines, else C# code will become invalid.                            

                        node.Code.A("ope" + Num(node) + ".labelGiven = new List<string>() {" + label5 + "};" + G.NL);
                        if (givenLabel != null) givenLabel = givenLabel.Replace(G.NL, ""); //remove any newlines, else C# code will become invalid.                            

                        node.Code.A("" + Globals.smpl + " = new GekkoSmpl(o" + Num(node) + ".t1, o" + Num(node) + ".t2); " + Globals.smpl + ".t0 = " + Globals.smpl + ".t0.Add(-2);" + G.NL);

                        ASTNode child = node.GetChild("ASTPRTELEMENTOPTIONFIELD");
                        if (child != null) node.Code.A(child.Code);

                        if (node.Text == "ASTPRTELEMENT")
                        {
                            node.Code.A("ope" + Num(node) + ".operatorsFinal = Program.GetElementOperators(o" + Num(node) + ", ope" + Num(node) + ");");
                            node.Code.A("bankNumbers = O.Prt.GetBankNumbers(null, ope" + Num(node) + ".operatorsFinal);");
                        }
                        else if (node.Text == "ASTTABLESETVALUESELEMENT")
                        {
                            node.Code.A("bankNumbers = O.Prt.GetBankNumbers(Globals.tableOption, new List<string>(){o" + Num(node) + ".operator2}" + ");" + G.NL);
                        }
                        node.Code.A(G.NL);
                        //node.Code.A("foreach(int bankNumber in bankNumbers) {" + G.NL);

                        //tt123
                        node.Code.A("for(int bankNumberI = 0; bankNumberI < bankNumbers.Count; bankNumberI++) {" + G.NL);
                        node.Code.A("int bankNumber = bankNumbers[bankNumberI];" + G.NL);
                        node.Code.A("" + Globals.smpl + ".bankNumber = bankNumber;" + G.NL);

                        //node.Code.A("smpl" 
                        node.Code.A("ope" + Num(node) + ".variable[bankNumber] = " + node[0].Code + ";" + G.NL);

                        node.Code.A("if(bankNumberI == 0) O.PrtElementHandleLabel(" + Globals.smpl + ", ope" + Num(node) + ");" + G.NL);
                        //node.Code.A("O.PrtElementHandleLabel(smpl, ope" + Num(node) + ");" + G.NL);

                        node.Code.A("}" + G.NL);  //end of bankNumbers
                        node.Code.A("" + Globals.smpl + ".bankNumber = 0;" + G.NL);  //resetting, probably superfluous
                        node.Code.A("o" + Num(node) + ".prtElements.Add(ope" + Num(node) + ");" + G.NL);
                        node.Code.A("}" + G.NL);  //avoid scope collisions

                        //node.Code.A(Globals.GekkoSmplNull);

                    }
                    break;

                case "ASTOLSELEMENTS":
                case "ASTPRTELEMENTS":
                    {
                        GetCodeFromAllChildren(node);
                    }
                    break;
                case "ASTRENAME":
                    {
                        //similar to ASTCOPY
                        node.Code.A("O.Rename o" + Num(node) + " = new O.Rename();" + G.NL);

                        if (node[0] != null) node.Code.A("o" + Num(node) + ".type = @`" + node[0].Text + "`;");
                        node.Code.A("o" + Num(node) + ".names0 = " + node[1].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".names1 = " + node[2].Code + ";" + G.NL);

                        if (node[3] != null) node.Code.A(node[2].Code); //options

                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;

                case "ASTSIGN":
                    {
                        node.Code.A("O.Sign();" + G.NL);
                    }
                    break;
                case "ASTSIM":
                    {
                        node.Code.A("O.Sim o" + Num(node) + " = new O.Sim();" + G.NL);
                        GetCodeFromAllChildren(node);  //gets dates and options
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTSMOOTH":
                    {
                        node.Code.A("O.Smooth o" + Num(node) + " = new O.Smooth();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".names0 = " + node[1].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".names1 = " + node[2].Code + ";" + G.NL);
                        if (node[3] != null) node.Code.A("o" + Num(node) + ".names2 = " + node[3].Code + ";" + G.NL);
                        if (node[0].ChildrenCount() > 0) node.Code.A(node[0][0].Code);  //method option
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTSPLICE":
                    {
                        node.Code.A("O.Splice o" + Num(node) + " = new O.Splice();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".names0 = " + node[0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".names1 = " + node[1].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".names2 = " + node[2].Code + ";" + G.NL);

                        if (node.ChildrenCount() > 3)
                        {
                            node.Code.A("o" + Num(node) + ".date = O.ConvertToDate(" + node[3].Code + ", O.GetDateChoices.Strict);" + G.NL);
                        }
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);

                    }
                    break;
                case "ASTTIME":
                    {
                        node.Code.A("O.Time o" + Num(node) + " = new O.Time();" + G.NL);
                        GetCodeFromAllChildren(node);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTTIMEFILTER":
                    {
                        node.Code.A("O.TimeFilter " + "o" + Num(node) + " = new O.TimeFilter();" + G.NL);
                        GetCodeFromAllChildren(node);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTTIMEFILTERPERIODS":
                    {
                        GetCodeFromAllChildren(node);
                    }
                    break;
                case "ASTTIMEFILTERPERIOD":
                    {
                        string tempName = "temp" + ++Globals.counter;
                        node.Code.A("O.TimeFilterHelper " + tempName + " = new O.TimeFilterHelper();" + G.NL);
                        if (node.ChildrenCount() == 1)
                        {
                            node.Code.A(tempName + ".from = O.ConvertToDate(" + node[0].Code + ", O.GetDateChoices.Strict);" + G.NL);
                        }
                        else if (node.ChildrenCount() == 2)
                        {
                            node.Code.A(tempName + ".from = O.ConvertToDate(" + node[0].Code + ", O.GetDateChoices.Strict);" + G.NL);
                            node.Code.A(tempName + ".to = O.ConvertToDate(" + node[1].Code + ", O.GetDateChoices.Strict);" + G.NL);
                        }
                        else if (node.ChildrenCount() == 3)
                        {
                            node.Code.A(tempName + ".from = O.ConvertToDate(" + node[0].Code + ", O.GetDateChoices.Strict);" + G.NL);
                            node.Code.A(tempName + ".to = O.ConvertToDate(" + node[1].Code + ", O.GetDateChoices.Strict);" + G.NL);
                            node.Code.A(tempName + ".step = O.ConvertToInt(" + node[2].Code + ");" + G.NL);
                        }
                        node.Code.A("o" + Num(node) + ".timeFilterPeriods.Add(" + tempName + ");" + G.NL);
                    }
                    break;
                case "ASTQ":
                    {
                        node.Code.A(AddOperator("q", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                    }
                    break;
                case "ASTEVAL":
                    {

                        bool isLoop = node.listLoopAnchor != null && node.listLoopAnchor.Count > 0;
                        isLoop = true;

                        node.Code.A("Globals.expressionText = @`" + G.StripQuotes(G.ReplaceGlueSymbols(node.specialExpressionAndLabelInfo[1])) + "`;" + G.NL);

                        string methodName = "Evalcode" + ++Globals.counter;

                        StringBuilder code = new StringBuilder();
                        string codeNew = null;

                        StringBuilder before = new StringBuilder();
                        StringBuilder after = new StringBuilder();
                        if (node.listLoopAnchor != null)
                        {
                            foreach (KeyValuePair<string, TwoStrings> kvp in node.listLoopAnchor)
                            {
                                before.Append("foreach (IVariable " + kvp.Value.s1 + " in new O.GekkoListIterator(O.DecompLooper(`" + Globals.symbolCollection + kvp.Key + "`))) {" + G.NL);
                                after.Append("}");
                            }
                        }

                        ////List<Func<GekkoSmpl, IVariable>> Evalcode555 = new List<Func<GekkoSmpl, IVariable>>();                                
                        code.Append("var " + methodName + " = new List<Func<GekkoSmpl, IVariable>>();");
                        code.Append(before);

                        code.Append("  " + methodName + ".Add((" + Globals.smpl + ") => { ");
                        code.Append("return " + node[0].Code.ToString() + " ;");
                        code.Append("  });");
                        code.Append(after);
                        string smplLocal; ReplaceSmpl(code.ToString(), out smplLocal, out codeNew);


                        StashIntoLocalFuncs(w, methodName, codeNew, isLoop);

                        if (isLoop)
                        {
                            node.Code.A("Globals.expressions = " + methodName + ";" + G.NL);
                        }
                        else
                        {
                            node.Code.A("Globals.expression = " + methodName + ";" + G.NL);
                        }

                        //node.Code.A("Globals.freeIndexedListsDecomp = null;" + G.NL);  //clearing it just in case

                    }
                    break;
                case "ASTDECOMP":
                    {
                        node.Code.A("O.Decomp1 o" + Num(node) + " = new O.Decomp1();" + G.NL);
                        node.Code.A("o" + Num(node) + ".label = @`" + G.StripQuotes(G.ReplaceGlueSymbols(node.specialExpressionAndLabelInfo[1])) + "`;" + G.NL);
                        GetCodeFromAllChildren(node);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTDECOMPITEMS":
                    {
                        string methodName = "Evalcode" + ++Globals.counter;
                        StashIntoLocalFuncs(w, methodName, node[0].Code.ToString(), false);
                        node.Code.A("o" + Num(node) + ".expression = " + methodName + ";" + G.NL);
                    }
                    break;
                case "ASTDECOMPITEMS2":
                    {
                        node.Code.A("o" + Num(node) + ".name = " + node[0].Code + ";" + G.NL);
                    }
                    break;
                case "ASTDECOMPITEMSEXPR":
                    {
                        string methodName = "Evalcode" + ++Globals.counter;
                        string code = null;

                        string n2 = "null";
                        if (node[2].ChildrenCount() > 0) n2 = node[2][0].Code.ToString();

                        if (node[1].ChildrenCount() == 1)
                        {
                            //DECOMP x1 + x2
                            code = node[1][0].Code.ToString();
                        }
                        else
                        {
                            //DECOMP y = x1 + x2                                
                            code = Program.EquationLhsRhs(node[1][0].Code.ToString(), node[1][1].Code.ToString(), false);
                        }

                        StashIntoLocalFuncs(w, methodName, code, false);
                        string n = "null";
                        if (node[0].ChildrenCount() > 0) n = node[0][0].Code.ToString();
                        node.Code.A("o" + Num(node) + ".decompItems.Add(new DecompItems(" + methodName + ", " + n + ", null, " + n2 + "))" + ";" + G.NL);
                    }
                    break;
                case "ASTDECOMPITEMSNAME":
                    {
                        string n2 = "null";
                        if (node[2].ChildrenCount() > 0) n2 = node[2][0].Code.ToString();
                        string n1 = "null";
                        if (node[1].ChildrenCount() > 0) n1 = node[1][0].Code.ToString();
                        string n = "null";
                        if (node[0].ChildrenCount() > 0) n = node[0][0].Code.ToString();
                        node.Code.A("o" + Num(node) + ".decompItems.Add(new DecompItems(null, " + n + ", " + n1 + ", " + n2 + "))" + ";" + G.NL);

                    }
                    break;
                case "ASTDECOMP2":
                case "ASTDECOMP3":
                    {
                        node.Code.A("O.Decomp2 o" + Num(node) + " = new O.Decomp2();" + G.NL);
                        node.Code.A("o" + Num(node) + ".type = @`" + node.Text + "`;" + G.NL);
                        node.Code.A("o" + Num(node) + ".label = @`" + G.StripQuotes(G.ReplaceGlueSymbols(node.specialExpressionAndLabelInfo[1])) + "`;" + G.NL);
                        GetCodeFromAllChildren(node);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTDECOMPWHERE":
                    {
                        for (int i = 0; i < node.ChildrenCount(); i++)
                        {
                            node.Code.A("o" + Num(node) + ".where.Add(" + node[i].Code + ");" + G.NL);
                        }
                    }
                    break;
                case "ASTDECOMPFROM":
                    {
                        if (node.ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".from.Add(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTDECOMPSELECT":
                    {
                        if (node.ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".select.Add(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTDECOMPENDO":
                    {
                        if (node.ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".endo.Add(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTDECOMPROWS":
                    {
                        if (node.ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".rows.Add(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTDECOMPCOLS":
                    {
                        if (node.ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".cols.Add(" + node[0].Code + ");" + G.NL);
                    }
                    break;
                case "ASTDECOMPWHERE2":
                    {
                        node.Code.A("new List<IVariable>() {" + node[0].Code + ", " + node[1].Code + "}");
                    }
                    break;
                case "ASTDECOMPGROUP":
                    {
                        for (int i = 0; i < node.ChildrenCount(); i++)
                        {
                            node.Code.A("o" + Num(node) + ".group.Add(" + node[i].Code + ");" + G.NL);
                        }
                    }
                    break;
                case "ASTDECOMPGROUP1c":
                case "ASTDECOMPGROUP1d":
                    {
                        if (node.Code.ToString() == "") node.Code.A("null");
                    }
                    break;
                case "ASTDECOMPGROUP1":
                    {
                        node.Code.A("new List<IVariable>() {" + node[0].Code + ", " + node[1].Code + ", " + node[2][0].Code + ", " + node[3][0].Code + "}");
                    }
                    break;
                case "ASTDECOMPLINK":
                    {
                        GetCodeFromAllChildren(node);
                    }
                    break;
                case "ASTDECOMPLINK1":
                    {
                        GetCodeFromAllChildren(node);
                    }
                    break;
                case "ASTUNFIX":
                    node.Code.A("O.Unfix();" + G.NL);
                    break;

                case "ASTPIPE":
                    {
                        node.Code.A("O.Pipe o" + Num(node) + " = new O.Pipe();" + G.NL);
                        GetCodeFromAllChildren(node);  //gets fileName and options
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTLIBRARY":
                    {
                        node.Code.A("O.Library o" + Num(node) + " = new O.Library();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);

                        GetCodeFromAllChildren(node, node[0]);  //options

                        ASTNode files = node[1];
                        foreach (ASTNode child in files.ChildrenIterator())
                        {
                            string s1 = child[0].Code.ToString();
                            node.Code.A("o" + Num(node) + ".files.Add(" + s1 + ");" + G.NL);
                            if (child.ChildrenCount() > 1)
                            {
                                string s2 = child[1].Code.ToString();
                                node.Code.A("o" + Num(node) + ".aliases.Add(" + s2 + ");" + G.NL);
                            }
                            else
                            {
                                node.Code.A("o" + Num(node) + ".aliases.Add(null);" + G.NL);
                            }
                        }

                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTREAD":
                    {
                        //node.Code.A(Globals.clearTsCsCode + G.NL);
                        node.Code.A("O.Read o" + Num(node) + " = new O.Read();" + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".type = @`" + node[0].Text + "`;");
                        GetCodeFromAllChildren(node);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTREADTO":
                    {
                        if (node.ChildrenCount() > 0)
                        {
                            if (node[0].Text == "ASTBANKISSTARCHEATCODE")
                            {
                                node.Code.A("o" + Num(node) + ".readTo = `*`;");
                            }
                            else
                            {
                                node.Code.A("o" + Num(node) + ".readTo =  O.ConvertToString(" + node[0].Code + ");");
                            }
                        }
                    }
                    break;

                case "ASTWRITE":
                    {
                        node.Code.A("O.Write o" + Num(node) + " = new O.Write();" + G.NL);
                        node.Code.A("o" + Num(node) + ".type = @`" + node[0].Text + "`;");
                        GetCodeFromAllChildren(node, node[1]);  //options
                        node.Code.A("o" + Num(node) + ".fileName = " + node[2].Code + ";" + G.NL);
                        if (!node[3].Code.IsNull()) node.Code.A("o" + Num(node) + ".list1 = " + node[3].Code + ";" + G.NL);
                        if (!node[4].Code.IsNull()) node.Code.A("o" + Num(node) + ".list2 = " + node[4].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".type = @`" + node[0].Text + "`;");
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;

                case "ASTNAMESLIST":
                    {
                        GetCodeFromAllChildren(node);
                    }
                    break;
                case "ASTSHEETIMPORT":
                    {
                        node.Code.A("O.SheetImport o" + Num(node) + " = new O.SheetImport();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);
                        node.Code.A(node[1].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".names = " + node[2].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTX12A":
                    {
                        node.Code.A("O.X12a o" + Num(node) + " = new O.X12a();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);
                        node.Code.A("o" + Num(node) + ".names = " + node[1].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTR_FILE":
                    {
                        node.Code.A("O.R_file o" + Num(node) + " = new O.R_file();" + G.NL);
                        node.Code.A("o" + Num(node) + ".fileName = O.ConvertToString(" + node[0][0].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTR_EXPORT":
                    {
                        node.Code.A("O.R_export o" + Num(node) + " = new O.R_export();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);
                        node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTR_RUN":
                    {
                        node.Code.A("O.R_run o" + Num(node) + " = new O.R_run();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);
                        if (node[1].ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        if (node[2].ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".fileName = O.ConvertToString(" + node[2][0].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;

                case "ASTPYTHON_FILE":
                    {
                        node.Code.A("O.Python_file o" + Num(node) + " = new O.Python_file();" + G.NL);
                        node.Code.A("o" + Num(node) + ".fileName = O.ConvertToString(" + node[0][0].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTPYTHON_EXPORT":
                    {
                        node.Code.A("O.Python_export o" + Num(node) + " = new O.Python_export();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);
                        node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTPYTHON_RUN":
                    {
                        node.Code.A("O.Python_run o" + Num(node) + " = new O.Python_run();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);
                        if (node[1].ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".names = " + node[1][0].Code + ";" + G.NL);
                        if (node[2].ChildrenCount() > 0) node.Code.A("o" + Num(node) + ".fileName = O.ConvertToString(" + node[2][0].Code + ");" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;

                case "ASTTRANSLATE":
                    {
                        node.Code.A("O.Translate o" + Num(node) + " = new O.Translate();" + G.NL);
                        GetCodeFromAllChildren(node);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTFILENAMESTAR":
                    {
                        node.Code.CA("new ScalarString(`*`)");
                    }
                    break;
                case "ASTHANDLEFILENAME":
                    {
                        if (node.ChildrenCount() > 0)
                        {
                            node.Code.A("o" + Num(node) + ".fileName = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        }
                    }
                    break;
                case "ASTHANDLEFILENAME2": //in case we have 2 filenames
                    {
                        if (node.ChildrenCount() > 0)
                        {
                            node.Code.A("o" + Num(node) + ".fileName2 = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        }
                    }
                    break;
                case "ASTRUN":
                    {
                        node.Code.A(LocalCode1(Num(node), null, null)); //see LocalCode2
                        node.Code.A("O.Run o" + Num(node) + " = new O.Run();" + G.NL);
                        //HMMM is this right:
                        node.Code.A("o" + Num(node) + ".fileName = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        if (node[1] != null) node.Code.A(node[1].Code + G.NL);
                        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        node.Code.A(LocalCode2(Num(node), null)); //see LocalCode1
                    }
                    break;

                case "ASTSTRINGINQUOTES":
                    {
                        string s = G.StripQuotes(node[0].Text);
                        //for instance, @"this is a ""word"" shown", where "" are kind of @-escaped.
                        //but @ will keep backslashes.
                        s = G.HandleQuoteInQuote(s);
                        node.Code.CA("O.HandleString(new ScalarString(@`" + s + "`))");
                    }
                    break;
                case "ASTSTRINGINQUOTESWITHCURLIES":
                    {
                        string ss = null;
                        for (int i = 0; i < node.ChildrenCount(); i += 2)
                        {
                            //always uneven number of items
                            //if there are 5 items, i will be 0, 2, 4, where 4 is the last.
                            string s1 = G.HandleQuoteInQuote(node[i].Text.Substring(1, node[i].Text.Length - 2));
                            string s2 = null;
                            string add = null;
                            if (i + 1 < node.ChildrenCount()) add = ".Add(" + Globals.smpl + ", O.CurlyMethod(" + Globals.smpl + ", " + node[i + 1].Code.ToString() + "))";

                            if (i >= 2) ss += ".Add(" + Globals.smpl + ", O.HandleString(new ScalarString(@`" + s1 + "`)))" + add;
                            else ss += "O.HandleString(new ScalarString(@`" + s1 + "`))" + add;
                        }

                        //for instance, @"this is a ""word"" shown", where "" are kind of @-escaped.
                        //but @ will keep backslashes.

                        node.Code.CA(ss.ToString());
                    }
                    break;

                case "ASTTRUNCATE":
                    {
                        node.Code.A("O.Truncate o" + Num(node) + " = new O.Truncate();" + G.NL);
                        node.Code.A(node[0].Code);  //options
                        GetCodeFromAllChildren(node, node[0]);  //options
                        node.Code.A("o" + Num(node) + ".names = " + node[1].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTCOMPARECOMMAND":
                    {
                        node.Code.A("O.Compare o" + Num(node) + " = new O.Compare();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);
                        if (node[1][0] != null)
                        {
                            node.Code.A("o" + Num(node) + ".listItems = " + node[1][0].Code + ";" + G.NL);
                        }
                        if (node[2][0] != null)
                        {
                            node.Code.A("o" + Num(node) + ".fileName = O.ConvertToString(" + node[2][0].Code + ");" + G.NL);
                        }
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTDISP":
                    {
                        node.Code.A("O.Disp o" + Num(node) + " = new O.Disp();" + G.NL);
                        node.Code.A("" + Globals.labelCounter + " = 0;");
                        node.Code.A(node[0].Code);  //dates
                        if (node[1][0] != null) node.Code.A("o" + Num(node) + ".type = @`" + node[1][0].Text + "`;");
                        node.Code.A("o" + Num(node) + ".iv = " + node[2].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTPREDICT":
                    {
                        node.Code.A("O.Predict o" + Num(node) + " = new O.Predict();" + G.NL);
                        node.Code.A(node[0].Code);  //dates
                                                    //if (node[1][0] != null) node.Code.A("o" + Num(node) + ".type = @`" + node[1][0].Text + "`;");
                        node.Code.A("o" + Num(node) + ".iv = " + node[1].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTFIND":
                    {
                        node.Code.A("O.Find o" + Num(node) + " = new O.Find();" + G.NL);
                        GetCodeFromAllChildren(node, node[0]);  //options                            
                        node.Code.A("o" + Num(node) + ".iv = " + node[1].Code + ";" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTINI":
                    {
                        node.Code.A("O.Ini(p);");
                    }
                    break;
                case "ASTINFO":
                    {
                        node.Code.A("O.Info o" + Num(node) + " = new O.Info();" + G.NL);
                        node.Code.A(node[0].Code);  //dates
                        node.Code.A(node[1].Code);  //list                            
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;

                case "ASTITERSHOW":
                    {

                        node.Code.A("O.Itershow o" + Num(node) + " = new O.Itershow();" + G.NL);
                        node.Code.A(node[0].Code);  //dates
                        node.Code.A("o" + Num(node) + ".names = " + node[1].Code + ";" + G.NL);
                        //node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        break;

                    }
                    break;
                case "ASTDISPSEARCH":
                    {
                        node.Code.A("O.Disp o" + Num(node) + " = new O.Disp();" + G.NL);
                        node.Code.A("o" + Num(node) + ".searchName = `" + G.StripQuotes(node[0][0].Text) + "`;" + G.NL);
                        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    }
                    break;
                case "ASTPAUSE":
                    {
                        if (node.ChildrenCount() == 0) node.Code.A("O.Pause(``);");
                        else node.Code.A("O.Pause(O.ConvertToString(" + node[0].Code + "));");
                        break;
                    }
                    break;


                case "ASTYES":
                    {
                        node.Code.A("new ScalarString(`yes`)");
                    }
                    break;
                case "ASTAVG":
                    {
                        node.Code.A("new ScalarString(`avg`)");
                    }
                    break;
                case "ASTTOTAL":
                    {
                        node.Code.A("new ScalarString(`total`)");
                    }
                    break;
                case "ASTTABLEMAIN":
                    {
                        node.Code.A("new ScalarString(`main`)");
                    }
                    break;
                case "ASTWILDCARD":
                    {
                        bool first = true;
                        string s = null;
                        foreach (ASTNode child in node.ChildrenIterator())
                        {
                            if (first) s = child.Code.ToString();
                            else s += ".Add(" + Globals.smpl + ", " + child.Code + ")";
                            first = false;
                        }
                        node.Code.CA(s);
                    }
                    break;
                case "ASTWILDSTAR":
                    {
                        node.Code.CA("new ScalarString(`*`)");
                    }
                    break;
                case "ASTWILDQUESTION":
                    {
                        node.Code.CA("new ScalarString(`?`)");
                    }
                    break;

            }
        }

        /// <summary>
        /// Get name of library if called like b1 in b1:x
        /// </summary>
        /// <param name="node"></param>
        /// <param name="hasLibrary"></param>
        /// <param name="libraryName"></param>
        private static void LibraryHelper(ASTNode node, out bool hasLibrary, out string libraryName)
        {
            hasLibrary = false;
            libraryName = "null";
            string temp = GetLibraryName(node);
            if (temp != null)
            {
                hasLibrary = true;
                libraryName = "`" + temp + "`";
            }
        }

        /// <summary>
        /// Get library from which the function is calling from.
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private static string CallingLibraryHelper(W w)
        {
            string libraryNameWhereTheFunctionIsCallingFrom = "null";  //the library from where the function call originates (may be null)
            if (w.libraryName != null)
            {
                libraryNameWhereTheFunctionIsCallingFrom = "`" + w.libraryName + "`";
            }

            return libraryNameWhereTheFunctionIsCallingFrom;
        }

        private static Tuple<string, string> HandleOptionAndBlock(ASTNode node, bool isBlock)
        {
            //See also #jkafjkaddasfas
            //Item1: the option variable, for instance Program.options.folder_working
            //Item2: the value as C# code (IVariable code)

            string ss7 = null;
            bool first = true;
            for (int i = 0; i < node.ChildrenCount() - 1; i++)
            {
                if (!first) ss7 += " ";
                string s = null;                
                s = node[i][0].Text.ToLower();
                ss7 += s;
                first = false;
            }

            //the list is short, ok to not use a Dictionary here
            foreach (List<string> ss in Globals.listSyntaxAlias)
            {                
                if (ss[0] == ss7)
                {
                    ss7 = ss[1];
                    break;
                }
            }

            List<string> rv = null;
            foreach (List<string> ss in Globals.listSyntax)
            {
                if (G.Equal(ss[0], ss7))
                {
                    rv = ss;
                    break;
                }
            }

            if (rv == null || rv[1] == null)
            {
                new Error("OPTION " + ss7 + " = ... does not exist");
                //throw new GekkoException();
            }

            if (!isBlock && rv[0] == "series dyn")
            {
                new Error("You cannot use 'option series dyn ...', use 'block series dyn ...' instead.");
                //throw new GekkoException();
            }

            string type = rv[1];
            string f = "(";

            if (type == Globals.xbool)
            {
                f = "O.XBool(";
            }
            else if (type == Globals.xstring)
            {
                f = "O.XString(";
            }
            else if (type == Globals.xint)
            {
                f = "O.XInt(";
            }
            else if (type == Globals.xval)
            {
                f = "O.XVal(";
            }
            else if (type == Globals.xval2String)
            {
                f = "O.XVal2String(";
            }
            else if (type == Globals.xnameOrString)
            {
                f = "O.XNameOrString(";
            }
            else if (type == Globals.xnameOrString2Freq)
            {
                f = "O.XNameOrString2Freq(";
            }
            else if (type == Globals.xnameOrStringOrFilename)
            {
                f = "O.XNameOrStringOrFilename(";
            }
            else if (type == Globals.xoptionSeriesMissing)
            {
                f = "O.XOptionSeriesMissing(";
            }
            else if (type == Globals.xsint)
            {
                f = "O.XSint(";
            }
            else
            {
                new Error("Option type problem");
            }

            Tuple<string, string> tup = new Tuple<string, string>("Program.options." + ss7.Replace(" ", "_"), f + node[node.ChildrenCount() - 1].Code + ")");
            return tup;
        }

        private static int GetLineRecursive(ASTNode node)
        {
            int line2 = 0;
            WalkASTSimple(node, 0, ref line2);
            return line2;
        }

        private static void LinesForSpecialCommands(ASTNode node)
        {              
            node.Code.A(G.NL + Globals.splitSpecial + Num(node) + G.NL);            
        }

        private static string MaybeControlledSet(ASTNode node)
        {
            string rv = null;
            
            string listName = GetSimpleHashName(node);
            if (listName != null)
            {
                if (node.Parent != null && node.Parent.listLoopAnchor != null && node.Number == 2)
                {
                    //this is #i in sum(#i, ...) or unfold(#i, ...). Note that the latter function is artificial, captures uncontrolled sets.
                    return rv;  //null
                }
                TwoStrings two = SearchUpwardsInTree2(node.Parent, listName);
                if (two != null) rv = two.s1;
            }
            return rv;
        }

        private static string MaybeControlledSet777(ASTNode node, string code)
        {
            string listName = GetSimpleHashName(node);
            if (listName != null)
            {
                TwoStrings two = SearchUpwardsInTree2(node.Parent, listName);
                if (two != null) code = two.s1;
            }

            return code;
        }

        private static string GetParametersInAList(ASTNode node, int numberOfParameters, int j)
        {
            string vars = null;
            for (int i = 0; i < numberOfParameters; i++)
            {
                if (j == 0) vars += ", GekkoArg " + node.functionDef[i].internalName + "_func"; //type is checked later on                                    
                else vars += ", " + node.functionDef[i].internalName + "_func"; //type is checked later on                                    
            }
            return vars;
        }

        private static void FunctionHelper10(List<string> args, out string aa1, out string aa2)
        {
            aa1 = Stringlist.GetListWithCommas(args.GetRange(0, 2));
            aa2 = Stringlist.GetListWithCommas(args.GetRange(2, args.Count - 2));
            if (args.Count - 2 > 0) aa2 = ", " + aa2;
        }
        
        private static void FunctionHelper2(ASTNode node, List<string> args, int i)
        {            
                string result = GetFuncArgumentCode(node, i);
                args.Add(result);                
        }

        private static void FunctionHelper4(ASTNode node, string functionName, ASTNode child)
        {
            string type = child[0].Text;
            string sigil = null;
            if (child[1][0][0] != null) sigil = child[1][0][0].Text;
            string name = child[1][1][0].Text;
            string s = null;
            if (sigil == "ASTPERCENT") s += Globals.symbolScalar;
            else if (sigil == "ASTHASH") s += Globals.symbolCollection;

            s += name;

            CheckTypeInFunctionDefProcedureDefForDef(functionName, type, s);

            if (node.functionDefAnchor == null) node.functionDefAnchor = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (node.functionDefAnchor.ContainsKey(s))
            {
                new Error("The variable name " + s + " is used several times in a FUNCTION definition");
                //throw new GekkoException();
            }
            node.functionDefAnchor.Add(s, Globals.functionArgName + ++Globals.counter);
            if (node.functionDef == null) node.functionDef = new List<ArgHelper>();
            node.functionDef.Add(new ArgHelper(type.ToLower(), Globals.functionArgName + Globals.counter, null, null));
        }

        private static void StashIntoLocalFuncs(W w, string c, string s0, bool isLoop)
        {
            string smplLocal, s0_changes; ReplaceSmpl(s0, out smplLocal, out s0_changes);
            if (w.wh.localFuncsCode == null) w.wh.localFuncsCode = new GekkoStringBuilder();
            if (!isLoop)
            {
                w.wh.localFuncsCode.Append("Func<GekkoSmpl, IVariable> " + c + " = (" + smplLocal + ") => { return " + s0_changes + ";" + G.NL + " };" + G.NL);
            }
            else
            {
                w.wh.localFuncsCode.Append(s0);
            }

        }

        private static void ReplaceSmpl(string inputCs, out string smplLocal, out string outputCs)
        {
            smplLocal = "smpl" + ++Globals.counter;
            outputCs = inputCs.Replace(Globals.smpl, smplLocal);
        }

        private static O.ELoopType LoopType(ASTNode node, int i)
        {
            O.ELoopType loopType = O.ELoopType.ForTo;
            if (node[0][i].Text == "ASTFORTYPE1") loopType = O.ELoopType.ForTo;  //superflous, but just to state it
            else if (node[0][i].Text == "ASTFORTYPE2") loopType = O.ELoopType.List;
            else throw new GekkoException();
            return loopType;
        }

        private static void CheckTypeInFunctionDefProcedureDefForDef(string functionName, string type, string s)
        {
            string x = " (type is " + type + ", def function/procedure '" + functionName + "')";
            if(functionName=="for-loop") x = " (type is " + type + ", for loop definition)";
            if (G.Equal(type, "series"))
            {
                if (G.Chop_HasSigil(s))
                {
                    new Error("Did not expect '" + s[0] + "' on variable " + s + x);
                    //throw new GekkoException();
                }
            }
            else if (G.Equal(type, "val") || G.Equal(type, "date") || G.Equal(type, "string"))
            {
                if (s[0] != Globals.symbolScalar)
                {
                    new Error("Expected '" + Globals.symbolScalar + "' on variable " + s + x);
                    //throw new GekkoException();
                }
            }
            else if (G.Equal(type, "list") || G.Equal(type, "map") || G.Equal(type, "matrix"))
            {
                if (s[0] != Globals.symbolCollection)
                {
                    new Error("Expected '" + Globals.symbolCollection + "' on variable " + s + x);
                    //throw new GekkoException();
                }
            }
        }

        private static string GetFuncArgumentCode(ASTNode node, int i)
        {
            
            string alternative = "null";

            string c = "spml" + ++Globals.counter;

            if (node[i].AlternativeCode != null) alternative = node[i].AlternativeCode.ToString().Replace("" + Globals.smpl + "", c);
            string original = node[i].Code.ToString().Replace("" + Globals.smpl + "", c);
            string result = "new GekkoArg((" + c + ") => " + original + ", " + "(" + c + ") => " + alternative + ")";
            return result;
        }
        
        private static string OperatorHelper(ASTNode node, int i)
        {
            //This helper is just to mark the two places it is used
            if (node != null) return "o" + Num(node);                       
            return "O.AdjustT0(" + Globals.smpl + ", " + i + ")";
            throw new GekkoException();
        }

        private static string LocalCode1(string num, string functionName, string fileName)
        {
            string s = null;            
            if (functionName != null)
            {
                //See also this: #08975389245253
                s = "p.lastFileSentToANTLR = O.LastText(`" + functionName + "`, @`" + fileName + "`); p.SetLastFileSentToANTLR(O.LastText(`" + functionName + "`, @`" + fileName + "`)); p.Deeper();";
            }
            return "Databank local" + num + " = Program.databanks.local;" + G.NL + "Program.databanks.local = new Databank(`" + Globals.Local + "`); LocalGlobal lg" + num + " = Program.databanks.localGlobal; Program.databanks.localGlobal = new LocalGlobal(); " + s + G.NL + "try {" + G.NL;
        }

        private static string LocalCode2(string num, string functionName)
        {
            string s = null;
            string s2 = null;
            if (functionName != null)
            {
                s = "p.RemoveLast();";  //must be inside finally {}, because there may be a return from the method (and also an exception should adjust the counter)
                s2 = "catch { p.Deeper(); throw; }" + G.NL;  //otherwise the following RemoveLast will remove too much for functions/procedures
            }
            return "} " + G.NL + s2 + " finally {" + G.NL + "Program.databanks.local = local" + num + "; Program.databanks.localGlobal = lg" + num + ";" + s + ";" + G.NL + "} " + G.NL;
        }

        private static string LocalCode3(string num)
        {
            return "ESeriesMissing r1_" + num + " = Program.options.series_array_print_missing; ESeriesMissing r2_" + num + " = Program.options.series_array_calc_missing; ESeriesMissing r3_" + num + " = Program.options.series_data_missing; try {" + G.NL + "O.HandleOptionBankRef1(o" + num + ".opt_bank, o" + num + ".opt_ref); O.HandleMissing1(o" + num + ".opt_missing);" + G.NL;
        }        

        private static string LocalCode4(string num)
        {
            //num not used
            return "}" + G.NL + "finally {" + G.NL + "O.HandleOptionBankRef2(); O.HandleMissing2(r1_" + num + ", r2_" + num + ", r3_" + num + ");" + G.NL + "}" + G.NL;
        }

        private static bool ReportHelperIsSum(string internalName, string internalFunction)
        {
            return internalName != null && G.Equal(internalFunction, "sum");
        }

        private static string ReportLabelHelper(ASTNode node)
        {
            if (node.specialExpressionAndLabelInfo == null)
            {
                return null;
            }
            string label = node.specialExpressionAndLabelInfo[1];
            label = label.Replace("\\", "\\\\");  //otherwise something like "#(listfile c:\tmp\m.lst)" will crash
            return label + "|" + node.specialExpressionAndLabelInfo[2] + "|" + node.specialExpressionAndLabelInfo[3];
        }

        private static string GekkoSmplCommandHelper2(int smplCommandNumber)
        {
            string s2 = "" + Globals.smpl + ".command = smplCommandRemember" + smplCommandNumber + ";";
            return s2;
        }

        private static string GekkoSmplCommandHelper1(int smplCommandNumber, string ss)
        {
            string s = "var smplCommandRemember" + smplCommandNumber + " = " + Globals.smpl + ".command; " + Globals.smpl + ".command = GekkoSmplCommand." + ss + ";";
            return s;
        }

        private static string GetSimpleName(ASTNode node)
        {
            string s = null;
            if (node[0][0] == null && node[1][2][0] == null)  //no bank and no freq indicator
            {
                //#746384984 merge these in a method
                if (node[1][1][0].Text == "ASTNAME" && node[1][1][0].ChildrenCount() == 1 && node[1][1][0][0].Text == "ASTIDENT")
                {
                    string sigil = GetSigilAsString(node[1][0]);
                    string ident = node[1][1][0][0][0].Text;
                    s = sigil + ident;
                }
            }

            return s;
        }

        private static Tuple<bool, string> CheckIfLeftSide(ASTNode node)
        {
            //We must detect x in these (will also cover dots):
            //x = 5
            //x[2000] = 5
            //x['a'] = 5
            //x['a'][2000] = 5
            //x['a'][2000] $ (2 == 3) = 5
            //Could be for instance:
            //ASTASSIGNMENT
            //  ASTLEFTSIDE
            //   ASTDOTORINDEXER
            //     ASTDOTORINDEXER
            //       ASTBANKVARNAME -- node is here
            //--> beware that these must be the first children!
            string type = "var"; //same as unknown
            bool isLeftSideVariable = true;
            if (node.Number != 0)
            {
                //is also ok regarding ASTDOLLAR
                isLeftSideVariable = false;
            }
            else
            {
                ASTNode parent = node.Parent;  //cannot be null
                while (true)
                {
                    if (parent == null) break;  //just for ultra safety, will not happen
                    if (parent.Number != 0)
                    {
                        //is also ok regarding ASTDOLLAR
                        isLeftSideVariable = false;
                        break;
                    }
                    if (parent.Text == "ASTDOTORINDEXER")
                    {
                        //ok, keep going
                    }
                    else if (parent.Text == "ASTDOLLAR")
                    {
                        //ok, keep going
                    }
                    else if (parent.Text == "ASTLEFTSIDE")
                    {
                        break;  //then isLeftSideVariable will be true
                    }
                    else
                    {
                        isLeftSideVariable = false;
                        break;
                    }
                    parent = parent.Parent;
                }

                parent = node.Parent;  //cannot be null
                while (true)
                {
                    if (parent == null) break;  //just for ultra safety, will not happen                    
                    if (parent.Text == "ASTASSIGNMENT")
                    {                        
                        type = parent[3].Text;
                        if (G.Equal(type, "VAR_KDUSJFLQO2")) type = "var";
                        break;
                    }
                    parent = parent.Parent;
                }
            }
            type = HandleVar(type);
            return new Tuple<bool, string>(isLeftSideVariable, type);
        }

        private static string HandleVar(string type)
        {
            if (type == "ASTPLACEHOLDER") type = "var";
            string s = type.Substring(0, 1).ToUpper() + type.Substring(1).ToLower();
            if (s == "Ser") s = "Series";
            //if (s == "Series") s = "Var";  //This is done because we otherwise get errors if rhs if for instance a list.
            return s;
        }

        private static void GetCodes(ASTNode node, int i, out string codeStart, out string codeEnd2, out string codeStep)
        {
            codeStart = node[0][i][2][0].Code.ToString();
            codeEnd2 = "null";
            if (node[0][i][3][0] != null) codeEnd2 = node[0][i][3][0].Code.ToString();
            codeStep = "null";
            if (node[0][i][4][0] != null) codeStep = node[0][i][4][0].Code.ToString();
        }

        private static List<string> GetForLoopVariables(ASTNode node)
        {
            List<string> rv = new List<string>();
            for (int i = 0; i < node[0].ChildrenCount(); i++)
            {
                string varname = node[0][i][1][0][1][0].Text;
                if (node[0][i][1][0][0][0] != null)
                {
                    if (node[0][i][1][0][0][0].Text == "ASTPERCENT") varname = Globals.symbolScalar + varname;
                    else if (node[0][i][1][0][0][0].Text == "ASTHASH") varname = Globals.symbolCollection + varname;
                    else throw new GekkoException();
                }

                if (varname.StartsWith(Globals.symbolCollection.ToString()))
                {
                    new Error("At present, only scalar variables (%) are allowed as FOR loop variablse");
                    //throw new GekkoException();
                }

                rv.Add(varname);
            }
            return rv;
        }

        private static string GetSigilAsString(ASTNode nn)
        {
            //returns null if not present
            string sigil = null;
            if (nn[0] != null)
            {
                if (nn[0].Text == "ASTPERCENT") sigil = Globals.symbolScalar.ToString();
                if (nn[0].Text == "ASTHASH") sigil = Globals.symbolCollection.ToString();
            }
            return sigil;
        }

        private static void ListDefHelper(ASTNode node)
        {
            //for instance (1 rep 2, 3, 4) becomes 1, 2, 3, null, 4, null --> always even number
            //this is to keep it speedy for long lists.
            for (int i = 0; i < node.ChildrenCount(); i++)
            {
                ASTNode child = node[i];
                string xx = "null";
                if (child[1] != null)
                {
                    xx = child[1].Code.ToString();
                }
                node.Code.A(child[0].Code + ", " + xx);
                if (i < node.ChildrenCount() - 1) node.Code.A(", ");                
            }
        }        

        private static string[] IsGamsSumFunctionOrUnfoldFunction(ASTNode node, string functionName)
        {
            return IsGamsSumFunctionOrUnfoldFunction(node, functionName, false);
        }

        private static string[] IsGamsSumFunction(ASTNode node, string functionName)
        {
            return IsGamsSumFunctionOrUnfoldFunction(node, functionName, true);
        }

        private static string[] IsGamsSumFunctionOrUnfoldFunction(ASTNode node, string functionName, bool onlySum)
        {
            //asdfg added [0]
            //if (node.Text == "ASTOBJECTFUNCTION" || node.Text == "ASTOBJECTFUNCTION_Q" || node.Text == "ASTOBJECTFUNCTIONNAKED" || node.Text == "ASTOBJECTFUNCTIONNAKED_Q") return null;
            if (node[0].Text == "ASTOBJECTFUNCTION" || node[0].Text == "ASTOBJECTFUNCTION_Q" || node[0].Text == "ASTOBJECTFUNCTIONNAKED" || node[0].Text == "ASTOBJECTFUNCTIONNAKED_Q") return null;
            //returns null if it is NOT a GAMS-like sum() function
            string[] rv = null;
            if (onlySum)
            {
                if (G.Equal(functionName, "sum")) rv = GetListnames(node);
            }
            else
            {
                if (G.Equal(functionName, "sum") || G.Equal(functionName, "unfold")) rv = GetListnames(node);
            }

            if (rv == null)
            {
                //success, do nothing
            }
            else
            {                
                if (G.Equal(functionName, "sum)")) //!!! HMMMM this "sum)" must be a mistake, but maybe the extra check is not necessary anyway
                {
                    //extra check, this may be, for instance, sum(#i, x) or sum(#i, #j) --> these are not GAMS-like sums
                    int[] found = new int[1];
                    WalkASTToCheckSumFunction(node[2], found);
                    if (found[0] == 0) rv = null;  
                }
            }
            
            return rv;
        }

        private static string[] GetListnames(ASTNode node)
        {
            string[] rv = null;
            if (true)
            {
                if (node.ChildrenCount() == 3 + 1)
                {
                    int ii = 2;

                    if (node[ii].Text == "ASTDOLLAR")
                    {
                        rv = GetListNames2(node[ii][0]);
                    }
                    else
                    {                        
                        rv = GetListNames2(node[ii]);
                    }
                }
            }

            return rv;
        }

        private static string[] GetListNames2(ASTNode node)
        {
            string[] rv;
            if (node.Text == "ASTLISTDEF")  //ZXCVB
            {
                //TODO: CHECK types of rv[i], are they all simple #i, #j, ...?
                rv = new string[node.ChildrenCount()];
                for (int i = 0; i < node.ChildrenCount(); i++)
                {
                    rv[i] = GetSimpleHashName(node[i][0]);  //ZXCVB
                }
            }
            else
            {
                rv = new string[1];
                rv[0] = GetSimpleHashName(node);
            }

            return rv;
        }

        
        private static string GetSimpleHashName(ASTNode node)
        {
            string rv = null;
            if (node != null && node.Text == "ASTBANKVARNAME")
            {
                if (node[0][0] == null)
                {
                    if (node[1][0][0] != null && node[1][0][0].Text == "ASTHASH" && node[1][1][0] != null && node[1][1][0].Text == "ASTNAME" && node[1][2][0] == null)
                    {
                        if (node[1][1][0][0].Text == "ASTIDENT")
                        {
                            rv = node[1][1][0][0][0].Text;
                        }
                    }
                }

            }
            return rv;
        }
        
        private static string GetLoopNameCs(ASTNode node, string i)
        {
            return "loop" + Num(node) + "_" + i;
        }

        private static string GetFunctionName(ASTNode node)
        {
            //asdfg added [0]
            //string functionName = node[0][0].Text.ToLower();  //no string composition allowed for functions, it is simple ident.
            string functionName = node[0][0][0].Text.ToLower();  //no string composition allowed for functions, it is simple ident.
            if (functionName == "string") functionName = "tostring";
            return functionName;
        }

        private static string GetLibraryName(ASTNode node)
        {
            string libraryName = null;
            ASTNode n = node[0][1][0]?[0];
            if (n != null) libraryName = n.Text.ToLower();  //no string composition allowed for library, it is simple ident.            
            return libraryName;
        }

        private static string GetFunctionName2(ASTNode node)
        {
            string functionName = node[1][0].Text.ToLower();  //no string composition allowed for functions, it is simple ident.
            if (functionName == "string") functionName = "tostring";
            return functionName;
        }
        
        private static TwoStrings SearchUpwardsInTree2(ASTNode node, string listName)
        {
            //Looks for list loop anchor, for instance looping in sum() or unfold()  -- or the left-hand side controlled lists in SERIES looping (like x[#i] = y[#i] + ...)
            ASTNode tmp = node.Parent;
            TwoStrings rv = null;         
            while (tmp != null)
            {
                bool ok = false;
                if (tmp.listLoopAnchor != null && tmp.listLoopAnchor.ContainsKey(listName)) ok = true;
                if (ok)
                {
                    rv = tmp.listLoopAnchor[listName];
                    break;
                }
                tmp = tmp.Parent;
            }
            return rv;
        }
        
        private static string SearchUpwardsInTree3(ASTNode node, string varName)
        {
            //Looking for function defintion (bound arguments), or for loop vars
            ASTNode tmp = node.Parent;
            ASTNode parent = null;
            string rv = null;
            while (tmp != null)
            {                
                if (tmp.functionDefAnchor != null && tmp.functionDefAnchor.ContainsKey(varName))                
                {
                    parent = tmp;
                    rv = tmp.functionDefAnchor[varName];
                    break;
                }
                else if (tmp.forLoopAnchor != null && tmp.forLoopAnchor.ContainsKey(varName))
                {
                    parent = tmp;
                    rv = tmp.forLoopAnchor[varName];
                    break;
                }
                tmp = tmp.Parent;
            }
            return rv;
        }

        private static string SearchUpwardsInTree4(ASTNode node)
        {
            //finds out if the variable is a LHS (left-side) variable
            //returns null if RHS or (LHS and there is a ASTDOTORINDEXER or ASTDOLLOARabove, the a in x.a, or the a in y $ (a == 1) = 100;
            
            ASTNode tmp = node;            
            string rv = null;
            while (tmp != null)
            {
                if (tmp.ivTempVarName != null)
                {
                    rv = tmp.ivTempVarName;
                    break;
                }

                if (tmp.Parent != null)
                {
                    if (tmp.Parent.Text == "ASTDOTORINDEXER")
                    {
                        return null;  //if any parent is like this, null is returned. We want to find the one highest in the tree.
                    }
                    else if (tmp.Parent.Text == "ASTDOLLAR")
                    {
                        if (tmp.Number == 1)
                        {
                            //In y $ (a == 1) = 100, we catch only the $-RHS of y $ (a == 1), that is the 'a' not the 'y'
                            return null;
                        }
                    }
                }
                tmp = tmp.Parent;
            }
            return rv;
        }

        private static bool SearchUpwardsInTree5(ASTNode node)
        {
            //finds out if node is inside [] or {}
            ASTNode tmp = node;
            string rv = null;
            while (tmp != null)
            {
                if (tmp.Text == "ASTCURLY" || ((tmp.Text == "ASTINDEXER" || tmp.Text == "ASTDOT") && tmp.Parent.Text == "ASTDOTORINDEXER")) return true;
                tmp = tmp.Parent;
            }
            return false;
        }
                
        private static bool SearchUpwardsInTree6(ASTNode node)
        {
            //finds out if node is inside sum() function of type sum(#x, ...)
            ASTNode tmp = node;
            string rv = null;
            while (tmp != null)
            {
                //if(IsGamsSumFunction(node, ))
                if (tmp.Text == "ASTFUNCTION" || tmp.Text == "ASTFUNCTION_Q")
                {
                    string functionName = GetFunctionName(tmp);
                    if (IsGamsSumFunction(tmp, functionName) != null)
                    {
                        //this is a gams-like sum
                        return true;
                    }
                }                 
                tmp = tmp.Parent;
            }
            return false;
        }

        private static ASTNode SearchUpwardsInTree7(ASTNode node)
        {
            //finds highest sum() function of type sum(#x, ...)
            ASTNode tmp = node;
            ASTNode highest = null;            
            while (tmp != null)
            {
                //if(IsGamsSumFunction(node, ))
                if (tmp.Text == "ASTFUNCTION" || tmp.Text == "ASTFUNCTION_Q")
                {
                    string functionName = GetFunctionName(tmp);
                    if (IsGamsSumFunction(tmp, functionName) != null)
                    {
                        //this is a gams-like sum
                        highest = tmp;
                    }
                }
                tmp = tmp.Parent;
            }
            return highest;
        }

        private static string SearchUpwardsInTree8(ASTNode node)
        {
            //Looking for function defintion (return value, not arguments)
            ASTNode tmp = node.Parent;
            ASTNode parent = null;
            string rv = null;
            while (tmp != null)
            {
                if (tmp.functionType != null)
                {
                    parent = tmp;
                    rv = tmp.functionType;
                    break;
                }                
                tmp = tmp.Parent;
            }
            return rv;
        }

        private static bool SearchUpwardsInTree9(ASTNode node)
        {
            //finds out if node has ASTMAPITEM above
            ASTNode tmp = node;
            string rv = null;
            while (tmp != null)
            {
                
                if (tmp.Text == "ASTMAPITEM") return true;
                tmp = tmp.Parent;
            }
            return false;
        }        

        private static StringBuilder GetHeaderCs(W w)
        {
            StringBuilder destination = null;            
            destination = w.headerCs;
            return destination;
        }        
        
        private static string Num(ASTNode node)
        {
            return "" + node.commandLinesCounter;
        }
        
        private static void GetCodeFromAllChildren(ASTNode node)
        {
            if (node == null)
            {
                return;
            }
            foreach (ASTNode child in node.ChildrenIterator())
            {
                node.Code.A(child.Code + G.NL);
            }
        }

        private static void GetCodeFromAllChildren(ASTNode receiver, ASTNode node)
        {
            if (node == null) return;
            foreach (ASTNode child in node.ChildrenIterator())
            {
                receiver.Code.A(child.Code + G.NL);
            }
        }

        private static void GetCodeFromAllChildren(GekkoSB receiver, ASTNode node)
        {
            if (node == null) return;
            foreach (ASTNode child in node.ChildrenIterator())
            {
                receiver.A(child.Code + G.NL);
            }
        }        
        
        private static string HandleNegate(ASTNode node)
        {
            //This is for speedup purposes, to avoid a Negate() function on primitives taking up time.
            //So we get ScalarVal(-2) instead of Negate(ScalarVal(2)).
            string minus = "";
            if (node.Parent != null)
            {
                if (node.Parent.Text == "NEGATE" || node.Parent.Text == "ASTINTEGERNEGATIVE" || node.Parent.Text == "ASTDOUBLENEGATIVE")
                {
                    //For parent NEGATE this parent will be ignored
                    minus = "-";
                    node.Parent.IgnoreNegate = true;
                }
                
            }
            return minus;
        }
        
        private static string AddOperator(string type, string s, string parentType, ASTNode node)
        {
            string o = "o" + Num(node) + ".operators"; 
            if (parentType == "ASTPRTELEMENTOPTIONFIELD") o = "ope" + Num(node) + ".operators";
            return o + ".Add(new OptString(`" + type + "`, O.ConvertToString(" + s + ")));" + G.NL;
        }        
    }

    public class W
    {         
        //W is created when running a .cmd/.gcm file
        public WalkHelper wh = null;  //is created when encountering a Gekko command (like SERIES, PRT, etc.)                
        public string fileNameContainingParsedCode = null;
        public int commandLinesCounter = -1;
        public int expressionCounter = -1;
        public StringBuilder headerCs = new StringBuilder(); //stuff to be put at the very start.                        
        public string libraryName = null;
    }

    /// <summary>
    /// Created for each new command (except IF, FOR, etc -- hmm is this true now?)
    /// </summary>
    public class WalkHelper
    {
        public enum seriesType
        {
            None,
            SeriesLhs,
            SeriesRhs
        }

        /// <summary>
        /// This is for GAMS-like sum functions, contains C# code, used in Gekko 3.0.
        /// </summary>
        public GekkoStringBuilder localFuncsCode = null;
                  
        public seriesType seriesHelper = seriesType.None;

        public string currentCommand = null;
        public bool isGotoOrTarget = false;             
    }
}
