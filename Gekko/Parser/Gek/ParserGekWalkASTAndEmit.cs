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

        public void Prepend(string s)
        {
            if (this.storage == null) this.storage = new StringBuilder();            
            this.storage.Insert(0, s);
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

        public static void WalkASTAndEmit(ASTNode node, int absoluteDepth, int relativeDepth, string textInput, W w, P p)
        {            
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

            if (node.Text == "ASTGENR" || node.Text == "ASTGENRLHSFUNCTION" || node.Text == "ASTPRTELEMENT" || node.Text == "ASTOLSELEMENT" || node.Text == "ASTTABLESETVALUESELEMENT")
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
                    node.Code.A("o" + Num(node) + ".opt_" + s2.ToLower() + " = O.GetString(" + node[0].Code + ");" + G.NL);
                }
            }
            else if (node.Text != null && node.Text.StartsWith("ASTOPT_VAL_"))
            {
                string s2 = node.Text.Substring(11);                
                if (node.ChildrenCount() == 0) throw new GekkoException();
                else node.Code.A("o" + Num(node) + ".opt_" + s2.ToLower() + " = O.GetVal(" + node[0].Code + ", t);" + G.NL);
            }
            else if (node.Text != null && node.Text.StartsWith("ASTOPT_DATE_"))
            {
                string s2 = node.Text.Substring(12);                
                if (node.ChildrenCount() == 0) throw new GekkoException();
                else node.Code.A("o" + Num(node) + ".opt_" + s2.ToLower() + " = O.GetDate(" + node[0].Code + ");" + G.NL);
            }
            else
            {
                //After sub-nodes
                switch (node.Text)
                {
                    case "+":
                        {
                            node.Code.CA("O.Add(" + node[0].Code + ", " + node[1].Code + ", t)");
                        }
                        break;                    
                    case "-":
                        {
                            node.Code.CA("O.Subtract(" + node[0].Code + ", " + node[1].Code + ", t)");
                        }
                        break;
                    case "*":
                        {
                            node.Code.CA("O.Multiply(" + node[0].Code + ", " + node[1].Code + ", t)");
                        }
                        break;
                    case "/":
                        {
                            node.Code.CA("O.Divide(" + node[0].Code + ", " + node[1].Code + ", t)");
                        }
                        break;
                    case "ASTPOW":
                        {
                            node.Code.CA("O.Power(" + node[0].Code + ", " + node[1].Code + ", t)");
                        }
                        break;
                    case "&+":
                        {
                            node.Code.CA("O.AndAdd(" + node[0].Code + ", " + node[1].Code + ", t)");
                        }
                        break;
                    case "&-":
                        {
                            node.Code.CA("O.AndSubtract(" + node[0].Code + ", " + node[1].Code + ", t)");
                        }
                        break;
                    case "&*":
                        {
                            node.Code.CA("O.AndMultiply(" + node[0].Code + ", " + node[1].Code + ", t)");
                        }
                        break;
                    case "[":  //indexer
                    case Globals.symbolGlueChar6:  //indexer, '[_['
                    case Globals.symbolGlueChar7:  //indexer, '[¨['
                        {
                            //At the moment only 1 dimension supported                        
                            if (node.ChildrenCount() > 3) throw new GekkoException();
                            if (node[1].Text == "ASTINDEXERELEMENTPLUS")
                            {
                                node.Code.A("O.IndexerPlus(" + node[0].Code + ", " + node[1].Code + ", t)");
                            }
                            else
                            {
                                if (node.ChildrenCount() == 2)
                                {
                                    node.Code.A("O.Indexer(" + node[0].Code + ", " + node[1].Code + ", t)");
                                }
                                else
                                {
                                    node.Code.A("O.Indexer(" + node[0].Code + ", " + node[1].Code + ", " + node[2].Code + ", t)");
                                }
                            }                            
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
                    case "ASTSTAR":
                        {
                            node.Code.CA("new ScalarVal(123454321)");  //signifies infinity for REP * in UPD.
                        }
                        break;
                    case "ASTUPDDATACOMPLICATED":
                        {
                            
                            GetCodeFromAllChildren(node);
                        }
                        break;
                    case "ASTS":                    
                        {
                            node.Code.A(AddPrintCode( Globals.printCode_s, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;                    
                    case "ASTSN":  //rn
                        {
                            node.Code.A(AddPrintCode(Globals.printCode_sn, node[0].Code.ToString(), node.Parent.Parent.Text, node));
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
                            node.Code.A(AddPrintCode(Globals.printCode_sd, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTSDP":
                        {
                            node.Code.A(AddPrintCode(Globals.printCode_sdp, node[0].Code.ToString(), node.Parent.Parent.Text, node));
                        }
                        break;
                    case "ASTSP":
                        {
                            node.Code.A(AddPrintCode(Globals.printCode_sp, node[0].Code.ToString(), node.Parent.Parent.Text, node));
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
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
                    case "ASTRESTART":
                        {
                            node.Code.A("O.Restart o" + Num(node) + " = new O.Restart();" + G.NL);
                            node.Code.A("o" + Num(node) + ".p = p;");
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
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

                            node.Code.A("o" + Num(node) + ".b1 = O.GetString(" + b1 + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".b0 = O.GetString(" + b0 + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".v1 =  O.GetString(" + node[0][1].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".v0 =  O.GetString(" + node[1][1].Code + ");" + G.NL);
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

                            node.Code.A("o" + Num(node) + ".b1 = O.GetString(" + b1 + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".b0 = O.GetString(" + b0 + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".v1 =  O.GetString(" + node[0][1].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".v0 =  O.GetString(" + node[1][1].Code + ");" + G.NL);
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
                            GetCodeFromAllChildren(node);
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
                        node.Code.A("Program.Hdg(O.GetString(" + node[0].Code + "));");
                        break;
                    case "ASTTELL":
                        {
                            string s = "false";
                            if (node.ChildrenCount() > 1)
                            {
                                s = "true";
                            }
                            node.Code.A("Program.Tell(O.GetString(" + node[0].Code + "), " + s + ");");                            
                        }
                        break;
                    case "ASTSYS":
                        {
                            if (node.ChildrenCount() == 0)
                            {
                                node.Code.A("Program.Sys(null);");
                            }
                            else
                            {
                                node.Code.A("Program.Sys(O.GetString(" + node[0].Code + "));");
                            }
                        }
                        break;
                    case "ASTHELP":
                        {
                            string txt = "null";
                            if (node.ChildrenCount() > 0) txt = "`" + node[0].Text + "`";
                            node.Code.A("Program.Help(" + txt + ");" + G.NL);
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
                            node.Code.CA(node[0].Code);
                        }
                        break;
                    case "ASTCURLYSIMPLE":
                        {                            
                            //node[0].Code = null;  //clearing this
                            node[0].Code = new GekkoSB();  //has a null inside as storage
                            HandleScalar(node[0], true, w);
                            node.Code.CA(node[0].Code);                         
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
                            else s1 = "O.GetDate(" + ss1 + ", O.GetDateChoices.FlexibleStart);" + G.NL;
                            string ss2 = node.GetChildCode(1).ToString();
                            string s2 = null;
                            if (ss2 == null) s2 = "Globals.globalPeriodEnd";
                            else s2 = "O.GetDate(" + ss2 + ", O.GetDateChoices.FlexibleEnd);" + G.NL;
                            node.Code.A("o").A(Num(node)).A(".t1 = ").A(s1).A(";").A(G.NL);
                            node.Code.A("o").A(Num(node)).A(".t2 = ").A(s2).A(";").A(G.NL);
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
                    case "ASTCOMPARE":
                        {
                            string op = node[0][0].Text;
                            string code1 = node[1].Code.ToString();
                            string code2 = node[2].Code.ToString();
                            if (op == "ASTIFOPERATOR4")  //"<"
                            {                                
                                node.Code.A("O.StrictlySmallerThan(" + code1 + "," + code2 + ", t)");
                            }
                            else if (op == "ASTIFOPERATOR6")  //"<="
                            {
                                node.Code.A("O.SmallerThanOrEqual(" + code1 + "," + code2 + ", t)");
                            }
                            else if (op == "ASTIFOPERATOR1") //"=="
                            {
                                node.Code.A("O.Equals(" + code1 + "," + code2 + ", t)");
                            }
                            else if (op == "ASTIFOPERATOR5")  //">="
                            {
                                node.Code.A("O.LargerThanOrEqual(" + code1 + "," + code2 + ", t)");
                            }
                            else if (op == "ASTIFOPERATOR3") //">"
                            {
                                node.Code.A("O.StrictlyLargerThan(" + code1 + "," + code2 + ", t)");
                            }
                            else if (op == "ASTIFOPERATOR2") //"<>"
                            {
                                node.Code.A("!O.Equals(" + code1 + "," + code2 + ", t)");
                            }
                        }
                        break;                    

                    case "ASTFORSTATEMENTS":
                        {
                            GetCodeFromAllChildren(node);
                            //AddSplitMarkers(node);                        
                        }
                        break;
                    case "ASTIFSTATEMENTS":
                        {
                            GetCodeFromAllChildren(node);
                            //AddSplitMarkers(node);                        
                        }
                        break;
                    case "ASTELSESTATEMENTS":
                        {
                            GetCodeFromAllChildren(node);
                            //AddSplitMarkers(node);                        
                        }
                        break;
                    case "ASTFUNCTIONDEFCODE":
                        {
                            GetCodeFromAllChildren(node);
                            //AddSplitMarkers(node);                        
                        }
                        break;

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
                                        s += "TimeSeries " + tempName + " = new TimeSeries(Program.options.freq, null);" + G.NL;
                                        s += "foreach (GekkoTime t2 in new GekkoTimeIterator(Globals.globalPeriodStart, Globals.globalPeriodEnd))" + G.NL;
                                        s += GekkoTimeIteratorStartCode(w, node);  //node is where this text is put below
                                        s += "    double data = O.GetVal(" + child.Code + ", t);" + G.NL;
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

                            if (w.uFunctionsHelper == null)
                            {
                                if (node.ChildrenCount() > 0)
                                {
                                    G.Writeln2("*** ERROR: RETURN with values only allowed inside FUNCTION");
                                    throw new GekkoException();
                                }
                                else
                                {
                                    //#9807235423 return problem, should it be return true?? C1(), C2(), ...
                                    node.Code.A("return;" + G.NL);  //probably the node[0].Code is always empty here (should be)
                                }
                            }
                            else
                            {
                                node.Code.A("return (" + G.GetVariableType(w.uFunctionsHelper.lhsTypes.Count) + ")(" + node[0].Code + ");" + G.NL);
                            }
                            
                            node.Code.A(Globals.splitSTART);
                        }
                        break;
                    case "ASTGOTO":
                        {
                            node.Code.A(Globals.splitSTOP);

                            node.Code.A("goto " + node[0].Text.ToLower().Trim() + ";" + G.NL);  //calls a C# label
                            w.wh.isGotoOrTarget = true;

                            node.Code.A(Globals.splitSTART);
                        }
                        break;
                    case "ASTTARGET":  //AREMOS: target
                        {
                            node.Code.A(Globals.splitSTOP);
                            
                            node.Code.A(node[0].Text.ToLower().Trim() + ":;" + G.NL);  //a C# label
                            w.wh.isGotoOrTarget = true;

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
                            node.Code.A("double " + stepName + " = " + codeStep + ".GetVal(t);" + G.NL);
                            node.Code.A("double " + startName + " = " + codeFrom + ".GetVal(t);" + G.NL);
                            node.Code.A("double " + endName + " = " + codeEnd + ".GetVal(t) + " + stepName + "/1000000d;" + G.NL);  //added a tiny bit of steplength, to guard against rounding errors
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
                            string setLoopStringCs = CacheRefScalarCs(out loopVariable, nameSimpleIdent, GetScalarCache(w), GetHeaderCs(w), EScalarRefType.Date, "Globals.tNull", false, true, false);                            
                            
                            node.Code.A(setLoopStringCs + G.NL);
                            node.Code.A("int " + stepName + " = O.GetInt(" + codeStep + ");" + G.NL);
                            node.Code.A("GekkoTime " + startName + " = O.GetDate(" + codeFrom + ");" + G.NL);
                            node.Code.A("GekkoTime " + endName + " = O.GetDate(" + codeEnd + ");" + G.NL);  //added a tiny bit of steplength, to guard against rounding errors
                            node.Code.A("ScalarDate " + tempName + " = (ScalarDate)" + loopVariable + ";" + G.NL);
                            node.Code.A("try {");
                            node.Code.A("for (" + tempName + ".date = " + startName + " ; O.ContinueIterating(" + tempName + ".date, " + endName + ", " + stepName + "); " + tempName + ".date = O.GetDate(" + tempName + ".Add(" + codeStep + ", t)))");
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

                            node.Code.A("if(" + node[0].Code + ") {");

                            node.Code.A(Globals.splitSTART);
                            node.Code.A(node[1].Code);
                            node.Code.A(Globals.splitSTOP);                            
                            
                            node.Code.A("}");
                            if (node[2] != null)
                            {
                                node.Code.A("else {");

                                node.Code.A(Globals.splitSTART);
                                node.Code.A(node[2].Code);
                                node.Code.A(Globals.splitSTOP);
                                
                                node.Code.A("}");
                            }

                            node.Code.A(Globals.splitSTART);
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
                            method += "public static " + lhsClassNameCode + " " + w.uFunctionsHelper.functionName.ToLower() + "(" + Globals.functionP2Cs + ", " + Globals.functionT2Cs + ", ";

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
                                else s += ".Add(new ScalarString(`/`), t).Add(" + child.Code + ", t)";
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
                                else s += ".Add(new ScalarString(`.`), t).Add(" + child.Code + ", t)";
                            }
                            node.Code.A(s);
                        }
                        break;
                    case "ASTURLFIRST1":
                        {
                            node.Code.CA(node[0].Code + ".Add(new ScalarString(`http://`, t)).Add(" + node[1].Code + ", t)");
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
                                else s += ".Add(new ScalarString(`\\\\`), t).Add(" + child.Code + ", t)";
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
                                else s += ".Add(new ScalarString(`.`), t).Add(" + child.Code + ", t)";
                            }
                            node.Code.A(s);
                        }
                        break;
                    case "ASTFILENAMEFIRST1":                    
                        {
                            node.Code.CA(node[0].Code + ".Add(new ScalarString(`:\\\\`), t).Add(" + node[1].Code + ", t)");
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
                            string functionName = node[0].Text.ToLower();  //no string composition allowed for functions.
                            if (functionName == "string") functionName = "tostring";                            

                            //TODO: Should these just override??? And what if inbuilt function does not exist??

                            if (Globals.lagFunctions.Contains(functionName))  //functionName is lower case
                            {
                                
                                if (Program.options.interface_lagfix)
                                {

                                    string lag1Code = null;  //for instance -1
                                    string lag2Code = null;  //for instance 0
                                    string code = null;

                                    switch (functionName)
                                    {
                                        case "movavg":
                                        case "movsum":
                                            {

                                                if (node.ChildrenCount() != 2 + 1)
                                                {
                                                    G.Writeln2("*** ERROR: Expected 2 arguments for function " + functionName);
                                                    throw new GekkoException();
                                                }
                                                lag1Code = "(-O.GetInt(" + node[2].Code.ToString() + ") + 1)";  //for instance -4, with movsum(..., 5)
                                                lag2Code = "0";                                                 //for instance 0, with movsum(..., 5)
                                                code = node[1].Code.ToString();
                                            }
                                            break;
                                        case "dif":
                                        case "diff":
                                        case "dlog":
                                        case "pch":                                        
                                            {

                                                if (node.ChildrenCount() != 1 + 1)
                                                {
                                                    G.Writeln2("*** ERROR: Expected 1 argument for function " + functionName);
                                                    throw new GekkoException();
                                                }
                                                lag1Code = "-1";
                                                lag2Code = "0";
                                                code = node[1].Code.ToString();
                                            }
                                            break;
                                                                                    
                                        case "dify":
                                        case "diffy":
                                        case "dlogy":
                                        case "pchy":
                                            {

                                                if (node.ChildrenCount() != 1 + 1)
                                                {
                                                    G.Writeln2("*** ERROR: Expected 1 argument for function " + functionName);
                                                    throw new GekkoException();
                                                }
                                                lag1Code = "(-O.CurrentSubperiods())";  //for instance -4 if freq is quarterly
                                                lag2Code = "0";
                                                code = node[1].Code.ToString();
                                            }
                                            break;

                                        default:
                                            {
                                                G.Writeln2("*** ERROR: Function '" + functionName + "' not expected");
                                                throw new GekkoException();
                                            }
                                            break;
                                    }                                                    

                                    W temp = w;
                                    
                                    //also remove parent if
                                    //w.headerCs.AppendLine("public static IVariable helper123(GekkoTime t) { return " + code + ";" + "}");

                                    int timeLoopDepth;
                                    ASTNode parentTimeLoop;
                                    SearchUpwardsInTreeForParentTimeLoopFunctions(node, out timeLoopDepth, out parentTimeLoop);

                                    int tCounter = 2 + timeLoopDepth;

                                    string storageName = "storage" + ++Globals.counter;
                                    string counterName = "counter" + ++Globals.counter;
                                    StringBuilder sb1 = new StringBuilder();
                                    sb1.AppendLine("double[] " + storageName + " = new double[" + lag2Code + " - (" + lag1Code + ") + 1];");  //remember lag1 and lag2 are <= 0
                                    sb1.AppendLine("int " + counterName + " = 0;");
                                    sb1.AppendLine("foreach (GekkoTime t" + tCounter + " in new GekkoTimeIterator(t" + (tCounter - 1) + ".Add(" + lag1Code + "), t" + (tCounter - 1) + ".Add(" + lag2Code + ")))");
                                    sb1.AppendLine("{");
                                    sb1.AppendLine("t = t" + tCounter + ";");

                                    if (node.timeLoopNestCode != null)
                                    {
                                        sb1.Append(node.timeLoopNestCode);
                                    }

                                    sb1.AppendLine("" + storageName + "[" + counterName + "] = O.GetVal(" + code + ", t);");
                                    sb1.AppendLine("" + counterName + "++;");
                                    sb1.AppendLine("}");

                                    if (parentTimeLoop == null)
                                    {
                                        G.Writeln2("*** ERROR: Internal error related to lag functions");
                                        throw new GekkoException();                                        
                                    }
                                    else
                                    {
                                        parentTimeLoop.timeLoopNestCode = sb1;
                                    }

                                    node.Code.A("O.HandleLags(`" + functionName + "`, " + storageName + ", " + lag1Code + ", " + lag2Code + ")");
                                }
                                else
                                {
                                    //This mega-hack is now switched off per default. Remove all of this at some point.
                                    if (node.ChildrenCount() > 2)
                                    {
                                        G.Writeln2("*** ERROR: Expected 1 argument for " + functionName + "() function");
                                        throw new GekkoException();
                                    }
                                    string code = node[1].Code.ToString();

                                    W temp = w;
                                                                        
                                    //HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA
                                    //HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA
                                    //HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA
                                    //HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA
                                    //HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA HACK MEGA 
                                    //How to know if a MetaTimeSeries or ScalarVal gets returned from code here...??
                                    //Functions. is allowed, for instance dif(log(...))
                                    if (code.StartsWith("Functions.") || code.StartsWith("O.Add(") || code.StartsWith("O.Divide(") || code.StartsWith("O.Multiply(") || code.StartsWith("O.Negate(") || code.StartsWith("O.Power(") || code.StartsWith("O.Subtract("))
                                    {

                                        //w.headerCs.AppendLine("public static IVariable helper123(GekkoTime t) { return " + code + ";" + "}");

                                        //code is for instance:
                                        //ser y = pch(x + 0);
                                        // ==> O.Add(ts4, i5, t)

                                        //Do this some more robust way....
                                        //first one: ___(t___ --> ___(t.Add(-1)___
                                        //next one: ___, t___ --> ___, t.Add(-1)___
                                        //string codeLag = code.Replace("(" + Globals.functionT1Cs, "(" + Globals.functionT1Cs + ".Add(-1)");
                                        //codeLag = codeLag.Replace(", " + Globals.functionT1Cs, ", " + Globals.functionT1Cs + ".Add(-1)");

                                        //first one: ___(t)___ --> ___(t.Add(-1))___
                                        //first one: ___(t,___ --> ___(t.Add(-1),___
                                        //next one: ___, t,___ --> ___, t.Add(-1),___
                                        //next one: ___, t)___ --> ___, t.Add(-1))___

                                        int lag = 1;
                                        if (G.equal(functionName, "dlogy") || G.equal(functionName, "dify") || G.equal(functionName, "diffy") || G.equal(functionName, "pchy"))
                                        {
                                            lag = O.CurrentSubperiods();
                                        }

                                        string codeLag = code.Replace("(" + Globals.functionT1Cs + ")", "(" + Globals.functionT1Cs + ".Add(-" + lag.ToString() + ")" + ")");
                                        codeLag = codeLag.Replace("(" + Globals.functionT1Cs + ",", "(" + Globals.functionT1Cs + ".Add(-" + lag.ToString() + ")" + ",");
                                        codeLag = codeLag.Replace(", " + Globals.functionT1Cs + ",", ", " + Globals.functionT1Cs + ".Add(-" + lag.ToString() + ")" + ",");
                                        codeLag = codeLag.Replace(", " + Globals.functionT1Cs + ")", ", " + Globals.functionT1Cs + ".Add(-" + lag.ToString() + ")" + ")");

                                        if (functionName == "dlog" || functionName == "dlogy")
                                        {
                                            node.Code.A("Functions.log(" + Globals.functionT1Cs + ", " + code + ").Subtract(Functions.log(" + Globals.functionT1Cs + ", " + codeLag + "), " + Globals.functionT1Cs + ")");
                                        }
                                        else if (functionName == "dif" || functionName == "diff" || functionName == "dify" || functionName == "diffy")
                                        {
                                            node.Code.A("(" + code + ").Subtract(" + codeLag + " ," + Globals.functionT1Cs + ")");
                                        }
                                        else if (functionName == "pch" || functionName == "pchy")
                                        {
                                            node.Code.A("(" + code + ").Divide(" + codeLag + ", " + Globals.functionT1Cs + ").Subtract(new ScalarVal(1d), " + Globals.functionT1Cs + ").Multiply(new ScalarVal(100d), " + Globals.functionT1Cs + ")");
                                        }
                                        else throw new GekkoException();

                                    }
                                    else
                                    {
                                        if (functionName == "dlog")
                                        {
                                            node.Code.A("Functions.dlog(" + Globals.functionT1Cs + ", " + code + ")");
                                        }
                                        else if (functionName == "dif" || functionName == "diff")
                                        {
                                            node.Code.A("Functions.dif(" + Globals.functionT1Cs + ", " + code + ")");
                                        }
                                        else if (functionName == "pch")
                                        {
                                            node.Code.A("Functions.pch(" + Globals.functionT1Cs + ", " + code + ")");
                                        }
                                        else if (functionName == "dlogy")
                                        {
                                            node.Code.A("Functions.dlogy(" + Globals.functionT1Cs + ", " + code + ")");
                                        }
                                        else if (functionName == "dify" || functionName == "diffy")
                                        {
                                            node.Code.A("Functions.dify(" + Globals.functionT1Cs + ", " + code + ")");
                                        }
                                        else if (functionName == "pchy")
                                        {
                                            node.Code.A("Functions.pchy(" + Globals.functionT1Cs + ", " + code + ")");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Globals.uFunctionStorageCs.ContainsKey(functionName))  //case-insensitive anyway
                                {
                                    node.Code.A(Globals.uProc).A(".").A(functionName).A("(").A(Globals.functionP1Cs).A(", ").A(Globals.functionT1Cs).A(", ");
                                }
                                else
                                {
                                    node.Code.A("Functions." + functionName + "(" + Globals.functionT1Cs + ", ");
                                }
                                for (int i = 1; i < node.ChildrenCount(); i++)
                                {
                                    node.Code.A(node[i].Code);
                                    if (i < node.ChildrenCount() - 1) node.Code.A(", ");
                                }

                                if (node.Code.ToString().EndsWith(", "))
                                {
                                    node.Code.CA(node.Code.ToString().Substring(0, node.Code.ToString().Length - 2));
                                }
                                node.Code.A(")");
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
                                node.Code.CA("O.Add(" + node[0].Code + ", new ScalarString(`:`), " + node[1].Code + ", t)");                             
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
                    case "ASTGENRLHSFUNCTION":
                        {
                            //GENR dlog(fy) = ...                            
                            node.Code.A(HandleGenr(node, Num(node), node[0].Code.ToString(), node[1].Code.ToString(), node[2].Code.ToString(), w, node[3].Text));
                        }
                        break;
                    case "ASTGENRINDEXER":
                        {
                            //GENR fy[2015] = ...
                            node.Code.A("O.GetTimeSeries(" + node[0].Code + ").SetData(O.GetDate(" + node[1].Code + "), O.GetVal(" + node[2].Code + ", t));" + G.NL);
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

                            if (node[0].Text == "?")
                            {
                                
                                if (node.ChildrenCount() > 1)
                                {
                                    node.Code.A("O.Matrix2.Q(" + Globals.QT + node[1].Text + Globals.QT + ");" + G.NL);
                                }
                                else
                                {
                                    node.Code.A("O.Matrix2.Q();" + G.NL);
                                }
                            }
                            else
                            {
                                //could use node[0].simpleIdent to speed up...?
                                node.Code.A("O.SetMatrixData(" + node[0].Code + ", " + node[1].Code + ");" + G.NL);
                            }
                        }
                        break;
                    case "ASTGENRLISTINDEXER":
                        {
                            node.Code.A(HandleGenr(node, Num(node), node[0].Code.ToString(), "O.GetTimeSeriesFromList(" + node[1].Code + ", " + node[2].Code + ", 1, t)", node[3].Code.ToString(), w, null));
                            //GENR #m[2] = ...
                        }
                        break;
                    case "ASTGENRLISTINDEXER2":
                        {
                            //GENR #m[2][2015] = ...
                            node.Code.A("O.GetTimeSeriesFromList(" + node[0].Code + ", " + node[1].Code + ", 1, t).ts.SetData(O.GetDate(" + node[2].Code + "), O.GetVal(" + node[3].Code + ", t));" + G.NL);
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
                    case "ASTHASH":
                        {
                            string simpleIdent = null;
                            bool stringify = false;
                            if (node.ChildrenCount() > 0 && node[0].Text == "ASTDOLLARHASHNAMESIMPLE") stringify = true;
                            if (node.ChildrenCount() > 0 && (node[0].Text == "ASTHASHNAMESIMPLE" || node[0].Text == "ASTDOLLARHASHNAMESIMPLE"))
                            {
                                simpleIdent = node[0][0].Text;
                            }

                            if (node[0].Text == "ASTLISTFILE")
                            {
                                node.Code.A("O.ZListFile(O.GetString(" + node[0][0].Code + "))");
                            }
                            else
                            {

                                if (simpleIdent != null)
                                {
                                    string fa = FindFunctionArguments(node, w, simpleIdent);
                                    if (fa != null)
                                    {
                                        node.Code.A(fa);
                                    }
                                    else
                                    {
                                        AstListHelper(node, w, simpleIdent, stringify);
                                    }
                                }
                                else
                                {
                                    node.Code.A("O.ZList(" + node[0].Code + ")");
                                }
                            }
                            break;
                        }
                    //case "ASTHASHNAMESIMPLE":
                    //    {
                    //        node.Code.CA("O.GetListFromCache(`" + node[0].Text + "`)";
                    //    }
                    //    break;
                    case "ASTIDENT":
                    case "ASTIDENTDIGIT":
                        {
                            node.Code.CA("new ScalarString(`" + node[0].Text + "`)");
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
                            node.Code.A("o" + Num(node) + ".dbUrl = `" + s + "` + O.GetString(" + node[1].Code + ");" + G.NL);
                            node.Code.A(node[2].Code);  //fileName
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
                    case "ASTREBASE":  //the REBASE command
                        {
                            node.Code.A("O.Rebase o" + Num(node) + " = new O.Rebase();" + G.NL);                                                        
                            node.Code.A(node[0].Code);

                            if (node[1][0] != null)
                            {
                                node.Code.A("o" + Num(node) + ".date1 = O.GetDate(" + node[1][0].Code + ");" + G.NL);
                            }
                            if (node[1][1] != null)
                            {
                                node.Code.A("o" + Num(node) + ".date2 = O.GetDate(" + node[1][1].Code + ");" + G.NL);
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
                            node.Code.A("O.Indexer(null, " + node[0].Code + ", t)");  //null signals that it has nothing on the left
                        }
                        break;
                    case "ASTINDEXERELEMENT":  //For ASTINDEXER, see "["
                    case "ASTINDEXERELEMENTPLUS":
                        {
                            if (node.ChildrenCount() == 2)
                            {
                                if (!node[0].Code.IsNull())
                                {
                                    node.Code.A("(" + node[0].Code + ")" + ".Add(new ScalarString(\":\"), t).Add(" + node[1].Code + ", t)");
                                }
                                else
                                {
                                    node.Code.A(node[1].Code);
                                }
                            }
                            else if (node.ChildrenCount() == 3)
                            {
                                if (node.Text == "ASTINDEXERELEMENTPLUS")
                                {
                                    G.Writeln2("*** ERROR: You cannot use '+' as starting character in ranges");
                                    throw new GekkoException();
                                }
                                if (!node[0].Code.IsNull())
                                {
                                    node.Code.A("new IVariablesFilterRange((" + node[0].Code + ")" + ".Add(new ScalarString(\":\"), t).Add(" + node[1].Code + ", t), " + node[2].Code + ")");
                                }
                                else
                                {
                                    node.Code.A("new IVariablesFilterRange(" + node[1].Code + ", " + node[2].Code + ")");
                                }
                            }
                            else throw new GekkoException();
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
                                    node.Code.CA("(" + bankCs + ").Add(new ScalarString(\":\"), t).Add(" + node[1].Code + ", t)");
                                }
                             
                            }
                            else
                            {
                                if (bankCs == null)
                                {
                                    node.Code.CA("(" + node[1].Code + ").Add(new ScalarString(\"..\"), t).Add(" + node[2].Code + ", t)");
                                }
                                else
                                {
                                    //range
                                    node.Code.CA("(" + bankCs + ").Add(new ScalarString(\":\"), t).Add(" + node[1].Code + ", t).Add(new ScalarString(\"..\"), t).Add(" + node[2].Code + ", t)");
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
                    case "ASTLISTITEM":
                        {                           

                            List<string> ss = new List<string>();
                            string number = "";
                            if (node.Parent.Text == "ASTLISTITEMS0") number = "0";
                            else if (node.Parent.Text == "ASTLISTITEMS1") number = "1";
                            else if (node.Parent.Text == "ASTLISTITEMS2") number = "2";
                            if (node.ChildrenCount() > 1)
                            {
                                G.Writeln2("*** ERROR: Unexpexted error #78963456");
                                throw new GekkoException();
                            }

                            ASTNode child = node[0];  //child is the variable, not the bank

                            string listNameCs = "o" + Num(child) + ".listItems" + number;

                            if (child.Text == "ASTNAMEWITHBANK")
                            {
                                //a
                                //b:a
                                node.Code.CA(listNameCs + ".AddRange(O.GetList(" + AstBankHelper(child, w, 1) + "));" + G.NL);
                            }
                            else if (child.Text == "ASTLISTITEMWILDRANGE")
                            {
                                //f*nz
                                node.Code.CA(listNameCs + ".Add(O.GetString(" + child.Code + "));" + G.NL);
                            }
                            else if ((child.Text == "NEGATE" && child.ChildrenCount() > 0 && child[0].Text == "ASTNAMEWITHBANK"))
                            {
                                //-a
                                node.Code.CA(listNameCs + ".AddRange(O.GetList(" + AstBankHelper(child, w, 2) + "));" + G.NL);
                            }
                            else if (child.Text == "ASTLISTWITHBANK")
                            {
                                //bank2:#m, interpreted as bank2:m1, bank2:m2, bank2:m3, ...
                                node.Code.CA(listNameCs + ".AddRange(O.GetList(" + AstBankHelperList(child, w) + "));" + G.NL);
                            }
                            else
                            {
                                //expression, for instance
                                //'a'
                                //'b:a'
                                //#m[2]
                                node.Code.CA(listNameCs + ".AddRange(O.GetList(" + child.Code + "));" + G.NL);
                            }
                                                        
                            //Node always has one child here, so this is not used anymore
                            //string cs = null;
                            //if (node[0].Text == "ASTNAMEWITHBANK")
                            //{
                            //    cs = AstBankHelper(node[0], w, 1);
                            //}
                            //else
                            //{
                            //    cs = node[0].Code.ToString();
                            //}
                            //node.Code.A("o" + Num(node) + ".listItems" + number + " = O.AddBankToListItems(o" + Num(node) + ".listItems" + number + ", O.GetString(" + cs + "));" + G.NL);
                            
                        }
                        break;
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
                            if (node.ChildrenCount() == 1 && node[0].Text == "ASTNAMEWITHBANK")
                            {
                                node.Code.CA("o" + Num(node[0]) + ".listPrefix = O.GetString(" + AstBankHelper(node[0], w, 1) + ");" + G.NL);
                            }
                            else
                            {
                                node.Code.A("o" + Num(node) + ".listPrefix = O.GetString(" + node[0].Code + ");" + G.NL);
                            }
                        }
                        break;
                    case "ASTLISTSUFFIX":
                        {
                            if (node.ChildrenCount() == 1 && node[0].Text == "ASTNAMEWITHBANK")
                            {
                                node.Code.CA("o" + Num(node[0]) + ".listSuffix = O.GetString(" + AstBankHelper(node[0], w, 1) + ");" + G.NL);
                            }
                            else
                            {
                                node.Code.A("o" + Num(node) + ".listSuffix = O.GetString(" + node[0].Code + ");" + G.NL);
                            }
                        }
                        break;
                    case "ASTLISTSTRIP":
                        {
                            if (node.ChildrenCount() == 1 && node[0].Text == "ASTNAMEWITHBANK")
                            {
                                node.Code.CA("o" + Num(node[0]) + ".listStrip = O.GetString(" + AstBankHelper(node[0], w, 1) + ");" + G.NL);
                            }
                            else
                            {
                                node.Code.A("o" + Num(node) + ".listStrip = O.GetString(" + node[0].Code + ");" + G.NL);
                            }
                        }
                        break;
                    case "ASTLISTSORT":
                        {
                            node.Code.A("o" + Num(node) + ".listSort = O.GetString(" + node[0].Code + ");" + G.NL);
                        }
                        break;
                    case "ASTLISTTRIM":
                        {
                            node.Code.A("o" + Num(node) + ".listTrim = O.GetString(" + node[0].Code + ");" + G.NL);
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
                                    else node.Code.A(".Add(" + child.Code + ", t)");
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
                                node.Code.A("O.Add(O.Add(" + node[0].Code + ", new ScalarString(`.`), t), " + node[1].Code + ", t)");
                                if (node[0].nameSimpleIdent != null && node[1].nameSimpleIdent != null)
                                {
                                    node.nameSimpleIdent = node[0].nameSimpleIdent + "." + node[1].nameSimpleIdent;
                                    node.Code.CA("new ScalarString(`" + node.nameSimpleIdent + "`)");  //overrides
                                }
                                else
                                {
                                }
                            }

                        }
                        break;
                    case "ASTNAMESLIST":
                        {
                            node.Code.A("o" + Num(node) + ".namesList = new List<string>();" + G.NL);
                            foreach (ASTNode child in node.ChildrenIterator())
                            {
                                node.Code.A("o" + Num(node) + ".namesList.Add(O.GetString(" + child.Code + "));" + G.NL);
                            }
                        }
                        break;
                    case "ASTLISTWITHBANK":
                        {
                            node.Code.A(AstBankHelperList(node, w));
                        }
                        break;
                    case "ASTNAMEWITHBANK":
                        {
                            //Must always have 2 children, ASTBANK and ASTNAMEWITHDOT
                            string lagTypeCs = null;
                            if (node[1].Text == "ASTNAMEWITHDOT")  //probably is always so, but we check it.
                            {
                                if (Globals.useDotFunctionalityInParser)
                                {
                                    lagTypeCs = node[1].dotNumber;
                                }
                            }                            

                            if (node[0].ChildrenCount() == 0 && node[1].ChildrenCount() == 1 && node[1][0].Text == "ASTNAME" && node[1][0].ChildrenCount() == 1 && node[1][0][0].Text == "ASTSCALAR")
                            {
                                G.Writeln2("*** ERROR #24737643");
                                throw new GekkoException();
                                ////For instance this structure corresponding to "%b". This is interpreted as a VAL scalar even though it might be a STRING scalar pointing to a timeseries.
                                ////ASTNAMEWITHBANK
                                ////  ASTBANK
                                ////  ASTNAMEWITHDOT
                                ////    ASTNAME
                                ////      ASTSCALAR
                                ////        ASTPERCENTNAMESIMPLE
                                ////          b
                                //node[1][0][0].Code = null;  //sub-nodes have been visited: this result gets overridden
                                //HandleScalar(node[1][0][0], false, wh2);
                                //node.Code.CA(node[1][0][0].Code;                                
                            }
                            else
                            {

                                string code = AstBankHelper(node, w, 0);
                                if (Globals.useDotFunctionalityInParser && lagTypeCs != null)
                                {
                                    //This is a fY.1 type of variable.
                                    //Why does this work, and why is 'code' not used??
                                    node.Code.CA("O.Indexer(" + node.Code + ", " + lagTypeCs + ", t)");
                                }
                                else
                                {
                                    node.Code.A(code);
                                }
                            }
                        }
                        break;
                    case "ASTNO":
                        {
                            node.Code.CA("new ScalarString(`no`)");
                        }
                        break;
                    case "NEGATE":
                        //HMMMMMMMMMMM, ASTNEGATE better name
                        {

                            if (node.IgnoreNegate)
                            {
                                //if marked as ignore-node, the after-stuff is not done for this node (typically a sub-node has signalled this).
                                node.Code.CA(node.GetChildCode(0));
                            }
                            else
                            {
                                node.Code.A("O.Negate(" + node.GetChildCode(0) + ", t)");
                            }

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
                    case "ASTOPENHELPER":
                        {
                            string as2 = null;
                            if (node[1].ChildrenCount() == 1) as2 = node[1][0].Text;                            
                            node.Code.A("o" + Num(node) + ".openFileNames.Add(new List<string>() {O.GetString(" + node[0].Code + "), `" + as2 + "`});" + G.NL);
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
                                    //s.AppendLine("Gui.ChangeWorkingFolder(Program.options.folder_working);");
                                    //Gui.ChangeWorkingFolder(Program.options.folder_working);
                                    node.Code.A("CrossThreadStuff.WorkingFolder(``);");
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
                            node.Code.CA("O.GetString(" + node[0].Code + ")");
                        }
                        break;
                    case "ASTNAMEHELPER":
                        {
                            if (node[0] != null)
                            {
                                node.Code.CA("o" + Num(node) + ".name = O.GetString(" + node[0].Code + ");" + G.NL);
                            }
                        }
                        break;
                    case "ASTIMPOSE":  //"impose = " in OLS
                        {
                            node.Code.A("o" + Num(node) + ".impose = " + node[0].Code + ";" + G.NL);                           
                        }
                        break;
                    case "ASTOLS":
                    case "ASTPRT":
                        {
                            if (node.Text == "ASTOLS")
                            {
                                node.Code.A("O.Ols o" + Num(node) + " = new O.Ols();" + G.NL);
                            }
                            else
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
                            node.Code.A("Program.CreateNewTable(O.GetString(" + node[0].Code + "));" + G.NL);
                        }
                        break;
                    case "ASTTABLENEXT":
                        {
                            node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.Next();");
                        }
                        break;
                    case "ASTTABLEPRINT":
                        {
                            if (node.ChildrenCount() > 1)
                            {
                                node.Code.A("Program.PrintTable(Program.GetTable(O.GetString(" + node[0].Code + ")), O.GetString(" + node[1].Code + "));" + G.NL);
                            }
                            else
                            {
                                node.Code.A("Program.PrintTable(Program.GetTable(O.GetString(" + node[0].Code + ")), null);" + G.NL);
                            }
                        }
                        break;
                    case "ASTTABLESETVALUES":
                        {
                            node.Code.A("O.Table.SetValues o" + Num(node) + " = new O.Table.SetValues();" + G.NL);
                            node.Code.A("o" + Num(node) + ".name = O.GetString(" + node[0].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".col = O.GetInt(" + node[1].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".t1 = O.GetDate(" + node[2].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".t2 = O.GetDate(" + node[3].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".printcode = O.GetString(" + node[5].Code + ");" + G.NL);
                            node.Code.A("o" + Num(node) + ".scale = O.GetVal(" + node[6].Code + ", t);" + G.NL);
                            node.Code.A("o" + Num(node) + ".format = O.GetString(" + node[7].Code + ");" + G.NL);

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
                            node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.Set" + ss + "Border(O.GetInt(" + node[2].Code + "), O.GetInt(" + node[3].Code + "));" + G.NL);
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
                                node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.Set" + ss + "Border(O.GetInt(" + node[2].Code + "), O.GetInt(" + node[3].Code + "));" + G.NL);                               
                            }
                            else
                            {
                                node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.Set" + ss + "Border(O.GetInt(" + node[2].Code + "));" + G.NL);                                                               
                            }
                        }
                        break;

                    case "ASTTABLEHIDELEFTBORDER":
                    case "ASTTABLEHIDERIGHTBORDER":
                        {                            
                            string ss = "Left";
                            if (node.Text == "ASTTABLEHIDERIGHTBORDER") ss = "Right";
                            node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.Hide" + ss + "Border(O.GetInt(" + node[2].Code + "));" + G.NL);
                        }
                        break;
                    case "ASTTABLESHOWBORDERS":
                        {                            
                            node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.ShowBorders();");
                        }                        
                        break;

                    case "ASTTABLESETTEXT":
                        {
                            node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.SetText(O.GetInt(" + node[2].Code + "), O.GetString(" + node[3].Code + "));");
                        }
                        break;

                    case "ASTTABLEALIGNLEFT":                        
                    case "ASTTABLEALIGNCENTER":                    
                    case "ASTTABLEALIGNRIGHT":
                        {
                            string type = "Left";
                            if (node.Text == "ASTTABLEALIGNCENTER") type = "Center";
                            else if (node.Text == "ASTTABLEALIGNRIGHT") type = "Right";
                            node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.Align" + type + "(O.GetInt(" + node[2].Code + "));");
                        }
                        break;
                    case "ASTTABLEMERGECOLS":
                        {
                            node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.MergeCols(O.GetInt(" + node[2].Code + "), O.GetInt(" + node[3].Code + "));");
                        }
                        break;

                    case "ASTTABLESETDATES":
                        {                            
                            node.Code.A("Program.GetTable(O.GetString(" + node[0].Code + ")).CurRow.SetDates(O.GetInt(" + node[2].Code + "), O.GetDate(" + node[3].Code + "), O.GetDate(" + node[4].Code + "));");                            
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
                            node.Code.A("ope" + Num(node) + ".linetype = O.GetString(" + node[0].Code + ");" + G.NL);
                        }
                        break;
                    case "ASTPRTELEMENTDASHTYPE":
                        {
                            node.Code.A("ope" + Num(node) + ".dashtype = O.GetString(" + node[0].Code + ");" + G.NL);
                        }
                        break;
                    case "ASTPRTELEMENTLINEWIDTH":
                        {
                            node.Code.A("ope" + Num(node) + ".linewidth = O.GetVal(" + node[0].Code + ", Globals.tNull);" + G.NL);
                        }
                        break;
                    case "ASTPRTELEMENTLINECOLOR":
                        {
                            node.Code.A("ope" + Num(node) + ".linecolor = O.GetString(" + node[0].Code + ");" + G.NL);
                        }
                        break;
                    case "ASTPRTELEMENTPOINTTYPE":
                        {
                            node.Code.A("ope" + Num(node) + ".pointtype = O.GetString(" + node[0].Code + ");" + G.NL);
                        }
                        break;
                    case "ASTPRTELEMENTPOINTSIZE":
                        {
                            node.Code.A("ope" + Num(node) + ".pointsize = O.GetVal(" + node[0].Code + ", Globals.tNull);" + G.NL);
                        }
                        break;
                    case "ASTPRTELEMENTFILLSTYLE":
                        {
                            node.Code.A("ope" + Num(node) + ".fillstyle = O.GetString(" + node[0].Code + ");" + G.NL);
                        }
                        break;
                    case "ASTPRTELEMENTY2":
                        {
                            node.Code.A("ope" + Num(node) + ".y2 = O.GetString(`yes`);" + G.NL);
                        }
                        break;
                    case "ASTPRTELEMENTDEC":
                        {
                            node.Code.A("ope" + Num(node) + ".dec = O.GetInt(" + node[0].Code + ");" + G.NL);
                        }
                        break;
                    case "ASTPRTELEMENTWIDTH":
                        {
                            node.Code.A("ope" + Num(node) + ".width = O.GetInt(" + node[0].Code + ");" + G.NL);
                        }
                        break;

                    case "ASTPRTELEMENTNDEC":
                        {
                            node.Code.A("ope" + Num(node) + ".ndec = O.GetInt(" + node[0].Code + ");" + G.NL);
                        }
                        break;

                    case "ASTPRTELEMENTNWIDTH":
                        {
                            node.Code.A("ope" + Num(node) + ".nwidth = O.GetInt(" + node[0].Code + ");" + G.NL);
                        }
                        break;

                    case "ASTPRTELEMENTPDEC":
                        {
                            node.Code.A("ope" + Num(node) + ".pdec = O.GetInt(" + node[0].Code + ");" + G.NL);
                        }
                        break;

                    case "ASTPRTELEMENTPWIDTH":
                        {
                            node.Code.A("ope" + Num(node) + ".pwidth = O.GetInt(" + node[0].Code + ");" + G.NL);
                        }
                        break;

                    case "ASTPRTTIMEFILTER":
                                        {
                                            node.Code.A("o").A(Num(node)).A(".timefilter = @`").A(node[0].Text).A("`;").A(G.NL);
                                        }
                                        break;


                    case "ASTOLSELEMENT":
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
                            node.Code.A("ope" + Num(node) + ".label = O.SubstituteScalarsAndLists(`" + givenLabel + "`, false);" + G.NL);
                            ASTNode child = node.GetChild("ASTPRTELEMENTOPTIONFIELD");
                            if (child != null) node.Code.A(child.Code);
                            if (node.Text == "ASTPRTELEMENT")
                            {
                                node.Code.A("bankNumbers = O.Prt.GetBankNumbers(null, Program.GetElementPrintCodes(o" + Num(node) + ", ope" + Num(node) + "));" + G.NL);
                            }
                            else if (node.Text == "ASTTABLESETVALUESELEMENT")
                            {
                                node.Code.A("bankNumbers = O.Prt.GetBankNumbers(Globals.tableOption, new List<string>(){o" + Num(node) + ".printcode}" + ");" + G.NL);
                            }
                            node.Code.A("foreach(int bankNumber in bankNumbers) {" + G.NL);  //For bankNumber = 2, no cache will ever be used to avoid confusion. Cache is only for 1 (Work).                            
                            node.Code.CA(EmitLocalCacheForTimeLooping(node.Code.ToString(), w));
                            node.Code.A("foreach (GekkoTime t2 in new GekkoTimeIterator(o" + Num(node) + ".t1.Add(-2), o" + Num(node) + ".t2))" + G.NL);
                            node.Code.A(GekkoTimeIteratorStartCode(w, node));
                            node.Code.A("O.GetVal777(" + node[0].Code + ", bankNumber, ope" + Num(node) + ", t);" + G.NL);                            
                            node.Code.A(GekkoTimeIteratorEndCode());                            
                            node.Code.A("}" + G.NL);
                            node.Code.A("o" + Num(node) + ".prtElements.Add(ope" + Num(node) + ");" + G.NL);                            
                            node.Code.A("}" + G.NL);  //avoid scope collisions
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
                                node.Code.A("o" + Num(node) + ".date = O.GetDate(" + node[3].Code + ");" + G.NL);
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
                                node.Code.A(tempName + ".from = O.GetDate(" + node[0].Code + ");" + G.NL);                                                                
                            }
                            else if (node.ChildrenCount() == 2) {
                                node.Code.A(tempName + ".from = O.GetDate(" + node[0].Code + ");" + G.NL);
                                node.Code.A(tempName + ".to = O.GetDate(" + node[1].Code + ");" + G.NL);                                                                
                            }
                            else if (node.ChildrenCount() == 3) {
                                node.Code.A(tempName + ".from = O.GetDate(" + node[0].Code + ");" + G.NL);
                                node.Code.A(tempName + ".to = O.GetDate(" + node[1].Code + ");" + G.NL);
                                node.Code.A(tempName + ".step = O.GetInt(" + node[2].Code + ");" + G.NL);
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
                                node.Code.A("o" + Num(node) + ".variable = O.GetString(" + AstBankHelper(child, w, 1) + ");" + G.NL);
                            }
                            else
                            {
                                G.Writeln2("*** ERROR: Sorry, decomposition of expressions not yet implemented in Gekko 2.0");
                                throw new GekkoException();
                                node.Code.A("o" + Num(node) + ".expressionCs = " + child.Code + ";" + G.NL);
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
                            GetCodeFromAllChildren(node);
                            node.Code.A("o" + Num(node) + ".type = @`" + node[0].Text + "`;");
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
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
                                node.Code.A("o" + Num(node) + ".fileName = O.GetString(" + node[0].Code + ");" + G.NL);
                            }
                        }
                        break;                    
                    case "ASTRUN":
                        {
                            node.Code.A("O.Run o" + Num(node) + " = new O.Run();" + G.NL);
                            //HMMM is this right:
                            node.Code.A("o" + Num(node) + ".fileName = " + node[0].Code + ".GetString();" + G.NL);
                            node.Code.A("o" + Num(node) + ".p = p;" + G.NL);
                            node.Code.A("o" + Num(node) + ".Exe();" + G.NL);
                        }
                        break;
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
                    case "ASTSCALAR":
                        {
                            HandleScalar(node, false, w);                            
                        }
                        break;
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
                            node.Code.CA("new ScalarString(@`" + s + "`)");                            
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
                            node.Code.A(node[1].Code);  //list                            
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
                            else node.Code.A("Program.Pause(O.GetString(" + node[0].Code + "));");
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
                                node.Code.A("o").A(Num(node)).A(".data[").A(i).A("] = (").A(node[i + nn][0].Code).A(").GetVal(t);").A(G.NL);
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
                                    node.Code.A("o").A(Num(node)).A(".rep[").A(i).A("] = (").A(repCs).A(").GetVal(t);").A(G.NL);
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
                                node.Code.A(HandleVal(node, node[1].Code.ToString(), w));
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

                            if (node[1].Text == "ASTFUNCTION" && (G.equal(node[1][0].Text.ToLower(), "laspchain") || G.equal(node[1][0].Text.ToLower(), "laspfixed")))
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
                                node.Code.A("Functions.HandleLasp(" + rhsCode + ", " + node[0][0][2].Code + ", " + node[0][1][2].Code + ");" + G.NL);                            
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
                                else s += ".Add(" + child.Code + ", t)";
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
            }
            
            if (relativeDepth == 1)
            {
                //#982375: if it is 0, walk the sub-tree to see...                  
                if (!w.wh.isGotoOrTarget)
                {
                    node.Code.CA("p.SetText(@`¤" + node.Line + "`);" + G.NL + node.Code + G.NL); //so errors get line numbers                                            
                }

                if (Program.options.system_code_split > 0)
                {
                    //See also #890752345
                    if (node.Text == "ASTGOTO" || node.Text == "ASTTARGET" || node.Text == "ASTFUNCTIONDEF" || node.Text == "ASTFORNAME" || node.Text == "ASTFORSTRING" || node.Text == "ASTFORVAL" || node.Text == "ASTFORDATE" || node.Text == "ASTIF")
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

        private static void SearchUpwardsInTreeForParentTimeLoopFunctions(ASTNode node, out int timeLoopDepth, out ASTNode parentTimeLoop)
        {
            timeLoopDepth = 0;
            ASTNode tmp = node.Parent;
            parentTimeLoop = null;
            while (tmp != null)
            {
                if ((
                    tmp.Text == "ASTFUNCTION" && Globals.lagFunctions.Contains(tmp[0].Text.ToLower())) 
                    || tmp.Text == "ASTOLSELEMENT" 
                    || tmp.Text == "ASTPRTELEMENT" 
                    || tmp.Text == "ASTTABLESETVALUESELEMENT"
                    || tmp.Text == "ASTGENR"
                    || tmp.Text == "ASTGENRLHSFUNCTION"
                    || tmp.Text == "ASTGENRLISTINDEXER"
                    || tmp.Text == "ASTRETURNTUPLE"
                    || (G.equal(tmp.Text, "series") && (tmp.Parent != null && tmp.Text== "ASTTUPLEITEM") && (tmp.Parent.Parent != null && tmp.Parent.Text == "ASTTUPLE"))
                    )
                {

                    timeLoopDepth++;
                    if (parentTimeLoop == null) parentTimeLoop = tmp;  //only first one                                            

                }

                tmp = tmp.Parent;
            }
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
            string stringifyString = "false"; if (stringify) stringifyString = "true";

            GekkoDictionary<string, string> listCache = GetListCache(w);

            string s = null; listCache.TryGetValue(simpleIdent, out s);
            if (s == null)
            {
                //has not been seen before
                string listWithNumber = "list" + ++Globals.counter;
                listCache.Add(simpleIdent, listWithNumber);
                GetHeaderCs(w).AppendLine("public static IVariable " + listWithNumber + " = null;");  //cannot set it to ScalarVal since it may change type...
                node.Code.A("O.GetScalarFromCache(ref " + listWithNumber + ", `" + Globals.symbolList + simpleIdent + "`, false, " + stringifyString + ")");
            }
            else
            {
                node.Code.A("O.GetScalarFromCache(ref " + s + ", `" + Globals.symbolList + simpleIdent + "`, false, " + stringifyString + ")");
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
                nodeCode += "o" + Num(node) + ".listFile = O.GetString(" + node[0].Code + ");" + G.NL;
            }
            else
            {
                nodeCode += "o" + Num(node) + ".name = O.GetString(" + node[0].Code + ");" + G.NL;
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
                    nodeCodeTemp += "double " + tempDoubleCs + " = (" + childCode + ").GetVal(t);" + G.NL;
                    string notUsed = null;
                    string leftSideCs = CacheRefScalarCs(out notUsed, node0.nameSimpleIdent, GetScalarCache(w), GetHeaderCs(w), EScalarRefType.Val, tempDoubleCs, false, true, false);
                    nodeCodeTemp += leftSideCs + G.NL;
                }
                else
                {
                    //fancy name like VAL x|%y = ... --> this will be slow!                                
                    nodeCodeTemp += "O.SetValData(" + node0.Code + ", " + childCode + ", t);" + G.NL;
                }
            }
            return nodeCodeTemp;
        }

        private static string HandleGenr(ASTNode node, string numNode, string childCodePeriod, string childCodeLhsName, string childCodeRhs, W w, string lhsFunction)
        {
            string nodeCode = null;
            nodeCode += "O.Genr o" + numNode + " = new O.Genr();" + G.NL;
            nodeCode = EmitLocalCacheForTimeLooping(nodeCode, w);
            nodeCode += childCodePeriod + G.NL;  //dates
            nodeCode += "o" + numNode + ".lhs = null;" + G.NL;
            nodeCode += "o" + numNode + ".p = p;" + G.NL;
            nodeCode += "foreach (GekkoTime t2 in new GekkoTimeIterator(o" + numNode + ".t1, o" + numNode + ".t2))" + G.NL;
            nodeCode += GekkoTimeIteratorStartCode(w, node);
            nodeCode += "  double data = O.GetVal(" + childCodeRhs + ", t);" + G.NL;
            nodeCode += "if(o" + numNode + ".lhs == null) o" + numNode + ".lhs = O.GetTimeSeries(" + childCodeLhsName + ");" + G.NL; //we want the rhs to be constructed first, so that SERIES xx1 = xx1; fails if y does not exist (otherwist it would have been autocreated).                        
            //nodeCode += "  double dataLag = O.GetVal(o" + numNode + ".lhs, t.Add(-1));" + G.NL;
            if (lhsFunction == null)
            {
                nodeCode += "o" + numNode + ".lhs.SetData(t, data);" + G.NL;
            }
            else if (G.equal(lhsFunction, "log"))
            {
                nodeCode += "o" + numNode + ".lhs.SetData(t, Math.Exp(data));" + G.NL;
            }
            else if (G.equal(lhsFunction, "dlog"))
            {
                nodeCode += "o" + numNode + ".lhs.SetData(t, o" + numNode + ".lhs.GetData(t.Add(-1)) * Math.Exp(data));" + G.NL;
            }
            else if (G.equal(lhsFunction, "pch"))
            {
                nodeCode += "o" + numNode + ".lhs.SetData(t, o" + numNode + ".lhs.GetData(t.Add(-1)) * (data/100d + 1));" + G.NL;
            }
            else if (G.equal(lhsFunction, "dif") || G.equal(lhsFunction, "diff"))
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

        private static string GekkoTimeIteratorStartCode(W w, ASTNode node)
        {            
            string nodeCode = Globals.startGekkoTimeIteratorCode;
            //if (w.wh.timeLoopCode != null) nodeCode += w.wh.timeLoopCode.ToString();

            if (node.timeLoopNestCode != null) nodeCode += node.timeLoopNestCode.ToString();
                      
            return nodeCode;
        }

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

        private static string EmitLocalCacheForTimeLooping(string s, W wh2)
        {
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

        private static void HandleScalar(ASTNode node, bool isCurlyWithoutPercent, W w)
        {
            bool stringify = false;
            if (node.ChildrenCount() > 0 && (node[0].Text == "ASTDOLLARPERCENTNAMESIMPLE" || node[0].Text == "ASTDOLLARPERCENTPAREN")) stringify = true;
            
            bool transformationAllowed = true;
            bool isPartOfComposedName = false;
            
            if ((node.Number == 1 && node.Parent.Text == "ASTNAMEWITHBANK") 
                || node.Parent.Text == "ASTNAME"
                || node.Parent.Text == "ASTCURLY"
                || node.Parent.Text == "ASTCURLYSIMPLE")
            {
                //For instance base:%s. If %s is NAME 'fy', this would be equal to base:fy.                            
                //In that case, O.GetScalar must not be allowed to transform the string/name into a timeseries,
                //because we are going to look up the timeseries in the databank (in the example: base databank).
                //Therefore we call an overload of O.GetScalar()
                transformationAllowed = false;
            }

            if (Globals.nameFix)
            {
                if (node.Parent.Text == "ASTLISTITEM")
                {
                    transformationAllowed = false;
                }                
            }

            if ((node.Parent.Text == "ASTNAME" && node.Parent.ChildrenCount() > 1)
                || (node.Parent.Text == "ASTCURLY" && node.Parent.ChildrenCount() == 1))
            {
                isPartOfComposedName = true; //composed names cannot be looked up in cache                                            
            }
            
            string scalarSimpleIdent = null;
            if (isCurlyWithoutPercent)
            {
                // {s}
                scalarSimpleIdent = node.Text;
            }
            else
            {
                if (node[0].Text == "ASTPERCENTNAMESIMPLE" || node[0].Text == "ASTDOLLARPERCENTNAMESIMPLE")
                {
                    // %s, not %(...), the %s may be inside {} like {%s}
                    scalarSimpleIdent = node[0][0].Text;
                }
            }

            if (scalarSimpleIdent != null)
            {
                //either {s} or %s
                string fa = FindFunctionArguments(node, w, scalarSimpleIdent);
                if (fa != null)
                {
                    node.Code.A(fa);  //????????? What is this????????
                }
                else
                {                   

                    //hmmm why do we have isPartOfComposedName and transformationAllowed at the same time
                    //is it not the same thing?

                    if (!isPartOfComposedName && w.wh.localStatementCache != null)
                    {
                        //not for instance a%s or %(%s) but clean %s, and part of GENR statement                        
                        //In that case, we look for the variable in the local GENR cache
                        string refCode = "ts" + ++Globals.counter;

                        string fallBackCode = null;

                        string t = "false";
                        if (transformationAllowed) t = "true";
                        string s = "false";
                        if (stringify) s = "true";

                        fallBackCode = "O.GetScalar(`" + scalarSimpleIdent + "`, " + t + ", " + s + ")";
                        
                        string xx = null; w.wh.localStatementCache.TryGetValue(fallBackCode, out xx);
                        if (xx != null)
                        {
                            //This complicated timeseries (or scalar) has been seen before in this particular GENR statement                        
                            node.Code.CA(xx);
                        }
                        else
                        {
                            //has not been seen before
                            w.wh.localStatementCache.Add(fallBackCode, refCode);
                            node.Code.CA(refCode);
                        }
                    }
                    else
                    {
                        string notUsed = null;
                        node.Code.A(CacheRefScalarCs(out notUsed, scalarSimpleIdent, GetScalarCache(w), GetHeaderCs(w), EScalarRefType.OnRightHandSide, null, false, transformationAllowed, stringify));
                    }
                }
            }
            else
            {
                //not {s} or {%s}, but something like {%s1+%s2}
                node.Code.A("O.ZScalar(" + node[0].Code + ")");
            }
        }

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
                    if (G.equal(fah.parameterName, simpleIdent))
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


        private static string AstBankHelperList(ASTNode node, W wh2)
        {
            string bankNumberCode = null;
            if (wh2.wh.currentCommand == "ASTPRT")
            {
                bankNumberCode = "bankNumber";
            }
            else
            {
                bankNumberCode = "1";  //default: Work
            }
            string code = "O.GetListWithBankPrefix(" + node[0].Code + ", " + node[1].Code + ", " + bankNumberCode + ")";            
            return code;
        }
        
        
        private static string AstBankHelper(ASTNode node, W wh2, int type)
        {
            string isLhsSoCanAutoCreate = null;
            if ((node.Number == 1 && (node.Parent.Text == "ASTGENR" || node.Parent.Text == "ASTGENRLHSFUNCTION")) || node.Parent.Text == "ASTTUPLEITEM")
            {
                isLhsSoCanAutoCreate = ", O.ECreatePossibilities.Can";
            }
            else if (node.Number == 0 && node.Parent.Text == "ASTCREATEEXPRESSION")
            {
                isLhsSoCanAutoCreate = ", O.ECreatePossibilities.Must";
            }
            
            string bankNumberCode = null;
            if (wh2.wh.currentCommand == "ASTPRT" || wh2.wh.currentCommand == "ASTTABLESETVALUES")
            {
                bankNumberCode = "bankNumber";
            }
            else
            {
                bankNumberCode = "1";  //default: Work
            }

            if (type == 1)
            {
                string listFallBackCode = null;
                if (node[0].ChildrenCount() == 0)
                {
                    listFallBackCode = node[1].Code.ToString();  //this is a ScalarString already, and we want to avoid a superfluous 'Work:a' for an 'a' item. So this will run ok fast for simple items.
                }
                else
                {
                    listFallBackCode = "new ScalarString(O.GetString(" + node[0].Code + ") + `:` + O.GetString(" + node[1].Code + "))";
                }
                return listFallBackCode;
            }
            else if (type == 2) //minus
            {
                string listFallBackCode = null;
                if (node[0][0].ChildrenCount() == 0)
                {
                    listFallBackCode = "O.Add(new ScalarString(`-`), " + node[0][1].Code + ", t)";  //this is a ScalarString already, and we want to avoid a superfluous 'Work:a' for an 'a' item. So this will run ok fast for simple items.
                }
                else
                {
                    listFallBackCode = "new ScalarString(O.GetString(" + node[0][0].Code + ") + `:-` + O.GetString(" + node[0][1].Code + "))";
                }
                return listFallBackCode;
            }

            string fallBackCode = "O.GetString(" + node[0].Code + ") + `:` + O.GetString(" + node[1].Code + ")";
            
            string simpleHash = null;
            //node[1].ChildrenCount() is always > 0
            if (node[0].ChildrenCount() > 0)
            {
                //there is a bank like a:b or %x:b or %x:%y
                if (node[0][0].nameSimpleIdent != null && node[1][0].nameSimpleIdent != null)
                {
                    //simple names like a:b
                    simpleHash = node[0][0].nameSimpleIdent + ":" + node[1][0].nameSimpleIdent;
                }
            }
            else
            {
                //name like a or %x
                if (node[1][0].nameSimpleIdent != null)
                {
                    //simple name like a
                    simpleHash = node[1][0].nameSimpleIdent;
                }
            }

            bool isSimple = false; if (simpleHash != null) isSimple = true;
                        
            string code = null;
            
            string fa;
            int choice;
            GetChoice(node, wh2, simpleHash, isSimple, out fa, out choice);

            if (choice == 1)
            {
                node.Code.A(fa);
            }
            else if (choice == 2)
            {
                //isSimple means that the name is simple like a or b:a.
                //Then we look for it in the global cache

                GekkoDictionary<string, string> tsCache = GetTsCache(wh2);

                string s = null; tsCache.TryGetValue(simpleHash, out s);
                if (s == null)
                {
                    //has not been seen before
                    string ivWithNumber = "iv" + ++Globals.counter;
                    tsCache.Add(simpleHash, ivWithNumber);
                    GetHeaderCs(wh2).AppendLine("public static IVariable " + ivWithNumber + " = null;");  //cannot set it to ScalarVal since it may change type...                    
                    node.Code.A("O.GetTimeSeriesFromCache(ref " + ivWithNumber + ", `" + simpleHash + "`, " + bankNumberCode + isLhsSoCanAutoCreate + ")");
                }
                else
                {
                    node.Code.A("O.GetTimeSeriesFromCache(ref " + s + ", `" + simpleHash + "`, " + bankNumberCode + isLhsSoCanAutoCreate + ")");
                }
            }
            else if (choice == 3)
            {
                //This means that the name is complicated like %x or b:%x or %y:a or %x:%y (or fx%i)                
                if (wh2.wh.localStatementCache != null)
                {
                    //GENR statement for instance, maybe also VAL if indexer fY[2010]??
                    //This means there is a GENR statement at the top of the AST tree
                    //In that case, we look for the variable in the local cache
                    string fallBackCode2 = "O.GetTimeSeries(" + fallBackCode + ", " + bankNumberCode + isLhsSoCanAutoCreate + ")";  //here, bankNumberCode will always be = "1", since this is not a PRT statement
                    string xx = null; wh2.wh.localStatementCache.TryGetValue(fallBackCode2, out xx);
                    if (xx != null)
                    {
                        //This complicated timeseries (or scalar) has been seen before in this particular GENR/PRT statement                        
                        code = xx;
                    }
                    else
                    {
                        string refCode = "ts" + ++Globals.counter;
                        wh2.wh.localStatementCache.Add(fallBackCode2, refCode);
                        code = refCode;
                    }
                }
                else
                {
                    node.Code.A("O.GetTimeSeries(" + fallBackCode + ", " + bankNumberCode + isLhsSoCanAutoCreate + ")");
                    //Complicated name, but not inside a GENR statement: just use the statement directly without use of any caches
                }
            }
            else throw new GekkoException();
            return code;
        }

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
        //    //This TimeSeries is not known, seen for the first time in the file.
        //    string[] split = hash.Split(',');
        //    string bank = split[0].Trim();
        //    string variable = split[1].Trim();
        //    string hash2 = "O.GetString(" + bank + "), O.GetString(" + variable + ")";
                        
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
            return o + ".Add(new OptString(`" + type + "`, O.GetString(" + s + ")));" + G.NL;
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
                        if (G.equal(child.GetChild(0).Text, "astyes") || G.equal(child.GetChild(0).Text, "true"))
                        {
                            s1a.Append("true");
                        }
                        else if (G.equal(child.GetChild(0).Text, "astno") || G.equal(child.GetChild(0).Text, "false"))
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
                        type = "string";
                        string temp = "";                        
                        string s7 = null;
                        if (child[0].Code.IsNull())
                        {
                            s7 = "`" + child[0].Text + "`";
                        }
                        else
                        {
                            s7 = "O.GetString(" + child[0].Code + ")";
                        }
                        if (G.equal(node[0].Text, "freq"))  //OPTION freq = ...
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

            string sss = s1a.ToString();
            s1 = s1.Replace("_", " ");
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
                //s.AppendLine("G.Writeln(" + s1.ToString() + "O.GetVal(`" + s3.ToString() + "`));");
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
                        if (G.equal(child.GetChild(0).Text, "yes") || G.equal(child.GetChild(0).Text, "true"))
                        {
                            s1a.Append("true");
                        }
                        else if (G.equal(child.GetChild(0).Text, "no") || G.equal(child.GetChild(0).Text, "false"))
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

                        //    temp += ("O.GetString(" + child[0].Code + ")");  //@ because it can contain slashes
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
                            s7 = "O.GetString(" + child[0].Code + ")";                            
                        }
                        if (G.equal(node[0].Text, "freq"))  //OPTION freq = ...
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
        
        //Created when running a .cmd/.gcm file
        public WalkHelper wh = null;
        //public ParserGekWalkASTAndEmit.ELastCommand lastCommand;
        public Dictionary<int, List<string>> prtItems;
        public Dictionary<int, List<string>> prtLabels;
        public string fileNameContainingParsedCode = null;
        public int commandLinesCounter = -1;
        public int expressionCounter = -1;

        public GekkoDictionary<string, string> scalarCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public GekkoDictionary<string, string> listCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public GekkoDictionary<string, string> tsCache = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public StringBuilder headerCs = new StringBuilder(); //stuff to be put at the very start.
        public StringBuilder headerMethodTsCs = new StringBuilder(); //stuff to clear TimeSeries pointers
        public StringBuilder headerMethodScalarCs = new StringBuilder(); //stuff to clear scalar pointers   
        
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
        //created for each new command (except IF, FOR, etc -- hmm is this true now?)
        public GekkoDictionary<string, string> localStatementCache = null;
        //public StringBuilder localStatementCode = null;
        public string currentCommand = null;
        public bool isGotoOrTarget = false;
        //public StringBuilder timeLoopCode = null;  //stuff to put into the GekkoTime t2 = ... loop (handles lag sub-loops)        
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
