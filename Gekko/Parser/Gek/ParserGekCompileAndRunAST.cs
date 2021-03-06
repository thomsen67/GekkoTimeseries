﻿using System;
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
using System.Diagnostics;
using System.Linq;

namespace Gekko.Parser.Gek
{
    public class ParserGekCompileAndRunAST
    {
        /// <summary>
        /// Run the compiled C# code from .gcm commands.
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="p"></param>
        public static void CompileAndRunAST(ConvertHelper ch, P p)
        {            

            Assembly a = null;            
            CompilerResults compilerResults = CompileAST(ch.code, p, a);
            if (compilerResults.Errors.HasErrors) return;
                       
            // Load the generated assembly into the ApplicationDomain    
            Object[] args = new Object[1];
            args[0] = p;

            try
            {
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

                //code = code;  //just so it is easy to see here                                                
                if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("RUN START");

                Assembly assembly = compilerResults.CompiledAssembly;
                
                Type tpe = assembly.GetType("Gekko.TranslatedCode");  //the class                       
                tpe.InvokeMember("CodeLines", BindingFlags.InvokeMethod, null, null, args);  //the method                     

                if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("RUN END");
            }
            catch (Exception e)
            {
                //This Exception typically has a GekkoException inside
                HandleCommandRunErrors(p, e);                
                throw;  //changed from return til throw here. This provides a 'more' link with C# line, also if the error occurs in a gcm file. The question is: does this break something??
                //return;
            }
            finally
            {                
                p.RemoveLast();
            }

            return;
        }

