using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using System.Collections;
using GAMS;
using System.Xml;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.Reflection;
using ProtoBuf;
using System.Windows.Forms;

namespace Gekko
{
    public enum EEquationsOrVariables 
    {
        None,
        Equations,
        Variables
    }

    public enum EEquationCountsOrVariableCounts 
    {
        None,
        EquationCounts,
        VariableCounts
    }

    public enum EModelEquationsOrVariables
    {
        None,
        Equations,
        Variables,
        Done
    }

    public enum EExtractTimeDimension
    {
        Full,
        NoIndexListOfStrings
    }

    [ProtoContract]
    public struct GamsWalkerInfo  //A struct is easier so that one child node does not affect its parent node
    {
        [ProtoMember(1)]
        public bool isInsideSum;
        [ProtoMember(2)]
        public bool isInsideDollar;        
        //public GamsWalkerInfo(bool isInsideSum, bool isInsideDollar)
        //{            
        //    this.isInsideSum = isInsideSum;
        //    this.isInsideDollar = isInsideDollar;
        //}
    }

    public class ExtractTimeDimensionHelper
    {
        //public string name = null;
        //public GekkoTime time = GekkoTime.tNull;
        public DName resultingFullName = null;
        //public List<string> indexes = null;
    }

    public class EquationLhsPoints
    {
        public string eqname = null;
        public double points = double.NaN;
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

