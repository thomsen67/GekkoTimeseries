using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Gekko.Parser.Gek
{
    public class ParserGekCompileAndRunAST
    {
        public static void CompileAndRunAST(ConvertHelper ch, P p)
        {
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("RunCmd start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.CompilerOptions = Globals.compilerOptions;  //has no effect it seems
            compilerParams.GenerateInMemory = true;
            compilerParams.IncludeDebugInformation = false;
            compilerParams.ReferencedAssemblies.Add("system.dll");
            compilerParams.ReferencedAssemblies.Add("system.windows.forms.dll");
            compilerParams.ReferencedAssemblies.Add("system.drawing.dll");

            if (G.IsUnitTesting())
            {
                //if running test cases, use this absolute path, this will never be run by users
                compilerParams.ReferencedAssemblies.Add(Globals.ttPath2 + "\\" + Globals.ttPath3 + @"\Gekko\bin\Debug\ANTLR.dll");
                compilerParams.ReferencedAssemblies.Add(Globals.ttPath2 + "\\" + Globals.ttPath3 + @"\Gekko\bin\Debug\gekko.exe");
            }
            else
            {
                compilerParams.ReferencedAssemblies.Add(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ANTLR.dll"));
                compilerParams.ReferencedAssemblies.Add(Application.ExecutablePath);
            }

                        
            compilerParams.GenerateExecutable = false;
            CSharpCodeProvider csCompiler = new CSharpCodeProvider();
                        
            string code = ch.code + " ";
            Globals.lastDynamicCsCode = ch.code;  //would be nicer to have this in the P object.

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Compile start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("COMPILE START");
            CompilerResults cr = provider.CompileAssemblyFromSource(compilerParams, ch.code);
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("COMPILE END");
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Compile end: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            if (cr.Errors.HasErrors)
            {
                p.hasBeenCompilationError = true;

                foreach (CompilerError ce in cr.Errors)
                {
                    if (G.equal(ce.ErrorNumber, "CS0159") || G.equal(ce.ErrorNumber, "CS0140"))
                    {
                        G.Writeln2("*** ERROR: Problem with GOTO and TARGET");
                        G.Writeln2(ce.ErrorText);
                    }
                    else if (G.equal(ce.ErrorNumber, "CS1501"))
                    {
                        //Se also #09835742345
                        G.Writeln2("*** ERROR: A function is called with the wrong number of parameters");
                        List<string> q = new List<string>();
                        foreach (Match match in Regex.Matches(ce.ErrorText, "'([^']*)'"))
                            q.Add(match.ToString());
                        List<string> d = new List<string>();
                        foreach (Match match in Regex.Matches(ce.ErrorText, "\\d+"))
                            d.Add(match.ToString());                        
                        if (q.Count == 1 && d.Count == 1)
                        {
                            int ii = int.Parse(d[0]);
                            if (ii > 0) G.Writeln("*** The function " + q[0] + " does not accept " + (ii - 1) + " arguments", Color.Red);
                            else G.Writeln("*** Problem with the function  " + q[0] + "", Color.Red);
                        }
                    }
                    else if (G.equal(ce.ErrorNumber, "CS0117"))
                    {
                        G.Writeln2("*** ERROR: A function could not be found");
                        //Se also #09835742345                        
                        List<string> q = new List<string>();
                        foreach (Match match in Regex.Matches(ce.ErrorText, "'([^']*)'"))
                            q.Add(match.ToString());
                        string hit = null;
                        if (q.Count > 0)
                        {
                            string line = G.ReplaceGlueNew(ch.commandsText);
                            for (int i = -20; i <= 20; i++)
                            {
                                if (i == 0) continue;
                                string s = null;
                                if (i <= 0) s = Program.StripQuotes(q[q.Count - 1]) + "(" + i + ")";
                                else s = Program.StripQuotes(q[q.Count - 1]) + "(+" + i + ")";
                                if (line.Contains(s)) hit = s;
                            }
                            //we use the last single quote match
                            G.Writeln("*** The function " + q[q.Count - 1] + " does not seem to exist", Color.Red);
                            if (hit != null) G.Writeln("*** Note: a timeseries lag like " + hit + " should be " + hit.Replace("(", "[").Replace(")", "]") + " in Gekko 2.0", Color.Red);
                        }                        
                    }
                    else if (G.equal(ce.ErrorNumber, "CS0161"))
                    {
                        G.Writeln2("*** ERROR: Problem with return values from user defined function");

                        int pFrom = ce.ErrorText.IndexOf("Gekko.TranslatedCode.") + "Gekko.TranslatedCode.".Length;
                        int pTo = ce.ErrorText.IndexOf("(");
                        if (pFrom != -1 && pTo != -1)
                        {
                            int dif = pTo - pFrom;
                            if (dif > 0)
                            {
                                string result = ce.ErrorText.Substring(pFrom, dif);
                                G.Writeln("*** The function '" + result + "' does not seem to return something in all cases", Color.Red);
                            }
                        }                        
                    }
                }

                string text = "*** ERROR: Internal Gekko error regarding file: " + p.lastFileSentToANTLR;
                if (p.lastFileSentToANTLR == "") text = "*** ERROR: Internal Gekko error regarding user input";
                WriteCompileErrorMessage(text, p.lastFileSentToANTLR);
                throw new GekkoException();
            }
            Assembly asm = cr.CompiledAssembly;

            // Load the generated assembly into the ApplicationDomain    
            Object[] args = new Object[1];
            args[0] = p;

            try
            {
                Type assembly = asm.GetType("Gekko.TranslatedCode");
                if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Running dll start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);
                p.Deeper();
                DateTime t0 = DateTime.Now;
                //---------------------------------------------------------------------------
                // Actually running the .gcm file (translated into .cs) is done below
                //---------------------------------------------------------------------------
                //This only takes time to JIT the first time it is invoked
                //But cmd files are typically only run 1 time and not called
                //again and again (like model SIM for instance). So the JIT overhead
                //is always there. But just to say that if the below line was multiplied,
                //only the first instance would takte time to JIT.
                //Seems NGEN can avoid the JIT if the image is cached, but is it really worth it?
                //The issue is worst for large cmd files full of simple lines like UPD or
                //GENR, and no loops etc. But then why not use a databank and a model for that
                //kind of stuff? For more normal kinds of programs, especially when we go the
                //AREMOS way with loops etc., the parsing/compiling/JITting would probably be
                //less visible. We would have smaller programs with more looping.
                //On a .cmd file with 1000 GENRs, the JITting is unreasonably slow (50 sec.). 
                //Probably the following takes place. Without splitting of the code, there is a
                //large method called. This starts to execute, and C# sees that it is 'big' and
                //hence should rather be optimized. So while executing it, C# starts to JIT it
                //aggressively which does not make sense since it is only run once. JIT'ing an
                //UPD statement takes a lot longer than just running it more slowly ('interpreted').
                //The solution is code splitting, forcing C# NOT to try to optimize.                
                //Splitting it into 
                //for instance 200 methods each with 5 GENRs speeds the JIT up to about 4 sec.
                //So splitting large cmd files seems to help a lot.                                                
                if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("RUN START");
                assembly.InvokeMember("CodeLines", BindingFlags.InvokeMethod, null, null, args);
                if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("RUN END");
            }
            catch (Exception e)
            {
                if (Globals.threadIsInProcessOfAborting)
                {
                    throw e;
                }
                string exception = "";
                if (e.InnerException != null) exception = e.InnerException.Message;
                if (exception.Length > 0)
                {
                    {
                        string originalFileName;
                        int lineNumber;
                        string problemLine;
                        List<string> commandLines;

                        Program.GetErrorLineAndText(p, p.GetDepth(), out lineNumber, out originalFileName, out commandLines);

                        if (lineNumber <= 0)
                        {
                            problemLine = "";
                        }
                        else
                        {
                            problemLine = commandLines[lineNumber - 1];
                        }

                        bool lexer = false;

                        if (p.hasShownErrorHandling == EHasShownErrorHandling.False)
                        {
                            if (exception.Contains("¤Model lexer error:"))
                            {
                                lexer = true;
                                List<string> temp = new List<string>();
                                temp.Add(exception);
                                ParserOLD.PrintModelLexerErrors(temp, Globals.modelFileLines, new ParseHelper());
                            }
                            if (exception.Contains("¤Cmd lexer error:"))
                            {
                                lexer = true;
                                List<string> temp = new List<string>();
                                temp.Add(exception);
                                ParserOLD.PrintModelLexerErrors(temp, Globals.cmdFileLines, new ParseHelper());
                            }

                            if (originalFileName == "" && commandLines.Count == 1)  //more-liners get file-type error messages
                            {
                                if (lexer == true) G.Writeln("*** ERROR: Problem parsing/lexing command line:");
                                else G.Writeln("*** ERROR: Running user input line:");
                                G.Writeln("              " + G.ReplaceGlueNew(problemLine), Color.Blue);
                            }
                            else
                            {
                                //file or text block user input (>1 line)
                                string xx = "Running";
                                if (lexer == true) xx = "Problem parsing/lexing";
                                string text = null;
                                string lineNumber3 = "" + lineNumber;
                                if (lineNumber == 0) lineNumber3 = "[unknown]";

                                if (originalFileName == null || originalFileName == "")
                                {
                                    //text block user input
                                    text = "*** ERROR: User input block, line " + lineNumber3 + ":";
                                }
                                else
                                {
                                    //file
                                    text = "*** ERROR: " + xx + " file: " + originalFileName + " line " + lineNumber3;
                                }
                                Program.WriteErrorMessage(lineNumber, problemLine, text, originalFileName);
                            }
                        }
                    }
                }
                throw e;
            }
            finally
            {
                //always remove any _tmptmp-variables in banks if present
                p.RemoveLast();                
            }

            return;
        }

        //TODO: Delete this when Parser.cs is deleted
        private static void WriteCompileErrorMessage(string text, string fileName)
        {
            if (Globals.threadIsInProcessOfAborting) return;
            G.Writeln(text, Color.Black, true);
        }

    }  //end of class
}

