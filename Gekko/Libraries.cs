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

        private Library GetLibrary(string name)
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

            new Error("Library '" + name + "' could not be found.");

            return null;  //will actually never get here
        }

        private Library GetLibrary(int i)
        {
            return this.libraries[i];
        }

        public GekkoFunction GetFunction(string functionName)
        {
            functionName = functionName.ToLower();
            GekkoFunction thisFunction = null;
            foreach (int i in this.GetHierarchy())
            {
                Library thisLib = this.GetLibrary(i);
                thisFunction = thisLib.GetFunction(functionName);
                if (thisFunction != null) break;
            }

            if(thisFunction==null)
            {
                new Error("Function '" + functionName + "' was not found.");
            }

            return thisFunction;
        }

        public GekkoFunction GetFunction(string libraryName, string functionName)
        {
            Library library = this.GetLibrary(libraryName);
            GekkoFunction function = library.GetFunction(functionName);
            return function;
        }        

        private List<int> GetHierarchy()
        {
            return this.hierarchy;
        }

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

            //putting the new id FIRST in hierarchy (will be searched first)
            List<int> temp = this.hierarchy;
            this.hierarchy = new List<int>();
            this.hierarchy.Add(lib.id);
            this.hierarchy.AddRange(temp);
            temp = null;  //to free the memory fast
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

        public void SetName(string s)
        {
            this.name = s.ToLower();
        }

        public string GetName()
        {
            return this.name;
        }

        private Dictionary<string, GekkoFunction> functions = new Dictionary<string, GekkoFunction>();

        public void AddFunction(GekkoFunction function)
        {
            this.functions.Add(function.GetName(), function);            
        }

        /// <summary>
        /// Find a library by name, for instance argument 'f' if we are finding f() or f(2, 3). The
        /// argument MUST be lowercase. This code has to be fast, could be inside a tight loop.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GekkoFunction GetFunction(string name)
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
                //error?
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
        private string name = null; //for instance 'f'

        string code = null;  //may contain code from several places, snippets of f(), f(...), f(..., ...)
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

        public void SetName(string s)
        {
            this.name = s.ToLower();
        }

        public string GetName()
        {
            return this.name;
        }
    }
}
