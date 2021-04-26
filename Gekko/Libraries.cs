using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private List<Library> libraries = new List<Library>();
        private List<int> hierarchy = new List<int>();  //order of packages
        private Library cache = null;

        public Libraries()
        {
            this.Add(new Library(Globals.globalLibraryString));
        }

        public Library GetLibrary(string name, bool abortWithError)
        {
            name = name.ToLower();

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

        private Library GetLibrary(int i)
        {
            return this.libraries[i];
        }

        /// <summary>
        /// Get a function from the loaded libraries/packages. Gekko will search for the function,
        /// starting with the last loaded package.
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public GekkoFunction GetFunction(string functionName)
        {
            //functionName = functionName.ToLower(); --> NO! no need to do this!
            GekkoFunction thisFunction = null;
            foreach (int i in this.GetHierarchy())
            {
                Library thisLib = this.GetLibrary(i);
                thisFunction = thisLib.GetFunction(functionName, false);
                if (thisFunction != null) break;
            }

            if(thisFunction==null)
            {
                new Error("Function '" + functionName.ToLower() + "' was not found.");
            }

            return thisFunction;
        }

        /// <summary>
        /// Get a function from a particular library/package, like p1:f(...).
        /// </summary>
        public GekkoFunction GetFunction(string libraryName, string functionName)
        {
            Library library = this.GetLibrary(libraryName, true);
            GekkoFunction function = library.GetFunction(functionName, false);
            return function;
        }        

        private List<int> GetHierarchy()
        {
            return this.hierarchy;
        }

        /// <summary>
        /// Add a library/package to Gekko. Packages are generally only added, not removed (until
        /// RESET/RESTART). A library contains functions/methods, often stored in a zip-file.
        /// If the library is already there (loaded), the method is ignored.        /// 
        /// </summary>
        /// <param name="lib"></param>
        public void Add(Library lib)
        {
            if (lib.GetName() == null)
            {
                new Error("Library name cannot be null");
            }

            foreach (Library x in this.libraries)
            {
                if (lib.GetName() == x.GetName())
                {
                    new Error("Library '" + lib.GetName() + "' has already been loaded.");
                }
            }
            lib.id = this.libraries.Count;
            this.libraries.Add(lib);

            if (true)
            {
                //putting the new id LAST in hierarchy (will be searched last)                
                this.hierarchy.Add(lib.id);
            }

            if (false)
            {
                //putting the new id FIRST in hierarchy (will be searched last)
                List<int> temp = this.hierarchy;
                this.hierarchy = new List<int>();
                this.hierarchy.Add(lib.id);
                this.hierarchy.AddRange(temp);
                temp = null;  //to free the memory fast
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
        /// The different functions of the library.
        /// </summary>
        /// 

        public int id = -12345;

        /// <summary>
        /// Create a new package/library.
        /// </summary>
        /// <param name="name"></param>
        public Library(string name)
        {
            this.name = name;
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

        public void AddFunction(GekkoFunction function)
        {
            function.packageName = this.GetName();
            this.functions.Add(function.GetName(), function);            
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
        public string packageName = null;
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
