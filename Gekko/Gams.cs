using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Text;
using System.IO;
using System.Drawing;
using ProtoBuf;
using ProtoBuf.Meta;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Antlr.Runtime.Debug;
using System.Collections;
using System.Windows.Forms;
using GAMS;
using System.Xml;
using System.Threading.Tasks;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Security.Cryptography;

namespace Gekko
{

    public enum EExtractTimeDimension
    {
        Full,
        NoIndexListOfStrings
    }

    public class ExtractTimeDimensionHelper
    {
        public string name = null;
        public GekkoTime time = GekkoTime.tNull;
        public string resultingFullName = null;
        public List<string> indexes = null;
    }

    public static class GamsModel  //The rest of this class is in GamsWrappers.cs
    {   
        
        private static void Compile5(List<string> eqsCs, Func<int, double[], double[][], double[], int[][], int[][], int, double>[] functions)
        {
            //NOTE: for each processor, about 2000 eqs max, else sub-chunk!

            DateTime dt0 = DateTime.Now;

            int n = eqsCs.Count;
            int threads = Program.options.system_threads;  //5 seems pretty good for this, maybe around 2000 eqs per chunk
            int eqsPerChunk = Globals.eqsPerChunk;

            List<List<TwoInts>> chunks = Chunker(n, threads, eqsPerChunk);

            Parallel.ForEach(chunks, () => 0, (x, pls, index, s) =>
            {
                List<TwoInts> chunkList = chunks[(int)index];
                foreach (TwoInts chunk in chunkList)
                {

                    DateTime dt1 = DateTime.Now;
                    StringBuilder code = new StringBuilder();

                    code.AppendLine("using System;");
                    code.AppendLine("using System.Collections.Generic;");
                    code.AppendLine("using System.Text;");
                    code.AppendLine("namespace Gekko");
                    code.AppendLine("{");
                    code.AppendLine("public class Equations");
                    code.AppendLine("{");
                    code.AppendLine("public static void Residuals(Func<int, double[], double[][], double[], int[][], int[][], int, double>[] functions)");
                    code.AppendLine("{");
                    for (int i = chunk.int1; i < chunk.int2; i++)
                    {
                        code.AppendLine("functions[" + i + "] = (i, r, a, c, bb, dd, t) =>");
                        code.AppendLine("{"); //start dynamic function
                        code.AppendLine("int[] b = bb[i];");
                        code.AppendLine("int[] d = dd[i];");
                        code.AppendLine("double sum = 0d;");
                        code.AppendLine(eqsCs[i]);
                        code.AppendLine("return sum;");
                        code.AppendLine("};");  //end dynamic function
                        code.AppendLine();
                    }
                    code.AppendLine("}");  //method
                    code.AppendLine("}");  //end class
                    code.AppendLine("}");  //end namespace

                    CompilerParameters compilerParams = new CompilerParameters();
                    compilerParams = new CompilerParameters();
                    compilerParams.CompilerOptions = Program.GetCompilerOptions();
                    compilerParams.GenerateInMemory = true;
                    compilerParams.IncludeDebugInformation = false;
                    compilerParams.ReferencedAssemblies.Add("system.dll");
                    Parser.Frm.ParserFrmCompileAST.ReferencedAssembliesGekko(compilerParams);
                    compilerParams.GenerateExecutable = false;
                    string s2 = code.ToString();
                    CompilerResults cr = null;
                    try
                    {
                        cr = Globals.iCodeCompiler.CompileAssemblyFromSource(compilerParams, s2);
                    }
                    catch (Exception e)
                    {
                        new Error("Compilation failed");
                    }
                    Assembly assembly = cr.CompiledAssembly;
                    DateTime dt2 = DateTime.Now;
                    Object[] o = new Object[1] { functions };
                    assembly.GetType("Gekko.Equations").InvokeMember("Residuals", BindingFlags.InvokeMethod, null, null, o);  //the method                                                                                                                                                  
                }
                return 0;
            }, _ => { });

            if (Globals.runningOnTTComputer) new Writeln("TTH: Complete Compile5 --> : " + G.Seconds(dt0));
        }


        /// <summary>
        /// Splits up equations (by int number 0...n-1) in the number of threads, 
        /// and for each thread cuts up so 
        /// that the number of equations in each method is not larger than eqsPerChunk.
        /// Afterwards, foreach (List&lt;TwoInts> c1 in chunks) { foreach (TwoInts c2 in c1)
        /// { for (int i = c2.int1; i&lt;c2.int2; i++) { ... will loop i from 0 to n-1 (including)
        /// with increment 1 and no holes. Here, chunks is the return value from method.
        /// For n &lt; 500, no chunking is done. Method has been tested on all 1 mio. combinations of
        /// n = 1..100, threads=1..100 and eqsPerChunk=1..100, and in all cases the resulting
        /// triple loop loops through the n values 0..n-1 with increment 1 and no holes.
        /// So method seems safe for input values > 0, not "forgetting" any equations. 
        /// Note: eqsPerChunk should be understood as max eqs per chunk.
        /// </summary>         
        public static List<List<TwoInts>> Chunker(int n, int threads, int eqsPerChunk)
        {
            List<List<TwoInts>> chunks = new List<List<TwoInts>>();

            List<TwoInts> chunksTemp = new List<TwoInts>();
            int k1 = n / threads;  //eqs per thread
            for (int j1 = 0; j1 < threads - 1; j1++)
            {
                //over threads-1                    
                chunksTemp.Add(new TwoInts(j1 * k1, (j1 + 1) * k1));
            }
            chunksTemp.Add(new TwoInts((threads - 1) * k1, n));

            foreach (TwoInts xx in chunksTemp)
            {
                List<TwoInts> chunksFor1Thread = new List<TwoInts>();
                int count = xx.int2 - xx.int1;
                int splits = count / eqsPerChunk + 1;

                for (int j2 = 0; j2 < splits - 1; j2++)
                {
                    //over threads-1                    
                    chunksFor1Thread.Add(new TwoInts(xx.int1 + j2 * eqsPerChunk, xx.int1 + (j2 + 1) * eqsPerChunk));
                }
                chunksFor1Thread.Add(new TwoInts(xx.int1 + (splits - 1) * eqsPerChunk, xx.int1 + count));
                chunks.Add(chunksFor1Thread);
            }

            return chunks;
        }

        private static bool DetectNullNode(CommonTree ast)
        {
            return ast.Text == null && !(ast.Children != null && ast.Children.Count > 0);
        }

        public static void Compile2(CommonTree ast, ASTNodeGAMS cmdNode, int depth, CommonTokenStream tokens, bool print)
        {
            if (DetectNullNode(ast))
            {
                //not sure why this happens in ANTLR: some empty CommonTree nodes
                //we filter them out: otherwise they just create empty
                //lines in the generated C# code (with linenumber=0 which is no good either)
                //No children = we are not cutting anything real from the AST tree anyway
                return;
            }

            cmdNode.Text = ast.Text;
            cmdNode.Line = ast.Line;

            CommonTree xx = (CommonTree)ast;
            int iStart = xx.TokenStartIndex;
            if (iStart - 1 >= 0)
            {
                CommonToken xxx = (CommonToken)tokens.Get(iStart - 1);
                if (xxx.Text.Trim() == "")
                {
                    cmdNode.leftBlanks = xxx.Text;
                }
            }

            if (print)
            {
                int length = 0;
                if (cmdNode.leftBlanks != null) length = cmdNode.leftBlanks.Length;
                using (Writeln text = new Writeln())
                {
                    text.MainAdd("|" + G.Blanks(depth * 2) + cmdNode.Text + "     [" + length + "]");
                    text.MainOmitVeryFirstNewLine();
                }
            }

            if (ast.Children == null)
            {
                return;
            }

            int num = ast.Children.Count;
            cmdNode.CreateChildren(num);
            for (int i = 0; i < num; ++i)
            {
                CommonTree d = (CommonTree)(ast.Children[i]);
                if (DetectNullNode(d)) continue;
                ASTNodeGAMS cmdNodeChild = new ASTNodeGAMS(null);  //unknown text
                cmdNodeChild.Parent = cmdNode;
                cmdNode.Add(cmdNodeChild);
                Compile2(d, cmdNodeChild, depth + 1, tokens, print);
            }
        }        

        public static void Compile3(ASTNodeGAMS node, int depth, WalkHelper wh, Controlled controlled)
        {            
            foreach (ASTNodeGAMS child in node.ChildrenIterator())
            {
                Compile3(child, depth + 1, wh, controlled);
            }
            Compile3After(node, wh, controlled);
        }        

        private static void Compile3After(ASTNodeGAMS node, WalkHelper wh, Controlled controlled)
        {
            switch (node.Text?.ToUpper())
            {
                case "ASTGAMS":
                    {
                        //No need to gather GAMS statements in one lump, we take it from the children
                        //foreach (ASTNodeGAMS child in node.ChildrenIterator())
                        //{
                        //    node.Code.A(child.Code).End();
                        //}
                    }
                    break;
                case "ASTEXPRESSION":
                    {
                        node.Code.A(node[0].Code);
                    }
                    break;
                case "ASTEQU":
                    {                        
                        node.Code.A("r[" + wh.eqNames.Count + "] = " + node[2].Code + "-(" + node[3].Code + ")");
                        wh.eqNames.Add(node[1][0].Text);
                    }
                    break;
                case "ASTEQU2":
                    {
                        //LHS
                        node.Code.A(node[0].Code);
                    }
                    break;
                case "ASTEQU3":
                    {
                        //RHS
                        node.Code.A(node[0].Code);
                    }
                    break;
                case "ASTVARWI":
                    {
                        //Can be both in ASTEQU -> ASTEQU1, and in ASTVALUE. The former case is more restricte syntax-wise.
                        if (node.Parent.Text == "ASTEQU1")
                        {
                            List<string> sets = new List<string>();
                            //equation definition, defining sets that the eq is looping over
                            string eqName = node[1].Text;  //[2] is not used here
                            ASTNodeGAMS child = node?[3]?[0]?[1]?[1];
                            if (child != null && child.ChildrenCount() > 0)
                            {
                                //the equation has indexes (controlled sets)
                                foreach (ASTNodeGAMS child2 in child.ChildrenIterator())
                                {
                                    if (child2.ChildrenCount() != 1)
                                    {
                                        new Error("In equation '" + eqName + "': expected simple all controlled sets to be simple names");
                                    }
                                    string s = child2?[0].Text;
                                    if (G.IsIdent(s))
                                    {
                                        sets.Add(s);
                                    }
                                    else
                                    {
                                        new Error("Expected simple set name, not this: " + s);
                                    }
                                }
                            }
                            ASTNodeGAMS childDollar = node?[4]?[0]?[1];

                            //Imagine we have e1[i, j] $ (i0(i) and (i.val > 30 or j.val > 40)) ..
                            //The logical values are backed up, resulting into for instance
                            //true and (false or true)

                            //maybe get this as C# code, depending on i and j sets and returning true/false.
                            //from List<string>sets, we can get the combinations of i and j elements.

                        }
                        else
                        {
                            //defining a variable or parameter (or even set condition like tx0(t))
                            string varname = node[1][0].Text.Trim();
                            string varname2 = varname;
                            if (wh.dictVars != null)
                            {
                                varname2 = wh.dictVars[int.Parse(varname.Substring(1))];
                            }
                            
                            ExtractTimeDimensionHelper helper = ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, varname2, true);
                            if (wh.time1.IsNull() || (helper.time.StrictlySmallerThan(wh.time1))) wh.time1 = helper.time;
                            if (wh.time2.IsNull() || (helper.time.StrictlyLargerThan(wh.time2))) wh.time2 = helper.time;
                            int i1 = helper.time.Subtract(wh.time0);
                            int i2 = wh.dictA.Count;
                            if (!wh.dictA.ContainsKey(helper.resultingFullName))
                            {
                                wh.dictA.Add(helper.resultingFullName, i2);
                            }
                            else
                            {
                                i2 = wh.dictA[helper.resultingFullName];
                            }
                            node.Code.A("a[" + i1 + "][" + i2 + "]");  //time can be tNull for timeless
                        }
                    }
                    break;
                case "ASTIDX":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "ASTIDXELEMENTS":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "ASTVARIABLEANDLEAD":
                    {
                        if (node.ChildrenCount() > 1)
                        {
                            //x[a+1], x[a-1]
                            //control...
                        }
                        else
                        {
                            if (node.Text.StartsWith("'") || node.Text.StartsWith("\""))
                            {
                                //x['a'], x["a"]
                                node.Code.A(node.Text.Substring(1, node.Text.Length - 2));
                            }
                            else
                            {
                                //control...
                            }
                        }
                    }
                    break;
                case "ASTCONDITIONAL":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "OR":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "AND":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "NOT":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "NONEQUAL":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "LESSTHANOREQUAL":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "GREATERTHANOREQUAL":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "EQUAL":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "LESSTHAN":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "GREATERTHAN":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "+":
                    {
                        if (wh.useMFunctions) node.Code.A("M.Add(" + node[0].Code + ", " + node[1].Code + ")");
                        else node.Code.A("(" + node[0].Code + " + " + node[1].Code + ")");
                    }
                    break;
                case "-":
                    {
                        if (wh.useMFunctions) node.Code.A("M.Subtract(" + node[0].Code + ", " + node[1].Code + ")");
                        else node.Code.A("(" + node[0].Code + " - " + node[1].Code + ")");
                    }
                    break;
                case "*":
                    {
                        if (wh.useMFunctions) node.Code.A("M.Multiply(" + node[0].Code + ", " + node[1].Code + ")");
                        else node.Code.A("(" + node[0].Code + " * " + node[1].Code + ")");
                    }
                    break;
                case "/":
                    {
                        if (wh.useMFunctions) node.Code.A("M.Divide(" + node[0].Code + ", " + node[1].Code + ")");
                        else node.Code.A("(" + node[0].Code + " / " + node[1].Code + ")");
                    }
                    break;
                case "**":
                    {
                        node.Code.A("M.Power(" + node[0].Code + ", " + node[1].Code + ")");
                    }
                    break;
                case "NEGATE":
                    {
                        if (wh.useMFunctions) node.Code.A("M.Negate(" + node[0].Code + ")");
                        else node.Code.A("(-" + node[0].Code + ")");
                    }
                    break;
                case "ASTDOLLAREXPRESSION":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "ASTEXPRESSION1":
                    {
                        node.Code.A(node[0].Code);
                    }
                    break;
                case "ASTEXPRESSION2":
                    {
                        node.Code.A(node[0].Code);
                    }
                    break;
                case "ASTEXPRESSION3":
                    {
                        node.Code.A(node[0].Code);
                    }
                    break;
                case "ASTVALUE":
                    {
                        node.Code.A(node[0].Code);
                    }
                    break;
                case "ASTFUNCTION":
                    {
                        string fname = node[1][0].Text;
                        string code = null;
                        if (G.Equal(fname, "log")) code = "M.Log(";
                        else if (G.Equal(fname, "exp")) code = "M.Exp(";
                        else if (G.Equal(fname, "abs")) code = "M.Abs(";
                        else if (G.Equal(fname, "max")) code = "M.Max(";
                        else if (G.Equal(fname, "min")) code = "M.Min(";
                        else if (G.Equal(fname, "power")) code = "M.Power(";
                        else if (G.Equal(fname, "sqr")) code = "M.Sqr(";
                        else if (G.Equal(fname, "sqrt")) code = "M.Sqrt(";
                        else if (G.Equal(fname, "tanh")) code = "M.Tanh(";

                        ASTNodeGAMS elements = node[2][0][1];
                        foreach (ASTNodeGAMS child in elements.ChildrenIterator())
                        {
                            code += child.Code.ToString() + ", ";
                        }
                        code = code.Substring(0, code.Length - ", ".Length);
                        code += ")";
                        node.Code.A(code);
                    }
                    break;                
                case "ASTSUM":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "ASTSUMCONTROLLED":
                    {
                        new Error("Not implemented");
                    }
                    break;
                case "ASTDOUBLE":
                    {
                        node.Code.A(node[0].Text);
                    }
                    break;
                case "ASTINTEGER":
                    {
                        node.Code.A(node[0].Text);
                    }
                    break;

            }
        }

        /// <summary>
        /// From a varname like x[i,j,2025] it extracts name "x", GekkoTime 2025a1, the resulting full name x[i,j], and the indexes ["i", "j"].
        /// For a name without blanks and year time last, like "x[i,j,2025]", the method can return resultingFullName and time much faster (with name and indexes both = null).
        /// With allowFast, it looks for a four-digit annual time as LAST index.
        /// </summary>
        public static ExtractTimeDimensionHelper ExtractTimeDimension(bool allowFast, EExtractTimeDimension settings, string varname, bool errorIfTimeNotFound)
        {
            bool simple = false;
            ExtractTimeDimensionHelper helper = new ExtractTimeDimensionHelper();

            //fast chop up of stuff like x[a,b,2022], with no blanks.
            if (allowFast)
            {
                simple = ExtractTimeDimensionHelper2(settings, varname, helper);
            }

            if (!simple)
            {
                //For some reason, this is really slow.
                //It is ok for a quarterly scalar-timeless model from .frm, but not  
                //for instance 1 million quarterly equations.
                List<string> fullName = new List<string>();
                string start = null;
                int i = varname.IndexOf('[');
                if (i >= 1)
                {
                    //with index (...)                                    
                    start = varname.Substring(0, i).Trim();
                    string rest = varname.Substring(i).Trim();
                    string rest2 = rest.Substring(1, rest.Length - 2);
                    string[] ss = rest2.Split(',');
                    int int2 = -12345;
                    for (int j = 0; j < ss.Length; j++)
                    {
                        string s = ss[j].Trim();

                        GekkoTime tt = GekkoTime.FromStringToGekkoTime(s, false, false);  //no error
                        bool good = true;
                        //This would be easier if time was known to be always last...
                        if (tt.IsNull()) good = false;
                        if (s.Length < 4) good = false; //avoid the 18 in x[18, 2020q2] is a hit.
                        if (tt.super < 1900 || tt.super > 4000) good = false; //sensible?
                        if (!(tt.freq == EFreq.A || tt.freq == EFreq.Q || tt.freq == EFreq.M)) good = false;

                        if (good)
                        {
                            //Time is in this index
                            if (!helper.time.IsNull()) new Error("Variable '" + start + "' seems to have > 1 time indexes: '" + varname + "'");
                            helper.time = tt;
                        }

                        if (helper.time.IsNull())
                        {
                            fullName.Add(s);
                        }
                        else
                        {
                            //not part of indexes
                        }
                    }

                    if (errorIfTimeNotFound && helper.time.IsNull()) new Error("Unexpected");
                    if (fullName.Count == 0) helper.resultingFullName = start;  //avoid an empty "x[]" name.
                    else helper.resultingFullName = start + "[" + Stringlist.GetListWithCommas(fullName) + "]";
                }
                else
                {
                    //without index
                    if (errorIfTimeNotFound) new Error("Unexpected");
                    start = varname;
                    helper.resultingFullName = varname;
                }
                helper.indexes = fullName;
                helper.name = start;
            }

            return helper;
        }

        private static bool ExtractTimeDimensionHelper2(EExtractTimeDimension settings, string input, ExtractTimeDimensionHelper helper)
        {
            //input like "x[a,b,2022]"
            bool simple = false;
            int end = input.Length - 1;
            if (input[end] != ']') return simple;
            if (input.Length < 7) return simple;  //if input has length 7, it is like '123456', where x[6] = x[end] = '6'. Here, x[end-6] = x[0] = '1' is legal.            
            string s = G.Substring(input, end - 4, end - 1);
            int i9 = G.IntParse(s);
            if (i9 == -12345 || char.IsDigit(input[end - 5])) return simple;
            helper.time = new GekkoTime(EFreq.A, i9, 1);
            if (input[end - 5] == '[')
            {
                //input like "x[2022]"
                //.resultingFullName --> "x"
                //.name --> "x"
                simple = true;
                helper.resultingFullName = G.Substring(input, 0, end - 6);
                helper.name = helper.resultingFullName;
                if (settings == EExtractTimeDimension.Full) helper.indexes = new List<string>();
            }
            else
            {
                //input like "x[a,b,2022]"
                //.resultingFullName --> "x[a,b]"
                //.name --> "x"
                simple = true;
                helper.resultingFullName = G.Substring(input, 0, end - 6) + "]";
                int idx = input.IndexOf('[');
                helper.name = G.Substring(input, 0, idx - 1);
                if (settings == EExtractTimeDimension.Full)
                {                    
                    string s2 = G.Substring(input, idx + 1, end - 1);                    
                    helper.indexes = s2.Split(',').ToList();
                }
            }

            return simple;
        }

