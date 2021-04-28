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

    /* same logic as databanks. Starts out like this:
     * 
     *             gekko (reserved)
     *             global (reserved, also local).
     *             lib1
     *             lib2
     *             
     * when using global:f() calling g(), it first searches global, then lib1, then lib2.
     * when using lib2:f() calling g(), it first searches lib2, then global, then lib1.
     * LIBRARY lib1, lib2; LIBRARY<first>...; LIBRARY<last>; LIBRARY<clear>; LIBRARY<remove>...;
     * BLOCK lib1, lib2, lib3 --> libs are restored after END.
     * can use y = lib2:f(x).
     * 
     * getlibraries(), setlibraries().
     * Gekko will look for .zip libraries the same places as for RUN commands.
     * LIBRARY xx will look for xx.zip. If it contains no gcm files --> error.
     * 
     * the gekko library cannot be masked by others, else --> error. This COULD be overwritten with option?
     * 
     * 
     * 
     */


    /// <summary>
    /// Functionality dealing with the LIBRARY command.
    /// </summary>
    public class Libraries
    {
        /// <summary>
        /// All items (loaded packages). Never unloaded, so the items list can only grow.
        /// The hierarchy list tells which packages are actually active, and in what order.
        /// </summary>
        //private List<Library> librariesCache = new List<Library>();
        private List<Library> libraries = new List<Library>();  //order of packages
        private Library cache = null;

        public Libraries()
        {
            this.Add(new Library(Globals.globalLibraryString, null));
        }

        public Library GetLibrary(string name2, bool abortWithError)
        {
            //if library name == null, it is understood as "global".

            Library rv = null;

            string name = Globals.globalLibraryString;
            if (name2 != null) name = name2.ToLower();

            if (this.cache != null)
            {
                //check for fast return if library name is same as last time
                if (this.cache.GetName() == name) return this.cache;
            }

            foreach (Library lib in this.libraries)
            {
                if (lib.GetName() == name)
                {
                    this.cache = lib;
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
            //functionName = functionName.ToLower(); --> NO! no need to do this!

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
                    new Error("The function '" + functionName.ToLower() + "()' does not seem to exist.");
                }
            }

            return rv;
        }

        /// <summary>
        /// Helper for the LIBRARY command. Unzips the library, extracts the function/procedure code, puts this in
        /// a Library object for later use. Does not compile the code, this is done lazily.
        /// </summary>
        /// <param name="o"></param>
        public static void LoadLibrary(O.Library o)
        {
            for (int i = 0; i < o.files.Count; i++)
            {
                string fileName3 = O.ConvertToString(o.files[i]);
                string fileName2 = G.AddExtension(fileName3, "." + "zip");
                string libraryNameLower = Path.GetFileNameWithoutExtension(fileName2).ToLower();
                if (o.aliases[i] != null) libraryNameLower = O.ConvertToString(o.aliases[i]);
                if (IsReservedName(libraryNameLower)) new Error("The name '" + libraryNameLower + "' is reserved regarding libraries");
                List<string> folders = new List<string>();
                folders.Add(Program.options.folder_command);
                folders.Add(Program.options.folder_command1);
                folders.Add(Program.options.folder_command2);
                string fileName = Program.FindFile(fileName2, folders);  //also calls CreateFullPathAndFileName()
                if (fileName == null)
                {
                    new Error("Could not find library file: " + fileName2);
                }

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

                Library library = new Library(libraryNameLower, fileName);
                Program.LibraryExtractor(tempPath, library);
                Program.functions.Add(library);

                try
                {
                    G.DeleteFolder(tempPath);
                }
                catch
                {
                    //not catastrofic if this fails
                }

                int count = library.GetFunctionNames().Count;

                new Writeln("Loaded library '" + libraryNameLower + "' with " + G.AddS(count, "function") + ". Library path: " + fileName);
            }
        }


        /// <summary>
        /// Add a library/package to Gekko. Packages are generally only added, not removed (until
        /// RESET/RESTART). A library contains functions/methods, often stored in a zip-file.
        /// If the library is already there (loaded), the method is ignored.        /// 
        /// </summary>
        /// <param name="library"></param>
        public void Add(Library library)
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
                if (library.GetName() == x.GetName())
                {
                    new Error("Library '" + library.GetName() + "' is already loaded.");
                }
            }

            this.libraries.Add(library);
        }

        public void Close(string libraryName2)
        {
            if (libraryName2 == "*")
            {
                int count = this.libraries.Count;  //includes 'Global'
                Library global = this.GetLibrary(Globals.globalLibraryString, true); //cannot be non-existing
                this.libraries = new List<Library>();
                this.Add(global);
                new Writeln("Removed " + G.AddS(count - 1, "library") + " (excluding Global library).");
            }
            else
            {
                string libraryName = libraryName2.ToLower();
                List<Library> temp = new List<Library>();
                bool found = false;
                foreach (Library x in this.libraries)
                {
                    if (libraryName == x.GetName())
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

        private static bool IsReservedName(string libraryName)
        {
            return G.Equal(libraryName, Globals.globalLibraryString) || G.Equal(libraryName, Globals.localLibraryString) || G.Equal(libraryName, Globals.gekkoLibraryString);
        }

        public void Clear(string libraryName)
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
        /// The different functions of the library.
        /// </summary>
        /// 

        /// <summary>
        /// Create a new package/library.
        /// </summary>
        /// <param name="name"></param>
        public Library(string name, string fileNameWithPath)
        {
            this.name = name;
            this.fileNameWithPath = fileNameWithPath;
        }

        /// <summary>
        /// Name of package/library
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.name;
        }

        private Dictionary<string, GekkoFunction> functions = new Dictionary<string, GekkoFunction>();

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
            ss.Sort();
            return ss;
        }        

        /// <summary>
        /// Find a library by name, for instance argument 'f' if we are finding f() or f(2, 3). The
        /// argument MUST be lowercase. This code has to be fast, could be inside a tight loop.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GekkoFunction GetFunction(string name, bool abortWithError)
        {
            name = name.ToLower();

            if (this.itemCache != null)
            {
                //fast check, will avoid dict lookup if there is a hit
                if (name == itemCache.GetName()) return itemCache;
            }
            GekkoFunction rv = null;
            this.functions.TryGetValue(name, out rv);
            if (rv == null)
            {
                if (abortWithError) new Error("Function name '" + name + "' not found in library '" + this.name + "'");
            }
            else
            {
                this.itemCache = rv;
            }
            return rv;
        }

        GekkoFunction itemCache = null;
    }

    public class GekkoFunction
    {
        public GekkoFunction(string name)
        {
            this.name = name;
        }

        private string name = null; //for instance 'f'
        public string libraryName = null;
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