        private static CompilerResults CompileAST(string code, P p, Assembly addedAssembly)
        {
            CompilerResults cr;
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("RunCmd start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.CompilerOptions = Program.GetCompilerOptions();  //has no effect it seems
            compilerParams.GenerateInMemory = false;  //cannot be set true, since the .gcm dll needs to refer to the user defined functions dll. But this should not be a problem.
            compilerParams.IncludeDebugInformation = false; //CHanged, maybe change back
            compilerParams.ReferencedAssemblies.Add("system.dll");
            compilerParams.ReferencedAssemblies.Add("system.windows.forms.dll");
            compilerParams.ReferencedAssemblies.Add("system.drawing.dll");
            compilerParams.ReferencedAssemblies.Add("system.core.dll");
            if (addedAssembly != null) compilerParams.ReferencedAssemblies.Add(addedAssembly.Location);

            if (Globals.excelDna)
            {
                compilerParams.ReferencedAssemblies.Add(Path.Combine(Globals.excelDnaPath, "ANTLR.dll"));
                compilerParams.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", "").Replace("/", "\\"));
            }
            else if (G.IsUnitTesting())
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

            //code = ch.code + " ";
            Globals.lastDynamicCsCode = code;  //would be nicer to have this in the P object.        

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Compile start: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("COMPILE START");

            cr = provider.CompileAssemblyFromSource(compilerParams, code);

            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("COMPILE END");
            if (Globals.runningOnTTComputer && Globals.showTimings) G.Writeln("Compile end: " + G.SecondsFormat((DateTime.Now - p.startingTime).TotalMilliseconds), Color.LightBlue);

            if (cr.Errors.HasErrors)
            {
                HandleCommandCompileErrors(p, cr);
            }
            return cr;
        }

        /// <summary>
        /// Run errors, that is, not lexer/parser/syntax errors, or compile errors. These are "real" error messages,
        /// typically from an Error(...) with implied GekkoException.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="e"></param>
        private static void HandleCommandRunErrors(P p, Exception e)
        {

            if (Globals.threadIsInProcessOfAborting)
            {
                throw e;
            }

            if (p.hasWrittenRunTimeErrorOnce) return;  //We now write a stack first time an error is encountered
            p.hasWrittenRunTimeErrorOnce = true;
            
            Program.PrintExceptionWraps(e.InnerException);

            string exception = "";
            if (e.InnerException != null) exception = e.InnerException.Message;
            if (exception.Length > 0)
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
                    problemLine = "";
                    if (lineNumber - 1 < commandLines.Count) problemLine = commandLines[lineNumber - 1];
                }

                bool lexer = false;

                if (p.hasShownErrorHandling == EHasShownErrorHandling.False)
                {
                    if (exception.Contains("¤Model lexer error:"))
                    {
                        lexer = true;
                        List<string> temp = new List<string>();
                        temp.Add(exception);
                        Parser.Frm.ParserFrmCompileAST.HandleModelLexerErrors(temp, Globals.modelFileLines, new ParseHelper());
                    }
                    if (exception.Contains("¤Cmd lexer error:"))
                    {
                        //------------------------------------------------------------------
                        //!! THIS WILL PROBABLY NEVER HAPPEN 
                        //!! -->  such errors gets caught in HandleCommandLexerErrors()
                        //!! Also strange that model lexer error printing is used here...?
                        //!! We will leave it here for now...
                        //------------------------------------------------------------------
                        lexer = true;
                        List<string> temp = new List<string>();
                        temp.Add(exception);
                        Parser.Frm.ParserFrmCompileAST.HandleModelLexerErrors(temp, Globals.cmdFileLines, new ParseHelper());
                    }

                    if (originalFileName == "" && commandLines.Count == 1)  //more-liners get file-type error messages
                    {
                        if (lexer == true) G.Writeln("*** ERROR: Problem parsing/lexing command line:");
                        else G.Writeln("*** ERROR: Running user input line:");
                        G.Writeln("              " + G.ReplaceGlueSymbols(problemLine), Color.Blue);
                    }
                    else
                    {
                        //file or text block user input (>1 line)
                        string xx = "Running";
                        if (lexer == true) xx = "Problem parsing/lexing";
                        string text = null;
                        string lineNumber3 = "" + lineNumber;
                        if (lineNumber == 0) lineNumber3 = "[unknown]";

                        string s2 = "*** ERROR: ";
                        if (p.hasSeenStopCommand > 0) s2 = null;  //do not issue an error here

                        if (originalFileName == null || originalFileName == "")
                        {
                            //text block user input
                            text = s2 + "User input block, line " + lineNumber3 + ":";
                        }
                        else
                        {
                            //file
                            text = s2 + xx + " file " + originalFileName + ", line " + lineNumber3;
                        }

                        Program.WriteErrorMessage(lineNumber, problemLine, text, originalFileName);

                    }
                }
            }
            Program.WriteCallStack(false, p);  //will only be performed once
            throw e;

        }        

        
        private static void HandleCommandCompileErrors(P p, CompilerResults cr)
        {

            using (Error e = new Error())
            {
                e.ThrowNoException();  //do not throw an exception, just print the error and move on

                p.hasBeenCompilationError = true;

                foreach (CompilerError ce in cr.Errors)
                {
                    if (G.Equal(ce.ErrorNumber, "CS0159") || G.Equal(ce.ErrorNumber, "CS0140"))
                    {
                        e.MainAdd("Problem with GOTO and TARGET. ");
                        e.MainAdd(ce.ErrorText);
                        //e.MainNewLineTight();
                    }
                    else if (G.Equal(ce.ErrorNumber, "CS1501"))
                    {
                        //Se also #09835742345
                        e.MainAdd("A function is called with the wrong number of parameters.");
                        List<string> q = new List<string>();
                        foreach (Match match in Regex.Matches(ce.ErrorText, "'([^']*)'"))
                            q.Add(match.ToString());
                        List<string> d = new List<string>();
                        foreach (Match match in Regex.Matches(ce.ErrorText, "\\d+"))
                            d.Add(match.ToString());
                        if (q.Count == 1 && d.Count == 1)
                        {
                            int ii = int.Parse(d[0]);
                            if (ii > 0) e.MainAdd("The function " + q[0] + " does not accept " + (ii - 1 - 2) + " arguments.");
                            else e.MainAdd("Problem with the function  " + q[0] + ".");
                        }
                        //e.MainNewLineTight();
                    }
                    else if (G.Equal(ce.ErrorNumber, "CS0117"))
                    {
                        //Se also #09835742345                        
                        List<string> q = new List<string>();
                        foreach (Match match in Regex.Matches(ce.ErrorText, "'([^']*)'"))
                            q.Add(match.ToString());
                        string hit = null;
                        if (q.Count > 0)
                        {
                            //we use the last single quote match
                            e.MainAdd("The function " + q[q.Count - 1] + " does not seem to exist locally.");
                            e.MainAdd("You may use use FUNCTION to define a function in your gcm file.");
                        }
                        else
                        {
                            e.MainAdd("A function could not be found.");
                        }
                        //e.MainNewLineTight();
                    }
                    else if (G.Equal(ce.ErrorNumber, "CS0161"))
                    {
                        e.MainAdd("Problem with return values from user defined function.");

                        int pFrom = ce.ErrorText.IndexOf("Gekko.TranslatedCode.") + "Gekko.TranslatedCode.".Length;
                        int pTo = ce.ErrorText.IndexOf("(");
                        if (pFrom != -1 && pTo != -1)
                        {
                            int dif = pTo - pFrom;
                            if (dif > 0)
                            {
                                string result = ce.ErrorText.Substring(pFrom, dif);
                                e.MainAdd("The function '" + result + "' does not seem to return something in all cases.");
                            }
                        }
                        //e.MainNewLineTight();
                    }
                    else if (G.Equal(ce.ErrorNumber, "CS0162"))
                    {
                        e.MainAdd("Unreachable code detected in user defined function.");
                        e.MainAdd("This may be because of a misplaced return statement.");

                        int pFrom = ce.ErrorText.IndexOf("Gekko.TranslatedCode.") + "Gekko.TranslatedCode.".Length;
                        int pTo = ce.ErrorText.IndexOf("(");
                        if (pFrom != -1 && pTo != -1)
                        {
                            int dif = pTo - pFrom;
                            if (dif > 0)
                            {
                                string result = ce.ErrorText.Substring(pFrom, dif);
                                e.MainAdd("The function '" + result + "' contains unreachable code.");
                            }
                        }
                        //e.MainNewLineTight();
                    }
                }

                if (Globals.threadIsInProcessOfAborting)
                {
                    //do nothing
                }
                else
                {
                    if (p.lastFileSentToANTLR == "") e.MainAdd("Internal Gekko error regarding user input.");
                    else e.MainAdd("Internal Gekko error regarding file: " + p.lastFileSentToANTLR + ".");
                }

                throw new GekkoException();
            }

        }
        

    }  //end of class
}