        /// <summary>
        /// Read a scalar model. For each model line, it calls HandleEqLine().
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static Model ReadGamsScalarModelEquations(GAMSScalarModelSettings settings, Model model)
        {
            //for c:\Thomas\Gekko\regres\MAKRO\test3\klon\Model\gams.gms and
            //    c:\Thomas\Gekko\regres\MAKRO\test3\klon\Model\dict.txt
            //Import dictionary: 8.53 sec
            //GAMS equations read: 48.44 sec-- > count 1063359 unique 12750    //// best: 40 (best release: 19)
            //Starting values read: 3.64 sec
            //Compile finished: 14.54 sec
            //Data preparation finished: 0.4667 sec
            //======================================================================
            //===> Setting up everything took:  (1:15 min), all included  //// best: 1:00 (best release: 36)
            //======================================================================
            //Loading Func<>'s took: 1.04 sec
            //1063359 evaluations x 100 took 25.60 sec
            //1063359 evaluations x 100 took 5.90 sec
            //1063359 evaluations x 100 took 5.75 sec
            //1063359 evaluations x 100 took 5.49 sec            
            //So after warmup about 5 sec for 1e8 evals in debug mode
            //  --> Sometimes seen it around 4.1 in debug mode (best release mode: around 3.40).

            //Note: cf. these interfaces from Python or Julia to GAMS: https://www.gams.com/blog/2020/06/new-and-improved-gams-links-for-pyomo-and-jump/

            EqLineHelper helper = new EqLineHelper();
            helper.dict_FromEqNumberToEqName = null;
            helper.dict_FromVarNumberToVarName = null;

            DateTime dt0 = DateTime.Now;  //everything
            DateTime dt1 = DateTime.Now;  //sub tasks

            string[] split = new string[] { ".fx", ".l", "=", ";" };
            string[] split2 = new string[] { " " };
                        
            int status2 = 0;
            int substatus2 = 0;
            int eqCounts2 = -12345;
            int varCounts2 = -12345;
            
            //read dictionary                        
            if (settings.scalarMemoryModelProducedByGekko)
            {
                StreamReader sr = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Stringlist.ExtractTextFromLines(settings.dictionary).ToString())));
                ReadScalarModelEquationsDictionaryLines(helper, split2, ref status2, ref substatus2, ref eqCounts2, ref varCounts2, sr);
            }
            else
            {
                using (FileStream fs = Program.WaitForFileStream(settings.ffh_unrolledNames.realPathAndFileName, settings.ffh_unrolledNames.prettyPathAndFileName, Program.GekkoFileReadOrWrite.Read))
                using (TextReader sr = new StreamReader(fs))
                {
                    ReadScalarModelEquationsDictionaryLines(helper, split2, ref status2, ref substatus2, ref eqCounts2, ref varCounts2, sr);
                }
            }            

            helper.dict_FromANumberToVarName = new string[helper.dict_FromVarNameToANumber.Count()];
            foreach (KeyValuePair<string, int> kvp in helper.dict_FromVarNameToANumber.GetDictionaryForIteration())
            {
                helper.dict_FromANumberToVarName[kvp.Value] = kvp.Key;
            }

            helper.dict_FromEqChunkNumberToEqName = new string[helper.dict_FromEqNameToEqChunkNumber.Count()];
            foreach (KeyValuePair<string, int> kvp in helper.dict_FromEqNameToEqChunkNumber.GetDictionaryForIteration())
            {
                helper.dict_FromEqChunkNumberToEqName[kvp.Value] = kvp.Key;
            }

            if (Globals.runningOnTTComputer) new Writeln("TTH: Import dictionary finished: " + G.Seconds(dt1));
            dt1 = DateTime.Now;

            TokenList tokensLast = null;

            List<string> eqs = new List<string>();      //1
            List<string> values = new List<string>();   //2
            List<string> end = new List<string>();      //3
            int status = 0;
            int substatus = 0;
            int eqCounts = -12345;
            int varCounts = -12345;
            int semis = 0;

            helper.tBasis = helper.t1;  //could perhaps lag this later on... ?
            helper.t3 = helper.t2;  //could perhaps lead this later on... ?
            int periods = GekkoTime.Observations(helper.t1, helper.t2);
            helper.a = new double[periods][];
            for (int i = 0; i < helper.a.GetLength(0); i++)
            {
                //for a static scalar model, this will not be of much use, just showing largest lead minus largest lag in first dimension.
                helper.a[i] = new double[helper.dict_FromVarNameToANumber.Count()]; //beware: 0-based
                G.SetNaN(helper.a[i]);
            }

            List<string> csCodeLines = new List<string>();

            List<string> equationDefs = new List<string>();
            StringBuilder eqLine = null;

            //read unrolled equations line by line            
            if (settings.scalarMemoryModelProducedByGekko)
            {
                StreamReader sr = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Stringlist.ExtractTextFromLines(settings.equations).ToString())));
                ReadGamsScalarModelEquationsLines(helper, split2, ref tokensLast, values, end, ref status, ref substatus, ref eqCounts, ref varCounts, ref semis, csCodeLines, ref eqLine, sr);
            }
            else
            {
                using (FileStream fs = Program.WaitForFileStream(settings.ffh_unrolledModel.realPathAndFileName, settings.ffh_unrolledModel.prettyPathAndFileName, Program.GekkoFileReadOrWrite.Read))
                using (StreamReader sr = new StreamReader(fs))
                {
                    ReadGamsScalarModelEquationsLines(helper, split2, ref tokensLast, values, end, ref status, ref substatus, ref eqCounts, ref varCounts, ref semis, csCodeLines, ref eqLine, sr);
                }
            }
            
            if (Globals.runningOnTTComputer) new Writeln("TTH: GAMS equations read: " + G.Seconds(dt1) + "   -->   " + "count " + helper.count + " unique " + helper.unique);
            dt1 = DateTime.Now;

            //new Writeln("Count " + helper.count + " hits " + helper.known + " unique " + helper.unique + " semis " + semis);
            if (helper.count != helper.known + helper.unique) new Error("Not summing up");
            if (helper.count != semis) new Error("Not summing up");

            if (false)
            {
                File.WriteAllText(@"c:\Thomas\Gekko\regres\MAKRO\test3\klon\Model\deleteme.gms", Stringlist.ExtractTextFromLines(csCodeLines).ToString());
            }

            foreach (string line in values)
            {
                if (line.Trim() == "" || line.StartsWith("*")) continue;
                string[] ss = line.Split(split, StringSplitOptions.None);
                int id = int.Parse(ss[0].Substring(1)) - 1;  //0-based
                string inputName = helper.dict_FromVarNumberToVarName[id];                
                ExtractTimeDimensionHelper helper2 = ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, inputName, true);
                int aNumber = helper.dict_FromVarNameToANumber.Get(helper2.resultingFullName);
                int i1 = helper2.time.Subtract(helper.tBasis);
                int i2 = aNumber;
                double d;
                if (ss[1].Trim() == "")
                {
                    //probably always so
                    d = double.Parse(ss[2]);
                }
                else
                {
                    d = double.Parse(ss[1]);
                }
                helper.a[i1][i2] = d;
            }
            if (Globals.runningOnTTComputer) new Writeln("TTH: Endogenous values read: " + G.Seconds(dt1));

            //new Writeln("eqCounts = " + eqCounts + ", varCounts = " + varCounts + ", eqCounts2 = " + eqCounts2 + ", varCounts2 = " + varCounts2);
            //if (eqCounts != varCounts) new Writeln("ERROR: counts do not match.");
            //if (eqCounts2 != varCounts2) new Writeln("ERROR: counts do not match.");
            //if (eqCounts != eqCounts2) new Writeln("ERROR: counts do not match.");

            double[] r = G.CreateNaN(eqCounts2);
            Func<int, double[], double[][], double[], int[][], int[][], int, double>[] functions = new Func<int, double[], double[][], double[], int[][], int[][], int, double>[helper.unique];
            double[][] a = helper.a;
            int[][] bb = helper.b.Select(x => x.ToArray()).ToArray();
            double[] cc = helper.c.ToArray();
            int[][] dd = helper.d.Select(x => x.ToArray()).ToArray();
            int[] ee = helper.eqPointers.ToArray();

            Compile5(csCodeLines, functions);

            dt1 = DateTime.Now;

            if (Globals.runningOnTTComputer) new Writeln("TTH: Data preparation finished: " + G.Seconds(dt1));

            dt1 = DateTime.Now;

            //The method below handles ANSI, but labels are not fetched here yet.   

            ModelGams modelGams = null;
            if (!settings.scalarMemoryModelProducedByGekko)
            {
                string text = Program.GetTextFromFileWithWait(settings.ffh_rawModel.realPathAndFileName);
                List<string> gamsFoldedModel = Stringlist.ExtractLinesFromText(text);
                IVariable nestedListOfDependents_opt_dep = null;
                Tuple<GekkoDictionary<string, string>, StringBuilder> tup = GamsModel.GetDependentsGams(nestedListOfDependents_opt_dep);
                GekkoDictionary<string, string> dependents = tup.Item1;
                modelGams = GamsModel.ReadGamsModelHelper(Stringlist.ExtractTextFromLines(gamsFoldedModel).ToString(), null, dependents, false, true, model);
                if (Globals.runningOnTTComputer) new Writeln("TTH: Get folded model: " + G.Seconds(dt1));
            }

            dt1 = DateTime.Now;

            ModelGamsScalar modelGamsScalar = new ModelGamsScalar(model);
                        
            // -------------- these can evaluate an equation --------
            modelGamsScalar.functions = functions;
            modelGamsScalar.a = a;

            modelGamsScalar.a_ref = new double[modelGamsScalar.a.Length][];
            for (int i = 0; i < modelGamsScalar.a.Length; i++)
            {
                modelGamsScalar.a_ref[i] = new double[modelGamsScalar.a[i].Length];
                G.SetNaN(modelGamsScalar.a_ref[i]);
            }

            modelGamsScalar.r = r;
            // ------------------------------------------------------
            modelGamsScalar.bb = bb;
            modelGamsScalar.cc = cc;
            modelGamsScalar.dd = dd;
            modelGamsScalar.ee = ee;
            // -------------- helpers, counts -----------------------
            modelGamsScalar.eqCounts = eqCounts;
            modelGamsScalar.count = helper.count;
            modelGamsScalar.known = helper.known;
            modelGamsScalar.unique = helper.unique;
            //
            // Note that GAMS equation periods are not very useful.
            // In principle, e1[2020] .. may designate an equation with
            // variables from 2025, so there are no guarantees.
            modelGamsScalar.tBasis = helper.tBasis;
            modelGamsScalar.absoluteT1 = helper.t1;
            modelGamsScalar.absoluteT2 = helper.t2;
            // -------------- helpers dictionaries ---------
            modelGamsScalar.dict_FromANumberToVarName = helper.dict_FromANumberToVarName;
            modelGamsScalar.dict_FromVarNameToANumber = helper.dict_FromVarNameToANumber;  //dict
            modelGamsScalar.dict_FromEqNumberToEqName = helper.dict_FromEqNumberToEqName;
            modelGamsScalar.dict_FromEqNameToEqNumber = helper.dict_FromEqNameToEqNumber;  //dict
            modelGamsScalar.dict_FromVarNumberToVarName = helper.dict_FromVarNumberToVarName;
            modelGamsScalar.dict_FromVarNameToVarNumber = helper.dict_FromVarNameToVarNumber;  //dict ... used at all???
            modelGamsScalar.dict_FromEqChunkNumberToEqName = helper.dict_FromEqChunkNumberToEqName;
            modelGamsScalar.dict_FromEqNameToEqChunkNumber = helper.dict_FromEqNameToEqChunkNumber;  //dict ... used at all???
            modelGamsScalar.dict_FromEqNumberToEqChunkNumber = helper.dict_FromEqNumberToEqChunkNumber;
            // -------------- raw codelines ---------
            modelGamsScalar.csCodeLines = csCodeLines;
            
            CalculatePrecedentsAndDependents(modelGamsScalar, modelGamsScalar.CountEqs(1));

            if (false && Globals.runningOnTTComputer)
            {
                foreach (KeyValuePair<PeriodAndVariable, List<int>> kvp in modelGamsScalar.dependents)
                {
                    string varName = modelGamsScalar.GetVarNameA(kvp.Key.variable);
                    GekkoTime t = modelGamsScalar.FromTimeIntegerToGekkoTime(kvp.Key.date);
                    string s7 = varName + "[" + t.ToString() + "] = ";
                    foreach (int i in kvp.Value)
                    {
                        string eqName = modelGamsScalar.dict_FromEqNumberToEqName[i];
                        s7 += eqName + ", ";
                    }
                    new Writeln(s7);
                }
            }

            if (Globals.runningOnTTComputer) new Writeln("TTH: Precedents/dependents: " + G.Seconds(dt1));

            if (Globals.runningOnTTComputer)
            {
                using (var txt = new Writeln())
                {
                    txt.MainAdd("======================================================================");
                    txt.MainNewLineTight();
                    txt.MainAdd("===> TTH: Setting up everything took: " + G.Seconds(dt0) + ", all included");
                    txt.MainNewLineTight();
                    txt.MainAdd("======================================================================");
                }
            }            

            return model;
        }

        private static void ReadGamsScalarModelEquationsLines(EqLineHelper helper, string[] split2, ref TokenList tokensLast, List<string> values, List<string> end, ref int status, ref int substatus, ref int eqCounts, ref int varCounts, ref int semis, List<string> csCodeLines, ref StringBuilder eqLine, StreamReader sr)
        {
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                if (status == 0)
                {
                    if (line.StartsWith("e1.."))
                    {
                        eqLine = new StringBuilder(line);
                        if (line.EndsWith(";"))
                        {
                            semis++;
                            int hits2 = helper.known;
                            tokensLast = HandleEqLine(eqLine, tokensLast, helper);
                            if (helper.known == hits2) RemoveDoubleDots(helper, csCodeLines);
                            eqLine = new StringBuilder();
                        }
                        status = 1;
                    }
                    else
                    {
                        //start.Add(line);
                        if (line.StartsWith("* Equation counts"))
                        {
                            substatus = 1;
                        }
                        else if (line.StartsWith("* Variable counts"))
                        {
                            substatus = 2;
                        }
                        if (substatus == 1)
                        {
                            string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string sx in ss)
                            {
                                if (G.IsInteger(sx))
                                {
                                    eqCounts = int.Parse(sx);
                                    substatus = 0;
                                    break;
                                }
                            }
                        }
                        else if (substatus == 2)
                        {
                            string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string sx in ss)
                            {
                                if (G.IsInteger(sx))
                                {
                                    varCounts = int.Parse(sx);
                                    substatus = 0;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (status == 1)
                {
                    if (line.StartsWith("* set non-default bounds", StringComparison.OrdinalIgnoreCase) || line.StartsWith("* set non-default levels", StringComparison.OrdinalIgnoreCase))
                    {
                        values.Add(line);
                        status = 2;
                    }
                    else
                    {
                        if (line.EndsWith(";"))
                        {
                            semis++;
                            eqLine.Append(line);
                            int hits2 = helper.known;
                            tokensLast = HandleEqLine(eqLine, tokensLast, helper);
                            if (helper.known == hits2) RemoveDoubleDots(helper, csCodeLines);
                            eqLine = new StringBuilder();
                        }
                        else
                        {
                            eqLine.Append(line);
                        }
                    }
                }
                else if (status == 2)
                {
                    if (line.ToLower().StartsWith("model ")) //model m / all /;
                    {
                        end.Add(line);
                        status = 3;
                    }
                    else
                    {
                        values.Add(line);
                    }
                }
                else
                {
                    end.Add(line);
                }
            }
        }

        private static void ReadScalarModelEquationsDictionaryLines(EqLineHelper helper, string[] split2, ref int status2, ref int substatus2, ref int eqCounts2, ref int varCounts2, TextReader sr)
        {
            bool b = false;
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Trim() == "") continue;
                if (line.ToLower().Contains("equation counts"))
                {
                    substatus2 = 1;
                }
                else if (line.ToLower().Contains("variable counts"))
                {
                    substatus2 = 2;
                }

                if (substatus2 == 1)
                {
                    string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string sx in ss)
                    {
                        if (G.IsInteger(sx))
                        {
                            eqCounts2 = int.Parse(sx);
                            substatus2 = 0;
                            helper.dict_FromEqNumberToEqName = new string[eqCounts2];
                            helper.dict_FromEqNumberToEqChunkNumber = new int[eqCounts2];
                            break;
                        }
                    }
                }
                else if (substatus2 == 2)
                {
                    string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string sx in ss)
                    {
                        if (G.IsInteger(sx))
                        {
                            varCounts2 = int.Parse(sx);
                            substatus2 = 0;
                            helper.dict_FromVarNumberToVarName = new string[varCounts2];
                            break;
                        }
                    }
                }

                if (line.ToLower().StartsWith("equations "))
                {
                    status2 = 1;
                    continue;
                }
                else if (line.ToLower().StartsWith("variables "))
                {
                    status2 = 2;
                    continue;
                }
                if (status2 == 1)
                {
                    string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                    int n = int.Parse(ss[0].Substring(1)) - 1; //so it is 0-based
                    string ss2 = ss[1].Replace("(", "[").Replace(")", "]");
                    string eqName = ss2;
                    int idx = ss2.IndexOf("[");
                    if (idx >= 0) eqName = ss2.Substring(0, idx);
                    helper.dict_FromEqNumberToEqName[n] = ss2;
                    helper.dict_FromEqNameToEqNumber.Add(ss2, n, b);  //filling this out could be postponed until decomp if loading is slow                        
                    helper.dict_FromEqNameToEqChunkNumber.AddIfNotAlreadyThere(eqName, helper.dict_FromEqNameToEqChunkNumber.Count(), b);
                    helper.dict_FromEqNumberToEqChunkNumber[n] = helper.dict_FromEqNameToEqChunkNumber.Count() - 1;
                }
                else if (status2 == 2)
                {
                    string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                    int n = int.Parse(ss[0].Substring(1)) - 1; //so it is 0-based
                    string ss2 = ss[1].Replace("(", "[").Replace(")", "]");
                    helper.dict_FromVarNumberToVarName[n] = ss2;
                    helper.dict_FromVarNameToVarNumber.Add(ss2, n, b);                    
                    ExtractTimeDimensionHelper helper2 = ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, ss2, true);                 
                    if (helper.t1.IsNull() || helper2.time.StrictlySmallerThan(helper.t1)) helper.t1 = helper2.time;
                    if (helper.t2.IsNull() || helper2.time.StrictlyLargerThan(helper.t2)) helper.t2 = helper2.time;
                    helper.dict_FromVarNameToANumber.AddIfNotAlreadyThere(helper2.resultingFullName, helper.dict_FromVarNameToANumber.Count(), b);
                }
            }
        }

        private static void CalculatePrecedentsAndDependents(ModelGamsScalar modelGamsScalar, int bigN)
        {
            modelGamsScalar.precedents = new List<ModelScalarEquation>();
            modelGamsScalar.dependents = new GekkoDictionary<PeriodAndVariable, List<int>>();

            for (int eqNumber = 0; eqNumber < bigN; eqNumber++)
            {
                ModelScalarEquation l = new ModelScalarEquation();
                modelGamsScalar.precedents.Add(l);
                //foreach precedent variable
                for (int i = 0; i < modelGamsScalar.bb[eqNumber].Length; i += 2)
                {
                    PeriodAndVariable dp = new PeriodAndVariable(modelGamsScalar.bb[eqNumber][i], modelGamsScalar.bb[eqNumber][i + 1]);
                    if (!l.vars.Contains(dp)) l.vars.Add(dp);  //avoid dublets
                }
            }

            //mapping from a varname to the equations it is part of                
            for (int eqNumber = 0; eqNumber < bigN; eqNumber++)
            {
                //foreach precedent variable
                foreach (PeriodAndVariable dp in modelGamsScalar.precedents[eqNumber].vars)
                {
                    List<int> eqsHere = null;
                    modelGamsScalar.dependents.TryGetValue(dp, out eqsHere);
                    if (eqsHere == null)
                    {
                        modelGamsScalar.dependents.Add(dp, new List<int>() { eqNumber });
                    }
                    else
                    {
                        if (eqsHere.Contains(eqNumber))
                        {
                            new Error("Strange!");
                        }
                        eqsHere.Add(eqNumber);
                    }
                }
            }
        }

        /// <summary>
        /// Real parsing of GAMS
        /// </summary>
        public static void GAMSParser()
        {
            DateTime dt0 = DateTime.Now;

            ANTLRStringStream input = new ANTLRStringStream(Program.GetTextFromFileWithWait(@"c:\Thomas\Gekko\regres\MAKRO\test3\klon\Model\cut.gms"));  //a newline for ease of use of ANTLR

            List<string> errors = null;
            CommonTree t = null;

            // Create a lexer attached to that input
            GAMSLexer lexer = new GAMSLexer(input);
            // Create a stream of tokens pulled from the lexer
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            // Create a parser attached to the token stream
            GAMSParser parser = new GAMSParser(tokens);
            // Invoke the program rule in get return value
            GAMSParser.gams_return gams = null;
            DateTime t0 = DateTime.Now;

            bool print = false;
            ASTNodeGAMS root = new ASTNodeGAMS(null);

            try
            {
                DateTime tt0 = DateTime.Now;
                new Writeln("START CUT PARSE ANTLR");
                gams = parser.gams();
                new Writeln("END BUT PARSE ANTLR -- " + G.Seconds(tt0));
                errors = parser.GetErrors();
                t = (CommonTree)gams.Tree;
                Compile2(t, root, 0, tokens, print);
                new Writeln("END ASTNODES -- " + G.Seconds(tt0));
                if (errors.Count > 0)
                {
                    new Warning("GAMS parse error");
                }
            }
            catch (Exception e)
            {
                new Warning("GAMS other error");
            }
        }

        /// <summary>
        /// GAMS GMO interface.
        /// </summary>
        public static void GamsGMO()
        {
            string msg2 = null;
            string gams = null;
            if (1 == 1)
            {
                gams = @"c:\Program Files\GAMS\38\";
            }
            else if (1 == 0)
            {
                gams = @"c:\Program Files\GAMS\34.2\";
            }
            else if (1 == 0)
            {
                gams = @"c:\Program Files (x86)\GAMS\29.1\";
            }
            else throw new GekkoException();

            Directory.SetCurrentDirectory(gams);  //necessary for some odd reason
            string control = @"c:\Thomas\Gekko\GekkoCS\Diverse\GAMS\225a\gamscntr.dat";
            gevmcs gev = new gevmcs(gams, ref msg2);
            gev.gevInitEnvironmentLegacy(control);
            gmomcs gmo = new gmomcs(gams, ref msg2);
            gmo.gmoRegisterEnvironment(gev.GetgevPtr(), ref msg2);
            gmo.gmoLoadDataLegacy(ref msg2);

            string varname0 = gmo.gmoGetVarNameOne(0);

            int ncols = gmo.gmoN();
            double[] x = new double[ncols];
            gmo.gmoGetVarL(ref x);
            for (int i = 0; i < ncols; i++)
            {
                string varname = gmo.gmoGetVarNameOne(i);
            }

            int nrows = gmo.gmoM();
            int numerr = -12345;
            double lhs = double.NaN;
            for (int i = 0; i < nrows; i++)
            {
                gmo.gmoEvalFunc(i, x, ref lhs, ref numerr);
                double rhs = gmo.gmoGetRhsOne(i);
                double residual = lhs - rhs;
                string eqname = gmo.gmoGetEquNameOne(i);
            }
        }

        private static void RemoveDoubleDots(EqLineHelper helper, List<string> output)
        {
            string s = helper.sb.ToString();
            s = "r[i] = " + s.Replace("..", "").Replace("=E=", "-(").Replace(";", ");");            
            output.Add(s);
        }

        private static TokenList HandleEqLine(StringBuilder eqLine, TokenList tokensLast, EqLineHelper helper)
        {
            //Remember: the human readable code is derived from this, so beware if changes are made,
            //cf. #af931klljaf89efw.            
            helper.Clear();
            int more = 2;
            TokenList tokens = StringTokenizer.GetTokensWithLeftBlanks(eqLine.ToString(), more);  //1 empty "" token
            //probe, checking for **
            for (int i = 0; i < tokens.Count() - more; i++)
            {
                if (tokens[i].s == "*" && tokens[i + 1].s == "*" && tokens[i + 1].leftblanks == 0)
                {

                    //Left
                    int lefttype = int.MaxValue;  //-100 for word, positive for parenthesis
                    if (i > 0 && (tokens[i - 1].type == ETokenType.Word || tokens[i - 1].type == ETokenType.Number))
                    {
                        lefttype = -100;
                    }
                    else if (i > 0 && tokens[i - 1].s == ")")
                    {
                        int counter = 1;
                        for (int i2 = i - 2; i2 > 0; i2--)
                        {
                            if (tokens[i2].s == ")") counter++;
                            else if (tokens[i2].s == "(") counter--;
                            if (counter == 0)
                            {
                                lefttype = i2;
                                break;
                            }
                        }
                    }

                    //Right
                    int righttype = int.MaxValue;  //-100 for word, positive for parenthesis
                    if (i < tokens.Count() && (tokens[i + 2].type == ETokenType.Word || tokens[i + 2].type == ETokenType.Number))
                    {
                        righttype = -100;
                    }
                    else if (i < tokens.Count() && tokens[i + 2].s == "(")
                    {
                        int counter = 1;
                        for (int i2 = i + 3; i2 < tokens.Count(); i2++)
                        {
                            if (tokens[i2].s == "(") counter++;
                            else if (tokens[i2].s == ")") counter--;
                            if (counter == 0)
                            {
                                righttype = i2;
                                break;
                            }
                        }
                    }

                    if (lefttype == int.MaxValue || righttype == int.MaxValue)
                        new Error("Problem resolving '**' power");

                    helper.remove.Add(i, "");
                    helper.remove.Add(i + 1, "");
                    helper.addBefore.Add(i, ",");
                    if (lefttype == -100) helper.addBefore.Add(i - 1, "M.Power(");
                    else if (lefttype > 0) helper.addBefore.Add(lefttype, "M.Power(");
                    if (righttype == -100) helper.addBefore.Add(i + 3, ")");
                    else if (righttype > 0) helper.addBefore.Add(righttype + 1, ")");
                }
            }            

            bool knownPattern = true;
            for (int i = 0; i < tokens.Count() - more; i++)
            {
                TokenHelper th2 = null;
                TokenHelper th2Next = null;
                if (tokensLast == null || i >= tokensLast.Count() - more)
                {
                    knownPattern = false;
                }
                else
                {
                    th2 = tokensLast[i];
                    th2Next = tokensLast[i + 1];
                }
                TokenHelper th1 = tokens[i];
                TokenHelper th1Next = tokens[i + 1];
                if (th1.type == ETokenType.Number)
                {
                    if (th2 != null && th2.type == ETokenType.Number)
                    {
                        //do nothing
                    }
                    else
                    {
                        knownPattern = false;
                    }
                    int i1 = helper.dict_Constants.Count;
                    if (helper.dict_Constants.ContainsKey(th1.s))
                    {
                        i1 = helper.dict_Constants[th1.s];
                    }
                    else
                    {
                        helper.dict_Constants.Add(th1.s, i1);
                        helper.exoValues.Add(double.Parse(th1.ToString()));
                    }                    
                    HandleEqLineAppend(helper, i, "c[d[" + helper.exo.Count + "]]");
                    helper.exo.Add(i1);
                }
                else if (IsEVariable(th1, th1Next))
                {
                    if (th2 != null && IsEVariable(th2, th2Next))
                    {      
                        //do nothing
                    }
                    else
                    {
                        knownPattern = false;
                    }
                    int number = int.Parse(th1.s.Substring(1)) - 1;  //0-based
                    string eqname = helper.dict_FromEqNumberToEqName[number];
                    string helper2 = "";
                    HandleEqLineAppend(helper, i, helper2);
                }
                else if (IsXVariable(th1, th1Next))
                {
                    if (th2 != null && IsXVariable(th2, th2Next))
                    {    
                        //do nothing
                    }
                    else
                    {
                        knownPattern = false;
                    }
                    int number = int.Parse(th1.s.Substring(1)) - 1;  //0-based
                    string varname = helper.dict_FromVarNumberToVarName[number];                    
                    ExtractTimeDimensionHelper helper2 = ExtractTimeDimension(true, EExtractTimeDimension.NoIndexListOfStrings, varname, true);
                    int i1 = helper2.time.Subtract(helper.tBasis);
                    int i2 = helper.dict_FromVarNameToANumber.Get(helper2.resultingFullName);
                                        
                    int ii1 = helper.endo.Count;
                    int ii2 = helper.endo.Count + 1;

                    bool seenBefore = false;
                    
                    HandleEqLineAppend(helper, i, "a[b[" + ii1 + "]+t][b[" + ii2 + "]]");
                                        
                    if (!seenBefore)
                    {
                        //avoid dublets in an equation (for instance y[2020] = x[2020] + x[2020]/z[2020])
                        helper.endo.Add(i1);
                        helper.endo.Add(i2);
                    }
                }
                else
                {
                    if (th2 != null && th1.s != th2.s) knownPattern = false;
                    string s = th1.s;
                    if (th1.type == ETokenType.Word && th1Next.s == "(")
                    {
                        //can be a function:                    
                        s = RenameFunctions(th1, true);
                    }
                    HandleEqLineAppend(helper, i, s);
                }
            }  //end of tokens loop
            
            if (knownPattern)
            {
                helper.known++;
            }
            else
            {
                //unseen equation type
                helper.unique++;
            }            
            helper.count++;
            helper.eqPointers.Add(helper.unique - 1);  //unique is 1 for the first equation. For the second, it may be 1 or 2. So 0 points to 0, 1 points to 0 or 1.
            helper.b.Add(helper.endo);  //also works as precedents
            
            helper.c.AddRange(helper.exoValues);            
            helper.d.Add(helper.exo);

            return tokens;  //to compare with next
        }

        public static string RenameFunctions(TokenHelper th1, bool b)
        {
            string s = null;
            if (b)
            {
                if (G.Equal(th1.s, "log")) s = "M.Log";
                else if (G.Equal(th1.s, "exp")) s = "M.Exp";
                else if (G.Equal(th1.s, "abs")) s = "M.Abs";
                else if (G.Equal(th1.s, "max")) s = "M.Max";
                else if (G.Equal(th1.s, "min")) s = "M.Min";
                else if (G.Equal(th1.s, "power")) s = "M.Power";
                else if (G.Equal(th1.s, "pow")) s = "M.Power";  //pow() happens when a .frm model produces GAMS equations
                else if (G.Equal(th1.s, "sqr")) s = "M.Sqr";
                else if (G.Equal(th1.s, "sqrt")) s = "M.Sqrt";
                else if (G.Equal(th1.s, "tanh")) s = "M.Tanh";
            }
            else
            {
                th1.s = th1.s.ToLower();  //the "M." is removed elsewhere
            }
            return s;
        }

        private static void HandleEqLineAppend(EqLineHelper helper, int i, string s)
        {
            //Remember: the human readable code is derived from this, so beware if changes are made,
            //cf. #af931klljaf89efw.
            if (helper.addBefore.ContainsKey(i))
            {
                helper.sb.Append(helper.addBefore[i]);
            }
            if (helper.remove.ContainsKey(i))
            {
                //do nothing
            }
            else
            {
                helper.sb.Append(s);
            }
        }

        private static bool IsXVariable(TokenHelper th, TokenHelper thNext)
        {
            //must be word starting with "x", these may differ. There cannot be a parenthesis following (then it could be a function call).
            return th.type == ETokenType.Word && th.s.StartsWith("x") && !(thNext.s == "(" || thNext.s == "[" || thNext.s == "{");
        }

        private static bool IsEVariable(TokenHelper th, TokenHelper thNext)
        {
            //must be word starting with "e", these may differ. There cannot be a parenthesis following (then it could be a function call).
            return th.type == ETokenType.Word && th.s.StartsWith("e") && !(thNext.s == "(" || thNext.s == "[" || thNext.s == "{");
        }

        private static void TraverseNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                // Do something with the node.
                TraverseNodes(node.ChildNodes);
            }
        }        

        /// <summary>
        /// Read a raw GAMS model from a .gms/.gmy model. Deals with possible cached version etc. See also ReadGamsScalarModel().
        /// </summary>
        public static Model ReadGamsRawModel(string textInputRaw, string fileName, O.Model o)
        {
            Model model = new Model();
            model.modelCommon.SetModelSourceType(EModelType.GAMSRaw);
            ModelGams modelGams = new ModelGams(model);

            Tuple<GekkoDictionary<string, string>, StringBuilder> tup = GetDependentsGams(o.opt_dep);
            GekkoDictionary<string, string> dependents = tup.Item1;

            //
            // Should #dependents list be reflected in hash ?????
            //

            //string dependentsHash = tup.Item2.ToString();
            //string modelHash = HandleModelFilesGams(textInputRaw + dependentsHash);

            //string mdlFileNameAndPath = Globals.localTempFilesLocation + "\\" + Globals.gekkoVersion + "_" + "gams" + "_" + modelHash + Globals.cacheExtensionModel;

            //if (Program.options.model_cache == true)
            //{
            //    if (File.Exists(mdlFileNameAndPath))
            //    {
            //        try
            //        {
            //            DateTime dt1 = DateTime.Now;                        
            //            modelGams = Program.ProtobufRead<ModelGams>(mdlFileNameAndPath);
            //            model.loadedFromCacheFile = true;
            //            G.WritelnGray("Loaded known model from cache in: " + G.SecondsFormat((DateTime.Now - dt1).TotalMilliseconds));
            //        }
            //        catch (Exception e)
            //        {
            //            if (G.IsUnitTesting())
            //            {
            //                throw;
            //            }
            //            else
            //            {
            //                //do nothing, we then have to parse the file
            //                model.loadedFromCacheFile = false;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    model.loadedFromCacheFile = false;
            //}

            //if (model.loadedFromCacheFile)
            //{
            //    //do nothing, also no writing of .mdl file of course
            //}
            //else
            {
                model.modelGams = ReadGamsModelHelper(textInputRaw, fileName, dependents, G.Equal(o.opt_dump, "yes"), false, model);
                if (Globals.runningOnTTComputer) Sniff2(model);

                DateTime t1 = DateTime.Now;

                //try //not the end of world if it fails (should never be done if model is read from zipped protobuffer (would be waste of time))
                //{
                //    DateTime dt1 = DateTime.Now;
                                        
                //    // ----- SERIALIZE
                //    string protobufFileName = Globals.gekkoVersion + "_" + "gams" + "_" + modelHash + Globals.cacheExtensionModel;
                //    string pathAndFilename = Globals.localTempFilesLocation + "\\" + protobufFileName;
                //    Program.ProtobufWrite(model.modelGams, pathAndFilename);
                //    G.WritelnGray("Created model cache file in " + G.SecondsFormat((DateTime.Now - dt1).TotalMilliseconds));
                //}
                //catch (Exception e)
                //{
                //    //do nothing, not the end of the world if it fails
                //}
            }
            return model;
        }

        /// <summary>
        /// Read/load a GAMS scalar model from a suitable zip file. See also ReadGamsRawModel().
        /// </summary>
        public static Model ReadGAMSScalarModel(O.Model o, List<string> folders, string fileName)
        {
            //TODO TODO TODO
            //TODO TODO TODO
            //TODO TODO TODO in a session, maybe look at file sizes and dates/times for the zip, like done for libraries
            //TODO TODO TODO
            //TODO TODO TODO

            Model model = new Model();
            model.modelCommon.SetModelSourceType(EModelType.GAMSScalar);
                        
            DateTime t = DateTime.Now;
            
            string timeLoadCache = null;
            string timeCompile = null;

            GAMSScalarModelSettings input = new GAMSScalarModelSettings();
            input.zipFilePathAndName = fileName;

            DateTime t2 = DateTime.Now;

            model = ReadGAMSScalarModel2(o, folders, model, input);

            //string modelHash = Program.GetMD5Hash(null, input.zipFilePathAndName);            

            //string mdlFileNameAndPath = Globals.localTempFilesLocation + "\\" + Globals.gekkoVersion + "_" + "gams" + "_" + modelHash + Globals.cacheExtensionModel;

            //if (Program.options.model_cache == true)
            //{
            //    try
            //    {
            //        //TODO 
            //        //TODO 
            //        //TODO do something about ms here
            //        //TODO 
            //        //TODO 
            //        double hashMs = 0d;
            //        DateTime t0 = DateTime.Now;
            //        Model modelTemp = Program.ReadParallelModel(input.zipFilePathAndName, modelHash);
            //        timeLoadCache = "cache: " + G.Seconds(t0);
                    
            //        if (modelTemp == null)
            //        {
            //            model.modelGamsScalar = new ModelGamsScalar(model);
            //            model.loadedFromCacheFile = false;
            //        }
            //        else
            //        {
            //            model = modelTemp;
            //            if (Globals.runningOnTTComputer) new Writeln("TTH: Parallel protobuf read: " + G.Seconds(t0));
            //            DateTime t1 = DateTime.Now;
            //            if (model.type == EModelType.GAMSScalar) GAMSScalarModelHelper(true, model.modelGamsScalar);
            //            model.loadedFromCacheFile = true;
            //            timeCompile = "compile: " + G.Seconds(t1);
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        if (G.IsUnitTesting())
            //        {
            //            throw;
            //        }
            //        else
            //        {
            //            //do nothing, we then have to parse the file
            //            model.loadedFromCacheFile = false;
            //        }
            //    }
            //}
            //else
            //{
            //    model.loadedFromCacheFile = false;
            //}

            //if (model.loadedFromCacheFile)
            //{
            //    //no writing of .mdl file of course                
            //}
            //else
            //{
            //    model = ReadGAMSScalarModel2(o, folders, model, input);

            //    try //not the end of world if it fails (should never be done if model is read from zipped protobuffer (would be waste of time))
            //    {
            //        DateTime dt1 = DateTime.Now;
            //        if (model.type == EModelType.GAMSScalar) GAMSScalarModelHelper(false, model.modelGamsScalar);
            //        //TODO
            //        //TODO what about last argument ms?
            //        //TODO
            //        Program.WriteParallelModel(Program.options.system_threads, input.zipFilePathAndName, modelHash, 0, model);
            //    }
            //    catch (Exception e)
            //    {
            //        //do nothing, not the end of the world if it fails
            //    }
            //}

            Table tab = new Table();

            tab.CurRow.SetTopBorder(1, 1);

            tab.CurRow.SetText(1, "MODEL " + Path.GetFileNameWithoutExtension(input.zipFilePathAndName));
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.Next();

            tab.CurRow.SetText(1, "Model   : " + input.zipFilePathAndName);
            tab.CurRow.Next();

            tab.CurRow.SetText(1, "Periods : " + model.modelGamsScalar.absoluteT1.ToString() + "-" + model.modelGamsScalar.absoluteT2.ToString() + " = " + GekkoTime.Observations(model.modelGamsScalar.absoluteT1, model.modelGamsScalar.absoluteT2) + " periods");
            //tab.CurRow.Next();                        

            //tab.CurRow.SetText(1, "Lags      : Largest lag = " + 0 + ", largest lead = " + 0);
            tab.CurRow.SetBottomBorder(1, 1);
            //tab.CurRow.Next();
            //tab.CurRow.SetText(1, "Periods         = " + modelGamsScalar.t1.ToString() + "-" + modelGamsScalar.t2.ToString() + " = " + GekkoTime.Observations(modelGamsScalar.t1, modelGamsScalar.t2) + " periods");
            //tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "All eqs         = " + model.modelGamsScalar.CountEqs(1) + " (all dimensions)");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Eqs per period  = " + model.modelGamsScalar.CountEqs(2) + " (no time dimension)");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Eq names        = " + model.modelGamsScalar.CountEqs(3) + " (no dimensions)");
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "All vars        = " + model.modelGamsScalar.CountVars(1) + " (all dimensions)");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Vars per period = " + model.modelGamsScalar.CountVars(2) + " (no time dimension)");
            tab.CurRow.Next();
            tab.CurRow.SetText(1, "Var names       = " + model.modelGamsScalar.CountVars(3) + " (no dimensions)");
            tab.CurRow.SetBottomBorder(1, 1);
            tab.CurRow.SetLeftBorder(1);
            tab.CurRow.SetRightBorder(1);

            int widthRemember = Program.options.print_width;
            Program.options.print_width = int.MaxValue;
            try
            {
                List<string> ss = tab.Print();
                using (Writeln txt = new Writeln())
                {
                    foreach (string s in ss)
                    {
                        txt.MainAdd(s);
                        txt.MainNewLineTight();
                    }
                    if (model.modelCommon.loadedFromCacheFile)
                    {
                        txt.MainAdd("Time: " + timeLoadCache + ", " + timeCompile + ", total: " + G.Seconds(t));                        
                    }
                    else
                    {
                        txt.MainAdd("Extracting from files, total time: " + G.Seconds(t));
                    }
                    txt.MainNewLineTight();
                }
            }
            finally
            {
                //resetting, also if there is an error
                Program.options.print_width = widthRemember;
            }
            
            return model;
            
        }

        private static Model ReadGAMSScalarModel2(O.Model o, List<string> folders, Model model, GAMSScalarModelSettings input)
        {
            //if (Globals.runningOnTTComputer) MessageBox.Show("TT comment: Parsing scalar model...");
            FindFileHelper ffh2 = Program.FindFile(input.zipFilePathAndName + "\\" + "ModelInfo.json", folders, true, true, o.p);
            string jsonCode = G.RemoveComments(Program.GetTextFromFileWithWait(ffh2.realPathAndFileName));
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> jsonTree = null;
            try
            {
                jsonTree = (Dictionary<string, object>)serializer.DeserializeObject(jsonCode);
            }
            catch (Exception e)
            {
                using (Error txt = new Error())
                {
                    txt.MainAdd("The ModelInfo.json file does not seem correctly formatted.");
                    txt.MoreAdd("Gekko needs a suitable ModelInfo.json inside the .zip file to describe the model files. See description in the {a{MODEL¤download.htm}a} commmand.");
                    txt.MoreNewLine();
                    txt.MoreAdd("The technical error message is the following: " + e.Message);
                }
            }

            DateTime t3 = DateTime.Now;

            try { input.unrolledModel = (string)jsonTree["unrolledModel"]; } catch { }
            if (input.unrolledModel == null)
            {
                new Error("JSON: setting unrolledModel not found");
            }
            else
            {
                input.ffh_unrolledModel = Program.FindFile(input.zipFilePathAndName + "\\" + input.unrolledModel, folders, true, true, o.p);
            }

            try { input.unrolledNames = (string)jsonTree["unrolledNames"]; } catch { }
            if (input.unrolledNames == null)
            {
                new Error("JSON: setting unrolledNames not found");
            }
            else
            {
                input.ffh_unrolledNames = Program.FindFile(input.zipFilePathAndName + "\\" + input.unrolledNames, folders, true, true, o.p);
            }

            try { input.rawModel = (string)jsonTree["rawModel"]; } catch { }
            if (input.rawModel == null)
            {
                //ignore
            }
            else
            {
                input.ffh_rawModel = Program.FindFile(input.zipFilePathAndName + "\\" + input.rawModel, folders, true, true, o.p);
            }

            if (Globals.runningOnTTComputer) new Writeln("TTH: Unzip: " + G.Seconds(t3));

            model = ReadGamsScalarModelEquations(input, model);

            DateTime t1 = DateTime.Now;
            return model;
        }

        /// <summary>
        /// Inflate/deflate objects that mitigate the problem that protobuf does not support jagged arrays.
        /// </summary>
        /// <param name="deserialize"></param>
        public static void GAMSScalarModelHelper(bool deserialize, ModelGamsScalar modelGamsScalar)
        {
            if (deserialize)
            {
                modelGamsScalar.bb = new int[modelGamsScalar.bbTemp.Length][];
                for (int i = 0; i < modelGamsScalar.bbTemp.Length; i++)
                {
                    modelGamsScalar.bb[i] = modelGamsScalar.bbTemp[i].storage;
                }
                modelGamsScalar.bbTemp = null;

                modelGamsScalar.dd = new int[modelGamsScalar.ddTemp.Length][];
                for (int i = 0; i < modelGamsScalar.ddTemp.Length; i++)
                {
                    modelGamsScalar.dd[i] = modelGamsScalar.ddTemp[i].storage;
                }
                modelGamsScalar.ddTemp = null;

                modelGamsScalar.a = new double[modelGamsScalar.aTemp.Length][];
                for (int i = 0; i < modelGamsScalar.aTemp.Length; i++)
                {
                    modelGamsScalar.a[i] = modelGamsScalar.aTemp[i].storage;
                }
                modelGamsScalar.aTemp = null;

                // -----

                modelGamsScalar.r_ref = G.CreateNaN(modelGamsScalar.CountEqs(1));
                modelGamsScalar.r = G.CreateNaN(modelGamsScalar.CountEqs(1));                

                //Loading of Func<>s
                modelGamsScalar.functions = new Func<int, double[], double[][], double[], int[][], int[][], int, double>[modelGamsScalar.unique];
                Compile5(modelGamsScalar.csCodeLines, modelGamsScalar.functions);                                
            }
            else
            {
                //Note: bbTemp, eeTemp and aTemp will never be changed, so we just point to these
                //      arrays inside the real bb, ee and a objects. This should be safe: protobuf
                //      does not tamper with these objects.
                modelGamsScalar.bbTemp = new IntArray[modelGamsScalar.bb.Length];
                for (int i = 0; i < modelGamsScalar.bb.Length; i++)
                {
                    modelGamsScalar.bbTemp[i] = new IntArray();
                    modelGamsScalar.bbTemp[i].storage = modelGamsScalar.bb[i];
                }

                modelGamsScalar.ddTemp = new IntArray[modelGamsScalar.dd.Length];
                for (int i = 0; i < modelGamsScalar.dd.Length; i++)
                {
                    modelGamsScalar.ddTemp[i] = new IntArray();
                    modelGamsScalar.ddTemp[i].storage = modelGamsScalar.dd[i];
                }

                modelGamsScalar.aTemp = new DoubleArray[modelGamsScalar.a.Length];
                for (int i = 0; i < modelGamsScalar.a.Length; i++)
                {
                    modelGamsScalar.aTemp[i] = new DoubleArray();
                    modelGamsScalar.aTemp[i].storage = modelGamsScalar.a[i];
                }
            }
        }

        /// <summary>
        /// Read (parse) a .gms GAMS model, transforming it into Gekko-understandable equations.
        /// Calls ReadGamsEquation() for each equation.
        /// </summary>
        /// <param name="textInputRaw"></param>
        /// <param name="fileName"></param>
        /// <param name="dependents"></param>
        /// <param name="o"></param>
        public static ModelGams ReadGamsModelHelper(string textInputRaw, string fileName, GekkoDictionary<string, string> dependents, bool dump, bool silent, Model model)
        {
            StringBuilder sb1 = new StringBuilder();
            sb1.AppendLine();

            StringBuilder sb2 = new StringBuilder();
            sb2.AppendLine();

            int eqCounter = 0;

            //GAMS comments: star as first char, $ontext/offtext, # as end of line, /* */,

            //See also #jkadf773js7s
            string txt = textInputRaw;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "!!", "#" };
            var tags3 = new List<Tuple<string, string>>() { new Tuple<string, string>("$ontext", "$offtext") };
            var tags4 = new List<string>() { "*" };

            TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(txt, tags1, tags2, tags3, tags4);
            GekkoDictionary<string, List<ModelGamsEquation>> equationsByVarname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);
            GekkoDictionary<string, List<ModelGamsEquation>> equationsByEqname = new GekkoDictionary<string, List<ModelGamsEquation>>(StringComparer.OrdinalIgnoreCase);

            List<string> problems = new List<string>();

            int counter = 0;            

            foreach (TokenHelper tok in tokens2.subnodes.storage)
            {
                if (tok.type == ETokenType.EOL)
                {
                    counter++;
                }

                if (tok.s == "." && tok.Offset(1).s == "." && tok.Offset(1).leftblanks == 0)
                {
                    if (tok.Offset(2).s == "\\" || tok.Offset(2).s == "/")
                    {
                        //this may be part of a path, $GDXIN ..\Data\ADAM\estbk_okt16.gdx
                    }
                    else
                    {
                        eqCounter = ReadGamsEquation(sb1, sb2, eqCounter, equationsByVarname, equationsByEqname, tok, dependents, problems, dump);
                    }
                }
            }
            ModelGams modelGams = new ModelGams(model);
            modelGams.equationsByVarname = equationsByVarname;
            modelGams.equationsByEqname = equationsByEqname;

            if (!silent)
            {
                using (Writeln txt2 = new Writeln())
                {
                    txt2.MainAdd("MODEL: " + Path.GetFileNameWithoutExtension(fileName));
                    txt2.MainNewLineTight();
                    txt2.MainAdd("Read " + counter + " lines from " + fileName);
                    txt2.MainNewLineTight();
                    txt2.MainAdd("Found " + equationsByVarname.Count + " distinct equations (use DISP to display them)");
                    txt2.MainNewLineTight();
                    if (problems.Count > 0)
                    {
                        txt2.MainAdd("There were the following problems while reading the model:");
                        txt2.MainNewLineTight();
                        foreach (string s in problems)
                        {
                            txt2.MainAdd("+++  " + s);
                            txt2.MainNewLineTight();
                        }
                    }
                }
            }

            if (dump)
            {
                using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\dump.gcm", null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(sb1);
                }

                using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\dump.gms", null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = G.GekkoStreamWriter(fs))
                {
                    sw.Write(sb2);
                }
            }
            return modelGams;
        }

        /// <summary>
        /// Read (parse) a .gms/.gmy GAMS equation, translating it into an equivalent Gekko equation.
        /// The result is put into a ModelGamsEquation object.
        /// </summary>
        private static int ReadGamsEquation(StringBuilder sb1, StringBuilder sb2, int eqCounter, Dictionary<string, List<ModelGamsEquation>> equationsByVarname, Dictionary<string, List<ModelGamsEquation>> equationsByEqname, TokenHelper tok, GekkoDictionary<string, string> dependents, List<string> problems, bool dump)
        {
            WalkTokensHelper wh = new WalkTokensHelper();

            int iEqStart = 0;
            //searches for '..' with no blank between (could be improved)
            //now we search backwards for start of line
            for (int i2 = -1; i2 > -int.MaxValue; i2--)
            {
                int iLineStart = -12345;
                if (tok.Offset(i2) == null || tok.Offset(i2).type == ETokenType.EOL)
                {
                    iEqStart = i2 + 1;
                    break;
                }
            }

            int i = iEqStart;

            //-----------------------------------------------
            //now we are ready for the equation definition
            //-----------------------------------------------

            //The equation is of this form:

            //e_pi(i,ds,t) $ (tx0(t) and d1i(i,ds,t)) .. pI(i,ds,t)*qI(i,ds,t) =E= vI(i,ds,t);

            //Tokenized in tree structure it looks like this:

            //e_pi(...) $ (...) .. pI(...)*qI(...) =E= vI(...);

            //So the following:
            // eqname
            // maybe a set parenthesis
            // maybe a dollar
            //     if so either a (...) or a variable with a (...)
            // a '..' always
            // a leftside until '=e='
            // a rightside after'=e=' until semicolon

            string eqnameGams = null;
            string conditionalsGams = null;
            string conditionalsCs = null;
            string setsGams = null;
            List<string> setsGamsList = new List<string>();
            string lhsGams = null;
            string rhsGams = null;
            TokenHelper lhsTokensGams = null;
            TokenHelper rhsTokensGams = null;

            string dollar = null;

            eqnameGams = tok.Offset(i)?.s;

            i++;

            //this may be parentheses
            TokenHelper tok2 = tok.Offset(i);
            if (tok2.SubnodesTypeParenthesisStart())
            {
                setsGams = tok2.subnodes.ToString();

                List<TokenHelperComma> split = tok2.SplitCommas(true);
                foreach (TokenHelperComma item in split)
                {
                    string set = item.list.ToString();
                    setsGamsList.Add(set.Trim());
                }

                i++;

                if (tok.Offset(i).s == "$")
                {
                    i++;
                    TokenHelper tok3 = tok.Offset(i);
                    if (tok3.subnodes != null)
                    {
                        //Gekko syntax
                        conditionalsGams = tok3.subnodes.ToString();

                        //C# syntax
                        TokenHelper conditionalsTokensCs = tok3.DeepClone(null);
                        WalkTokensHandleParentheses(conditionalsTokensCs); //changes '[' and '{' into '('
                        WalkTokensHelper temp = new WalkTokensHelper();
                        WalkTokensCsSyntax(conditionalsTokensCs, temp, null);
                        conditionalsCs = conditionalsTokensCs.ToStringTrim();
                    }

                    // see also #9872034985732, removing stray " and"
                    if (tok3.SubnodesTypeParenthesisStart())
                    {

                        TokenList list = new TokenList();
                        for (int ii = 0; ii < tok3.subnodes.storage.Count; ii++)
                        {
                            if (ii < tok3.subnodes.Count() - 1 && tok3.subnodes[ii].HasNoChildren() && tok3.subnodes[ii + 1] != null && tok3.subnodes[ii + 1].HasChildren())
                            {
                                //Remove anything that looks like time restriction
                                List<TokenHelperComma> temp = tok3.subnodes[ii + 1].SplitCommas(true);
                                if (temp.Count == 1 && G.Equal(temp[0].list.ToString().Trim(), wh.t))
                                {
                                    ii += 2;
                                    if (G.Equal(tok3.subnodes[ii]?.s, "and"))
                                    {
                                        ii++;  //also check before
                                    }
                                    ii--;  //will get 1 added at loop start
                                    continue;
                                }
                            }
                            list.storage.Add(tok3.subnodes[ii]);
                        }

                        WalkTokensHelper wh2 = new WalkTokensHelper();
                        wh2.checkIfVariableIsASet = true;

                        WalkTokensHandleParentheses(list);
                        WalkTokensGekkoSyntax(list, wh2);

                        dollar = list.ToStringTrim();

                        if (dollar.StartsWith("(") && dollar.EndsWith(")"))
                        {
                            dollar = dollar.Substring(1, dollar.Length - 2).Trim();
                            if (dollar.StartsWith("and ")) dollar = dollar.Substring("and ".Length).Trim();
                        }

                        i++;
                    }
                    else
                    {
                        string s7 = tok.Offset(i).ToStringTrim();
                        if (!G.IsIdent(s7))
                        {
                            new Error("Expected a name instead of '" + s7 + "' , " + tok.Offset(i).LineAndPosText());
                            //throw new GekkoException();
                        }
                        i++;

                        string s8 = tok.Offset(i).ToStringTrim();
                        if (!(tok.Offset(i).SubnodesTypeParenthesisStart()))
                        {
                            new Error("Expected a (...) parenthesis instead of '" + s8 + "' , " + tok.Offset(i).LineAndPosText());
                            //throw new GekkoException();
                        }
                        i++;
                    }
                }
            }

            if (tok.Offset(i)?.s == "." && tok.Offset(i + 1)?.s == ".")
            {
                //good, we are at the '..' part, now comes the LHS expression
            }
            else
            {
                new Error("Expected '..' in eq definition, " + tok.Offset(i).LineAndPosText());
                //throw new GekkoException();
            }
            i++;
            i++;

            //now ready for the contents of the equation

            //find lhs of equation -----------------------------------------
            int i1Start = i;

            List<string> eqsign = new List<string>() { "=", "e", "=" };

            int iEqual = tok.Search(i1Start, eqsign, false, false);

            if (iEqual == -12345)
            {
                new Error("Could not find '=e=' in eq definition, " + tok.Offset(i).LineAndPosText());
            }

            int i1End = iEqual - 1;
            int i2Start = i1End + eqsign.Count + 1;
            int iSemi = tok.Search(i2Start, new List<string>() { ";" }, false, false);

            if (iSemi == -12345)
            {
                new Error("Could not find ending ';' in eq definition, " + tok.Offset(i).LineAndPosText());
            }

            int iEqEnd = iSemi;

            lhsGams = tok.OffsetInterval(i1Start, i1End).ToString().Trim();
            lhsTokensGams = tok.OffsetInterval(i1Start, i1End);

            rhsGams = tok.OffsetInterval(i2Start, iSemi - 1).ToString().Trim();
            rhsTokensGams = tok.OffsetInterval(i2Start, iSemi - 1);

            eqCounter++;

            if (false && eqCounter < 10)
            {
                G.Writeln2("Eqname:  " + eqnameGams);
                G.Writeln("Sets:    " + setsGams);
                G.Writeln("Condit.: " + conditionalsGams);
                G.Writeln("LHS:     " + lhsGams);
                G.Writeln("RHS:     " + rhsGams);
            }

            ModelGamsEquation equation = new ModelGamsEquation();

            equation.nameGams = eqnameGams;
            equation.setsGams = setsGams;
            equation.setsGamsList = setsGamsList;
            equation.conditionalsGams = conditionalsGams;
            equation.lhsGams = lhsGams;
            equation.rhsGams = rhsGams;
            equation.lhsTokensGams = lhsTokensGams;
            equation.rhsTokensGams = rhsTokensGams;

            //Gekko syntax

            TokenHelper lhsTokensGekko = equation.lhsTokensGams.DeepClone(null);
            WalkTokensHandleParentheses(lhsTokensGekko); //changes '[' and '{' into '('
            WalkTokensHelper wt1Gekko = new WalkTokensHelper();
            WalkTokensGekkoSyntax(lhsTokensGekko, wt1Gekko);
            string lhsGekko = lhsTokensGekko.ToStringTrim();

            TokenHelper rhsTokensGekko = equation.rhsTokensGams.DeepClone(null);
            WalkTokensHandleParentheses(rhsTokensGekko); //changes '[' and '{' into '('
            WalkTokensHelper wt2Gekko = new WalkTokensHelper();
            WalkTokensGekkoSyntax(rhsTokensGekko, wt2Gekko);
            string rhsGekko = rhsTokensGekko.ToStringTrim();

            ////C# syntax

            //TokenHelper lhsTokensCs = equation.lhsTokensGams.DeepClone(null);
            //WalkTokensHandleParentheses(lhsTokensCs); //changes '[' and '{' into '('
            //WalkTokensHelper wt1Cs= new WalkTokensHelper();
            //Controlled controlledLhs = new Controlled();
            //WalkTokensCsSyntax(lhsTokensCs, wt1Cs, controlledLhs);
            //string lhsCs = lhsTokensCs.ToStringTrim();

            //TokenHelper rhsTokensCs = equation.rhsTokensGams.DeepClone(null);
            //WalkTokensHandleParentheses(rhsTokensCs); //changes '[' and '{' into '('
            //WalkTokensHelper wt2Cs = new WalkTokensHelper();
            //Controlled controlledRhs = new Controlled();
            //WalkTokensCsSyntax(rhsTokensCs, wt2Cs, controlledRhs);
            //string rhsCs = rhsTokensCs.ToStringTrim();

            if (true)
            {
                int v = 3;
                if (v == 1)
                {
                    sb1.Append("PRT " + lhsGekko + ";" + G.NL);
                    sb1.Append("PRT " + rhsGekko + ";" + G.NL);
                    sb1.AppendLine();
                }
                else if (v == 2)
                {
                    sb1.Append("PRT<n> " + lhsGekko + " - ( " + rhsGekko + " );" + G.NL);
                }
                else
                {
                    string dollar2 = null;
                    if (dollar != null && dollar.Trim() != "" && dollar.Trim() != "()")
                    {
                        dollar2 = dollar.Trim();
                    }

                    sb1.AppendLine("Equation: " + eqnameGams);
                    if (dollar2 != null)
                    {
                        sb1.Append("(" + lhsGekko + ") $ (" + dollar2 + ") = " + rhsGekko + ";" + G.NL);  //always add parentheses
                    }
                    else
                    {
                        sb1.Append(lhsGekko + " = " + rhsGekko + ";" + G.NL);
                    }

                    sb2.AppendLine("" + equation.nameGams);
                    sb2.AppendLine("" + equation.setsGams);
                    sb2.AppendLine("" + equation.conditionalsGams);
                    sb2.AppendLine(equation.lhsGams + "  =  ");
                    sb2.AppendLine(equation.rhsGams);
                    sb2.AppendLine();
                    sb2.AppendLine("--------------------------------------");
                    sb2.AppendLine();

                    //if (dollar2 != null)
                    //{
                    //    sb.Append("(" + lhs + ") $ (" + dollar2 + ")" + G.NL);  //always add parentheses
                    //}
                    //else
                    //{
                    //    sb.Append(lhs + " = " + G.NL);
                    //}
                }
            }

            if (true)
            {
                equation.lhs = lhsGekko;
                equation.rhs = rhsGekko;

                //equation.lhsCs = lhsCs;
                //equation.rhsCs = rhsCs;
                equation.conditionalsCs = conditionalsCs;

                // ------------- conditionals ---------------
                // see also #9872034985732

                string conditionals2 = null;
                if (dollar != null) conditionals2 = dollar.Trim();
                if (!G.NullOrEmpty(conditionals2))
                {
                    //removes a stray ending " and" that may be left after removing time conditionals
                    if (conditionals2.EndsWith(" and", StringComparison.OrdinalIgnoreCase)) conditionals2 = conditionals2.Substring(0, conditionals2.Length - " and".Length);
                }
                equation.conditionals = conditionals2;
            }

            bool fromList = false;
            string lhsVariable = ReadGamsModelGetLhsName(equationsByVarname, equationsByEqname, lhsTokensGekko, equation, eqnameGams, dependents, problems, ref fromList);
            string s = null;
            if (fromList) s = ", designated from list";
            if (lhsVariable == null) lhsVariable = "[not identified]";
            sb1.AppendLine("--> " + lhsVariable + " (dependent" + s + ")");
            sb1.AppendLine();
            sb1.AppendLine("----------------------------------------------------------------------------------------------------------------");
            sb1.AppendLine();

            return eqCounter;
        }

        /// <summary>
        /// Tries to identify what is the LHS variable in the GAMS equation, and puts this into dictionaries for later retrieval by variable name or equation name.
        /// The method reacts to option model gams dep method = lhs|eqname, and also reacts to a #dependents list.
        /// </summary>
        private static string ReadGamsModelGetLhsName(Dictionary<string, List<ModelGamsEquation>> equationsByVarname, Dictionary<string, List<ModelGamsEquation>> equationsByEqname, TokenHelper lhsTokensGams2, ModelGamsEquation e, string eqnameGams, GekkoDictionary<string, string> dependents, List<string> problems, ref bool fromList)
        {
            string lhs = null;

            if (G.Equal(Program.options.model_gams_dep_method, "lhs"))
            {
                Program.GetLhsVariable(lhsTokensGams2, ref lhs);
            }
            else if (G.Equal(Program.options.model_gams_dep_method, "eqname"))
            {
                if (eqnameGams.Contains("__"))
                {
                    new Error("Eqname '" + eqnameGams + "': did not expect '__' substring in name");
                }
                string[] ss = eqnameGams.Split('_');
                if (ss.Length <= 1)
                {
                    new Error("Eqname '" + eqnameGams + "': did not find any '_' separators");
                }
                if (!G.Equal(ss[0], "e"))
                {
                    new Error("Eqname '" + eqnameGams + "': expected it to start with 'e_'");
                }
                if (!G.IsIdent(ss[1]))  //we use the e_{here}_..._..._... part
                {
                    new Error("Eqname '" + eqnameGams + "': could not resolve variable name");
                }
                lhs = ss[1];
            }
            else
            {
                new Error("option model gams dep method = lhs|eqname.");
            }

            string d = null; if (dependents != null) dependents.TryGetValue(eqnameGams, out d);
            string varnameFound = null;
            if (d != null)
            {
                //found in #dependents
                varnameFound = d;
                fromList = true;
            }

            if (varnameFound == null && lhs != null) varnameFound = lhs;

            if (varnameFound == null)
            {
                problems.Add("Could not find lhs variable in equation '" + eqnameGams + "' (line " + lhsTokensGams2.line + ")");
            }
            else
            {
                if (equationsByVarname.ContainsKey(varnameFound))
                {
                    equationsByVarname[varnameFound].Add(e);  //can have more than one eq with same lhs variable
                }
                else
                {
                    List<ModelGamsEquation> e2 = new List<ModelGamsEquation>();
                    e2.Add(e);
                    equationsByVarname.Add(varnameFound, e2);
                }

                if (equationsByEqname.ContainsKey(eqnameGams))
                {
                    new Error("The equation name '" + eqnameGams + "' appears multiple times");
                }
                else
                {
                    List<ModelGamsEquation> e2 = new List<ModelGamsEquation>();
                    e2.Add(e);
                    equationsByEqname.Add(eqnameGams, e2);
                }
            }
            return varnameFound;
        }

        /// <summary>
        /// Finds the GAMS quation, either looking up eqname or varname.
        /// These equations are already translated, so they are in Gekko form (no explicit time).
        /// Uses Program.CallEval() internally to convert the eqs into suitable C# Func&lt;&gt; code.
        /// And the Func&lt;&gt; code is later on used to find precedents (which variables with which lags
        /// affect the equation).
        /// </summary>
        /// <param name="eqname"></param>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static ModelGamsEquation DecompEvalGams(string eqname, string varname, Model model)
        {
            List<ModelGamsEquation> eqs = null;
            ModelGamsEquation found = null;
            if (eqname != null)
            {
                eqs = GetGamsEquationsByEqname(eqname, model);
                if (eqs == null || eqs.Count == 0)
                {
                    new Error("Equation '" + eqname + "' was not found");
                    //throw new GekkoException();
                }
                if (eqs.Count > 1)
                {
                    new Error("Internal error #809735208375", false);
                }
                found = eqs[0];  //pick the first one, probably always only one here, cf. #820948324:
            }
            else
            {
                eqs = GetGamsEquationsByVarname(varname, model);
                if (eqs == null || eqs.Count == 0)
                {
                    new Error("Variable '" + varname + "' was not found");
                    //throw new GekkoException();
                }
                if (eqs.Count > 1)
                {
                    new Warning("Variable '" + varname + "' appears in several equations, first one is picked");
                }
                found = eqs[0];  //#820948324: pick the first one, a variable name may point to several equations, for instance if y is present on the lhs in several equations.
            }

            string rhs = found.rhs.Trim();
            string lhs = found.lhs.Trim();

            string s1 = Program.EquationLhsRhs(lhs, rhs, true) + ";";  //this is a generic method, not just a GAMS method

            if (found.expressions == null || found.expressions.Count == 0)
            {
                Globals.expressions = null;  //maybe not necessary
                Program.CallEval(found.conditionals, s1);
                found.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here, similar to found.expressions = Globals.expressions
                Globals.expressions = null;  //maybe not necessary
            }
            else
            {
                //has already been done
            }

            return found;
        }

        private static void Sniff2(Model model)
        {
            DateTime dt = DateTime.Now;
            double ms1 = 0;
            double ms2 = 0;
            int n1 = 0;
            int n2 = 0;
            int n3 = 0;

            int counterA = 0;
            int counterError1 = 0;
            int counterError2 = 0;

            foreach (KeyValuePair<string, List<ModelGamsEquation>> kvp in model.modelGams.equationsByEqname)
            {
                //if (counterA > 6) break;
                if (counterA % 50 == 0) G.Writeln2("--> " + counterA);

                counterA++;
                ModelGamsEquation eq = kvp.Value[0];

                eq.expressionVariablesWithSets = new List<EquationVariablesGams>();

                string rhs = eq.rhs.Trim();
                string lhs = eq.lhs.Trim();
                string s1 = Program.EquationLhsRhs(lhs, rhs, true) + ";";

                if (eq.expressions == null || eq.expressions.Count == 0)
                {
                    Globals.expressions = null;  //maybe not necessary

                    try
                    {
                        DateTime dt1 = DateTime.Now;
                        Program.CallEval(eq.conditionals, s1);
                        ms1 += (dt1 - DateTime.Now).TotalMilliseconds;
                        n1++;
                    }
                    catch (Exception e)
                    {
                        counterError1++;
                        if (e.Message.Contains("System.OutOfMemoryException"))
                        {
                            G.Writeln2("+++ ERROR: MEMORY in equation (type 2): " + eq.nameGams);
                        }
                        else
                        {
                            G.Writeln2("+++ ERROR: in equation  (type 2): " + eq.nameGams);
                        }
                        continue;
                    }
                    eq.expressions = new List<Func<GekkoSmpl, IVariable>>(Globals.expressions);  //probably needs cloning/copying as it is done here

                    DateTime dt2 = DateTime.Now;
                    foreach (Func<GekkoSmpl, IVariable> expression in eq.expressions)
                    {

                        //Function call start --------------
                        //O.AdjustSmplForDecomp(smpl, 0);
                        //TODO: can be deleted, #p24234oi32

                        try
                        {
                            DecompOperator op = new DecompOperator("d");
                            GekkoTime per1 = new GekkoTime(EFreq.A, 2020, 1);
                            GekkoTime per2 = new GekkoTime(EFreq.A, 2020, 1);
                            string residualName = "residual___";
                            int funcCounter = 0;
                            DecompData dd = Gekko.Decomp.DecompLowLevel(per1, per2, expression, Gekko.Decomp.DecompBanks_OLDREMOVESOON(op), residualName, ref funcCounter);

                            List<string> m1 = new List<string>();
                            List<string> m2 = new List<string>();
                            foreach (string s in dd.cellsContribD.storage.Keys)
                            {
                                string ss5 = Program.DecompGetNameFromContrib(s);
                                if (!m1.Contains(ss5, StringComparer.OrdinalIgnoreCase))
                                {
                                    m1.Add(ss5);
                                }
                            }
                            EquationVariablesGams temp = new EquationVariablesGams();
                            temp.equationVariables = m1;
                            eq.expressionVariablesWithSets.Add(temp);
                        }
                        catch (Exception e)
                        {
                            counterError2++;
                            eq.expressionVariablesWithSets.Add(null); //keep alignment
                            if (e.Message.Contains("System.OutOfMemoryException"))
                            {
                                G.Writeln2("+++ ERROR: MEMORY in equation: " + eq.nameGams);
                            }
                            else
                            {
                                G.Writeln2("+++ ERROR: in equation: " + eq.nameGams);
                            }
                            break;
                        }
                        n2++;
                    }
                    ms2 += (dt2 - DateTime.Now).TotalMilliseconds;
                    n3++;
                    Globals.expressions = null;  //maybe not necessary
                }
            }
            G.Writeln2("EVAL on " + counterA + " eqs, errors in " + counterError1 + "/" + counterError2 + " of these, " + (dt - DateTime.Now).TotalMilliseconds / 1000d + " " + (-ms1 / 1000d) + " " + (-ms2 / 1000d));
            G.Writeln2("n1 " + n1 + " n2 " + n2 + " n3 " + n3);
        }

        private static List<ModelGamsEquation> GetGamsEquationsByEqname(string variable, Model model)
        {            
            if (model.modelGams.equationsByEqname == null || model.modelGams.equationsByEqname.Count == 0)
            {
                new Error("No GAMS equations found");
            }
            List<ModelGamsEquation> eqs = null; model.modelGams.equationsByEqname.TryGetValue(variable, out eqs);
            return eqs;
        }

        public static List<ModelGamsEquation> GetGamsEquationsByVarname(string variable, Model model)
        {
            if (model.modelGams.equationsByVarname == null || model.modelGams.equationsByVarname.Count == 0)
            {
                new Error("No GAMS equations found");
            }
            List<ModelGamsEquation> eqs = null; model.modelGams.equationsByVarname.TryGetValue(variable, out eqs);
            return eqs;
        }

        public static Tuple<GekkoDictionary<string, string>, StringBuilder> GetDependentsGams(IVariable opt_dep)
        {
            GekkoDictionary<string, string> dependents = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            //hashHelper: will get the format: "--- dependents ---<NL>a;b;c<NL>c,d,e<NL>"
            //the dependents list does not change the model per se, but it changes how DISP and other commands
            //like DECOMP show stuff.
            StringBuilder hashHelper = new StringBuilder();
            hashHelper.AppendLine();
            hashHelper.AppendLine("--- dependents ---");

            IVariable lhsList = opt_dep;
            if (lhsList != null)
            {
                List lhsList_list = lhsList as List;
                if (lhsList_list == null)
                {
                    new Error("Variable #dependents should be of list type");
                    //throw new GekkoException();
                }
                int c = 0;
                foreach (IVariable x in lhsList_list.list)
                {
                    c++;
                    if (x.Type() != EVariableType.List)
                    {
                        new Error("#dependents sublist line " + c + ": should be of list type");
                        //throw new GekkoException();
                    }
                    List x_list = x as List;

                    List<string> ss = null;

                    try
                    {
                        ss = Stringlist.GetListOfStringsFromList(x_list);
                    }
                    catch
                    {
                        new Error("#dependents sublist line " + c + ": all elements should be strings");
                        throw;
                    }

                    foreach (string s in ss)
                    {
                        hashHelper.Append(s.ToLower()).Append(";");
                    }
                    hashHelper.AppendLine();

                    if (ss.Count < 2)
                    {
                        new Error("#dependents sublist line " + c + ": must have > 1 elements");
                        //throw new GekkoException();
                    }
                    string lhs = ss[0];
                    for (int i = 1; i < ss.Count; i++)
                    {
                        //The ss list has this form for each line:
                        //qG; E_qG; E_qG_tot    --> first the lhs name, then the equations where it is a lhs variable
                        //Since each equation can only have 1 lhs, the eqnames (E_qG etc.) can at most appear 1 time in
                        //the ss list.

                        string temp = null; dependents.TryGetValue(ss[i], out temp);
                        if (temp != null)
                        {
                            new Error("#dependents sublist line " + c + ": The equation '" + ss[i] + "' already assigns '" + temp + "' as lhs");
                            //throw new GekkoException();
                        }
                        dependents.Add(ss[i], lhs);
                    }
                }
            }

            return new Tuple<GekkoDictionary<string, string>, StringBuilder>(dependents, hashHelper);
        }

        private static bool CheckIfVarIsASet(string name, WalkTokensHelper th)
        {
            bool isSetWithIndexer = false;
            if (th.checkIfVariableIsASet)
            {

                IVariable iv = Program.databanks.GetFirst().GetIVariable("#" + name);
                if (iv != null && iv.Type() == EVariableType.List)
                {
                    isSetWithIndexer = true;
                }
            }
            return isSetWithIndexer;
        }

        /// <summary>
        /// Helper
        /// </summary>
        public static void WalkTokensGekkoSyntax(TokenList nodes, WalkTokensHelper th)
        {
            foreach (TokenHelper child in nodes.storage)
            {
                WalkTokensGekkoSyntax(child, th);
            }
        }

        /// <summary>
        /// Actual transformation of GAMS equations into Gekko statements. IMPORTANT: Keep this 100% synchronized with WalkTokensCsSyntax
        /// </summary>
        /// <param name="node"></param>
        /// <param name="th"></param>
        public static void WalkTokensGekkoSyntax(TokenHelper node, WalkTokensHelper th)
        {
            //Performs these transformations:
            //- GAMS functions are not touched (log, etc)
            //-      but sqr() becomes sqrt()
            //- sum() function has # put in on sets
            //- parameter t is removed, and lags/leads like t-1 are transformed into [-1] etc. So x(a, t) --> x[#a], and x(t) --> x not x().
            //- tBase handled
            //- strings have quotes removed, x['a'] --> x[a]
            //- stuff like a.val becomes #a.val(), whereas t.val is ignored for now
            //- sameas(i,j) and sameas(i,'a') become #i==#j and #i=='a'
            //- single '=' becomes '=='
            //
            //- all t or t+1 or t-1 etc. are recorded, together with any tBase

            if (node.HasNoChildren())
            {
                //not a sub-node
                if (node.type == ETokenType.Comment)
                {
                    //handle comments so that they are eatable by Gekko
                    //TODO: $offtext/$ontext and 
                    //See also #jkadf773js7s
                    if (node.s.StartsWith("#") || node.s.StartsWith("*"))
                    {
                        node.s = "//" + node.s.Substring(1);
                    }
                    else if (node.s.StartsWith("!!"))
                    {
                        node.s = "//" + node.s.Substring(2);
                    }
                }
                else if (node.s != "" && node.type == ETokenType.Word)
                {
                    //an IDENT-type leaf node, not symbols etc.
                    //patterns like "log(" or "exp(" or "sum(" are skipped, also stuff like "*(" is avoided

                    string word = node.s;

                    TokenHelper nextNode = node.Offset(1);
                    if (nextNode != null && nextNode.HasChildren() && nextNode.SubnodesType() == "(" && nextNode.subnodes[0].leftblanks == 0)
                    {
                        //a pattern like "x(" with no blanks in between

                        if (G.Equal(node.s, "sameas"))
                        {
                            List<TokenHelperComma> split = nextNode.SplitCommas(false);
                            if (split.Count != 2)
                            {
                                new Error("Expected sameas() function with 2 arguments");
                            }

                            node.s = "";
                            split[1].comma.s = "==";
                        }
                        else if (Globals.gamsFunctions.ContainsKey(node.s))
                        {
                            string x = Globals.gamsFunctions[node.s];
                            if (x != null)
                            {
                                node.s = x;  //sqr() --> sqrt()
                            }

                            //"sum(" or "log(" or "exp(" etc.
                            if (G.Equal(node.s, "sum"))
                            {
                                if (nextNode.subnodes.Count() > 0)
                                {
                                    if (nextNode.subnodes[1].HasNoChildren())
                                    {
                                        //stuff like "sum(i, x(i))"
                                        if (nextNode.subnodes[1].type == ETokenType.Word && (G.Equal(nextNode.subnodes[2].s, ",") || G.Equal(nextNode.subnodes[2].s, "$")))
                                        {
                                            //checks that it has "sum(x," or sum(x$" pattern
                                            nextNode.subnodes[1].s = "#" + nextNode.subnodes[1].s;
                                        }
                                    }
                                    else
                                    {
                                        //stuff like "sum((i, j), x(i, j))"
                                        List<TokenHelperComma> list2 = nextNode.subnodes[1].SplitCommas(true);
                                        foreach (TokenHelperComma item in list2)
                                        {
                                            if (item.list.Count() == 1 && item.list[0].type == ETokenType.Word)
                                            {
                                                item.list[0].s = "#" + item.list[0].s;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //a "sum()" --> not handled
                                }
                            }
                        }
                        else
                        {
                            //first we check for stuff like a15t100(a), where a15t100 is a set, not a variable
                            //so it should be #a15t100[#a], not a15t100[#a]

                            bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                            if (isSetWithIndexer) node.s = "#" + node.s;

                            bool removeParenthesis = false;
                            if (true)
                            {
                                //now we look at the arguments, x(a1, a2, 's', t) or x(a1, a2, 's', t-1) or x(a1, a2, 's')
                                List<TokenHelperComma> split = nextNode.SplitCommas(true);

                                for (int iSplit = 0; iSplit < split.Count; iSplit++)
                                {
                                    TokenHelperComma helper = split[iSplit];
                                    if (helper.list.storage.Count == 0)
                                    {
                                        //empty parenthesis, how is that possible?
                                    }
                                    else if (helper.list.storage.Count == 1)
                                    {
                                        //a single token in the slot , .... , so this is not an expression like t+1 etc.

                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //helper.list[0] is the single token

                                            if (iSplit == split.Count - 1 && (G.Equal(helper.list[0].s, th.t) || G.Equal(helper.list[0].s, th.tBase)))
                                            {
                                                //t or tBase at last position

                                                if (G.Equal(helper.list[0].s, th.t))
                                                {
                                                    //normal t
                                                    //remove the trailing t
                                                    helper.list[0].Clear();
                                                    if (helper.comma == null)
                                                    {
                                                        removeParenthesis = true;  //t is the only argument as in "x(t)" which becomes "x" not "x()"
                                                    }
                                                    else
                                                    {
                                                        helper.comma.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    //tBase
                                                    //x(i, tBase) --> x[#i][%tBase]
                                                    //we need to transform one []-subnode into two consequtive
                                                    //see also #89075203489

                                                    TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                    //[%tBase]
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper(Globals.symbolScalar + helper.list[0].s));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                    TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                    if (split.Count > 1)
                                                    {
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                        for (int iii = 0; iii < split.Count - 1; iii++)
                                                        {
                                                            if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                            nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                        }
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("]"));
                                                    }
                                                    else
                                                    {
                                                        //x(i, tBase) --> x[#i][%tBase], but x(tBase) --> x[%tBase]
                                                    }

                                                    int id = nextNode.id;
                                                    TokenHelper parent = nextNode.parent;

                                                    parent.subnodes.storage.RemoveAt(id);
                                                    parent.subnodes.storage.Insert(id, nextNode2);
                                                    parent.subnodes.storage.Insert(id, nextNode1);
                                                    parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok

                                                }
                                            }
                                            else
                                            {
                                                //x(i) --> x(#i) --actually--> x[#i]
                                                helper.list[0].s = "#" + helper.list[0].s;
                                            }
                                        }
                                        else if (helper.list[0].type == ETokenType.QuotedString)
                                        {
                                            //remove the quotes
                                            helper.list[0].s = G.StripQuotes(helper.list[0].s);
                                        }
                                    }
                                    else if (helper.list.storage.Count == 3)  //x and plusminus and number
                                    {

                                        //the ... argument in (... , ... , ... , ...) is an expression, for instance t-1 etc.
                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //if (iSplit == split.Count - 1 && helper.list[0].s == "t")
                                            if (true)
                                            {
                                                //does not need to be last. Can be "t" in "x(a, 'b', t-1)", but also "a" in "x(y, a-1, t)"
                                                if (helper.list[1] != null && (helper.list[1].s == "-" || helper.list[1].s == "+"))
                                                {
                                                    //...t+... or ...t-...
                                                    if (helper.list[2] != null && (helper.list[2].type == ETokenType.Number))
                                                    {
                                                        string plusMinus = helper.list[1].s;
                                                        if (plusMinus != "+" && plusMinus != "-")
                                                        {
                                                            new Error("Expected t plus/minus an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        string number = helper.list[2].s;
                                                        int iNumber = -12345;
                                                        bool ok = int.TryParse(number, out iNumber);
                                                        if (!ok)
                                                        {
                                                            new Error("Expected '" + number + "' to be an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        //if (plusMinus == "-") iNumber = -iNumber;

                                                        if (iSplit == split.Count - 1 && G.Equal(helper.list[0].s, th.t))
                                                        {
                                                            if (iSplit == 0)
                                                            {
                                                                //x(t-1) --> x[-1]
                                                                //helper.comma will be = null
                                                                helper.list[0].Clear(); //kill the 't'completely including blanks
                                                                helper.list[1].leftblanks = 0; //no blanks to the left of for instance '-1'
                                                            }
                                                            else
                                                            {
                                                                //x(i, t-1) --> x[#i][-1]
                                                                //we need to transform one []-subnode into two consequtive
                                                                //see also #89075203489
                                                                TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 1; iii < helper.list.storage.Count; iii++)
                                                                {
                                                                    nextNode2.subnodes.storage.Add(helper.list[iii]);
                                                                }
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                                TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 0; iii < split.Count - 1; iii++)
                                                                {
                                                                    if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                                    nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                                }
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("]"));

                                                                int id = nextNode.id;
                                                                TokenHelper parent = nextNode.parent;

                                                                parent.subnodes.storage.RemoveAt(id);
                                                                parent.subnodes.storage.Insert(id, nextNode2);
                                                                parent.subnodes.storage.Insert(id, nextNode1);
                                                                parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                                                            }
                                                        }
                                                        else
                                                        {
                                                            helper.list[0].s = "#" + helper.list[0].s;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (removeParenthesis)
                            {
                                nextNode.subnodes[0].Clear();
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].Clear();
                            }
                            else
                            {
                                nextNode.subnodes[0].s = "[";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].s = "]";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].leftblanks = 0; //we do not want x[#i, #j ], x[#i, #j] is nicer.
                            }
                        }
                    }
                    else
                    {

                        //could be a standalone a here: ... $ (sameas(a, '15'))
                        bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                        if (isSetWithIndexer) node.s = "#" + node.s;

                        TokenHelper nextNode1 = node.Offset(1);
                        TokenHelper nextNode2 = node.Offset(2);

                        if (nextNode1 != null && nextNode2 != null)
                        {

                            if (nextNode1.s == "." && G.Equal(nextNode2.s, "val"))
                            {
                                //a pattern like a.val or t.val, used in for instance a.val > 15 etc.
                                //now we transform a.val into #a.val().
                                //it must use val(), since the #a elements are strings.
                                //the fact that x[#a+1] works is a special exception.
                                //node.s = "#" + node.s;
                                nextNode2.s = nextNode2.s + "()";
                            }
                        }
                    }
                }
                else if (node.s == "=")
                {
                    TokenHelper prevNode1 = node.Offset(-1);
                    if (prevNode1 != null && (prevNode1.s == "<" || prevNode1.s == ">"))
                    {
                        //do nothing, we do not want <= to become <== !
                    }
                    else
                    {
                        node.s = "==";  //stuff like ... $ (a.val = 15)
                    }
                }
                else if (node.s == "$")
                {
                    TokenHelper nextNode = node.Offset(1);  //b
                    TokenHelper nextNode2 = node.Offset(2);  //(i, j)
                    //We look for the pattern "a $ b(i, j)", where Gekko does not allow simply a $ b[#i, #j], but must use a $ (b[#i, #j])
                    if (nextNode != null && nextNode.s != "" && nextNode.type == ETokenType.Word)
                    {
                        if (nextNode2 != null && nextNode2.HasChildren() && nextNode2.SubnodesType() == "(" && nextNode2.subnodes[0].leftblanks == 0)
                        {
                            int id = nextNode.id;
                            TokenHelper parent = nextNode.parent;
                            TokenHelper newNode = new TokenHelper(); newNode.subnodes = new TokenList();
                            newNode.subnodes.storage.Add(new TokenHelper("("));
                            newNode.subnodes.storage.Add(nextNode);
                            newNode.subnodes.storage.Add(nextNode2);
                            newNode.subnodes.storage.Add(new TokenHelper(")"));
                            parent.subnodes.storage.RemoveAt(id);
                            parent.subnodes.storage.Insert(id, newNode);
                            parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                        }
                    }
                }
            }
            else
            {
                //an empty node with children
                for (int i = 0; i < node.subnodes.storage.Count; i++)  //the count may increase, because subnodes may be added dynamically (translating x[i, t-1] into x[#i][-1])
                {
                    WalkTokensGekkoSyntax(node.subnodes.storage[i], th);
                }
            }
        }

        /// <summary>
        /// Actual transformation of GAMS equations into Gekko statements. IMPORTANT: Keep this 100% synchronized with WalkTokensGekkoSyntax
        /// </summary>
        /// <param name="node"></param>
        /// <param name="th"></param>
        public static void WalkTokensCsSyntax(TokenHelper node, WalkTokensHelper th, Controlled controlled)
        {
            //Performs these transformations:
            //- GAMS functions are not touched (log, etc)
            //-      but sqr() becomes sqrt()
            //- sum() function has # put in on sets
            //- parameter t is removed, and lags/leads like t-1 are transformed into [-1] etc. So x(a, t) --> x[#a], and x(t) --> x not x().
            //- tBase handled
            //- strings have quotes removed, x['a'] --> x[a]
            //- stuff like a.val becomes #a.val(), whereas t.val is ignored for now
            //- sameas(i,j) and sameas(i,'a') become #i==#j and #i=='a'
            //- single '=' becomes '=='
            //
            //- all t or t+1 or t-1 etc. are recorded, together with any tBase

            if (node.HasNoChildren())
            {
                //not a sub-node
                if (node.s != "" && node.type == ETokenType.Word)
                {
                    //an IDENT-type leaf node, not symbols etc.
                    //patterns like "log(" or "exp(" or "sum(" are skipped, also stuff like "*(" is avoided

                    string word = node.s;

                    TokenHelper nextNode = node.Offset(1);
                    if (nextNode != null && nextNode.HasChildren() && nextNode.SubnodesType() == "(" && nextNode.subnodes[0].leftblanks == 0)
                    {
                        //a pattern like "x(" with no blanks in between

                        if (G.Equal(node.s, "sameas"))
                        {
                            List<TokenHelperComma> split = nextNode.SplitCommas(false);
                            if (split.Count != 2)
                            {
                                new Error("Expected sameas() function with 2 arguments");
                            }

                            node.s = "";
                            split[1].comma.s = "==";
                        }
                        else if (Globals.gamsFunctions.ContainsKey(node.s))
                        {
                            string x = Globals.gamsFunctions[node.s];
                            if (x != null)
                            {
                                node.s = x;  //sqr() --> sqrt()
                            }

                            //"sum(" or "log(" or "exp(" etc.
                            if (G.Equal(node.s, "sum"))
                            {
                                List<string> names = new List<string>();
                                List<List<string>> elements = new List<List<string>>();
                                string condition = null;
                                string content = null;

                                if (nextNode.subnodes.Count() > 0)
                                {
                                    if (nextNode.subnodes[1].HasNoChildren())
                                    {
                                        //stuff like "sum(i, x(i))"
                                        if (nextNode.subnodes[1].type == ETokenType.Word && (G.Equal(nextNode.subnodes[2].s, ",") || G.Equal(nextNode.subnodes[2].s, "$")))
                                        {
                                            //if it has "sum(x," or sum(x$" pattern
                                            if (G.Equal(nextNode.subnodes[2].s, "$"))
                                            {
                                                int i = StringTokenizer.FindS(nextNode.subnodes.storage, 3, ",");
                                                condition = StringTokenizer.GetTextFromLeftBlanksTokens(nextNode.subnodes.storage, 3, i - 1, true).Trim();
                                            }
                                            WalkTokensCsSyntaxHelper1(names, elements, nextNode.subnodes[1].s);

                                        }
                                    }
                                    else
                                    {
                                        //stuff like "sum((i, j), x(i, j))"
                                        if (G.Equal(nextNode.subnodes[2].s, ",") || G.Equal(nextNode.subnodes[2].s, "$"))
                                        {
                                            if (G.Equal(nextNode.subnodes[2].s, "$"))
                                            {
                                                int i = StringTokenizer.FindS(nextNode.subnodes.storage, 3, ",");
                                                condition = StringTokenizer.GetTextFromLeftBlanksTokens(nextNode.subnodes.storage, 3, i - 1, true).Trim();
                                            }
                                            List<TokenHelperComma> list2 = nextNode.subnodes[1].SplitCommas(true);
                                            foreach (TokenHelperComma item in list2)
                                            {
                                                if (item.list.Count() == 1 && item.list[0].type == ETokenType.Word)
                                                {
                                                    WalkTokensCsSyntaxHelper1(names, elements, item.list[0].s);
                                                }
                                            }
                                        }
                                    }

                                    //Now, we do the combinations ... + ... + ... + ...

                                    Controlled controlledNew = controlled.Clone();
                                    int depth = 0;
                                    Loop(depth, names, elements, controlledNew);
                                }
                                else
                                {
                                    //a "sum()" --> not handled
                                }
                            }
                        }
                        else
                        {
                            //first we check for stuff like a15t100(a), where a15t100 is a set, not a variable
                            //so it should be #a15t100[#a], not a15t100[#a]

                            bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                            if (isSetWithIndexer) node.s = "#" + node.s;

                            bool removeParenthesis = false;
                            if (true)
                            {
                                //now we look at the arguments, x(a1, a2, 's', t) or x(a1, a2, 's', t-1) or x(a1, a2, 's')
                                List<TokenHelperComma> split = nextNode.SplitCommas(true);

                                for (int iSplit = 0; iSplit < split.Count; iSplit++)
                                {
                                    TokenHelperComma helper = split[iSplit];
                                    if (helper.list.storage.Count == 0)
                                    {
                                        //empty parenthesis, how is that possible?
                                    }
                                    else if (helper.list.storage.Count == 1)
                                    {
                                        //a single token in the slot , .... , so this is not an expression like t+1 etc.

                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //helper.list[0] is the single token

                                            if (iSplit == split.Count - 1 && (G.Equal(helper.list[0].s, th.t) || G.Equal(helper.list[0].s, th.tBase)))
                                            {
                                                //t or tBase at last position

                                                if (G.Equal(helper.list[0].s, th.t))
                                                {
                                                    //normal t
                                                    //remove the trailing t
                                                    helper.list[0].Clear();
                                                    if (helper.comma == null)
                                                    {
                                                        removeParenthesis = true;  //t is the only argument as in "x(t)" which becomes "x" not "x()"
                                                    }
                                                    else
                                                    {
                                                        helper.comma.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    //tBase
                                                    //x(i, tBase) --> x[#i][%tBase]
                                                    //we need to transform one []-subnode into two consequtive
                                                    //see also #89075203489

                                                    TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                    //[%tBase]
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper(Globals.symbolScalar + helper.list[0].s));
                                                    nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                    TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                    if (split.Count > 1)
                                                    {
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                        for (int iii = 0; iii < split.Count - 1; iii++)
                                                        {
                                                            if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                            nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                        }
                                                        nextNode1.subnodes.storage.Add(new TokenHelper("]"));
                                                    }
                                                    else
                                                    {
                                                        //x(i, tBase) --> x[#i][%tBase], but x(tBase) --> x[%tBase]
                                                    }

                                                    int id = nextNode.id;
                                                    TokenHelper parent = nextNode.parent;

                                                    parent.subnodes.storage.RemoveAt(id);
                                                    parent.subnodes.storage.Insert(id, nextNode2);
                                                    parent.subnodes.storage.Insert(id, nextNode1);
                                                    parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok

                                                }
                                            }
                                            else
                                            {
                                                //x(i) --> x(#i) --actually--> x[#i]
                                                helper.list[0].s = "#" + helper.list[0].s;
                                            }
                                        }
                                        else if (helper.list[0].type == ETokenType.QuotedString)
                                        {
                                            //remove the quotes
                                            helper.list[0].s = G.StripQuotes(helper.list[0].s);
                                        }
                                    }
                                    else if (helper.list.storage.Count == 3)  //x and plusminus and number
                                    {

                                        //the ... argument in (... , ... , ... , ...) is an expression, for instance t-1 etc.
                                        if (helper.list[0].type == ETokenType.Word)
                                        {
                                            //if (iSplit == split.Count - 1 && helper.list[0].s == "t")
                                            if (true)
                                            {
                                                //does not need to be last. Can be "t" in "x(a, 'b', t-1)", but also "a" in "x(y, a-1, t)"
                                                if (helper.list[1] != null && (helper.list[1].s == "-" || helper.list[1].s == "+"))
                                                {
                                                    //...t+... or ...t-...
                                                    if (helper.list[2] != null && (helper.list[2].type == ETokenType.Number))
                                                    {
                                                        string plusMinus = helper.list[1].s;
                                                        if (plusMinus != "+" && plusMinus != "-")
                                                        {
                                                            new Error("Expected t plus/minus an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        string number = helper.list[2].s;
                                                        int iNumber = -12345;
                                                        bool ok = int.TryParse(number, out iNumber);
                                                        if (!ok)
                                                        {
                                                            new Error("Expected '" + number + "' to be an integer, " + helper.list[2].LineAndPosText());
                                                            //throw new GekkoException();
                                                        }
                                                        //if (plusMinus == "-") iNumber = -iNumber;

                                                        if (iSplit == split.Count - 1 && G.Equal(helper.list[0].s, th.t))
                                                        {
                                                            if (iSplit == 0)
                                                            {
                                                                //x(t-1) --> x[-1]
                                                                //helper.comma will be = null
                                                                helper.list[0].Clear(); //kill the 't'completely including blanks
                                                                helper.list[1].leftblanks = 0; //no blanks to the left of for instance '-1'
                                                            }
                                                            else
                                                            {
                                                                //x(i, t-1) --> x[#i][-1]
                                                                //we need to transform one []-subnode into two consequtive
                                                                //see also #89075203489
                                                                TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 1; iii < helper.list.storage.Count; iii++)
                                                                {
                                                                    nextNode2.subnodes.storage.Add(helper.list[iii]);
                                                                }
                                                                nextNode2.subnodes.storage.Add(new TokenHelper("]"));

                                                                TokenHelper nextNode1 = new TokenHelper(); nextNode1.subnodes = new TokenList();
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("["));
                                                                for (int iii = 0; iii < split.Count - 1; iii++)
                                                                {
                                                                    if (split[iii].comma != null) nextNode1.subnodes.storage.Add(split[iii].comma);
                                                                    nextNode1.subnodes.storage.AddRange(split[iii].list.storage);
                                                                }
                                                                nextNode1.subnodes.storage.Add(new TokenHelper("]"));

                                                                int id = nextNode.id;
                                                                TokenHelper parent = nextNode.parent;

                                                                parent.subnodes.storage.RemoveAt(id);
                                                                parent.subnodes.storage.Insert(id, nextNode2);
                                                                parent.subnodes.storage.Insert(id, nextNode1);
                                                                parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                                                            }
                                                        }
                                                        else
                                                        {
                                                            helper.list[0].s = "#" + helper.list[0].s;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (removeParenthesis)
                            {
                                nextNode.subnodes[0].Clear();
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].Clear();
                            }
                            else
                            {
                                nextNode.subnodes[0].s = "[";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].s = "]";
                                nextNode.subnodes[nextNode.subnodes.Count() - 1].leftblanks = 0; //we do not want x[#i, #j ], x[#i, #j] is nicer.
                            }
                        }
                    }
                    else
                    {

                        //could be a standalone a here: ... $ (sameas(a, '15'))
                        bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                        if (isSetWithIndexer) node.s = "#" + node.s;

                        TokenHelper nextNode1 = node.Offset(1);
                        TokenHelper nextNode2 = node.Offset(2);

                        if (nextNode1 != null && nextNode2 != null)
                        {

                            if (nextNode1.s == "." && G.Equal(nextNode2.s, "val"))
                            {
                                //a pattern like a.val or t.val, used in for instance a.val > 15 etc.
                                //now we transform a.val into #a.val().
                                //it must use val(), since the #a elements are strings.
                                //the fact that x[#a+1] works is a special exception.
                                //node.s = "#" + node.s;
                                nextNode2.s = nextNode2.s + "()";
                            }
                        }
                    }
                }
                else if (node.s == "=")
                {
                    TokenHelper prevNode1 = node.Offset(-1);
                    if (prevNode1 != null && (prevNode1.s == "<" || prevNode1.s == ">"))
                    {
                        //do nothing, we do not want <= to become <== !
                    }
                    else
                    {
                        node.s = "==";  //stuff like ... $ (a.val = 15)
                    }
                }
                else if (node.s == "$")
                {
                    TokenHelper nextNode = node.Offset(1);  //b
                    TokenHelper nextNode2 = node.Offset(2);  //(i, j)
                    //We look for the pattern "a $ b(i, j)", where Gekko does not allow simply a $ b[#i, #j], but must use a $ (b[#i, #j])
                    if (nextNode != null && nextNode.s != "" && nextNode.type == ETokenType.Word)
                    {
                        if (nextNode2 != null && nextNode2.HasChildren() && nextNode2.SubnodesType() == "(" && nextNode2.subnodes[0].leftblanks == 0)
                        {
                            int id = nextNode.id;
                            TokenHelper parent = nextNode.parent;
                            TokenHelper newNode = new TokenHelper(); newNode.subnodes = new TokenList();
                            newNode.subnodes.storage.Add(new TokenHelper("("));
                            newNode.subnodes.storage.Add(nextNode);
                            newNode.subnodes.storage.Add(nextNode2);
                            newNode.subnodes.storage.Add(new TokenHelper(")"));
                            parent.subnodes.storage.RemoveAt(id);
                            parent.subnodes.storage.Insert(id, newNode);
                            parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok
                        }
                    }
                }
            }
            else
            {
                //an empty node with children
                for (int i = 0; i < node.subnodes.storage.Count; i++)  //the count may increase, because subnodes may be added dynamically (translating x[i, t-1] into x[#i][-1])
                {
                    WalkTokensCsSyntax(node.subnodes.storage[i], th, controlled);
                }
            }
        }

        public static void Loop(int depth, List<string> names, List<List<string>> elements, Controlled controlled)
        {
            for (int i = 0; i < elements[depth].Count; i++)
            {
                controlled.names.Add(names[depth]);
                controlled.elements.Add(elements[depth][i]);
                if (depth + 1 < names.Count)
                {
                    Loop(depth + 1, names, elements, controlled);
                }
                else
                {
                    //walk......

                    new Writeln(Stringlist.GetListWithCommas(controlled.names) + " ----- " + Stringlist.GetListWithCommas(controlled.elements));

                    controlled.names.RemoveAt(controlled.names.Count - 1);
                    controlled.elements.RemoveAt(controlled.elements.Count - 1);
                }
            }
            if (depth > 0)
            {
                controlled.names.RemoveAt(controlled.names.Count - 1);
                controlled.elements.RemoveAt(controlled.elements.Count - 1);
            }
        }

        private static void WalkTokensCsSyntaxHelper1(List<string> controlled1, List<List<string>> controlled2, string name)
        {
            IVariable m = O.GetIVariableFromString("#" + name, O.ECreatePossibilities.NoneReturnNullButErrorForParentArraySeries);
            if (m == null) new Error("Cannot find the list #" + name + " (representing the GAMS set " + name + ")");
            controlled1.Add(name);
            controlled2.Add(new List<string>());
            foreach (IVariable iv in O.ConvertToList(m))
            {
                controlled2[controlled2.Count - 1].Add(O.ConvertToString(iv));
            }
        }

        public static void WalkTokensHandleParentheses(TokenList nodes)
        {
            foreach (TokenHelper child in nodes.storage)
            {
                WalkTokensHandleParentheses(child);
            }
        }


        public static void WalkTokensHandleParentheses(TokenHelper node)
        {
            //All [, {, } and ] are changed into soft parentheses ( )
            if (node.HasNoChildren())
            {
                //not a sub-node
                if (node.s == "[") node.s = "(";
                else if (node.s == "{") node.s = "(";
                else if (node.s == "]") node.s = ")";
                else if (node.s == "}") node.s = ")";
                return;
            }
            else
            {
                //an empty node with children

                foreach (TokenHelper child in node.subnodes.storage)
                {
                    WalkTokensHandleParentheses(child);
                }
            }
        }


        public static string HandleModelFilesGams(string input)
        {
            List<string> lines = Stringlist.ExtractLinesFromText(input);
            return GetModelHashGams(lines);
        }

        private static string GetModelHashGams(List<string> lines)
        {
            string trueHash = Program.GetMD5Hash(Stringlist.ExtractTextFromLines(lines).ToString(), null, null); //Pretty unlikely that two different gams files could produce the same hash.
            trueHash = trueHash.Trim();  //probably not necessary
            return trueHash;
        }

        public class WalkHelper
        {
            public bool useMFunctions = false;
            public List<string> eqNames = new List<string>();
            public GekkoDictionary<string, int> dictA = null;
            public string[] dictEqs = null;
            public string[] dictVars = null;
            public GekkoTime time0 = GekkoTime.tNull;  //corresponds to index 0, a[0][...]
            public GekkoTime time1 = GekkoTime.tNull;  //lowest time encounterede in variable
            public GekkoTime time2 = GekkoTime.tNull;  //highest time encounterede in variable
        }
    }

    public static class GamsData
    {
        public static void ReadGdx(Databank databank, Program.ReadInfo readInfo, string fileLocal)
        {
            //merge and date truncation:
            //do this by first reading into a Gekko databank, and then merge that with the merge facilities from gbk read

            // ---------------------------------------
            // gdx              no t               has t
            // dims
            // ---------------------------------------
            // 0                normal timeless    NA
            //
            //
            // 1                gdim = 1           normal series
            //                  timeless
            //
            // 2                gdim = 2           gdim = 1
            //                  timeless
            //
            //3                 gdim = 3           gdim = 2
            //                  timeless

            // gdxdim = gdim + (1 - istimeless)

            // only complication is that Gekko may mix timeless and non-timeless
            // subseries, maybe that should not be allowed?
            // maybe the array-superseries should know if it is timeless or not?

            string prefix = Program.options.gams_time_prefix.Trim().ToLower();
            bool hasPrefix = prefix.Length > 0;
            string file = G.AddExtension(fileLocal, "." + "gdx");
            int offset = (int)Program.options.gams_time_offset;
            DateTime dt1 = DateTime.Now;
            //int skippedSets = 0;
            int importedSets = 0;
            int counterVariables = 0;
            int counterParameters = 0;
            int yearMax = int.MinValue;
            int yearMin = int.MaxValue;

            int counterFixed = 0;

            EFreq freq = EFreq.A;
            if (G.Equal(Program.options.gams_time_freq, "u")) freq = EFreq.U;
            else if (G.Equal(Program.options.gams_time_freq, "q")) freq = EFreq.Q;
            else if (G.Equal(Program.options.gams_time_freq, "m")) freq = EFreq.M;

            string gamsDir = null; GAMSWorkspace ws = null;
            GetGAMSWorkspace(ref gamsDir, ref ws);

            if (Program.options.gams_fast)
            {
                ReadGdxFast(databank, prefix, hasPrefix, file, offset, ref importedSets, ref counterVariables, ref counterParameters, ref yearMax, ref yearMin, freq, ref gamsDir);
            }
            else
            {
                new Error("The slow gdx reader is not maintained, try the faster GDX reader with: OPTION gams fast = yes;");
            }
            readInfo.gamsNote = counterVariables + " variables, " + counterParameters + " parameters and " + importedSets + " sets";

            readInfo.startPerInFile = yearMin;
            readInfo.endPerInFile = yearMax;
            readInfo.nanCounter = 0;

            readInfo.variables = counterVariables + counterParameters + importedSets;
            readInfo.time = (DateTime.Now - dt1).TotalMilliseconds;

            readInfo.startPerResultingBank = readInfo.startPerInFile;
            readInfo.endPerResultingBank = readInfo.endPerInFile;

            databank.FileNameWithPath = readInfo.fileName; databank.FileNameWithPathPretty = readInfo.fileNamePretty;

            //TODO: Maybe only do this on the gdx variables if possible
            //Anyway, the speed penalty is small anyway.
            databank.Trim();
        }

        private static void ReadGdxFast(Databank databank, string prefix, bool hasPrefix, string file, int offset, ref int importedSets, ref int counterVariables, ref int counterParameters, ref int yearMax, ref int yearMin, EFreq freq, ref string gamsDir)
        {
            if (Program.options.gams_time_detect_auto)
            {
                new Note("'OPTION gams time detect_auto = yes' ignored in 'OPTION gams fast = yes' mode");
            }
            try
            {
                
                string msg = string.Empty;
                string producer = string.Empty;
                int errNr = 0;
                int rc;
                int[] index = new int[gamsglobals.maxdim];
                string[] indexString = new string[gamsglobals.maxdim];
                double[] values = new double[gamsglobals.val_max];
                int[] domainSyNrs = new int[gamsglobals.maxdim];
                string[] domainStrings = new string[gamsglobals.maxdim];
                int varNr = 0;
                int nrRecs = 0;
                int n = 0;
                int gdxDimensions = 0;
                string varName = string.Empty;
                int varType = 0;
                int d;
                if (gamsDir == null) gamsDir = "";                
                List<string> paramsWithoutTimeDimensionCounter = new List<string>();
                List<string> varsWithoutTimeDimensionCounter = new List<string>();

                gdxcs gdx = new gdxcs(gamsDir, ref msg);  //it seems ok if gamsSysDir = "", then it will autolocate it (but there may be a 64-bit problem...)
                if (msg != string.Empty)
                {
                    new Error("Could not load GDX library. Message: " + msg, false);
                    GdxErrorMessage();
                    throw new GekkoException();
                }
                if (true)
                {
                    rc = gdx.gdxOpenRead(file, ref errNr);
                    if (errNr != 0)
                    {
                        {
                            new Error("gdx io error");
                            //throw new GekkoException();
                        }
                    }
                    int timeIndex = -12345;
                    int uelCount = -1; int uelHighest = -1;
                    gdx.gdxUMUelInfo(ref uelCount, ref uelHighest);
                    if (uelHighest != 0)
                    {
                        new Error("Internal UEL problem (GDX)");
                        //throw new GekkoException();
                    }
                    string[] uel = new string[uelCount + 1];
                    for (int u = 1; u <= uelCount; u++)
                    {
                        string s = null;
                        int error = -1;
                        int error2 = gdx.gdxUMUelGet(u, ref s, ref error);
                        uel[u] = s;  //remember that uel[0] is empty and not meaningful
                    }

                    timeIndex = -12345; gdx.gdxFindSymbol(Program.options.gams_time_set, ref timeIndex);

                    if (timeIndex == 0 || Program.options.gams_time_set == "")
                    {
                        //this will never be true --> remove it??
                        //hmm does it ever return 0? See below regarding -1 value
                        new Error("Could not find the time set ('" + Program.options.gams_time_set + "')");
                    }

                    //varType = 0: SET
                    //varType = 1: PARAM
                    //varType = 2: VARIABLE
                    //varType = 3: EQU
                    //varType = 4: ALIAS
                    for (int i = 1; i < int.MaxValue; i++)
                    {
                        gdx.gdxSymbolInfo(i, ref varName, ref gdxDimensions, ref varType);

                        string label = null; int records = -12345; int userInfo = -12345;
                        gdx.gdxSymbolInfoX(i, ref records, ref userInfo, ref label);



                        if (gdxDimensions == -1)
                        {
                            break;  //no more symbols
                        }
                        if (varType == 0 || varType == 4)
                        {
                            //
                            //  ======================================
                            //              sets
                            //  ======================================
                            //

                            List<string> setData = null; //contains names of sets (entryNr --> symbolName)
                            List setData2 = null; //list of above
                            if (gdxDimensions == 1)
                            {
                                setData = new List<string>();
                            }
                            else
                            {
                                setData2 = new List();
                            }
                            
                            if (gdx.gdxDataReadRawStart(i, ref nrRecs) == 0)
                            {
                                new Error("gdx error");
                            }
                            
                            while (gdx.gdxDataReadRaw(ref index, ref values, ref n) != 0)
                            {
                                if (gdxDimensions == 1)
                                {
                                    string s = null;
                                    s = uel[index[0]];
                                    setData.Add(s);
                                }
                                else
                                {
                                    List<string> m = new List<string>();
                                    for (int ii = 0; ii < gdxDimensions; ii++)
                                    {
                                        m.Add(uel[index[ii]]);
                                    }
                                    List mm = new List(m);
                                    setData2.Add(mm);
                                }
                            }
                            gdx.gdxDataReadDone();

                            //add the list to databank
                            string name = Globals.symbolCollection + varName;
                            if (databank.ContainsIVariable(name))
                            {
                                databank.RemoveIVariable(name);
                            }

                            List ml = null;

                            if (gdxDimensions == 1)
                            {
                                ml = new List(setData);
                            }
                            else
                            {
                                ml = setData2;
                            }
                            
                            databank.AddIVariable(name, ml);

                            importedSets++;
                        }
                        else if (varType == 1 || varType == 2) //parameter or variable
                        {
                            //
                            //  ======================================
                            //       parameters (1) and variables (2)
                            //  ======================================
                            //

                            string varNameWithFreq = varName + Globals.freqIndicator + G.ConvertFreq(freq);

                            //always fetched, since we use it for domains
                            gdx.gdxSymbolGetDomainX(i, ref domainStrings);
                            int timeDimNr = GdxGetTimeDimNumber(ref domainSyNrs, domainStrings, gdxDimensions, gdx, timeIndex, i);

                            if (timeDimNr == -12345)
                            {
                                if (varType == 1) paramsWithoutTimeDimensionCounter.Add(varName);
                                if (varType == 2) varsWithoutTimeDimensionCounter.Add(varName);
                            }

                            if (gdx.gdxDataReadRawStart(i, ref nrRecs) == 0)
                            {
                                new Error("gdx error");
                            }

                            int hasTimeDimension = 0;
                            if (timeDimNr != -12345) hasTimeDimension = 1;

                            int gekkoDimensions; bool isMultiDim;
                            IsMultiDim(gdxDimensions, hasTimeDimension, out gekkoDimensions, out isMultiDim);

                            //See also #asf87aufkdh where similar loading is done regarding data from GAMS scalar model

                            Series tsSuperseries = null;
                            if (isMultiDim)
                            {
                                //Multi-dim timeseries
                                string[] domains = new string[gekkoDimensions];
                                int counter = 0;
                                for (d = 0; d < gdxDimensions; d++)
                                {
                                    if (d == timeDimNr) continue; //skipping time dimension
                                    if (domainStrings[counter] == "*") domains[counter] = domainStrings[d];
                                    else domains[counter] = Globals.symbolCollection + domainStrings[d];
                                    counter++;
                                }
                                if (databank.ContainsIVariable(varNameWithFreq)) databank.RemoveIVariable(varNameWithFreq);  //should not be possible, since merging is not allowed...
                                tsSuperseries = new Series(freq, varNameWithFreq);
                                tsSuperseries.meta.label = label;
                                tsSuperseries.meta.domains = domains;
                                if (hasTimeDimension == 0) tsSuperseries.type = ESeriesType.Timeless;
                                tsSuperseries.SetArrayTimeseries(gdxDimensions, hasTimeDimension == 1);
                                if (varType == 1) tsSuperseries.meta.fix = EFixedType.Parameter;
                                databank.AddIVariable(tsSuperseries.name, tsSuperseries);
                            }
                            else
                            {
                                //Zero-dimensional timeseries (that is, normal timeseries)
                                //A zero-dim timeseries in the Gekko sense can be timeless (scalar) or non-timeless (normal timeseries)
                                //in this case, we just construct a normal timeseries
                                if (databank.ContainsIVariable(varNameWithFreq)) databank.RemoveIVariable(varNameWithFreq);  //should not be possible, since merging is not allowed...
                                tsSuperseries = new Series(freq, varNameWithFreq);
                                tsSuperseries.meta.label = label;
                                if (hasTimeDimension == 0) tsSuperseries.type = ESeriesType.Timeless;
                                if (varType == 1) tsSuperseries.meta.fix = EFixedType.Parameter;
                                databank.AddIVariable(tsSuperseries.name, tsSuperseries);
                            }

                            if (varType == 1)
                            {
                                counterParameters++;
                            }
                            if (varType == 2)
                            {
                                counterVariables++;
                            }

                            List<string> oldDims = new List<string>() { "     " }; //will not match anything

                            Series tsSubseries = null;  //the subseries in one of the dimension coordinates

                            while (gdx.gdxDataReadRaw(ref index, ref values, ref n) != 0)
                            {
                                //Reading the dimension coordinates

                                int tt = -12345;
                                //StringBuilder sb = new StringBuilder();
                                List<string> dims = new List<string>();
                                for (d = 0; d < gdxDimensions; d++)
                                {
                                    if (d == timeDimNr)
                                    {
                                        //FIXME
                                        //FIXME
                                        //FIXME
                                        //FIXME pre-construct an uel_time with uel --> GekkoTime.
                                        //FIXME if there is a prefix and offset, handle that too!
                                        //FIXME
                                        //FIXME

                                        string timeElement = uel[index[d]];
                                        if (hasPrefix)
                                        {
                                            if (!timeElement.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                                            {
                                                using (Error e = new Error())
                                                {
                                                    e.MainAdd("GAMS variable/parameter " + varName + " has element '" + timeElement + "' in the time dimension (" + Program.options.gams_time_set + ").");
                                                    e.MainAdd("The time elements are expected to start with '" + prefix + "'.");
                                                    e.MainAdd("See 'OPTION gams time set' and 'OPTION gams time prefix.");
                                                }
                                            }
                                            timeElement = timeElement.Substring(prefix.Length);
                                        }

                                        tt = G.IntParse(timeElement);
                                        if (tt == -12345)
                                        {
                                            string txt = null;
                                            if (hasPrefix)
                                            {
                                                txt = ". Original time element name: '" + uel[index[d]] + "'";
                                            }
                                            new Error("Could not convert '" + timeElement + "' into an annual time period" + txt);
                                        }
                                        tt = tt + offset;
                                        continue;  //do not add it to the dims
                                    }
                                    string s = uel[index[d]];

                                    dims.Add(s);
                                }

                                bool equal = CompareDims(oldDims, dims);

                                if (equal)
                                {
                                    //keep the same ts2
                                    //if time is the last dimension, the hash is the same for all periods
                                    //this avoids getting the same Gekko variable over and over
                                }
                                else
                                {
                                    //create it
                                    if (isMultiDim)
                                    {
                                        MultidimItem mmi = new MultidimItem(dims.ToArray(), tsSuperseries);
                                        IVariable iv = null; tsSuperseries.dimensionsStorage.TryGetValue(mmi, out iv); //probably never present, if merging is not allowed
                                        if (iv == null)
                                        {
                                            tsSubseries = new Series(ESeriesType.Normal, freq, Globals.seriesArraySubName + Globals.freqIndicator + G.ConvertFreq(freq));
                                            if (timeDimNr == -12345) tsSubseries.type = ESeriesType.Timeless;
                                            tsSuperseries.dimensionsStorage.AddIVariableWithOverwrite(mmi, tsSubseries);
                                        }
                                        else
                                        {
                                            tsSubseries = iv as Series;
                                        }
                                    }
                                    else
                                    {
                                        //zero-dimensional series
                                        tsSubseries = tsSuperseries;  //just use that for this purpose
                                    }
                                }

                                double value = values[gamsglobals.val_level];

                                if (value == Globals.gamsEps)
                                {
                                    value = 0d;  //infinitely small value, in Gekko it is a real zero
                                }
                                else if (value == Globals.gamsNegInf)
                                {
                                    value = double.NegativeInfinity;
                                }
                                else if (value == Globals.gamsPosInf)
                                {
                                    value = double.PositiveInfinity;
                                }
                                else if (value == Globals.gamsNA)
                                {
                                    value = double.NaN;
                                }
                                else if (value == Globals.gamsUndf)
                                {
                                    value = double.NaN;
                                }

                                if (tt == -12345)
                                {
                                    tsSubseries.SetTimelessData(value);
                                    if (GamsIsFixed(values, value))
                                    {
                                        tsSubseries.meta.fix = EFixedType.Timeless;
                                    }
                                }
                                else
                                {
                                    //TODO
                                    //TODO
                                    //TODO record data in an array, and use setDataSequence().
                                    //TODO
                                    //TODO

                                    GekkoTime gt = new GekkoTime(freq, tt, 1);
                                    tsSubseries.SetData(gt, value);
                                    yearMax = Math.Max(tt, yearMax);
                                    yearMin = Math.Min(tt, yearMin);

                                    if (varType == 2 && GamsIsFixed(values, value))  //not for varType == 1 (parameter)
                                    {
                                        tsSubseries.meta.fix = EFixedType.Normal;  //will overwrite a lot, but never mind it is fast
                                        if (tsSubseries.meta.fixedNormal == null) tsSubseries.meta.fixedNormal = new GekkoTimeSpans();
                                        if (tsSubseries.meta.fixedNormal.data.Count == 0)
                                        {
                                            //the very first
                                            tsSubseries.meta.fixedNormal.data.Add(new GekkoTimeSpan(gt, gt));
                                        }
                                        else
                                        {
                                            GekkoTimeSpan gts = tsSubseries.meta.fixedNormal.data[tsSubseries.meta.fixedNormal.data.Count - 1];
                                            if (gts.tEnd.EqualsGekkoTime(gt.Add(-1)))
                                            {
                                                gts.tEnd = gt;
                                            }
                                            else
                                            {
                                                tsSubseries.meta.fixedNormal.data.Add(new GekkoTimeSpan(gt, gt));
                                            }
                                        }
                                    }
                                }

                                oldDims = dims; //ok to point, dims will be created from scratch at beginning of loop
                            }  //end of records/dimensions for the variable or parameter

                            gdx.gdxDataReadDone();

                        }
                        else
                        {
                            //do nothing, skip this symbol
                        }
                    }
                    
                    if (paramsWithoutTimeDimensionCounter.Count() > 0 || varsWithoutTimeDimensionCounter.Count() > 0)
                    {
                        int temp1 = counterParameters;
                        int temp2 = counterVariables;
                        Action<GAO> a = (gao) =>
                        {
                            Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPageOutput;
                            O.Cls("output");
                            if (paramsWithoutTimeDimensionCounter.Count() > 0)
                            {
                                G.Writeln("There were " + paramsWithoutTimeDimensionCounter.Count() + " out of " + temp1 + " parameters without time dimension '" + Program.options.gams_time_set + "' indicated:", ETabs.Output);
                                G.Writeln("", ETabs.Output);
                                G.Writeln(Stringlist.GetListWithCommas(paramsWithoutTimeDimensionCounter.OrderBy(q => q).ToList()), ETabs.Output);
                                G.Writeln("", ETabs.Output);
                            }

                            if (varsWithoutTimeDimensionCounter.Count() > 0)
                            {
                                G.Writeln("There were " + varsWithoutTimeDimensionCounter.Count() + " out of " + temp2 + " variables without time dimension '" + Program.options.gams_time_set + "' indicated:", ETabs.Output);
                                G.Writeln("", ETabs.Output);
                                G.Writeln(Stringlist.GetListWithCommas(varsWithoutTimeDimensionCounter.OrderBy(q => q).ToList()), ETabs.Output);
                                G.Writeln("", ETabs.Output);
                            }
                        };
                        
                        using (Warning txt = new Warning())
                        {
                            //#0897aef todo
                            txt.MainAdd((paramsWithoutTimeDimensionCounter.Count() + varsWithoutTimeDimensionCounter.Count()) + " variables/parameters without time dimension encountered");                            
                            txt.MoreAdd("There were " + paramsWithoutTimeDimensionCounter.Count() + " parameters and " + varsWithoutTimeDimensionCounter.Count() + " variables without a time dimension set '" + Program.options.gams_time_set + "' assigned as domain (" + G.GetLinkAction("show", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ").");
                            txt.MoreAdd("This is ok if the GAMS variables/parameters are really timeless, but if not, there is a problem.");
                            txt.MoreNewLine();
                            txt.MoreAdd("For Gekko to identify a time dimension for a given parameter or variable, the dimension needs to be defined over this time domain. For instance, if in GAMS IDE or GAMS Studio a variable x is shown as x[*, *], ");
                            txt.MoreAdd("this means the no domains (sets) are assigned to the dimensions. In contrast, if it is for instance shown as x[i, t] in GAMS, this means that the first dimension is assigned to the set i (#i in Gekko),");
                            txt.MoreAdd("whereas Gekko uses the second dimension as time dimension. In Gekko, a GAMS variable x[i, t] will show up as the 1-dimensional x[#i], because the time dimension is implicit.");
                            txt.MoreNewLine();
                            txt.MoreAdd("If, for some reason, Gekko does not or cannot recognize some dimension of a parameter or variable x as the time dimension,");
                            txt.MoreAdd("the imported data will look strange. For instance, if x is defined over countries and years in the GAMS gdx,");
                            txt.MoreAdd("the resulting array-timeseries in Gekko is expected to be 1-dimensional (with time as an implicit dimension).");
                            txt.MoreAdd("If the time dimension is not recognized, a 2-dimensional array-series (containing so-called timeless timeseries as sub-elements) will show up in Gekko, and this array-series");
                            txt.MoreAdd("will be fundamentally useless inside Gekko. ");
                            txt.MoreNewLine();
                            txt.MoreAdd("If the time dimension in the gdx file has assigned a set name different from 't', you can use 'OPTION gams time set' to change the name.");
                            txt.MoreAdd("Your parameter or variable then needs to be defined over this set. Defining over the universal set '*' will not do.");
                            txt.MoreNewLine();
                            txt.MoreAdd("If you have a gdx file with a parameter or variable x without domain information, you may fix the problem like this.");
                            txt.MoreAdd("Let us assume that x is defined over countries and time periods, but that x shows up as x[*, *] in GAMS IDE or GAMS Studio,");
                            txt.MoreAdd("telling us that x has no domain information. In GAMS, you can now do the following (we are assuming that x is a parameter):");
                            txt.MoreNewLine();
                            txt.MoreAdd("Set countries; Set t;");
                            txt.MoreNewLineTight();
                            txt.MoreAdd("Parameter x(countries, t);");
                            txt.MoreNewLineTight();
                            txt.MoreAdd("$gdxin 'input.gdx'");
                            txt.MoreNewLineTight();
                            txt.MoreAdd("$load countries < x.dim1 t < x.dim2 x = x");
                            txt.MoreNewLineTight();
                            txt.MoreAdd("execute_unload 'output.gdx';");
                            txt.MoreNewLine();
                            txt.MoreAdd("After this, you may now read output.gdx into Gekko, where x will show up as a 1-dimensional array-series.");
                        }
                    }
                }
                errNr = gdx.gdxClose();
                if (errNr != 0)
                {
                    new Error("gdx io error");
                }
            }
            catch (Exception e)
            {
                new Error("GDX import failed with an unexpected error.");
            }
        }

        /// <summary>
        /// Finds out if this is a Gekko array-series, and how many dimensions in Gekko. Parameter
        /// gdxDimensions contains all dimensions possibly including time. Parameter
        /// hasTimeDimension can be 0 or 1.
        /// </summary>
        /// <param name="gdxDimensions"></param>
        /// <param name="hasTimeDimension"></param>
        /// <param name="gekkoDimensions"></param>
        /// <param name="isMultiDim"></param>
        public static void IsMultiDim(int gdxDimensions, int hasTimeDimension, out int gekkoDimensions, out bool isMultiDim)
        {
            gekkoDimensions = gdxDimensions - hasTimeDimension;
            isMultiDim = true;
            if (gekkoDimensions == 0) isMultiDim = false;
        }        

        public static void WriteGdx(Databank databank, GekkoTime t1, GekkoTime t2, string pathAndFilename, List<ToFrom> list)
        {
            //merge and date truncation:
            //do this by first reading into a Gekko databank, and then merge that with the merge facilities from gbk read

            DateTime t = DateTime.Now;
            double[] gdxValues = G.CreateArrayDouble(gamsglobals.val_max, 0d);
            gdxValues[gamsglobals.val_scale] = 1d;

            string prefix = Program.options.gams_time_prefix.Trim().ToLower();
            bool hasPrefix = prefix.Length > 0;
            //string file = AddExtension(file2, "." + "gdx");
            int offset = (int)Program.options.gams_time_offset;
            DateTime dt1 = DateTime.Now;
            int skippedSets = 0;
            int exportedSets = 0;
            int counterVariables = 0;
            int counterParameters = 0;
            int yearMax = int.MinValue;
            int yearMin = int.MaxValue;

            string gamsDir = null; GAMSWorkspace ws = null;
            GetGAMSWorkspace(ref gamsDir, ref ws);

            EFreq freq = EFreq.A;
            if (G.Equal(Program.options.gams_time_freq, "u")) freq = EFreq.U;
            else if (G.Equal(Program.options.gams_time_freq, "q")) freq = EFreq.Q;
            else if (G.Equal(Program.options.gams_time_freq, "m")) freq = EFreq.M;

            double[] d = new double[1];  //used for sets

            int syCnt = 0, uelCnt = 0;

            //GAMSWorkspace ws = null;

            if (true)
            {
                string Msg = string.Empty;

                string Sysdir;
                string Producer = string.Empty;
                int ErrNr = 0;
                int rc;
                string[] Indx = new string[gamsglobals.maxdim];
                double[] Values = new double[gamsglobals.val_max];
                int VarNr = 0;
                int NrRecs = 0;
                int N = 0;
                int Dimen = 0;
                string VarName = string.Empty;
                int VarTyp = 0;
                int D;

                gdxcs gdx = new gdxcs(gamsDir, ref Msg);  //it seems ok if gamsSysDir = "", then it will autolocate it (but there may be a 64-bit problem...)
                //GdxFast gdx = new gdxcs(Sysdir, ref Msg);
                if (Msg != string.Empty)
                {
                    if (false)
                    {
                        Console.WriteLine("**** Could not load GDX library");
                        Console.WriteLine("**** " + Msg);
                    }
                }
                gdx.gdxGetDLLVersion(ref Msg);
                if (false)
                {
                    Console.WriteLine("Using GDX DLL version: " + Msg);
                }                

                if (true)
                {
                    gdx.gdxOpenWrite(pathAndFilename, "Gekko", ref ErrNr);
                    if (ErrNr != 0)
                    {
                        //xp_example1.ReportIOError(ErrNr);
                        throw new GekkoException();
                    }
                    //int counter = 0;
                    foreach (ToFrom bnv in list)
                    {

                        IVariable iv = O.GetIVariableFromString(bnv.s1, O.ECreatePossibilities.NoneReportError, true);

                        string name = bnv.s2;
                        string nameWithoutFreq = G.Chop_GetName(name);

                        if (iv.Type() == EVariableType.Series)
                        {

                            Series ts = iv as Series;

                            string label = ""; if (ts.meta?.label != null) label = ts.meta.label;  //label = null will fail with weird error later on

                            int timeDimension = 1;
                            if (ts.type == ESeriesType.Timeless)
                            {
                                timeDimension = 0;
                            }
                            else if (ts.type == ESeriesType.ArraySuper)
                            {
                                int ntimeless = 0;
                                int nnontimeless = 0;
                                foreach (IVariable iv2 in ts.dimensionsStorage.storage.Values)
                                {
                                    if ((iv2 as Series).type == ESeriesType.Timeless) ntimeless++;
                                    else nnontimeless++;
                                }
                                if (ntimeless > 0 && nnontimeless > 0)
                                {
                                    new Error("The array-timeseries " + ts.name + " has subseries that are both timeless and non-timeless --> cannot write to GDX.");
                                }
                                if (ntimeless > 0) timeDimension = 0;
                                //if ntimeless + nnontimeless == 0 it will be assumed to have time-dim in GAMS --> hard to know.
                            }

                            string[] gekkoDomains = ts?.meta.domains;

                            string[] domains = new string[ts.dimensions + timeDimension];
                            for (int i = 0; i < domains.Length; i++) domains[i] = "*";  //default

                            if (gekkoDomains != null)
                            {
                                for (int i = 0; i < domains.Length; i++)
                                {
                                    try
                                    {
                                        domains[i] = gekkoDomains[i].Substring(1);  //removes starting '#'
                                    }
                                    catch
                                    {
                                        //if something is wrong here regarding indexes and their length, the whole thing does not crash (and worst case, domains[i] just has a '*')
                                        //also guards agains tricky stuff if it is timeless series.
                                    }
                                }
                            }

                            if (timeDimension == 1) domains[domains.Length - 1] = Program.options.gams_time_set;  //we alway put the t domain last

                            //counter++;

                            //Choose if the (array)series is a variable or parameter (in GAMS sense).
                            int dt_ = gamsglobals.dt_var;                            
                            if (ts.meta != null && ts.meta.fix == EFixedType.Parameter) dt_ = gamsglobals.dt_par;                            
                            if (gdx.gdxDataWriteStrStart(nameWithoutFreq, label, domains.Length, dt_, 0) == 0)
                            {
                                new Error("Internal GAMS/gdx problem (gdxDataWriteStrStart)");
                            }

                            gdx.gdxSystemInfo(ref syCnt, ref uelCnt);

                            if (gdx.gdxSymbolSetDomainX(syCnt, domains) == 0)
                            {
                                new Error("Could not write domain names (gdxSymbolSetDomainX)");
                            }

                            if (ts.type == ESeriesType.ArraySuper)
                            {
                                foreach (KeyValuePair<MultidimItem, IVariable> kvp in ts.dimensionsStorage.storage)
                                {
                                    string[] ss = kvp.Key.storage;
                                    WriteGdxHelper2(t1, t2, hasPrefix, gdx, kvp.Value as Series, ss, gdxValues);
                                }
                            }
                            else
                            {
                                //normal timeseries
                                WriteGdxHelper2(t1, t2, hasPrefix, gdx, ts, new string[0], gdxValues);
                            }

                            if (gdx.gdxDataWriteDone() == 0)
                            {                                
                                throw new GekkoException();
                            }
                            counterVariables++;
                        }
                        else if (iv.Type() == EVariableType.List)
                        {
                            if (gdx.gdxDataWriteStrStart(nameWithoutFreq.Replace(Globals.symbolCollection.ToString(), ""), "", 1, gamsglobals.dt_set, 0) == 0)
                            {                                
                                throw new GekkoException();
                            }

                            List l = iv as List;

                            foreach (string s in Stringlist.GetListOfStringsFromListOfIvariables(l.list.ToArray()))
                            {
                                if (gdx.gdxDataWriteStr(new string[] { s }, d) == 0)
                                {
                                    new Error("Problem writing set (list) for gdx");
                                }
                            }

                            if (gdx.gdxDataWriteDone() == 0)
                            {                                
                                throw new GekkoException();
                            }
                            exportedSets++;
                        }
                        else continue;
                    }
                }

                ErrNr = gdx.gdxClose();
                if (ErrNr != 0)
                {                    
                    throw new GekkoException();
                }

                G.Writeln2("Wrote " + counterVariables + " variables and " + exportedSets + " sets to " + pathAndFilename + " (" + G.Seconds(t) + ")");
                if (skippedSets > 0) new Note(skippedSets + " sets with dim > 1 were not imported");
            }
        }

        public static void WriteGdxSlow(Databank databank, GekkoTime t1, GekkoTime t2, string pathAndFilename, List<ToFrom> list)
        {
            //TODO: try-catch if writing fails

            bool usePrefix = false;
            if (Program.options.gams_time_prefix.Length > 0) usePrefix = true;

            DateTime t00 = DateTime.Now;
            int counterVariables = 0;
            int timelessCounter = 0;

            DateTime dt1 = DateTime.Now;

            string gamsDir = Program.options.gams_exe_folder.Trim();
            if (gamsDir.EndsWith("\\")) gamsDir = gamsDir.Substring(0, gamsDir.Length - "\\".Length);
            if (gamsDir.Trim() == "") gamsDir = null;  //must be so and not an empty string in the GAMSWorkspace call later on

            GAMSWorkspace ws = null;
            try
            {
                ws = new GAMSWorkspace(workingDirectory: Program.options.folder_working, systemDirectory: gamsDir);
            }
            catch (Exception e)
            {
                using (Error err = new Error())
                {
                    err.MainAdd("*** ERROR: Import of gdx file (GAMS) failed. GAMSWorkspace problem.");
                    err.MainNewLineTight();
                    err.MainAdd("Technical error:");
                    err.MainNewLineTight();
                    err.MainAdd(e.Message);
                    err.MainNewLineTight();
                    err.MainAdd("Note: you may manually indicate the GAMS program folder with 'OPTION gams exe folder = ...;'");
                }
            }

            GAMSDatabase db = ws.AddDatabase();

            foreach (ToFrom bnv in list)
            {
                string name = bnv.s2;  // bnv.name;


                string nameWithoutFreq = G.Chop_RemoveFreq(name);


                IVariable iv = O.GetIVariableFromString(bnv.s1, O.ECreatePossibilities.NoneReportError, true);


                Series ts = iv as Series;
                if (ts == null) continue;  //only write timeseries at the moment

                string label = ""; if (ts.meta?.label != null) label = ts.meta.label;  //label = null will fail with weird error later on

                int timeDimension = 1;
                if (ts.type == ESeriesType.Timeless)
                {
                    timeDimension = 0;
                }
                else if (ts.type == ESeriesType.ArraySuper)
                {
                    int ntimeless = 0;
                    int nnontimeless = 0;
                    foreach (IVariable iv2 in ts.dimensionsStorage.storage.Values)
                    {
                        if ((iv2 as Series).type == ESeriesType.Timeless) ntimeless++;
                        else nnontimeless++;
                    }
                    if (ntimeless > 0 && nnontimeless > 0)
                    {
                        new Error("The array-timeseries " + ts.name + " has subseries that are both timeless and non-timeless --> cannot write to GDX.");
                    }
                    if (ntimeless > 0) timeDimension = 0;
                    //if ntimeless + nnontimeless == 0 it will be assumed to have time-dim in GAMS --> hard to know.
                }

                string[] domains = new string[ts.dimensions + timeDimension];
                for (int i = 0; i < domains.Length; i++) domains[i] = "*";
                if (timeDimension == 1) domains[domains.Length - 1] = Program.options.gams_time_set;  //we alway put the t domain last

                GAMSVariable gvar = db.AddVariable(nameWithoutFreq, VarType.Free, label, domains);

                counterVariables = WriteGdxHelperSlow(t1, t2, usePrefix, counterVariables, ts, gvar);

            }

            db.Export(pathAndFilename);

            G.Writeln2("Exported " + counterVariables + " variables to " + pathAndFilename + " (" + G.SecondsFormat((DateTime.Now - t00).TotalMilliseconds) + ")");
            if (timelessCounter > 0) new Note(timelessCounter + " timeless timeseries skipped");
        }

        private static void WriteGdxHelper2(GekkoTime t1, GekkoTime t2, bool usePrefix, gdxcs gdx, Series ts2, string[] ss, double[] gdxValues)
        {

            if (ts2.type == ESeriesType.Timeless)
            {
                try
                {
                    gdxValues[gamsglobals.val_level] = ts2.GetTimelessData();
                    gdx.gdxDataWriteStr(ss, gdxValues);
                    //gvar.AddRecord(ss).Level = ts2.GetTimelessData();  //timeless data location
                }
                catch
                {

                }
            }
            else
            {
                GekkoTime gt1 = t1;
                GekkoTime gt2 = t2;
                if (t1.IsNull())
                {
                    gt1 = ts2.GetRealDataPeriodFirst();
                    gt2 = ts2.GetRealDataPeriodLast();
                }
                if (gt1.IsNull())
                {
                    //do not write a weird record if the timeseries has no data
                }
                else
                {
                    string[] ss2 = new string[ss.Length + 1];
                    foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                    {
                        Array.Copy(ss, 0, ss2, 0, ss.Length);
                        string date = null;
                        if (usePrefix && t.freq == EFreq.A)
                        {
                            date = Program.options.gams_time_prefix + (t.super - (int)Program.options.gams_time_offset).ToString();
                        }
                        else
                        {
                            date = t.ToString();
                        }
                        ss2[ss2.Length - 1] = date;

                        gdxValues[gamsglobals.val_level] = ts2.GetDataSimple(t);
                        gdx.gdxDataWriteStr(ss2, gdxValues);

                        //gdx.gdxDataWriteRaw()  --> more efficient, see https://www.gams.com/~bussieck/LohBusWesReb.pdf, but then we need to maintain an UEL (each label has a number).
                        //and it seems that gdxDataWriteRaw() recuires lexical ordering of the array of indices??

                    }
                }
            }

            return;
        }

        private static int WriteGdxHelperSlow(GekkoTime t1, GekkoTime t2, bool usePrefix, int counterVariables, Series ts, GAMSVariable gvar)
        {

            if (ts.type == ESeriesType.ArraySuper)
            {
                foreach (KeyValuePair<MultidimItem, IVariable> kvp in ts.dimensionsStorage.storage)
                {
                    string[] ss = kvp.Key.storage;
                    WriteGdxHelperSlow2(t1, t2, usePrefix, gvar, kvp.Value as Series, ss);
                }
            }

            else
            {
                //normal timeseries
                WriteGdxHelperSlow2(t1, t2, usePrefix, gvar, ts, new string[0]);
            }
            counterVariables++;
            return counterVariables;
        }

        private static void WriteGdxHelperSlow2(GekkoTime t1, GekkoTime t2, bool usePrefix, GAMSVariable gvar, Series ts2, string[] ss)
        {
            if (ts2.type == ESeriesType.Timeless)
            {
                try
                {
                    gvar.AddRecord(ss).Level = ts2.GetTimelessData();  //timeless data location
                }
                catch
                {

                }
            }
            else
            {
                GekkoTime gt1 = t1;
                GekkoTime gt2 = t2;
                if (t1.IsNull())
                {
                    gt1 = ts2.GetRealDataPeriodFirst();
                    gt2 = ts2.GetRealDataPeriodLast();
                }
                if (gt1.IsNull())
                {
                    //do not write a weird record if the timeseries has no data
                }
                else
                {
                    foreach (GekkoTime t in new GekkoTimeIterator(gt1, gt2))
                    {
                        string[] ss2 = new string[ss.Length + 1];
                        Array.Copy(ss, 0, ss2, 0, ss.Length);
                        string date = null;
                        if (usePrefix && t.freq == EFreq.A)
                        {
                            date = Program.options.gams_time_prefix + (t.super - (int)Program.options.gams_time_offset).ToString();
                        }
                        else
                        {
                            date = t.ToString();
                        }
                        ss2[ss2.Length - 1] = date;

                        gvar.AddRecord(ss2).Level = ts2.GetDataSimple(t);

                    }
                }
            }

            return;
        }

        private static bool CompareDims(List<string> oldDims, List<string> dims)
        {
            //no test if they are null
            if (dims.Count != oldDims.Count) return false;
            for (int i = 0; i < dims.Count; i++)
            {
                if (!G.Equal(dims[i], oldDims[i])) return false;
            }
            return true;
        }


        private static bool GamsIsFixed(double[] values, double value)
        {
            return value == values[gamsglobals.val_lower] || value == values[gamsglobals.val_upper];
        }

        private static void GdxErrorMessage()
        {
            using (Note n = new Note())
            {
                n.MainAdd("You may manually indicate the GAMS program folder with 'OPTION gams exe folder',");
                n.MainAdd("for instance 'OPTION gams exe folder = c:\\GAMS\\win32\\24.8;'. In general, the");
                n.MainAdd("GAMS component is pretty good at auto-detecting the location of GAMS on the pc,");
                n.MainAdd("including finding a 32-bit GAMS if 32-bit Gekko is used, and a 64-bit GAMS if 64-bit");
                n.MainAdd("Gekko is used. It is probably not possible to use a 32-bit GAMS from a 64-bit Gekko,");
                n.MainAdd("but the inverse may be possible. In general, consider the bitness of both GAMS and");
                n.MainAdd("Gekko. Newer GAMS versions are 64-bit only, and in general, using Gekko 64-bit is");
                n.MainAdd("advised, too.");
                n.MainAdd("Bitness info: " + Program.Get64Bitness(0) + ".");
            }
        }

        private static void GetGAMSWorkspace(ref string gamsDir, ref GAMSWorkspace ws)
        {
            gamsDir = Program.options.gams_exe_folder.Trim();
            if (gamsDir.EndsWith("\\")) gamsDir = gamsDir.Substring(0, gamsDir.Length - "\\".Length);
            if (gamsDir.Trim() == "") gamsDir = null;  //must be so and not an empty string in the GAMSWorkspace call later on
            if (Program.options.gams_fast && gamsDir != null)
            {
                //do nothing
            }
            else
            {
                try
                {
                    if (Globals.gamsWorkspace == null || Globals.gamsWorkspaceHelper != gamsDir)
                    {
                        ws = new GAMSWorkspace(workingDirectory: Program.options.folder_working, systemDirectory: gamsDir);
                        Globals.gamsWorkspace = ws;
                        Globals.gamsWorkspaceHelper = gamsDir;  //record the param it was called with
                    }
                    else ws = Globals.gamsWorkspace;
                    gamsDir = ws.SystemDirectory;
                }
                catch (Exception e)
                {
                    using (Error err = new Error())
                    {
                        err.MainAdd("*** ERROR: Import of gdx file (GAMS) failed. Could not locate GAMS (GAMSWorkspace problem).");
                        err.MainNewLineTight();
                        err.MainAdd("Technical error:");
                        err.MainNewLineTight();
                        err.MainAdd(e.Message);
                        err.ThrowNoException();
                    }
                    GdxErrorMessage();
                    throw;
                }
            }
        }

        /// <summary>
        /// Tries to find the dimension number of a possible time index. Will try to do it fast with int[] domainSyNrs, else it reverts to a
        /// string compare on a string[] domainStrings. It seems that if the set t (or other name) is actually present in the gdx
        /// this runs faster. If not, we probably revert to string matching "t" of domain names.
        /// </summary>
        /// <param name="domainSyNrs"></param>
        /// <param name="domainStrings"></param>
        /// <param name="dimensions"></param>
        /// <param name="gdx"></param>
        /// <param name="timeIndex"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int GdxGetTimeDimNumber(ref int[] domainSyNrs, string[] domainStrings, int dimensions, gdxcs gdx, int timeIndex, int i)
        {
            int timeDimNr = -12345;
            gdx.gdxSymbolGetDomain(i, ref domainSyNrs);
            //only way to check it properly:
            int success = 1;
            for (int d2 = 0; d2 < dimensions; d2++)
            {
                if (domainSyNrs[d2] == 0)
                {
                    success = 0;
                    break;
                }
            }

            if (success == 1)
            {
                for (int d2 = dimensions - 1; d2 >= 0; d2--)  //backwards is faster since t is typically there
                {
                    //
                    // Note: this probably demands that the set t (or other name) is actually present in the gdx
                    // file. If not, we probably revert to string matching "t".
                    //
                    if (domainSyNrs[d2] == timeIndex)
                    {
                        timeDimNr = d2;
                        break;
                    }
                }
            }
            else
            {
                //slower, but still not in the innermost loop
                //gdx.gdxSymbolGetDomainX(i, ref domainStrings);
                for (int d2 = dimensions - 1; d2 >= 0; d2--)  //backwards is faster since t is typically there
                {
                    if (G.Equal(domainStrings[d2], Program.options.gams_time_set))
                    {
                        timeDimNr = d2;
                        break;
                    }
                }
            }

            return timeDimNr;
        }
    }

    public class ASTNodeGAMS
    {
        /// <summary>
        /// See comments for very similar and more complicated ASTNode class for .gcm file reading.
        /// </summary>
        /// <returns></returns>

        private List<ASTNodeGAMS> children = null; //private so that the implementation might change (for instance LinkedList etc.)
        public Parser.Gek.GekkoSB Code = new Parser.Gek.GekkoSB(); //the C# code produced while walking the tree
        public Parser.Gek.GekkoSB Gekko = new Parser.Gek.GekkoSB(); //the Gekko code produced while walking the tree
        public Parser.Gek.GekkoSB GAMS = new Parser.Gek.GekkoSB(); //the unfolded GAMS code produced while walking the tree
        public ASTNodeGAMS Parent = null;
        public string Text = null;  //ANTLR decoration of the node (for instance 'ASTPRT' or '1.45').
        public int Line = 0;
        public int Number = 0;  //used to check position among siblings
        public string leftBlanks = null;

        public IEnumerable ChildrenIterator()
        {
            if (this.children != null)
            {
                foreach (ASTNodeGAMS child in this.children)
                {
                    yield return child;
                }
            }
        }

        public void RemoveLast()
        {
            this.children.RemoveAt(this.children.Count - 1);
        }

        public ASTNodeGAMS GetChild(string s)
        {
            foreach (ASTNodeGAMS child in this.ChildrenIterator())
            {
                if (child.Text == s) return child;
            }
            return null;
        }

        public ASTNodeGAMS this[int i]
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
            ASTNodeGAMS child = this.GetChild(i);
            if (child == null)
            {
                Parser.Gek.GekkoSB xx = new Parser.Gek.GekkoSB();
                return xx;
            }
            else return child.Code;
        }

        //Prepares an AST node to have children
        public void CreateChildren(int n)
        {
            this.children = new List<ASTNodeGAMS>(n);
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

        //Sets the text of the AST node
        public ASTNodeGAMS(string text)
        {
            this.Text = text;
        }

        //Sets the text of the AST node
        public ASTNodeGAMS(string text, string leftBlanks)
        {
            this.Text = text;
            this.leftBlanks = leftBlanks;
        }

        //Sets the text of the AST node, and augments with children.
        public ASTNodeGAMS(string text, bool withChildren)
        {
            this.Text = text;
            if (withChildren)
            {
                this.children = new List<ASTNodeGAMS>();
            }
        }

        public ASTNodeGAMS GetChild(int i)
        {
            if (this.children == null) return null;
            if (i >= this.children.Count) return null;  //does not exist
            return this.children[i];
        }

        public void Add(ASTNodeGAMS child)
        {
            this.children.Add(child);
            child.Parent = this;
            child.Number = children.Count - 1;
        }

        public string ToString()
        {
            return this.Text;
        }

        public void PrintAST2(ASTNodeGAMS node, int depth)
        {
            G.Writeln(G.Blanks(depth * 2) + node.Text);
            if (node.children != null)
            {
                for (int i = 0; i < node.children.Count; ++i)
                {
                    ASTNodeGAMS child = (ASTNodeGAMS)(node.children[i]);
                    PrintAST2(child, depth + 1);
                }
            }
        }
    }

    public class EqLineHelper
    {
        public int count = 0;
        public int known = 0;
        public int unique = 0;

        public string[] dict_FromANumberToVarName = null;
        public GekkoDictionaryDimensional dict_FromVarNameToANumber = new GekkoDictionaryDimensional();

        public GekkoDictionary<string, int> dict_Constants = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public double[][] a = null;
        public List<List<int>> b = new List<List<int>>();
        public List<double> c = new List<double>();
        public List<List<int>> d = new List<List<int>>();
        public List<int> eqPointers = new List<int>();        

        public string[] dict_FromEqNumberToEqName = null;
        public GekkoDictionaryDimensional dict_FromEqNameToEqNumber = new GekkoDictionaryDimensional();
        public string[] dict_FromVarNumberToVarName = null;
        public GekkoDictionaryDimensional dict_FromVarNameToVarNumber = new GekkoDictionaryDimensional();
        public string[] dict_FromEqChunkNumberToEqName = null;
        public GekkoDictionaryDimensional dict_FromEqNameToEqChunkNumber = new GekkoDictionaryDimensional();
        public int[] dict_FromEqNumberToEqChunkNumber = null;

        //public List<List<PeriodAndVariable>> precedentsScalar = new List<List<PeriodAndVariable>>();

        public GekkoTime tBasis = GekkoTime.tNull;
        public GekkoTime t1 = GekkoTime.tNull;
        public GekkoTime t2 = GekkoTime.tNull;
        public GekkoTime t3 = GekkoTime.tNull;

        // ================================ fields below are cleared for each new equation ==========

        public List<int> endo = new List<int>();  //comes in pairs (time, variable)
        public List<int> exo = new List<int>();
        public List<double> exoValues = new List<double>();
        public StringBuilder sb = new StringBuilder(); //contains the C# code
        public Dictionary<int, string> addBefore = new Dictionary<int, string>();  //inefficient?
        public Dictionary<int, string> remove = new Dictionary<int, string>();     //inefficient?        

        public void Clear()
        {
            //just so that one of the above is not forgotten
            this.endo = new List<int>();         //this.endo.Clear(); --> will fail
            this.exo = new List<int>();          //this.exo.Clear(); --> will fail
            this.exoValues = new List<double>(); //this.exoValues.Clear(); --> will fail
            this.sb.Clear();
            this.addBefore.Clear();
            this.remove.Clear();
        }
    }    

    /// <summary>
    /// Info on which gradients have been already computed, for which periods, and for which databanks.
    /// This is to avoid doing too much work when calling DECOMP several times.
    /// </summary>
    public class Data
    {
        public EDecompBanks type = EDecompBanks.Multiplier;
        public Series dataCellsGradQuo = null;
        public Series dataCellsGradRef = null;
    }

    //public class GamsTestInput
    //{
    //    public bool testForZeroResiduals = false;
    //    public string file = null;
    //    public string file2 = null;
    //    public GekkoTime time0 = GekkoTime.tNull;
    //    public int rep1 = 1;
    //    public int rep2 = 1;
    //}

    //public class GamsTestOutput
    //{
    //    public int count;
    //    public int known;
    //    public int unique;
    //    public double rss;  //sqrt
    //    public double[] r;
    //}
}

