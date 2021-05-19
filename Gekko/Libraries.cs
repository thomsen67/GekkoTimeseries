using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Gekko
{
    //
    // Library x:
    // ---------------------
    // name = 'x';
    // items = 
    // 'f'             code --> original gcm code
    //                 0 --> f()
    //                 1 --> f(...)
    //                 2 --> f(..., ...)
    // - - - - - - - - - - - - - - - -
    // 'g'             code
    //                 0
    //                 1
    //                 2
    // --------------------------------------
    //
    // If at some point, a Gekko inbuilt function/procedure is provided in Gekko .gcm form, perhaps it could
    // be stated as a string inside Functions.cs, and loaded via RunGekkoCommands() or something akin to the way
    // library zip-files are loaded.

    /// <summary>
    /// Functionality dealing with the LIBRARY command.
    /// </summary>
    public class Libraries
    {
        /// <summary>
        /// Active libraries (excluding the Glbobal library). There are also non-active libraries in .libraryCache.
        /// </summary>        
        private List<Library> libraries = new List<Library>();        
        private Library globalLibrary = new Library(Globals.globalLibraryString, null, new DateTime(0l));

        /// <summary>
        /// All historically loaded libraries. Active (non-Global) libraries are in .libraries.
        /// </summary>        
        private List<Library> libraryCache = new List<Library>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public Libraries()
        {                     
        }

        /// <summary>
        /// If "Global", "Local" or "Gekko"
        /// </summary>
        /// <param name="libraryName"></param>
        /// <returns></returns>
        public static bool IsReservedName(string libraryName)
        {
            return G.Equal(libraryName, Globals.globalLibraryString) || G.Equal(libraryName, Globals.localLibraryString) || G.Equal(libraryName, Globals.gekkoLibraryString);
        }

        /// <summary>
        /// Handles LIBRARY ? and LIBRARY ? name.
        /// </summary>
        public static void Q(IVariable iv)
        {
            string name = null;
            if (iv != null) name = O.ConvertToString(iv);

            bool hit = false;
            using (Writeln writeln = new Writeln())
            {
                writeln.MainAdd("Libraries:");
                writeln.MainNewLineTight();
                int counter = 0;
                
                foreach (Library library in Program.libraries.GetLibrariesIncludingGlobal())
                {
                    counter++;
                    if (name != null && !G.Equal(library.GetName(), name))
                    {
                        continue;
                    }
                    else
                    {
                        hit = true;
                    }

                    List<string> functions, procedures;
                    QHelper(library, out functions, out procedures);

                    Action<GAO> a = (gao) =>
                    {
                        Action<GAO> a3 = (gao3) =>
                        {
                            string fname = gao3.s1;
                            if (fname.EndsWith("()")) fname = fname.Substring(0, fname.Length - "()".Length);
                            else fname = Globals.procedure + fname;
                            GekkoFunction f = library.GetFunction(fname, true);  //should be there
                            new Writeln(f.code);
                        };

                        string ff5 = null;
                        foreach (string s in functions)
                        {
                            ff5 += G.GetLinkAction(s, new GekkoAction(EGekkoActionTypes.Unknown, null, a3, new GAO() { s1 = s })) + ", ";
                        }
                        string pp5 = null;
                        foreach (string s in procedures)
                        {
                            pp5 += G.GetLinkAction(s, new GekkoAction(EGekkoActionTypes.Unknown, null, a3, new GAO() { s1 = s })) + ", ";
                        }
                        using (Writeln writeln2 = new Writeln())
                        {
                            writeln2.MainAdd("Library '" + library.GetName() + "':");
                            writeln2.MainNewLineTight();
                            if (functions.Count > 0)
                            {
                                writeln2.MainAdd(G.AddS(functions.Count, "function") + ": " + ff5.Substring(0, ff5.Length - ", ".Length));
                                writeln2.MainNewLineTight();
                            }
                            if (procedures.Count > 0) writeln2.MainAdd(G.AddS(procedures.Count, "procedure") + ": " + pp5.Substring(0, pp5.Length - ", ".Length));
                        }
                    };

                    string more = null;
                    if (functions.Count + procedures.Count > 0)
                    {
                        more = " (" + G.GetLinkAction("more", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ")";
                    }
                    writeln.MainAdd("'" + library.GetName() + "' with " + G.AddS(functions.Count, "function") + " and " + G.AddS(procedures.Count, "procedure") + more);
                    writeln.MainNewLineTight();
                }
                if (name != null && !hit)
                {
                    new Writeln("Could not find library '" + name + "'");
                }
            }
        }

        private static void QHelper(Library library, out List<string> functions, out List<string> procedures)
        {
            functions = new List<string>();
            procedures = new List<string>();
            foreach (string s in library.GetFunctionNames())
            {
                if (s.StartsWith(Globals.procedure)) procedures.Add(s.Substring(Globals.procedure.Length));
                else functions.Add(s + "()");
            }
        }

        /// <summary>
        /// Returns a list of libraries. Includes the Global library as the last element.
        /// </summary>
        /// <returns></returns>
        public List<Library> GetLibrariesIncludingGlobal()
        {
            List<Library> temp = new List<Library>();
            temp.AddRange(this.libraries);
            temp.Add(this.globalLibrary);
            return temp;
        }

        /// <summary>
        /// Get a library via a name. Option to choose if Gekko should abort or return null if it does not exist.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="abortWithError"></param>
        /// <returns></returns>
        public Library GetLibrary(string name, bool abortWithError)
        {
            //By convention, calling with null means the Global library. This is the default place
            //for functions/procedures.
            
            if (name == null) return this.globalLibrary;

            foreach (Library lib in this.GetLibrariesIncludingGlobal())
            {
                if (G.Equal(lib.GetName(), name))
                {                    
                    return lib;
                }
            }

            if (abortWithError) new Error("Library '" + name + "' could not be found.");

            return null;
        }

        /// <summary>
        /// Get a function from the loaded libraries/packages. If libraryName == null, Gekko will search for the function,
        /// in the list of libraries (first opened first). If libraryName != null, only this particular library will be searched
        /// for the function. When libraryName == null, and callingLibraryName != null, the callingLibraryName will be searched
        /// first, so that functions in the same library have priority.
        /// We are obtaining a GekkoFunction of a particular name, and the GekkoFunction contains all overloads inside.
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public GekkoFunction GetFunction(string callingLibraryName, string libraryName, string functionName)
        {            
            GekkoFunction rv = null;

            if (libraryName != null)
            {
                //explicit calling, like lib1:f1().
                Library thisLib = this.GetLibrary(libraryName, true);
                rv = thisLib.GetFunction(functionName, true);
            }
            else
            {
                if (callingLibraryName == null)
                {
                    foreach (Library thisLib in this.GetLibrariesIncludingGlobal())
                    {
                        rv = thisLib.GetFunction(functionName, false);
                        if (rv != null) break;
                    }
                }
                else
                {
                    //first we try the calling library
                    Library callingLibrary = this.GetLibrary(callingLibraryName, true);  //cannot give an error, must exist
                    rv = callingLibrary.GetFunction(functionName, false);
                    if (rv == null)
                    {
                        //then we search the rest normally
                        foreach (Library thisLib2 in this.GetLibrariesIncludingGlobal())
                        {
                            if (thisLib2 == callingLibrary) continue;  //skip this, we have tested it, and the function does not exist.
                            rv = thisLib2.GetFunction(functionName, false);
                            if (rv != null) break;
                        }
                    }
                }

                if (rv == null)
                {
                    if (functionName == Globals.stopHelper)
                    {
                        using (Writeln writeln = new Writeln())
                        {
                            writeln.MainAdd("-------------------------------------------------------------");
                            writeln.MainNewLineTight();
                            writeln.MainAdd("------------ The job was stopped by STOP command ------------");
                            writeln.MainNewLineTight();
                            writeln.MainAdd("-------------------------------------------------------------");
                        }                        
                    }
                    else
                    {
                        string s = "function '" + functionName + "()'";
                        if (functionName.StartsWith(Globals.procedure)) s = "procedure '" + functionName.Substring(Globals.procedure.Length) + "'";
                        new Error("The " + s + " does not seem to exist.");
                    }
                }
            }
            return rv;
        }

        /// <summary>
        /// Helper for the LIBRARY command. Unzips the library, extracts the function/procedure code, puts this in
        /// a Library object for later use. Does not compile the code, this is done lazily.
        /// </summary>
        /// <param name="o"></param>
        public void LoadLibraryFromZip(O.Library o)
        {
            for (int i = 0; i < o.files.Count; i++)
            {
                string fileName3 = O.ConvertToString(o.files[i]);
                string fileName2 = G.AddExtension(fileName3, "." + "zip");
                string libraryName = Path.GetFileNameWithoutExtension(fileName2);
                if (o.aliases[i] != null) libraryName = O.ConvertToString(o.aliases[i]);
                if (IsReservedName(libraryName)) new Error("The name '" + libraryName + "' is reserved regarding libraries");
                List<string> folders = new List<string>();
                folders.Add(Program.options.folder_command);
                folders.Add(Program.options.folder_command1);
                folders.Add(Program.options.folder_command2);
                string fileNameWithPath = Program.FindFile(fileName2, folders);  //also calls CreateFullPathAndFileName()
                if (fileNameWithPath == null)
                {
                    new Error("Could not find library file: " + fileName2);
                }

                // library p1; library p2; library p3;   installed = p1, p2, p3     libraries = p1, p2, p3
                // library <remove> p2;                  installed = p1, p2, p3     libraries = p1, p3
                // library p2;                           installed = p1, p2, p3     libraries = p1, p3, p2            

                if (libraryName == null)
                {
                    new Error("Library name cannot be null");  //probably cannot happen?
                }

                foreach (Library x in this.libraries)
                {
                    if (G.Equal(x.GetName(), libraryName))
                    {
                        new Error("There is already a library with the name '" + libraryName + "', cf. LIBRARY ?.");
                    }
                    if (G.Equal(x.GetFileNameWithPath(), fileNameWithPath))
                    {
                        using (Error error = new Error())
                        {
                            error.MainAdd("The existing library '" + x.GetName() + "' also represents the file '" + fileNameWithPath + "', and the same library .zip file cannot be loaded/opened two times");
                        }
                    }
                }

                DateTime stamp = File.GetLastWriteTime(fileNameWithPath);                

                //We know that the zip file name is not in .libraries, but it could be in cache.
                //If so, no need to process the zip file, just take the cache object.
                Library hit = this.CheckCache(fileNameWithPath);

                if (hit != null && hit.GetStamp().Ticks != 0l && stamp.CompareTo(hit.GetStamp()) != 0)
                {
                    //if .Tics == 0l, it is the Global library.
                    //if not, we compare the stamp from the .zip file with the stored stamp.
                    //if different, we need to reload the .zip (because it has been altered).
                    //so we just remove it from cache, and sets hit = null.
                    hit = null;
                    this.RemoveFromCache(fileNameWithPath);  //further down, the updated .zip file will be put into the cache, so the old version needs to be removed first.
                }

                Library library = null;
                if (hit != null)
                {
                    //found in cache, has been loaded before
                    library = hit;
                    //it may be stored in the .libraryCache under another name than the name part of the zip file (if LIBRARY ... AS ... has been used). 
                    //It is not legal to open the same zip file as two different libraries, so there should be no dangers here.
                    //When setting this name, it is changed both in .library and .libraryCache, since it is the same object.
                    library.SetName(libraryName);
                }
                else
                {
                    string tempPath = Program.GetTempGbkFolderPath();
                    if (!Directory.Exists(tempPath))  //should almost never exist, since name is random
                    {
                        Directory.CreateDirectory(tempPath);
                    }
                    else
                    {
                        Directory.Delete(tempPath, true);  //in the very rare case, any files here will be deleted first
                    }

                    Program.WaitForZipRead(tempPath, fileNameWithPath);
                    library = new Library(libraryName, fileNameWithPath, stamp);
                    library.LibraryExtractor(tempPath, fileNameWithPath);

                    try
                    {
                        G.DeleteFolder(tempPath);
                    }
                    catch
                    {
                        //not catastrofic if this fails
                    }

                    //all files loaded from .zip end up here, and the cache only grows (cannot shrink).
                    this.libraryCache.Add(library);  //if a lib is closed and reopned, this can be done fast.
                }                    

                this.libraries.Add(library);
                List<string> functions, procedures;
                QHelper(library, out functions, out procedures);
                new Writeln("Loaded library '" + libraryName + "' with " + G.AddS(functions.Count, "function") + " and " + G.AddS(procedures.Count, "procedure")+". Library path: " + fileNameWithPath);
            }
        }

        /// <summary>
        /// Find a Library object from cache.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private Library CheckCache(string fileName)
        {
            Library hit = null;
            foreach (Library x in this.libraryCache)
            {
                if (G.Equal(x.GetFileNameWithPath(), fileName))
                {
                    hit = x;
                    break;
                }
            }
            return hit;
        }

        /// <summary>
        /// Remove the lib file from cache. Only call if CheckCache() returns not-null.
        /// </summary>
        /// <param name="fileName"></param>
        private void RemoveFromCache(string fileName)
        {
            bool hit = false;
            List<Library> temp = new List<Library>();
            foreach (Library x in this.libraryCache)
            {
                if (G.Equal(x.GetFileNameWithPath(), fileName))
                {
                    hit = true;
                }
                else
                {
                    temp.Add(x);
                }
            }

            if (hit == false)
            {
                new Error("Internal error #7899342y34357, could not remove from cache");
            }

            this.libraryCache = temp;

            return;
        }

        /// <summary>
        /// Close the library, so its functions cannot be used. The library will stay in the cache, though.
        /// </summary>
        /// <param name="libraryName"></param>
        public void CloseLibrary(string libraryName)
        {
            if (IsReservedName(libraryName)) new Error("The name '" + libraryName + "' is special and cannot be closed. You may try LIBRARY<clear> instead.");

            if (libraryName == "*")
            {
                int count = this.libraries.Count;  //excludes 'Global'                
                this.libraries = new List<Library>();
                //the Global library is not touched.
                new Writeln("Closed " + G.AddS(count, "library") + ", not including the Global library).");
            }
            else
            {                
                List<Library> temp = new List<Library>();
                bool found = false;
                foreach (Library x in this.libraries)  //excludes Global
                {
                    if (G.Equal(x.GetName(), libraryName))
                    {
                        found = true;
                    }
                    else
                    {
                        temp.Add(x);
                    }
                }
                if (found == false) new Error("Library '" + libraryName + "' is not opened/loaded, and can therefore not be closed.");
                this.libraries = temp;
                new Writeln("Closed library '" + libraryName + "'");
            }
        }

        public void ClearLibrary(string libraryName)
        {
            if (G.Equal(libraryName, Globals.globalLibraryString))
            {
                this.globalLibrary = new Library(Globals.globalLibraryString, null, new DateTime(0l));
            }
            else
            {
                new Error("You can only clear 'Global' (the global library)");
            }
        }

    }

    /// <summary>
    /// All the functions 'f', 'g', etc. for this package
    /// </summary>
    public class Library
    {
        /// <summary>
        /// Name of the library, like in LIBRARY mylib;
        /// </summary>
        private string name = null;

        /// <summary>
        /// Filename of zip. Can be null, for the global library.
        /// </summary>
        private string fileNameWithPath = null;

        /// <summary>
        /// Stamp: when the file was written.
        /// </summary>
        private DateTime stamp = new DateTime(0l);  //null

        GekkoFunction fastLookup = null;
                
        /// <summary>
        /// Create a new package/library.
        /// </summary>
        /// <param name="name"></param>
        public Library(string name, string fileNameWithPath, DateTime stamp)
        {
            //this.name = name;
            //this.fileNameWithPath = fileNameWithPath;
            if (name != null) this.name = name.Trim();
            if (fileNameWithPath != null) this.fileNameWithPath = fileNameWithPath.Trim();
            this.stamp = stamp;
        }

        /// <summary>
        /// Name of package/library
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.name;
        }

        public void SetName(string s)
        {
            this.name = s;
        }

        public string GetFileNameWithPath()
        {
            return this.fileNameWithPath;
        }

        public DateTime GetStamp()
        {
            return this.stamp;
        }

        private GekkoDictionary<string, GekkoFunction> functions = new GekkoDictionary<string, GekkoFunction>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Adds a function to the library.
        /// </summary>
        /// <param name="function"></param>
        public void AddFunction(GekkoFunction function)
        {
            function.libraryName = this.GetName();
            this.functions.Add(function.GetName(), function);            
        }

        /// <summary>
        /// Gets a list of function names inside the library. Does not show overloads (number of arguments), just the names proper.
        /// </summary>
        /// <returns></returns>
        public List<string> GetFunctionNames()
        {
            List<string> ss = new List<string>(this.functions.Keys);
            ss.Sort(StringComparer.InvariantCultureIgnoreCase);
            return ss;
        }

        /// <summary>
        /// Find a library by name, for instance argument 'f' if we are finding f(), or f(...) or f(..., ...). This code has to be fast, could be inside a tight loop.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GekkoFunction GetFunction(string name, bool abortWithError)
        {
            if (this.fastLookup != null)
            {
                //fast check, will avoid dict lookup if there is a hit
                if (G.Equal(name, fastLookup.GetName())) return fastLookup;
            }
            GekkoFunction rv = null;
            this.functions.TryGetValue(name, out rv);
            if (rv == null)
            {
                string s = "Function '" + name + "()'";
                if (name.StartsWith(Globals.procedure)) s = "Procedure '" + name.Substring(Globals.procedure.Length) + "'";
                if (abortWithError) new Error(s + " not found in library '" + this.name + "'");
            }
            else
            {
                this.fastLookup = rv;
            }
            return rv;
        }        

        /// <summary>
        /// Finds all .gcm files in a folder structure, and extracts functions/procedures.
        /// </summary>
        /// <param name="targetDirectory"></param>
        public void LibraryExtractor(string targetDirectory, string zipFileName)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                if (fileName.EndsWith("." + Globals.extensionCommand, StringComparison.OrdinalIgnoreCase))
                {
                    this.LibraryExtractorHandleGcmFile(fileName, targetDirectory, zipFileName);
                }
            }

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                this.LibraryExtractor(subdirectory, zipFileName);
            }
        }

        /// <summary>
        /// Extract function/procedure code as text.
        /// </summary>
        /// <param name="file"></param>
        private void LibraryExtractorHandleGcmFile(string file, string targetDirectory, string zipFileName)
        {
            // ...
            // ...
            // function ...           --> or procedure
            // ...
            // ...
            // function ...
            // ...
            // ...

            // will be cut like this:

            //=========================================
            // ...
            // ...
            // function ...                                functionNames[0], functionCounter = 1
            // ...
            // ...
            //=========================================
            // function ...                                functionNames[1], functionCounter = 2
            // ...
            // ...
            //=========================================

            //It basically cuts before second "function" and so on.

            string pathToFileInsideZip = zipFileName + file.Replace(targetDirectory, "");

            string s = Program.GetTextFromFileWithWait(file);
            int fat = 5;
            var tags1 = new List<Tuple<string, string>>() { new Tuple<string, string>("/*", "*/") };
            var tags2 = new List<string>() { "//" };
            TokenHelper t2 = StringTokenizer.GetTokensWithLeftBlanksRecursive(s, tags1, tags2, null, null);

            int functionCounter = 0;
            int i0 = 0;
            List<string> functionNamesLower = new List<string>();
            for (int i = 0; i < t2.subnodes.Count(); i++)
            {
                //for instance "function val f(val %x); ... ; end;"   --> function must be followed by two words at least
                //or "procedure f val %x; ... ; end;                  --> procedure must be followed by one word at least
                //both must be first line or follow a ";"
                //this will guard against for instance series definitions like "function = 3;" or "procedure = 3;" (unlikely though).

                if (t2.subnodes[i].type == ETokenType.Word && (G.Equal(t2.subnodes[i].s, "function") || G.Equal(t2.subnodes[i].s, "procedure")))
                {
                    bool isFunction = false;
                    if (G.Equal(t2.subnodes[i].s, "function")) isFunction = true;

                    TokenHelper th1 = null;
                    TokenHelper th2 = null;
                    TokenHelper th3 = null;
                    th1 = t2.subnodes[i].SiblingBefore(1, true);
                    th2 = t2.subnodes[i].SiblingAfter(1, true);
                    th3 = t2.subnodes[i].SiblingAfter(2, true);
                    bool problem = false;
                    if (th1 != null && th1.s != ";") problem = true;
                    if (th2 == null || th2.type != ETokenType.Word) problem = true; //
                    if (isFunction && (th3 == null || th3.type != ETokenType.Word)) problem = true;

                    if (!problem)
                    {

                        functionCounter++;

                        if (isFunction)
                        {
                            TokenHelper temp2 = t2.subnodes[i].SiblingAfter(2, true);  //skips comments, newlines, etc.
                            functionNamesLower.Add(temp2.s.ToLower());
                        }
                        else
                        {
                            TokenHelper temp2 = t2.subnodes[i].SiblingAfter(1, true);  //skips comments, newlines, etc.
                            functionNamesLower.Add(Globals.procedure + temp2.s.ToLower());
                        }

                        if (functionCounter >= 2)
                        {
                            this.LibraryExtractorGetFunctionCode(i0, i, functionNamesLower[functionCounter - 2], t2.subnodes, pathToFileInsideZip);
                            i0 = i;
                        }
                    }
                }
            }

            if (functionCounter > 0) this.LibraryExtractorGetFunctionCode(i0, t2.subnodes.Count(), functionNamesLower[functionNamesLower.Count - 1], t2.subnodes, pathToFileInsideZip);  //get the rest
        }

        /// <summary>
        /// Extracts the code (as text) corresponding to one function/procedure definition. Puts it into the corresponding library object.
        /// Starts at token i0 (included), and ends 1 token before token i (so i is excluded).
        /// </summary>
        /// <param name="library"></param>
        /// <param name="i0"></param>
        /// <param name="i"></param>
        /// <param name="th"></param>
        /// <param name="t"></param>
        private void LibraryExtractorGetFunctionCode(int i0, int i, string functionNameLower, TokenList t, string file)
        {
            StringBuilder sb = new StringBuilder();
            for (int i2 = i0; i2 < i; i2++)
            {
                sb.Append(t[i2].ToString());
            }

            GekkoFunction function = null;
            function = this.GetFunction(functionNameLower, false);
            if (function == null)
            {
                function = new GekkoFunction(functionNameLower);
                this.AddFunction(function);
            }

            int offset = t[i0].line;
            function.code += G.NL;
            function.code += Globals.libraryZipfileIndicator + file + "¤" + offset + G.NL;
            function.code += sb.ToString();
        }

    }

    public class GekkoFunction
    {
        public GekkoFunction(string name)
        {
            this.name = name;
        }

        private string name = null; //for instance 'f'
        public string libraryName = null;  //where the function is stored
        public bool hasBeenCompiled = false;

        public string code = null;  //may contain code from several places, snippets of f(), f(...), f(..., ...)
        // ---------------------------------    
        public Func<GekkoSmpl, P, bool, IVariable> function0 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, IVariable> function1 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, IVariable> function2 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, IVariable> function3 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function4 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function5 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function6 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function7 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function8 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function9 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function10 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function11 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function12 = null;
        public Func<GekkoSmpl, P, bool, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, GekkoArg, IVariable> function13 = null;
        
        /// <summary>
        /// Name of function, irrespective of overloads.
        /// </summary>
        public string GetName()
        {
            return this.name;
        }
    }
}