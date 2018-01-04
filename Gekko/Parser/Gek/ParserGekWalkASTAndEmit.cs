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

        //public void SetNull()
        //{
        //    this.storage = null;
        //}

            

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

    //public class GekkoStringBuilder
    //{
    //    public StringBuilder sb = null;
    //    public GekkoStringBuilder()
    //    {
    //        this.sb = new StringBuilder();
    //    }
        
    //    public void __(GekkoStringBuilder sb2)
    //    {
    //        this.sb.Append(sb2);
    //    }

    //    public void __(string s)
    //    {
    //        this.sb.Append(s);
    //    }

    //    public void __Line(GekkoStringBuilder sb2)
    //    {
    //        this.sb.Append(sb2);
    //        this.sb.AppendLine();
    //    }

    //    public void __Line(string s)
    //    {
    //        this.sb.AppendLine(s);
    //    }

    //    public void __Clear(GekkoStringBuilder sb2)
    //    {
    //        this.sb.Clear();
    //        this.sb.Append(sb2);
    //    }
        
    //    public void __Clear(string s)
    //    {
    //        this.sb.Clear();
    //        this.sb.Append(s);
    //    }
    //}
    
    public enum EOptionType
    {
        String,
        Val,
        Date
    }

    public class ParserGekWalkASTAndEmit
    {

        public static readonly GekkoTime tNULL = new GekkoTime(EFreq.Annual, -12345, 1);



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
                G.Writeln2("*** ERROR: #8937524309");
                throw new GekkoException();
            }
            return node[0].Text;
        }

        public static void FindFunctionsUsedInGekkoCode(ASTNode node, Dictionary<string, int> functions)
        {
            if (node.Text == "ASTFUNCTION")
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

        public static void WalkASTAndEmitUnfold(ASTNode node)
        {
            //before subnodes
            //return;

            if (node.Text == "ASTBANKVARNAME")
            {
                string s = GetSimpleName(node);
                
                if (s != null && s[0] == Globals.symbolCollection)
                {
                    string listnameWithoutSigil = s.Substring(1);
                    //naked #m
                    if (node.Parent.Text == "ASTINDEXERELEMENT" || node.Parent.Text == "ASTCURLY" || (node.Parent.Text == "ASTCOMPARE2" && node.Number==1))
                    {
                        //#m is inside a x[#m], or inside a x{#m} or is a #m1[#m] conditional
                        ASTNode node2 = node.Parent.Parent;
                        //Assign it to ASTPRTELEMENT, unless it is assigned to sum(#m,...) or unfold(#m,...)
                        while (true)
                        {
                            if (node2 == null || node2.Text == null) break;
                            if (node2.Text == "ASTFUNCTION" && (G.Equal(node2[0][0].Text, "sum") || G.Equal(node2[0][0].Text, "unfold")))
                            {
                                if (node2[1].Text == "ASTBANKVARNAME")
                                {
                                    string s2 = GetSimpleName(node2[1]);
                                    if (G.Equal(s, s2))
                                    {
                                        //node2 = node2.Parent;
                                        goto Label;
                                    }
                                }
                                else if (node2[1].Text == "ASTLISTDEF")
                                {
                                    foreach (ASTNode node3 in node2[1].ChildrenIterator())
                                    {
                                        string s2 = GetSimpleName(node3);
                                        if (G.Equal(s, s2))
                                        {
                                            //node2 = node2.Parent;
                                            goto Label;
                                        }
                                    }
                                }
                            }
                            if (node2.Text == "ASTPRTELEMENT")
                            {
                                if (node2.freeIndexedLists == null) node2.freeIndexedLists = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                                if (!node2.freeIndexedLists.ContainsKey(listnameWithoutSigil)) node2.freeIndexedLists.Add(listnameWithoutSigil, null);
                            }
                            node2 = node2.Parent;
                        }
                        Label: node2 = node2;
                    }
                }
            }
            foreach (ASTNode child in node.ChildrenIterator())
            {
                WalkASTAndEmitUnfold(child);
            }
            //after subnodes

            if (node.Text == "ASTPRTELEMENT")
            {
                if (node[0].Text == "ASTEXPRESSION")
                {
                    if (node.freeIndexedLists != null && node.freeIndexedLists.Count > 0)
                    {
                        List<string> xx = new List<string>();
                        foreach (string ss in node.freeIndexedLists.Keys)
                        {
                            xx.Add(ss);
                        }
                        xx.Sort(StringComparer.OrdinalIgnoreCase);

                        //here: onfolding 1 list (#m)

                        //    ASTPRTELEMENT
                        // 0    ASTEXPRESSION
                        // 1      ASTFUNCTION    <----------- our insert begins here
                        // 2        ASTIDENT  
                        // 3          unfold
                        // 4        ASTBANKVARNAME         <-- if there are > 1 lists, an LISTDEF node is inserted here, and the ASTBANKVARNAME nodes are subnodes
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
                        ASTNode n2 = new ASTNode("ASTIDENT", true);
                        ASTNode n3 = new ASTNode("unfold", true);

                        n0.Add(n1);
                        n1.Add(n2);
                        n2.Add(n3);

                        ASTNode list = new ASTNode("ASTLISTDEF", true);

                        for (int i = 0; i < xx.Count; i++)
                        {
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
                            n4.Add(n5);
                            n4.Add(n6);
                            n6.Add(n7);
                            n7.Add(n8);
                            n6.Add(n9);
                            n9.Add(n10);
                            n10.Add(n11);
                            n11.Add(n12);
                            n6.Add(n13);
                            if (xx.Count==1)
                            {
                                n1.Add(n4);
                            }
                            else
                            {
                                list.Add(n4);
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

        public static void WalkASTAndEmit(ASTNode node, int absoluteDepth, int relativeDepth, string textInput, W w, P p)
        {

            //if (node != null) G.Writeln(G.Blanks(absoluteDepth) + node.Text);

            if (node.Parent != null)
            {
                string s = null;
                if (node.Text == "ASTTUPLEITEM")
                {
                    s = "_tuple_" + node.Number;  //a (VAL x, GENR y) = ... tuple is kind of like two separate commands.
                }
                node.commandLinesCounter = node.Parent.commandLinesCounter + s;  //default, may be overridden if new command is encountered.               
            }
            
            //See also #890752345
            if (node.Text == "ASTIFSTATEMENTS" || node.Text == "ASTELSESTATEMENTS" || node.Text == "ASTFORSTATEMENTS" || node.Text == "ASTFUNCTIONDEFCODE")
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

            //#9874352093573
            if (node.Text == "ASTGENR" || node.Text == "ASTSERIES" || node.Text == "ASTGENRLHSFUNCTION" || node.Text == "ASTPRTELEMENT" || node.Text == "ASTOLSELEMENT" || node.Text == "ASTTABLESETVALUESELEMENT")
            {
                //This local cache is only used for commands that do implicit timeseries looping with expressions
                //For instance PRT fX%i (PRT fXnz would end in global cache), where we do not have to 
                //  find fx%i for each period in the time loop (the reference fx%i is always fixed over that loop
                //  which is internal/implicit inside the GENR statement).
                ClearLocalStatementCache(w);
            }

            //Before sub-nodes
            switch (node.Text)
            {
                case "ASTFUNCTIONDEF":
                    {
                        if (w.uFunctionsHelper != null)
                        {
                            G.Writeln2("*** ERROR: Function definition inside function definition not allowed");
                            throw new GekkoException();
                        }
                        if (absoluteDepth > 1)
                        {
                            G.Writeln2("*** ERROR: Function definition cannot be inside loop, IF-statement etc. Place at top or end of file.");
                            throw new GekkoException();
                        }
                        w.uFunctionsHelper = new FunctionArgumentsHelper();
                    }
                    break;
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
                            node.forLoopAnchor.Add(varname, "forloop_" + ++Globals.counter);
                            if (node.forLoop == null) node.forLoop = new List<Tuple<string, string>>();
                            node.forLoop.Add(new Tuple<string, string>("IVariable", "forloop_" + Globals.counter));
                        }
                    }
                    break;
                case "ASTFUNCTIONDEF2":
                    {
                        string returnType = node[0].Text;
                        string functionName = GetFunctionName2(node);  //node[1]
                        foreach (ASTNode child in node[2].ChildrenIterator())
                        {
                            string type = child[0].Text;
                            string sigil = null;
                            if (child[1][0][0] != null) sigil = child[1][0][0].Text;
                            string name = child[1][1][0].Text;
                            string s = null;
                            if (sigil == "ASTPERCENT") s += Globals.symbolScalar;
                            else if (sigil == "ASTHASH") s += Globals.symbolCollection;
                            
                            s += name;

                            s = G.AddSigil(s, type);  //see also #980753275

                            if (node.functionDefAnchor == null) node.functionDefAnchor = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            if (node.functionDefAnchor.ContainsKey(s))
                            {
                                G.Writeln2("*** ERROR: The variable name " + s + " is used several times in a FUNCTION definition");
                                throw new GekkoException();
                            }
                            node.functionDefAnchor.Add(s, "functionarg_" + ++Globals.counter);
                            if (node.functionDef == null) node.functionDef = new List<Tuple<string, string>>();
                            node.functionDef.Add(new Tuple<string, string>("IVariable", "functionarg_" + Globals.counter));
                        }

                    }
                    break;
                case "ASTFUNCTION":  //kind of like ASTFUNCTIONDEF, but the difference is that these sum() functions may be nested, so the nodes themselves need to keep the anchor info
                    {
                       

                        string functionName = GetFunctionName(node);
                        string[] listNames = IsGamsSumFunctionOrUnfoldFunction(node, functionName);
                        if (listNames != null)
                        {
                            if (node.listLoopAnchor == null) node.listLoopAnchor = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            foreach (string s in listNames)
                            {
                                if (node.listLoopAnchor.ContainsKey(s))
                                {
                                    G.Writeln2("*** ERROR: The list " + Globals.symbolCollection + s + " is used several times for multidimensional looping in sum() or unfold() function");
                                    throw new GekkoException();
                                }
                                node.listLoopAnchor.Add(s, "listloop_" + s + ++Globals.counter);
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
            
            foreach (ASTNode child in node.ChildrenIterator())
            {                   
                WalkASTAndEmit(child, absoluteDepth + 1, relativeDepth + 1, textInput, w, p);
                //return; Globals.testing = true;
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
            else
            {
                //After sub-nodes                

                switch (node.Text)
                {
                    case "+":
                        {
                            node.Code.CA("O.Add(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;                    
                    case "-":
                        {
                            node.Code.CA("O.Subtract(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "*":
                        {
                            node.Code.CA("O.Multiply(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "/":
                        {
                            node.Code.CA("O.Divide(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "ASTPOW":
                        {
                            node.Code.CA("O.Power(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "&+":
                        {
                            node.Code.CA("O.AndAdd(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "&-":
                        {
                            node.Code.CA("O.AndSubtract(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "&*":
                        {
                            node.Code.CA("O.AndMultiply(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "ASTPLUS":
                        {
                            node.Code.CA("O.Add(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "ASTMINUS":
                        {
                            node.Code.CA("O.Subtract(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "ASTSTAR":
                        {
                            node.Code.CA("O.Multiply(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "ASTDIV":
                        {
                            node.Code.CA("O.Divide(smpl, " + node[0].Code + ", " + node[1].Code + ")");
                        }
                        break;
                    case "ASTPOWER":
                        {
                            node.Code.CA("O.Power(smpl, " + node[0].Code + ", " + node[1].Code + ")");
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
                            node.Code.CA("new ScalarString(`@`)");
                        }
                        break;                    
                    case "ASTUPDDATACOMPLICATED":
                        {
                            
                            GetCodeFromAllChildren(node);
                        }
                        break;
                    case "ASTS":                    
                        {
                            node.Code.A(AddPrintCode( Globals.printCode_r, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;                    
                    case "ASTSN":  //rn
                        {
                            node.Code.A(AddPrintCode(Globals.printCode_rn, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTBANK":
                        {                            
                            if (node.ChildrenCount() == 0)
                            {
                                node.Code.CA("new ScalarString(`" + Globals.firstCheatString + "`)");
                            }
                            else node.Code.CA(node[0].Code.ToString());
                        }
                        break;
                    case "ASTSD":
                        {
                            node.Code.A(AddPrintCode(Globals.printCode_rd, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTSDP":
                        {
                            node.Code.A(AddPrintCode(Globals.printCode_rdp, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTSP":
                        {
                            node.Code.A(AddPrintCode(Globals.printCode_rp, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTPRTROWS":
                        {
                            GetCodeFromAllChildren(node);
                        }
                        break;                    
                    case "ASTCLEAR":
                        {
                            node.Code.A("O.Clear o" + Num(node) + " = new O.Clear();" + G.NL);
                            node.Code.A("o" + Num(node) + ".p = p;");
                            if (node[0].ChildrenCount() > 0)
                            {
                                node.Code.A("o" + Num(node) + ".name = `" + node[0][0].Text + "`;" + G.NL);
                            }
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTLOCK":
                        {
                            node.Code.A("O.Lock o" + Num(node) + " = new O.Lock();" + G.NL);
                            //node.Code.A("o" + Num(node) + ".p = p;");
                            node.Code.A("o" + Num(node) + ".bank = `" + node[0].Text + "`;" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTUNLOCK":
                        {
                            node.Code.A("O.Unlock o" + Num(node) + " = new O.Unlock();" + G.NL);
                            //node.Code.A("o" + Num(node) + ".p = p;");
                            node.Code.A("o" + Num(node) + ".bank = `" + node[0].Text + "`;" + G.NL);
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
                            node.Code.A("o" + Num(node) + ".name = `" + node[0].Text + "`;" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTRESET":
                        {
                            node.Code.A("O.Reset o" + Num(node) + " = new O.Reset();" + G.NL);
                            node.Code.A("o" + Num(node) + ".p = p;");
                            node.Code.A("o" + Num(node) + ".Exe(smpl);" + G.NL);
                        }
                        break;
                    case "ASTRESTART":
                        {
                            node.Code.A("O.Restart o" + Num(node) + " = new O.Restart();" + G.NL);
                            node.Code.A("o" + Num(node) + ".p = p;");
                            node.Code.A("o" + Num(node) + ".Exe(smpl);" + G.NL);
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
                            node.Code.A("Program.Cls(``);"); //main window
                            node.Code.A("Program.Cls(`output`);");  //output window
                        }
                        break;
                    case "ASTCOLLAPSE":
                        {
                            node.Code.A("O.Collapse o" + Num(node) + " = new O.Collapse();" + G.NL);
                            string type = "null";
                            if (node.ChildrenCount() >= 3) type = "`" + node[2].Text + "`";
                            string b1 = node[0][0].Code.ToString(); if (b1 == null) b1 = "``";
                            string b0 = node[1][0].Code.ToString(); if (b0 == null) b0 = "``";

                            node.Code.A("o" + Num(node) + ".b1 = O.ConvertToString(" + b1 + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".b0 = O.ConvertToString(" + b0 + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".v1 =  O.ConvertToString(" + node[0][1].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".v0 =  O.ConvertToString(" + node[1][1].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".type = " + type + ";" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);                            
                        }
                        break;
                    case "ASTINTERPOLATE":
                        {
                            node.Code.A("O.Interpolate o" + Num(node) + " = new O.Interpolate();" + G.NL);
                            string type = "null";
                            if (node.ChildrenCount() >= 3) type = "`" + node[2].Text + "`";
                            string b1 = node[0][0].Code.ToString(); if (b1 == null) b1 = "``";
                            string b0 = node[1][0].Code.ToString(); if (b0 == null) b0 = "``";

                            node.Code.A("o" + Num(node) + ".b1 = O.ConvertToString(" + b1 + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".b0 = O.ConvertToString(" + b0 + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".v1 =  O.ConvertToString(" + node[0][1].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".v0 =  O.ConvertToString(" + node[1][1].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".type = " + type + ";" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTDOC":
                        {
                            node.Code.A("O.Doc o" + Num(node) + " = new O.Doc();" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTANALYZE":
                        {
                            node.Code.A("O.Analyze o" + Num(node) + " = new O.Analyze();" + G.NL);
                            node.Code.A("o" + Num(node) + ".x = " + node[0].Code).End();
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTOLS":
                        {
                            node.Code.A("O.Ols o" + Num(node) + " = new O.Ols();" + G.NL);
                            GetCodeFromAllChildren(node, node[0]);
                            if(!node[1].Code.IsNull()) node.Code.A("o" + Num(node) + ".name = " + node[1].Code).End();
                            if (!node[2].Code.IsNull()) node.Code.A("o" + Num(node) + ".impose = " + node[2].Code).End();
                            node.Code.A("o" + Num(node) + ".lhs = " + node[3].Code).End();
                            node.Code.A("o" + Num(node) + ".rhs = " + node[4].Code).End();
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
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTDELETE":
                        {
                            node.Code.A("O.Delete o" + Num(node) + " = new O.Delete();" + G.NL);
                            GetCodeFromAllChildren(node);
                            //node.Code.A(node[0].Code);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                            break;
                        }
                    case "ASTDELETEALL":
                        {
                            node.Code.A("O.Delete o" + Num(node) + " = new O.Delete();" + G.NL);
                            node.Code.A("o" + Num(node) + ".all = true;" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                            break;
                        }
                    case "ASTCREATE":
                        {
                            node.Code.A("O.Create o" + Num(node) + " = new O.Create();" + G.NL);
                            node.Code.A(node[0].Code);
                            node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                            break;
                        }
                    case "ASTCREATEEXPRESSION":
                        {
                            node.Code.A("O.CreateExpression o" + Num(node) + " = new O.CreateExpression();" + G.NL);
                            node.Code.A("o" + Num(node) + ".lhs = " + node[0].Code + ";" + G.NL);
                            node.Code.A("o" + Num(node) + ".rhs = " + node[1].Code + ";" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                            break;
                        }
                    case "ASTENDO":
                        {
                            node.Code.A("O.Endo o" + Num(node) + " = new O.Endo();" + G.NL);
                            if (node.ChildrenCount() > 0) node.Code.A(node[0].Code);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTENDOQUESTION":
                        {
                            node.Code.A("O.Endo o" + Num(node) + " = new O.Endo();" + G.NL);
                            node.Code.A("o" + Num(node) + ".question = true;" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTEXO":
                        {
                            node.Code.A("O.Exo o" + Num(node) + " = new O.Exo();" + G.NL);
                            if (node.ChildrenCount() > 0) node.Code.A(node[0].Code);
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
                    case "ASTEXIT":  // <command>
                        {
                            node.Code.A("Program.Exit();" + G.NL);
                            //#9807235423 return problem
                            //node.Code.A("return true;" + G.NL;  //probably superfluous
                            node.Code.A("return;" + G.NL);  //probably superfluous
                        }
                        break;

                    case "ASTSTOP":
                        {
                            node.Code.A("Program.Stop(p);" + G.NL);
                            //#9807235423 return problem
                            //node.Code.A("return true;" + G.NL;  //probably superfluous
                            node.Code.A("return;" + G.NL);  //probably superfluous
                        }
                        break;

                    case "ASTREPLACE":
                        {

                        }
                        break;
                    case "ASTFINDMISSINGDATA": // <command> <period>
                        {
                            node.Code.A("O.Findmissingdata o" + Num(node) + " = new O.Findmissingdata();" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);                            
                        }
                        break;
                    case "ASTHDG":
                        node.Code.A("Program.Hdg(O.ConvertToString(" + node[0].Code + "));");
                        break;
                    case "ASTTELL":
                        {
                            string s = "false";
                            if (node.ChildrenCount() > 1)
                            {
                                s = "true";
                            }
                            node.Code.A("Program.Tell(O.ConvertToString(" + node[0].Code + "), " + s + ");");                            
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
                            node.Code.A("Program.Help(" + code + ");" + G.NL);
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
                                node.Code.A(node[0].Code);
                            }
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                            break;
                        }
                    case "ASTCURLY":
                        {                           
                            
                            string listName = GetSimpleHashName(node[0]);
                            string internalName = null;
                            if (listName != null)
                            {
                                internalName = SearchUpwardsInTree2(node, listName);                                
                            }

                            if (internalName != null)
                            {
                                node.Code.CA(internalName);
                            }
                            else
                            {
                                node.Code.CA(node[0].Code);
                            }
                            
                        }
                        break;
                    //case "ASTCURLYSIMPLE":
                    //    {                            
                    //        //node[0].Code = null;  //clearing this
                    //        node[0].Code = new GekkoSB();  //has a null inside as storage
                    //        HandleScalar(node[0], true, w);
                    //        node.Code.CA(node[0].Code);                         
                    //    }
                    //    break;
                    case "ASTEXPRESSIONNEW":
                        {

                        }
                        break;
                    case "ASTEXPRESSION":
                        {
                            node.Code.CA(node[0].Code);
                        }
                        break;
                    case "ASTD":
                        {
                            node.Code.A(AddPrintCode("d", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTDP":
                        {
                            node.Code.A(AddPrintCode("dp", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTDATE":
                        {
                            if (node[0].Text == "?")
                            {
                                if (node.ChildrenCount() > 1)
                                {
                                    node.Code.A("O.Date.Q(`" + node[1].Text + "`);" + G.NL);
                                }
                                else
                                {
                                    node.Code.A("Program.Mem(`date`);" + G.NL);
                                }
                            }
                            else
                            {
                                string nodeCode = HandleDate(node, node[1].Code.ToString());
                                node.Code.A(nodeCode);
                            }
                        }
                        break;
                    case "ASTDATE2":
                        {
                            //TODO:
                            //TODO:
                            //TODO: FromStringToDate() depends upon current freq -- fix that!
                            //TODO:
                            //TODO:
                            node.Code.A("new ScalarDate(G.FromStringToDate(`" + node[0].Text + "`))");
                        }
                        break;
                    case "ASTDATES":
                        {
                            //what about G.GetStart/EndDate()??
                            //We use  O.GetDateChoices.FlexibleStart and  O.GetDateChoices.FlexibleEnd
                            //These can transform an integer into a quarterly or monthly frequency.
                            //This is only legal/possible for two such dates (from/to).
                            string ss1 = node.GetChildCode(0).ToString();
                            string s1 = null;
                            if (ss1 == null) s1 = "Globals.globalPeriodStart";
                            else s1 = "O.ConvertToDate(" + ss1 + ", O.GetDateChoices.FlexibleStart);" + G.NL;
                            string ss2 = node.GetChildCode(1).ToString();
                            string s2 = null;
                            if (ss2 == null) s2 = "Globals.globalPeriodEnd";
                            else s2 = "O.ConvertToDate(" + ss2 + ", O.GetDateChoices.FlexibleEnd);" + G.NL;

                            string xx = node?.Parent?.Parent?.Parent?.Text;
                            if (xx == "ASTASSIGNMENT")  //these do not have an o object
                            {
                                node.Code.A("smpl.t1 = ").A(s1).A(";").A(G.NL);
                                node.Code.A("smpl.t2 = ").A(s2).A(";").A(G.NL);
                            }
                            else
                            {
                                node.Code.A("o").A(Num(node)).A(".t1 = ").A(s1).A(";").A(G.NL);
                                node.Code.A("o").A(Num(node)).A(".t2 = ").A(s2).A(";").A(G.NL);
                            }
                        }
                        break;
                    case "ASTDOTINDEXER":
                        {

                        }
                        break;
                    case "ASTDOUBLENEGATIVE":
                        {
                            node.Code.A(node[0].Code);
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
                            node.Code.A("((");
                            node.Code.A(node[0].Code);
                            node.Code.A(") || (");
                            node.Code.A(node[1].Code);
                            node.Code.A("))");
                        }
                        break;                    
                    case "ASTAND":                    
                        {
                            node.Code.A("((");
                            node.Code.A(node[0].Code);
                            node.Code.A(") && (");
                            node.Code.A(node[1].Code);
                            node.Code.A("))");
                        }
                        break;                    
                    case "ASTNOT":
                        {
                            node.Code.A("!(");
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
                            if (listName != null)
                            {
                                internalName = SearchUpwardsInTree2(node, listName);
                            }

                            if (internalName != null)
                            {
                                indexes += internalName;
                            }
                            else
                            {
                                indexes += node[1].Code.ToString();
                            }

                            node.Code.A("O.ListContains(" + node[0].Code + "," + indexes + ")");
                        }
                        break;
                    case "ASTCOMPARE":
                        {
                            string op = node[0][0].Text;
                            string code1 = node[1].Code.ToString();
                            string code2 = node[2].Code.ToString();
                            if (op == "ASTIFOPERATOR4")  //"<"
                            {                                
                                node.Code.A("O.StrictlySmallerThan(smpl, " + code1 + "," + code2 + ")");
                            }
                            else if (op == "ASTIFOPERATOR6")  //"<="
                            {
                                node.Code.A("O.SmallerThanOrEqual(smpl, " + code1 + "," + code2 + ")");
                            }
                            else if (op == "ASTIFOPERATOR1") //"=="
                            {
                                node.Code.A("O.Equals(smpl, " + code1 + "," + code2 + ")");
                            }
                            else if (op == "ASTIFOPERATOR5")  //">="
                            {
                                node.Code.A("O.LargerThanOrEqual(smpl, " + code1 + "," + code2 + ")");
                            }
                            else if (op == "ASTIFOPERATOR3") //">"
                            {
                                node.Code.A("O.StrictlyLargerThan(smpl, " + code1 + "," + code2 + ")");
                            }
                            else if (op == "ASTIFOPERATOR2") //"<>"
                            {
                                node.Code.A("!O.Equals(smpl, " + code1 + "," + code2 + ")");
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
                                    G.Writeln2("*** ERROR: Internal error related to $ on left-hand side");
                                    throw new GekkoException();
                                }                              

                            }
                            else
                            {
                                //right-hand side, much easier
                                node.Code.A("O.Dollar(smpl, " + node[0].Code + ", " + node[1].Code + ")");
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
                    case "ASTFORSTATEMENTS":
                        {
                            GetCodeFromAllChildren(node);
                            //AddSplitMarkers(node);                        
                        }
                        break;
                    case "ASTIFSTATEMENTS":
                        {
                            GetCodeFromAllChildren(node[0]);
                            GetCodeFromAllChildren(node);
                        }
                        break;
                    case "ASTELSESTATEMENTS":
                        {
                            GetCodeFromAllChildren(node[0]);
                            GetCodeFromAllChildren(node);
                        }
                        break;
                    //case "ASTFUNCTIONDEFCODE":
                    //    {
                    //        GetCodeFromAllChildren(node);
                    //        //AddSplitMarkers(node);                        
                    //    }
                    //    break;

                    // ================= INDENTATION CODE START ==================
                    // ================= INDENTATION CODE START ==================
                    // ================= INDENTATION CODE START ==================

                    case "ASTRETURNTUPLE":  //see ASTRETURN
                        {
                            node.Code.A(Globals.splitSTOP);                            
                            
                            if (w.uFunctionsHelper == null)
                            {
                                if (!node[0].Code.IsNull())
                                {
                                    G.Writeln2("*** ERROR: RETURN with multiple values only allowed inside FUNCTION");
                                    throw new GekkoException();
                                }
                            }
                            else
                            {
                                if (w.uFunctionsHelper.lhsTypes.Count != node.ChildrenCount())
                                {
                                    G.Writeln2("*** ERROR: Return with " + node.ChildrenCount() + " items instead of " + w.uFunctionsHelper.lhsTypes.Count);
                                    throw new GekkoException();
                                }

                                string tempCs = "temp" + ++Globals.counter;
                                string classCs = G.GetVariableType(w.uFunctionsHelper.lhsTypes.Count);

                                string s = null;
                                string s2 = null;
                                int counter = -1;
                                foreach (ASTNode child in node.ChildrenIterator())
                                {
                                    counter++;
                                    string tn = "temp" + ++Globals.counter;

                                    if (w.uFunctionsHelper.lhsTypes[counter] == "series")
                                    {
                                        string tempName = "temp" + ++Globals.counter;
                                        s += "Series " + tempName + " = new Series(Program.options.freq, null);" + G.NL;
                                        s += "foreach (GekkoTime t2 in new GekkoTimeIterator(Globals.globalPeriodStart, Globals.globalPeriodEnd))" + G.NL;
                                        s += GekkoTimeIteratorStartCode(w, node);  //node is where this text is put below
                                        s += "    double data = O.ConvertToVal(" + child.Code + ", t);" + G.NL;  //uuu
                                        s += "    " + tempName + ".SetData(t, data);" + G.NL;                                        
                                        s += GekkoTimeIteratorEndCode();
                                        s += "IVariable " + tn + " = new MetaTimeSeries(" + tempName + ");" + G.NL;
                                    }
                                    else
                                    {
                                        s += "IVariable " + tn + " = " + child.Code + ";" + G.NL;
                                    }
                                    s2 += tn + ", ";
                                }
                                if (s2.EndsWith(", ")) s2 = s2.Substring(0, s2.Length - 2);

                                node.Code.A(s + classCs + " " + tempCs + " = new " + classCs + "(" + s2 + ");" + G.NL);

                                if (node.ChildrenCount() != w.uFunctionsHelper.lhsTypes.Count)
                                {
                                    G.Writeln2("*** ERROR: RETURN with " + node.ChildrenCount() + " values encountered in '" + w.uFunctionsHelper.functionName + "' function");
                                    throw new GekkoException();
                                }

                                node.Code.A("return " + tempCs + ";" + G.NL);
                            }

                            node.Code.A(Globals.splitSTART);
                        }
                        break;

                    case "ASTRETURN":
                        {
                            node.Code.A(Globals.splitSTOP);

                            if (node.ChildrenCount() > 0)
                            {
                                node.Code.A("return " + node[0].Code + ";" + G.NL);
                            }
                            else
                            {
                                //#9807235423 return problem, should it be return true?? C1(), C2(), ...
                                node.Code.A("return;" + G.NL);  //probably the node[0].Code is always empty here (should be)
                            }

                            node.Code.A(Globals.splitSTART);
                        }
                        break;
                    case "ASTGOTO":
                        {
                            node.Code.A(Globals.splitSTOP);
                            node.Code.A("goto " + GetStringFromIdent(node[0]).ToLower().Trim() + ";" + G.NL);  //calls a C# label
                            w.wh.isGotoOrTarget = true;
                            node.Code.A(Globals.splitSTART);
                        }
                        break;
                    case "ASTTARGET":  //AREMOS: target
                        {
                            node.Code.A(Globals.splitSTOP);                            
                            node.Code.A(GetStringFromIdent(node[0]).ToLower().Trim() + ":;" + G.NL);  //a C# label
                            w.wh.isGotoOrTarget = true;
                            node.Code.A(Globals.splitSTART);
                        }
                        break;

                    case "ASTFOR":
                        {
                            node.Code.A(Globals.splitSTOP);

                            GetCodeFromAllChildren(node[1]);

                            List<string> varnames = GetForLoopVariables(node);

                            if (node[0].ChildrenCount() == 1)
                            {
                                int i = 0;
                                string codeStart, codeEnd2, codeStep;
                                GetCodes(node, i, out codeStart, out codeEnd2, out codeStep);
                                string temp = "counter" + ++Globals.counter;
                                node.Code.A(node.forLoop[i].Item1 + " " + node.forLoop[i].Item2 + " = null").End();
                                node.Code.A("int " + temp + " = 0").End();
                                node.Code.A("for (O.IterateStart(ref " + node.forLoop[i].Item2 + ", " + codeStart + "); O.IterateContinue(" + node.forLoop[i].Item2 + ", " + codeStart + ", " + codeEnd2 + ", " + codeStep + ", ref " + temp + "); O.IterateStep(" + node.forLoop[i].Item2 + ", " + codeStart + ", " + codeStep + ", " + temp + "))" + G.NL);
                                node.Code.A("{").End();                                
                                node.Code.A(node[1].Code);
                                node.Code.A("}").End();
                            }
                            else
                            {
                                string listsname = "lists" + ++Globals.counter;
                                node.Code.A("List<List<IVariable>> " + listsname + " = new List<List<IVariable>>()").End();
                                for (int i = 0; i < node[0].ChildrenCount(); i++)
                                {
                                    string codeStart, codeEnd2, codeStep;
                                    GetCodes(node, i, out codeStart, out codeEnd2, out codeStep);
                                    if (codeEnd2 != "null" || codeStep != "null")
                                    {
                                        G.Writeln2("*** ERROR: You cannot use TO or STEP/BY in a parallel loop");
                                        throw new GekkoException();
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


                            node.Code.A(Globals.splitSTART);
                        }
                        break;


                    case "ASTFORVAL":
                        {
                            node.Code.A(Globals.splitSTOP);
                            
                            if (node[0].Text != "ASTFORLEFTSIDE" || node[1].Text != "ASTFORRIGHTSIDE" || node[2].Text != "ASTFORSTATEMENTS")
                            {
                                throw new GekkoException();
                            }
                            string nameSimpleIdent = node[0][0].nameSimpleIdent;
                            if (nameSimpleIdent == null)
                            {
                                G.Writeln2("*** ERROR: Composed names not (yet) allowed for loop variables");
                                throw new GekkoException();
                            }
                            string codeFrom = node[1][0].Code.ToString();
                            string codeEnd = node[1][1].Code.ToString();
                            string codeStep = null;
                            ASTNode stepNode = node[1][2];
                            if (stepNode == null) codeStep = "new ScalarVal(1d)";
                            else codeStep = node[1][2].Code.ToString();
                            string statements = node[2].Code.ToString();  //has been done in "ASTFORSTATEMENTS"
                            string tempName = "temp" + ++Globals.counter;                            
                            string startName = "start" + ++Globals.counter;
                            string endName = "end" + ++Globals.counter;
                            string stepName = "step" + ++Globals.counter;
                            //NOTE this will mean that the end and step are fixed when seeing the FOR. Should be ok. Alternative is crazy.                            

                            string loopVariable = null;
                            string setLoopStringCs = CacheRefScalarCs(out loopVariable, nameSimpleIdent, GetScalarCache(w), GetHeaderCs(w), EScalarRefType.Val, "double.NaN", false, true, false);
                                                        
                            node.Code.A(setLoopStringCs + G.NL);                            
                            node.Code.A("double " + stepName + " = " + codeStep + ".GetVal("+Globals.smpl+");" + G.NL);
                            node.Code.A("double " + startName + " = " + codeFrom + ".GetVal("+Globals.smpl+");" + G.NL);
                            node.Code.A("double " + endName + " = " + codeEnd + ".GetVal("+Globals.smpl+") + " + stepName + "/1000000d;" + G.NL);  //added a tiny bit of steplength, to guard against rounding errors
                            node.Code.A("ScalarVal " + tempName + " = (ScalarVal)" + loopVariable + ";" + G.NL);
                            node.Code.A("try {");                            
                            node.Code.A("for (" + tempName + ".val = " + startName + " ; O.ContinueIterating(" + tempName + ".val, " + endName + ", " + stepName + "); " + tempName + ".val += " + stepName + ")");
                            node.Code.A("{" + G.NL);
                            
                            node.Code.A(Globals.splitSTART);
                            node.Code.A(statements);
                            node.Code.A(Globals.splitSTOP);
                            
                            node.Code.A("}" + G.NL);
                            node.Code.A("} //end of try" + G.NL);  //assign var is always removed, also in case of error
                            node.Code.A("finally {" + G.NL);                            
                            node.Code.A("O.RemoveScalar(`" + nameSimpleIdent + "`);" + G.NL);
                            node.Code.A("}  //end of finally" + G.NL);  //end of finally   

                            node.Code.A(Globals.splitSTART);
                        }
                        break;

                    case "ASTFORDATE":
                        {
                            node.Code.A(Globals.splitSTOP);
                            
                            if (node[0].Text != "ASTFORLEFTSIDE" || node[1].Text != "ASTFORRIGHTSIDE" || node[2].Text != "ASTFORSTATEMENTS")
                            {
                                throw new GekkoException();
                            }
                            string nameSimpleIdent = node[0][0].nameSimpleIdent;
                            if (nameSimpleIdent == null)
                            {
                                G.Writeln2("*** ERROR: Composed names not (yet) allowed for loop variables");
                                throw new GekkoException();
                            }
                            string codeFrom = node[1][0].Code.ToString();
                            string codeEnd = node[1][1].Code.ToString();
                            string codeStep = null;
                            ASTNode stepNode = node[1][2];
                            if (stepNode == null) codeStep = "new ScalarVal(1d)";
                            else codeStep = node[1][2].Code.ToString();
                            string statements = node[2].Code.ToString();  //has been done in "ASTFORSTATEMENTS"
                            string tempName = "temp" + ++Globals.counter;
                            string startName = "start" + ++Globals.counter;
                            string endName = "end" + ++Globals.counter;
                            string stepName = "step" + ++Globals.counter;
                            //NOTE this will mean that the end and step are fixed when seeing the FOR. Should be ok. Alternative is crazy.                            

                            string loopVariable = null;
                            string setLoopStringCs = CacheRefScalarCs(out loopVariable, nameSimpleIdent, GetScalarCache(w), GetHeaderCs(w), EScalarRefType.Date, "GekkoTime.tNull", false, true, false);                            
                            
                            node.Code.A(setLoopStringCs + G.NL);
                            node.Code.A("int " + stepName + " = O.ConvertToInt(" + codeStep + ");" + G.NL);
                            node.Code.A("GekkoTime " + startName + " = O.ConvertToDate(" + codeFrom + ", O.GetDateChoices.Strict);" + G.NL);
                            node.Code.A("GekkoTime " + endName + " = O.ConvertToDate(" + codeEnd + ", O.GetDateChoices.Strict);" + G.NL);  //added a tiny bit of steplength, to guard against rounding errors
                            node.Code.A("ScalarDate " + tempName + " = (ScalarDate)" + loopVariable + ";" + G.NL);
                            node.Code.A("try {");
                            node.Code.A("for (" + tempName + ".date = " + startName + " ; O.ContinueIterating(" + tempName + ".date, " + endName + ", " + stepName + "); " + tempName + ".date = O.ConvertToDate(" + tempName + ".Add(smpl, " + codeStep + "), O.GetDateChoices.Strict))");
                            node.Code.A("{" + G.NL);

                            node.Code.A(Globals.splitSTART);
                            node.Code.A(statements);
                            node.Code.A(Globals.splitSTOP);

                            node.Code.A("}" + G.NL);
                            node.Code.A("} //end of try" + G.NL);  //assign var is always removed, also in case of error
                            node.Code.A("finally {" + G.NL);
                            node.Code.A("O.RemoveScalar(`" + nameSimpleIdent + "`);" + G.NL);
                            node.Code.A("}  //end of finally" + G.NL);  //end of finally 

                            node.Code.A(Globals.splitSTART);
                        }
                        break;
                    case "ASTFORNAME":
                    case "ASTFORSTRING":
                        {
                            node.Code.A(Globals.splitSTOP);
                            
                            bool x = false;
                            if (node.Text == "ASTFORNAME") x = true;
                            if (node[0].Text != "ASTFORLEFTSIDE2" || node[1].Text != "ASTFORRIGHTSIDE2" || node[2].Text != "ASTFORSTATEMENTS")
                            {
                                throw new GekkoException();
                            }

                            int n0 = node[0].ChildrenCount();
                            int n1 = node[1].ChildrenCount();
                            if (n0 != n1) throw new GekkoException();  //is not possible anyway                            

                            if (n0 == 1)
                            {                               
                                
                                //Normal string loop
                                string nameSimpleIdent = node[0][0].nameSimpleIdent;
                                if (nameSimpleIdent == null)
                                {
                                    G.Writeln2("*** ERROR: Composed names not allowed for loop variables");
                                    throw new GekkoException();
                                }
                                node.Code.A("O.ForString o" + Num(node) + " = new O.ForString();" + G.NL);
                                string rightSide = node[1][0].Code.ToString();
                                node.Code.A(rightSide);
                                string statements = node[2].Code.ToString();  //has been done in "ASTFORSTATEMENTS"                                                                                                                
                                //string iName = "i" + ++Globals.counter;
                                string tempName = "temp" + ++Globals.counter;
                                node.Code.A("try {");
                                node.Code.A("foreach(string " + tempName + " in o" + Num(node) + ".listItems) {" + G.NL);
                                string loopVariable = null; //The line below emits "O.SetValFromCache(..., tempName)", same as a "STRING x = ..." statement
                                string setLoopStringCs = CacheRefScalarCs(out loopVariable, nameSimpleIdent, GetScalarCache(w), GetHeaderCs(w), EScalarRefType.String, tempName, x, true, false);                                
                                node.Code.A(setLoopStringCs + G.NL);

                                node.Code.A(Globals.splitSTART);
                                node.Code.A(statements);
                                node.Code.A(Globals.splitSTOP);
                                
                                node.Code.A("}" + G.NL);
                                node.Code.A("} //end of try" + G.NL);  //assign var is always removed, also in case of error
                                node.Code.A("finally {" + G.NL);
                                //node.Code.A(loopVariable + " = null;" + G.NL; //invalidates pointer
                                node.Code.A("O.RemoveScalar(`" + nameSimpleIdent + "`);" + G.NL);
                                node.Code.A("}  //end of finally" + G.NL);  //end of finally
                            }
                            else
                            {                               
                                
                                //Parallel string loop
                                node.Code.A("O.ForString o" + Num(node) + " = new O.ForString();" + G.NL);
                                for (int i = 0; i < n0; i++)
                                {
                                    if (node[0][i].nameSimpleIdent == null)
                                    {
                                        G.Writeln2("*** ERROR: Composed names not allowed for loop variables");
                                        throw new GekkoException();
                                    }
                                    //node.Code.A("string s" + Num(node) + "_" + i + " = `" + node[0][i].nameSimpleIdent + "`;" + G.NL;
                                }
                                string nme = "test" + ++Globals.counter;
                                string nme2 = "test" + ++Globals.counter;
                                string nme3 = "test" + ++Globals.counter;

                                node.Code.A("List<List<string>> " + nme + " = new List<List<string>>();" + G.NL);
                                node.Code.A("List<string> " + nme2 + " = new List<string>();" + G.NL);
                                for (int i = 0; i < n0; i++)
                                {
                                    node.Code.A(node[1][i].Code);
                                    node.Code.A("List<string> x" + Num(node) + "_" + i + " = o" + Num(node) + ".listItems;" + G.NL);
                                    node.Code.A(nme + ".Add(x" + Num(node) + "_" + i + ");" + G.NL);
                                    node.Code.A(nme2 + ".Add(`" + node[0][i].nameSimpleIdent + "`);" + G.NL);
                                }

                                string statements = node[2].Code.ToString();  //has been done in "ASTFORSTATEMENTS"                                                                                                                                                
                                string tempName = "temp" + ++Globals.counter;
                                node.Code.A("try {" + G.NL);
                                //node.Code.A("foreach(string " + tempName + " in o" + Num(node) + ".listItems) {" + G.NL;

                                node.Code.A("int " + nme3 + "= O.ForListMax(" + nme + ");" + G.NL);
                                node.Code.A("O.ForListCheck(" + nme2 + ");" + G.NL);

                                //node.Code.A("for (int i = 0; i < " + nme3 + "; i++) {" + G.NL);
                                node.Code.A("for (int i").A(Num(node)).A(" = 0; i").A(Num(node)).A(" < ").A(nme3).A("; i").A(Num(node)).A("++) {").A(G.NL);

                                for (int i = 0; i < n0; i++)
                                {
                                    string loopVariable = null; //The line below emits "O.SetValFromCache(..., tempName)", same as a "STRING x = ..." statement
                                    string setLoopStringCs = CacheRefScalarCs(out loopVariable, node[0][i].nameSimpleIdent, GetScalarCache(w), GetHeaderCs(w), EScalarRefType.String, "x" + Num(node) + "_" + i + "[i" + Num(node) + "]", true, true, false);
                                    node.Code.A(setLoopStringCs + G.NL);
                                }

                                node.Code.A(Globals.splitSTART);
                                node.Code.A(statements);
                                node.Code.A(Globals.splitSTOP);
                                
                                node.Code.A("}" + G.NL);
                                //node.Code.A("}" + G.NL;
                                node.Code.A("} //end of try" + G.NL);  //assign var is always removed, also in case of error
                                node.Code.A("finally {" + G.NL);
                                //node.Code.A(loopVariable + " = null;" + G.NL; //invalidates pointer
                                for (int i = 0; i < n0; i++)
                                {
                                    node.Code.A("O.RemoveScalar(`" + node[0][i].nameSimpleIdent + "`);" + G.NL);
                                }
                                node.Code.A("}  //end of finally" + G.NL);  //end of finally
                            }

                            node.Code.A(Globals.splitSTART);
                        }
                        break;

                    case "ASTIF":
                        {
                            node.Code.A(Globals.splitSTOP);
                            node.Code.A("if(O.IsTrue(smpl, " + node[0].Code + ")) {");
                            node.Code.A(Globals.splitSTART);
                            GetCodeFromAllChildren(node, node[1][0]);                            
                            node.Code.A(Globals.splitSTOP);
                            node.Code.A("}");
                            node.Code.A("else {");
                            node.Code.A(Globals.splitSTART);
                            GetCodeFromAllChildren(node, node[2][0]);
                            node.Code.A(Globals.splitSTOP);
                            node.Code.A("}");
                            node.Code.A(Globals.splitSTART);
                        }
                        break;
                    case "ASTFUNCTIONDEF2":
                        {
                            StringBuilder sb = new StringBuilder();
                            
                            string returnTypeLower = node[0].Text.ToLower();
                            string functionNameLower = node[1][0].Text.ToLower();
                            int numberOfArguments = node[2].ChildrenCount();
                            
                            string internalName = "FunctionDef" + ++Globals.counter;

                            GetCodeFromAllChildren(node[3]);  //it is a placeholder node that does not get code

                            sb.AppendLine(internalName + "();" + G.NL);

                            //string vars = null; for (int i = 0; i < numberOfArguments; i++) vars += ", IVariable i" + (i + 1);
                            string vars = null;

                            if (node.functionDef == null)
                            {
                                //do nothing
                            }
                            else
                            {
                                for (int i = 0; i < node.functionDef.Count; i++)
                                {
                                    vars += ", " + node.functionDef[i].Item1 + " " + node.functionDef[i].Item2;
                                }
                            }
                            w.headerCs.AppendLine("public static void " + internalName + "() {" + G.NL);
                            w.headerCs.AppendLine(Globals.splitSTOP);
                            w.headerCs.AppendLine("O.PrepareUfunction(" + numberOfArguments + ", `" + functionNameLower + "`);" + G.NL);
                            w.headerCs.AppendLine("Globals.ufunctions" + numberOfArguments + ".Add(`" + functionNameLower + "`, (GekkoSmpl smpl, P p" + vars + ") => { " + node[3].Code.ToString() + " ; return null; });" + G.NL);
                            w.headerCs.AppendLine(Globals.splitSTART);
                            w.headerCs.AppendLine("}" + G.NL);

                            if (false)
                            {
                                sb.AppendLine("{");                                
                                GetCodeFromAllChildren(node[3]);  //it is a placeholder node that does not get code
                                sb.AppendLine(node[3].Code.ToString());
                                sb.AppendLine("}");
                            }
                            node.Code.A(sb.ToString());
                            

                        }
                        break;


                    case "ASTFUNCTIONDEF":
                        {
                            //NOTE: Splitting
                            //      All content is put into the header, w.headerCs, so it will not be split
                            //      in the split method, no matter if there are split markers or not
                            //      So node.Code will not be changed here, everything is piped to w.headeCs
                            //      As of now, we do not split the content of such user-defined methods into
                            //      Ci-blocks. If so, we would have to add markers to headerCs, put headerCs
                            //      through the splitting machine, and beware of "params_" lines (these should not
                            //      be put into Ci-methods).

                            w.uHeaderCs = new StringBuilder();

                            if (Globals.uFunctionStorageCs.ContainsKey(w.uFunctionsHelper.functionName))
                            {
                                //For now, we just overwrite the function, even if it has different overload signature
                                Globals.uFunctionStorageCs.Remove(w.uFunctionsHelper.functionName);
                                ////TODO: allow overloads
                                //G.Writeln2("*** ERROR: User function with name '" + w.uFunctionsHelper.functionName + "' has already been defined");
                                //throw new GekkoException();
                            }
                                                        
                            //if (w.functionUserDefined.ContainsKey(w.uFunctionsHelper.functionName))
                            //{
                            //    G.Writeln2("*** ERROR: User function with name '" + w.uFunctionsHelper.functionName + "' has already been defined");
                            //    throw new GekkoException();
                            //}
                            //w.functionUserDefined.Add(w.uFunctionsHelper.functionName, true);

                            //We use ToLower(), since user functions are not distinguished by means of capitalization
                            string lhsClassNameCode = G.GetVariableType(w.uFunctionsHelper.lhsTypes.Count);

                            if (w.uFunctionsHelper.lhsTypes.Count > 1)
                            {
                                //Create the class corresponding to the return tuple (lhs)
                                string tupleClassName = G.GetVariableType(w.uFunctionsHelper.lhsTypes.Count);
                                //CreateTupleClass(w.uHeaderCs, w.uFunctionsHelper.lhsTypes.Count, tupleClassName, w.tupleClasses);
                            }

                            string method = Globals.splitSTOP;
                            method += "public static " + lhsClassNameCode + " " + w.uFunctionsHelper.functionName.ToLower() + "(" + Globals.functionP2Cs + ", " + Globals.functionTP2Cs + ", ";

                            for (int i = 0; i < w.uFunctionsHelper.storage.Count; i++)
                            {
                                FunctionArgumentsHelperElements fah = w.uFunctionsHelper.storage[i];
                                //method += G.GetVariableType(fah.type) + " " + fah.parameterCode;                                
                                //TODO type checks...
                                if (fah.tupleCount > 1)
                                {
                                    //Create the class corresponding to the input tuple (in rhs params)
                                    string tupleClassName = G.GetVariableType(fah.tupleCount);
                                    //CreateTupleClass(w.uHeaderCs, fah.tupleCount, tupleClassName, w.tupleClasses);
                                    //this is a tuple
                                    method += tupleClassName + " " + fah.tupleNameCode;
                                    i = i + (fah.tupleCount - 1);  //we skip the rest of these tuples here!
                                }
                                else
                                {
                                    method += "IVariable" + " " + fah.parameterCode;
                                }
                                method += ", ";
                            }
                            if (method.EndsWith(", ")) method = method.Substring(0, method.Length - 2);  //we remove the last ", "
                            method += ") {" + G.NL;

                            method += node[3].Code + G.NL;  //expressions, should always be subnode #4                            

                            method += "}" + G.NL;

                            w.uHeaderCs.AppendLine(w.uFunctionsHelper.headerCs.ToString());

                            w.uHeaderCs.AppendLine(method);

                            Globals.uFunctionStorageCs.Add(w.uFunctionsHelper.functionName, w.uHeaderCs.ToString());

                            ResetUFunctionHelpers(w);

                        }
                        break;


                    // ================= INDENTATION CODE END ==================
                    // ================= INDENTATION CODE END ==================
                    // ================= INDENTATION CODE END ==================
                    
                    case "ASTURL":
                        {
                            //node.Code.CA(node[0].Code;
                            string s = null;
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                if (child.IsFirstChild()) s = child.Code.ToString();
                                else s += ".Add(smpl, new ScalarString(`/`)).Add(smpl, " + child.Code + ")";
                            }
                            node.Code.A(s);
                        }
                        break;
                    case "ASTURLPART":
                        {
                            string s = null;
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                if (child.IsFirstChild()) s = child.Code.ToString();
                                else s += ".Add(smpl, new ScalarString(`.`)).Add(smpl, " + child.Code + ")";
                            }
                            node.Code.A(s);
                        }
                        break;
                    case "ASTURLFIRST1":
                        {
                            node.Code.CA(node[0].Code + ".Add(smpl, new ScalarString(`http://`)).Add(smpl, " + node[1].Code + ")");
                        }
                        break;
                    case "ASTURLFIRST3":                    
                        {
                            node.Code.CA(node[0].Code);
                        }
                        break;
                    case "ASTFILENAME":
                        {
                            //node.Code.CA(node[0].Code;
                            string s = null;                            
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                if (child.IsFirstChild()) s = child.Code.ToString();
                                else s += ".Add(smpl, new ScalarString(`\\\\`)).Add(smpl, " + child.Code + ")";
                            }
                            node.Code.A(s);
                        }
                        break;
                    case "ASTFILENAMEPART":
                        {
                            string s = null;
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                if (child.IsFirstChild()) s = child.Code.ToString();
                                else s += ".Add(smpl, new ScalarString(`.`)).Add(smpl, " + child.Code + ")";
                            }
                            node.Code.A(s);
                        }
                        break;
                    case "ASTFILENAMEFIRST1":                    
                        {
                            node.Code.CA(node[0].Code + ".Add(smpl, new ScalarString(`:\\\\`)).Add(smpl, " + node[1].Code + ")");
                        }
                        break;                    
                    case "ASTFILENAMEFIRST2":
                    case "ASTFILENAMEFIRST3":
                        {
                            node.Code.CA(node[0].Code);
                        }
                        break;
                    case "ASTFUNCTION":
                        {
                            string functionNameLower = GetFunctionName(node);                            
                            string[] listNames = IsGamsSumFunctionOrUnfoldFunction(node, functionNameLower);  //also checks that the name is "sum"

                            if (listNames != null)
                            {
                                //GAMS-like sum function, for instance sum(#i, x[#i]+1),
                                //or unfold()-function, for instance unfold(#i, x[#i]+1)

                                //string funcName = "localFunc" + ++Globals.counter;

                                string parentListLoopVars1 = null;
                                string parentListLoopVars2 = null;

                                int parentListLoopCounter = 0;
                                ASTNode node2 = node;
                                while (true)
                                {
                                    node2 = node2.Parent;
                                    if (node2 == null) break;
                                    if (node2.listLoopAnchor != null)
                                    {
                                        foreach (KeyValuePair<string, string> kvp in node2.listLoopAnchor)
                                        {
                                            parentListLoopVars1 += "IVariable " + kvp.Value + ", ";
                                            parentListLoopVars2 += kvp.Value + ", ";
                                            parentListLoopCounter++;
                                        }
                                    }
                                }
                                if (parentListLoopVars1 != null) parentListLoopVars1 = parentListLoopVars1.Substring(0, parentListLoopVars1.Length - ", ".Length);
                                if (parentListLoopVars2 != null) parentListLoopVars2 = parentListLoopVars2.Substring(0, parentListLoopVars2.Length - ", ".Length);

                                string tempName = "temp" + ++Globals.counter;
                                string funcName = "func" + ++Globals.counter;
                                StringBuilder sb1 = new StringBuilder();
                                string listName = null;
                                if (node.listLoopAnchor == null)
                                {
                                    G.Writeln2("*** ERROR: Internal error #98973422");
                                    throw new GekkoException();
                                }
                                //method def:

                                string iv = "IVariable";
                                for (int i = 0; i < parentListLoopCounter; i++)
                                {
                                    iv = iv + ", IVariable";
                                }

                                int smplCommandNumber = ++Globals.counter;

                                sb1.AppendLine("Func<" + iv + "> " + funcName + " = (" + parentListLoopVars1 + ") => {");
                                //sb1.AppendLine("public static IVariable " + tempName + "(GekkoSmpl smpl" + parentListLoopVars1 + ") {");
                                if (G.Equal(functionNameLower, "sum"))
                                {                                    
                                    sb1.AppendLine(GekkoSmplCommandHelper1(smplCommandNumber, "Sum"));
                                    sb1.AppendLine("Series " + tempName + " = new Series(ESeriesType.Normal, Program.options.freq, null); " + tempName + ".SetZero(smpl);" + G.NL);
                                }
                                else
                                {
                                    sb1.AppendLine(GekkoSmplCommandHelper1(smplCommandNumber, "Unfold"));
                                    sb1.AppendLine("List " + tempName + " = new List();" + G.NL);
                                }
                                
                                foreach (KeyValuePair<string, string> kvp in node.listLoopAnchor)
                                {
                                    sb1.AppendLine("foreach (IVariable " + kvp.Value + " in new O.GekkoListIterator(O.Lookup(smpl, null, ((O.scalarStringHash).Add(smpl, (new ScalarString(" + Globals.QT + kvp.Key + Globals.QT + ")))), null, false, EVariableType.Var))) {");  //false is regarding isLeftSide
                                }

                                if (G.Equal(functionNameLower, "sum"))
                                {
                                    sb1.AppendLine(tempName + ".InjectAdd(smpl, " + tempName + ", " + node[2].Code.ToString() + ");" + G.NL);
                                }
                                else
                                {
                                    sb1.AppendLine(tempName + ".Add(" + node[2].Code.ToString() + ");" + G.NL);
                                }

                                foreach (KeyValuePair<string, string> kvp in node.listLoopAnchor)
                                {
                                    sb1.AppendLine("}");
                                }
                                sb1.AppendLine(GekkoSmplCommandHelper2(smplCommandNumber));  //resets command name to what it was previously
                                sb1.AppendLine("return " + tempName + ";" + G.NL);
                                sb1.AppendLine("};");  //method def, must end with ;
                                //w.headerCs.Append(sb1);
                                if (w.wh.localFuncs == null) w.wh.localFuncs = new GekkoStringBuilder();
                                w.wh.localFuncs.AppendLine(sb1.ToString());
                                node.Code.A(funcName + "(" + parentListLoopVars2 + ")"); //functionname may be for instance temp27(smpl)

                            }
                            else
                            {
                                //Not a sum() or unfold() function that is going to be looped                                

                                

                                string meta = null;
                                if (Globals.gekkoInbuiltFunctions.TryGetValue(functionNameLower, out meta))
                                {
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
                                                extra = "O.Smpl(smpl, " + -int.Parse(meta) + "), ";
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

                                    string args = null;
                                    for (int i = 1; i < node.ChildrenCount(); i++)
                                    {
                                        if (lagIndex == i)
                                        {
                                            if (lagIndexOffset == 0)
                                            {
                                                extra = "O.Smpl(smpl, " + node[i].Code + "), ";
                                            }
                                            else
                                            {
                                                extra = "O.Smpl(smpl, O.Add(smpl, " + node[i].Code + ", new ScalarVal(" + lagIndexOffset + "d))), ";
                                            }
                                        }                                        
                                        
                                        args += ", " + node[i].Code;
                                        
                                    }
                                    int numberOfArguments = node.ChildrenCount() - 1;
                                    node.Code.A("Functions." + functionNameLower).A("(" + extra + Globals.functionT1Cs + "").A(args).A(")");
                                }
                                else
                                {
                                    string args = null;
                                    for (int i = 1; i < node.ChildrenCount(); i++)
                                    {
                                        args += ", " + node[i].Code;
                                    }
                                    int numberOfArguments = node.ChildrenCount() - 1;
                                    node.Code.A("O.FunctionLookup").A(numberOfArguments).A("(`").A(functionNameLower).A("`)(" + Globals.functionTP1Cs + "").A(args).A(")");
                                }

                            }                        
                        }
                        break;
                    
                    case "ASTFUNCTIONDEFARGS":
                        {
                            //No code to be harvested, function args are completely primitive {type, name} items put in functionArguments container.
                        }
                        break;
                    case "ASTFUNCTIONDEFARG":
                        {
                            //No code to be harvested, function args are completely primitive {type, name} items put in functionArguments container.
                            //handled in ASTFUNCTIONDEFRHSSIMPLE and ASTFUNCTIONDEFRHSTUPLE
                        }
                        break;
                    case "ASTFUNCTIONDEFRHSTUPLE":
                        {                            
                            //TODO TODO TODO check that args have different names     
                            int counter = 0;
                            int paramCount = -12345;
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                counter++;
                                if (counter == 1) paramCount = w.uFunctionsHelper.storage.Count;
                                FunctionArgumentsHelperElements fah = new FunctionArgumentsHelperElements();                                
                                fah.parameterName = child[1].Text;
                                fah.type = child[0].Text;
                                fah.tupleNameCode = Globals.functionParameterCode + fah.type + "_" + paramCount;
                                fah.parameterCode = Globals.functionParameterCode + fah.type + "_" + paramCount + ".tuple" + (counter - 1);
                                fah.tupleCount = node.ChildrenCount();
                                w.uFunctionsHelper.storage.Add(fah);
                            }                            
                        }
                        break;
                    case "ASTFUNCTIONDEFRHSSIMPLE":
                        {
                            //TODO TODO TODO check that args have different names                         
                            ASTNode child = node[0];
                            FunctionArgumentsHelperElements fah = new FunctionArgumentsHelperElements();
                            fah.parameterName = child[1].Text;
                            fah.type = child[0].Text;
                            fah.parameterCode = Globals.functionParameterCode + fah.type + "_" + fah.parameterName + w.uFunctionsHelper.storage.Count;
                            w.uFunctionsHelper.storage.Add(fah);
                        }
                        break;                                        
                    case "ASTFUNCTIONDEFLHSTUPLE":
                        {
                            //do nothing
                        }
                        break;                    
                    case "ASTFUNCTIONDEFTYPE":
                            {
                                if (node[0].Text == "ASTFUNCTIONDEFLHSTUPLE")
                                {                                    
                                    foreach (ASTNode child in node[0].ChildrenIterator())
                                    {
                                        w.uFunctionsHelper.lhsTypes.Add(child.Text);
                                    }                                    
                                }
                                else
                                {
                                    w.uFunctionsHelper.lhsTypes.Add(node[0].Text);
                                }

                            }
                            break;
                    case "ASTFUNCTIONDEFNAME":
                        {
                            w.uFunctionsHelper.functionName = node[0].Text;
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
                                node.Code.CA("O.Add(smpl, " + node[0].Code + ", new ScalarString(`:`), " + node[1].Code + ")");                             
                            }
                        }
                        break;
                    case "ASTSERIESQUESTION":
                        {
                            //GENR fy = ...                            
                            node.Code.A("O.SeriesQuestion();" + G.NL);
                        }
                        break;
                    case "ASTGENR":                    
                        {
                            //GENR fy = ...                            
                            node.Code.A(HandleGenr(node, Num(node), node[0].Code.ToString(), node[1].Code.ToString(), node[2].Code.ToString(), w, null));
                        }
                        break;
                    case "ASTSERIES":
                        {
                            

                            //ASTNode node, string numNode, string childCodePeriod, string childCodeLhsName, string childCodeRhs, W w, string lhsFunction

                            string childCodePeriod = "";  //FIXME                            
                            string childCodeLhsName = node[0].Code.ToString();
                            string childCodeRhs = node[1].Code.ToString();
                            string numNode = Num(node);
                            string nodeCode = null;
                            
                            if (w.wh.seriesHelperListNames != null)
                            {
                                foreach (KeyValuePair<string, string> kvp in w.wh.seriesHelperListNames)
                                {
                                    nodeCode += EmitListLoopingCode(node, kvp);
                                }
                            }                            

                            nodeCode += "O.SeriesDef o" + numNode + " = new O.SeriesDef();" + G.NL;

                            nodeCode += "o" + numNode + ".t1 = Globals.globalPeriodStart;";
                            nodeCode += "o" + numNode + ".t2 = Globals.globalPeriodEnd;";
                                                        
                            nodeCode += childCodePeriod + G.NL;  //dates
                            
                            nodeCode += "smpl = new GekkoSmpl(o" + numNode + ".t1, o" + numNode + ".t2);" + G.NL;

                            nodeCode += "o" + numNode + ".p = p;" + G.NL;

                            nodeCode += EmitLocalCacheForTimeLooping(w);

                            nodeCode += "o" + numNode + ".lhs = " + childCodeLhsName + ";" + G.NL; //we want the rhs to be constructed first, so that SERIES xx1 = xx1; fails if y does not exist (otherwist it would have been autocreated).                        

                            //NB NB NB
                            //NB NB NB
                            //NB NB NB   EmitLocalCacheForTimeLooping should perhaps be only RHS for series???
                            //NB NB NB   it contains the lhs variable too --> superfluous!
                            //NB NB NB

                            //nodeCode += "o" + numNode + ".rhs = O.ConvertToTimeSeriesLight(" + childCodeRhs + ");" + G.NL;

                            nodeCode += "o" + numNode + ".rhs = O.ConvertToTimeSeriesLight(" + Globals.smpl + ", GekkoExpression1(" + Globals.smpl + ", 1, p));" + G.NL;

                            w.headerExpressions.Append("public static IVariable GekkoExpression1(GekkoSmpl " + Globals.smpl + ", int bankNumber, P p) {" + G.NL + EmitLocalCacheForTimeLooping(w) + G.NL + "return " + childCodeRhs + ";" + G.NL + "}" + G.NL);

                            if (node.Parent != null && node.Parent.Text == "ASTMETA" && node.Parent.specialExpressionAndLabelInfo != null && node.Parent.specialExpressionAndLabelInfo.Length > 1)
                            {
                                //specialExpressionAndLabelInfo[0] should be "ASTMETA" here
                                nodeCode += "o" + numNode + ".meta = @`" + node.Parent.specialExpressionAndLabelInfo[1] + "`;" + G.NL;
                            }
                            nodeCode += "o" + numNode + ".Exe();" + G.NL;

                            node.Code.A(Globals.GekkoSmplNull);

                            if (w.wh.seriesHelperListNames != null)
                            {
                                foreach (KeyValuePair<string, string> kvp in w.wh.seriesHelperListNames)
                                {
                                    nodeCode += "}" + G.NL;
                                }
                            }

                            node.Code.A(nodeCode);

                        }
                        break;                    
                    case "ASTSERIESLHS":
                        {

                            GetCodeFromAllChildren(node);                                                       

                            //kind of a hack, but avoids some complexity
                            //alternative: have "ASTSERIESLHS" register in the w object

                            
                            //node.Code.Replace("O.seriesHelperFalse", "O.seriesHelperTrue");
                           
                                  
                        }
                        break;
                    case "ASTSERIESRHS":
                        {                            
                            node.Code.CA(node[0].Code.ToString());
                                                        
                        }
                        break;
                    case "ASTGENRLHSFUNCTION":
                        {
                            //GENR dlog(fy) = ...                            
                            node.Code.A(HandleGenr(node, Num(node), node[0].Code.ToString(), node[1].Code.ToString(), node[2].Code.ToString(), w, node[3].Text));
                        }
                        break;
                    case "ASTGENRINDEXER":
                        {
                            //GENR fy[2015] = ...
                            node.Code.A("O.GetTimeSeries(" + node[0].Code + ").SetData(O.ConvertToDate(" + node[1].Code + ", O.GetDateChoices.Strict), O.ConvertToVal(" + node[2].Code + "));" + G.NL);
                            node.Code.A("O.GetTimeSeries(" + node[0].Code + ").Stamp();" + G.NL);
                        }
                        break;
                    case "ASTMATRIXINDEXER":
                        {
                            //MATRIX a[3, 5] = ...
                            node.Code.A("O.GetMatrixFromString(" + node[0].Code + ").SetData(" + node[1].Code + ", " + node[2].Code + ", " + node[3].Code + ");" + G.NL);
                        }
                        break;                    
                    case "ASTMATRIX":
                        {
                            //MATRIX a = matrix(100, 100);  etc.
                            //node.Code.A("O.SetMatrix(" + node[0].Code + ", " + node[1].Code + ");" + G.NL;

                            if (node[1].Text == "?")
                            {

                                if (node.ChildrenCount() > 2)
                                {
                                    node.Code.A("O.Matrix2.Q(" + Globals.QT + node[2].Text + Globals.QT + ");" + G.NL);
                                }
                                else
                                {
                                    node.Code.A("O.Matrix2.Q();" + G.NL);
                                }
                            }
                            else
                            {
                                string s = "null";

                                if (node.ChildrenCount() > 2)
                                {
                                    s = node[2].Code.ToString();
                                }

                                node.Code.A("O.Matrix2 o" + Num(node) + " = new O.Matrix2();" + G.NL);
                                if (node[0].ChildrenCount() > 0)
                                {
                                    GetCodeFromAllChildren(node[0]); //options  
                                    node.Code.A(node[0].Code);
                                }                                
                                                                                              
                                node.Code.A("o" + Num(node) + ".Exe(" + node[1].Code + ", " + s + ");" + G.NL);
                            }
                        }
                        break;
                    case "ASTGENRLISTINDEXER":
                        {
                            node.Code.A(HandleGenr(node, Num(node), node[0].Code.ToString(), "O.GetTimeSeriesFromList(smpl, " + node[1].Code + ", " + node[2].Code + ", 1)", node[3].Code.ToString(), w, null));
                            //GENR #m[2] = ...
                        }
                        break;
                    case "ASTGENRLISTINDEXER2":
                        {
                            //GENR #m[2][2015] = ...
                            node.Code.A("O.GetTimeSeriesFromList(smpl, " + node[0].Code + ", " + node[1].Code + ", 1).ts.SetData(O.ConvertToDate(" + node[2].Code + ", O.GetDateChoices.Strict), O.ConvertToVal(" + node[3].Code + "));" + G.NL);
                        }
                        break;
                    case "ASTHASHPAREN":
                        {
                            node.Code.CA(node[0].Code);
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
                    //case "ASTHASH":
                    //    {
                    //        //if (node.Parent != null && node.Number == 1 && node.Parent.Text == "ASTFUNCTION")
                    //        //{
                    //        //    //for instance the first #i in sum(#i, x[#i])

                    //        //    string[] sumFunctionListNames = IsGamsLikeSumFunction1(false, node.Parent, w, node.Parent[0].Text.ToLower());  //can return null                                                        
                                
                    //        //    if (sumFunctionListNames != null)  //Is first argument of GAMS-like sum function, sum(#i, x[#i])
                    //        //    {


                    //        //        string functionName = GetFunctionName(node.Parent);
                    //        //        string[] listNames = IsGamsLikeSumFunction1(true, node.Parent, w, functionName);
                    //        //        if (listNames != null) HandleGamsLikeSumFunction(listNames, true, w, null);



                    //        //        string nodeCode = null;
                    //        //        string dName = "sumHelper" + ++Globals.counter;
                    //        //        nodeCode += "double " + dName + "" + " = 0d;" + G.NL;

                    //        //        foreach (KeyValuePair<string, string> kvp in w.wh.sumHelperListNames)
                    //        //        {
                    //        //            nodeCode += EmitListLoopingCode(node, kvp);  //foreach(...
                    //        //            nodeCode += dName + " += " + node[2].Code.ToString() + ";";
                    //        //        }

                    //        //        foreach (KeyValuePair<string, string> kvp in w.wh.sumHelperListNames)
                    //        //        {
                    //        //            nodeCode += "}" + G.NL;
                    //        //        }

                    //        //        HandleGamsLikeSumFunction(sumFunctionListNames, false, w, node.Code.ToString());

                    //        //        node.Code.A(nodeCode);

                    //        //    }
                    //        //}


                    //        string simpleIdent = null;
                    //        bool stringify = false;
                    //        if (node.ChildrenCount() > 0 && node[0].Text == "ASTDOLLARHASHNAMESIMPLE") stringify = true;
                    //        if (node.ChildrenCount() > 0 && (node[0].Text == "ASTHASHNAMESIMPLE" || node[0].Text == "ASTDOLLARHASHNAMESIMPLE"))
                    //        {
                    //            simpleIdent = node[0][0].Text;
                    //        }

                    //        if (node[0].Text == "ASTLISTFILE")
                    //        {
                    //            node.Code.A("O.ZListFile(O.ConvertToString(" + node[0][0].Code + "))");
                    //        }
                    //        else
                    //        {

                    //            if (simpleIdent != null)
                    //            {
                    //                string fa = FindFunctionArguments(node, w, simpleIdent);
                    //                if (fa != null)
                    //                {
                    //                    node.Code.A(fa);
                    //                }
                    //                else
                    //                {
                    //                    AstListHelper(node, w, simpleIdent, stringify);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                node.Code.A("O.ZList(" + node[0].Code + ")");
                    //            }
                    //        }
                    //        break;
                    //    }
                    //case "ASTHASHNAMESIMPLE":
                    //    {
                    //        node.Code.CA("O.GetListFromCache(`" + node[0].Text + "`)";
                    //    }
                    //    break;
                    case "ASTIDENT":                    
                        {
                            node.Code.CA("new ScalarString(`" + node[0].Text + "`)");  //problem is that we now allow VAL %v = 1, for instance. Here %v is not recursive.
                        }
                        break;
                                            
                    case "ASTIDENTDIGIT":
                        {
                            if (node[0].Text == "ASTIDENT")
                            {
                                node.Code.CA(node[0].Code);
                            }
                            else
                            {
                                node.Code.CA("new ScalarString(`" + node[0].Text + "`)");
                            }                            
                        }
                        break;
                    //case "ASTIMPORT":
                    //    {
                    //        node.Code.A(Globals.clearTsCsCode + G.NL);
                    //        node.Code.A("O.Import o" + Num(node) + " = new O.Import();" + G.NL);
                    //        node.Code.A("o" + Num(node) + ".p = p;");
                    //        GetCodeFromAllChildren(node);
                    //        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    //    }
                    //    break;
                    case "ASTDOWNLOAD":
                        {
                            node.Code.A("O.Download o" + Num(node) + " = new O.Download();" + G.NL);
                            string s = null;
                            if (node[0].Text != null) s = "http://";
                            node.Code.A("o" + Num(node) + ".dbUrl = `" + s + "` + O.ConvertToString(" + node[1].Code + ");" + G.NL);
                            node.Code.A(node[2].Code);  //fileName json
                            if (node[3] != null) node.Code.A(node[3].Code); //px file dump
                            if (node[4] != null) node.Code.A(node[4].Code); //options
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTEMPTYRANGEELEMENT":
                        {
                            node.Code.CA("null");
                        }
                        break;                        
                    case "ASTINDEX":  //the INDEX command
                        {                                               
                            node.Code.A("O.Index o" + Num(node) + " = new O.Index();" + G.NL);
                            string nodeCode = "";
                            if (node[1][0] != null) nodeCode = HandleListFile(node[1], nodeCode);
                            node.Code.A(nodeCode);                            
                            node.Code.A(node[0].Code);
                            if (node[2] != null) node.Code.A(node[2].Code);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTPRT":
                        {
                            if (true)
                            {
                                
                                {
                                    //PRT
                                    node.Code.A("O.Prt o" + Num(node) + " = new O.Prt();" + G.NL);
                                }
                                GetCodeFromAllChildren(node);

                                if (node.Text == "ASTPRT")
                                {
                                    //Globals.lastPrtCsSnippet = node.Code.ToString();  //without the o117.Exe()
                                    //Globals.lastPrtCsSnippet += "return o" + Num(node) + ";" + G.NL;
                                    //Globals.lastPrtCsSnippetHeader = w.headerCs.ToString();  //may contain a lot of unnecessary IVariables, but never mind (not a problem when used interactively)

                                    Globals.prtCsSnippetsCounter++;
                                    node.Code.A("o" + Num(node) + ".counter = " + Globals.prtCsSnippetsCounter + ";" + G.NL);
                                    Globals.prtCsSnippets.Add(Globals.prtCsSnippetsCounter, node.Code.ToString() + "return o" + Num(node) + ";" + G.NL);
                                    Globals.prtCsSnippetsHeaders.Add(Globals.prtCsSnippetsCounter, w.headerCs.ToString());
                                }
                                node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                            }
                            

                        }
                        break;
                    case "ASTREBASE":  //the REBASE command
                        {
                            node.Code.A("O.Rebase o" + Num(node) + " = new O.Rebase();" + G.NL);                                                        
                            node.Code.A(node[0].Code);

                            if (node[1][0] != null)
                            {
                                node.Code.A("o" + Num(node) + ".date1 = O.ConvertToDate(" + node[1][0].Code + ", O.GetDateChoices.Strict);" + G.NL);
                            }
                            if (node[1][1] != null)
                            {
                                node.Code.A("o" + Num(node) + ".date2 = O.ConvertToDate(" + node[1][1].Code + ", O.GetDateChoices.Strict);" + G.NL);
                            }
                            if (node[2] != null) node.Code.A(node[2].Code);  //options
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTCOUNT":  //the COUNT command
                        {                            
                            node.Code.A("O.Count o" + Num(node) + " = new O.Count();" + G.NL);                            
                            node.Code.A("o" + Num(node) + ".listItems = O.GetList(" + node[0].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTINDEXERALONE":  //indexer with nothing at the left: [a*], not #z[a*]. For ASTINDEXER, see "["
                        {
                            //Only 1 dimension supported                        
                            if (node.ChildrenCount() > 1) throw new GekkoException();
                            node.Code.A("O.Indexer(O.Indexer2(smpl, " + node[0].Code + "), smpl, null, " + node[0].Code + ")"); //null signals that it has nothing on the left
                        }
                        break;
                    case "ASTDOTORINDEXER":
                        {

                            string ivTempVar = SearchUpwardsInTree4(node);

                            //isLhs is true if the indexer is on the left-hand side, and is the last indexer.
                            //For instance #m[2][3] = 'a' -----> here the [3] indexer will get "true"
                            //string isLhs = "false";
                            //if (node.Parent.Text == "ASTLEFTSIDE") isLhs = "true";

                            //LIGHTFIXME, isRhs

                            string indexes = null;
                            for (int i = 0; i < node[1].ChildrenCount(); i++)
                            {
                                ASTNode child = node[1][i];

                                string listName = GetSimpleHashName(child[0]);
                                string internalName = null;
                                if (listName != null)
                                {
                                    internalName = SearchUpwardsInTree2(node, listName);
                                }

                                if (internalName != null)
                                {
                                    indexes += internalName;
                                }
                                else
                                {
                                    indexes += node[1][i].Code.ToString();
                                }
                                if (i < node[1].ChildrenCount() - 1) indexes += ", ";
                            }
                            if (ivTempVar == null)
                            {
                                node.Code.A("O.Indexer(O.Indexer2(smpl, " + indexes + "), smpl, " + node[0].Code + ", " + indexes + ")");
                            }
                            else
                            {
                                node.Code.A("O.IndexerSetData(smpl, ").A(node[0].Code).A(",  ").A(ivTempVar).A(", ").A(indexes).A(")");
                            }

                        }
                        break;
                    case "ASTINDEXER":
                        {
                            //handled in ASTDOTORINDEXER
                            //GetCommaCodeFromAllChildren(node);
                        }
                        break;
                    case "ASTINDEXERELEMENTIDENT": 
                        {
                            string s = node[0][0].Text;
                            node.Code.A("new ScalarString(\"" + s + "\")");
                            
                        }
                        break;
                    case "ASTINDEXERELEMENT":  //For ASTINDEXER, see "["
                    case "ASTINDEXERELEMENTPLUS":
                        {
                            //if (node.ChildrenCount() == 2)
                            //{
                            //    if (!node[0].Code.IsNull())
                            //    {
                            //        node.Code.A("(" + node[0].Code + ")" + ".Add(smpl, new ScalarString(\":\")).Add(smpl, " + node[1].Code + ")");
                            //    }
                            //    else
                            //    {
                            //        node.Code.A(node[1].Code);
                            //    }
                            //}
                            //else if (node.ChildrenCount() == 3)
                            //{
                            //    if (node.Text == "ASTINDEXERELEMENTPLUS")
                            //    {
                            //        G.Writeln2("*** ERROR: You cannot use '+' as starting character in ranges");
                            //        throw new GekkoException();
                            //    }
                            //    if (!node[0].Code.IsNull())
                            //    {
                            //        node.Code.A("new IVariablesFilterRange((" + node[0].Code + ")" + ".Add(smpl, new ScalarString(\":\")).Add(smpl, " + node[1].Code + "), " + node[2].Code + ")");
                            //    }
                            //    else
                            //    {
                            //        node.Code.A("new IVariablesFilterRange(" + node[1].Code + ", " + node[2].Code + ")");
                            //    }
                            //}
                            //else throw new GekkoException();
                            if (node.ChildrenCount() == 1) GetCodeFromAllChildren(node);
                            else
                            {
                                node.Code.A("(new Range(").A(node[0].Code).A(", ").A(node[1].Code).A("))");
                            }
                        }
                        break;
                    case "ASTINDEXERELEMENTBANK":                    
                        {
                            GetCodeFromAllChildren(node);                            
                        }
                        break;
                    case "ASTINTEGER":
                        {
                            //TODO 
                            //TODO 
                            //TODO use cache to avoid dublets
                            //TODO Maybe not: this simplifies user defined functions
                            //TODO 
                            string minus = HandleNegate(node);
                            string intWithNumber = "i" + ++Globals.counter;
                            string s = "new ScalarVal(" + minus + node[0].Text + "d)";
                            GetHeaderCs(w).AppendLine("public static readonly ScalarVal " + intWithNumber + " = " + s + ";");
                            node.Code.CA(intWithNumber);  //no need for checking if it exists
                        }
                        break;
                    case "ASTINTEGERNEGATIVE":
                        {
                            node.Code.A(node[0].Code);
                        }
                        break;
                    case "ASTMISSING":
                        {
                            //TODO 
                            //TODO 
                            //TODO use cache to avoid dublets
                            //TODO Maybe not: this simplifies user defined functions
                            //TODO                             
                            string intWithNumber = "d" + ++Globals.counter;
                            string s = "new ScalarVal(double.NaN)";
                            GetHeaderCs(w).AppendLine("public static readonly ScalarVal " + intWithNumber + " = " + s + ";");
                            node.Code.CA(intWithNumber);  //no need for checking if it exists
                        }
                        break;
                    case "ASTLISTDEF":
                        {
                            node.Code.A("O.ListDefHelper(");
                            GetCommaCodeFromAllChildren(node);                            
                            node.Code.A(")");
                        }
                        break;
                    case "ASTLIST":
                        {
                            if (node[0].Text == "?")
                            {
                                if (node.ChildrenCount() < 2)
                                {
                                    node.Code.A("Program.List(`?`, null, null, false);" + G.NL);
                                }
                                else
                                {
                                    node.Code.A("O.List.Q(" + Globals.QT + node[1].Text + Globals.QT + ");" + G.NL);
                                }
                            }
                            else
                            {                                
                                node.Code.A(HandleList(node, node[1].Code.ToString()));
                            }
                        }
                        break;
                    case "ASTLISTFILE":
                        {
                            node.Code.A(node[0].Code);
                        }
                        break;
                    case "ASTRANGEWITHBANK":
                    case "ASTWILDCARDWITHBANK":
                    case "ASTLISTITEMWILDRANGE": //can be deleted, does not exist anymore
                        {
                            string bankCs = node[0].Code.ToString();
                            if (node.ChildrenCount() == 2)
                            {
                                //wildcard   
                                if (bankCs == null)
                                {
                                    node.Code.CA(node[1].Code);
                                }
                                else
                                {
                                    node.Code.CA("(" + bankCs + ").Add(smpl, new ScalarString(\":\")).Add(smpl, " + node[1].Code + ")");
                                }
                             
                            }
                            else
                            {
                                if (bankCs == null)
                                {
                                    node.Code.CA("(" + node[1].Code + ").Add(smpl, new ScalarString(\"..\")).Add(smpl, " + node[2].Code + ")");
                                }
                                else
                                {
                                    //range
                                    node.Code.CA("(" + bankCs + ").Add(smpl, new ScalarString(\":\")).Add(smpl, " + node[1].Code + ").Add(smpl, new ScalarString(\"..\")).Add(smpl, " + node[2].Code + ")");
                                }
                            }                            
                        }
                        break;
                    case "ASTLISTITEMWILDRANGEBANK":
                        {
                            if (node.ChildrenCount() > 0)
                            {                            
                                node.Code.A(node[0].Code);
                            }
                        }
                        break;
                    //case "ASTLISTITEM":
                    //    {                           

                    //        List<string> ss = new List<string>();
                    //        string number = "";
                    //        if (node.Parent.Text == "ASTLISTITEMS0") number = "0";
                    //        else if (node.Parent.Text == "ASTLISTITEMS1") number = "1";
                    //        else if (node.Parent.Text == "ASTLISTITEMS2") number = "2";
                    //        if (node.ChildrenCount() > 1)
                    //        {
                    //            G.Writeln2("*** ERROR: Unexpexted error #78963456");
                    //            throw new GekkoException();
                    //        }

                    //        ASTNode child = node[0];  //child is the variable, not the bank

                    //        string listNameCs = "o" + Num(child) + ".listItems" + number;

                    //        if (child.Text == "ASTNAMEWITHBANK")
                    //        {
                    //            //a
                    //            //b:a
                    //            node.Code.CA(listNameCs + ".AddRange(O.GetList(" + AstBankHelper(child, w, 1) + "));" + G.NL);
                    //        }
                    //        else if (child.Text == "ASTLISTITEMWILDRANGE")
                    //        {
                    //            //f*nz
                    //            node.Code.CA(listNameCs + ".Add(O.ConvertToString(" + child.Code + "));" + G.NL);
                    //        }
                    //        else if ((child.Text == "NEGATE" && child.ChildrenCount() > 0 && child[0].Text == "ASTNAMEWITHBANK"))
                    //        {
                    //            //-a
                    //            node.Code.CA(listNameCs + ".AddRange(O.GetList(" + AstBankHelper(child, w, 2) + "));" + G.NL);
                    //        }
                    //        else if (child.Text == "ASTLISTWITHBANK")
                    //        {
                    //            //bank2:#m, interpreted as bank2:m1, bank2:m2, bank2:m3, ...
                    //            node.Code.CA(listNameCs + ".AddRange(O.GetList(" + AstBankHelperList(child, w) + "));" + G.NL);
                    //        }
                    //        else
                    //        {
                    //            //expression, for instance
                    //            //'a'
                    //            //'b:a'
                    //            //#m[2]
                    //            node.Code.CA(listNameCs + ".AddRange(O.GetList(" + child.Code + "));" + G.NL);
                    //        }
                                                        
                    //        //Node always has one child here, so this is not used anymore
                    //        //string cs = null;
                    //        //if (node[0].Text == "ASTNAMEWITHBANK")
                    //        //{
                    //        //    cs = AstBankHelper(node[0], w, 1);
                    //        //}
                    //        //else
                    //        //{
                    //        //    cs = node[0].Code.ToString();
                    //        //}
                    //        //node.Code.A("o" + Num(node) + ".listItems" + number + " = O.AddBankToListItems(o" + Num(node) + ".listItems" + number + ", O.ConvertToString(" + cs + "));" + G.NL);
                            
                    //    }
                    //    break;
                    case "ASTLISTITEMS":
                        {
                            node.Code.A("o" + Num(node) + ".listItems = new List<string>();" + G.NL);
                            GetCodeFromAllChildren(node);                            
                        }
                        break;                    
                    case "ASTLISTITEMS0":
                        {
                            node.Code.A("o" + Num(node) + ".listItems0 = new List<string>();" + G.NL);
                            GetCodeFromAllChildren(node);
                        }
                        break;
                    case "ASTLISTITEMS1":
                        {
                            node.Code.A("o" + Num(node) + ".listItems1 = new List<string>();" + G.NL);
                            GetCodeFromAllChildren(node);
                        }
                        break;
                    case "ASTLISTITEMS2":
                        {
                            node.Code.A("o" + Num(node) + ".listItems2 = new List<string>();" + G.NL);
                            GetCodeFromAllChildren(node);
                        }
                        break;          
                    case "ASTLISTPREFIX":
                        {
                            //if (node.ChildrenCount() == 1 && node[0].Text == "ASTNAMEWITHBANK")
                            //{
                            //    node.Code.CA("o" + Num(node[0]) + ".listPrefix = O.ConvertToString(" + AstBankHelper(node[0], w, 1) + ");" + G.NL);
                            //}
                            //else
                            {
                                node.Code.A("o" + Num(node) + ".listPrefix = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                            }
                        }
                        break;
                    case "ASTLISTSUFFIX":
                        {
                            //if (node.ChildrenCount() == 1 && node[0].Text == "ASTNAMEWITHBANK")
                            //{
                            //    node.Code.CA("o" + Num(node[0]) + ".listSuffix = O.ConvertToString(" + AstBankHelper(node[0], w, 1) + ");" + G.NL);
                            //}
                            //else
                            {
                                node.Code.A("o" + Num(node) + ".listSuffix = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                            }
                        }
                        break;
                    case "ASTLISTSTRIP":
                        {
                            //if (node.ChildrenCount() == 1 && node[0].Text == "ASTNAMEWITHBANK")
                            //{
                            //    node.Code.CA("o" + Num(node[0]) + ".listStrip = O.ConvertToString(" + AstBankHelper(node[0], w, 1) + ");" + G.NL);
                            //}
                            //else
                            {
                                node.Code.A("o" + Num(node) + ".listStrip = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                            }
                        }
                        break;
                    case "ASTLISTSORT":
                        {
                            node.Code.A("o" + Num(node) + ".listSort = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        }
                        break;
                    case "ASTLISTTRIM":
                        {
                            node.Code.A("o" + Num(node) + ".listTrim = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                        }
                        break;
                    case "ASTM":
                        {
                            node.Code.A(AddPrintCode("m", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTV":
                        {
                            node.Code.A(AddPrintCode("v", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTMEM":
                        {
                            node.Code.A("Program.Mem(null);" + G.NL);
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
                    case "ASTTIMEQUESTION":
                        {                            
                            node.Code.A("O.Time.Q();" + G.NL);                            
                        }
                        break;     
                    case "ASTMP":
                        {
                            node.Code.A(AddPrintCode("mp", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    //case "ASTMULBK":
                    //    {
                    //        node.Code.A(Globals.clearTsCsCode + G.NL);
                    //        node.Code.A("O.Mulbk o" + Num(node) + " = new O.Mulbk();" + G.NL);
                    //        GetCodeFromAllChildren(node);
                    //        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    //    }
                    //    break;
                    case "ASTN":
                        {
                            node.Code.A(AddPrintCode("n", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTASSIGNMENT":
                        {
                            string type = HandleVar(node[3].Text);  //2 is options   
                            GetCodeFromAllChildren(node, node[2]);                         
                            if (G.Equal(type, "STRING2")) type = "string";
                            string ivTempVar = SearchUpwardsInTree4(node[0]);
                            if (ivTempVar == null)
                            {
                                G.Writeln2("*** ERROR: Internal error #7698248427");
                                throw new GekkoException();
                            }
                            string convertTo = null;

                            string temp = node[1].Code.ToString();

                            //string temp = null;
                            //if (node[4].Text == "=") temp = node[1].Code.ToString();
                            //else if (node[4].Text == "+=") temp = "O.Add(smpl, " + node[0].Code.ToString() + ", " + node[1].Code.ToString() + ")";
                            //else if (node[4].Text == "-=") temp = "O.Subtract(smpl, " + node[0].Code.ToString() + ", " + node[1].Code.ToString() + ")";
                            //else if (node[4].Text == "*=") temp = "O.Multiply(smpl, " + node[0].Code.ToString() + ", " + node[1].Code.ToString() + ")";
                            //else if (node[4].Text == "/=") temp = "O.Divide(smpl, " + node[0].Code.ToString() + ", " + node[1].Code.ToString() + ")";
                            //else
                            //{
                            //    G.Writeln2("*** ERRROR: Unknown assignment operator");
                            //    throw new GekkoException();
                            //}

                            if (type != null) node.Code.A("IVariable " + ivTempVar + " = O.IvConvertTo(EVariableType." + type + ", ").A(temp).A(")").End();
                            else  node.Code.A("IVariable " + ivTempVar + " = ").A(temp).End();

                            node.Code.A(node[0].Code).End();

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
                    case "ASTMAPDEF":
                        {
                            string funcName = "MapDef_" + node.mapTempVarName;
                            string s2 = "Map " + node.mapTempVarName + " = new Map();" + G.NL;
                            foreach (ASTNode child in node.ChildrenIterator()) s2 += child.Code.ToString();
                            string s = "public static IVariable " + funcName + "(GekkoSmpl smpl) {" + G.NL + s2 + G.NL + "return " + node.mapTempVarName + ";" + G.NL + "}";
                            w.headerCs.Append(s);                            
                            node.Code.A(funcName + "(smpl)");
                        }
                        break;
                    case "ASTMAPITEM":
                        {
                            GetCodeFromAllChildren(node);
                        }
                        break;

                    case "ASTBANKVARNAMELIST":
                        {
                            //In this case, if we override what has been made in subtree (where the items will produce O.Lookup(...)
                            //Instead, we convert it to a list of strings.
                            //See the structure under "ASTBANKVARNAME"
                            string code = "O.CreateListFromStrings(new string[] {";
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                bool fail = false;
                                if (child.Text != "ASTBANKVARNAME") fail = true; //should not be possible
                                string bankname = null; if (child?[0]?[0]?[0]?.Text != null && child?[0]?[0]?[0]?.Text != "ASTIDENT") fail = true; else bankname = child?[0]?[0]?[0]?[0]?.Text;
                                string sigil = child?[1]?[0]?[0]?.Text;
                                if (sigil == "ASTPERCENT") sigil = Globals.symbolScalar.ToString();
                                else if (sigil == "ASTHASH") sigil = Globals.symbolCollection.ToString();
                                string varname = null; if (child?[1]?[1]?[0]?[0]?.Text != null && child?[1]?[1]?[0]?[0]?.Text != "ASTIDENT") fail = true; else varname = child?[1]?[1]?[0]?[0]?[0]?.Text;
                                string freq = null; if (child?[1]?[2]?[0]?[0]?.Text != null && child?[1]?[2]?[0]?[0]?.Text != "ASTIDENT") fail = true; else freq = child?[1]?[2]?[0]?[0]?[0]?.Text;
                                if (fail)
                                {
                                    G.Writeln2("*** ERROR: List item is not a simple name compsed of bankname, varname and frequency");
                                    G.Writeln2("*** ERROR: Allowed examples: x, b:x, %x, #x, b:%x, b:x!q and similar simple names");
                                    throw new GekkoException();
                                }
                                string name = sigil + varname;
                                if (freq != null) name = name + Globals.freqIndicator + freq;
                                if (bankname != null) name = bankname + Globals.symbolBankColon + name;
                                code += "`" + name + "`, ";
                            }
                            code = code.Substring(0, code.Length - ", ".Length);
                            code += "})";
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


                            Tuple<bool, string> tuple = CheckIfLeftSide(node);  //In x[%s1, %s2][%date] = ... this will only be true for x, not for the other vars
                            bool isLeftSideVariable = tuple.Item1;
                            string type = tuple.Item2;

                            string isLeftSideVariableString = "false"; if (isLeftSideVariable) isLeftSideVariableString = "true";
                            bool isInsidePrintStatement = SearchUpwardsInTree5(node);

                            //string bankNumber = "0"; if (isInsidePrintStatement) bankNumber = Globals.bankNumberiName;


                            //if (sigil == Globals.symbolCollection.ToString())
                            //{
                            //    //naked #m
                            //    if (node.Parent.Text == "ASTINDEXERELEMENT")
                            //    {
                            //        //#m is inside a x[#m]
                            //        ASTNode node2 = node.Parent.Parent;
                            //        while (true)
                            //        {
                            //            if (node2 == null || node2.Text == null) break;
                            //            if (node2.Text.StartsWith("ASTPRTELEMENT"))
                            //            {
                            //                if (node2.freeIndexedLists == null) node2.freeIndexedLists = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            //                if (!node2.freeIndexedLists.ContainsKey(s)) node2.freeIndexedLists.Add(s, null);
                            //            }
                            //            node2 = node2.Parent;
                            //        }                                                    
                            //    }
                            //}


                            string s = GetSimpleName(node);
                            string internalName = null;
                            if (s != null) internalName = SearchUpwardsInTree3(node, s);
                            bool functionHit = false;
                            if (internalName != null)
                            {
                                node.Code.CA(internalName);
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

                                string ivTempVar = SearchUpwardsInTree4(node);
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
                                            simpleBank = "Ref";
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

                                    string hasSigilText = "false";
                                    if (sigil != null) hasSigilText = "true";
                                    string simpleBankText = Globals.QT + simpleBank + Globals.QT;
                                    if (simpleBank == "") simpleBankText = "null";
                                    string simpleFreqText = Globals.QT + simpleFreq + Globals.QT;
                                    if (simpleFreq == "") simpleFreqText = "null";



                                    string lookupCode = "O.Lookup(smpl, " + mapName + ", " + simpleBankText + ", " + Globals.QT + sigil + simpleName + Globals.QT + ", " + simpleFreqText + ", " + ivTempVar + ", " + isLeftSideVariableString + ", EVariableType." + type + ")";

                                    node.Code.CA(lookupCode);

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
                                        nameAndBankCode = "(" + bankNameCs + ")" + ".Add(smpl, O.scalarStringColon)" + ".Add(smpl, " + node[1].Code + ")";
                                    }

                                    node.Code.A("O.Lookup(smpl, " + mapName + ", " + nameAndBankCode + ", " + ivTempVar + ", " + isLeftSideVariableString + ", EVariableType." + type + ")");

                                }
                            }
                        }
                        break;
                        case "ASTPRINT":
                        {
                            if (true)
                            {
                                string listName = "m" + ++Globals.counter;  //for ultra-safety
                                string code = "List " + listName + " = null; try { " + listName + " = new List();" + G.NL;
                                code += "for (smpl." + Globals.bankNumberiName + " = 0; smpl." + Globals.bankNumberiName + " < " + Globals.bankNumberiMax + "; smpl." + Globals.bankNumberiName + "++) {" + G.NL;
                                //code += node[0].Code + ";" + G.NL;
                                code += listName + ".Add(" + node[0].Code + ");" + G.NL;
                                code += "}" + G.NL;  //end of for
                                code += "}" + G.NL;  //end of try
                                code += "finally" + G.NL;  //end of try
                                code += "{" + G.NL;
                                code += "smpl." + Globals.bankNumberiName + " = 0;" + G.NL;
                                code += "}" + G.NL;
                                node.Code.A(code);
                                if(false) node.Code.LoopSmplCode("O.Print(smpl, " + listName + ")");
                                node.Code.A("O.Print(smpl, " + listName + ");");
                            }
                            else { 

                                string code = null;
                                string funcName = "PrintHelper_" + ++Globals.counter;
                                string listName = "m" + ++Globals.counter;  //for ultra-safety
                                string methodCode = "public static List " + funcName + "(GekkoSmpl smpl) { try { List " + listName + " = new List(); for (smpl." + Globals.bankNumberiName + " = 0; smpl." + Globals.bankNumberiName + " < " + Globals.bankNumberiMax + "; smpl." + Globals.bankNumberiName + "++)";
                                methodCode += "{" + G.NL;
                                methodCode += "" + listName + ".Add(" + node[0].Code + ");" + G.NL;
                                methodCode += "}" + G.NL;
                                methodCode += "return " + listName + ";" + G.NL;
                                methodCode += "}" + G.NL;  //end of try
                                methodCode += "finally" + G.NL;  //end of try
                                methodCode += "{" + G.NL;
                                methodCode += "smpl." + Globals.bankNumberiName + " = 0;" + G.NL;
                                methodCode += "}" + G.NL;
                                methodCode += "}" + G.NL;  //end of method
                                w.headerCs.Append(methodCode);
                                if(false) node.Code.LoopSmplCode("O.Print(smpl, (" + funcName + "(smpl)" + "))");
                                node.Code.A("O.Print(smpl, (" + funcName + "(smpl)" + "));");
                            }
                        }
                        break;
                    case "ASTVARNAME":
                        {
                            bool s0 = false; if (node[0][0] != null) s0 = true;  //sigil % or #
                            bool s2 = false; if (node[2][0] != null) s2 = true;  //freq !
                            
                            if (s0)
                            {
                                if(s2)
                                {
                                    //%a!q, does not make sense...
                                    node.Code.A("(" + node[0][0].Code + ")").A(".Add(smpl, " + node[1][0].Code + ")").A(".Add(smpl, O.scalarStringTilde)").A(".Add(smpl, " + node[2][0].Code + ")");
                                }   
                                else
                                {
                                    //%a
                                    node.Code.A("(" + node[0][0].Code + ")").A(".Add(smpl, " + node[1][0].Code + ")");
                                }                         
                            }
                            else
                            {
                                if(s2)
                                {
                                    //a!q
                                    node.Code.A("(" + node[1][0].Code + ")").A(".Add(smpl, O.scalarStringTilde)").A(".Add(smpl, " + node[2][0].Code + ")");
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
                                    else node.Code.A(".Add(smpl, " + child.Code + ")");
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
                    case "ASTNAMEWITHDOT":
                        {
                            //Can only have 2 ASTNAME's as children
                            if (node.ChildrenCount() == 1)
                            {                                
                                //no dot in this name
                                node.Code.A(node[0].Code);
                                node.nameSimpleIdent = node[0].nameSimpleIdent;  //inherits this info
                            }
                            else if (node[1].Text == "ASTINTEGER")
                            {
                                //stuff like fy.1
                                if (Globals.useDotFunctionalityInParser)
                                {
                                    //for instance in fy.1                            
                                    node.Code.A(node[0].Code);
                                    node.nameSimpleIdent = node[0].nameSimpleIdent;  //inherits this info
                                    node.dotNumber = node[1].Code.ToString();
                                }
                                else
                                {
                                    G.Writeln2("*** ERROR: Problem with .1, .2, etc. -- please use [-1], [-2], etc.");
                                    throw new GekkoException();
                                    node.Code.A(node[0].Code);
                                    node.nameSimpleIdent = node[0].nameSimpleIdent;  //inherits this info
                                }

                            }
                            else
                            {
                                ////NOT ACTIVE AT THE MOMENT
                                ////NOT ACTIVE AT THE MOMENT (versions, like fY.sim1)
                                ////NOT ACTIVE AT THE MOMENT
                                //G.Writeln2("*** ERROR: name with versions (e.g. fY.s0) not allowed at this point");
                                //throw new GekkoException();
                                node.Code.A("O.Add(smpl, O.Add(smpl, " + node[0].Code + ", new ScalarString(`.`)), " + node[1].Code + ")");
                                if (node[0].nameSimpleIdent != null && node[1].nameSimpleIdent != null)
                                {
                                    node.nameSimpleIdent = node[0].nameSimpleIdent + "." + node[1].nameSimpleIdent;
                                    node.Code.CA("new ScalarString(`" + node.nameSimpleIdent + "`)");  //overrides
                                }                               
                            }
                        }
                        break;
                    //case "ASTNAMESLIST":
                    //    {
                    //        node.Code.A("o" + Num(node) + ".namesList = new List<string>();" + G.NL);
                    //        foreach (ASTNode child in node.ChildrenIterator())
                    //        {
                    //            node.Code.A("o" + Num(node) + ".namesList.Add(O.ConvertToString(" + child.Code + "));" + G.NL);
                    //        }
                    //    }
                    //    break;
                    //case "ASTLISTWITHBANK":
                    //    {
                    //        node.Code.A(AstBankHelperList(node, w));
                    //    }
                    //    break;
                    //case "ASTNAMEWITHBANK":
                    //    {
                    //        if (Globals.version24)
                    //        {

                    //        }
                    //        else
                    //        {

                    //            //Must always have 2 children, ASTBANK and ASTNAMEWITHDOT
                    //            string lagTypeCs = null;
                    //            if (node[1].Text == "ASTNAMEWITHDOT")  //probably is always so, but we check it.
                    //            {
                    //                if (Globals.useDotFunctionalityInParser)
                    //                {
                    //                    lagTypeCs = node[1].dotNumber;
                    //                }
                    //            }

                    //            if (node[0].ChildrenCount() == 0 && node[1].ChildrenCount() == 1 && node[1][0].Text == "ASTNAME" && node[1][0].ChildrenCount() == 1 && node[1][0][0].Text == "ASTSCALAR")
                    //            {
                    //                G.Writeln2("*** ERROR #24737643");
                    //                throw new GekkoException();
                    //                ////For instance this structure corresponding to "%b". This is interpreted as a VAL scalar even though it might be a STRING scalar pointing to a timeseries.
                    //                ////ASTNAMEWITHBANK
                    //                ////  ASTBANK
                    //                ////  ASTNAMEWITHDOT
                    //                ////    ASTNAME
                    //                ////      ASTSCALAR
                    //                ////        ASTPERCENTNAMESIMPLE
                    //                ////          b
                    //                //node[1][0][0].Code = null;  //sub-nodes have been visited: this result gets overridden
                    //                //HandleScalar(node[1][0][0], false, wh2);
                    //                //node.Code.CA(node[1][0][0].Code;                                
                    //            }
                    //            else
                    //            {

                    //                string code = AstBankHelper(node, w, 0);
                    //                if (Globals.useDotFunctionalityInParser && lagTypeCs != null)
                    //                {
                    //                    //This is a fY.1 type of variable.
                    //                    //Why does this work, and why is 'code' not used??
                    //                    node.Code.CA("O.Indexer(smpl, " + node.Code + ", false, " + lagTypeCs + ")");
                    //                }
                    //                else
                    //                {
                    //                    node.Code.A(code);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    break;
                    case "ASTNO":
                        {
                            node.Code.CA("new ScalarString(`no`)");
                        }
                        break;
                    case "ASTNEGATE":
                        {
                            node.Code.A("O.Negate(smpl, " + node.GetChildCode(0) + ")");
                        }
                        break;
                    case "ASTOPEN":
                        {
                            node.Code.A(Globals.clearTsCsCode + G.NL);
                            node.Code.A("O.Open o" + Num(node) + " = new O.Open();" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTLEFTSIDE":
                        {                            
                            string s = null;
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                s += child.Code + G.NL;
                            }
                            if(false) node.Code.LoopSmplCode(s);
                            node.Code.A(s);
                        }
                        break;
                    case "ASTOPENHELPER":
                        {
                            string as2 = null;
                            if (node[1].ChildrenCount() == 1) as2 = node[1][0].Text;                            
                            node.Code.A("o" + Num(node) + ".openFileNames.Add(new List<string>() {O.ConvertToString(" + node[0].Code + "), `" + as2 + "`});" + G.NL);
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
                            if (node.GetChild(0).Text == "?")
                            {
                                node.Code.A("Program.PrintOptions(`Program.options`);");
                            }
                            else
                            {
                                string o = "";
                                //Use GekkoStringBuilder??
                                StringBuilder s = new StringBuilder();
                                CreateOptionVariable(node, s, ref o);
                                node.Code.A(s.ToString());
                                if (o == "freq")
                                {
                                    node.Code.A(Globals.clearTsCsCode + G.NL);
                                    node.Code.A("Program.AdjustFreq();");
                                }
                                if (o == "interface_sound_type")
                                {
                                    if (!p.hasBeenCmdFile)
                                    {
                                        node.Code.A("Program.PlaySound();");
                                    }
                                }
                                if (o == "folder_menu" || o == "menu_startfile")
                                {
                                    node.Code.A("CrossThreadStuff.RestartMenuBrowser();");
                                }
                                if (o == "interface_zoom")
                                {
                                    node.Code.A("CrossThreadStuff.Zoom();");
                                }
                                if (o == "folder_working")
                                {                                    
                                    node.Code.A("CrossThreadStuff.WorkingFolder(``);");
                                }
                                if (o == "interface_remote")
                                {
                                    node.Code.A("Program.RemoteInit();");
                                }
                                if (o == "solve_gauss_reorder")
                                {
                                    node.Code.A("G.Writeln();");
                                    node.Code.A("G.Writeln(`+++ NOTE: Reorder: you must issue a MODEL statement afterwards, for this option to take effect.`);");
                                    node.Code.A("G.Writeln(`+++       (In command files, place this option before any MODEL statements).`);");
                                }
                                //if (o == "databank_file_format")
                                //{
                                //    node.Code.A("Globals.hasBeenTsdTsdxOptionChangeSinceLastClear = true;");
                                //}
                                if (o == "timefilter_type")  //TODO: only issue if really avg
                                {
                                    node.Code.A("G.Writeln();");
                                    node.Code.A("G.Writeln(`+++ NOTE: Timefilter type = 'avg' only works for PRT and MULPRT.`);");
                                }                                
                                if (o == "solve_forward_nfair_damp" || o == "solve_forward_fair_damp" || o == "solve_gauss_damp")
                                {
                                    node.Code.A("G.Writeln();");
                                    node.Code.A("G.Writeln(`+++ NOTE: Damping in Gekko 2.0 should be set to 1 minus damping in Gekko 1.8.`);");
                                }
                                if (o == "r_exe_path")
                                {
                                    node.Code.A("G.Writeln();");
                                    node.Code.A("G.Writeln(`+++ NOTE: Please use OPTION r exe folder ... instead`);");
                                }
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
                            node.Code.A(AddPrintCode("p", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTPCH":
                        {
                            node.Code.A(AddPrintCode("pch", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTABS":
                        {
                            node.Code.A(AddPrintCode("abs", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTDIF":
                        {
                            node.Code.A(AddPrintCode("dif", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTGDIF":
                        {
                            node.Code.A(AddPrintCode("gdif", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTLEV":
                        {
                            node.Code.A(AddPrintCode("lev", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTPERCENTNAMESIMPLE":
                        {
                            node.Code.CA("`" + node[0].Text + "`");
                        }
                        break;
                    case "ASTDOLLARPERCENTNAMESIMPLE":
                        {
                            node.Code.CA("`" + node[0].Text + "`");
                        }
                        break;
                    case "ASTPERCENTPAREN":
                        {
                            node.Code.CA("O.ConvertToString(" + node[0].Code + ")");
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
                    case "===ASTPRTROWS":
                        {
                            node.Code.CA(node[0].Code);
                        }
                        break;
                    case "ASTNEWTABLE":
                        {
                            node.Code.A("Program.CreateNewTable(O.ConvertToString(" + node[0].Code + "));" + G.NL);
                        }
                        break;
                    case "ASTTABLENEXT":
                        {
                            node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Next();");
                        }
                        break;
                    case "ASTTABLEPRINT":
                        {
                            if (node.ChildrenCount() > 1)
                            {
                                node.Code.A("Program.PrintTable(Program.GetTable(O.ConvertToString(" + node[0].Code + ")), O.ConvertToString(" + node[1].Code + "));" + G.NL);
                            }
                            else
                            {
                                node.Code.A("Program.PrintTable(Program.GetTable(O.ConvertToString(" + node[0].Code + ")), null);" + G.NL);
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
                            node.Code.A("o" + Num(node) + ".printcode = O.ConvertToString(" + node[5].Code + ");" + G.NL);
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
                            node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Set" + ss + "Border(O.ConvertToInt(" + node[2].Code + "), O.ConvertToInt(" + node[3].Code + "));" + G.NL);
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
                                node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Set" + ss + "Border(O.ConvertToInt(" + node[2].Code + "), O.ConvertToInt(" + node[3].Code + "));" + G.NL);                               
                            }
                            else
                            {
                                node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Set" + ss + "Border(O.ConvertToInt(" + node[2].Code + "));" + G.NL);                                                               
                            }
                        }
                        break;

                    case "ASTTABLEHIDELEFTBORDER":
                    case "ASTTABLEHIDERIGHTBORDER":
                        {                            
                            string ss = "Left";
                            if (node.Text == "ASTTABLEHIDERIGHTBORDER") ss = "Right";
                            node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Hide" + ss + "Border(O.ConvertToInt(" + node[2].Code + "));" + G.NL);
                        }
                        break;
                    case "ASTTABLESHOWBORDERS":
                        {                            
                            node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.ShowBorders();");
                        }                        
                        break;

                    case "ASTTABLESETTEXT":
                        {
                            node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.SetText(O.ConvertToInt(" + node[2].Code + "), O.ConvertToString(" + node[3].Code + "));");
                        }
                        break;

                    case "ASTTABLEALIGNLEFT":                        
                    case "ASTTABLEALIGNCENTER":                    
                    case "ASTTABLEALIGNRIGHT":
                        {
                            string type = "Left";
                            if (node.Text == "ASTTABLEALIGNCENTER") type = "Center";
                            else if (node.Text == "ASTTABLEALIGNRIGHT") type = "Right";
                            node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.Align" + type + "(O.ConvertToInt(" + node[2].Code + "));");
                        }
                        break;
                    case "ASTTABLEMERGECOLS":
                        {
                            node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.MergeCols(O.ConvertToInt(" + node[2].Code + "), O.ConvertToInt(" + node[3].Code + "));");
                        }
                        break;

                    case "ASTTABLESETDATES":
                        {                            
                            node.Code.A("Program.GetTable(O.ConvertToString(" + node[0].Code + ")).CurRow.SetDates(O.ConvertToInt(" + node[2].Code + "), O.ConvertToDate(" + node[3].Code + ", O.GetDateChoices.Strict), O.ConvertToDate(" + node[4].Code + ", O.GetDateChoices.Strict));");                            
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
                            node.Code.A("ope" + Num(node) + ".linewidth = O.ConvertToVal(" + node[0].Code + ", GekkoTime.tNull);" + G.NL);
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
                            node.Code.A("ope" + Num(node) + ".pointsize = O.ConvertToVal(" + node[0].Code + ", GekkoTime.tNull);" + G.NL);
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
                                    givenLabel = node.specialExpressionAndLabelInfo[2];
                                    givenLabel = Program.StripQuotes(givenLabel);
                                    givenLabel = Globals.labelCheatString + givenLabel;
                                }
                                else givenLabel = node.specialExpressionAndLabelInfo[1];                                
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
                                freelists = "|||" + freelists + "|||";
                            }

                            node.Code.A("ope" + Num(node) + ".label = O.SubstituteScalarsAndLists(`" + freelists + givenLabel + "`, false);" + G.NL);
                            node.Code.A("smpl = new GekkoSmpl(o" + Num(node) + ".t1.Add(-2), o" + Num(node) + ".t2);" + G.NL);
                            ASTNode child = node.GetChild("ASTPRTELEMENTOPTIONFIELD");
                            if (child != null) node.Code.A(child.Code);
                            
                            if (node.Text == "ASTPRTELEMENT")
                            {
                                node.Code.A("ope" + Num(node) + ".printCodesFinal = Program.GetElementPrintCodes(o" + Num(node) + ", ope" + Num(node) + ");");
                                node.Code.A("bankNumbers = O.Prt.GetBankNumbers(null, ope" + Num(node) + ".printCodesFinal);");
                                //node.Code.A("bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o" + Num(node) + ", ope" + Num(node) + "));" + G.NL);
                            }
                            else if (node.Text == "ASTTABLESETVALUESELEMENT")
                            {
                                node.Code.A("bankNumbers = O.Prt.GetBankNumbers(Globals.tableOption, new List<string>(){o" + Num(node) + ".printcode}" + ");" + G.NL);
                            }
                            node.Code.A("foreach(int bankNumber in bankNumbers) {" + G.NL);

                            node.Code.A("smpl.bankNumber = bankNumber;" + G.NL);
                            node.Code.A(EmitLocalCacheForTimeLooping(w));
                                                        
                            //node.Code.A("smpl" 
                            node.Code.A("ope" + Num(node) + ".variable[bankNumber] = " + node[0].Code + ";" + G.NL);
                            
                            node.Code.A("}" + G.NL);  //end of bankNumbers
                            node.Code.A("smpl.bankNumber = 0;" + G.NL);  //resetting, probably superfluous
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
                            //node.Code.A("o" + Num(node) + ".listItems = new List<string>();" + G.NL;
                            node.Code.A(node[0].Code);  //list1
                            //node.Code.A("o" + Num(node) + ".listItems1 = o" + Num(node) + ".listItems;" + G.NL;                            
                            //node.Code.A("o" + Num(node) + ".listItems = new List<string>();" + G.NL;
                            node.Code.A(node[1].Code);  //list2
                            if (node[2] != null) node.Code.A(node[2].Code); //options
                            //node.Code.A("o" + Num(node) + ".listItems2 = o" + Num(node) + ".listItems;" + G.NL;                            
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;                    
                    case "ASTEXPRESSIONTUPLE":  //see ASTRETURNTUPLE
                        {
                            int counter = 0;
                            List<string> types = new List<string>();

                            string classCs = G.GetVariableType(node.ChildrenCount());
                            //CreateTupleClass(w.uHeaderCs, node.ChildrenCount(), classCs, w.tupleClasses);

                            string tempCs = "temp" + ++Globals.counter;                            
                                                        
                            string s = null;
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                //counter++;                                
                                //node.Code.A(tempCs + ".tuple" + (counter - 1) + " = " + child.Code + ";" + G.NL;
                                s += child.Code + ", ";
                            }
                            if (s.EndsWith(", ")) s = s.Substring(0, s.Length - 2);

                            node.Code.CA("(new " + classCs + "(" + s + "))");

                            //node.Code.A(classCs + " " + tempCs + " = new " + classCs + "(" + s + ");" + G.NL;
                            //node.Code.A("return " + tempCs + ";" + G.NL;
                        }
                        break;
                    
                    case "ASTSIGN":
                        {
                            node.Code.A("Program.Sign();" + G.NL);
                        }
                        break;
                    case "ASTSIM":
                        {
                            node.Code.A("O.Sim o" + Num(node) + " = new O.Sim();" + G.NL);
                            //node.Code.A("o" + Num(node) + ".t1 = G.GetStartDate(o" + Num(node) + ".t1);" + G.NL;
                            //node.Code.A("o" + Num(node) + ".t2 = G.GetEndDate(o" + Num(node) + ".t2);" + G.NL;
                            GetCodeFromAllChildren(node);  //gets dates and options
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTRES":
                        {
                            node.Code.A("O.Res o" + Num(node) + " = new O.Res();" + G.NL);                            
                            GetCodeFromAllChildren(node);  //gets dates
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTSMOOTH":
                        {
                            node.Code.A("O.Smooth o" + Num(node) + " = new O.Smooth();" + G.NL);
                            node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                            //node.Code.A("o" + Num(node) + ".lhs = (" + node[0].Code + ");" + G.NL;
                            //node.Code.A("o" + Num(node) + ".rhs = (" + node[1].Code + ");" + G.NL;
                            //for (int i = 2; i < node.ChildrenCount(); i++)  //get the rest
                            //{
                            //    node.Code.A(node[i].Code;
                            //}
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTSPLICE":
                        {
                            node.Code.A("O.Splice o" + Num(node) + " = new O.Splice();" + G.NL);
                            node.Code.A(node[0].Code);
                            node.Code.A(node[1].Code);
                            node.Code.A(node[2].Code);                       
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
                            else if (node.ChildrenCount() == 2) {
                                node.Code.A(tempName + ".from = O.ConvertToDate(" + node[0].Code + ", O.GetDateChoices.Strict);" + G.NL);
                                node.Code.A(tempName + ".to = O.ConvertToDate(" + node[1].Code + ", O.GetDateChoices.Strict);" + G.NL);                                                                
                            }
                            else if (node.ChildrenCount() == 3) {
                                node.Code.A(tempName + ".from = O.ConvertToDate(" + node[0].Code + ", O.GetDateChoices.Strict);" + G.NL);
                                node.Code.A(tempName + ".to = O.ConvertToDate(" + node[1].Code + ", O.GetDateChoices.Strict);" + G.NL);
                                node.Code.A(tempName + ".step = O.ConvertToDate(" + node[2].Code + ");" + G.NL);
                            }
                            node.Code.A("o" + Num(node) + ".timeFilterPeriods.Add(" + tempName + ");" + G.NL);
                        }
                        break;
                    case "ASTQ":
                        {
                            node.Code.A(AddPrintCode("q", node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    //case "ASTTRIMVARS":
                    //    {
                    //        node.Code.A("Program.Trimvars();" + G.NL);
                    //    }
                    //    break;

                    case "ASTDECOMP":
                        {
                            //string cs = helper.decompExpressionCs.Replace("`", "\\\"");
                            //cs = cs.Replace(G.NL, "");
                            //node.Code.A("Program.Decomp(`" + helper.decompType.ToLower() + "`, " + helper.timePeriod + ", `" + helper.prtOption + "`, `" + cs + "`, precedents, expression);");                            
                            node.Code.A("O.Decomp o" + Num(node) + " = new O.Decomp();" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);

                        }
                        break;
                    case "ASTDECOMPITEMS":
                        {                            
                            ASTNode child = node[0];
                            if (child.Text == "ASTNAMEWITHBANK")
                            {
                                //Same logic as ASTLISTITEMS, where "LIST a" is parsed as an expression but translated into 'a' in the parser.
                                //node.Code.A("o" + Num(node) + ".variable = O.ConvertToString(" + AstBankHelper(child, w, 1) + ");" + G.NL);

                            //TODO: implement this

                            }
                            else
                            {
                                G.Writeln2("*** ERROR: Sorry, decomposition of expressions not yet implemented in Gekko 2.0");
                                throw new GekkoException();
                                //node.Code.A("o" + Num(node) + ".expressionCs = " + Globals.QT + child.Code.ToString().Replace("\"", "\\\"") + Globals.QT + ";" + G.NL);
                                string sss = child.Code.ToString();
                                node.Code.A("o" + Num(node) + ".expressionCs = " + Globals.QT + sss.Replace("`", "\\\"") + Globals.QT + ";" + G.NL);
                            }                            
                        }
                        break;
                    case "ASTUNFIX":
                        node.Code.A("Program.Unfix();" + G.NL);
                        break;
                    case "ASTUNSWAP":
                        {
                            node.Code.A("Program.Unswap();" + G.NL);
                        }
                        break;
                    case "ASTVERS":
                        {
                            node.Code.A("Program.Vers();" + G.NL);
                        }
                        break;
                    //case "ASTUPDPRT":
                    //    {
                    //        node.Code.A("O.Updprt o" + Num(node) + " = new O.Updprt();" + G.NL);                            
                    //        node.Code.A(node[0].Code);
                    //        node.Code.A(node[1].Code);
                    //        node.Code.A("o" + Num(node) + ".op = `" + node[2].Code + "`;" + G.NL);                            
                    //        if (node.ChildrenCount() > 3)
                    //        {
                    //            node.Code.A(node[3].Code);
                    //        }
                    //        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    //    }
                    //    break;
                    case "ASTPIPE":
                        {                            
                            node.Code.A("O.Pipe o" + Num(node) + " = new O.Pipe();" + G.NL);                            
                            GetCodeFromAllChildren(node);  //gets fileName and options
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTREAD":
                        {
                            node.Code.A(Globals.clearTsCsCode + G.NL);
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
                                node.Code.A("o" + Num(node) + ".readTo = `" + node[0].Text + "`;");
                            }                            
                        }
                        break;
                    //case "ASTIMPORTTO":
                    //    {
                    //        if (node.ChildrenCount() > 0)
                    //        {
                    //            node.Code.A("o" + Num(node) + ".importTo = `" + node[0].Text + "`;");
                    //        }
                    //    }
                    //    break;
                    case "ASTWRITE":
                        {                            
                            node.Code.A("O.Write o" + Num(node) + " = new O.Write();" + G.NL);
                            node.Code.A("o" + Num(node) + ".type = @`" + node[0].Text + "`;");
                            GetCodeFromAllChildren(node, node[1]);  //options
                            node.Code.A("o" + Num(node) + ".fileName = " + node[2].Code + ";" + G.NL);
                            if(!node[3].Code.IsNull()) node.Code.A("o" + Num(node) + ".list = " + node[3].Code + ";" + G.NL);                            
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTFLEXIBLELIST":
                    case "ASTNAMESLIST":
                        {                            
                            GetCodeFromAllChildren(node);
                        }
                        break;
                    case "ASTSHEETIMPORT":
                        {
                            node.Code.A("O.SheetImport o" + Num(node) + " = new O.SheetImport();" + G.NL);
                            GetCodeFromAllChildren(node);                            
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTX12A":
                        {
                            node.Code.A("O.X12a o" + Num(node) + " = new O.X12a();" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTR_FILE":
                        {
                            node.Code.A("O.R_file o" + Num(node) + " = new O.R_file();" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTR_EXPORT":
                        {
                            node.Code.A("O.R_export o" + Num(node) + " = new O.R_export();" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTR_RUN":
                        {
                            node.Code.A("O.R_run o" + Num(node) + " = new O.R_run();" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTR_EXPORTITEMS":
                        {
                            //G.Writeln2(node.Text);                            
                            string s2 = "";
                            bool first = true;
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                if (first) s2 += "`" + child.Text + "`";
                                else s2 += ", `" + child.Text + "`";
                                first = false;
                            }
                            node.Code.A("o" + Num(node) + ".r_exportItems = new List<string>() {" + s2 + "};" + G.NL);
                        }
                        break;
                    case "ASTTRANSLATE":
                        {
                            node.Code.A("O.Translate o" + Num(node) + " = new O.Translate();" + G.NL);
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    //case "ASTEXPORT":
                    //    {
                    //        node.Code.A("O.Export o" + Num(node) + " = new O.Export();" + G.NL);
                    //        GetCodeFromAllChildren(node);
                    //        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    //    }
                    //    break;
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
                            node.Code.A("O.Run o" + Num(node) + " = new O.Run();" + G.NL);
                            //HMMM is this right:
                            node.Code.A("o" + Num(node) + ".fileName = O.ConvertToString(" + node[0].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    //case "ASTRUN":
                    //    {
                    //        node.Code.A("O.Run o" + Num(node) + " = new O.Run();" + G.NL);
                    //        //HMMM is this right:
                    //        node.Code.A("o" + Num(node) + ".fileName = " + node[0].Code + ".GetString();" + G.NL);
                    //        node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                    //        node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                    //    }
                    //    break;
                    case "ASTLIBRARY":
                        {
                            if(node.Number != 0)
                            {
                                G.Writeln2("*** ERROR: The LIBRARY command must be the first command in a command file");
                                throw new GekkoException();
                            }
                            string libName = node[0].Text;  //simple ident name
                            O.Library o = new O.Library();
                            o.fileName = libName;
                            o.p = p;
                            o.Exe();                            
                        }
                        break;
                    //case "ASTSCALAR":
                    //    {
                    //        HandleScalar(node, false, w);                            
                    //    }
                    //    break;
                    case "ASTSTAMP":
                        {
                            node.Code.A("Program.Stamp();" + G.NL);
                        }
                        break;
                    case "ASTSTRINGINQUOTES":
                        {
                            string s = Program.StripQuotes(node[0].Text);
                            //for instance, @"this is a ""word"" shown", where "" are kind of @-escaped.
                            //but @ will keep backslashes.
                            s = s.Replace("\"", "\"\"");
                            node.Code.CA("new ScalarString(ScalarString.SubstituteScalarsInString(@`" + s + "`, true, false))");
                        }
                        break;
                    case "ASTSTRING":
                    case "ASTNAME2":
                        {
                            if (node[0].Text == "?")
                            {
                                if (node.ChildrenCount() > 1)
                                {
                                    node.Code.A("O.String2.Q(`" + node[1].Text + "`);" + G.NL);
                                }
                                else
                                {
                                    if (node.Text == "ASTNAME2") node.Code.A("Program.Mem(`name`);" + G.NL);
                                    else node.Code.A("Program.Mem(`string`);" + G.NL);
                                }
                            }
                            else
                            {
                                bool x = false;
                                if (node.Text == "ASTNAME2") x = true;
                                string nodeCode = HandleString(node, node[1].Code.ToString(), x);
                                node.Code.A(nodeCode);
                            }
                        }
                        break;
                    case "ASTTRUNCATE":
                        {
                            node.Code.A("O.Truncate o" + Num(node) + " = new O.Truncate();" + G.NL);
                            node.Code.A(node[0].Code);  //options
                            //node.Code.A("o" + Num(node) + ".listItems = new List<string>();" + G.NL;
                            node.Code.A(node[1].Code);  //list1                            
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTCOMPARECOMMAND":
                        {
                            node.Code.A("O.Compare o" + Num(node) + " = new O.Compare();" + G.NL);                            
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTDISP":
                        {                            
                            node.Code.A("O.Disp o" + Num(node) + " = new O.Disp();" + G.NL);                            
                            node.Code.A(node[0].Code);  //dates
                                                        
                            node.Code.A("o" + Num(node) + ".iv = " + node[1].Code + ";" + G.NL);

                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);                            
                        }
                        break;
                    case "ASTINI":
                        {
                            node.Code.A("Program.Ini(p);");
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
                            node.Code.A(node[1].Code);  //list                            
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTDISPSEARCH":
                        {
                            node.Code.A("O.Disp o" + Num(node) + " = new O.Disp();" + G.NL);
                            node.Code.A("o" + Num(node) + ".searchName = `" + Program.StripQuotes(node[0].Text) + "`;" + G.NL);                            
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTPAUSE":
                        {                            
                            if (node.ChildrenCount() == 0) node.Code.A("Program.Pause(``);");
                            else node.Code.A("Program.Pause(O.ConvertToString(" + node[0].Code + "));");
                            break;
                        }
                        break;
                    case "ASTEFTER":
                        {
                            node.Code.A("O.Efter o" + Num(node) + " = new O.Efter();" + G.NL);
                            node.Code.A(node[0].Code);  //dates                            
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTUPD":
                        {
                            //TODO
                            //TODO hmm what about $ operator??
                            //TODO and what about 'xx' prefix? Maybe allow this for GENR too!
                            //TODO
                            //node.Code.A("O.Upd o" + Num(node) + " = new O.Upd();" + G.NL);
                            node.Code.A("O.Upd o").A(Num(node)).A(" = new O.Upd();").A(G.NL);                            
                            node.Code.A("o" + Num(node) + ".p = p;" + G.NL);

                            if (node.Parent != null && node.Parent.Text == "ASTMETA" && node.Parent.specialExpressionAndLabelInfo != null && node.Parent.specialExpressionAndLabelInfo.Length > 1)
                            {
                                //specialExpressionAndLabelInfo[0] should be "ASTMETA" here
                                node.Code.A("o").A(Num(node)).A(".meta = @`").A(node.Parent.specialExpressionAndLabelInfo[1]).A("`;").A(G.NL);
                            }
                            node.Code.A(node[0].Code);  //listItems
                            node.Code.A(node[1].Code);  //dates
                            string op = node[2].Code.ToString();
                            int nn = 3;
                            int n = node.ChildrenCount() - nn;
                            node.Code.A("o").A(Num(node)).A(".op = ").A(op).A(";").A(G.NL);
                            node.Code.A("o").A(Num(node)).A(".data = new double[").A(n).A("];").A(G.NL);
                            node.Code.A("o").A(Num(node)).A(".rep = new double[").A(n).A("];").A(G.NL);
                            for (int i = 0; i < n; i++)
                            {
                                node.Code.A("o").A(Num(node)).A(".data[").A(i).A("] = (").A(node[i + nn][0].Code).A(").GetVal(" + Globals.smpl + ");").A(G.NL);
                                string repCs = "new ScalarVal(1d)";
                                bool one = false;
                                if (node[i + nn].ChildrenCount() > 1)
                                {
                                    ASTNode rep = node[i + nn][1];
                                    repCs = node[i + nn][1].Code.ToString();
                                    if (rep.Text == "ASTSTAR")
                                    {
                                        repCs = "new ScalarVal(-12345d)";  //secret code for '*'                                        
                                    }
                                    else if (rep.Text == "ASTEMPTY")
                                    {
                                        one = true;
                                    }
                                }
                                else
                                {
                                    one = true;
                                }
                                if (one)
                                {
                                    node.Code.A("o").A(Num(node)).A(".rep[").A(i).A("] = 1d;").A(G.NL);
                                }
                                else
                                {
                                    node.Code.A("o").A(Num(node)).A(".rep[").A(i).A("] = (").A(repCs).A(").GetVal(" + Globals.smpl + ")").A(G.NL);
                                }
                            }                            
                            node.Code.A("o").A(Num(node)).A(".Exe();").A(G.NL);
                            //G.Writeln(node.Code);
                        }
                        break;
                    case "ASTUPDOPERATOR":
                        {
                            node.Code.A("`" + node[0].Code + "`");
                        }
                        break;
                    case "ASTVAL":
                        {
                            if (node[0].Text == "?")
                            {
                                if (node.ChildrenCount() > 1)
                                {
                                    node.Code.A("O.Val.Q(`" + node[1].Text + "`);" + G.NL);
                                }
                                else
                                {
                                    node.Code.A("Program.Mem(`val`);" + G.NL);
                                }
                            }
                            else
                            {
                                if (Globals.version24)
                                {
                                    string s1 = node[0].Code.ToString();
                                    string s2 = node[1].Code.ToString();

                                    node.Code.A("O.Assign(" + s1 + ", " + s2 + ", " + "EVariableType.Val");


                                }
                                else
                                {
                                    node.Code.A(HandleVal(node, node[1].Code.ToString(), w));
                                }
                            }
                        }
                        break;
                    case "ASTMETA":
                        {
                            GetCodeFromAllChildren(node);
                        }
                        break;
                    case "ASTSHOW":
                        {
                            string givenLabel = null;
                            if (node.specialExpressionAndLabelInfo[2] != "")
                            {
                                givenLabel = node.specialExpressionAndLabelInfo[2];
                                givenLabel = Program.StripQuotes(givenLabel);
                            }
                            else givenLabel = node.specialExpressionAndLabelInfo[1];                            
                            
                            node.Code.A("O.Show o" + Num(node) + " = new O.Show();" + G.NL);

                            node.Code.A("o" + Num(node) + ".input = " + node[0].Code + ";" + G.NL);
                            node.Code.A("o" + Num(node) + ".label = @`" + givenLabel + "`;" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTTUPLE":
                        {
                            string rhsCode = node[1].Code.ToString();

                            if (node[1].Text == "ASTFUNCTION" && (G.Equal(node[1][0].Text.ToLower(), "laspchain") || G.Equal(node[1][0].Text.ToLower(), "laspfixed")))
                            {
                                //hack to make it work. Problem is that method cannot run year-by-year.
                                if (node[0].ChildrenCount() != 2)
                                {
                                    G.Writeln2("laspchain() and laspfixed() must be called with (series x, series y) on left side");
                                    throw new GekkoException();
                                }
                                //string cs1 = "IVariable p" + Num(node) + "= " + node[0][0][2].Code + ";";
                                //string cs2 = "IVariable q" + Num(node) + " = " + node[0][1][2].Code + ";";
                                //string cs3 = "GekkoTuple.Tuple2 temp = " + rhsCode + ";";
                                node.Code.A("Functions.HELPER_HandleLasp(" + rhsCode + ", " + node[0][0][2].Code + ", " + node[0][1][2].Code + ");" + G.NL);                            
                            }
                            else
                            {


                                string tempName = "temp" + ++Globals.counter;
                                string nodeCodeTemp = null;
                                int number = 0;
                                foreach (ASTNode child in node[0].ChildrenIterator())
                                {
                                    if (child.Text != "ASTTUPLEITEM")
                                    {
                                        G.Writeln2("*** ERROR #74343641");
                                        throw new GekkoException();
                                    }
                                    number++;

                                    if (child[0].Text == "val")
                                    {
                                        nodeCodeTemp += HandleVal(child[1], tempName + ".tuple" + (number - 1), w);
                                    }
                                    else if (child[0].Text == "date")
                                    {
                                        nodeCodeTemp += HandleDate(child[1], tempName + ".tuple" + (number - 1));
                                    }
                                    else if (child[0].Text == "name")
                                    {
                                        nodeCodeTemp += HandleString(child[1], tempName + ".tuple" + (number - 1), true);
                                    }
                                    else if (child[0].Text == "string")
                                    {
                                        nodeCodeTemp += HandleString(child[1], tempName + ".tuple" + (number - 1), false);
                                    }
                                    else if (child[0].Text == "series")
                                    {
                                        ClearLocalStatementCache(w);
                                        nodeCodeTemp += HandleGenr(child, Num(child), child[1].Code.ToString(), child[2].Code.ToString(), tempName + ".tuple" + (number - 1), w, null);
                                    }
                                    else if (child[0].Text == "list")
                                    {
                                        string s = "o" + Num(child[1]) + ".listItems.AddRange(O.GetList(" + tempName + ".tuple" + (number - 1) + "));" + G.NL;
                                        nodeCodeTemp += HandleList(child[1], s);
                                    }
                                }

                                string className = G.GetVariableType(number);
                                node.Code.A(className + " " + tempName + " = " + rhsCode + ";" + G.NL);  //for instance "ScalarVal_ScalarVal temp117 = f()"                            
                                node.Code.A(nodeCodeTemp);
                            }
                            
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
                                else s += ".Add(smpl, " + child.Code + ")";
                                first = false;
                            }
                            //node.Code.CA("O.IndexerAlone(" + s + ")";
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
                    case "ASTUPDOPERATOREQUAL":
                        {
                            node.Code.CA("=");
                        }
                        break;
                    case "ASTUPDOPERATORSTAR":
                        {
                            node.Code.CA("*");
                        }
                        break;
                    case "ASTUPDOPERATORHAT":
                        {
                            node.Code.CA("^");
                        }
                        break;
                    case "ASTUPDOPERATORPLUS":
                        {
                            node.Code.CA("+");
                        }
                        break;
                    case "ASTUPDOPERATORHASH":
                        {
                            node.Code.CA("#");
                        }
                        break;
                    case "ASTUPDOPERATORPERCENT":
                        {
                            node.Code.CA("%");
                        }
                        break;
                    case "ASTUPDOPERATOREQUALDOLLAR":
                        {
                            node.Code.CA("=$");
                        }
                        break;
                    case "ASTUPDOPERATORSTARDOLLAR":
                        {
                            node.Code.CA("*$");
                        }
                        break;
                    case "ASTUPDOPERATORHATDOLLAR":
                        {
                            node.Code.CA("^$");
                        }
                        break;
                    case "ASTUPDOPERATORPLUSDOLLAR":
                        {
                            node.Code.CA("+$");
                        }
                        break;
                    case "ASTUPDOPERATORHASHDOLLAR":
                        {
                            node.Code.CA("#$");
                        }
                        break;
                    case "ASTUPDOPERATORPERCENTDOLLAR":
                        {
                            node.Code.CA("%$");
                        }
                        break;                   

                }
            }  //end of switch on node.Text AFTER sub-nodes are done
            
            if (relativeDepth == 1)
            {
                //#982375: if it is 0, walk the sub-tree to see...                  
                if (!w.wh.isGotoOrTarget)
                {
                    //string target = "Target" + ++Globals.counter;
                    //node.Code.CA("p.SetText(@`¤" + node.Line + "`); " + Globals.gekkoSmplInitCommand + G.NL + target + ":" + G.NL + node.Code + G.NL + "if (smpl.HasError()) { O.TryNewSmpl(smpl); goto " + target + ";}"); //so errors get line numbers. Hmm with A() instead of CA() we get the command run 2 times...  //init the smpl for every command (this excludes IF, FOR, etc.? never mind).                    
                    node.Code.CA("p.SetText(@`¤" + node.Line + "`); " + Globals.gekkoSmplInitCommand + G.NL + w.wh?.localFuncs?.ToString() + G.NL + node.Code);

                }

                if (Program.options.system_code_split > 0)
                {
                    //See also #890752345
                    if (node.Text == "ASTRETURN" || node.Text == "ASTGOTO" || node.Text == "ASTTARGET" || node.Text == "ASTFUNCTIONDEF" || node.Text == "ASTFORNAME" || node.Text == "ASTFORSTRING" || node.Text == "ASTFORVAL" || node.Text == "ASTFORDATE" || node.Text == "ASTIF")
                    {
                        //skip split markers for these, but 
                    }
                    else
                    {
                        node.Code.Prepend(G.NL + Globals.splitCommandBlockStart + G.NL);
                        node.Code.A(G.NL + Globals.splitCommandBlockEnd + G.NL);
                    }
                }
            }            

            if (node != null && absoluteDepth == 0)
            {
                //returning from the whole tree                
                node.Code.A(Globals.splitSTART);
                foreach (ASTNode child in node.ChildrenIterator())
                {                    
                    node.Code.A(child.Code + G.NL);                
                }
                node.Code.A(Globals.splitSTOP);
            }
        }

        private static string GekkoSmplCommandHelper2(int smplCommandNumber)
        {
            string s2 = "smpl.command = smplCommandRemember" + smplCommandNumber + ";";
            return s2;
        }

        private static string GekkoSmplCommandHelper1(int smplCommandNumber, string ss)
        {
            string s = "var smplCommandRemember" + smplCommandNumber + " = smpl.command; smpl.command = GekkoSmplCommand." + ss + ";";
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
            if (s == "Series") s = "Var";  //This is done because we otherwise get errors if rhs if for instance a list.
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
                    G.Writeln2("*** ERROR: At present, only scalar variables (%) are allowed as FOR loop variablse");
                    throw new GekkoException();
                }

                varname = G.AddSigil(varname, EVariableType.Val);  //will add % to VAL/STRING/DATE if missing.
                                
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

        private static void GetCommaCodeFromAllChildren(ASTNode node)
        {
            for (int i = 0; i < node.ChildrenCount(); i++)
            {
                node.Code.A(node[i].Code);
                if (i < node.ChildrenCount() - 1) node.Code.A(", ");
            }
        }

        private static string EmitListLoopingCode(ASTNode node, KeyValuePair<string, string> kvp)
        {
            string nameCs = GetLoopNameCs(node, kvp.Key);
            string nodeCode = "foreach (ScalarString " + nameCs + " in new O.GekkoListIterator(" + kvp.Value + ")) {" + G.NL;
            return nodeCode;
        }

        private static string[] IsGamsSumFunctionOrUnfoldFunction(ASTNode node, string functionName)
        {
            //returns null if it is NOT a GAMS-like sum() function
            string[] rv = null;
            if (G.Equal(functionName, "sum") || G.Equal(functionName, "unfold")) rv = GetListnames(node);

            if (rv == null)
            {
                //success, do nothing
            }
            else
            {                
                if (G.Equal(functionName, "sum)"))
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
                if (node.ChildrenCount() == 3)
                {
                    if (node[1].Text == "ASTLISTDEF")
                    {
                        //TODO: CHECK types of rv[i], are they all simple #i, #j, ...?

                        rv = new string[node[1].ChildrenCount()];
                        for (int i = 0; i < node[1].ChildrenCount(); i++)
                        {
                            rv[i] = GetSimpleHashName(node[1][i]);
                        }
                    }
                    else
                    {
                        rv = new string[1];
                        rv[0] = GetSimpleHashName(node[1]);
                    }
                }
            }

            return rv;
        }

        //private static void HandleGamsLikeSumFunction(string[] listNames, bool firstTime, W w, string code)
        //{
        //    if (firstTime)
        //    {
        //        //Add the item
        //        if (w.wh.sumHelperListNames == null) w.wh.sumHelperListNames = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        //        w.wh.sumHelperListNames.Add(listNames[0], code);
        //    }
        //    else
        //    {
        //        //Remove the item
        //        if (w.wh.sumHelperListNames == null)
        //        {
        //            G.Writeln2("*** ERROR: Internal error #7986432523");
        //            throw new GekkoException();
        //        }
        //        if (!w.wh.sumHelperListNames.ContainsKey(listNames[0]))
        //        {
        //            G.Writeln2("*** ERROR: Internal error #7986432524");
        //            throw new GekkoException();
        //        }
        //        w.wh.sumHelperListNames.Remove(listNames[0]);
        //    }

        //}



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
            string functionName = node[0][0].Text.ToLower();  //no string composition allowed for functions, it is simple ident.
            if (functionName == "string") functionName = "tostring";
            return functionName;
        }

        private static string GetFunctionName2(ASTNode node)
        {
            string functionName = node[1][0].Text.ToLower();  //no string composition allowed for functions, it is simple ident.
            if (functionName == "string") functionName = "tostring";
            return functionName;
        }

        private static void SearchUpwardsInTree(ASTNode node, out int timeLoopDepth, out ASTNode parentTimeLoop)
        {
            timeLoopDepth = 0;
            ASTNode tmp = node.Parent;
            parentTimeLoop = null;
            while (tmp != null)
            {
                bool ok = false;

                if (

                       (tmp.Text == "ASTFUNCTION" && (Globals.lagFunctions.Contains(tmp[0].Text.ToLower()) || tmp[0].Text.ToLower() == "sum"))  //check that sum is gams-like
                    || tmp.Text == "ASTOLSELEMENT"
                    || tmp.Text == "ASTPRTELEMENT"
                    || tmp.Text == "ASTTABLESETVALUESELEMENT"
                    || tmp.Text == "ASTSERIES"
                    || tmp.Text == "ASTGENR"
                    || tmp.Text == "ASTGENRLHSFUNCTION"
                    || tmp.Text == "ASTGENRLISTINDEXER"
                    || tmp.Text == "ASTRETURNTUPLE"
                    || (G.Equal(tmp.Text, "series") && (tmp.Parent != null && tmp.Text == "ASTTUPLEITEM") && (tmp.Parent.Parent != null && tmp.Parent.Text == "ASTTUPLE"))

                    ) ok = true;

                if (ok)
                {
                    timeLoopDepth++;
                    if (parentTimeLoop == null) parentTimeLoop = tmp;  //only first one
                }
                tmp = tmp.Parent;
            }
        }

        private static string SearchUpwardsInTree2(ASTNode node, string listName)
        {            
            ASTNode tmp = node.Parent;
            string rv = null;         
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
            //returns null if RHS or (LHS and there is a ASTBANKVAR or ASTDOTORINDEXER above.
            ASTNode tmp = node;            
            string rv = null;
            while (tmp != null)
            {
                if (tmp.ivTempVarName != null)
                {
                    rv = tmp.ivTempVarName;
                    break;
                }                
                tmp = tmp.Parent;
                if (tmp != null && (tmp.Text == "ASTBANKVAR" || tmp.Text == "ASTDOTORINDEXER")) return null;  //if any parent is like this, null is returned. We want to find the one highest in the tree.
            }
            return rv;
        }

        private static bool SearchUpwardsInTree5(ASTNode node)
        {
            //finds out if the variable is inside a PRINT statement         
            ASTNode tmp = node;
            string rv = null;
            while (tmp != null)
            {
                if (tmp.Text == "ASTPRINT") return true;
                tmp = tmp.Parent;
            }
            return false;
        }

        private static void ResetUFunctionHelpers(W w)
        {
            w.uFunctionsHelper = null;  //do not remove this line: important!      
            w.uListCache = new Gekko.GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            w.uScalarCache = new Gekko.GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            w.uTsCache = new Gekko.GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        private static StringBuilder GetHeaderCs(W w)
        {
            StringBuilder destination = null;
            if (w.uFunctionsHelper != null) destination = w.uFunctionsHelper.headerCs;
            else destination = w.headerCs;
            return destination;
        }

        //private static void AddSplitMarkers(ASTNode node)
        //{
        //    if (Globals.newSplit)
        //    {
        //        node.Code.Prepend(Globals.splitSTART);
        //        node.Code.A(Globals.splitSTOP);
        //    }
        //}

        private static void AstListHelper(ASTNode node, W w, string simpleIdent, bool stringify)
        {

            node.Code.CA(""); //clearing

            string nameCode = null;
            string listName = simpleIdent;

            //trying both left-side of SERIES indexer and sum function
            string found = null; if (w.wh.seriesHelperListNames != null) w.wh.seriesHelperListNames.TryGetValue(listName, out found);
            if(found == null) if (w.wh.sumHelperListNames != null) w.wh.sumHelperListNames.TryGetValue(listName, out found);

            if (found != null)  //not found
            {
                nameCode = GetLoopNameCs(node, listName);
                node.Code.A(nameCode);
            }
            else
            {                
                string stringifyString = "false"; if (stringify) stringifyString = "true";
                GekkoDictionary<string, string> listCache = GetListCache(w);
                string s = null; listCache.TryGetValue(simpleIdent, out s);
                if (s == null)
                {
                    //has not been seen before
                    string listWithNumber = "list" + ++Globals.counter;
                    listCache.Add(simpleIdent, listWithNumber);
                    GetHeaderCs(w).AppendLine("public static IVariable " + listWithNumber + " = null;");  //cannot set it to ScalarVal since it may change type...
                    node.Code.A("O.GetScalarFromCache(ref " + listWithNumber + ", `" + Globals.symbolCollection + simpleIdent + "`, false, " + stringifyString + ")");
                }
                else
                {
                    node.Code.A("O.GetScalarFromCache(ref " + s + ", `" + Globals.symbolCollection + simpleIdent + "`, false, " + stringifyString + ")");
                }
            }
        }

        private static GekkoDictionary<string, string> GetListCache(W w)
        {
            GekkoDictionary<string, string> listCache = null;
            if (w.uFunctionsHelper != null) listCache = w.listCache;
            else listCache = w.uListCache;
            return listCache;
        }

        private static GekkoDictionary<string, string> GetScalarCache(W w)
        {
            GekkoDictionary<string, string> scalarCache = null;
            if (w.uFunctionsHelper != null) scalarCache = w.scalarCache;
            else scalarCache = w.uScalarCache;
            return scalarCache;
        }

        private static GekkoDictionary<string, string> GetTsCache(W w)
        {
            GekkoDictionary<string, string> tsCache = null;
            if (w.uFunctionsHelper != null) tsCache = w.tsCache;
            else tsCache = w.uTsCache;
            return tsCache;
        }

        private static void ClearLocalStatementCache(W w)
        {
            w.wh.localStatementCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        private static string HandleList(ASTNode node, string childCode)
        {
            string nodeCode = null;
            nodeCode += "O.List o" + Num(node) + " = new O.List();" + G.NL;
            nodeCode = HandleListFile(node, nodeCode);
            nodeCode += "o" + Num(node) + ".listItems = new List<string>();" + G.NL;
            nodeCode += "o" + Num(node) + ".p = p;" + G.NL;

            //Quite an ugly hack. Problem is that with "LIST<direct>xx = 0, 1, 2" Gekko will try to convert 0 into a list and will
            //throw an exception before O.List.Exe() is even called. So with <direct> we need to suppress the O.GetList()-code that 
            //puts stuff inside the elements.
            bool directOption = false;
            string extraCode = null;
            for (int i = 2; i < node.ChildrenCount(); i++)
            {
                if (node[i].Text == "ASTDIRECT") directOption = true;
                extraCode += node[i].Code;
            }
            if (directOption)
            {
                nodeCode += "o" + Num(node) + ".direct = true;" + G.NL;
                nodeCode += "o" + Num(node) + ".rawfood = " + "@`" + node.specialExpressionAndLabelInfo[1] + "`" + ";" + G.NL;
            }
            else
            {
                nodeCode += childCode;
            }
            nodeCode += extraCode;

            nodeCode += "o" + Num(node) + ".Exe();" + G.NL;
            return nodeCode;
        }

        private static string HandleListFile(ASTNode node, string nodeCode)
        {
            if (node[0].Text == "ASTLISTFILE")
            {
                nodeCode += "o" + Num(node) + ".listFile = O.ConvertToString(" + node[0].Code + ");" + G.NL;
            }
            else
            {
                nodeCode += "o" + Num(node) + ".name = O.ConvertToString(" + node[0].Code + ");" + G.NL;
            }

            return nodeCode;
        }

        private static string HandleString(ASTNode node, string childCode, bool isName)
        {
            string x = "false";
            if (isName) x = "true";

            string nodeCode = "O.SetStringData(" + node[0].Code + ", " + childCode + ", " + x + ");" + G.NL;
            return nodeCode;
        }

        private static string HandleDate(ASTNode node, string childCode)
        {
            string nodeCode = "O.SetDateData(" + node[0].Code + ", " + childCode + ");" + G.NL;
            return nodeCode;
        }

        private static string HandleVal(ASTNode node, string childCode, W w)
        {
            string nodeCodeTemp = null;            
            ASTNode node0 = node[0];
            if (node[0].Text == "?")
            {
                nodeCodeTemp = "O.Val.Q(" + Globals.QT + node[1].Text + Globals.QT + ");" + G.NL;
            }
            else
            {
                if (node0.nameSimpleIdent != null)
                {
                    //It is a simple ident code, such as VAL x = ...                                
                    string tempDoubleCs = "tempDouble" + ++Globals.counter;
                    nodeCodeTemp += "double " + tempDoubleCs + " = (" + childCode + ").GetVal(" + Globals.smpl + ");" + G.NL;
                    string notUsed = null;
                    string leftSideCs = CacheRefScalarCs(out notUsed, node0.nameSimpleIdent, GetScalarCache(w), GetHeaderCs(w), EScalarRefType.Val, tempDoubleCs, false, true, false);
                    nodeCodeTemp += leftSideCs + G.NL;
                }
                else
                {
                    //fancy name like VAL x|%y = ... --> this will be slow!                                
                    nodeCodeTemp += "O.SetValData(smpl, " + node0.Code + ", " + childCode + ");" + G.NL;
                }
            }
            return nodeCodeTemp;
        }

        private static string HandleGenr(ASTNode node, string numNode, string childCodePeriod, string childCodeLhsName, string childCodeRhs, W w, string lhsFunction)
        {
            string nodeCode = null;
            nodeCode += "O.Genr o" + numNode + " = new O.Genr();" + G.NL;
            nodeCode += EmitLocalCacheForTimeLooping(w);
            nodeCode += childCodePeriod + G.NL;  //dates
            nodeCode += "o" + numNode + ".lhs = null;" + G.NL;
            nodeCode += "o" + numNode + ".p = p;" + G.NL;
            nodeCode += "foreach (GekkoTime t2 in new GekkoTimeIterator(o" + numNode + ".t1, o" + numNode + ".t2))" + G.NL;
            nodeCode += GekkoTimeIteratorStartCode(w, node);
            nodeCode += "  double data = O.ConvertToVal(" + childCodeRhs + ", t);" + G.NL;  //uuu
            nodeCode += "if(o" + numNode + ".lhs == null) o" + numNode + ".lhs = O.GetTimeSeries(" + childCodeLhsName + ");" + G.NL; //we want the rhs to be constructed first, so that SERIES xx1 = xx1; fails if y does not exist (otherwist it would have been autocreated).                        
            //nodeCode += "  double dataLag = O.ConvertToVal(o" + numNode + ".lhs, t.Add(-1));" + G.NL;
            if (lhsFunction == null)
            {
                nodeCode += "o" + numNode + ".lhs.SetData(t, data);" + G.NL;
            }
            else if (G.Equal(lhsFunction, "log"))
            {
                nodeCode += "o" + numNode + ".lhs.SetData(t, Math.Exp(data));" + G.NL;
            }
            else if (G.Equal(lhsFunction, "dlog"))
            {
                nodeCode += "o" + numNode + ".lhs.SetData(t, o" + numNode + ".lhs.GetData(t.Add(-1)) * Math.Exp(data));" + G.NL;
            }
            else if (G.Equal(lhsFunction, "pch"))
            {
                nodeCode += "o" + numNode + ".lhs.SetData(t, o" + numNode + ".lhs.GetData(t.Add(-1)) * (data/100d + 1));" + G.NL;
            }
            else if (G.Equal(lhsFunction, "dif") || G.Equal(lhsFunction, "diff"))
            {
                nodeCode += "o" + numNode + ".lhs.SetData(t, o" + numNode + ".lhs.GetData(t.Add(-1)) + data);" + G.NL;
            }
            else
            {
                G.Writeln2("*** ERROR: Left-hand side function '" + lhsFunction + "' is not recognized");
                G.Writeln("           Legal functions are log, dlog, pch, dif or diff");
                throw new GekkoException();
            }
            nodeCode += GekkoTimeIteratorEndCode();

            if (node.Parent != null && node.Parent.Text == "ASTMETA" && node.Parent.specialExpressionAndLabelInfo != null && node.Parent.specialExpressionAndLabelInfo.Length > 1)
            {
                //specialExpressionAndLabelInfo[0] should be "ASTMETA" here
                nodeCode += "o" + numNode + ".meta = @`" + node.Parent.specialExpressionAndLabelInfo[1] + "`;" + G.NL;
            }
            nodeCode += "o" + numNode + ".Exe();" + G.NL;
            return nodeCode;
        }

        private static string GekkoTimeIteratorEndCode()
        {
            return Globals.endGekkoTimeIteratorCode;            
        }

        //private static string GekkoListIteratorEndCode()
        //{
        //    return Globals.endGekkoListIteratorCode;
        //}

        private static string GekkoTimeIteratorStartCode(W w, ASTNode node)
        {            
            string nodeCode = Globals.startGekkoTimeIteratorCode;
            if (node.timeLoopNestCode != null) nodeCode += node.timeLoopNestCode;                      
            return nodeCode;
        }

        //private static string GekkoListIteratorStartCode(W w, ASTNode node)
        //{
        //    string nodeCode = Globals.startGekkoListIteratorCode;
        //    if (node.listLoopNestCode != null) nodeCode += node.listLoopNestCode;
        //    return nodeCode;
        //}

        //private static void CreateTupleClass(StringBuilder headerCs, int number, string className, GekkoDictionary<string, bool> tupleClasses)
        //{
        //    //Tuples 2-10 are now predefined.

        //    //if(tupleClasses.ContainsKey(className)) return;  //do not duplicate, has been encountered before/elsewhere            
        //    ////a bit inefficient, but oh well
        //    //string ss = null;
        //    //string uu = null;
        //    //string vv = null;
        //    //int count = 0;
        //    //for (int i = 0; i < number; i++)
        //    //{
        //    //    count++;
        //    //    ss += "public IVariable tuple" + (count - 1) + ";" + G.NL;
        //    //    uu += "tuple" + (count - 1) + " = ptuble" + (count - 1) + ";" + G.NL;
        //    //    vv += "IVariable ptuble" + (count - 1) + ", ";
        //    //}
        //    //if (vv.EndsWith(", ")) vv = vv.Substring(0, vv.Length - 2);
        //    //headerCs.AppendLine("public class " + className + " { " + ss + G.NL + "public " + className + "(" + vv + ") {" + G.NL + uu + "} }");
        //    //tupleClasses.Add(className, true);
        //}        

        private static string Num(ASTNode node)
        {
            return "" + node.commandLinesCounter;
        }        

        private static string EmitLocalCacheForTimeLooping(W wh2)
        {
            string s = null;
            StringBuilder sb = null;
            if (wh2.wh != null && wh2.wh.localStatementCache != null && wh2.wh.localStatementCache.Count > 0)
            {
                sb = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in wh2.wh.localStatementCache)
                {
                    sb.AppendLine("IVariable " + kvp.Value + " = " + kvp.Key + ";");
                }
                return s + sb.ToString();
            }
            return s;
        }

        //private static void HandleScalar(ASTNode node, bool isCurlyWithoutPercent, W w)
        //{
        //    bool stringify = false;
        //    if (node.ChildrenCount() > 0 && (node[0].Text == "ASTDOLLARPERCENTNAMESIMPLE" || node[0].Text == "ASTDOLLARPERCENTPAREN")) stringify = true;
            
        //    bool transformationAllowed = true;
        //    bool isPartOfComposedName = false;
            
        //    if ((node.Number == 1 && node.Parent.Text == "ASTNAMEWITHBANK") 
        //        || node.Parent.Text == "ASTNAME"
        //        || node.Parent.Text == "ASTCURLY"
        //        || node.Parent.Text == "ASTCURLYSIMPLE")
        //    {
        //        //For instance base:%s. If %s is NAME 'fy', this would be equal to base:fy.                            
        //        //In that case, O.GetScalar must not be allowed to transform the string/name into a timeseries,
        //        //because we are going to look up the timeseries in the databank (in the example: base databank).
        //        //Therefore we call an overload of O.GetScalar()
        //        transformationAllowed = false;
        //    }

        //    if (Globals.nameFix)
        //    {
        //        if (node.Parent.Text == "ASTLISTITEM")
        //        {
        //            transformationAllowed = false;
        //        }                
        //    }

        //    if ((node.Parent.Text == "ASTNAME" && node.Parent.ChildrenCount() > 1)
        //        || (node.Parent.Text == "ASTCURLY" && node.Parent.ChildrenCount() == 1))
        //    {
        //        isPartOfComposedName = true; //composed names cannot be looked up in cache                                            
        //    }
            
        //    string scalarSimpleIdent = null;
        //    if (isCurlyWithoutPercent)
        //    {
        //        // {s}
        //        scalarSimpleIdent = node.Text;
        //    }
        //    else
        //    {
        //        if (node[0].Text == "ASTPERCENTNAMESIMPLE" || node[0].Text == "ASTDOLLARPERCENTNAMESIMPLE")
        //        {
        //            // %s, not %(...), the %s may be inside {} like {%s}
        //            scalarSimpleIdent = node[0][0].Text;
        //        }
        //    }

        //    if (scalarSimpleIdent != null)
        //    {
        //        //either {s} or %s
        //        string fa = FindFunctionArguments(node, w, scalarSimpleIdent);
        //        if (fa != null)
        //        {
        //            node.Code.A(fa);  //????????? What is this????????
        //        }
        //        else
        //        {                   

        //            //hmmm why do we have isPartOfComposedName and transformationAllowed at the same time
        //            //is it not the same thing?

        //            if (!isPartOfComposedName && w.wh.localStatementCache != null)
        //            {
        //                //not for instance a%s or %(%s) but clean %s, and part of GENR statement                        
        //                //In that case, we look for the variable in the local GENR cache
        //                string refCode = "ts" + ++Globals.counter;

        //                string fallBackCode = null;

        //                string t = "false";
        //                if (transformationAllowed) t = "true";
        //                string s = "false";
        //                if (stringify) s = "true";

        //                fallBackCode = "O.GetScalar(`" + scalarSimpleIdent + "`, " + t + ", " + s + ")";
                        
        //                string xx = null; w.wh.localStatementCache.TryGetValue(fallBackCode, out xx);
        //                if (xx != null)
        //                {
        //                    //This complicated timeseries (or scalar) has been seen before in this particular GENR statement                        
        //                    node.Code.CA(xx);
        //                }
        //                else
        //                {
        //                    //has not been seen before
        //                    w.wh.localStatementCache.Add(fallBackCode, refCode);
        //                    node.Code.CA(refCode);
        //                }
        //            }
        //            else
        //            {
        //                string notUsed = null;
        //                node.Code.A(CacheRefScalarCs(out notUsed, scalarSimpleIdent, GetScalarCache(w), GetHeaderCs(w), EScalarRefType.OnRightHandSide, null, false, transformationAllowed, stringify));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //not {s} or {%s}, but something like {%s1+%s2}
        //        node.Code.A("O.ZScalar(" + node[0].Code + ")");
        //    }
        //}

        //This method converts a simple scalar like '%s' into a reference to 'scalar117' (global IVariable), via the method O.GetScalarFromCache()
        private static string CacheRefScalarCs(out string scalarNameInGlobalCache, string scalarSimpleIdent, GekkoDictionary<string, string> scalarCache, StringBuilder headerCs, EScalarRefType type, string rhsCs, bool isName, bool transformationAllowed, bool stringify)
        {
            string scalarCs = null;
            scalarNameInGlobalCache = null; scalarCache.TryGetValue(scalarSimpleIdent, out scalarNameInGlobalCache);
            if (scalarNameInGlobalCache == null)
            {
                //this scalar name has not been encountered before
                scalarNameInGlobalCache = "scalar" + ++Globals.counter;
                scalarCache.Add(scalarSimpleIdent, scalarNameInGlobalCache);
                headerCs.AppendLine("public static IVariable " + scalarNameInGlobalCache + " = null;");                
            }            
            //using the scalar name (for instance scalar117) found or created in the global cache
            if (type == EScalarRefType.OnRightHandSide)
            {
                string b = "false";
                if (transformationAllowed) b = "true";
                string s = "false";
                if (stringify) s = "true";
                scalarCs = "O.GetScalarFromCache(ref " + scalarNameInGlobalCache + ", `" + scalarSimpleIdent + "`, " + b + ", " + s + ")";
            }
            else if (type == EScalarRefType.Val)
            {
                scalarCs = "O.SetValFromCache(ref " + scalarNameInGlobalCache + ", `" + scalarSimpleIdent + "`, " + rhsCs + ");" + G.NL;
            }
            else if (type == EScalarRefType.Date)
            {
                scalarCs = "O.SetDateFromCache(ref " + scalarNameInGlobalCache + ", `" + scalarSimpleIdent + "`, " + rhsCs + ");" + G.NL;
            }
            else if (type == EScalarRefType.String)
            {
                string x = "false";
                if (isName) x = "true";
                scalarCs = "O.SetStringFromCache(ref " + scalarNameInGlobalCache + ", `" + scalarSimpleIdent + "`, " + rhsCs + ", " + x + ");" + G.NL;
            }
            else if (type == EScalarRefType.Matrix)
            {
                //see also SetMatrixData(), should that be used??
                scalarCs = "O.SetMatrixFromCache(ref " + scalarNameInGlobalCache + ", `" + scalarSimpleIdent + "`, " + rhsCs + ");" + G.NL;
            }
            else throw new GekkoException();    
            return scalarCs;
        }        

        private static string FindFunctionArguments(ASTNode node, W wh2, string simpleIdent)
        {            
            if (wh2.uFunctionsHelper != null)
            {
                foreach (FunctionArgumentsHelperElements fah in wh2.uFunctionsHelper.storage)
                {
                    if (G.Equal(fah.parameterName, simpleIdent))
                    {
                        //this list is a function argument, use that                        
                        return fah.parameterCode;
                    }
                }
            }
            return null;
        }        

        private static void GetCodeFromAllChildren(ASTNode node)
        {
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


        //private static string AstBankHelperList(ASTNode node, W wh2)
        //{
        //    string bankNumberCode = null;
        //    if (wh2.wh.currentCommand == "ASTPRT")
        //    {
        //        bankNumberCode = "bankNumber";
        //    }
        //    else
        //    {
        //        bankNumberCode = "1";  //default: Work
        //    }
        //    string code = "O.GetListWithBankPrefix(" + node[0].Code + ", " + node[1].Code + ", " + bankNumberCode + ")";            
        //    return code;
        //}


        //private static string AstBankHelper(ASTNode node, W wh2, int type)
        //{
        //    string isLhsSoCanAutoCreate = null;
        //    if ((node.Number == 1 && (node.Parent.Text == "ASTGENR" || node.Parent.Text == "ASTGENRLHSFUNCTION")) || node.Parent.Text == "ASTTUPLEITEM" ||  wh2.wh.seriesHelper == WalkHelper.seriesType.SeriesLhs)
        //    {
        //        isLhsSoCanAutoCreate = ", O.ECreatePossibilities.Can";
        //    }
        //    else if (node.Number == 0 && node.Parent.Text == "ASTCREATEEXPRESSION")
        //    {
        //        isLhsSoCanAutoCreate = ", O.ECreatePossibilities.Must";
        //    }

        //    string bankNumberCode = null;
        //    if (wh2.wh.currentCommand == "ASTPRT" || wh2.wh.currentCommand == "ASTTABLESETVALUES")
        //    {
        //        bankNumberCode = "bankNumber";
        //    }
        //    else
        //    {
        //        bankNumberCode = "1";  //default: Work
        //    }

        //    if (type == 1)
        //    {
        //        string listFallBackCode = null;
        //        if (node[0].ChildrenCount() == 0)
        //        {
        //            listFallBackCode = node[1].Code.ToString();  //this is a ScalarString already, and we want to avoid a superfluous 'Work:a' for an 'a' item. So this will run ok fast for simple items.
        //        }
        //        else
        //        {
        //            listFallBackCode = "new ScalarString(O.ConvertToString(" + node[0].Code + ") + `:` + O.ConvertToString(" + node[1].Code + "))";
        //        }
        //        return listFallBackCode;
        //    }
        //    else if (type == 2) //minus
        //    {
        //        string listFallBackCode = null;
        //        if (node[0][0].ChildrenCount() == 0)
        //        {
        //            listFallBackCode = "O.Add(smpl, new ScalarString(`-`), " + node[0][1].Code + ")";  //this is a ScalarString already, and we want to avoid a superfluous 'Work:a' for an 'a' item. So this will run ok fast for simple items.
        //        }
        //        else
        //        {
        //            listFallBackCode = "new ScalarString(O.ConvertToString(" + node[0][0].Code + ") + `:-` + O.ConvertToString(" + node[0][1].Code + "))";
        //        }
        //        return listFallBackCode;
        //    }

        //    string fallBackCode = "O.ConvertToString(" + node[0].Code + ") + `:` + O.ConvertToString(" + node[1].Code + ")";

        //    string simpleHash = null;
        //    //node[1].ChildrenCount() is always > 0
        //    if (node[0].ChildrenCount() > 0)
        //    {
        //        //there is a bank like a:b or %x:b or %x:%y
        //        if (node[0][0].nameSimpleIdent != null && node[1][0].nameSimpleIdent != null)
        //        {
        //            //simple names like a:b
        //            simpleHash = node[0][0].nameSimpleIdent + ":" + node[1][0].nameSimpleIdent;
        //        }
        //    }
        //    else
        //    {
        //        //name like a or %x
        //        if (node[1][0].nameSimpleIdent != null)
        //        {
        //            //simple name like a
        //            simpleHash = node[1][0].nameSimpleIdent;
        //        }
        //    }

        //    bool isSimple = false; if (simpleHash != null) isSimple = true;

        //    string code = null;

        //    string fa;
        //    int choice;
        //    GetChoice(node, wh2, simpleHash, isSimple, out fa, out choice);

        //    if (choice == 1)
        //    {
        //        node.Code.A(fa);
        //    }
        //    else if (choice == 2)
        //    {
        //        //isSimple means that the name is simple like a or b:a.
        //        //Then we look for it in the global cache

        //        GekkoDictionary<string, string> tsCache = GetTsCache(wh2);

        //        string s = null; tsCache.TryGetValue(simpleHash, out s);
        //        if (s == null)
        //        {
        //            //has not been seen before
        //            string ivWithNumber = "iv" + ++Globals.counter;
        //            tsCache.Add(simpleHash, ivWithNumber);
        //            GetHeaderCs(wh2).AppendLine("public static IVariable " + ivWithNumber + " = null;");  //cannot set it to ScalarVal since it may change type...                    
        //            node.Code.A("O.GetTimeSeriesFromCache(ref " + ivWithNumber + ", `" + simpleHash + "`, " + bankNumberCode + isLhsSoCanAutoCreate + ")");
        //        }
        //        else
        //        {
        //            node.Code.A("O.GetTimeSeriesFromCache(ref " + s + ", `" + simpleHash + "`, " + bankNumberCode + isLhsSoCanAutoCreate + ")");
        //        }
        //    }
        //    else if (choice == 3)
        //    {
        //        //This means that the name is complicated like %x or b:%x or %y:a or %x:%y (or fx%i)                
        //        if (wh2.wh.seriesHelper != WalkHelper.seriesType.SeriesLhs && wh2.wh.localStatementCache != null)
        //        {
        //            //GENR statement for instance, maybe also VAL if indexer fY[2010]??
        //            //This means there is a GENR statement at the top of the AST tree
        //            //In that case, we look for the variable in the local cache
        //            string fallBackCode2 = "O.GetTimeSeries(smpl, " + fallBackCode + ", " + bankNumberCode + isLhsSoCanAutoCreate + ")";  //here, bankNumberCode will always be = "1", since this is not a PRT statement
        //            string xx = null; wh2.wh.localStatementCache.TryGetValue(fallBackCode2, out xx);
        //            if (xx != null)
        //            {
        //                //This complicated timeseries (or scalar) has been seen before in this particular GENR/PRT statement                        
        //                code = xx;                                                
        //            }
        //            else
        //            {
        //                string refCode = "ts" + ++Globals.counter;
        //                wh2.wh.localStatementCache.Add(fallBackCode2, refCode);
        //                code = refCode;
        //            }
        //        }
        //        else
        //        {
        //            if (wh2.wh.seriesHelper == WalkHelper.seriesType.SeriesLhs)
        //            {
        //                node.Code.A("O.FindTimeSeries(" + fallBackCode + ", " + bankNumberCode + isLhsSoCanAutoCreate + ")");
        //            }
        //            else
        //            {
        //                node.Code.A("O.GetTimeSeries(smpl, " + fallBackCode + ", " + bankNumberCode + isLhsSoCanAutoCreate + ")");
        //                //Complicated name, but not inside a GENR statement: just use the statement directly without use of any caches
        //            }
        //        }
        //    }
        //    else throw new GekkoException();
        //    return code;
        //}

        private static void GetChoice(ASTNode node, W wh2, string simpleHash, bool isSimple, out string fa, out int choice)
        {
            fa = null;
            choice = 3;  //1, 2, 3
            if (isSimple)
            {
                fa = FindFunctionArguments(node, wh2, simpleHash);
                if (fa != null) choice = 1;
                else choice = 2;
            }
            if (choice == 2 && Globals.useCache == false) choice = 3;
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
                else if (node.Parent.Text == "ASTNAMEWITHDOT")
                {
                    //For parent ASTNAMEWITHBANK nothing is ignored 
                    minus = "-";                    
                }
            }
            return minus;
        }

        private static string ExtractInnerString(string s)
        {
            string s2 = s;
            if (s.StartsWith("new ScalarString(`") && s.EndsWith("`)"))
            {
                s2 = s.Substring(18, s.Length - 18 - 2);
            }
            else if (s.StartsWith("(new ScalarString(`") && s.EndsWith("`))"))
            {
                s2 = s.Substring(19, s.Length - 19 - 3);
            }
            return s2;
        }

        //private static void SendStuffUpwardsInTree2(ASTNode node, string scalarWithNumber, string s, WalkHelper2 wh2)
        //{
        //    if (true)
        //    {
        //        wh2.headerCs += "public static IVariable " + scalarWithNumber + " = null;" + G.NL;
        //        wh2.headerMethodScalarCs += scalarWithNumber + " = null;" + G.NL;
        //    }
        //    else
        //    {
        //        ASTNode node2 = node.Parent;
        //        bool succes = false;
        //        while (true)
        //        {
        //            if (node2 == null) break;
        //            //TODO TODO TODO TODO
        //            //TODO TODO TODO TODO
        //            //TODO TODO TODO TODO do something intellingent regarding uploading
        //            //TODO TODO TODO TODO
        //            //TODO TODO TODO TODO
        //            if (node2.Text == "ASTGENR" || node2.Text == "ASTVAL" || node2.Text == "ASTSTRING" || node2.Text == "ASTDATE" || node2.Text == "ASTPRTELEMENT")
        //            {
        //                node2.CodeSentFromSubTree += "IVariable " + scalarWithNumber + " = " + s + ";" + G.NL;
        //                succes = true;
        //                break;
        //            }
        //            node2 = node2.Parent;
        //        }
        //        if (succes == false)
        //        {
        //            node.Code.A(s;
        //        }
        //        else
        //        {
        //            node.Code.A(scalarWithNumber;
        //        }
        //    }
        //    return;
        //}

        //private static void SendTimeSeriesUpwardsInTree(ASTNode node, string hash, string tsWithNumber, string refCode, bool isSimple, WalkHelper2 wh2)
        //{
        //    //This Series is not known, seen for the first time in the file.
        //    string[] split = hash.Split(',');
        //    string bank = split[0].Trim();
        //    string variable = split[1].Trim();
        //    string hash2 = "O.ConvertToString(" + bank + "), O.ConvertToString(" + variable + ")";
                        
        //    if (isSimple)
        //    {
        //        //name like adambk:fy, not bank%1:fx%i
        //        wh2.headerCs += "public static IVariable " + refCode + " = null;" + G.NL;
        //        //wh2.headerMethodTsCs += "((MetaTimeSeries)" +refCode + ").ts = null;" + G.NL;  //clear this
        //        wh2.headerMethodTsCs += "" + refCode + " = null;" + G.NL;  //clear this
        //    }
        //    else
        //    {
        //        //composed name like bank%1:fx%i
        //        ASTNode node2 = node.Parent;
        //        bool succes = false;
        //        while (true)
        //        {
        //            if (node2 == null) break;
        //            //TODO TODO TODO TODO
        //            //TODO TODO TODO TODO
        //            //TODO TODO TODO TODO do something intellingent regarding uploading
        //            //TODO TODO TODO TODO
        //            //TODO TODO TODO TODO                

        //            if (node2.Text == "ASTGENR" || node2.Text == "ASTVAL" || node2.Text == "ASTSTRING" || node2.Text == "ASTDATE" || node2.Text == "ASTUPD")
        //            {
        //                node2.CodeSentFromSubTree += "IVariable " + tsWithNumber + " = new MetaTimeSeries(O.GetTS(" + hash2 + "));" + G.NL;                        
        //                succes = true;
        //                break;
        //            }
        //            else if (node2.Text == "ASTPRTELEMENT")
        //            {
        //                //This variant reacts to a bankNumber (if for instance called with <m> or <b>).
        //                node2.CodeSentFromSubTree += "IVariable " + tsWithNumber + " = new MetaTimeSeries(O.GetTS(" + hash2 + ", bankNumber));" + G.NL;  //null, null because it is not a simple name
        //                succes = true;
        //                break;
        //            }
        //            node2 = node2.Parent;
        //        }
        //        if (succes == false)
        //        {
        //            //node.Code.A("new MetaTimeSeries(O.GetTS(" + hash + "))" + G.NL;
        //            //G.Writeln2("*** ERROR: Unexpected use of time series: did you intend to use a scalar?");
        //            //throw new GekkoException();
        //            node.Code.A("new MetaTimeSeries(O.GetTS(" + hash2 + "))";
        //        }
        //        else
        //        {
        //        }
        //    }
        //    return;
        //}        

        private static string AddPrintCode(string type, string s, string parentType, ASTNode node)
        {
            string o = "o" + Num(node) + ".printCodes";
            if (parentType == "ASTPRTELEMENTOPTIONFIELD") o = "ope" + Num(node) + ".printCodes";
            return o + ".Add(new OptString(`" + type + "`, O.ConvertToString(" + s + ")));" + G.NL;
        }        

        //private static string CondenseChildrenNodesToList(ASTNode node)  //last arg. just so that it is more likely that the user remembers to augment Globals.counter before call
        //{
        //    Globals.counter++;
        //    string name = "l" + Globals.counter;
        //    node.GetCode().AppendLine("List<string>" + name + " = new List<string>();");
        //    foreach (ASTNode child in node.ChildrenIterator())
        //    {
        //        node.GetCode().AppendLine(name + ".Add(" + child.GetCode() + ");");
        //    }
        //    return name;
        //}

        //private static void CondenseChildrenNodesToString(ASTNode node, string divider)
        //{
        //    foreach (ASTNode child in node.ChildrenIterator())
        //    {
        //        node.GetCode().Append(child.GetCode().ToString());
        //        if (!child.IsLastChild()) node.GetCode().Append(" " + divider + " ");
        //    }
        //}

        private static void Emit1(ASTNode node, GekkoST st)
        {
            Globals.counter++;
            st.Append("List<string> xx{#} = new List<string>();" + G.NL);
            foreach (ASTNode child in node.ChildrenIterator())
            {
                st.Append("xx{#}.Add({0});", child);
            }
        }        

        private static void CreateOptionVariable(ASTNode node, StringBuilder s, ref string o)
        {
            StringBuilder s1 = new StringBuilder();
            StringBuilder s1a = new StringBuilder();
            StringBuilder s2 = new StringBuilder();
            string type = null;
            s2.Append("Program.options.");
            for (int i = 0; i < node.ChildrenCount(); ++i)
            {
                ASTNode child = node[i];

                if (child.Text == "?")
                {
                    s.Append("Program.PrintOptions(`" + s2.ToString() + s1.ToString() + "`);");
                    return;
                }

                if (i < node.ChildrenCount() - 2)  //up to and including third-last
                {
                    s1.Append(child.Text.ToLower() + "_");
                }
                else if (i == node.ChildrenCount() - 2)  //second-last
                {
                    s1.Append(child.Text.ToLower());
                }
                else if (i == node.ChildrenCount() - 1)  //last
                {
                    if (child.Text == "ASTINTEGER")
                    {
                        type = "val";
                        if (child.GetChild(0).Text == "-")
                        {
                            s1a.Append(child.GetChild(0).Text);
                            s1a.Append(child.GetChild(1).Text);
                        }
                        else
                        {
                            s1a.Append(child.GetChild(0).Text);
                        }
                    }
                    else if (child.Text == "ASTDOUBLE")
                    {
                        type = "val";
                        int index = 0;
                        if (child.GetChild(0).Text == "-")
                        {
                            s1a.Append(child.GetChild(0).Text);
                            index = 1;
                        }
                        string s5 = child.GetChild(index).Text;
                        s5 = Program.MaybeAddPointAndZero(s5);
                        s1a.Append(s5);
                    }
                    else if (child.Text == "ASTBOOL")
                    {
                        type = "string";
                        if (G.Equal(child.GetChild(0).Text, "astyes") || G.Equal(child.GetChild(0).Text, "true"))
                        {
                            s1a.Append("true");
                        }
                        else if (G.Equal(child.GetChild(0).Text, "astno") || G.Equal(child.GetChild(0).Text, "false"))
                        {
                            s1a.Append("false");
                        }
                        else
                        {
                            G.Writeln2("*** ERROR with options");
                            throw new GekkoException();
                        }
                    }
                    else if (child.Text == "ASTSTRINGSIMPLE")
                    {

                        bool resolvePath = false;
                        List<string> folder = new List<string>();
                        folder.Add("folder_bank");
                        folder.Add("folder_bank1");
                        folder.Add("folder_bank2");
                        folder.Add("folder_command");
                        folder.Add("folder_command1");
                        folder.Add("folder_command2");
                        folder.Add("folder_help");
                        folder.Add("folder_menu");
                        folder.Add("folder_model");
                        folder.Add("folder_pipe");
                        folder.Add("folder_table");
                        folder.Add("folder_table1");
                        folder.Add("folder_table2");
                        folder.Add("folder_working");
                        folder.Add("gams_exe_folder");
                        folder.Add("r_exe_folder");
                        if (folder.Contains(s1.ToString().ToLower())) resolvePath = true;

                        type = "string";
                        string temp = "";
                        string s7 = null;
                        if (child[0].Code.IsNull())
                        {
                            s7 = "`" + child[0].Text + "`";
                        }
                        else
                        {
                            s7 = "O.ConvertToString(" + child[0].Code + ")";
                        }
                        if (G.Equal(node[0].Text, "freq"))  //OPTION freq = ...
                        {
                            s7 = "G.GetFreq(" + s7 + ")";
                        }
                        if (resolvePath)
                        {
                            s7 = "O.ResolvePath(" + s7 + ")";
                        }
                        temp += s7;
                        //}
                        s1a.Append(temp);
                    }
                    else
                    {
                        throw new GekkoException();
                    }
                }
                else throw new GekkoException();
            }
            o = s1.ToString();
            s.Append(s2);
            s.Append(s1);
            s.Append(" = ");
            s.Append(s1a);
            s.AppendLine(";");            

            StringBuilder s3 = new StringBuilder();
            s3.Append("option_");
            s3.Append(s1);
            s3.Append(" = ");
            string s1b = s1a.ToString();
            if (s1b.StartsWith("@")) s1b = s1b.Substring(1);
            s3.Append(s1b);
            s3.Replace(@"\", @"\\");
            s3.Replace("_", " ");
            s3.Replace("`", "");
            s3.Replace("true", "yes");
            s3.Replace("false", "no");
            s.AppendLine("G.Writeln();");
            //#987350932752
            //s.AppendLine("G.Writeln(Program.SubstituteAssignVars(`" + s3.ToString() + "`));");

            string sss = s1a.ToString();
            s1 = s1.Replace("_", " ");
            //#0934580980
            sss = sss.Replace("True", "`yes`");
            sss = sss.Replace("False", "`no`");
            sss = sss.Replace("true", "`yes`");
            sss = sss.Replace("false", "`no`");
            //string ss = "IVariable iv = " + s1a + ";";
            //if (type == "string")
            //{
                s.AppendLine("G.Writeln(`option " + s1.ToString() + " = ` + " + sss + " + ``);");
            //}
            //else if (type == "val")
            //{
                //s.AppendLine("G.Writeln(" + s1.ToString() + "O.ConvertToVal(`" + s3.ToString() + "`));");
            //    s.AppendLine("G.Writeln(" + s1.ToString() + "(`" + s1a.ToString() + "`));");
            //}
        }

        private static void CreateOptionVariableOLD(ASTNode node, StringBuilder s, ref string o)
        {
            StringBuilder s1 = new StringBuilder();
            StringBuilder s1a = new StringBuilder();
            StringBuilder s2 = new StringBuilder();
            s2.Append("Program.options.");
            for (int i = 0; i < node.ChildrenCount(); ++i)
            {
                ASTNode child = node[i];

                if (child.Text == "?")
                {
                    s.Append("Program.PrintOptions(`" + s2.ToString() + s1.ToString() + "`);");
                    return;
                }

                if (i < node.ChildrenCount() - 2)  //up to and including third-last
                {
                    s1.Append(child.Text.ToLower() + "_");
                }
                else if (i == node.ChildrenCount() - 2)  //second-last
                {
                    s1.Append(child.Text.ToLower());
                }
                else if (i == node.ChildrenCount() - 1)  //last
                {
                    if (child.Text == "ASTINTEGER")
                    {
                        if (child.GetChild(0).Text == "-")
                        {
                            s1a.Append(child.GetChild(0).Text);
                            s1a.Append(child.GetChild(1).Text);
                        }
                        else
                        {
                            s1a.Append(child.GetChild(0).Text);
                        }
                    }
                    else if (child.Text == "ASTDOUBLE")
                    {
                        int index = 0;
                        if (child.GetChild(0).Text == "-")
                        {
                            s1a.Append(child.GetChild(0).Text);
                            index = 1;
                        }
                        string s5 = child.GetChild(index).Text;
                        s5 = Program.MaybeAddPointAndZero(s5);
                        s1a.Append(s5);
                    }
                    else if (child.Text == "ASTBOOL")
                    {
                        if (G.Equal(child.GetChild(0).Text, "yes") || G.Equal(child.GetChild(0).Text, "true"))
                        {
                            s1a.Append("true");
                        }
                        else if (G.Equal(child.GetChild(0).Text, "no") || G.Equal(child.GetChild(0).Text, "false"))
                        {
                            s1a.Append("false");
                        }
                        else
                        {
                            G.Writeln2("*** ERROR with options");
                            throw new GekkoException();
                        }
                    }
                    else if (child.Text == "ASTSTRINGSIMPLE")
                    {
                        string temp = "";
                        //if (child.GetChild(0).Text == "ASTFILENAME")  //very unlikely this should ever be used as a string name here
                        //{
                        //    ////When does this happen??????????????
                        //    //string s4 = "";
                        //    //ASTNode gchild = child.GetChild(0);
                        //    //foreach (ASTNode child2 in gchild.ChildrenIterator())
                        //    //{
                        //    //    s4 += child2.Text;
                        //    //}
                        //    //s4 = Program.StripQuotes(s4);
                        //    //temp += ("@`" + s4 + "`");  //@ because it can contain slashes

                        //    temp += ("O.ConvertToString(" + child[0].Code + ")");  //@ because it can contain slashes
                        //}
                        //else
                        //{
                        string s7 = null;
                        if (child[0].Code.IsNull())
                        {
                            s7 = "`" + child[0].Text + "`";
                        }
                        else
                        {
                            s7 = "O.ConvertToString(" + child[0].Code + ")";                            
                        }
                        if (G.Equal(node[0].Text, "freq"))  //OPTION freq = ...
                        {
                            s7 = "G.GetFreq(" + s7 + ")";
                        }
                        temp += s7;
                        //}
                        s1a.Append(temp);
                    }
                    else
                    {
                        throw new GekkoException();
                    }
                }
                else throw new GekkoException();
            }
            o = s1.ToString();            
            s.Append(s2);
            s.Append(s1);
            s.Append(" = ");
            s.Append(s1a);
            s.AppendLine(";");
            StringBuilder s3 = new StringBuilder();
            s3.Append("option_");
            s3.Append(s1);
            s3.Append(" = ");
            string s1b = s1a.ToString();
            if (s1b.StartsWith("@")) s1b = s1b.Substring(1);
            s3.Append(s1b);
            s3.Replace(@"\", @"\\");
            s3.Replace("_", " ");
            s3.Replace("`", "");
            s3.Replace("true", "yes");
            s3.Replace("false", "no");
            s.AppendLine("G.Writeln();");
            //#987350932752
            //s.AppendLine("G.Writeln(Program.SubstituteAssignVars(`" + s3.ToString() + "`));");
        }
    }

    public class W
    {         
        //W is created when running a .cmd/.gcm file
        public WalkHelper wh = null;  //is created when encountering a Gekko command (like SERIES, PRT, etc.)        
        public Dictionary<int, List<string>> prtItems;
        public Dictionary<int, List<string>> prtLabels;
        public string fileNameContainingParsedCode = null;
        public int commandLinesCounter = -1;
        public int expressionCounter = -1;

        public GekkoDictionary<string, string> scalarCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public GekkoDictionary<string, string> listCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public GekkoDictionary<string, string> tsCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public StringBuilder headerCs = new StringBuilder(); //stuff to be put at the very start.
        public StringBuilder headerMethodTsCs = new StringBuilder(); //stuff to clear Series pointers
        public StringBuilder headerMethodScalarCs = new StringBuilder(); //stuff to clear scalar pointers   
        public StringBuilder headerExpressions = new StringBuilder();

        //public GekkoDictionary<string, bool> functionUserDefined = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        public GekkoDictionary<string, bool> tupleClasses = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        public StringBuilder uHeaderCs = new StringBuilder(); //stuff to be put at the very start.        
        public FunctionArgumentsHelper uFunctionsHelper = null; //important that it starts out as null here
        public GekkoDictionary<string, string> uScalarCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public GekkoDictionary<string, string> uListCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public GekkoDictionary<string, string> uTsCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);        
    }

    public class FunctionArgumentsHelperElements
    {
        public string parameterName;
        public string type;
        public string parameterCode;
        public int tupleCount = 1;
        public string tupleNameCode;
    }

    public class FunctionArgumentsHelper
    {
        public List<string> lhsTypes = new List<string>();
        public string functionName;
        public List<FunctionArgumentsHelperElements> storage = new List<FunctionArgumentsHelperElements>();
        public StringBuilder headerCs = new StringBuilder();
    }

    public class WalkHelper
    {
        public enum seriesType
        {
            None,
            SeriesLhs,
            SeriesRhs
        }

        //created for each new command (except IF, FOR, etc -- hmm is this true now?)

        public GekkoStringBuilder localFuncs = null;

        public GekkoDictionary<string, string> localStatementCache = null;        
        public seriesType seriesHelper = seriesType.None;

        public GekkoDictionary<string, string> seriesHelperListNames = null;
        public GekkoDictionary<string, string> sumHelperListNames = null;

        public string currentCommand = null;
        public bool isGotoOrTarget = false;
             
    }

    public class OPrt : O_OLD
    {
        private string fileName = null;
        private EDataFormat type = EDataFormat.None;  //type of data(bank)
        private bool merge = false;  //merge or not.
        private string as2 = null; //for OPEN AS.
        private string orientation = null;  //rows or cols

        public string FileName
        {
            get { return fileName; }
            set { FailIfImmutable(); fileName = value; }
        }

        public EDataFormat Type
        {
            get { return type; }
            set { FailIfImmutable(); type = value; }
        }

        public bool Merge
        {
            get { return merge; }
            set { FailIfImmutable(); merge = value; }
        }

        public string As
        {
            get { return as2; }
            set { FailIfImmutable(); as2 = value; }
        }

        public string Orientation
        {
            get { return orientation; }
            set { FailIfImmutable(); orientation = value; }
        }

    }
}
