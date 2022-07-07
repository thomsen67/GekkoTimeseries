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
using ProtoBuf;
using ProtoBuf.Meta;

namespace Gekko.Parser.Frm
{
    class ParserFrmCompileAST
    {
        public static void ParserFrmOrderAndCompileAST(ECompiledModelType modelType, bool isCalledFromModelStatement, bool isFix)
        {
            //seems modelType is not used at all
            bool newM2 = false;
            DateTime t0 = DateTime.Now;
            string cacheKey = GetCacheKey(isFix);

            if (Program.model.modelGekko.m2cache.lru.ContainsKey(cacheKey))  //MODEL statement should always issue a real compile, because in that case, Program.model.modelGekko.m2 is newly created
            {
                Program.model.modelGekko.m2 = (Model2)Program.model.modelGekko.m2cache.lru[cacheKey];
                G.WritelnGray("¤¤¤ Got in cache: " + cacheKey);
            }
            else
            {

                G.WritelnGray("¤¤¤ Has to do .m2 stuff: " + cacheKey);
                newM2 = true;
                Program.model.modelGekko.m2 = new Model2();  //deleting everything here, this is most safe rather than reusing the object

                //this runs very fast
                
                SolveOrdering.EndogenizeExogenizeStuff(isFix); //depends upon which endo/exo variables are set

                //takes about 0.6 sec on dec09

                SolveOrdering.FeedbackOrderingStuff(modelType, isCalledFromModelStatement); //depends upon which endo/exo variables are set
                
            }

            //The .m2 object is in principle recreated each time this method is called (for instance because of ENDO/EXO statement),
            //or with a MODEL statement. But since there is a cache, it will often be found there if it is because of ENDO/EXO.
            //For each modelType (Gauss, GaussFailSafe, Res, Newton, After, Unknown) there is a dedicated .dll in .m2. If
            //this is missing, it will be made in the method below.
            //The way this is done now is more robust, since it will be impossible to obtain a .dll the does not have the
            //corresponding ENDO/EXO vars set. This could be a problem before, for instance doing a ENDO/EXO, and then
            //afterwards Gauss-simulation with failsafe on.

            if (modelType == ECompiledModelType.GaussFailSafe)
            {
                EmitCsCodeAndCompileModel(ECompiledModelType.Gauss, isCalledFromModelStatement);  //This method is only called from here
            }
            
            EmitCsCodeAndCompileModel(modelType, isCalledFromModelStatement);  //This method is only called from here            

            G.WritelnGray("¤¤¤ Hash: " + cacheKey);

            if (isCalledFromModelStatement) PrintInfoFilesCreateVarsEtc(isCalledFromModelStatement);  //so the "endogenous" are endogenous in original model without ENDO/EXO.

            if (newM2) Program.model.modelGekko.m2cache.lru.Add(cacheKey, Program.model.modelGekko.m2);
        }

        public static void ParserFrmMakeProtobuf()
        {
            try //not the end of world if it fails (should never be done if model is read from zipped protobuffer (would be waste of time))
            {
                DateTime dt1 = DateTime.Now;

                PutListsIntoModelListHelper();

                //May take a little time to create: so use static serializer if doing serialize on a lot of small objects
                RuntimeTypeModel serializer = RuntimeTypeModel.Create();
                serializer.UseImplicitZeroDefaults = false;  //otherwise an int that has default constructor value -12345 but is set to 0 will reappear as a -12345 (instead of 0). For int, 0 is default, false for bools etc.


                // ----- SERIALIZE
                //string outputPath = Globals.localTempFilesLocation;
                //DeleteFolder(outputPath);
                //Directory.CreateDirectory(outputPath);
                string protobufFileName = Globals.gekkoVersion + "_" + Program.model.modelGekko.modelHashTrue + Globals.cacheExtensionModel;
                string pathAndFilename = Globals.localTempFilesLocation + "\\" + protobufFileName;
                using (FileStream fs = Program.WaitForFileStream(pathAndFilename, null, Program.GekkoFileReadOrWrite.Write))
                {
                    //Serializer.Serialize(fs, m);
                    serializer.Serialize(fs, Program.model.modelGekko);
                }
                //Program.WaitForZipWrite(outputPath, Globals.localTempFilesLocation + "\\" + protobufFileName);
                G.WritelnGray("Created model cache file in " + G.SecondsFormat((DateTime.Now - dt1).TotalMilliseconds));
            }
            catch (Exception e)
            {
                //do nothing, not the end of the world if it fails
            }
        }

