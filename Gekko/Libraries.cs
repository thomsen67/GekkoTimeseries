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
        /// Active libraries. There are also non-active libraries in .libraryCache.
        /// </summary>        
        private List<Library> libraries = new List<Library>();

        /// <summary>
        /// All historically loaded libraries. Active libraries are in .libraries.
        /// </summary>        
        private List<Library> libraryCache = new List<Library>();

        /// <summary>
        /// Constructor.
        /// </summary>
        public Libraries()
        {
            //Is always born with an empty Global library
            this.libraries.Add(new Library(Globals.globalLibraryString, null));
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
                foreach (Library library in Program.libraries.GetLibraries())
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

                    List<string> functions = new List<string>();
                    List<string> procedures = new List<string>();
                    foreach (string s in library.GetFunctionNames())
                    {
                        if (s.StartsWith(Globals.procedure)) procedures.Add(s.Substring(Globals.procedure.Length));
                        else functions.Add(s + "()");
                    }

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

        /// <summary>
        /// Returns a list of libraries. Do not alter the list: it points directly to the real list.
        /// </summary>
        /// <returns></returns>
        public List<Library> GetLibraries()
        {
            return this.libraries;
        }

        /// <summary>
        /// Get a library via a name. Option to choose if Gekko should abort or return null if it does not exist.
        /// </summary>
        /// <param name="name2"></param>
        /// <param name="abortWithError"></param>
        /// <returns></returns>
        public Library GetLibrary(string name2, bool abortWithError)
        {
            //if library name == null, it is understood as "global".
            Library rv = null;
            string name = Globals.globalLibraryString;
            if (name2 != null) name = name2;

            foreach (Library lib in this.libraries)
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
        /// for the function. 
        /// We are obtaining a GekkoFunction of a particular name, and the GekkoFunction contains all overloads inside.
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public GekkoFunction GetFunction(string libraryName, string functionName)
        {            
            GekkoFunction rv = null;

            if (libraryName != null)
            {
                Library thisLib = this.GetLibrary(libraryName, true);
                rv = thisLib.GetFunction(functionName, true);
            }
            else
            {
                foreach (Library thisLib in  this.libraries)
                {
                    rv = thisLib.GetFunction(functionName, false);
                    if (rv != null) break;
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
                string fileName = Program.FindFile(fileName2, folders);  //also calls CreateFullPathAndFileName()
                if (fileName == null)
                {
                    new Error("Could not find library file: " + fileName2);
                }

                Library hit = this.CheckCache(fileName);

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

                    Program.WaitForZipRead(tempPath, fileName);
                    library = new Library(libraryName, fileName);
                    Program.LibraryExtractor(tempPath, library);

                    try
                    {
                        G.DeleteFolder(tempPath);
                    }
                    catch
                    {
                        //not catastrofic if this fails
                    }
                }

                this.AddLibrary(library);

                int count = library.GetFunctionNames().Count;

                //TODO TODO: use Q() to print funcs/procs separately.

                new Writeln("Loaded library '" + libraryName + "' with " + G.AddS(count, "function") + ". Library path: " + fileName);
            }
        }

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
        /// Add a library/package to Gekko. 
        /// This method is much like adding it to the .library list directly, but
        /// the method also handles the cache.
        /// </summary>
        /// <param name="library"></param>
        public void AddLibrary(Library library)
        {
            // library p1; library p2; library p3;   installed = p1, p2, p3     libraries = p1, p2, p3
            // library <remove> p2;                  installed = p1, p2, p3     libraries = p1, p3
            // library p2;                           installed = p1, p2, p3     libraries = p1, p3, p2            

            if (library.GetName() == null)
            {
                new Error("Library name cannot be null");  //probably cannot happen?
            }

            foreach (Library x in this.libraries)
            {
                if (G.Equal(x.GetName(), library.GetName()))
                {
                    new Error("There is already a library with the name '" + library.GetName() + "', cf. LIBRARY ?.");
                }
                if (G.Equal(x.GetFileNameWithPath(), library.GetFileNameWithPath()))
                {
                    using (Error error = new Error())
                    {
                        error.MainAdd("The existing library '" + x.GetName() + "' also represents the file '" + library.GetFileNameWithPath() + "', and the same library .zip file cannot be loaded/opened two times");
                    }
                }
            }

            this.libraries.Add(library);

            if (library.GetFileNameWithPath() != null)
            {
                //do not add to cache if it is the Global lib, or another lib not from a .zip file.
                Library hit = this.CheckCache(library.GetFileNameWithPath());
                if (hit == null) this.libraryCache.Add(library);  //if a lib is closed and reopned, this can be done fast.
            }

        }

        /// <summary>
        /// Close the library, so its functions cannot be used. The library will stay in the cache, though.
        /// </summary>
        /// <param name="libraryName"></param>
        public void CloseLibrary(string libraryName)
        {
            if (libraryName == "*")
            {
                int count = this.libraries.Count;  //includes 'Global'
                Library global = this.GetLibrary(Globals.globalLibraryString, true); //cannot be non-existing
                this.libraries = new List<Library>();
                this.libraries.Add(global);                
                new Writeln("Closed " + G.AddS(count - 1, "library") + ", not including the Global library).");
            }
            else
            {                
                List<Library> temp = new List<Library>();
                bool found = false;
                foreach (Library x in this.libraries)
                {
                    if (G.Equal(x.GetName(), libraryName))
                    {
                        found = true;
                        if (IsReservedName(libraryName)) new Error("The name '" + libraryName + "' is special and cannot be closed. You may try LIBRARY<clear> instead.");                        
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
            bool found = false;
            for (int i = 0; i < this.libraries.Count; i++)
            {
                Library x = this.libraries[i];                
                if (G.Equal(x.GetName(), libraryName))
                {
                    found = true;
                    if (IsReservedName(libraryName))
                    {
                        this.libraries[i] = new Library(x.GetName(), null);
                        new Writeln("Library '" + libraryName + "' is now cleared (empty).");
                        break;
                    }
                    else
                    {
                        new Error("You cannot clear library '" + libraryName + "' loaded from a .zip file. Use LIBRARY<close> instead.");
                    }
                }
            }
            if (!found) new Error("Could not find library '" + libraryName + "' for clearing.");
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
        /// Create a new package/library.
        /// </summary>
        /// <param name="name"></param>
        public Library(string name, string fileNameWithPath)
        {
            //this.name = name;
            //this.fileNameWithPath = fileNameWithPath;
            if (name != null) this.name = name.Trim();
            if (fileNameWithPath != null) this.fileNameWithPath = fileNameWithPath.Trim();

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
        GekkoFunction fastLookup = null;
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