            bool hasErrors = false;

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
                        if (eqsCs[i] == "")
                        {
                        }
                        else
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
                    cr = Globals.iCodeCompiler.CompileAssemblyFromSource(compilerParams, s2);
                    if (cr.Errors.HasErrors)
                    {
                        hasErrors = true;
                    }
                    else
                    {
                        Assembly assembly = cr.CompiledAssembly;
                        DateTime dt2 = DateTime.Now;
                        Object[] o = new Object[1] { functions };
                        assembly.GetType("Gekko.Equations").InvokeMember("Residuals", BindingFlags.InvokeMethod, null, null, o);  //the method                                                                                                                                                  
                    }
                }
                return 0;
            }, _ => { });

            if (hasErrors)
            {
                new Error("The GAMS scalar model could not be translated into equivalent Gekko code. This may be due to functions or math operators in the GAMS scalar model that are not properly translated into Gekko code. You may want to check your GAMS model regarding the use of unusual/rare functions or math operators/constructs.");
            }

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

        /// <summary>
        /// Read a scalar model. For each model line, it calls HandleEqLine().
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static Model ReadGamsScalarModelEquations(GAMSScalarModelSettings settings, Model model)
        {
            //for c:\Thomas\Gekko\regres\DREAM\MAKRO\2022-01-26-yyyyyyy\klon\Model\gams.gms and
            //    c:\Thomas\Gekko\regres\DREAM\MAKRO\2022-01-26-yyyyyyy\klon\Model\dict.txt
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
            
            int eqCounts2 = -12345;
            int varCounts2 = -12345;
            int fakeEqCounts2 = 0;
            int fakeVarCounts2 = 0;
            bool hasResVariables = false;  //if there are any res_... variables in model
            Dictionary<int, int> timeless = new Dictionary<int, int>();  //records timeless vars for later use in .isTimeless array.

            //read dictionary                        
            if (settings.scalarMemoryModelProducedByGekko)
            {
                StreamReader sr = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Stringlist.ExtractTextFromLines(settings.dictionary).ToString())));
                ReadScalarModelEquationsDictionaryLines(settings.t1, settings.t2, helper, split2, timeless, model.modelCommon.GetRealFreq(), ref hasResVariables, ref eqCounts2, ref varCounts2, ref fakeEqCounts2, ref fakeVarCounts2, sr);
            }
            else
            {
                using (FileStream fs = Program.WaitForFileStream(settings.ffh_unrolledNames.realPathAndFileName, settings.ffh_unrolledNames.prettyPathAndFileName, Program.GekkoFileReadOrWrite.Read))
                using (TextReader sr = new StreamReader(fs))
                {
                    ReadScalarModelEquationsDictionaryLines(settings.t1, settings.t2, helper, split2, timeless, model.modelCommon.GetRealFreq(), ref hasResVariables, ref eqCounts2, ref varCounts2, ref fakeEqCounts2, ref fakeVarCounts2, sr);
                }
            }

            helper.isTimeless = new bool[helper.dict_FromVarNameToANumber.Count()];
            foreach (int i in timeless.Keys)
            {
                helper.isTimeless[i] = true;
            }

            helper.dict_FromANumberToVarName = new DName[helper.dict_FromVarNameToANumber.Count()];
            foreach (KeyValuePair<DName, int> kvp in helper.dict_FromVarNameToANumber)
            {
                helper.dict_FromANumberToVarName[kvp.Value] = kvp.Key;
            }

            helper.dict_FromEqChunkNumberToEqName = new DName[helper.dict_FromEqNameToEqChunkNumber.Count()];
            foreach (KeyValuePair<DName, int> kvp in helper.dict_FromEqNameToEqChunkNumber)
            {
                helper.dict_FromEqChunkNumberToEqName[kvp.Value] = kvp.Key;
            }

            if (Globals.runningOnTTComputer) new Writeln("TTH: Import dictionary finished: " + G.Seconds(dt1));
            dt1 = DateTime.Now;

            TokenList tokensLast = null;

            List<string> eqs = new List<string>();      //1
            List<string> values = new List<string>();   //2
            List<string> end = new List<string>();      //3            
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
                //No need to taste: time is *always* last dimension
                StreamReader sr = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(Stringlist.ExtractTextFromLines(settings.equations).ToString())));
                ReadGamsScalarModelEquationsLines(helper, split2, ref tokensLast, values, end, ref eqCounts, ref varCounts, ref semis, csCodeLines, ref eqLine, sr);
            }
            else
            {                
                using (FileStream fs = Program.WaitForFileStream(settings.ffh_unrolledModel.realPathAndFileName, settings.ffh_unrolledModel.prettyPathAndFileName, Program.GekkoFileReadOrWrite.Read))
                using (StreamReader sr = new StreamReader(fs))
                {
                    ReadGamsScalarModelEquationsLines(helper, split2, ref tokensLast, values, end, ref eqCounts, ref varCounts, ref semis, csCodeLines, ref eqLine, sr);
                }
            }
            
            if (Globals.runningOnTTComputer) new Writeln("TTH: GAMS equations read: " + G.Seconds(dt1) + "   -->   " + "count " + helper.count + " unique " + helper.unique);
            dt1 = DateTime.Now;

            //new Writeln("Count " + helper.count + " hits " + helper.known + " unique " + helper.unique + " semis " + semis);
            if (helper.count != helper.known + helper.unique) new Error("Not summing up");
            if (helper.count != semis) new Error("Not summing up");

            if (true) //#sss87uakjdsfs
            {
                DateTime dt00 = DateTime.Now;
                //Get fixed variables
                
                helper.fix = new byte[periods][];
                for (int i = 0; i < helper.fix.Length; i++)
                {
                    helper.fix[i] = new byte[helper.dict_FromVarNameToANumber.Count()]; //beware: 0-based
                }

                foreach (string line in values)
                {
                    if (line.StartsWith("*")) continue;                    
                    int iFix = line.IndexOf(".fx");
                    if (iFix > -1)
                    {
                        string sFix = G.Substring(line, 1, iFix - 1);                        
                        int id = -12345;
                        try
                        {
                            id = int.Parse(sFix) - 1;  //0-based
                        }
                        catch
                        {
                            new Error("Could not parse integer part of the string '" + line + "'");
                        }

                        DName inputName = helper.dict_FromVarNumberToVarName[id];
                        
                        //qwerty remove time?
                        int aNumber; if (!helper.dict_FromVarNameToANumber.TryGetValue(inputName.RemoveTime(), out aNumber)) aNumber = -12345;
                        if (aNumber == -12345)
                        {
                            if (Globals.greuHack) continue;
                            new Error("When reading fixed variable, could not find name '" + inputName + "' in dictionary");
                        }
                        int i1 = -12345;
                        int i2 = aNumber;
                        if (inputName.GetTime().IsNull())  //reading .fx values
                        {
                            i1 = 0;
                        }
                        else
                        {
                            i1 = inputName.GetTime().Subtract(helper.tBasis);
                        }
                        try
                        {
                            helper.fix[i1][i2] = 1;  //Is 0 or 1
                        }
                        catch
                        {
                            if (Globals.greuHack) continue;
                            new Error("Index out of range when finding fixed GAMS variable");
                        }
                    }                    
                }
                if (Globals.runningOnTTComputer) new Writeln("TTH: -sub- Finding fixed vars: " + G.Seconds(dt00));
            }
            
            int hasReadSomeData = 0;

            if (Program.options.model_gams_scalar_data)
            {
                DateTime dt01 = DateTime.Now;
                if (Globals.runningOnTTComputer) MessageBox.Show("Beware: read scalar model data");
                //Read data from the scalar model (gams.gms)
                DateTime dt00 = DateTime.Now;
                foreach (string line in values)
                {                    
                    if(G.NullOrBlanks(line)) continue;
                    if (line.StartsWith("*")) continue;
                    if (!(G.Contains(line, ".l=") || G.Contains(line, ".l =") || G.Contains(line, ".fx=") || G.Contains(line, ".fx ="))) continue; //We assume this pattern. There may be .m and others.
                    //Not efficient
                    //Not efficient
                    //Not efficient
                    string[] ss = line.Split(split, StringSplitOptions.None);
                    int id = -12345;
                    try
                    {
                        id = int.Parse(ss[0].Substring(1)) - 1;  //0-based
                    }
                    catch
                    {
                        new Error("Could not parse integer part of the string '" + ss[0] + "'");
                    }

                    DName inputName = helper.dict_FromVarNumberToVarName[id];

                    //qwerty remove time (?)
                    DName inputNameWithoutTime = inputName.RemoveTime();
                    int aNumber; if (!helper.dict_FromVarNameToANumber.TryGetValue(inputNameWithoutTime, out aNumber)) aNumber = -12345;
                    if (aNumber == -12345)
                    {
                        new Error("When reading equation, could not find name '" + inputNameWithoutTime + "' in dictionary");
                    }
                    int i1 = -12345;
                    if (inputName.GetTime().IsNull()) //reading timeless data (not activated)
                    {
                        //TODO TODO TODO
                        //TODO TODO TODO what to do about these, if read from .fx lines
                        //TODO TODO TODO
                        i1 = 0;

                        //bool b = false; if (timeless.ContainsKey(aNumber)) b = true;
                        //bool b1 = helper.isTimeless[aNumber - 1];
                        //bool b2 = helper.isTimeless[aNumber];
                        //bool b3 = helper.isTimeless[aNumber + 1];
                    }
                    else
                    {
                        i1 = inputName.GetTime().Subtract(helper.tBasis);
                    }
                    int i2 = aNumber;
                    double d;
                    string toParse = "";
                    if (ss[1].Trim() == "")
                    {
                        //probably always so
                        toParse = ss[2].Trim();
                    }
                    else
                    {
                        toParse = ss[1].Trim();
                    }
                    if (G.Equal(toParse, "eps"))
                    {
                        d = 0d;
                    }
                    else
                    {
                        try
                        {
                            d = double.Parse(toParse);
                        }
                        catch
                        {
                            new Error("Could not parse the number '" + toParse + "' as a floating-point value.");
                            throw;
                        }
                    }
                    try
                    {
                        helper.a[i1][i2] = d;
                    }
                    catch
                    {
                        new Error("Index out of range when reading GAMS scalar equation");
                    }
                    hasReadSomeData++;
                }
                if (Globals.runningOnTTComputer) new Writeln("TTH: -sub- Read variable data: " + G.Seconds(dt01));
            }

            if (Globals.runningOnTTComputer) new Writeln("TTH: GAMS total data reading " + hasReadSomeData + " obs, " + G.Seconds(dt1));
            dt1 = DateTime.Now;

            //new Writeln("eqCounts = " + eqCounts + ", varCounts = " + varCounts + ", eqCounts2 = " + eqCounts2 + ", varCounts2 = " + varCounts2);
            //if (eqCounts != varCounts) new Writeln("ERROR: counts do not match.");
            //if (eqCounts2 != varCounts2) new Writeln("ERROR: counts do not match.");
            //if (eqCounts != eqCounts2) new Writeln("ERROR: counts do not match.");

            double[] r = G.CreateNaN(eqCounts2);
            Func<int, double[], double[][], double[], int[][], int[][], int, double>[] functions = new Func<int, double[], double[][], double[], int[][], int[][], int, double>[helper.unique];
            double[][] a = helper.a;
            byte[][] fix = helper.fix;
            int[][] bb = helper.b.Select(x => x.ToArray()).ToArray();
            double[] cc = helper.c.ToArray();
            int[][] dd = helper.d.Select(x => x.ToArray()).ToArray();
            int[] ee = helper.eqPointers.ToArray();
                        
            Compile5(csCodeLines, functions);
            
            if (Globals.runningOnTTComputer) new Writeln("TTH: Data preparation finished: " + G.Seconds(dt1));
            dt1 = DateTime.Now;

            //The method below handles ANSI, but labels are not fetched here yet.        

            ModelGams modelGams = null;
            if (!settings.scalarMemoryModelProducedByGekko)
            {
                if (settings.ffh_rawModel.realPathAndFileName != null) //if raw.gms does not exist, this is skipped
                {
                    string text = Program.GetTextFromFileWithWait(settings.ffh_rawModel.realPathAndFileName);
                    List<string> gamsFoldedModel = Stringlist.ExtractLinesFromText(text);
                    IVariable nestedListOfDependents_opt_dep = null;
                    Tuple<Dictionary<DName, DName>, StringBuilder> tup = GamsModel.GetDependentsGams(nestedListOfDependents_opt_dep);
                    Dictionary<DName, DName> dependents = tup.Item1;
                    modelGams = GamsModel.ReadGamsModelHelper(false, Stringlist.ExtractTextFromLines(gamsFoldedModel).ToString(), null, dependents, false, true, model);                    
                    modelGams.rawGmsFile = text;

                    //Model m = Program.model;

                }
            }

            if (Globals.runningOnTTComputer) new Writeln("TTH: Get folded model: " + G.Seconds(dt1));
            dt1 = DateTime.Now;

            ModelGamsScalar modelGamsScalar = new ModelGamsScalar(model);

            modelGamsScalar.t1 = settings.t1; //Local period from MODEL<%t1 %t2>...
            modelGamsScalar.t2 = settings.t2; //Local period from MODEL<%t1 %t2>...

            // -------------- these can evaluate an equation --------
            modelGamsScalar.functions = functions;
            modelGamsScalar.a = a;
            modelGamsScalar.fix = fix;

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
            modelGamsScalar.fakeEqCounts = fakeEqCounts2;
            modelGamsScalar.fakeVarCounts = fakeVarCounts2;

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
            // --------------
            modelGamsScalar.isTimeless = helper.isTimeless;  //the a-vars that are timeless
            
            CalculatePrecedentsAndDependents(modelGamsScalar, modelGamsScalar.CountEqs(1));

            if (Globals.runningOnTTComputer) new Writeln("TTH: Precedents/dependents: " + G.Seconds(dt1));
            dt1 = DateTime.Now;                                                   

            modelGamsScalar.hasResVariables = hasResVariables;
            modelGamsScalar.hasReadSomeData = hasReadSomeData;

            if (Program.options.model_gams_scalar_data && modelGamsScalar.hasReadSomeData > 0)  //don't do if no data was found in scalar model
            {
                //modelGamsScalar.a = helper.a; --> not necessary, is already so.
                DateTime dt00 = DateTime.Now;
                if (Globals.runningOnTTComputer) MessageBox.Show("Beware: scalar model data handled A");
                modelGamsScalar.FromAToDatabankScalarModel(Program.databanks.GetFirst(), false);
            }

            if (Globals.runningOnTTComputer) new Writeln("TTH: From A to Databank: : " + G.Seconds(dt1));
            dt1 = DateTime.Now;

            if (true) // !model.modelGamsScalar.hasResVariables)
            {
                //Doesn't take much time, and can act as fallback even if res_... vars are present
                try
                {
                    model.modelGamsScalar.depNames = GamsModel.DepNames(model);  //Finding out which variables are dependent, from eq naming conventions.                    
                }
                catch
                {
                    //No need to choke on this
                    new Note("The module that identifies dependent variables from equation names failed to load");
                }
            }
            if (Globals.runningOnTTComputer) new Writeln("TTH: DepNames() took: " + G.Seconds(dt1) + " with " + model.modelGamsScalar.depNames.Count + " items");
            dt1 = DateTime.Now;

            new Writeln("TTH: ====> Setting up everything took: " + G.Seconds(dt0) + ", all included");

            return model;
        }

        /// <summary>
        /// From a model (modelGamsScalar primarily) and variableName (and time), the equations that the variable appears in
        /// are found, and the "best" one is shown first (the one that has "res_[variableName]"). You may set tHere = GekkoTime.tNull,
        /// in which case the method uses t-1, where t is the end period of the scalar model. The method uses .modelGamsScalar.dependents,
        /// which returns a list of eq numbers. For instance, with intput qBNP, the method returns E_qBNP, E_pBNP, E_qBNP_via_rpBNP, where
        /// E_qBNP is shown first because the equation contains the variable res_qBNP. The rest of the eqs are alphabetically sorted.
        /// </summary>     
        /// <returns></returns>
        public static List<EqInfoSimple> GetSortedEquations(DName variableName, GekkoTime tHere, Model model, bool onlySortFirstItem, bool abortIfError, bool noBlanksEtc)
        {
            //            x1     x2     x3     x4    res_x1   res_x2   res_x3   res_x4
            // --------------------------------------------------------------------------
            //   e1       +      x      x              x
            //   e2       x      +                              x
            //   e3              x      +                                x
            //   e4       x             x      +                                   x
            // --------------------------------------------------------------------------
            //
            // For at given variable, say x1, we look vertically at its equations, say e1, e2 and e4 to see which one is best.
            // They get scored with ScoreEquationGivenVariable(), from 0.5 to 100+ (the '+' are best).
            // An ordered list of the equations is returned.
            //
            // In DISP x1, we know that x1 is in e2 and e4 too, but would like these to appear as x2 and x4. To do this
            // we loop over the e2 variables to find the one that has max score.
            //
            ModelGamsScalar modelGamsScalar = model.modelGamsScalar;
            ModelGams modelGams = model.modelGams;
            List<EqInfoSimple> rv = new List<EqInfoSimple>();

            if (tHere.IsNull()) tHere = modelGamsScalar.Maybe2000GekkoTime(modelGamsScalar.GetDecompT());

            int aNumber; if (!modelGamsScalar.dict_FromVarNameToANumber.TryGetValue(variableName, out aNumber)) aNumber = -12345;
            if (aNumber == -12345)
            {
                return rv;
            }            

            int timeIndex = modelGamsScalar.FromGekkoTimeToTimeInteger(tHere);
            long pav = ModelGamsScalar.PackPeriodAndVariable(timeIndex, aNumber);
            List<int> eqNumbers = null;            
            if (modelGamsScalar.dependents != null) modelGamsScalar.dependents.TryGetValue(pav, out eqNumbers);
            
            if (eqNumbers == null)
            {
                //G.WarningInternal("Eq browser: '" + variableName + "' returns 'null' for eqNumbers"); --> seems ok, occurs for exogenous vars it seems.
                eqNumbers = new List<int>();
            }

            foreach (int eqNumber in eqNumbers)
            {
                EqInfoSimple eqInfo = new EqInfoSimple();
                eqInfo.eqName = model.modelGamsScalar.GetEqName(eqNumber);
                eqInfo.eqNameWithLag = model.modelGamsScalar.GetEqName(eqNumber).ConvertToLag(tHere); // G.Chop_DimensionConvertToLag(eqInfo.eqName, tHere, false, false, ""); //We preserve Gekko style for eqs, because the string is used when clicking
                eqInfo.eqNumber = eqNumber;
                ScoreEquationGivenVariable(eqInfo, variableName, model, modelGams, modelGamsScalar);
                rv.Add(eqInfo);
            }

            if (rv.Count == 0)
            {
                if (model.modelCommon.GetModelSourceType() == EModelType.Gekko)
                {
                    //Some variable has an "e_" prefixed, but the equation may not exist if it is an exogenous variable.
                    return new List<EqInfoSimple>();
                }
                else
                {
                    if (abortIfError)
                    {
                        string s = ". You may want to adjust the DECOMP time period.";
                        bool b = false; try { b = modelGamsScalar.isTimeless[ModelGamsScalar.UnpackVariable(pav)]; } catch { }
                        if (b) s = ". Note that the variable " + variableName + " is timeless (without time dimension): it may therefore not make sense to try to decompose it.";
                        new Error("Could not find " + variableName + "[" + modelGamsScalar.FromTimeIntegerToGekkoTime(ModelGamsScalar.UnpackPeriod(pav)).ToString() + "] as an endogenous variable. " + modelGamsScalar.GamsModelDefinedString() + s);
                    }
                    else return new List<EqInfoSimple>();  //Flowgraph just ignores the problem
                }
            }

            List<EqInfoSimple> eqsNewA = null;
            if (onlySortFirstItem)
            {
                //Flowgraph at deeper depths
                for (int i = 0; i < rv.Count; i++)
                {
                    if (rv[i].score >= Globals.lhsScore2)
                    {
                        EqInfoSimple temp = rv[0];
                        rv[0] = rv[i];
                        rv[i] = temp;
                        break;  //No need to do further work: only rv[0] is ever used.
                    }
                }
                eqsNewA = rv;
            }
            else
            {
                eqsNewA = rv.OrderByDescending(x => x.score).ThenBy(x => x.eqNameWithLag.ToString(), new G.NaturalComparer(G.NaturalComparerOptions.Default)).ToList();
            }

            return eqsNewA;
        }

        public static void ScoreEquationGivenVariable(EqInfoSimple eqInfo, DName variableName, Model model, ModelGams modelGams, ModelGamsScalar modelGamsScalar)
        {
            if (modelGamsScalar.isPerpetualModel)
            {
                if (G.Equal(Globals.decompGekkoEquationPrefix + variableName.ToString(), eqInfo.eqName.GetName()))
                {
                    eqInfo.score += Globals.lhsScore1 + Globals.lhsScore2;
                }
            }
            else
            {
                List<DName> lhsVars = Program.BeforeEqualSign(new DNameSimplest(eqInfo.eqName.GetName()), modelGams);
                bool hit2 = false;
                foreach (DName s in lhsVars)
                {
                    if (G.Equal(new DNameSimplest(variableName.GetName()), s)) { hit2 = true; break; }
                }
                if (hit2) eqInfo.score += Globals.lhsScore1; //0.5                    

                double extra = 0d;
                if (modelGamsScalar.hasResVariables)
                {
                    //res_... variables, trying those first, then eq names as backup, depending on option
                    extra = GetSortedEquationsByResVariable(eqInfo.eqNumber, variableName, modelGamsScalar);
                    //Regarding the call below, this does not look whether the var is LHS, this has been done above and will be added later on
                    //It only looks at the equation name and performs some magic. When res_... are present, not need to use that magic.
                    if (Program.options.bugfix_score_even_with_res_vars && extra == 0d) extra = GetSortedEquationsByEqName(eqInfo.eqName, variableName, model, modelGamsScalar);
                }
                else
                {
                    //eq names
                    extra = GetSortedEquationsByEqName(eqInfo.eqName, variableName, model, modelGamsScalar);                    
                }
                eqInfo.score += extra;
            }
        }

        private static double GetSortedEquationsByResVariable(int eqNumber, DName variableName, ModelGamsScalar modelGamsScalar)
        {
            string dep = GetDependentVariable(eqNumber, modelGamsScalar);
            double extra = 0d;
            if (G.EqualHandleBlanks(variableName.ToString(), dep)) extra = Globals.lhsScore3;  //101
            return extra;
        }

        private static double GetSortedEquationsByEqName(DName eqName, DName variableName, Model model, ModelGamsScalar modelGamsScalar)
        {
            if (Globals.greu && model.modelCommon.GetModelSourceType() == EModelType.GAMSScalar)
            {                
                DName eqNameWithoutLast = eqName.RemoveLastIndex();                
                List<DName> temp; modelGamsScalar.depNames2Inverted.TryGetValue(variableName, out temp);
                bool hit1 = false;
                if (temp != null)
                {
                    foreach (DName varName in temp)
                    {
                        if (G.Equal(varName, eqNameWithoutLast)) { hit1 = true; break; }
                    }
                }
                double extra = 0d;
                if (hit1) extra = Globals.lhsScore2; //100
                return extra;
            }
            else
            {
                DName eqNameWithoutLast = eqName.RemoveTime();  //Note: what about lagged/leaded equation???

                bool hit1 = false;
                //SLACK SLACK SLACK
                //SLACK SLACK SLACK
                //SLACK SLACK SLACK
                //SLACK SLACK SLACK --> GetDependentEquations() is not so fast because it is not a dict lookup. Will use time for flowgraph. Could make the dict inverted and faster, but we are moving away from eqnames anyway...?
                //SLACK SLACK SLACK
                //SLACK SLACK SLACK
                //SLACK SLACK SLACK
                List<DName> lhsEqs = modelGamsScalar.GetDependentEquations(variableName, model.modelCommon.GetModelSourceType() == EModelType.Gekko);
                foreach (DName s in lhsEqs)
                {
                    if (G.Equal(eqNameWithoutLast, s)) { hit1 = true; break; }
                }
                double extra = 0d;
                if (hit1) extra = Globals.lhsScore2; //100
                return extra;
            }            
        }

        /// <summary>
        /// For an equation number (in a scalar model), the dependent variable name is returned. May return null.
        /// </summary>
        /// <param name="eqNumber"></param>
        /// <param name="modelGamsScalar"></param>
        /// <returns></returns>
        public static string GetDependentVariable(int eqNumber, ModelGamsScalar modelGamsScalar)
        {
            string dep = null;
            //Look for the special "res_..." variable name in the equation variables
            foreach (long dp in modelGamsScalar.precedents[eqNumber].vars)
            {
                //foreach precedent variable
                string varName = modelGamsScalar.GetVarNameA_OLD(ModelGamsScalar.UnpackVariable(dp));
                if (G.StartsWith(varName, Globals.decompResidualPrefix))
                {
                    dep = varName.Substring(Globals.decompResidualPrefix.Length);
                }
            }
            return dep;
        }

        /// <summary>
        /// If for instance n = 7, it returns 0, 7, 1, 6, 2, 5, 3, 4. So it takes the first and last first.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static List<int> GenerateSpiral(int n)
        {
            List<int> result = new List<int>();
            int start = 0;
            int end = n;

            while (start <= end)
            {
                if (start <= end) result.Add(start++);
                if (start <= end) result.Add(end--);
            }

            return result;
        }

        /// <summary>
        /// For a scalar model, finds (with help of equation names) the dependent variable in each equation (abstracting from
        /// the time dimensions). Looks at the scalar equations as they are shown in FIND or DECOMP for an equation, and tries
        /// to match the "e_..." equation name to variables there. Mostly uses model.modelGamsScalar, but also model.modelGams
        /// is used when finding equation text (maybe model.modelGams is not used at all at the moment, but could be in the future
        /// if analyzing the equations gets more advanced).
        /// </summary>
        /// <param name="modelGamsScalar"></param>
        public static Dictionary<DName, DName> DepNames(Model model)
        {

            //Is called with MODEL statement, used for GetSortedEquations() point system.
            //batches is a dict of around 1000 (for MAKRO) elements, where each key is an eq name
            //and each value is a list of sub-eq name + raw gams code + scalar gams code (no time dimension in the list)
            //Example:
            // ----------------------------------------------------------------------------------------------------------------------------
            //E_vUdlAktRenter
            // ----------------------------------------------------------------------------------------------------------------------------
            //eqName:              E_vUdlAktRenter[Obl]
            //eqMathRaw:           vUdlAktRenter[portf, t] = E = (rRente[portf, t] + jrUdlAktRenter[portf, t]) * vUdlAkt[portf, t - 1] / fv;
            //                       over sets: [portf, t], with $-condition: (tx0[t] and d1vudlakt[portf, t] and t.val > 1994)
            //eqMathScalar:        -0.972592347643409 * vUdlAkt[Obl][-1] * (jrUdlAktRenter[Obl] + rRente[Obl]) + vUdlAktRenter[Obl] = 0
            // ----------------------------------------------------------------------------------------------------------------------------

            //lhsEquations is a dict<string, string>, where for each eqName (sub-eq name) a LHS is designated.
            //
            //So for e1: y = c + i + g, e2: c = 0.8 y, we have this dict: (e1 --> y1), (e2 --> y2)
            //
            //       y   c   i   g 
            // ------------------------
            // e1    x   .   .   .
            // e2    .   x
            // ------------------------
            //
            //Here, e1 points to y, e2 points to c. When doing a DISP for y, we will show e1. But we also want to show all equations
            //y appear in, in this case e1 and e2. But we do not want to show this as "influences: e1, e2" but rather
            //"influences: y, c". So y also influences e2, 

            //# m1 = 3 --> sub-equations, first batch
            //# m2 = 3 --> ()-columns in each sub-equation
            //# m3 = 2 --> dimensions
            // e_x1    (tot, m)  (a, m)  (b, m)
            // e_x1    (tot, n)  (a, n)  (b, n)
            // e_x1    (tot, k)  (a, k)  (b, k)
            // e_x2    (a)
            // e_x2    (b)  

            //EQUATION E_qK_spTot[k,t];
            //E_qK_spTot[k, t]$(tx0[t])  ..  pKI[k, spTot, t - 1] / fp * qK[k, spTot, t] = E = sum(sp, pI_s[k, sp, t - 1] / fp * qK[k, sp, t]);
            //sub1: (ib, spTot), (ib, tje), (ib, fre), ... , (ib, udv), 9 elements
            //sub2: (im, spTot), (im, tje), (im, fre), ... , (im, udv), 8 elements
            //Since ib or im do not vary, dim #1 is set as ib or im. For dim #2, first one is spTot and last one is udv. From the name, we choose spTot.
            //

            DateTime t0 = DateTime.Now;

            if (model.modelGamsScalar == null) new Error("No scalar model defined");

            // ------------------------------------------------------------
            bool mayUseDatabank = true;
            bool spelling = true;
            // -------- internal, normally false --------
            bool shouldWrite = false;
            bool createProtobufferFileForUnitTests = false;  //Set it back to false right afterwards!! Perhaps even take copies of the current file before overwriting.
            // ------------------------------------------------------------

            int nAll = 0;
            List<DName> notFoundInModel = new List<DName>();
            List<DName> notFoundInEq = new List<DName>();
            int nFail = 0;

            List<string> writer = new List<string>();
            Dictionary<DName, List<EquationHelper2>> batches = GetScalarEquations(model);

            GekkoDictionary<string, bool> varsNoIndex = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            List<string> varsNoIndex2 = model.modelGamsScalar.GetVars(3);
            foreach (string s in varsNoIndex2) varsNoIndex.Add(s, false);

            Dictionary<DName, DName> lhsEquationsStrings = new Dictionary<DName, DName>(Multidim2Comparer.IgnoreCase);            

            //For each equation name (without indexes)
            foreach (KeyValuePair<DName, List<EquationHelper2>> kvp in batches)
            {
                DName equationNameWithoutIndexes = kvp.Key;

                //For each scalar equation (no time dimension)
                foreach (EquationHelper2 eh in kvp.Value)
                {
                    nAll++;
                    DName equationNameWithIndexes = eh.eqName;
                    if (Globals.runningOnTTComputer && equationNameWithIndexes.GetName().Contains("__"))
                    {
                        //MessageBox.Show("Hovsa3"); //Not possible
                    }

                    DName equationNameWithoutIndexesTemp = equationNameWithoutIndexes;
                    if (spelling)
                    {
                        if (equationNameWithoutIndexesTemp.GetName().Contains("E_vHhTilBorn_aTot")) equationNameWithoutIndexesTemp = new DNameSimplest(equationNameWithoutIndexesTemp.GetName().Replace("E_vHhTilBorn_aTot", "E_vHhTilBoern_aTot"));
                        if (equationNameWithoutIndexesTemp.GetName().Contains("E_rOffTilVirk")) equationNameWithoutIndexesTemp = new DNameSimplest(equationNameWithoutIndexesTemp.GetName().Replace("E_rOffTilVirk", "E_rOffTilVirk2BNP"));
                        if (equationNameWithoutIndexesTemp.GetName().Contains("E_tSubLoen_sTot")) equationNameWithoutIndexesTemp = new DNameSimplest(equationNameWithoutIndexesTemp.GetName().Replace("E_tSubLoen_sTot", "E_vSubLoen_sTot"));
                    }

                    string[] eqNameChunks = equationNameWithoutIndexesTemp.GetName().Split('_');

                    DName lhsName = null;
                    string indexName = null;
                    for (int i = eqNameChunks.Length - 1; i > 0; i--)
                    {
                        string s = null;
                        for (int j = 1; j <= i; j++)
                        {
                            s += eqNameChunks[j] + "_";
                        }
                        s = s.Substring(0, s.Length - "_".Length);
                        if (varsNoIndex.ContainsKey(s))
                        {
                            //Good
                            lhsName = new DNameSimplest(s);
                            if (i + 1 < eqNameChunks.Length)
                            {
                                indexName = eqNameChunks[i + 1];  //TODO: What about > 1 index names???
                            }
                            break;
                        }
                    }

                    if (lhsName == null)
                    {
                        if (equationNameWithoutIndexes.GetName().StartsWith("e_j", StringComparison.OrdinalIgnoreCase))
                        {
                            //ignore
                        }
                        else
                        {
                            notFoundInModel.Add(equationNameWithoutIndexes);
                        }
                        continue; //Variable does not exist at all
                    }

                    //if (eqNameChunks.Length >= 3) indexName = eqNameChunks[2];
                    VariableDims m1 = GetScalarModelVariables(lhsName, eh); //qwerty

                    if (m1.storage.Count == 0)
                    {
                        if (equationNameWithoutIndexes.GetName().StartsWith("e_j", StringComparison.OrdinalIgnoreCase))
                        {
                            //ignore
                        }
                        else
                        {
                            notFoundInEq.Add(equationNameWithoutIndexes);
                        }
                        if (shouldWrite) WriteEquation(eh, lhsName, equationNameWithIndexes, new string[] { "...unknown..." }, writer);
                        continue;  //Variable exists, but is not found in equation in any form
                    }

                    GekkoDictionary<string, bool>[] span = GetIndexesFromScalarEquations(m1);
                    List<string> eqIndexes = equationNameWithIndexes.GetIndexesExceptTime().Select(x => x.ToString()).ToList();
                    int nDim = GetDim(m1);
                    int summedDimensions = nDim - eqIndexes.Count;
                    string[] names = new string[nDim];

                    if (summedDimensions > 1)
                    {
                        int sum = 0;
                        for (int i = 0; i < nDim; i++)
                        {
                            if (span[i].Count > 1) sum++;
                        }
                        if (sum > 1)
                        {
                            //Probably a double sum, seems only around two of these
                        }
                    }

                    bool success = false;

                    if (nDim == 0)
                    {
                        success = true;  //can only be that one, without indexes (shown as x[]).                        
                    }
                    else
                    {
                        bool[] successDim = new bool[nDim];
                        for (int i = 0; i < nDim; i++)
                        {
                            if (span[i].Count == 0)
                            {
                                if (Globals.runningOnTTComputer) MessageBox.Show("Hovsa3"); //Not possible
                            }
                            else if (span[i].Count == 1)
                            {
                                names[i] = span[i].First().Key;  //Same as last
                                successDim[i] = true;
                            }
                            else
                            {
                                // -----------------------------------------------------------------------
                                // We must find which one fits best.
                                // Cannot count on first/last, because GAMS may garble this.
                                // -----------------------------------------------------------------------

                                List<int> spiral = GenerateSpiral(m1.storage.Count - 1);

                                //See if an index from the equation name (like "e_x_tot" indicating x[tot])
                                //can be used.
                                //
                                //TODO: What if there are several indexes, like "e_x_tot_atot"??
                                //
                                if (!successDim[i] && indexName != null)
                                {
                                    foreach (int j in spiral)
                                    {
                                        string s = m1.storage[j].storage[i];
                                        if (G.EqualHandleBlanks(s, indexName))
                                        {
                                            names[i] = s;
                                            successDim[i] = true;
                                            break;
                                        }
                                    }
                                }

                                //See if an index from an equation name like "e_x_atot" is a set/list, like #atot instead of 'atot'.
                                //
                                //TODO: What if there are several indexes, like "e_x_tot_atot"??
                                //TODO: This would be best to do dynamically, when calling the FIND window, so that a databank
                                //      with sets is loaded already.
                                //
                                if (!successDim[i] && mayUseDatabank)
                                {
                                    List iv = O.GetIVariableFromString("#" + indexName, O.ECreatePossibilities.NoneReturnNullAlways) as List;
                                    if (iv != null)
                                    {
                                        if (iv.list.Count() == 1)
                                        {
                                            ScalarString ss = iv.list[0] as ScalarString;
                                            if (ss != null)
                                            {
                                                foreach (int j in spiral)
                                                {
                                                    string s = m1.storage[j].storage[i];
                                                    if (G.Equal(s, ss.string2))
                                                    {
                                                        names[i] = s;
                                                        successDim[i] = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //If an element has the name "tot", it is assumed that it is the LHS element.
                                //This is done before checking if the name *contains* "tot", which is done below.
                                if (!successDim[i])
                                {
                                    foreach (int j in spiral)
                                    {
                                        string s = m1.storage[j].storage[i];
                                        if (G.Equal(s, "tot"))
                                        {
                                            names[i] = s;
                                            successDim[i] = true;
                                            break;
                                        }
                                    }
                                }

                                //If an element contains "tot", it is assumed that it is the LHS element.
                                if (!successDim[i])
                                {
                                    foreach (int j in spiral)
                                    {
                                        string s = m1.storage[j].storage[i];
                                        if (G.Contains(s, "tot"))
                                        {
                                            names[i] = s;
                                            successDim[i] = true;
                                            break;
                                        }
                                    }
                                }

                                //Here, we look at an equation name like e_x_tot[i, j] and try to match the "i" and "j"
                                //with the variable dimension elements (that may be for instance x[tot, j, i]).
                                //TODO: Use domain 
                                //TODO: Use domain 
                                //TODO: Use domain to know how dimensions match. Perhaps it can be induced from the scalar eqs rolling out?
                                //TODO: Use domain 
                                //TODO: Use domain 
                                //A bit hacky and not completely accurate as it is.                                
                                if (!successDim[i])
                                {
                                    foreach (string index in eqIndexes)
                                    {
                                        foreach (int j in spiral)
                                        {
                                            string s = m1.storage[j].storage[i];
                                            if (G.Contains(s, index))
                                            {
                                                names[i] = s;
                                                successDim[i] = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        success = true;
                        foreach (bool b in successDim)
                        {
                            if (!b) success = false;  //all must be true
                        }
                    }

                    if (success)
                    {
                        if (eh.eqName == null)
                        {
                        }                        
                        if (names.Length > 0) lhsName = new DNameNoTimeOrLag(lhsName.GetName(), EFreq.None, names.Select(s => (StringOrTime)s).ToArray());
                        
                        if (Globals.greu)
                        {
                            if (Globals.runningOnTTComputer && lhsEquationsStrings.ContainsKey(eh.eqName))
                            {
                                MessageBox.Show("Hovsa6"); //Not possible?
                            }
                            else
                            {
                                lhsEquationsStrings.Add(eh.eqName, lhsName);                                
                            }
                        }
                        else
                        {
                            if (Globals.runningOnTTComputer && lhsEquationsStrings.ContainsKey(eh.eqName))
                            {
                                MessageBox.Show("Hovsa6"); //Not possible?
                            }
                            else
                            {
                                lhsEquationsStrings.Add(eh.eqName, lhsName);
                            }
                        }
                    }
                    else
                    {
                        nFail++;
                        if (shouldWrite) WriteEquation(eh, lhsName, equationNameWithIndexes, names, writer);

                        if (true && Globals.runningOnTTComputer)
                        {
                            //Just for inspection in the debugger
                            string s = equationNameWithoutIndexes + G.NL;
                            s += equationNameWithIndexes + G.NL + G.NL;
                            s += eh.eqMathRaw + G.NL + G.NL;
                            s += eh.eqMathScalar + G.NL;
                        }
                    }
                }
            }

            if (Globals.runningOnTTComputer && shouldWrite && writer.Count > 0)
            {
                using (FileStream fs = Program.WaitForFileStream(@"c:\Thomas\Desktop\gekko\testing\lhs.txt", null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter res = G.GekkoStreamWriter(fs))
                {
                    foreach (string s in writer)
                    {
                        res.WriteLine(s);
                    }
                }
            }
            if (Globals.runningOnTTComputer)
            {
                new Writeln("TTH: nAll = " + nAll + ", nFail = " + nFail + " (notFoundInModel = " + notFoundInModel.Count + ", notFoundInEq = " + notFoundInEq.Count + "). EqDict = " + lhsEquationsStrings.Count() + ". Time: " + G.Seconds(t0));

                if (createProtobufferFileForUnitTests)
                {
                    //To find where this file is used in unit tests, go here: #tbjjjdf7hdsfas
                    Program.ProtobufWrite(lhsEquationsStrings, Globals.ttPath2 + @"\regres\Models\Decomp\decompfind_equations.data");
                }
            }

            if (G.IsUnitTestingOrNotShowingGUI())
            {
                Globals.unitTestLhsNotFoundInModel = notFoundInModel;
                Globals.unitTestLhsNotFoundInEq = notFoundInEq;
            }            

            return lhsEquationsStrings;
        }        

        private static void WriteEquation(EquationHelper2 eh, DName lhsName, DName equationNameWithIndexes, string[] names, List<string> writer)
        {
            writer.Add(equationNameWithIndexes + " ..");
            writer.Add(eh.eqMathRaw);
            writer.Add(eh.eqMathScalar);
            writer.Add("--> " + lhsName + "[" + Stringlist.GetListWithCommas(names) + "]");
            writer.Add(" ------------------------------------------------------------------------------------ ");
            writer.Add("");
        }

        private static void WalkScalarEquations(DName lhsName, TokenHelper tok, VariableDims m2)
        {
            if (tok.HasNoChildren())
            {
                if (G.Equal(tok.s, lhsName.ToString()))
                {
                    TokenHelper next = tok.SiblingAfter();
                    if (next != null)
                    {
                        if (next.SubnodesType() == "[")
                        {
                            List<TokenHelperComma> split = next.SplitCommas(true);

                            if (split.Count == 1 && (split[0].list.ToStringTrim().StartsWith("-") || split[0].list.ToStringTrim().StartsWith("+")))
                            {
                                //do nothing
                            }
                            else
                            {
                                bool isLag2 = false;
                                TokenHelper next2 = next.SiblingAfter();
                                if (next2.SubnodesType() == "[")
                                {
                                    List<TokenHelperComma> split2 = next2.SplitCommas(true);
                                    if (split2.Count == 1 && (split2[0].list.ToStringTrim().StartsWith("-") || split2[0].list.ToStringTrim().StartsWith("+")))
                                    {
                                        isLag2 = true;
                                    }
                                }

                                if (!isLag2)
                                {
                                    Dims m3 = new Dims();
                                    foreach (TokenHelperComma xx in split)
                                    {
                                        m3.storage.Add(xx.list.ToStringTrim());
                                    }
                                    m2.storage.Add(m3);
                                }
                            }
                        }
                        else
                        {
                            //No dimensions, no lag
                            Dims m3 = new Dims();
                            m2.storage.Add(m3);
                        }
                    }
                }
            }
            else
            {
                if (tok.artificialTopNode || tok.SubnodesType() == "(")
                {
                    for (int i = 0; i < tok.subnodes.storage.Count; i++)  //the count may increase, because subnodes may be added dynamically (translating x[i, t-1] into x[#i][-1])
                    {
                        WalkScalarEquations(lhsName, tok.subnodes[i], m2);
                    }
                }
            }
        }

        /// <summary>
        /// For a list of occurrences of the same varible like x[..., ...], this findes the occurrences of elements, for
        /// instace if we have input "x[a, m], x[b, m]" we get returned dim1 = ('a', 'b') and dim2 = ('m',).
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        public static GekkoDictionary<string, bool>[] GetIndexesFromScalarEquations(VariableDims variables)
        {
            int nDim = GetDim(variables);
            GekkoDictionary<string, bool>[] occurrences = new GekkoDictionary<string, bool>[nDim];
            for (int i = 0; i < nDim; i++) occurrences[i] = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            VariableDims temp = variables;
            foreach (Dims dims in temp.storage) //for each variable
            {                
                int c = -1;
                foreach (string s7 in dims.storage) //for each variable dimension, for instance "iB", "spTot".
                {
                    c++;
                    if (!occurrences[c].ContainsKey(s7)) occurrences[c].Add(s7, false);                    
                }
            }
            return occurrences;
        }

        private static int GetDim(VariableDims m1)
        {
            int nDim = 0;

            foreach (Dims dim in m1.storage)
            {
                nDim = Math.Max(dim.storage.Count, nDim);
            }

            return nDim;
        }

        private static VariableDims GetScalarModelVariables(DName lhsName, EquationHelper2 eh)
        {
            int nM2 = -12345;
            //For each sub-equation under the equation name
            //See also #jkadf773js7s
            string s = eh.eqMathScalar;
            string txt = s;
            TokenHelper tokens2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(txt, null, null, null, null);
            VariableDims m2 = new VariableDims();  //Count is # found variables in equation
            //For each token in the equation name
            WalkScalarEquations(lhsName, tokens2, m2);
            nM2 = m2.storage.Count;            
            return m2;
        }

        /// <summary>
        /// Gets all scalar equations by equation name (abstracting from indexes). Inside each value of key-value-pair, a list of equations
        /// is stored, showing the dimensions (abstracting from time).
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static Dictionary<DName, List<EquationHelper2>> GetScalarEquations(Model model)
        {            
            List<DName> eqs = model.modelGamsScalar.GetEqs(1);

            Dictionary<DName, List<EquationHelper2>> batches = new Dictionary<DName, List<EquationHelper2>>(Multidim2Comparer.IgnoreCase);
            Dictionary<DName, bool> known = new Dictionary<DName, bool>(Multidim2Comparer.IgnoreCase);

            foreach (DName eq in eqs)
            {
                if (eq == null) continue;
                try
                {                    
                    if (eq.GetName().Contains(Globals.scalarModelExtraVariable)) continue;
                    if (eq.GetName().Contains("e_temp")) continue;

                    DName noTime = eq.RemoveTime();

                    if (!known.ContainsKey(noTime))
                    {
                        known.Add(noTime, false);
                    }
                    else
                    {
                        continue;
                    }

                    GekkoTime time = eq.GetTime();

                    if (!time.IsNull())  //ignore for instance a timeless equation like E_tIOy_tBase[d,s]
                    {
                        DName noIndex = new DNameSimplest(eq.GetName());

                        if (!batches.ContainsKey(noIndex))
                        {
                            batches.Add(noIndex, new List<EquationHelper2>());
                        }

                        EquationTextHelper helper = new EquationTextHelper();
                        GetEquationTextHelper helper22 = model.GetEquationText(new List<DName>() { eq }, helper, time);
                        string scalar = helper22.s_scalarModel;
                        EquationHelper2 eh = new EquationHelper2();
                        eh.eqMathScalar = helper22.s_scalarModel;
                        eh.eqMathRaw = helper22.s_gamsOrFrnSyntax;
                        eh.eqName = eq.RemoveTime();
                        batches[noIndex].Add(eh);
                    }
                }
                catch { }  //We live with a fail on this
            }

            return batches;
        }

        public static void Identities()
        {
            ModelGamsScalar modelGamsScalar = Program.model.modelGamsScalar;
            GekkoDictionary<string, List<EquationNameAndNumber>> combos = new GekkoDictionary<string, List<EquationNameAndNumber>>(StringComparer.OrdinalIgnoreCase);  //key:varname, value:equation names
            GekkoTime t1 = new GekkoTime(EFreq.A, 2022, 1, 1);
            GekkoTime t2 = new GekkoTime(EFreq.A, 2024, 1, 1);

            //if (false)
            //{
            //    //List<IdentityHelper> eqs = new List<IdentityHelper>();

            //    ScalarDictionary sd = new ScalarDictionary();

            //    using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\" + "gams.gms", null, Program.GekkoFileReadOrWrite.Write))
            //    using (StreamWriter sw = G.GekkoStreamWriter(fs))
            //    {
            //        for (int i = 0; i < modelGamsScalar.CountEqs(1); i++)
            //        {
            //            string eq = Program.model.modelGamsScalar.dict_FromEqNumberToEqName[i];
            //            EquationTextHelper helper = new EquationTextHelper();
            //            helper.showTime = true;
            //            helper.emitScalarModel = true;
            //            //if (!eq.Contains(t.ToString() + "]")) continue;
            //            //if (eq.StartsWith("e_temp1")) continue;  //cf. gekko_equations.py
            //            //if (eq.StartsWith("e_temp2")) continue;  //cf. gekko_equations.py
            //            GetEquationTextHelper2 two = Program.model.modelGamsScalar.GetEquationTextUnfolded(eq, helper, false, t, sd);
            //            string eqText = two.s1 + ".. " + two.s2.Replace("=", "=E=") + ";";  //First one MUST be e1.. 
            //            sw.WriteLine(eqText);
            //        }
            //    }

            //    using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\" + "dict.txt", null, Program.GekkoFileReadOrWrite.Write))
            //    using (StreamWriter sw = G.GekkoStreamWriter(fs))
            //    {

            //        sw.WriteLine("Equation counts");  //Gekko will find this, and look for the next number
            //        sw.WriteLine("Total");
            //        sw.WriteLine(sd.eqsList.Count);
            //        sw.WriteLine();

            //        sw.WriteLine("Variable counts");  //Gekko will find this, and look for the next number
            //        sw.WriteLine("Total");
            //        sw.WriteLine(sd.varsList.Count);
            //        sw.WriteLine();

            //        sw.WriteLine("Equations 1 to " + sd.eqsList.Count);
            //        int j = -1;
            //        foreach (string s in sd.eqsList)
            //        {
            //            j++;
            //            sw.WriteLine("e" + (j + 1) + "  " + s.Replace("[", "(").Replace("]", ")"));
            //        }

            //        sw.WriteLine();
            //        sw.WriteLine("Variables 1 to " + sd.varsList.Count);
            //        j = -1;
            //        foreach (string s in sd.varsList)
            //        {
            //            j++;
            //            sw.WriteLine("x" + (j + 1) + "  " + s.Replace("[", "(").Replace("]", ")"));
            //        }
            //    }

            //    return;
            //}

            GekkoDictionary<string, bool> names; int dublets;
            HandleG_data(out names, out dublets);

            int a1 = 0;
            int a2 = 0;
            int a3 = 0;
            int n = modelGamsScalar.CountEqs(1);
            List<IdentityHelper> eqs = new List<IdentityHelper>();
            for (int i = 0; i < n; i++)
            {
                DName xx = modelGamsScalar.dict_FromEqNumberToEqName[i];                

                if (xx.GetTime().LargerThanOrEqual(t1) && xx.GetTime().SmallerThanOrEqual(t2))
                {
                    a1++;
                    EquationTextHelper helper = new EquationTextHelper();
                    helper.showTime = true;
                    List<string> precedentsTemp = modelGamsScalar.GetPrecedentsNames(i, helper, t1);
                    int c1 = 0;
                    int c2 = 0;
                    foreach (string variableName in precedentsTemp)
                    {
                        if (G.StartsWith(variableName, "res_")) continue;
                        c1++;
                        string variableNameWithoutBlanks = variableName.Replace(" ", "");
                        if (names.ContainsKey(variableNameWithoutBlanks)) c2++;
                        //string variableNameWithoutLagOrLead = G.Chop_RemoveLagOrLead(variableName);
                        //if (!combos.ContainsKey(variableNameWithoutLagOrLead)) combos.Add(variableNameWithoutLagOrLead, new List<EquationNameAndNumber>());
                        //combos[variableNameWithoutLagOrLead].Add(new EquationNameAndNumber() { i = i, name = equationName });
                    }
                    if (c1 == c2)
                    {
                        a2++;
                        DName eqName = modelGamsScalar.dict_FromEqNumberToEqName[i];
                        DName eqNameWithoutIndex = new DNameSimplest(eqName.GetName());
                        bool found = false;
                        foreach (IdentityHelper ih in eqs)
                        {
                            if (G.Equal(eqNameWithoutIndex, ih.eqName))
                            {
                                found = true;
                                ih.children.Add(eqName);
                                break;
                            }
                        }
                        if (!found)
                        {
                            IdentityHelper ih = new IdentityHelper();
                            ih.eqName = eqNameWithoutIndex;
                            ih.children.Add(eqName);
                            eqs.Add(ih);
                        }
                    }
                    else if (c1 == c2 + 1)
                    {
                        a3++;
                    }
                }
            }

            StringBuilder sb_gams_gms = new StringBuilder();
            StringBuilder sb_raw_gms = new StringBuilder();
            ScalarDictionary sd2 = new ScalarDictionary();
            eqs = eqs.OrderBy(x1 => x1.eqName, new MultidimSortComparer(true)).ToList();
            using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\" + "identities.txt", null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter sw = G.GekkoStreamWriter(fs))
            {
                foreach (IdentityHelper ih in eqs)
                {
                    List<DName> childrenSorted = ih.children.OrderBy(x2 => x2, new MultidimSortComparer(true)).ToList();
                    List<DName> xx = new List<DName>();
                    foreach (DName s in childrenSorted)
                    {
                        xx.Add(ih.eqName.RemoveTime());
                    }
                    EquationTextHelper eh = new EquationTextHelper();
                    eh.showTime = false;
                    GetEquationTextHelper output1 = Program.model.GetEquationText(new List<DName>() { childrenSorted[0] }, eh, t1);
                    string extra = null;
                    if (childrenSorted.Count > 1) extra = " (" + childrenSorted.Count + " sub-equations)";
                    sw.WriteLine(ih.eqName + extra);
                    if (childrenSorted.Count > 1) sw.WriteLine(Stringlist.GetListWithCommas(xx));
                    sw.WriteLine();
                    sw.WriteLine(output1.s_gamsOrFrnSyntax);
                    sw.WriteLine(output1.s_scalarModel);
                    if (childrenSorted.Count > 1)
                    {
                        sw.WriteLine("...");
                        GetEquationTextHelper output2 = Program.model.GetEquationText(new List<DName>() { childrenSorted[childrenSorted.Count - 1] }, eh, t1);
                        sw.WriteLine(output2.s_scalarModel);
                    }
                    sw.WriteLine();
                    sw.WriteLine("================================================================================");
                    sw.WriteLine();

                    foreach (DName child in childrenSorted)
                    {
                        DName eq = child; // Program.model.modelGamsScalar.dict_FromEqNumberToEqName[i];
                        EquationTextHelper helper = new EquationTextHelper();
                        helper.showTime = true;
                        helper.emitScalarModel = true;
                        //if (!eq.Contains(t.ToString() + "]")) continue;
                        //if (eq.StartsWith("e_temp1")) continue;  //cf. gekko_equations.py
                        //if (eq.StartsWith("e_temp2")) continue;  //cf. gekko_equations.py
                        GetEquationTextHelper2 two = Program.model.modelGamsScalar.GetEquationTextUnfolded(eq, helper, false, t1, sd2);
                        string eqText = two.s1 + ".. " + two.s2.Replace("=", "=E=") + ";";  //First one MUST be e1.. 
                        sb_gams_gms.AppendLine(eqText);
                    }
                    sb_raw_gms.AppendLine(output1.s_gamsOrFrnSyntax);
                }
                sw.Flush(); sw.Close();
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("* " + Globals.string_equation_counts);  //Gekko will find this, and look for the next number
            sb.AppendLine("* " + Globals.string_total);
            sb.AppendLine("* " + sd2.eqsList.Count.ToString());
            sb.AppendLine();
            sb.AppendLine("* " + Globals.string_variable_counts);  //Gekko will find this, and look for the next number
            sb.AppendLine("* " + Globals.string_total);
            sb.AppendLine("* " + sd2.varsList.Count.ToString());
            sb.AppendLine();
            sb_gams_gms.Insert(0, sb);

            using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\" + "dict.txt", null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter sw = G.GekkoStreamWriter(fs))
            {

                sw.WriteLine(Globals.string_equation_counts);  //Gekko will find this, and look for the next number
                sw.WriteLine(Globals.string_total);
                sw.WriteLine(sd2.eqsList.Count);
                sw.WriteLine();

                sw.WriteLine(Globals.string_variable_counts);  //Gekko will find this, and look for the next number
                sw.WriteLine(Globals.string_total);
                sw.WriteLine(sd2.varsList.Count);
                sw.WriteLine();

                sw.WriteLine(Globals.string_equations_1_to + sd2.eqsList.Count);
                int j = -1;
                foreach (DName s in sd2.eqsList)
                {
                    j++;
                    sw.WriteLine("e" + (j + 1) + "  " + s.ToString().Replace("[", "(").Replace("]", ")"));
                }

                sw.WriteLine();
                sw.WriteLine(Globals.string_variables_1_to + sd2.varsList.Count);
                j = -1;
                foreach (DName s in sd2.varsList)
                {
                    j++;
                    sw.WriteLine("x" + (j + 1) + "  " + s.ToString().Replace("[", "(").Replace("]", ")"));
                }
            }

            using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\" + Globals.string_gams_gms, null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter sw = G.GekkoStreamWriter(fs))
            {
                sw.WriteLine(sb_gams_gms);
            }            

            using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\" + Globals.string_raw_gms, null, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter sw = G.GekkoStreamWriter(fs))
            {
                sw.WriteLine(Program.model.modelGams.rawGmsFile);
            }

            new Writeln("Found " + a1 + " eqs for 2024, of which " + a2 + " are identities, and " + a3 + " are near-identities");
            new Writeln("Found " + eqs.Count + " super-eqs for 2024");
            new Writeln("Dublets: " + dublets + " out of " + names.Count + " names");

            return;
        }

        private static void HandleG_data(out GekkoDictionary<string, bool> names, out int dublets)
        {
            Databank db = Program.databanks.GetDatabank("m");
            names = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            dublets = 0;
            foreach (KeyValuePair<string, IVariable> kvp in db.storage)
            {
                if (kvp.Key.StartsWith("#G_data_"))
                {
                    string name = kvp.Key.Substring("#G_data_".Length);
                    List m1 = kvp.Value as List;
                    if (m1.Count() == 0)
                    {
                        if (names.ContainsKey(name)) { dublets++; }
                        else names.Add(name, false);
                        continue;
                    }
                    if (m1.list[0].Type() == EVariableType.String)
                    {
                        foreach (ScalarString ss2 in m1.list)
                        {
                            string fullName = name + "[" + ss2.string2 + "]";
                            if (names.ContainsKey(fullName)) { dublets++; }
                            else names.Add(fullName, false);
                        }
                    }
                    else
                    {
                        foreach (List m2 in m1.list)
                        {
                            string idx = null;
                            foreach (ScalarString ss in m2.list)
                            {
                                idx += ss.string2 + ",";
                            }
                            idx = idx.Substring(0, idx.Length - 1);
                            string fullName = name + "[" + idx + "]";
                            if (names.ContainsKey(fullName)) { dublets++; }
                            else names.Add(fullName, false);
                        }
                    }
                }
            }
        }

        private static void ReadGamsScalarModelEquationsLines(EqLineHelper helper, string[] split2, ref TokenList tokensLast, List<string> values, List<string> end, ref int eqCounts, ref int varCounts, ref int semis, List<string> csCodeLines, ref StringBuilder eqLine, StreamReader sr)
        {
            EModelEquationsOrVariables status = EModelEquationsOrVariables.None;
            EEquationCountsOrVariableCounts substatus = EEquationCountsOrVariableCounts.None;            
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                if (status == EModelEquationsOrVariables.None)
                {
                    if (line.StartsWith("e1.."))
                    {
                        eqLine = new StringBuilder(line);
                        if (line.EndsWith(";"))
                        {
                            semis++;
                            int hits2 = helper.known;
                            bool ignore = false;
                            tokensLast = HandleEqLine(eqLine, tokensLast, helper, ref ignore);  //first line cannot suffer from that
                            if (helper.known == hits2) RemoveDoubleDots(helper, csCodeLines);
                            eqLine = new StringBuilder();
                        }
                        status = EModelEquationsOrVariables.Equations;
                    }
                    else
                    {
                        //start.Add(line);
                        if (line.StartsWith("* " + Globals.string_equation_counts))
                        {
                            substatus = EEquationCountsOrVariableCounts.EquationCounts;
                        }
                        else if (line.StartsWith("* " + Globals.string_variable_counts))
                        {
                            substatus = EEquationCountsOrVariableCounts.VariableCounts;
                        }
                        if (substatus == EEquationCountsOrVariableCounts.EquationCounts)
                        {
                            string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string sx in ss)
                            {
                                if (G.IsInteger(sx))
                                {
                                    eqCounts = int.Parse(sx);
                                    substatus = EEquationCountsOrVariableCounts.None;
                                    break;
                                }
                            }
                        }
                        else if (substatus == EEquationCountsOrVariableCounts.VariableCounts)
                        {
                            string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string sx in ss)
                            {
                                if (G.IsInteger(sx))
                                {
                                    varCounts = int.Parse(sx);
                                    substatus = EEquationCountsOrVariableCounts.None;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (status == EModelEquationsOrVariables.Equations)
                {
                    if (line.StartsWith("* set non-default bounds", StringComparison.OrdinalIgnoreCase) || line.StartsWith("* set non-default levels", StringComparison.OrdinalIgnoreCase))
                    {
                        values.Add(line);
                        status = EModelEquationsOrVariables.Variables;
                    }
                    else
                    {
                        if (line.EndsWith(";"))
                        {
                            semis++;
                            eqLine.Append(line);
                            int hits2 = helper.known;
                            bool ignore = false;
                            tokensLast = HandleEqLine(eqLine, tokensLast, helper, ref ignore);
                            if (!ignore)
                            {
                                if (helper.known == hits2) RemoveDoubleDots(helper, csCodeLines);
                            }
                            eqLine = new StringBuilder();
                        }
                        else
                        {
                            eqLine.Append(line);
                        }
                    }
                }
                else if (status == EModelEquationsOrVariables.Variables)
                {
                    if (line.ToLower().StartsWith("model ")) //model m / all /;
                    {
                        end.Add(line);
                        status = EModelEquationsOrVariables.Done;
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

        
        /// <summary>
        /// Reads GAMS dictionary dict.txt (made by GAMS CONVERT). Beware that GAMS treats "..." or '...' as a string, and that "ab'cd" or 'ab"cd' are legal, representing ab'cd or ab"cd.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="split2"></param>
        /// <param name="timeless"></param>
        /// <param name="status2"></param>
        /// <param name="substatus2"></param>
        /// <param name="eqCounts2"></param>
        /// <param name="varCounts2"></param>
        /// <param name="fakeEqCounts2"></param>
        /// <param name="fakeVarCounts2"></param>
        /// <param name="sr"></param>
        private static void ReadScalarModelEquationsDictionaryLines(GekkoTime t1, GekkoTime t2, EqLineHelper helper, string[] split2, Dictionary<int, int> timeless, EFreq gekkoModelFreq, ref bool res_variables, ref int eqCounts2, ref int varCounts2, ref int fakeEqCounts2, ref int fakeVarCounts2, TextReader sr)
        {
            if ((!t1.IsNull() && !t2.IsNull()) && ( t1.freq != EFreq.A || t2.freq!=EFreq.A)) new Error("MODEL with time period only implemented for annual time periods");
            EEquationsOrVariables status2 = EEquationsOrVariables.None;            
            EEquationCountsOrVariableCounts substatus2 = EEquationCountsOrVariableCounts.None;
            bool b = false;
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Trim() == "") continue;
                if (G.Contains(line, Globals.string_equation_counts))
                {
                    substatus2 = EEquationCountsOrVariableCounts.EquationCounts;
                }
                else if (G.Contains(line, Globals.string_variable_counts))
                {
                    substatus2 = EEquationCountsOrVariableCounts.VariableCounts;
                }

                if (substatus2 == EEquationCountsOrVariableCounts.EquationCounts)
                {
                    string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string sx in ss)
                    {
                        if (G.IsInteger(sx))
                        {
                            eqCounts2 = int.Parse(sx);
                            substatus2 = 0;
                            helper.dict_FromEqNumberToEqName = new DName[eqCounts2];

                            //Must items be added?
                            //for (int i = 0; i < eqCounts2; i++)
                            //{
                            //    helper.dict_FromEqNumberToEqName[i] = new DName();  //because of protobuf when truncating periods
                            //}

                            helper.dict_FromEqNumberToEqChunkNumber = new int[eqCounts2];
                            break;
                        }
                    }
                }
                else if (substatus2 == EEquationCountsOrVariableCounts.VariableCounts)
                {
                    string[] ss = line.Split(split2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string sx in ss)
                    {
                        if (G.IsInteger(sx))
                        {
                            varCounts2 = int.Parse(sx);
                            substatus2 = 0;
                            helper.dict_FromVarNumberToVarName = new DName[varCounts2];
                            break;
                        }
                    }
                }

                if (line.StartsWith(Globals.string_equations_1_to))
                {
                    status2 = EEquationsOrVariables.Equations;
                    continue;
                }
                else if (line.StartsWith(Globals.string_variables_1_to))
                {
                    status2 = EEquationsOrVariables.Variables;
                    continue;
                }
                if (status2 == EEquationsOrVariables.Equations)
                {                    
                    int n; string nameWithIndexes; string nameWithIndexesNoTime; string nameWithoutIndexes;
                    List<string> parts; string time;
                    LineChopper(line, 'e', gekkoModelFreq, out n, out nameWithIndexes, out nameWithIndexesNoTime, out nameWithoutIndexes, out parts, out time);

                    if (time != null) //Could in principle be a timeless equation
                    {
                        if (!t1.IsNull() && !t2.IsNull())
                        {
                            int itime = G.IntParse(time);
                            if (itime == -12345)
                            {
                                new Error("The MODEL statement uses local time period, but an equation has a non-annual time period ('" + time + "')");
                            }
                            if (itime < t1.super || itime > t2.super) continue; //skip eqs outside given annual period
                        }
                    }

                    string eqName = nameWithIndexes;
                    if (G.Contains(eqName, Globals.scalarModelExtraVariable))
                    {
                        fakeEqCounts2++;
                    }
                    
                    eqName = nameWithoutIndexes;
                    DName temp1 = Program.DName_HACK1(nameWithIndexes);
                    helper.dict_FromEqNumberToEqName[n] = temp1;
                    helper.dict_FromEqNameToEqNumber.Add(temp1, n);  //filling this out could be postponed until decomp if loading is slow                        
                    DName temp2 = Program.DName_HACK1(eqName);
                    if (!helper.dict_FromEqNameToEqChunkNumber.ContainsKey(temp2))
                    {
                        helper.dict_FromEqNameToEqChunkNumber.Add(temp2, helper.dict_FromEqNameToEqChunkNumber.Count());
                    }
                    
                    helper.dict_FromEqNumberToEqChunkNumber[n] = helper.dict_FromEqNameToEqChunkNumber.Count() - 1;
                }
                else if (status2 == EEquationsOrVariables.Variables)
                {                    
                    int n; string nameWithIndexes; string nameWithIndexesNoTime; string nameWithoutIndexes;
                    List<string> parts; string time;
                    LineChopper(line, 'x', gekkoModelFreq, out n, out nameWithIndexes, out nameWithIndexesNoTime, out nameWithoutIndexes, out parts, out time);

                    //if (Globals.greuHack)
                    //{
                    //    int itime = G.IntParse(time);
                    //    if (itime != -12345 && (itime < 2020 || itime > 2025)) continue;
                    //}

                    if (!res_variables && G.StartsWith(nameWithIndexes, Globals.decompResidualPrefix)) res_variables = true;                    

                    if (G.Contains(nameWithIndexes, Globals.scalarModelExtraVariable))
                    {
                        fakeVarCounts2++;
                    }

                    DName temp5 = Program.DName_HACK1(nameWithIndexes);
                    helper.dict_FromVarNumberToVarName[n] = temp5;
                    helper.dict_FromVarNameToVarNumber.Add(temp5, n);

                    GekkoTime t = GekkoTime.tNull;
                    if (time != null)
                    {
                        EFreq freq = GetResultingFreq(gekkoModelFreq);
                        if (freq == EFreq.A)
                        {
                            //We try to do it fast for YYYY annual type.
                            int i = G.IntParse(time);
                            if (i != -12345) t = new GekkoTime(EFreq.A, i, 1);                            
                        }
                        else if (freq == EFreq.Q)
                        {
                            t = GekkoTime.FromStringToGekkoTime(time, false, true, false);                            
                        }
                        else new Error("Only Annual or Quarterly freq supported");                        
                    }

                    if (t.IsNull())
                    {
                        if (!timeless.ContainsKey(helper.dict_FromVarNameToANumber.Count())) timeless.Add(helper.dict_FromVarNameToANumber.Count(), 1); //1 is just arbitrary
                    }
                    else
                    {
                        if (helper.t1.IsNull() || t.StrictlySmallerThan(helper.t1)) helper.t1 = t;
                        if (helper.t2.IsNull() || t.StrictlyLargerThan(helper.t2)) helper.t2 = t;
                    }
                    //helper.dict_FromVarNameToANumber.AddIfNotAlreadyThere(nameWithIndexesNoTime, helper.dict_FromVarNameToANumber.Count(), b);
                    DName temp = Program.DName_HACK1(nameWithIndexesNoTime);
                    if (!helper.dict_FromVarNameToANumber.ContainsKey(temp))
                    {
                        helper.dict_FromVarNameToANumber.Add(temp, helper.dict_FromVarNameToANumber.Count());
                    }
                }
            }
        }

        /// <summary>
        /// Chops up a GAMS dictionary line like "x1  x(i, j, 2025)" into eq number (n), name variants, a list of strings of non-time dimensions, 
        /// and lastly time (may be null). For annual, time must be 4 times 0..9 where the first is 1 or 2. Like 1990 or 2350.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="ex"></param>
        /// <param name="gekkoModelFreq"></param>
        /// <param name="n"></param>
        /// <param name="nameWithIndex"></param>
        /// <param name="nameWithIndexNoTime"></param>
        /// <param name="nameWithoutIndex"></param>
        /// <param name="parts"></param>
        /// <param name="time"></param>
        private static void LineChopper(string line, char ex, EFreq gekkoModelFreq, out int n, out string nameWithIndex, out string nameWithIndexNoTime, out string nameWithoutIndex, out List<string> parts, out string time)
        {
            nameWithoutIndex = null;
            time = null;
            parts = null;
            int timePart = -12345;
            int idx5 = G.FirstNonBlankIndexOf(line, 0);
            if (idx5 == -1 || line[idx5] != ex) new Error("Malformed " + ex + "... line: " + line);
            int idx6 = G.FirstBlankIndexOf(line, idx5 + 1);
            if (idx6 == -1) new Error("Malformed " + ex + "... line: " + line);
            int idx7 = G.FirstNonBlankIndexOf(line, idx6 + 1);
            if (idx7 == -1) new Error("Malformed " + ex + "... line: " + line);
            n = -12345;
            try
            {
                n = int.Parse(G.Substring(line, idx5 + 1, idx6 - 1)) - 1; //so it is 0-based
            }
            catch
            {
                new Error("Malformed " + ex + "... line integer: " + line);
            }
            
            nameWithIndex = G.ReplaceIgnoreCaseIgnoreQuoted(G.ReplaceIgnoreCaseIgnoreQuoted(G.Substring(line, idx7, line.Length - 1), "(", "["), ")", "]");            
            nameWithIndexNoTime = nameWithIndex;
            int i = nameWithIndex.IndexOf('[');
            if (i != -1)
            {
                EFreq freq = GetResultingFreq(gekkoModelFreq);                                
                //This version does not need the time index to be last, also stuff like x[a, b, 'a,b', 2020] is valid, 4 elements.

                nameWithoutIndex = nameWithIndex.Substring(0, i).Trim();
                string rest = nameWithIndex.Substring(i).Trim();
                string rest2 = rest.Substring(1, rest.Length - 2);
                parts = G.SplitIgnoringQuotedCommas(rest2, true);
                int counter = -1;
                foreach (string part in parts)
                {
                    counter++;                    
                    bool isTime = false;

                    if (freq == EFreq.A)
                    {                        
                        if (G.LooksLikeYear(part))
                        {
                            if (timePart != -12345) new Error("2 time indexes found: " + nameWithIndex);
                            timePart = counter;
                        }
                    }
                    else if (freq == EFreq.Q)
                    {                        
                        if (G.LooksLikeQuarter(part))
                        {
                            if (timePart != -12345) new Error("2 time indexes found: " + nameWithIndex);
                            timePart = counter;
                        }
                    }
                    else new Error("Model: only Annual and Quarterly supported at the moment");
                }

                if (timePart != -12345)
                {
                    if (timePart != parts.Count - 1)
                    {
                        //Move time part last
                        string temp = parts[timePart];
                        parts[timePart] = parts[parts.Count - 1];
                        parts[parts.Count - 1] = temp;

                        StringBuilder sb1 = new StringBuilder();
                        for (int j = 0; j < parts.Count; j++)
                        {
                            string s = parts[j];
                            sb1.Append(s).Append(",");
                        }
                        if (sb1.Length > 0) sb1.Length--;
                        string s3 = null; if (sb1.Length > 0) s3 = "[" + sb1.ToString() + "]";
                        nameWithIndex = nameWithoutIndex + s3;
                    }
                    else
                    {
                        //is already done at top of method
                    }

                    StringBuilder sb2 = new StringBuilder();
                    for (int j = 0; j < parts.Count - 1; j++)  //skips the last, which is time
                    {
                        string s = parts[j];
                        sb2.Append(s).Append(",");
                    }
                    if (sb2.Length > 0) sb2.Length--;
                    string s2 = null; if (sb2.Length > 0) s2 = "[" + sb2.ToString() + "]";
                    nameWithIndexNoTime = nameWithoutIndex + s2;
                    time = parts[parts.Count - 1];
                }
                else
                {
                    //No time
                    //is already done at top of method
                }

            }
            else
            {
                nameWithoutIndex = nameWithIndex;
            }
        }        

        private static EFreq GetResultingFreq(EFreq gekkoModelFreq)
        {
            EFreq freq = Program.options.model_gams_scalar_freq;  //Default: .A
            if (gekkoModelFreq != EFreq.None) freq = gekkoModelFreq; //This only happens for a Gekko .frm model decorated with frequency info
            return freq;
        }

        private static void CalculatePrecedentsAndDependents(ModelGamsScalar modelGamsScalar, int bigN)
        {
            modelGamsScalar.precedents = new List<ModelScalarEquation>();
            modelGamsScalar.dependents = new Dictionary<long, List<int>>();
            Dictionary<long, bool> helper = new Dictionary<long, bool>();

            DateTime dt1 = DateTime.Now;

            //First precedents
            for (int eqNumber = 0; eqNumber < bigN; eqNumber++)
            {                
                if (Globals.greuHack)
                {
                    if (modelGamsScalar.dict_FromEqNumberToEqName[eqNumber] == null)
                    {
                        modelGamsScalar.precedents.Add(new ModelScalarEquation());
                        continue;
                    }
                }
                
                ModelScalarEquation precedentsInEquation = new ModelScalarEquation();
                modelGamsScalar.precedents.Add(precedentsInEquation);
                helper.Clear();  //prepare for next equation (eliminate dublets)                
                for (int i = 0; i < modelGamsScalar.bb[eqNumber].Length; i += 2)
                {
                    //foreach precedent variable
                    long dp = ModelGamsScalar.PackPeriodAndVariable(modelGamsScalar.bb[eqNumber][i], modelGamsScalar.bb[eqNumber][i + 1]);
                    if (Globals.runningOnTTComputer)  //Just an assert here
                    {
                        if (ModelGamsScalar.UnpackVariable(dp) == -12345)
                        {
                            if (!Globals.greuHack) G.WarningInternal("TTH: Variable number == -12345...");
                        }
                        else
                        {
                            bool b = modelGamsScalar.isTimeless[ModelGamsScalar.UnpackVariable(dp)];
                            if (b && ModelGamsScalar.UnpackPeriod(dp) != Globals.decompTimelessNumber)
                            {
                                if (!Globals.greuHack) G.WarningInternal("TTH: Expected timeless .date = " + Globals.decompTimelessNumber);
                            }
                        }                        
                    }
                    if (!helper.ContainsKey(dp)) //Faster to look up than using equ.vars list. There can be hundreds of variables!
                    {
                        helper.Add(dp, false);
                        precedentsInEquation.vars.Add(dp);  //avoid dublets.
                    }
                }
            }
            
            if (Globals.runningOnTTComputer) new Writeln("TTH: --sub-- precedents: " + G.Seconds(dt1));
            dt1 = DateTime.Now;

            //Then dependents
            //mapping from a varname to the equations it is part of                
            for (int eqNumber = 0; eqNumber < bigN; eqNumber++)
            {
                if (Globals.greuHack)
                {
                    if (modelGamsScalar.dict_FromEqNumberToEqName[eqNumber] == null)
                    {                        
                        continue;
                    }
                }

                //foreach precedent variable
                foreach (long dp in modelGamsScalar.precedents[eqNumber].vars)
                {
                    List<int> eqsHere = null;
                    modelGamsScalar.dependents.TryGetValue(dp, out eqsHere);
                    if (eqsHere == null)
                    {
                        modelGamsScalar.dependents.Add(dp, new List<int>() { eqNumber });
                    }
                    else
                    {                        
                        eqsHere.Add(eqNumber);
                    }
                }
            }
            
            if (Globals.runningOnTTComputer) new Writeln("TTH: --sub-- dependents: " + G.Seconds(dt1));
            dt1 = DateTime.Now;
        }        

        private static void RemoveDoubleDots(EqLineHelper helper, List<string> output)
        {
            string s = helper.sb.ToString();
            if (s == "")
            {
                //ignore (may be date-truncated)
                //output.Add(s);  //HMMMM, these are empty, but needed when compiling...?
            }
            else
            {
                s = "r[i] = " + s.Replace("..", "").Replace("=E=", "-(").Replace(";", ");");
                output.Add(s);
            }            
        }

        /// <summary>
        /// Note: tokensLast is the last line that got tokenized, which is compared to the current line. Only if they differ the equation is added,
        /// so that we do not add mathematically identical equation lines (they are often identical over time).
        /// </summary>
        /// <param name="eqLine"></param>
        /// <param name="tokensLast"></param>
        /// <param name="helper"></param>
        /// <param name="shouldBeIgnored"></param>
        /// <returns></returns>
        private static TokenList HandleEqLine(StringBuilder eqLine, TokenList tokensLast, EqLineHelper helper, ref bool shouldBeIgnored)
        {
            //Remember: the human readable code is derived from this, so beware if changes are made,
            //cf. #af931klljaf89efw.            
            helper.Clear();
            int more = 2;
            bool knownPattern = true;
            TokenList tokens = null;

            string sEqLine = eqLine.ToString();
            int iDot = sEqLine.IndexOf("..");            
            int equationNumber = int.Parse(sEqLine.Substring(1, iDot - 1)) - 1; //0-based, ignoring the first 'e'                                       
            if (helper.dict_FromEqNumberToEqName[equationNumber] != null)
            {
                //if (Globals.runningOnTTComputer && helper.dict_FromEqNumberToEqName[equationNumber] == null) G.WarningInternal("Did not expect null in equation name");
                tokens = StringTokenizer.GetTokensWithLeftBlanks(sEqLine, more);  //1 empty "" token
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

                        if (lefttype == int.MaxValue || righttype == int.MaxValue) new Error("Problem resolving '**' power");

                        helper.remove.Add(i, "");
                        helper.remove.Add(i + 1, "");
                        helper.addBefore.Add(i, ",");
                        if (lefttype == -100) helper.addBefore.Add(i - 1, "M.Power(");
                        else if (lefttype > 0) helper.addBefore.Add(lefttype, "M.Power(");
                        if (righttype == -100) helper.addBefore.Add(i + 3, ")");
                        else if (righttype > 0) helper.addBefore.Add(righttype + 1, ")");
                    }
                }
                
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
                    if (IsNumber(th1))
                    {
                        if (th2 != null && IsNumber(th2))
                        {
                            //do nothing
                        }
                        else
                        {
                            knownPattern = false;
                        }

                        string sNumber = th1.ToString().Trim();
                        if (G.Equal(sNumber, "eps"))
                        {
                            sNumber = "0";
                        }

                        int i1 = helper.dict_Constants.Count;
                        if (helper.dict_Constants.ContainsKey(sNumber))
                        {
                            i1 = helper.dict_Constants[sNumber];
                        }
                        else
                        {
                            helper.dict_Constants.Add(sNumber, i1);
                            try
                            {
                                helper.exoValues.Add(double.Parse(sNumber));
                            }
                            catch
                            {
                                new Error("Could not parse the string '" + sNumber + "' as a value");
                            }
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

                        int number = -12345;

                        try
                        {
                            number = int.Parse(th1.s.Substring(1)) - 1;  //0-based
                        }
                        catch
                        {
                            new Error("Could not parse integer part of the string '" + th1.s + "'");
                            throw;
                        }

                        string eqname = helper.dict_FromEqNumberToEqName[number].ToString();

                        if (eqname.StartsWith("e" + Globals.scalarModelExtraVariable))
                        {
                            //Such equations may be either (too many eqs or too few):                      

                            //equation egekkoextra0; egekkoextra0 .. xgekkoextra0 + xgekkoextra1 + ... = E = 0;
                            //
                            // --or-- 
                            //
                            //equation egekkoextra0; egekkoextra0 .. sum(t, qBnp[t]) =E= 0;
                            //equation egekkoextra1; egekkoextra1 .. sum(t, qBnp[t]) =E= 0;
                            //...

                            shouldBeIgnored = true;

                            //#oijlksaa
                        }

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
                        int number = -12345;
                        try
                        {
                            number = int.Parse(th1.s.Substring(1)) - 1;  //0-based
                        }
                        catch
                        {
                            new Error("Could not parse integer part of the string '" + th1.s + "'");
                        }

                        DName varname = helper.dict_FromVarNumberToVarName[number]; //#oijlksaa                        

                        int i1 = -12345;
                        if (varname.GetTime().IsNull())
                        {
                            i1 = Globals.decompTimelessNumber; //signals timeless (-12345)
                        }
                        else
                        {
                            i1 = varname.GetTime().Subtract(helper.tBasis);
                        }
                        
                        int i2; if (!helper.dict_FromVarNameToANumber.TryGetValue(varname.RemoveTime(), out i2)) i2 = -12345;

                        int ii1 = helper.endo.Count;
                        int ii2 = helper.endo.Count + 1;

                        bool seenBefore = false;

                        HandleEqLineAppend(helper, i, "a[b[" + ii1 + "]+t][b[" + ii2 + "]]");

                        if (!seenBefore)
                        {
                            //avoid dublets in an equation (for instance y[2020] = x[2020] + x[2020]/z[2020])
                            helper.endo.Add(i1);  //time
                            helper.endo.Add(i2);  //variable
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
            }
            else
            {                
                //helper.unique++; //This is WRONG!          
                helper.known++; //This is WRONG!          
                helper.count++; //And this too
                //helper.eqPointers.Add(helper.unique - 1);  //unique is 1 for the first equation. For the second, it may be 1 or 2. So 0 points to 0, 1 points to 0 or 1.
                helper.eqPointers.Add(-12345);
                helper.b.Add(new List<int>());
                //helper.c.AddRange(...);
                helper.d.Add(new List<int>());
                tokens = tokensLast;  //Just pass this on: no equation tokens were added, so we just pass on the tokens of the last real equation.
            }           

            return tokens;  //to compare with next
        }

        private static bool IsNumber(TokenHelper th1)
        {
            return th1.type == ETokenType.Number || G.Equal(th1.s, "eps");
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
                else if (G.Equal(th1.s, "errorf")) s = "M.Errorf";  //GREU, seems it is "Integral of the standard normal distribution"
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

            Tuple<Dictionary<DName, DName>, StringBuilder> tup = GetDependentsGams(o.opt_dep);
            Dictionary<DName, DName> dependents = tup.Item1;
            //
            // Should #dependents list be reflected in hash ?????
            //
            model.modelGams = ReadGamsModelHelper(false, textInputRaw, fileName, dependents, G.Equal(o.opt_dump, "yes"), false, model);            
            DateTime t1 = DateTime.Now;
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

            GAMSScalarModelSettings input = new GAMSScalarModelSettings();
            input.t1 = o.t1;
            input.t2 = o.t2;
            if (!input.t1.IsNull() && input.t2.IsNull()) new Error("Expected both periods to be either null or non-null"); //Can probably not happen
            
            input.zipFilePathAndName = fileName;

            DateTime t2 = DateTime.Now;

            model = ReadGAMSScalarModel2(o, folders, model, input);

            ModelInfoGamsScalar mi = new ModelInfoGamsScalar();  //so that we also get this info when loading from cache
            mi.modelName = input.zipFilePathAndName;
            mi.periodT1 = model.modelGamsScalar.absoluteT1;
            mi.periodT2 = model.modelGamsScalar.absoluteT2;
            mi.countEqs1 = model.modelGamsScalar.CountEqs(1);
            mi.countEqs2 = model.modelGamsScalar.CountEqs(2);
            mi.countEqs3 = model.modelGamsScalar.CountEqs(3);
            mi.countVars1 = model.modelGamsScalar.CountVars(1);
            mi.countVars2 = model.modelGamsScalar.CountVars(2);
            mi.countVars3 = model.modelGamsScalar.CountVars(3);
            mi.hasReadSomeData = model.modelGamsScalar.hasReadSomeData;

            model.modelGamsScalar.modelInfoGamsScalar = mi;
            model.modelGamsScalar.modelInfoGamsScalar.Print(false, model.modelGamsScalar.hasResVariables, t);

            return model;

        }

        
        private static Model ReadGAMSScalarModel2(O.Model o, List<string> folders, Model model, GAMSScalarModelSettings input)
        {
            //if (Globals.runningOnTTComputer) MessageBox.Show("TT comment: Parsing scalar model...");
            FindFileHelper ffh2 = Program.FindFile(input.zipFilePathAndName + "\\" + "ModelInfo.json", folders, true, true, false, false, o.p);

            //defaults
            input.unrolledModel = Globals.string_gams_gms;
            input.unrolledNames = Globals.string_dict_txt;
            input.rawModel = Globals.string_raw_gms;

            if (ffh2.realPathAndFileName == null)
            {
                //no ModelInfo.json found, then we assume defaults                
            }
            else
            {
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
                        txt.MoreAdd("Gekko uses a suitable ModelInfo.json inside the .zip file to describe the model files. See description in the {a{MODEL¤download.htm}a} commmand.");
                        txt.MoreNewLine();
                        txt.MoreAdd("The technical error message is the following: " + e.Message);
                    }
                }

                try { input.unrolledModel = (string)jsonTree["unrolledModel"]; } catch { }
                try { input.unrolledNames = (string)jsonTree["unrolledNames"]; } catch { }                
                try { input.rawModel = (string)jsonTree["rawModel"]; } catch { }                
            }

            input.ffh_unrolledModel = Program.FindFile(input.zipFilePathAndName + "\\" + input.unrolledModel, folders, true, true, true, true, o.p);
            input.ffh_unrolledNames = Program.FindFile(input.zipFilePathAndName + "\\" + input.unrolledNames, folders, true, true, true, true, o.p);
            input.ffh_rawModel = Program.FindFile(input.zipFilePathAndName + "\\" + input.rawModel, folders, true, true, true, false, o.p);  //this will not abort with error if file not found 

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
                if (Program.options.model_gams_scalar_data && modelGamsScalar.hasReadSomeData > 0)  //don't do if no data was found in scalar model
                {
                    //Get these modelGamsScalar.a values into databank
                    if (Globals.runningOnTTComputer) MessageBox.Show("Beware: scalar model data handled B");
                    modelGamsScalar.FromAToDatabankScalarModel(Program.databanks.GetFirst(), false);
                }

                modelGamsScalar.fix = new byte[modelGamsScalar.fixTemp.Length][];
                for (int i = 0; i < modelGamsScalar.fixTemp.Length; i++)
                {
                    modelGamsScalar.fix[i] = modelGamsScalar.fixTemp[i].storage;
                }
                modelGamsScalar.fixTemp = null;

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

                modelGamsScalar.fixTemp = new ByteArray[modelGamsScalar.fix.Length];
                for (int i = 0; i < modelGamsScalar.fix.Length; i++)
                {
                    modelGamsScalar.fixTemp[i] = new ByteArray();
                    modelGamsScalar.fixTemp[i].storage = modelGamsScalar.fix[i];
                }
            }
        }

        /// <summary>
        /// Read (parse) a .gms GAMS model, transforming it into Gekko-understandable equations.
        /// Calls ReadGamsEquation() for each equation. If allowAsssignments we can accept
        /// something like "y[t] = 2 * x[t];" so it does not have to be
        /// "e1[t] .. y[t] = 2 * x[t];" with double dots.
        /// </summary>
        /// <param name="textInputRaw"></param>
        /// <param name="fileName"></param>
        /// <param name="dependents"></param>
        /// <param name="o"></param>
        public static ModelGams ReadGamsModelHelper(bool allowAssignments, string textInputRaw, string fileName, Dictionary<DName, DName> dependents, bool dump, bool silent, Model model)
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
            GekkoDictionary<DName, List<ModelGamsEquation>> equationsByVarname = new GekkoDictionary<DName, List<ModelGamsEquation>>(Multidim2Comparer.IgnoreCase);
            GekkoDictionary<DName, List<ModelGamsEquation>> equationsByEqname = new GekkoDictionary<DName, List<ModelGamsEquation>>(Multidim2Comparer.IgnoreCase);

            List<string> problems = new List<string>();  //vars
            List<string> problems2 = new List<string>(); //eqs

            int counter = 0;

            //With allowAssignments == false, it is a little bit safer, since double dots ".." are used to identify 
            //equations. This is done by going left and right of the "..".
            //With allowAssignments == true, we use semicolon ";" to cut up the input string. This is done by going
            //left of the ";" to look for the previous ";" (or start of string).

            foreach (TokenHelper tok in tokens2.subnodes.storage)
            {
                if (tok.type == ETokenType.EOL)
                {
                    counter++;
                }

                bool good = false;
                if (allowAssignments)
                {
                    if (tok.s == ";")
                    {
                        good = true;
                    }                    
                }
                else
                {
                    if ((tok.s == "." && tok.Offset(1).s == "." && tok.Offset(1).leftblanks == 0) && !(tok.Offset(2).s == "\\" || tok.Offset(2).s == "/"))
                    {
                        good = true;
                    }
                }
                if (good)
                {
                    eqCounter = ReadGamsEquation(allowAssignments, sb1, sb2, eqCounter, equationsByVarname, equationsByEqname, tok, dependents, problems, problems2, dump);
                }
            }
            ModelGams modelGams = new ModelGams(model);
            modelGams.equationsByVarname = equationsByVarname;
            modelGams.equationsByEqname = equationsByEqname;            

            if (silent)
            {
                if (problems2.Count > 0)
                {
                    using (Note txt2 = new Note())
                    {
                        txt2.MainAdd(problems2.Count + " equation dublets in raw GAMS model file.");
                        txt2.MoreAdd("In the GAMS raw file (inside a scalar model .zip file: typically the file raw.gms), there are " + problems2.Count + " equation dublets. You may try to unpack raw.gms and issue the statement 'MODEL <gms> raw.gms;' too see the problematic equations. If nothing is done, Gekko will use the first occurrence of such dublet equations.");
                    }
                }            
            }
            else
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
                        txt2.MainAdd("There were the following variable problems while reading the model:");
                        txt2.MainNewLineTight();
                        foreach (string s in problems)
                        {
                            txt2.MainAdd("+++  " + s);
                            txt2.MainNewLineTight();
                        }
                    }
                    if (problems2.Count > 0)
                    {
                        txt2.MainAdd("There were the following equation problems while reading the model:");
                        txt2.MainNewLineTight();
                        foreach (string s in problems2)
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
        /// Read (parse) a .gms/.gmy GAMS equation/assignment, translating it into an equivalent Gekko equation/assignment.
        /// The resulting equation is put into equationsByVarname and equationsByEqname.
        /// Has quite a lot of try-catch.
        /// </summary>
        private static int ReadGamsEquation(bool allowAssignments, StringBuilder sb1, StringBuilder sb2, int eqCounter, Dictionary<DName, List<ModelGamsEquation>> equationsByVarname, Dictionary<DName, List<ModelGamsEquation>> equationsByEqname, TokenHelper tok, Dictionary<DName, DName> dependents, List<string> problems, List<string> problems2, bool dump)
        {
            TokenHelper lhsTokensGekko = null;
            ModelGamsEquation equation = null;
            DName eqnameGams = null;
            int i = -12345;
            List<DName> lhsVars = new List<DName>();
            List<EquationNameChunks> lhsVars2 = new List<EquationNameChunks>();
            List<DName> rhsVars = new List<DName>();
            List<EquationNameChunks> rhsVars2 = new List<EquationNameChunks>();            

            try
            {

                //if allowAssignments == true, we are at ";", else we are at "..".

                WalkTokensHelper wh = new WalkTokensHelper();

                int iEqStart = 0;

                if (allowAssignments)
                {
                    //now we search backwards for previous ";" (or start of tokens)
                    for (int i2 = -1; i2 > -int.MaxValue; i2--)
                    {
                        if (tok.Offset(i2) == null || tok.Offset(i2)?.s == ";")
                        {
                            iEqStart = i2 + 1;
                            break;
                        }
                    }
                }
                else
                {
                    //now we search backwards for start of line or a semicolon
                    for (int i2 = -1; i2 > -int.MaxValue; i2--)
                    {
                        if (tok.Offset(i2) == null || tok.Offset(i2).type == ETokenType.EOL || tok.Offset(i2).s == ";")
                        {
                            iEqStart = i2 + 1;
                            break;
                        }
                    }
                }

                i = iEqStart; //for allowAssignments this is previous ";", for !allowAssignments this is start of line

                //-----------------------------------------------
                //now we are ready for the equation definition
                //-----------------------------------------------

                //The equation is of this form:

                //e_pi(i,ds,t) $ (tx0(t) and d1i(i,ds,t)) .. pI(i,ds,t)*qI(i,ds,t) =E= vI(i,ds,t);

                //Tokenized in tree structure it looks like this:

                //e_pi(...) $ (...) .. pI(...)*qI(...) =E= vI(...);

                //NOTE: for allowAssignments we cheat and ignore ".." and stuff before them --> only handles pI(...) = 1/qI(...) * vI(...) type assignment.

                //So the following:
                // eqname
                // maybe a set parenthesis
                // maybe a dollar
                //     if so either a (...) or a variable with a (...)
                // a '..' always
                // a leftside until '=e='
                // a rightside after'=e=' until semicolon

                eqnameGams = null;
                string conditionalsGams = null;

                string setsGams = null;
                List<string> setsGamsList = new List<string>();
                string lhsGams = null;
                string rhsGams = null;
                TokenHelper lhsTokensGams = null;
                TokenHelper rhsTokensGams = null;

                string dollar = null;

                eqnameGams = new DNameSimplest(tok.Offset(i)?.s);
                
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
                            List<DName> vars = new List<DName>();
                            List<EquationNameChunks> vars2 = new List<EquationNameChunks>();
                            WalkTokensGekkoSyntax(list, wh2, vars, vars2, new GamsWalkerInfo());

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
                            }
                            i++;

                            string s8 = tok.Offset(i).ToStringTrim();
                            if (!(tok.Offset(i).SubnodesTypeParenthesisStart()))
                            {
                                new Error("Expected a (...) parenthesis instead of '" + s8 + "' , " + tok.Offset(i).LineAndPosText());
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
                }
                i++;
                i++;

                //now ready for the contents of the equation

                // "y[t] = x[t];" make sure there are not ".." in it
                // Here i is at "y" in y[t] and iSemi is at ";".
                // If no ".." we cannot have "=e=" but only "=".
                // If no ".." and no "=" we have an "expression" (only RHS).
                //

                //find lhs of equation -----------------------------------------
                int i1Start = i;

                List<string> eqsign = new List<string>() { "=", "e", "=" };

                int iEqual = tok.Search(i1Start, eqsign, false, false);

                if (iEqual == -12345)
                {
                    G.Warning("w1.1", "GAMS file: " + tok.Offset(i).LineAndPosText());                    
                    return eqCounter;
                }

                int i1End = iEqual - 1;
                int i2Start = i1End + eqsign.Count + 1;
                int iSemi = tok.Search(i2Start, new List<string>() { ";" }, false, false);

                if (iSemi == -12345)
                {
                    G.Warning("w1.2", "GAMS file: " + tok.Offset(i).LineAndPosText());                    
                    return eqCounter;
                }

                lhsGams = tok.OffsetInterval(i1Start, i1End).ToString().Trim();
                lhsTokensGams = tok.OffsetInterval(i1Start, i1End);

                rhsGams = tok.OffsetInterval(i2Start, iSemi - 1).ToString().Trim();
                rhsTokensGams = tok.OffsetInterval(i2Start, iSemi - 1);

                equation = new ModelGamsEquation();

                equation.nameGams = eqnameGams;
                equation.setsGams = setsGams;
                equation.setsGamsList = setsGamsList;
                equation.conditionalsGams = conditionalsGams;
                equation.lhsGams = lhsGams;
                equation.rhsGams = rhsGams;
                equation.lhsTokensGams = lhsTokensGams;
                equation.rhsTokensGams = rhsTokensGams;

                //Gekko syntax

                //VariableChops chops = new VariableChops();
                lhsTokensGekko = equation.lhsTokensGams.DeepClone(null);
                WalkTokensHandleParentheses(lhsTokensGekko); //changes '[' and '{' into '('
                WalkTokensHelper wt1Gekko = new WalkTokensHelper();                
                WalkTokensGekkoSyntax(lhsTokensGekko, wt1Gekko, lhsVars, lhsVars2, new GamsWalkerInfo());
                string lhsGekko = lhsTokensGekko.ToStringTrim();

                TokenHelper rhsTokensGekko = equation.rhsTokensGams.DeepClone(null);
                WalkTokensHandleParentheses(rhsTokensGekko); //changes '[' and '{' into '('
                WalkTokensHelper wt2Gekko = new WalkTokensHelper();                
                WalkTokensGekkoSyntax(rhsTokensGekko, wt2Gekko, rhsVars, rhsVars2, new GamsWalkerInfo());
                string rhsGekko = rhsTokensGekko.ToStringTrim();

                if (true)
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

                }

                if (true)
                {
                    equation.lhs = lhsGekko;
                    equation.rhs = rhsGekko;
                    equation.lhsVars= lhsVars;
                    equation.rhsVars= rhsVars;
                    equation.lhsVarsChunks= lhsVars2;
                    equation.rhsVarsChunks= rhsVars2;

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
            }
            catch
            {
                //Hopefully will not happen, but more so a bad line does not crash the whole thing
                G.Warning("w1.7", "Parsing error in GAMS file: " + tok.Offset(i).LineAndPosText());                
                return eqCounter;
            }
            
            bool fromList = false;
            DName lhsVariable = ReadGamsModelGetLhsNameAndStoreEquation(equationsByVarname, equationsByEqname, lhsTokensGekko, equation, eqnameGams, dependents, problems, problems2, lhsVars, lhsVars2, rhsVars, rhsVars2, ref fromList);
            string s = null;
            if (fromList) s = ", designated from list";
            if (lhsVariable == null) lhsVariable = new DNameSimplest("[not identified]");
            sb1.AppendLine("--> " + lhsVariable + " (dependent" + s + ")");
            sb1.AppendLine();
            sb1.AppendLine("----------------------------------------------------------------------------------------------------------------");
            sb1.AppendLine();

            eqCounter++;
            return eqCounter;
        }

        /// <summary>
        /// Tries to identify what is the LHS variable in the GAMS equation, and puts this into dictionaries for later retrieval by variable name or equation name.
        /// The method reacts to option model gams dep method = lhs|eqname, and also reacts to a #dependents list.
        /// </summary>
        private static DName ReadGamsModelGetLhsNameAndStoreEquation(Dictionary<DName, List<ModelGamsEquation>> equationsByVarname, Dictionary<DName, List<ModelGamsEquation>> equationsByEqname, TokenHelper lhsTokensGams2, ModelGamsEquation equation, DName eqnameGams, Dictionary<DName, DName> dependents, List<string> problems, List<string> problems2, List<DName> lhsVars, List<EquationNameChunks> lhsVars2, List<DName> rhsVars, List<EquationNameChunks> rhsVars2, ref bool fromList)
        {
            DName lhs = null;

            if (G.Equal(Program.options.model_gams_dep_method, "lhs"))
            {
                Program.GetLhsVariable(lhsTokensGams2, ref lhs);
            }
            else if (G.Equal(Program.options.model_gams_dep_method, "eqname") || G.Equal(Program.options.model_gams_dep_method, "both"))
            {
                string[] ss = SplitEqName(eqnameGams.ToString());
                if (ss.Length > 1)
                {
                    if (!G.IsIdent(ss[1]))  //we use the e_{here}_..._..._... part
                    {
                        G.Warning("w1.6", "Eqname '" + eqnameGams + "': could not resolve variable name");
                    }
                    lhs = new DNameSimplest(ss[1]);
                }
                else
                {
                    lhs = new DNameSimplest(ss[0]);
                }                
            }
            else
            {
                new Error("option model gams dep method = lhs|eqname|both.");
            }

            DName d = null; if (dependents != null) dependents.TryGetValue(eqnameGams, out d);
            DName varnameFound = null;
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
                    equationsByVarname[varnameFound].Add(equation);  //can have more than one eq with same lhs variable
                }
                else
                {
                    List<ModelGamsEquation> e2 = new List<ModelGamsEquation>();
                    e2.Add(equation);
                    equationsByVarname.Add(varnameFound, e2);
                }

                if (equationsByEqname.ContainsKey(eqnameGams))
                {
                    problems2.Add("Equation '" + eqnameGams + "' appears multiple times: first occurrence is used.");
                }
                else
                {
                    List<ModelGamsEquation> e2 = new List<ModelGamsEquation>();
                    e2.Add(equation);
                    equationsByEqname.Add(eqnameGams, e2);
                }
            }
            return varnameFound;
        }

        /// <summary>
        /// Splits "e_a_b" into ["e", "a", "b"]
        /// </summary>
        /// <param name="eqnameGams"></param>
        /// <returns></returns>
        public static string[] SplitEqName(string eqnameGams)
        {
            string[] delimiters = new string[] { "__", "_" };
            if (eqnameGams.Contains("___"))
            {
                G.Warning("w1.3", "Eqname '" + eqnameGams + "': did not expect '___' substring in name");
            }
            string[] ss = eqnameGams.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            if (ss.Length <= 1)
            {
                G.Warning("w1.4", "Eqname '" + eqnameGams + "': did not find any '_' separators");
            }
            if (!G.Equal(ss[0], "e"))
            {
                G.Warning("w1.5", "Eqname '" + eqnameGams + "': expected it to start with 'e_'");
            }
            return ss;
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
        public static ModelGamsEquation DecompEvalGams(DName eqname, DName varname, Model model)
        {
            List<ModelGamsEquation> eqs = null;
            ModelGamsEquation found = null;
            if (eqname != null)
            {
                eqs = GetGamsEquationsByEqname(eqname, model);
                if (eqs == null || eqs.Count == 0)
                {
                    new Error("Equation '" + eqname + "' was not found");
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
                }
                if (eqs.Count > 1)
                {
                    G.Warning("w7.1", "Variable '" + varname + "' appears in several equations, first one is picked");
                }
                found = eqs[0];  //#820948324: pick the first one, a variable name may point to several equations, for instance if y is present on the lhs in several equations.
            }

            string rhs = found.rhs.Trim();
            string lhs = found.lhs.Trim();

            string s1 = Decomp.EquationLhsRhs(lhs, rhs, true) + ";";  //this is a generic method, not just a GAMS method

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
        

        

        private static List<ModelGamsEquation> GetGamsEquationsByEqname(DName variable, Model model)
        {            
            if (model.modelGams.equationsByEqname == null || model.modelGams.equationsByEqname.Count == 0)
            {
                new Error("No GAMS equations found");
            }
            if (variable.HasTime() && variable.GetTime().freq == EFreq.Lag && variable.GetTime().super == 0)
            {
                variable = variable.RemoveTime(); // new  new DNameNoTimeOrLag(variable.GetName(), variable.GetTime().freq, variable.GetIndexesExceptTime());
            }
            List<ModelGamsEquation> eqs = null; model.modelGams.equationsByEqname.TryGetValue(variable, out eqs);
            return eqs;
        }

        public static List<ModelGamsEquation> GetGamsEquationsByVarname(DName variable, Model model)
        {
            if (model.modelGams.equationsByVarname == null || model.modelGams.equationsByVarname.Count == 0)
            {
                new Error("No GAMS equations found");
            }
            List<ModelGamsEquation> eqs = null; model.modelGams.equationsByVarname.TryGetValue(variable, out eqs);
            return eqs;
        }

        public static Tuple<Dictionary<DName, DName>, StringBuilder> GetDependentsGams(IVariable opt_dep)
        {
            Dictionary<DName, DName> dependents = new GekkoDictionary<DName, DName>(Multidim2Comparer.IgnoreCase);
            //hashHelper: will get the format: "--- dependents ---<NL>a;b;c<NL>c,d,e<NL>"
            //the dependents list does not change the model per se, but it changes how DISP and other statements
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
                    }
                    string lhs = ss[0];
                    for (int i = 1; i < ss.Count; i++)
                    {
                        //The ss list has this form for each line:
                        //qG; E_qG; E_qG_tot    --> first the lhs name, then the equations where it is a lhs variable
                        //Since each equation can only have 1 lhs, the eqnames (E_qG etc.) can at most appear 1 time in
                        //the ss list.

                        //qwerty, hmmm ?
                        DName temp = null; dependents.TryGetValue(new DNameSimplest(ss[i]), out temp);
                        if (temp != null)
                        {
                            new Error("#dependents sublist line " + c + ": The equation '" + ss[i] + "' already assigns '" + temp + "' as lhs");
                        }
                        dependents.Add(new DNameSimplest(ss[i]), new DNameSimplest(lhs));
                    }
                }
            }

            return new Tuple<Dictionary<DName, DName>, StringBuilder>(dependents, hashHelper);
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
        public static void WalkTokensGekkoSyntax(TokenList nodes, WalkTokensHelper th, List<DName>vars, List<EquationNameChunks>vars2, GamsWalkerInfo info)
        {
            foreach (TokenHelper child in nodes.storage)
            {
                WalkTokensGekkoSyntax(child, th, vars, vars2, info);
            }
        }

        /// <summary>
        /// Actual translation of GAMS equations into Gekko statements.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="th"></param>
        public static void WalkTokensGekkoSyntax(TokenHelper node, WalkTokensHelper th, List<DName> vars, List<EquationNameChunks> vars2, GamsWalkerInfo info)
        {
            //Performs these transformations:
            //- GAMS functions are not touched (log, etc)
            //-      but sqr() becomes sqrt()
            //- sum() function has # put in on sets
            //- parameter t is removed, and lags/leads like t-1 are transformed into [-1] etc. So x(a, t) --> x[#a], and x(t) --> x not x().
            //- tBase handled, x[i, tBase] --> x[#i][%tbase]
            //- Hardcoded years handled: x[i, '2018'] --> x[#i][2018]
            //- strings have quotes removed, x['a'] --> x[a]
            //- stuff like a.val becomes #a.val(), whereas t.val is ignored for now
            //- sameas(i,j) and sameas(i,'a') become #i==#j and #i=='a'
            //- single '=' becomes '=='
            //- all t or t+1 or t-1 etc. are recorded, together with any tBase (hmm, not sure)

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

                        string fullName = node.ToStringTrim() + nextNode.ToStringTrim();

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
                            // -------------------------------------
                            // This is probably a variable
                            // -------------------------------------

                            //first we check for stuff like a15t100(a), where a15t100 is a set, not a variable
                            //so it should be #a15t100[#a], not a15t100[#a]
                            //

                            List<TokenHelperComma> split = nextNode.SplitCommas(true);
                            bool isSetWithIndexer = CheckIfVarIsASet(node.s, th);
                            if (isSetWithIndexer) node.s = "#" + node.s;

                            //Beware, this will also put int sets like a18t100(a). These are filtered out later on,
                            //but kept here for simplicity.
                            GetVariableChunks(node, vars, vars2, nextNode, split, info);                            

                            bool removeParenthesis = false;

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

                                    bool looksLikeFixedYear = false;  //we have to do this analysis here, to get it treated together with ETokenType.Word
                                    if (helper.list[0].type == ETokenType.QuotedString)
                                    {
                                        string stripped = G.StripQuotes(helper.list[0].s);
                                        if (G.IsInteger(stripped))
                                        {
                                            if (G.IsYear(int.Parse(stripped)))
                                            {
                                                looksLikeFixedYear = true;
                                            }
                                        }
                                    }

                                    if (helper.list[0].type == ETokenType.Word || looksLikeFixedYear)
                                    {
                                        //helper.list[0] is the single token

                                        if (iSplit == split.Count - 1 && (G.Equal(helper.list[0].s, th.t) || G.Equal(helper.list[0].s, th.tBase) || looksLikeFixedYear))
                                        {
                                            //t or tBase or '2018' (or other hardcoded year) at last position
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
                                            else if (G.Equal(helper.list[0].s, th.tBase) || looksLikeFixedYear)
                                            {
                                                //tBase or '2018'
                                                //x(i, tBase) --> x[#i][%tBase]
                                                //x(i, '2018') --> x[#i][2018]
                                                //we need to transform one []-subnode into two consequtive
                                                //see also #89075203489

                                                TokenHelper nextNode2 = new TokenHelper(); nextNode2.subnodes = new TokenList();
                                                //[%tBase]
                                                nextNode2.subnodes.storage.Add(new TokenHelper("["));
                                                if (looksLikeFixedYear)
                                                {
                                                    //x(i, '2018') --> x[#i][2018]
                                                    nextNode2.subnodes.storage.Add(new TokenHelper(G.StripQuotes(helper.list[0].s)));
                                                }
                                                else
                                                {
                                                    //x(i, tBase) --> x[#i][%tBase]
                                                    nextNode2.subnodes.storage.Add(new TokenHelper(Globals.symbolScalar + helper.list[0].s));
                                                }
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
                                                    //x(i, '2018') --> x[#i][2018], but x('2018') --> x[2018]
                                                }

                                                int id = nextNode.id;
                                                TokenHelper parent = nextNode.parent;

                                                parent.subnodes.storage.RemoveAt(id);
                                                parent.subnodes.storage.Insert(id, nextNode2);
                                                parent.subnodes.storage.Insert(id, nextNode1);
                                                parent.OrganizeSubnodes();  //to get the id's and pointers to parent ok

                                            }
                                            else throw new GekkoException("Error_Walk_Tokens");
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
                if (node.id > 0 && node.parent.subnodes[node.id - 1].s == "$") info.isInsideDollar = true;                
                if (node.id > 0 && node.parent.subnodes[node.id - 1].s == "sum") info.isInsideSum = true;                
                for (int i = 0; i < node.subnodes.storage.Count; i++)  //the count may increase, because subnodes may be added dynamically (translating x[i, t-1] into x[#i][-1])
                {
                    WalkTokensGekkoSyntax(node.subnodes.storage[i], th, vars, vars2, info);
                }
            }
        }

        /// <summary>
        /// Helper method used for sorting in the FIND window (trying to show the LHS equation first)
        /// </summary>
        /// <param name="node"></param>
        /// <param name="vars"></param>
        /// <param name="vars2"></param>
        /// <param name="nextNode"></param>
        /// <param name="split"></param>
        private static void GetVariableChunks(TokenHelper node, List<DName> vars, List<EquationNameChunks> vars2, TokenHelper nextNode, List<TokenHelperComma> split, GamsWalkerInfo info)
        {
            vars.Add(Program.DName_HACK1(node.ToString() + nextNode.ToString()));  //pretty raw version, as it is
            EquationNameChunks vars2a = new EquationNameChunks();
            vars2a.info = info;
            string name = node.ToString();
            string[] ss = name.Split('_');
            foreach (string s in ss)
            {
                vars2a.chunks.Add(s.Replace(" ", ""));
            }

            //now we look at the arguments, x(a1, a2, 's', t) or x(a1, a2, 's', t-1) or x(a1, a2, 's')                            

            foreach (TokenHelperComma thc in split)
            {
                string s7 = thc.list.ToString().Replace(" ", "");
                vars2a.chunks.Add(s7);                
            }
            vars2.Add(vars2a);
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
            string trueHash = Program.GetMD5Hash(Stringlist.ExtractTextFromLines(lines).ToString(), null, null, null); //Pretty unlikely that two different gams files could produce the same hash.
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

                gdxcs gdx = null;
                try
                {
                    gdx = new gdxcs(gamsDir, ref msg);  //it seems ok if gamsSysDir = "", then it will autolocate it (but there may be a 64-bit problem...)
                }
                catch
                {
                    new Error("Could not create GAMS/gdx environment", false);
                    GdxErrorMessage();
                    throw new GekkoException();
                }
                
                if (msg != string.Empty)
                {
                    G.Warning("w36.2", null);
                }

                if (true)
                {
                    rc = gdx.gdxOpenRead(file, ref errNr);
                    if (errNr != 0)
                    {                        
                        new Error("gdx io error");                        
                    }
                    int timeIndex = -12345;
                    int uelCount = -1; int uelHighest = -1;
                    gdx.gdxUMUelInfo(ref uelCount, ref uelHighest);
                    if (uelHighest != 0)
                    {
                        new Error("Internal UEL problem (GDX)");
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
                                new Error("Gdx error, starting the reader");
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
                                if (Program.options.gams_time_detect_auto)
                                {
                                    EFreq?[] couldBeTime = new EFreq?[gdxDimensions];
                                    for (d = 0; d < gdxDimensions; d++) couldBeTime[d] = null;

                                    //Tasting the variable/parameter to see if time is there...
                                    if (gdx.gdxDataReadRawStart(i, ref nrRecs) == 0) new Error("Gdx error, starting the reader");
                                    while (gdx.gdxDataReadRaw(ref index, ref values, ref n) != 0)
                                    {
                                        //a new record
                                        for (d = 0; d < gdxDimensions; d++)
                                        {
                                            if (couldBeTime[d] != null && couldBeTime[d] == EFreq.None) continue;  //no need to see more
                                            GekkoTime gt = GekkoTime.tNull;
                                            string x = uel[index[d]];
                                            if (x.Length >= 4)
                                            {
                                                int y = G.IntParse(x.Substring(0, 4));
                                                if (y != -12345)
                                                {
                                                    //first 4 chars looks like a year                                                        
                                                    try
                                                    {
                                                        //we are conditioning/screening before this try for efficienty reasons
                                                        gt = GekkoTime.FromStringToGekkoTime(x, false, false, false);
                                                    }
                                                    catch { }

                                                }
                                            }

                                            if (gt.IsNull())
                                            {
                                                couldBeTime[d] = EFreq.None;  //signals fail
                                            }
                                            else
                                            {
                                                if (couldBeTime[d] == null)
                                                {
                                                    couldBeTime[d] = gt.freq;  //first time a date is ecountered, put it in
                                                }
                                                else if (couldBeTime[d] != gt.freq)
                                                {
                                                    couldBeTime[d] = EFreq.None;  //signals fail
                                                }
                                            }
                                        }
                                    }
                                    gdx.gdxDataReadDone();

                                    int counter = 0;
                                    int dFound = -12345;
                                    for (d = 0; d < gdxDimensions; d++)
                                    {
                                        if (couldBeTime[d] != null && couldBeTime[d] != EFreq.None)
                                        {
                                            counter++;
                                            dFound = d;
                                        }
                                    }

                                    if (counter == 1)
                                    {
                                        timeDimNr = dFound;
                                    }
                                }                                
                            }

                            if (timeDimNr == -12345)
                            {
                                if (varType == 1) paramsWithoutTimeDimensionCounter.Add(varName);
                                else if (varType == 2) varsWithoutTimeDimensionCounter.Add(varName);
                            }

                            if (gdx.gdxDataReadRawStart(i, ref nrRecs) == 0) new Error("Gdx error, starting the reader");                            

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

                            int gdxElementCounter = 0;
                            int gdxRealElementCounter = 0;

                            while (gdx.gdxDataReadRaw(ref index, ref values, ref n) != 0)
                            {
                                //Reading the dimension coordinates

                                gdxElementCounter++;                                

                                int tt = -12345;
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

                                bool equal = Program.CompareDims(oldDims, dims);

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
                                        MultidimElement mmi = new MultidimElement(dims.ToArray(), tsSuperseries);
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

                                if (value == 0d)
                                {
                                    //not counted
                                }
                                else
                                {
                                    gdxRealElementCounter++;  //skips 0's
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

                            if (gdxRealElementCounter >= Program.options.gams_trim)  //option is 0 per default
                            {
                                //If this is skipped, the tsSuperseries just dies with its data (not transferred to the ram Gekko databank)
                                //Note: will not just skip vars/params with 0 elements: elements that are clean 0 or GAMS eps are skipped, too.
                                databank.AddIVariable(tsSuperseries.name, tsSuperseries);
                            }
                            else
                            {
                                //skip it!
                            }
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

                        using (Warning txt = new Warning(EWarningType.UsingWithTypeId, "w36.1"))
                        {
                            //#0897aef todo
                            txt.MainAdd((paramsWithoutTimeDimensionCounter.Count() + varsWithoutTimeDimensionCounter.Count()) + " variables/parameters without explicit time domain/dimension encountered");
                            txt.MoreAdd("There were " + paramsWithoutTimeDimensionCounter.Count() + " parameters and " + varsWithoutTimeDimensionCounter.Count() + " variables without a time dimension set '" + Program.options.gams_time_set + "' assigned as domain (" + G.GetLinkAction("show", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ").");
                            txt.MoreAdd("This is ok if the GAMS variables/parameters are really timeless, but if not, there is a problem.");
                            txt.MoreNewLine();
                            txt.MoreAdd("A quick fix regarding this can be to simply set 'option gams time detect auto = yes;', in which case Gekko tries to detect");
                            txt.MoreAdd("a time dimension automatically, even if the dimension is defined over the universal set '*'. To do this, Gekko looks for time-like elements like, say, '2020', '2021', '2022' etc. in order");
                            txt.MoreAdd("to guess if a particular dimension is a time dimension. This usually works pretty well.");
                            txt.MoreNewLine();
                            txt.MoreAdd("If you do not want to use 'option gams time detect auto', the time dimensions need to be assigned a time domain (for instance defined over a set named 't'). For instance, if in GAMS IDE or GAMS Studio a variable x is shown as x[*, *], ");
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
                    new Error("Gdx io error");
                }
            }
            catch (Exception e)
            {
                new Error("The external GAMS gdx reader failed with an unexpected error.");
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

        public static void WriteGdx(Databank databank, GekkoTime t1, GekkoTime t2, string pathAndFilename, List<Tuple<string, IVariable>> list2)
        {
            //merge and date truncation:
            //do this by first reading into a Gekko databank, and then merge that with the merge facilities from gbk read

            try
            {

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

                double[] d = new double[1];  //used for setss

                int syCnt = 0, uelCnt = 0;

                //GAMSWorkspace ws = null;

                List<string> timelessProblems = new List<string>();  //only used in rare cases

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

                gdxcs gdx = null;

                try
                {
                    gdx = new gdxcs(gamsDir, ref Msg);  //it seems ok if gamsSysDir = "", then it will autolocate it (but there may be a 64-bit problem...) //GdxFast gdx = new gdxcs(Sysdir, ref Msg);
                }
                catch
                {
                    new Error("Could not create GAMS/gdx environment", false);
                    GdxErrorMessage();
                    throw new GekkoException();
                }

                if (Msg != string.Empty)
                {
                    G.Warning("w43.1", null);
                }

                if (true)
                {
                    if (Globals.gdxReaderDebug)
                    {
                        new Writeln("Grane1 --> pathAndFilename = " + pathAndFilename);
                    }

                    gdx.gdxOpenWrite(pathAndFilename, "Gekko", ref ErrNr);
                    if (ErrNr != 0)
                    {
                        new Error("GAMS gdx write error number " + ErrNr);
                    }

                    foreach (Tuple<string, IVariable> tup in list2)
                    {
                        IVariable iv = tup.Item2; // O.GetIVariableFromString(inputVariableName, O.ECreatePossibilities.NoneReportError, true);

                        string name = tup.Item1;// bnv.s2;
                        string nameWithoutFreq = G.Chop_GetName(name);

                        if (iv.Type() == EVariableType.Series)
                        {

                            Series ts = iv as Series;

                            GekkoTime t1Timeless = GekkoTime.tNull; //only used in rare cases
                            GekkoTime t2Timeless = GekkoTime.tNull; //only used in rare cases

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
                                    //Mix of timeless and normal
                                    //new Error("The array-timeseries " + ts.name + " has subseries that are both timeless and non-timeless --> cannot write to GDX.");

                                    foreach (IVariable iv2 in ts.dimensionsStorage.storage.Values)
                                    {
                                        Series sub = iv2 as Series;
                                        if (sub.type != ESeriesType.Timeless)
                                        {
                                            GekkoTime tReal1 = sub.GetRealDataPeriodFirst();
                                            if (!tReal1.IsNull())
                                            {
                                                if (t1Timeless.IsNull() || tReal1.StrictlySmallerThan(t1Timeless)) t1Timeless = tReal1;
                                            }
                                            GekkoTime tReal2 = sub.GetRealDataPeriodLast();
                                            if (!tReal2.IsNull())
                                            {
                                                if (t2Timeless.IsNull() || tReal2.StrictlyLargerThan(t2Timeless)) t2Timeless = tReal2;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (ntimeless > 0) timeDimension = 0;
                                }
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
                                new Error("Internal GAMS/gdx problem (variable '" + tup.Item1 + "'). It may be a name collision problem, for instance writing the series 'i' and the list '#i'.");
                            }

                            gdx.gdxSystemInfo(ref syCnt, ref uelCnt);

                            if (gdx.gdxSymbolSetDomainX(syCnt, domains) == 0)
                            {
                                new Error("Could not write domain names (gdxSymbolSetDomainX), variable '" + tup.Item1 + "'");
                            }

                            if (ts.type == ESeriesType.ArraySuper)
                            {
                                foreach (KeyValuePair<MultidimElement, IVariable> kvp in ts.dimensionsStorage.storage)
                                {
                                    string[] ss = kvp.Key.storage;
                                    WriteGdxHelper2(t1, t2, t1Timeless, t2Timeless, hasPrefix, gdx, kvp.Value as Series, ss, gdxValues, timelessProblems);
                                }
                            }
                            else
                            {
                                //normal timeseries
                                WriteGdxHelper2(t1, t2, GekkoTime.tNull, GekkoTime.tNull, hasPrefix, gdx, ts, new string[0], gdxValues, null);
                            }

                            if (gdx.gdxDataWriteDone() == 0)
                            {
                                new Error("GAMS gdx did not terminate properly, variable '" + tup.Item1 + "'.");
                            }
                            counterVariables++;
                        }
                        else if (iv.Type() == EVariableType.List)
                        {
                            try
                            {
                                List l = iv as List;

                                int nTuples = 1;
                                foreach (IVariable x in l.list)
                                {
                                    if (x.Type() == EVariableType.List)
                                    {
                                        nTuples = (x as List).list.Count;
                                        break;
                                    }
                                }

                                if (nTuples > 1)
                                {
                                    bool ok = true;
                                    for (int ii = 0; ii < 2; ii++)
                                    {
                                        //Runs it 2 times: first time just to see if it is well-defined
                                        //Data is constructed 2 times, we live with that for simplicity
                                        if (ii == 1 && ok == true)
                                        {
                                            if (gdx.gdxDataWriteStrStart(nameWithoutFreq.Replace(Globals.symbolCollection.ToString(), ""), "", nTuples, gamsglobals.dt_set, 0) == 0)
                                            {
                                                new Error("Internal GAMS/gdx problem (variable '" + tup.Item1 + "'). It may be a name collision problem, for instance writing the series 'i' and the list '#i'.");
                                            }
                                        }
                                        foreach (IVariable x in l.list)
                                        {
                                            if (x.Type() == EVariableType.List)
                                            {
                                                List<IVariable> m = (x as List).list;
                                                List<string> temp = new List<string>();
                                                if (nTuples != m.Count)
                                                {
                                                    ok = false;
                                                }
                                                foreach (IVariable y in m)
                                                {
                                                    if (y.Type() == EVariableType.String)
                                                    {
                                                        temp.Add(O.ConvertToString(y));
                                                    }
                                                    else
                                                    {
                                                        //TODO: Handle vals that are ints, maybe with leading zeroes
                                                        ok = false;
                                                    }
                                                }
                                                if (ii == 1 && ok == true)
                                                {
                                                    if (gdx.gdxDataWriteStr(temp.ToArray(), d) == 0)
                                                    {
                                                        new Error("Problem writing set (list) element for gdx, variable '" + tup.Item1 + "'");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ok = false;
                                            }
                                        }
                                        if (ii == 1 && ok == true)
                                        {
                                            if (gdx.gdxDataWriteDone() == 0)
                                            {
                                                new Error("GAMS gdx did not terminate properly, variable '" + tup.Item1 + "'.");
                                            }
                                            exportedSets++;
                                        }
                                    }
                                    if (ok == false) skippedSets++;
                                }
                                else
                                {
                                    //Old code

                                    if (gdx.gdxDataWriteStrStart(nameWithoutFreq.Replace(Globals.symbolCollection.ToString(), ""), "", nTuples, gamsglobals.dt_set, 0) == 0)
                                    {
                                        new Error("Internal GAMS/gdx problem (variable '" + tup.Item1 + "'). It may be a name collision problem, for instance writing the series 'i' and the list '#i'.");
                                    }

                                    string[] temp = Stringlist.GetListOfStringsFromListOfIvariables(l.list.ToArray());

                                    foreach (string s in temp)
                                    {
                                        if (gdx.gdxDataWriteStr(new string[] { s }, d) == 0)
                                        {
                                            new Error("Problem writing set (list) element for gdx, variable '" + tup.Item1 + "'");
                                        }
                                    }

                                    if (gdx.gdxDataWriteDone() == 0)
                                    {
                                        new Error("GAMS gdx did not terminate properly, variable '" + tup.Item1 + "'.");
                                    }
                                    exportedSets++;
                                }
                            }
                            catch
                            {
                                skippedSets++;
                            }
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
                if (skippedSets > 0) new Note(skippedSets + " lists could not be exported as sets (may not conform to GAMS standard)");
                if (timelessProblems.Count > 0)
                {
                    Action<GAO> a = (gao) =>
                    {
                        string s = null;
                        s += G.NL; //to avoid annoying visible blank
                        s += "The following " + timelessProblems.Count + " subseries are timeless in the Gekko databank, ";
                        s += "but since they are inside an array-series with mixed timeless and non-timeless (normal) subseries, these ";
                        s += "timeless subseries are converted to normal timeseries in the gdx file. The data period for ";
                        s += "this conversion reflects the data period of the normal subseries inside the particular array-series.";
                        s += G.NL;
                        s += G.NL;
                        s += "The converted subseries are:";
                        s += G.NL;
                        s += G.NL;
                        foreach (string ss in timelessProblems)
                        {
                            s += ss + G.NL; //funny break in output tab for the first of these, strange...
                        }
                        s += G.NL;
                        Gui.gui.tabControl1.SelectedTab = Gui.gui.tabPageOutput;
                        O.Cls("output");
                        G.Writeln(s, ETabs.Output);
                    };
                    new Note(timelessProblems.Count + " timeless array-subseries were converted to normal timeseries (" + G.GetLinkAction("more", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ").");
                }
            }
            catch (Exception e)
            {
                new Error("The external GAMS gdx writer failed with an unexpected error.", false);
                throw;
            }
        }

        public static void WriteGdxSlow(Databank databank, GekkoTime t1, GekkoTime t2, string pathAndFilename, List<Tuple<string, IVariable>> list2)
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
                //for Python and R, some users had problems with the system shell calling python.exe and r.exe and had
                //to use .bat files. But the following use is different, not calling the system shell like that.
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

            foreach (Tuple<string, IVariable> tup in list2)
            {
                IVariable iv = tup.Item2; //O.GetIVariableFromString(bnv.s1, O.ECreatePossibilities.NoneReportError, true);

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
                        //Note: this is fixed in WriteGdx(), the fast version
                        new Error("The array-timeseries " + ts.name + " has subseries that are both timeless and non-timeless --> cannot write to GDX.");
                    }
                    if (ntimeless > 0) timeDimension = 0;
                    //if ntimeless + nnontimeless == 0 it will be assumed to have time-dim in GAMS --> hard to know.
                }

                string[] domains = new string[ts.dimensions + timeDimension];
                for (int i = 0; i < domains.Length; i++) domains[i] = "*";
                if (timeDimension == 1) domains[domains.Length - 1] = Program.options.gams_time_set;  //we alway put the t domain last

                GAMSVariable gvar = db.AddVariable(tup.Item1, VarType.Free, label, domains);

                counterVariables = WriteGdxHelperSlow(t1, t2, usePrefix, counterVariables, ts, gvar);

            }

            db.Export(pathAndFilename);

            G.Writeln2("Exported " + counterVariables + " variables to " + pathAndFilename + " (" + G.SecondsFormat((DateTime.Now - t00).TotalMilliseconds) + ")");
            if (timelessCounter > 0) new Note(timelessCounter + " timeless timeseries skipped");
        }

        private static void WriteGdxHelper2(GekkoTime t1, GekkoTime t2, GekkoTime t1Timeless, GekkoTime t2Timeless, bool usePrefix, gdxcs gdx, Series ts2, string[] ss, double[] gdxValues, List<string> timelessProblems)
        {
            if (ts2.type == ESeriesType.Timeless && t1Timeless.IsNull())
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
                //May be timeless, and if so must be converted to non-timeless here
                //Will only be timeless here if t1Timeless and t2Timeless are not null.

                GekkoTime gt1 = t1;
                GekkoTime gt2 = t2;

                if (ts2.type == ESeriesType.Timeless)
                {
                    gt1 = t1Timeless;
                    gt2 = t2Timeless;
                    timelessProblems.Add(ts2.GetNameWithoutCurrentFreq(true));
                }
                else
                {                    
                    if (t1.IsNull())
                    {
                        gt1 = ts2.GetRealDataPeriodFirst();
                        gt2 = ts2.GetRealDataPeriodLast();
                    }
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
                foreach (KeyValuePair<MultidimElement, IVariable> kvp in ts.dimensionsStorage.storage)
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


        private static bool GamsIsFixed(double[] values, double value)
        {
            return value == values[gamsglobals.val_lower] || value == values[gamsglobals.val_upper];
        }

        private static void GdxErrorMessage()
        {
            using (Note n = new Note())
            {
                n.MainAdd("In order for Gekko to read .gdx files, you need to have GAMS installed on your pc (the GAMS version does not need to be licenced, and an expired GAMS version may possibly work, too).");
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
                        err.MainAdd("*** ERROR: Gdx file (GAMS) failed. Could not locate GAMS (GAMSWorkspace problem).");
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

        /// <summary>
        /// 0 --> "x1[a]", 1 --> "x1[b]"      
        /// </summary>
        public DName[] dict_FromANumberToVarName = null;

        /// <summary>
        /// --> "x1[a]", 1 --> "x1[b]"
        /// </summary>
        public Dictionary<DName, int> dict_FromVarNameToANumber = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);
        public GekkoDictionary<string, int> dict_Constants = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public double[][] a = null;
        public byte[][] fix = null;  //fixed varibles, around 2.5 MB for 85 years and 30.000 variables. Not too much.
        public List<List<int>> b = new List<List<int>>();
        public List<double> c = new List<double>();
        public List<List<int>> d = new List<List<int>>();
        public List<int> eqPointers = new List<int>();

        /// <summary>
        /// 0 --> e"[a,2001]"´, 1 --> "e[b,2001]"
        /// </summary>
        public DName[] dict_FromEqNumberToEqName = null;

        /// <summary>
        /// "e[a,2001]" --> 0, "e[b,2001]" --> 1
        /// </summary>
        public Dictionary<DName, int> dict_FromEqNameToEqNumber = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);

        /// <summary>
        /// 0 --> "x1[a,2001]", 1 --> "x1[b,2001]"
        /// </summary>
        public DName[] dict_FromVarNumberToVarName = null;

        /// <summary>
        ///  //"x1[a,2001]" --> 0, "x1[b,2001]" --> 1
        /// </summary>
        public Dictionary<DName, int> dict_FromVarNameToVarNumber = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);

        /// <summary>
        /// //0 --> "e"
        /// </summary>
        public DName[] dict_FromEqChunkNumberToEqName = null;

        /// <summary>
        /// //"e" --> 0
        /// </summary>
        public Dictionary<DName, int> dict_FromEqNameToEqChunkNumber = new Dictionary<DName, int>(Multidim2Comparer.IgnoreCase);

        /// <summary>
        /// //0 --> 0, 1 --> 0
        /// </summary>
        public int[] dict_FromEqNumberToEqChunkNumber = null; 

        public bool[] isTimeless = null;

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