        private static void EmitCsCodeAndCompileModel(ECompiledModelType modelType, bool isCalledFromModelStatement)
        {
            DateTime t0 = DateTime.Now;

            bool failSafe = false;
            if (modelType == ECompiledModelType.GaussFailSafe) failSafe = true;
            string failSafeString = "";
            if (failSafe) failSafeString = "FailSafe";

            string type = Enum.GetName(typeof(ECompiledModelType), modelType);
            bool didWork = false;

            // =====
            // ===== Gauss-type
            // =====

            if ((modelType == ECompiledModelType.Gauss && Program.model.modelGekko.m2.assemblyGauss == null) ||
                (modelType == ECompiledModelType.GaussFailSafe && Program.model.modelGekko.m2.assemblyGaussFailSafe == null) ||
                (modelType == ECompiledModelType.Res && Program.model.modelGekko.m2.assemblyRes == null))
            {
                didWork = true;
                StringBuilder codeGauss = new StringBuilder();
                {

                    codeGauss.Append(
                        @"using System;
                using System.Collections.Generic;
                using System.Text;
                namespace Gekko
                {
                public class " + type +
                        @"{
                public static void eqs(double[] b)
                {");

                    if (modelType == ECompiledModelType.Res)
                    {
                        codeGauss.AppendLine("double [] c = new double[b.Length];");
                        codeGauss.AppendLine("for (int i5 = 0; i5 < c.Length; i5++)");
                        codeGauss.AppendLine("{");
                        codeGauss.AppendLine("   c[i5]=b[i5];");
                        codeGauss.AppendLine("}");
                    }

                    List<EquationHelper> prologue = new List<EquationHelper>();
                    List<EquationHelper> eqs = new List<EquationHelper>();
                    List<EquationHelper> epilogue = new List<EquationHelper>();

                    if (Globals.fastGauss)
                    {
                        foreach (int endoNumber in Program.model.modelGekko.m2.prologue)
                        {
                            EquationHelper eh = Program.model.modelGekko.equations[endoNumber];
                            prologue.Add(eh);
                        }

                        if (Program.options.solve_gauss_reorder)
                        {
                            foreach (int endoNumber in Program.model.modelGekko.m2.simulRecursive)
                            {
                                EquationHelper eh = Program.model.modelGekko.equations[endoNumber];
                                eqs.Add(eh);
                            }

                            foreach (int endoNumber in Program.model.modelGekko.m2.simulFeedback)
                            {
                                EquationHelper eh = Program.model.modelGekko.equations[endoNumber];
                                eqs.Add(eh);
                            }
                        }
                        else
                        {

                            //This is old code: it is checked that the
                            //ArrayList al = new ArrayList();
                            //al.AddRange(Program.model.modelGekko.m2.simulRecursive);
                            //al.AddRange(Program.model.modelGekko.m2.simulFeedback);
                            //SortedList<int, EquationHelper> sorted = new SortedList<int, EquationHelper>();
                            //foreach (int eq in al)
                            //{
                            //    EquationHelper eh = Program.model.modelGekko.equations[eq];
                            //    sorted.Add(eh.equationNumber, eh);
                            //}
                            //for (int i = 0; i < sorted.Count; i++)
                            //{
                            //    eqs.Add(sorted.Values[i]);
                            //}

                            List<int> allSimul = GetLeftsideBNumbers();
                            foreach (int i in allSimul)
                            {
                                eqs.Add(Program.model.modelGekko.equations[i]);
                            }

                        }

                        foreach (int endoNumber in Program.model.modelGekko.m2.epilogue)
                        {
                            EquationHelper eh = Program.model.modelGekko.equations[endoNumber];
                            epilogue.Add(eh);
                        }

                        //G.Writeln(Program.model.modelGekko.equations.Count + " == " + (prologue.Count + eqs.Count + epilogue.Count));
                        if (Program.model.modelGekko.equations.Count - (prologue.Count + eqs.Count + epilogue.Count) != 0) throw new GekkoException();
                    }

                    List<EquationHelper> gaussEquations = Program.model.modelGekko.equations;
                    if (Globals.fastGauss) gaussEquations = eqs;

                    foreach (EquationHelper eh in gaussEquations)
                    {
                        if (modelType == ECompiledModelType.Res)
                        {
                            codeGauss.Append(eh.csCodeLhsJacobi);
                        }
                        else  //Gauss of GaussFailSafe
                        {
                            codeGauss.Append(eh.csCodeLhsGauss);
                        }

                        codeGauss.Append(" = ");
                        codeGauss.AppendLine(eh.csCodeRhs);
                        codeGauss.AppendLine(";");

                        if (modelType == ECompiledModelType.GaussFailSafe)  //cannot be jacobi failsafe...
                        {
                            codeGauss.AppendLine("if(Double.IsInfinity(" + eh.csCodeLhsGauss + ") || Double.IsNaN(" + eh.csCodeLhsGauss + ")) {");
                            codeGauss.AppendLine("Program.model.modelGekko.simulateResults[1] = 12345;");
                            codeGauss.AppendLine("Program.model.modelGekko.simulateResults[2] = " + eh.equationNumber + ";");
                            codeGauss.AppendLine("return;");
                            codeGauss.AppendLine("}");
                        }
                    }

                    codeGauss.AppendLine();
                    codeGauss.AppendLine();


                    if (modelType == ECompiledModelType.Res)
                    {
                        codeGauss.AppendLine("for (int i5 = 0; i5 < c.Length; i5++)");
                        codeGauss.AppendLine("{");
                        codeGauss.AppendLine("   b[i5]=c[i5];");
                        codeGauss.AppendLine("}");
                    }
                    codeGauss.AppendLine(@"}");  //end of eqs()
                    codeGauss.AppendLine(@"" + "}}");  //namespace and class

                    CompilerParameters compilerParams = new CompilerParameters();
                    compilerParams.CompilerOptions = Program.GetCompilerOptions();
                    compilerParams.GenerateInMemory = true;
                    compilerParams.IncludeDebugInformation = false;
                    compilerParams.ReferencedAssemblies.Add("system.dll");
                    ReferencedAssembliesGekko(compilerParams);
                    compilerParams.GenerateExecutable = false;

                    CompilerResults cr = Globals.iCodeCompiler.CompileAssemblyFromSource(compilerParams, codeGauss.ToString());

                    if (cr.Errors.HasErrors)
                    {
                        new Error("Model not compiled due to errors while compiling for Gauss-Seidel algorithm.");
                    }
                    if (modelType == ECompiledModelType.Gauss)
                    {
                        //Assembly temp = Assembly.LoadFile(@"c:\Users\Thomas\AppData\Local\Temp\gauss.dll");
                        //Program.model.modelGekko.m2.assemblyGauss = temp.GetType("Gekko." + type);
                        Program.model.modelGekko.m2.assemblyGauss = cr.CompiledAssembly.GetType("Gekko." + type);
                    }
                    else if (modelType == ECompiledModelType.GaussFailSafe)
                    {
                        Program.model.modelGekko.m2.assemblyGaussFailSafe = cr.CompiledAssembly.GetType("Gekko." + type);
                    }
                    else if (modelType == ECompiledModelType.Res)
                    {
                        Program.model.modelGekko.m2.assemblyRes = cr.CompiledAssembly.GetType("Gekko." + type);
                    }
                    else throw new GekkoException();  //must be one of these
                }
            }


            // =====
            // ===== Newton-type
            // =====

            if ((modelType == ECompiledModelType.Newton && Program.model.modelGekko.m2.assemblyNewton == null))
            {
                didWork = true;
                StringBuilder codeNewton = new StringBuilder();
                {
                    codeNewton.AppendLine("using System;");
                    codeNewton.AppendLine("using System.Collections.Generic;");
                    codeNewton.AppendLine("using System.Text;");
                    codeNewton.AppendLine("namespace Gekko");
                    codeNewton.AppendLine("{");
                    codeNewton.AppendLine("public class " + type);
                    codeNewton.AppendLine("{");

                    codeNewton.AppendLine("public static void simulPrologue(double[] b)");
                    codeNewton.AppendLine("{");
                    foreach (int endoNumber in Program.model.modelGekko.m2.simulRecursive)
                    {
                        StringBuilder sb = new StringBuilder();
                        EquationHelper eh = Program.model.modelGekko.equations[endoNumber];
                        sb.Append(eh.csCodeLhsGauss);
                        sb.Append(" = ");
                        sb.AppendLine(eh.csCodeRhs);
                        sb.AppendLine(";");
                        sb.AppendLine();
                        if (Program.options.solve_newton_robust) NewtonStartingValuesFixHelper2(sb);
                        codeNewton.Append(sb);

                    }
                    codeNewton.AppendLine("}");

                    codeNewton.AppendLine("public static void simulFeedbackAll(double[] b, double[] r, double[] scale)");
                    codeNewton.AppendLine("{");

                    for (int i = 0; i < Program.model.modelGekko.m2.simulFeedback.Count; i++)
                    {
                        StringBuilder sb = new StringBuilder();
                        int endoNumber = (int)Program.model.modelGekko.m2.simulFeedback[i];
                        EquationHelper eh = Program.model.modelGekko.equations[endoNumber];
                        sb.Append("r[" + i + "] = ");
                        sb.Append(eh.csCodeLhsGauss);
                        sb.Append(" -( ");
                        sb.AppendLine(eh.csCodeRhs);
                        sb.AppendLine(")");
                        sb.AppendLine(";");
                        sb.AppendLine();
                        if (Program.options.solve_newton_robust) NewtonStartingValuesFixHelper2(sb);
                        codeNewton.Append(sb);
                    }
                    codeNewton.AppendLine("}");


                    codeNewton.AppendLine("public static void simulFeedbackSingle(double[] b, double[] r, int n, double[] scale)");
                    codeNewton.AppendLine("{");
                    codeNewton.AppendLine("switch(n)");
                    codeNewton.AppendLine("{");

                    for (int i = 0; i < Program.model.modelGekko.m2.simulFeedback.Count; i++)
                    {
                        StringBuilder sb = new StringBuilder();
                        int endoNumber = (int)Program.model.modelGekko.m2.simulFeedback[i];
                        EquationHelper eh = Program.model.modelGekko.equations[endoNumber];
                        sb.AppendLine("case " + i + ":");
                        sb.Append("r[" + i + "] = ");
                        sb.Append(eh.csCodeLhsGauss);
                        sb.Append(" -( ");
                        sb.AppendLine(eh.csCodeRhs);
                        sb.AppendLine(" ); ");
                        sb.Append("break;");
                        sb.AppendLine();
                        if (Program.options.solve_newton_robust) NewtonStartingValuesFixHelper2(sb);
                        codeNewton.Append(sb);
                    }
                    codeNewton.AppendLine("}");  //case
                    codeNewton.AppendLine("}");  //method

                    codeNewton.AppendLine("}");  //class
                    codeNewton.AppendLine("}");  //namespace
                    //codeNewton.Flush();
                    //codeNewton.Close();

                    CompilerParameters compilerParams = new CompilerParameters();
                    compilerParams = new CompilerParameters();
                    compilerParams.CompilerOptions = Program.GetCompilerOptions();
                    compilerParams.GenerateInMemory = true;
                    compilerParams.IncludeDebugInformation = false;
                    compilerParams.ReferencedAssemblies.Add("system.dll");
                    //compilerParams.ReferencedAssemblies.Add(Application.ExecutablePath);
                    ReferencedAssembliesGekko(compilerParams);
                    compilerParams.GenerateExecutable = false;
                    string s = codeNewton.ToString();
                    //CompilerResults cr = Program.model.modelGekko.iCodeCompiler.CompileAssemblyFromFile(compilerParams, Globals.localTempFilesLocation + "\\" + type + ".cs");
                    CompilerResults cr = Globals.iCodeCompiler.CompileAssemblyFromSource(compilerParams, s);
                    if (modelType == ECompiledModelType.Newton)
                    {
                        Program.model.modelGekko.m2.assemblyNewton = cr.CompiledAssembly.GetType("Gekko." + type);
                    }
                    else throw new GekkoException();  //must be one of these
                }
            }


            // =====
            // ===== Is always used, both for Gauss and Newton
            // ===== Reverted (auto-JZ and Y-equations) -- put in .model, not .model.m2, because it is common for all EXO/ENDO set.
            // =====

            if ((!failSafe && Program.model.modelGekko.assemblyReverted == null) ||
               (failSafe && Program.model.modelGekko.assemblyRevertedFailSafe == null))
            {

                didWork = true;

                StringBuilder code = new StringBuilder();
                {
                    code.Append(
                        @"using System;
                using System.Collections.Generic;
                using System.Text;
                namespace Gekko
                {
                public class " + "Reverted" + failSafeString +
                        @"{");

                    EmitRevertedEquations(code);  //Same code with and without failsafe. Could maybe introduce failsafe here, for the Y-equations. But never mind for now.
                    //Are static given model, in principle they could be shared for Program.model.modelGekko, and not Program.model.modelGekko.m2 (but maybe not worth the trouble and risk of errors)

                    code.AppendLine("}");  //class
                    code.AppendLine("}");  //namespace

                    CompilerParameters compilerParams = new CompilerParameters();
                    compilerParams = new CompilerParameters();
                    compilerParams.CompilerOptions = Program.GetCompilerOptions();
                    compilerParams.GenerateInMemory = true;
                    compilerParams.IncludeDebugInformation = false;
                    compilerParams.ReferencedAssemblies.Add("system.dll");
                    //compilerParams.ReferencedAssemblies.Add(Application.ExecutablePath);
                    ReferencedAssembliesGekko(compilerParams);
                    compilerParams.GenerateExecutable = false;

                    CompilerResults cr = Globals.iCodeCompiler.CompileAssemblyFromSource(compilerParams, code.ToString());

                    if (cr.Errors.HasErrors)
                    {
                        throw new GekkoException();
                    }

                    if (!failSafe)
                    {
                        Program.model.modelGekko.assemblyReverted = cr.CompiledAssembly.GetType("Gekko.Reverted");
                    }
                    else
                    {
                        Program.model.modelGekko.assemblyRevertedFailSafe = cr.CompiledAssembly.GetType("Gekko.RevertedFailSafe");
                    }
                }
            }

            // =====
            // ===== Is always used, both for Gauss and Newton
            // ===== Prologue/Epilogue (depends upon ENDO/EXO set, so put in .m2)
            // =====

            if ((!failSafe && Program.model.modelGekko.m2.assemblyPrologueEpilogue == null) ||
                (failSafe && Program.model.modelGekko.m2.assemblyPrologueEpilogueFailSafe == null))
            {
                didWork = true;
                StringBuilder code = new StringBuilder();
                {
                    code.Append(
                        @"using System;
                using System.Collections.Generic;
                using System.Text;
                namespace Gekko
                {
                public class " + "PrologueEpilogue" + failSafeString +
                        @"{");

                    EmitPrologue(failSafeString, code);  //=====> but these are not static with endo/exo???
                    EmitEpilogue(failSafeString, code);  //=====> but these are not static with endo/exo???

                    code.AppendLine("}");  //class
                    code.AppendLine("}");  //namespace

                    CompilerParameters compilerParams = new CompilerParameters();
                    compilerParams = new CompilerParameters();
                    compilerParams.CompilerOptions = Program.GetCompilerOptions();
                    compilerParams.GenerateInMemory = true;
                    compilerParams.IncludeDebugInformation = false;
                    compilerParams.ReferencedAssemblies.Add("system.dll");
                    //compilerParams.ReferencedAssemblies.Add(Application.ExecutablePath);
                    ReferencedAssembliesGekko(compilerParams);
                    compilerParams.GenerateExecutable = false;

                    CompilerResults cr = Globals.iCodeCompiler.CompileAssemblyFromSource(compilerParams, code.ToString());

                    if (cr.Errors.HasErrors)
                    {
                        throw new GekkoException();
                    }

                    if (failSafeString == "")
                    {
                        Program.model.modelGekko.m2.assemblyPrologueEpilogue = cr.CompiledAssembly.GetType("Gekko.PrologueEpilogue");
                    }
                    else
                    {
                        Program.model.modelGekko.m2.assemblyPrologueEpilogueFailSafe = cr.CompiledAssembly.GetType("Gekko.PrologueEpilogueFailSafe");
                    }
                }
            }

            // =====
            // ===== After (put in .model, not .model.m2, because it is common for all EXO/ENDO set.)
            // =====
            // TODO: after introduction of M2 object, checking for failsafe here is probably not necessay at all
            if (modelType == ECompiledModelType.After && ((!failSafe && Program.model.modelGekko.assemblyAfter == null) ||
                (failSafe && Program.model.modelGekko.assemblyAfterFailSafe == null)))
            {
                //TODO: does not always have to be done, but for now we just keep it.
                didWork = true;
                StringBuilder code = new StringBuilder();
                {
                    code.Append(
                        @"using System;
                using System.Collections.Generic;
                using System.Text;
                namespace Gekko
                {
                public class " + "After" + failSafeString +
                        @"{");

                    EmitAfter(failSafeString, code);    //Are static given model, after() and after2() methods, , in principle they could be shared for Program.model.modelGekko, and not Program.model.modelGekko.m2 (but maybe not worth the trouble and risk of errors)

                    code.AppendLine("}");  //class
                    code.AppendLine("}");  //namespace

                    CompilerParameters compilerParams = new CompilerParameters();
                    compilerParams = new CompilerParameters();
                    compilerParams.CompilerOptions = Program.GetCompilerOptions();
                    compilerParams.GenerateInMemory = true;
                    compilerParams.IncludeDebugInformation = false;
                    compilerParams.ReferencedAssemblies.Add("system.dll");
                    //compilerParams.ReferencedAssemblies.Add(Application.ExecutablePath);
                    ReferencedAssembliesGekko(compilerParams);
                    compilerParams.GenerateExecutable = false;

                    CompilerResults cr = Globals.iCodeCompiler.CompileAssemblyFromSource(compilerParams, code.ToString());

                    if (cr.Errors.HasErrors)
                    {
                        throw new GekkoException();
                    }

                    if (failSafeString == "")
                    {
                        Program.model.modelGekko.assemblyAfter = cr.CompiledAssembly.GetType("Gekko.After");
                    }
                    else
                    {
                        Program.model.modelGekko.assemblyAfterFailSafe = cr.CompiledAssembly.GetType("Gekko.AfterFailSafe");
                    }
                }
            }  //finished After
            if (didWork)
            {
                string duration = G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds);
                if (isCalledFromModelStatement)
                {
                    Program.model.modelGekko.modelInfo.lastCompileDuration = duration;
                }
                else
                {
                    G.Writeln("Compiling lasted " + duration);
                }
            }
        }

        private static List<int> GetLeftsideBNumbers()
        {
            //Gets equation numbers in simultaneous gauss-seidel block
            //Note that this method takes a little time: do not put inside loop!
            List<int> allSimul = new List<int>();
            allSimul.AddRange(Program.model.modelGekko.m2.simulRecursive);
            allSimul.AddRange(Program.model.modelGekko.m2.simulFeedback);
            allSimul.Sort();
            return allSimul;
        }

        private static void EmitEpilogue(string failsafe, StringBuilder codeCommon)
        {
            codeCommon.AppendLine("public static void epilogue(double[] b)");
            codeCommon.AppendLine("{");

            foreach (int endoNumber in Program.model.modelGekko.m2.epilogue)
            {
                EquationHelper eh = Program.model.modelGekko.equations[endoNumber];

                codeCommon.Append(eh.csCodeLhsGauss);
                codeCommon.Append(" = ");
                codeCommon.AppendLine(eh.csCodeRhs);
                codeCommon.AppendLine(";");
                codeCommon.AppendLine();
                if (failsafe != "")
                {
                    codeCommon.AppendLine("if(Double.IsInfinity(" + eh.csCodeLhsGauss + ") || Double.IsNaN(" + eh.csCodeLhsGauss + ")) {");
                    codeCommon.AppendLine("Program.model.modelGekko.simulateResults[1] = 12345;");
                    codeCommon.AppendLine("Program.model.modelGekko.simulateResults[2] = " + eh.equationNumber + ";");
                    codeCommon.AppendLine("return;");
                    codeCommon.AppendLine("}");
                }
            }
            codeCommon.AppendLine("}");
        }

        private static void EmitPrologue(string failsafe, StringBuilder codeCommon)
        {
            codeCommon.AppendLine("public static void prologue(double[] b)");
            codeCommon.AppendLine("{");

            foreach (int endoNumber in Program.model.modelGekko.m2.prologue)
            {
                EquationHelper eh = Program.model.modelGekko.equations[endoNumber];
                codeCommon.Append(eh.csCodeLhsGauss);
                codeCommon.AppendLine(" = ");
                codeCommon.AppendLine(eh.csCodeRhs);
                codeCommon.AppendLine(";");
                codeCommon.AppendLine();

                if (failsafe != "")
                {
                    codeCommon.AppendLine("if(Double.IsInfinity(" + eh.csCodeLhsGauss + ") || Double.IsNaN(" + eh.csCodeLhsGauss + ")) {");
                    codeCommon.AppendLine("Program.model.modelGekko.simulateResults[1] = 12345;");
                    codeCommon.AppendLine("Program.model.modelGekko.simulateResults[2] = " + eh.equationNumber + ";");
                    codeCommon.AppendLine("return;");
                    codeCommon.AppendLine("}");
                }
            }
            codeCommon.AppendLine("}");
        }

        private static void EmitAfter(string failsafe, StringBuilder codeCommon)
        {
            //This is for safety: EmitAfter must not depend upon stuff in .m2!
            Model2 temp = Program.model.modelGekko.m2;
            Program.model.modelGekko.m2 = null;

            try
            {
                int count = 0, count2 = 0;
                codeCommon.AppendLine("public static void after(double[] b) {");
                foreach (EquationHelper eh in Program.model.modelGekko.equations)
                {
                    if (eh.isAfterModel && !eh.isAfter2Model)  //if both are set, it is considered after2
                    {
                        codeCommon.Append(eh.csCodeLhsGauss);
                        codeCommon.Append(" = ");
                        codeCommon.AppendLine(eh.csCodeRhs);
                        codeCommon.AppendLine(";");
                        count++;
                    }
                }
                codeCommon.AppendLine("}");  //end of after()
                codeCommon.AppendLine();

                codeCommon.AppendLine("public static void after2(double[] b) {");
                foreach (EquationHelper eh in Program.model.modelGekko.equations)
                {
                    if (eh.isAfter2Model)  //eh.isAfterModel may or not be true here --> in any case it is considered after2
                    {
                        codeCommon.Append(eh.csCodeLhsGauss);
                        codeCommon.Append(" = ");
                        codeCommon.AppendLine(eh.csCodeRhs);
                        codeCommon.AppendLine(";");
                        count2++;
                    }
                }
                codeCommon.AppendLine("}");  //end of after2()
            }
            catch
            {
                throw;
            }
            finally
            {
                Program.model.modelGekko.m2 = temp;
            }
        }

        private static void EmitRevertedEquations(StringBuilder code)
        {
            //This is for safety: EmitAfter must not depend upon stuff in .m2!
            Model2 temp = Program.model.modelGekko.m2;
            Program.model.modelGekko.m2 = null;

            foreach (EquationHelper eh in Program.model.modelGekko.equationsReverted)
            {
                if (! Parser.Frm.ParserFrmWalkAST.EquationIsRunSeparatelyAfterSim(eh))
                {
                    //Sanity check: each eq must be either auto, Y, T or L
                    //Should not be possible
                    new Error("Model equation code error #843784272449");
                }
            }

            try
            {
                code.AppendLine("public static void revertedAuto(double[] b) {");
                foreach (EquationHelper eh in Program.model.modelGekko.equationsReverted)
                {
                    if (eh.equationType == EEquationType.RevertedAutoGenerated)
                    {
                        code.Append(eh.csCodeLhsGauss);
                        code.Append(" = ");
                        code.AppendLine(eh.csCodeRhs);
                        code.AppendLine(";");
                    }
                }
                code.AppendLine("}");  //end of revertedAuto()

                code.AppendLine("public static void reverted" + Globals.equationCodeY.ToUpper() + "(double[] b) {");
                foreach (EquationHelper eh in Program.model.modelGekko.equationsReverted)
                {
                    if (eh.equationType == EEquationType.RevertedY)
                    {
                        code.Append(eh.csCodeLhsGauss);
                        code.Append(" = ");
                        code.AppendLine(eh.csCodeRhs);
                        code.AppendLine(";");
                    }
                }
                code.AppendLine("}");  //end of revertedY()

                code.AppendLine("public static void reverted" + Globals.equationCodeT.ToUpper() + "(double[] b) {");
                foreach (EquationHelper eh in Program.model.modelGekko.equationsReverted)
                {
                    if (eh.equationType == EEquationType.RevertedT)
                    {
                        code.Append(eh.csCodeLhsGauss);
                        code.Append(" = ");
                        code.AppendLine(eh.csCodeRhs);
                        code.AppendLine(";");
                    }
                }
                code.AppendLine("}");  //end of revertedX()                
            }
            catch
            {
                throw;
            }
            finally
            {
                Program.model.modelGekko.m2 = temp;
            }
        }

        private static string GetCacheKey(bool isFix)
        {
            //isFix: if active, the endo/exo goals are added as lists
            //       if inactive, the endo/exo goals are always reported as [none] no matter if there ARE goals or not
            //       like this, we can use the same dll for a model with SIM and any endo/exo goals set -- the 
            //       difference only kick in regarding SIM<fix>            
            List<string> temp1 = new List<string>();
            if (isFix) foreach (string s in Program.model.modelGekko.endogenized.Keys) temp1.Add(s.ToLower());
            List<string> temp2 = new List<string>();
            if (isFix) foreach (string s in Program.model.modelGekko.exogenized.Keys) temp2.Add(s.ToLower());
            temp1.Sort();
            temp2.Sort();
            StringBuilder ss = new StringBuilder("ENDO-EXO-info. Endogenized: ");
            foreach (string s in temp1) ss.Append(s + ",");
            if (temp1.Count == 0) ss.Append("[none],");
            ss.Remove(ss.Length - 1, 1);
            ss.Append(". Exogenized: ");
            foreach (string s in temp2) ss.Append(s + ",");
            if (temp2.Count == 0) ss.Append("[none],");
            ss.Remove(ss.Length - 1, 1);
            ss.Append(". ");
            string stacked = "false";
            //if (G.Equal(Program.options.solve_forward_method, "stacked")) stacked = "true";  //stacked is obsolete
            ss.Append("Stacked: " + stacked);
            return ss.ToString();
        }

        private static void NewtonStartingValuesFixHelper2(StringBuilder sb)
        {
            sb = sb.Replace("O.Log(", "O.Special_Log(");
            sb = sb.Replace("O.Pow(", "O.Special_Pow(");
        }

        private static void PrintInfoFilesCreateVarsEtc(bool isCalledFromModelStatement)
        {
            //------------------- printing of info files etc. -----------------------------------------------
            if (true)
            {
                ArrayList al = new ArrayList(Program.model.modelGekko.varsAType.Keys);
                al.Sort(StringComparer.InvariantCulture);


                List<string> exod = new List<string>();
                List<string> exoj = new List<string>();
                List<string> exoz = new List<string>();
                List<string> exodjz = new List<string>();
                List<string> exo = new List<string>();
                List<string> exotrue = new List<string>();
                List<string> endo = new List<string>();
                List<string> all = new List<string>();

                foreach (string var in al)
                {
                    if (Program.model.modelGekko.m2.endogenous.ContainsKey(var))
                    {
                        Program.model.modelGekko.numberOfEndo++;
                        endo.Add(var);
                    }
                }


                foreach (string var in al)
                {
                    all.Add(var);
                }

                foreach (string var in al)
                {
                    if (!Program.model.modelGekko.m2.endogenous.ContainsKey(var))
                    {
                        exo.Add(var);
                    }
                    if (!Program.model.modelGekko.m2.endogenous.ContainsKey(var) && !Program.model.modelGekko.varsJTypeAutoGenerated.ContainsKey(var) && !Program.model.modelGekko.varsDTypeAutoGenerated.ContainsKey(var) && !Program.model.modelGekko.varsZTypeAutoGenerated.ContainsKey(var))
                    {
                        Program.model.modelGekko.numberOfExo++;
                        exotrue.Add(var);
                    }
                }


                foreach (string var in al)
                {
                    if (Program.model.modelGekko.varsJTypeAutoGenerated.ContainsKey(var) || Program.model.modelGekko.varsDTypeAutoGenerated.ContainsKey(var) || Program.model.modelGekko.varsZTypeAutoGenerated.ContainsKey(var))
                    {
                        Program.model.modelGekko.numberOfDjz++;
                        exodjz.Add(var);
                    }
                    if (Program.model.modelGekko.varsJTypeAutoGenerated.ContainsKey(var))
                    {
                        exoj.Add(var);
                    }
                    if (Program.model.modelGekko.varsDTypeAutoGenerated.ContainsKey(var))
                    {
                        exod.Add(var);
                    }
                    if (Program.model.modelGekko.varsZTypeAutoGenerated.ContainsKey(var))
                    {
                        exoz.Add(var);
                    }
                }

                exod.Sort(StringComparer.InvariantCulture);
                exoj.Sort(StringComparer.InvariantCulture);
                exoz.Sort(StringComparer.InvariantCulture);
                exodjz.Sort(StringComparer.InvariantCulture);
                exo.Sort(StringComparer.InvariantCulture);
                exotrue.Sort(StringComparer.InvariantCulture);
                endo.Sort(StringComparer.InvariantCulture);
                all.Sort(StringComparer.InvariantCulture);

                Program.databanks.GetGlobal().AddIVariableWithOverwrite(Globals.symbolCollection + "exod", new List(Stringlist.GetListOfIVariablesFromListOfStrings(exod.ToArray())));
                Program.databanks.GetGlobal().AddIVariableWithOverwrite(Globals.symbolCollection + "exoj", new List(Stringlist.GetListOfIVariablesFromListOfStrings(exoj.ToArray())));
                Program.databanks.GetGlobal().AddIVariableWithOverwrite(Globals.symbolCollection + "exoz", new List(Stringlist.GetListOfIVariablesFromListOfStrings(exoz.ToArray())));
                Program.databanks.GetGlobal().AddIVariableWithOverwrite(Globals.symbolCollection + "exodjz", new List(Stringlist.GetListOfIVariablesFromListOfStrings(exodjz.ToArray())));
                Program.databanks.GetGlobal().AddIVariableWithOverwrite(Globals.symbolCollection + "exo", new List(Stringlist.GetListOfIVariablesFromListOfStrings(exo.ToArray())));
                Program.databanks.GetGlobal().AddIVariableWithOverwrite(Globals.symbolCollection + "exotrue", new List(Stringlist.GetListOfIVariablesFromListOfStrings(exotrue.ToArray())));
                Program.databanks.GetGlobal().AddIVariableWithOverwrite(Globals.symbolCollection + "endo", new List(Stringlist.GetListOfIVariablesFromListOfStrings(endo.ToArray())));
                Program.databanks.GetGlobal().AddIVariableWithOverwrite(Globals.symbolCollection + "all", new List(Stringlist.GetListOfIVariablesFromListOfStrings(all.ToArray())));

                List<string> files = new List<string>();
                files.Add("exod");
                files.Add("exoj");
                files.Add("exoz");
                files.Add("exodjz");
                files.Add("exo");
                files.Add("exotrue");
                files.Add("endo");
                files.Add("all");

                List<List<string>> lists = new List<List<string>>();
                lists.Add(exod);
                lists.Add(exoj);
                lists.Add(exoz);
                lists.Add(exodjz);
                lists.Add(exo);
                lists.Add(exotrue);
                lists.Add(endo);
                lists.Add(all);

                for (int i = 0; i < files.Count; i++)
                {
                    using (FileStream temp = Program.WaitForFileStream(Program.GetModelInfoPath() + "\\" + files[i] + ".lst", null, Program.GekkoFileReadOrWrite.Write))
                    using (StreamWriter tempFs = G.GekkoStreamWriter(temp))
                    {
                        List<string> oneList = lists[i];
                        foreach (string s in oneList)
                        {
                            tempFs.WriteLine(s);
                        }
                        tempFs.Flush();
                        tempFs.Close();
                    }
                }

                string folder = Program.GetModelInfoPath();  //always a <modelname>__info subfolder to local temp files folder

                if (G.Equal(Program.options.model_infofile, "yes"))
                {
                    string zipFileNameInput = Program.CreateFullPathAndFileName(Globals.modelFileName.Replace(".frm", "") + "__info.zip");
                    Program.WaitForZipWrite(folder, zipFileNameInput);
                }

                //if (isCalledFromModelStatement) G.Writeln("Details regarding model: see " + Path.GetFileName(zipFileNameInput));

            }
        }


        public static void PutListsIntoModelListHelper()
        {
            ModelListHelper modelListHelper = new ModelListHelper();

            if (Program.databanks.GetGlobal().ContainsIVariable(Globals.symbolCollection + "all")) modelListHelper.all = Stringlist.GetListOfStringsFromList(Program.databanks.GetGlobal().GetIVariable(Globals.symbolCollection + "all"));
            if (Program.databanks.GetGlobal().ContainsIVariable(Globals.symbolCollection + "endo")) modelListHelper.endo = Stringlist.GetListOfStringsFromList(Program.databanks.GetGlobal().GetIVariable(Globals.symbolCollection + "endo"));
            if (Program.databanks.GetGlobal().ContainsIVariable(Globals.symbolCollection + "exo")) modelListHelper.exo = Stringlist.GetListOfStringsFromList(Program.databanks.GetGlobal().GetIVariable(Globals.symbolCollection + "exo"));
            if (Program.databanks.GetGlobal().ContainsIVariable(Globals.symbolCollection + "exod")) modelListHelper.exod = Stringlist.GetListOfStringsFromList(Program.databanks.GetGlobal().GetIVariable(Globals.symbolCollection + "exod"));
            if (Program.databanks.GetGlobal().ContainsIVariable(Globals.symbolCollection + "exodjz")) modelListHelper.exodjz = Stringlist.GetListOfStringsFromList(Program.databanks.GetGlobal().GetIVariable(Globals.symbolCollection + "exodjz"));
            if (Program.databanks.GetGlobal().ContainsIVariable(Globals.symbolCollection + "exoj")) modelListHelper.exoj = Stringlist.GetListOfStringsFromList(Program.databanks.GetGlobal().GetIVariable(Globals.symbolCollection + "exoj"));
            if (Program.databanks.GetGlobal().ContainsIVariable(Globals.symbolCollection + "exotrue")) modelListHelper.exotrue = Stringlist.GetListOfStringsFromList(Program.databanks.GetGlobal().GetIVariable(Globals.symbolCollection + "exotrue"));
            if (Program.databanks.GetGlobal().ContainsIVariable(Globals.symbolCollection + "exoz")) modelListHelper.exoz = Stringlist.GetListOfStringsFromList(Program.databanks.GetGlobal().GetIVariable(Globals.symbolCollection + "exoz"));

            Program.model.modelGekko.modelInfo.modelListHelper = modelListHelper;
        }

        public static void ReferencedAssembliesGekko(CompilerParameters compilerParams)
        {
            if (G.IsUnitTesting())
            {
                //if running test cases, use this absolute path                
                compilerParams.ReferencedAssemblies.Add(Globals.ttPath2 + @"\GekkoCS\Gekko\bin\Debug\gekko.exe");
            }
            else
            {
                compilerParams.ReferencedAssemblies.Add(Application.ExecutablePath);
            }
        }

        public static void ParserFrmHandleVarlist(ModelCommentsHelper modelCommentsHelper, P p)
        {
            StringBuilder varList = null;

            string fileNameTemp = null;
            bool foundInFrm = false;
            if (modelCommentsHelper.cutout_varlist != null && modelCommentsHelper.cutout_varlist.Length > 0)
            {
                foundInFrm = true;
                varList = new StringBuilder(modelCommentsHelper.cutout_varlist);
            }
            else
            {
                //try to find it externally, look also in model path!
                List<string> folders = new List<string>();
                folders.Add(Program.options.folder_model);
                FindFileHelper ffh = Program.FindFile("varlist.dat", folders, true, true, p);
                fileNameTemp = ffh.realPathAndFileName;
                if (fileNameTemp != null)
                {
                    string s = Program.GetTextFromFileWithWait(fileNameTemp);  //can read an ANSI file without problems
                    s = "varlist$" + "\n" + s; //a bit hacky, just like the string-StringBuilder-StringReader stuff is convoluted. Anyway, not critical code here.
                    varList = new StringBuilder(s);
                }
            }

            if (varList != null && varList.Length > 0)
            {
                string s = Program.UnfoldVariableList(new StringReader(varList.ToString()));
                if (foundInFrm)
                {
                    if (s != null) s = s + " (found inside .frm file)";
                }
                else
                {
                    if (s != null && fileNameTemp != null) s = s + " (" + fileNameTemp + ")";  //should always be != null, but for safety...
                }
                Program.model.modelGekko.modelInfo.varlistStatus = s;
            }
            else
            {
                Program.model.modelGekko.modelInfo.varlistStatus = "Not found inside .frm file or as 'varlist.dat' file";
            }
        }

        public static void HandleModelLexerErrors(List<string> errors, List<string> inputFileLines, ParseHelper ph)
        {
            if (Globals.threadIsInProcessOfAborting) return;            
            Program.StopPipeAndMute(2);
            int number = 0;
            foreach (string s in errors)
            {
                number++;
                if (errors.Count > 1)  //always just one
                {
                    if (number == 1) G.Writeln();
                    G.Writeln("--------------------- error #" + number + " of " + errors.Count + "-----------------");
                    //G.Writeln();
                }
                else G.Writeln();

                string[] ss = s.Split(Globals.parserErrorSeparator);
                int lineNumberTemp = int.Parse(ss[0]) - 1;  //seems 1-based before subtract 1
                int lineNumber = lineNumberTemp + 1;  //1-based
                int positionNo = int.Parse(ss[1]) + 1;  //1-based
                string fileName = ph.fileName;
                Program.CorrectLineNumber(ref fileName, ref lineNumber);  //this has NO effect, just calling the method to keed these things together!

                string errorMessage = ss[3];

                errorMessage = errorMessage.Replace(@"'\\r\\n'", "<newline>");  //easier to understand

                if (lineNumber > inputFileLines.Count)
                {
                    new Error(errorMessage, false);
                    continue;  //doesn't give meaning
                }
                string line = inputFileLines[lineNumber - 1];
                int firstWordPosInLine = line.Length - line.TrimStart().Length + 1;

                bool previousLineProbablyCulprit = false;
                if (positionNo == firstWordPosInLine && errorMessage.Contains("no viable"))
                {
                    //get preceding line (or really: statement) -- most probably the culprit.
                    previousLineProbablyCulprit = true;
                }

                string paranthesesError = "";

                if (ph.isOneLinerFromGui == true && lineNumber != 1)
                {
                    G.Writeln("*** ERROR: Parsing this line:");
                    G.Writeln("    " + G.ReplaceGlueSymbols(inputFileLines[0]), Color.Blue);
                    G.Writeln("*** ERROR: " + errorMessage);
                }
                else
                {
                    if (ph.isOneLinerFromGui == false)
                    {
                        string fn = fileName;
                        if (fn == null || fn == "")
                        {
                            G.Writeln("*** ERROR: Parsing user input block, line " + lineNumber + " pos " + positionNo);
                        }
                        else
                        {
                            G.Writeln("*** ERROR: Parsing file: " + fn + " line " + lineNumber + " pos " + positionNo);
                        }

                        string e2 = errorMessage.Replace("Der blev udløst en undtagelse af typen ", "");
                        G.Writeln("           " + e2);

                    }
                    else
                    {
                        G.Writeln("*** ERROR: Parsing pos " + positionNo + ":  " + errorMessage);
                    }
                    line = line + "  ";  //hack to avoid ending problems.....
                    string lineTemp = line;
                    string line0 = lineTemp.Substring(0, positionNo - 1);
                    string line1 = lineTemp.Substring(positionNo - 1, 1);
                    string line2 = lineTemp.Substring(positionNo - 1 + 1);

                    if (previousLineProbablyCulprit && lineNumber > 1)
                    {
                        G.Writeln("    " + "Line " + (lineNumber - 1) + " may be the real cause of the problem");
                        string lineBefore = inputFileLines[lineNumber - 1 - 1];
                        G.Writeln("    " + "[" + G.IntFormat(lineNumber - 1, 4) + "]:" + "   " + G.ReplaceGlueSymbols(lineBefore), Color.Blue);
                    }

                    G.Write("    " + "[" + G.IntFormat(lineNumber, 4) + "]:" + "   " + G.ReplaceGlueSymbols(line0), Color.Blue);
                    G.Write(G.ReplaceGlueSymbols(line1), Color.Red);
                    G.Writeln(G.ReplaceGlueSymbols(line2), Color.Blue);

                    G.Writeln(G.Blanks(positionNo - 1 + 4 + 5 + 5) + "^", Color.Blue);
                    G.Writeln(G.Blanks(positionNo - 1 + 4 + 5 + 5) + "^", Color.Blue);
                    //G.Writeln();
                }

                if (paranthesesError != "") G.Writeln(paranthesesError);

            }
            if (errors.Count > 1) G.Writeln("--------------------- end of " + errors.Count + " errors --------------");
        }






    }
}
