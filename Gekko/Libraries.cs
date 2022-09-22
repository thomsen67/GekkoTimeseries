﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;
using System.IO.Compression;

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
        private Library localLibrary = new Library(Globals.localLibraryString, null, new DateTime(0l));        

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
        /// If "Global", "Local", "this", or starts with "Gekko"
        /// </summary>
        /// <param name="libraryName"></param>
        /// <returns></returns>
        public static bool IsReservedName(string libraryName)
        {
            return G.Equal(libraryName, Globals.globalLibraryString) || G.Equal(libraryName, Globals.localLibraryString) || G.Equal(libraryName, Globals.thisLibraryString) || G.Equal(libraryName, Globals.nullLibraryString) || libraryName.ToLower().StartsWith(Globals.gekkoLibraryString.ToLower());
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
                writeln.MainAdd("---------------------------------------------------------------");
                writeln.MainNewLineTight();
                writeln.MainAdd("Libraries:");
                writeln.MainNewLineTight();
                writeln.MainAdd("---------------------------------------------------------------");
                writeln.MainNewLineTight();
                int counter = 0;
                
                foreach (Library library in Program.libraries.GetLibrariesIncludingLocal())
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

                    List<string> functions, procedures, files;
                    QHelper(library, out functions, out procedures, out files);                    

                    string more = null;
                    if (functions.Count + procedures.Count + files.Count > 0)
                    {
                        more = " (" + G.GetLinkAction("info", new GekkoAction(EGekkoActionTypes.Unknown, null, QHelperActions(library, functions, procedures, files))) + ")";
                    }
                    writeln.MainAdd("'" + library.GetName() + "' with " + G.AddS(functions.Count, "function") + ", " + G.AddS(procedures.Count, "procedure") + ", " + G.AddS(files.Count, "file") + more);
                    writeln.MainNewLineTight();
                }
                writeln.MainNewLineTight();
                if (name != null && !hit)
                {
                    writeln.MainAdd("--> could not find library '" + name + "'");
                    writeln.MainNewLineTight();
                }
                writeln.MainAdd("---------------------------------------------------------------");                
            }
        }

        private static Action<GAO> QHelperActions(Library library, List<string> functions, List<string> procedures, List<string> files)
        {
            Action<GAO> a = (gao) =>
            {
                Action<GAO> a3 = (gao3) =>
                {
                    string fname = gao3.s1;
                    if (fname.EndsWith("()")) fname = fname.Substring(0, fname.Length - "()".Length);
                    else fname = Globals.procedure + fname;
                    GekkoFunction f = library.GetFunction(fname, true);  //should be there

                    using (Writeln w = new Writeln("", -12345, System.Drawing.Color.Empty, false, ETabs.Output))
                    {
                        w.lineWidth = int.MaxValue;
                        foreach (GekkoFunctionCode gfc in f.overloads)
                        {
                            List<string> xx = Stringlist.ExtractLinesFromText(gfc.code);
                            if (xx.Count > 1000)
                            {
                                List<string> temp = new List<string>();
                                for (int i = 0; i < 900; i++)
                                {
                                    temp.Add(xx[i]);
                                }
                                temp.Add("");
                                temp.Add("---------------------------------------------------------------------");
                                temp.Add("...");
                                temp.Add("...");
                                temp.Add("File has " + xx.Count + " lines and has been truncated here");
                                temp.Add("...");
                                temp.Add("...");
                                temp.Add("---------------------------------------------------------------------");
                                temp.Add("");
                                for (int i = xx.Count - 20; i < xx.Count; i++)
                                {
                                    temp.Add(xx[i]);
                                }
                                xx = temp;
                            }
                            StringBuilder code = Stringlist.ExtractTextFromLines(xx);

                            //See also this: #08975389245253     
                            w.MainAdd("//[file = " + gfc.fileNameWithPath + " line " + gfc.line + "]");
                            w.MainNewLineTight();
                            w.MainAdd(code.ToString());
                            w.MainNewLine();
                        }
                    }
                };

                string functions_string = null;
                foreach (string s in functions)
                {
                    functions_string += G.GetLinkAction(s, new GekkoAction(EGekkoActionTypes.Unknown, null, a3, new GAO() { s1 = s })) + ", ";
                }
                string procedures_string = null;
                foreach (string s in procedures)
                {
                    procedures_string += G.GetLinkAction(s, new GekkoAction(EGekkoActionTypes.Unknown, null, a3, new GAO() { s1 = s })) + ", ";
                }
                string files_string = null;
                foreach (string s in files)
                {
                    files_string += s + ", ";
                }
                using (Writeln txt = new Writeln())
                {
                    txt.MainAdd("---------------------------------------------------------------");
                    txt.MainNewLineTight();
                    txt.MainAdd("Library '" + library.GetName() + "':");
                    txt.MainNewLineTight();
                    txt.MainAdd("---------------------------------------------------------------");
                    txt.MainNewLineTight();
                    if (functions.Count > 0)
                    {
                        txt.MainAdd(G.AddS(functions.Count, "function") + ": " + functions_string.Substring(0, functions_string.Length - ", ".Length));
                        txt.MainNewLineTight();
                    }
                    if (procedures.Count > 0)
                    {
                        txt.MainAdd(G.AddS(procedures.Count, "procedure") + ": " + procedures_string.Substring(0, procedures_string.Length - ", ".Length));
                        txt.MainNewLineTight();
                    }
                    if (files.Count > 0)
                    {
                        txt.MainAdd(G.AddS(files.Count, "file") + ": " + files_string.Substring(0, files_string.Length - ", ".Length));
                        txt.MainNewLineTight();
                    }
                    txt.MainAdd("---------------------------------------------------------------");
                }
            };
            return a;
        }

        private static void QHelper(Library library, out List<string> functions, out List<string> procedures, out List<string> files)
        {
            functions = new List<string>();
            procedures = new List<string>();
            files = new List<string>();
            foreach (string s in library.GetFunctionNames())
            {
                if (s.StartsWith(Globals.procedure)) procedures.Add(G.FromLibraryToFunctionProcedureName(s, 1));
                else functions.Add(G.FromLibraryToFunctionProcedureName(s, 1));
            }
            files = new List<string>(library.GetDataFiles().Keys);
            files.Sort(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns a list of libraries. Includes the Local library as the first element.
        /// </summary>
        /// <returns></returns>
        public List<Library> GetLibrariesIncludingLocal()
        {
            List<Library> temp = new List<Library>();
            temp.Add(this.localLibrary);
            temp.AddRange(this.libraries);            
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
            //By convention, calling with null means the Local library. This is the default place
            //for functions/procedures.
            
            if (name == null) return this.localLibrary;

            foreach (Library lib in this.GetLibrariesIncludingLocal())
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
        /// Note that callingLibraryName is never = "Local", in that case it is = null.
        /// We are obtaining a GekkoFunction of a particular name, and the GekkoFunction contains all overloads inside.
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public GekkoFunction GetFunction(string callingLibraryName, string libraryName, string functionName)
        {
            // +--------------- #kja890adsfjkaas1 ------------------+
            // |                                                    |
            // |    Note that something very similar takes place    |
            // |    in Program.FindFile(). This method also         |
            // |    deals with "this" and "__" prefix, etc.         |            
            // |                                                    |            
            // +----------------------------------------------------+

            GekkoFunction rv = null;

            if (functionName.StartsWith("__"))
            {
                if (callingLibraryName == null)
                {
                    //called from prompt or command file or function/procedure defined in promt/command file.
                    using (Error txt = new Error())
                    {
                        txt.MainAdd("Calling a private '__' method (like '%y = __f1();') is only permitted from within library functions/procedures.");
                    }
                }
                else
                {
                    //called from inside library
                    if (libraryName != null && !G.Equal(libraryName, Globals.thisLibraryString))
                    {
                        using (Error txt = new Error())
                        {
                            txt.MainAdd("You are calling a private '__' method with a library name (like '%y = lib1:__f1();'). You cannot use library names with private '__' methods.");
                        }
                    }
                    //if we are calling from within lib1, we interpret the call __f2() as being equal to lib1:__f2()
                    libraryName = callingLibraryName;  
                }
            }

            if (libraryName != null)
            {
                //explicit calling, like lib1:f1() or this:f1()
                //if we are calling from within lib1, we interpret the call this:f2() as being equal to lib1:f2()
                //difference between this:f2() and __f2() is that the latter is private and can not be 
                //called by prompt or from other libraries.
                //If the function f2() really exists in lib1, there is no difference between the two, because
                //this:f2() makes sure that lib1 is searched first. The difference only shows when it is forgotten
                //that f2() does not exist in lib1, but f2() does exist in some other library (or promt-level).
                //In that case, using __f2() is more safe, and if using this:f2(), the user may forget the
                //'this'. With '__', there can be no forgetting.

                if (G.Equal(libraryName, Globals.thisLibraryString)) libraryName = callingLibraryName;                
                Library thisLib = this.GetLibrary(libraryName, true);                
                rv = thisLib.GetFunction(functionName, true);
            }
            else
            {
                //non-explicit calling, like f1()
                if (callingLibraryName == null)
                {
                    foreach (Library thisLib in this.GetLibrariesIncludingLocal())
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
                        foreach (Library thisLib2 in this.GetLibrariesIncludingLocal())
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
                        new Error("The " + G.FromLibraryToFunctionProcedureName(functionName, 4) + " does not seem to exist.");
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
                //Similar code regarding READ caching (for instance large gdx files). See #k50dfi4lkdf098.

                DateTime dt0 = DateTime.Now;
                string type = "[unknown]";  //file | cache | ram

                string fileName3 = O.ConvertToString(o.files[i]);
                string fileName2 = G.AddExtension(fileName3, Globals.zip);
                string libraryName = Path.GetFileNameWithoutExtension(fileName2);
                if (o.aliases[i] != null) libraryName = O.ConvertToString(o.aliases[i]);
                if (IsReservedName(libraryName))
                {
                    using (Error txt = new Error())
                    {
                        txt.MainAdd("The name '" + libraryName + "' is reserved regarding libraries. ");
                        txt.MoreAdd("The names 'Global' and 'Local' are reserved, because normal functions/procedures are stored in these (not 'Global' at the moment, though). ");
                        txt.MoreAdd("Similarly, 'this' and 'null' are reserved names, too, because the may be used in the future. ");
                        txt.MoreAdd("In addition, a user library cannot start with 'Gekko'. This is reserved because at some point, Gekko-developed functions/procedures ");
                        txt.MoreAdd("will be distributed together with Gekko, in the form of library zip files. These will be named for instance 'Gekko' or 'Gekko_3_1_12' or something similar and ");
                        txt.MoreAdd("should not be confused with user-developed libraries.");
                    }
                }

                if (libraryName.Length <= 1)
                {
                    using (Error txt = new Error())
                    {
                        txt.MainAdd("The library name '" + libraryName + "' cannot be just 1 character. ");
                        txt.MoreAdd("Library names must be of length > 1. The length restriction is to avoid the possibility of storing for instance a pretty.gpt schema file for plots inside a c.zip file, and later refer to the schema file with c:pretty.gpt. This would be confusing, since c: could be a drive letter.");
                    }
                }

                List<string> folders = new List<string>();
                folders.Add(Program.options.folder_command);
                folders.Add(Program.options.folder_command1);
                folders.Add(Program.options.folder_command2);
                FindFileHelper ffh = Program.FindFile(fileName2, folders, true, false, null);  //also calls CreateFullPathAndFileName()
                string fileNameWithPath = ffh.realPathAndFileName;
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

                //We know that the zip file name is not in .libraries, but it could be in ram cache.
                //If so, no need to process the zip file, just take the cache object.
                Library hit = this.CheckCache(fileNameWithPath);

                if (hit != null && hit.GetStamp().Ticks != 0l && stamp.CompareTo(hit.GetStamp()) != 0)
                {
                    //if .Tics == 0l, it is the Local library.
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
                    type = "ram";
                }
                else
                {
                    //Not found in RAM cache.
                    //Now we try the disk cache.                    
                    //We only allow a match if it is BOTH the same bytes in the file, AND the filepath + alias is the same.
                    //This makes file references easier, less to think about. So two identical libs may be parsed two times if the are in two different file locations (or a different alias is used)

                    //TODO: This may be slow: first the network file is read and UTF-converted, then MD5.
                    //      Maybe faster to do local copy of whole file first (if copylocal?...), and then find some
                    //      fast MD5 (and salt with filename etc.).

                    string s = Program.GetTextFromFileWithWait(fileNameWithPath, false);
                    string ss = s + G.NL + "Filename: " + fileNameWithPath + "Alias: " + libraryName;
                    string libHash = Program.GetMD5Hash(ss, null); //Pretty unlikely that two different libs could produce the same hash.
                    libHash = libHash.Trim();  //probably not necessary
                    string libFileNameAndPath = Globals.localTempFilesLocation + "\\" + Globals.gekkoVersion + "_" + "lib" + "_" + libHash + Globals.cacheExtension;
                    bool loadedFromProtobuf = false;
                    if (Program.options.library_cache == true)
                    {
                        if (File.Exists(libFileNameAndPath))
                        {
                            try
                            {                                
                                library = Program.ProtobufRead<Library>(libFileNameAndPath);
                                loadedFromProtobuf = true;
                            }
                            catch (Exception e)
                            {
                                if (G.IsUnitTesting())
                                {
                                    throw;
                                }
                                else
                                {
                                    //do nothing, we then have to parse the file
                                    loadedFromProtobuf = false;
                                }
                            }
                            type = "cache";
                        }
                    }
                    else
                    {
                        loadedFromProtobuf = false;
                    }

                    if (loadedFromProtobuf)
                    {
                        //do nothing, also no writing of .lib file of course
                    }
                    else
                    {
                        //We have to parse it
                        library = new Library(libraryName, fileNameWithPath, stamp);
                        library.LibraryExtractor(fileNameWithPath);
                        
                        try //not the end of world if it fails
                        {                            
                            // ----- SERIALIZE                    
                            string protobufFileName = Globals.gekkoVersion + "_" + "lib" + "_" + libHash + Globals.cacheExtension;
                            string pathAndFilename = Globals.localTempFilesLocation + "\\" + protobufFileName;                            
                            Program.ProtobufWrite(library, pathAndFilename);
                        }
                        catch (Exception e)
                        {
                            //do nothing, not the end of the world if it fails
                        }
                        type = "file";
                    }

                    //all files loaded from .zip end up here, and the cache only grows (cannot shrink).
                    this.libraryCache.Add(library);  //if a lib is closed and reopned, this can be done fast.
                }                    

                this.libraries.Add(library);
                List<string> functions, procedures, files;
                QHelper(library, out functions, out procedures, out files);

                string more = null;
                if (functions.Count + procedures.Count + files.Count > 0)
                {
                    more = " (" + G.GetLinkAction("info", new GekkoAction(EGekkoActionTypes.Unknown, null, QHelperActions(library, functions, procedures, files))) + ")";
                }

                using (Writeln txt = new Writeln())
                {
                    txt.MainAdd("Loaded library '" + libraryName + "' with " + G.AddS(functions.Count, "function") + ", " + G.AddS(procedures.Count, "procedure") + ", " + G.AddS(files.Count, "file") + more + ".");
                    txt.MainNewLineTight();
                    txt.MainAdd("Library path: " + fileNameWithPath + ", ");
                    txt.MainAdd("loaded from " + type + " in: " + G.SecondsFormat((DateTime.Now - dt0).TotalMilliseconds) + ".");
                }
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
            if (G.Equal(libraryName, Globals.localLibraryString))
            {
                this.localLibrary = new Library(Globals.localLibraryString, null, new DateTime(0l));
            }
            else
            {
                new Error("You can only clear 'Local' (the local library)");
            }
        }
    }


    /// <summary>
    /// All the functions 'f', 'g', etc. for this package
    /// </summary>
    [ProtoContract]
    public class Library
    {

        /// <summary>
        /// Name of the library, like in LIBRARY mylib;
        /// </summary>        
        [ProtoMember(1)]
        private string name = null;

        /// <summary>
        /// Filename of zip. Can be null, for the global library.
        /// </summary>
        [ProtoMember(2)]
        private string fileNameWithPath = null;

        [ProtoMember(3)]
        private GekkoDictionary<string, GekkoFunction> functions = new GekkoDictionary<string, GekkoFunction>(StringComparer.OrdinalIgnoreCase);

        [ProtoMember(4)]
        private GekkoDictionary<string, string> dataFiles = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [ProtoMember(5)]
        private GekkoDictionary<string, string> metaFiles = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Stamp: when the file was written. Do not protobuf.
        /// </summary>
        private DateTime stamp = new DateTime(0l);  //null

        /// <summary>
        /// Do not protobuf.
        /// </summary>
        GekkoFunction fastLookup = null;

        /// <summary>
        /// Only for protobuf
        /// </summary>
        public Library()
        {
        }

        /// <summary>
        /// Create a new package/library.
        /// </summary>
        /// <param name="name"></param>
        public Library(string name, string fileNameWithPath, DateTime stamp)
        {            
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
        
        /// <summary>
        /// Adds a function to the library.
        /// </summary>
        /// <param name="function"></param>
        public void AddFunction(GekkoFunction function)
        {
            function.libraryName = this.GetName();
            this.functions.Add(function.GetName(), function);            
        }

        public Dictionary<string, string> GetDataFiles()
        {
            return this.dataFiles;
        }

        public Dictionary<string, string> GetMetaFiles()
        {
            return this.metaFiles;
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
                if (abortWithError) new Error("Problem: " + G.FromLibraryToFunctionProcedureName(name, 4) + " not found in library '" + this.name + "'");
            }
            else
            {
                this.fastLookup = rv;
            }
            return rv;
        }        
        
        /// <summary>
        /// Finds all .gcm files in a folder structure, and extracts functions/procedures. Is recursive.
        /// Will also catalogue all "external" files from the \data subfolder.
        /// </summary>        
        public void LibraryExtractor(string zipFileNameWithPath)
        {
            // Process the list of files found in the directory.

            //GekkoDictionary<string, string> gcmDictionary = new GekkoDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using (ZipArchive archive = ZipFile.OpenRead(fileNameWithPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string fileNameWithRelativePath = entry.FullName.Replace("/", "\\");
                    string fileNameWithoutPath = entry.Name;
                    if (fileNameWithRelativePath.EndsWith("\\") && fileNameWithoutPath == "") continue;  //represents a folder. If not skipped, it would create an empty file ---> weird!
                    string relativePath = fileNameWithRelativePath.Replace(fileNameWithoutPath, "");
                    if (relativePath.EndsWith("\\")) relativePath = relativePath.Substring(0, relativePath.Length - 1);
                    fileNameWithRelativePath = "\\" + fileNameWithRelativePath;
                    relativePath = "\\" + relativePath;

                    if (relativePath.ToLower() == "\\" + Globals.dataLibraryString || relativePath.ToLower().StartsWith("\\" + Globals.dataLibraryString + "\\"))
                    {
                        //Normal external files. These are not extracted: just recorded.
                        if (this.dataFiles.ContainsKey(fileNameWithoutPath))
                        {
                            using (var txt = new Error())
                            {
                                string ss = this.dataFiles[fileNameWithoutPath];
                                txt.MainAdd("In the zip archive " + this.fileNameWithPath + ", in the \\data subfolder, there are duplicate versions of the file " + fileNameWithoutPath + ".");
                                txt.MainAdd("It seems the file is both present in the subfolder " + ss + " and in the subfolder " + relativePath + ".");
                            }
                        }
                        else
                        {
                            this.dataFiles.Add(fileNameWithoutPath, relativePath);
                        }
                    }
                    else if (relativePath.ToLower() == "\\" + Globals.metaLibraryString || relativePath.ToLower().StartsWith("\\" + Globals.metaLibraryString + "\\"))
                    {
                        //Normal external files with metadata. These are not extracted: just recorded.
                        if (this.metaFiles.ContainsKey(fileNameWithoutPath))
                        {
                            using (var txt = new Error())
                            {
                                string ss = this.metaFiles[fileNameWithoutPath];
                                txt.MainAdd("In the zip archive " + this.fileNameWithPath + ", in the \\meta subfolder, there are duplicate versions of the file " + fileNameWithoutPath + ".");
                                txt.MainAdd("It seems the file is both present in the subfolder " + ss + " and in the subfolder " + relativePath + ".");
                            }
                        }
                        else
                        {
                            this.metaFiles.Add(fileNameWithoutPath, relativePath);
                        }
                    }
                    else
                    {
                        if (fileNameWithRelativePath.EndsWith("." + Globals.extensionCommand, StringComparison.OrdinalIgnoreCase))
                        {                            
                            string tempFileNameWithPath = Program.WaitForExtractZipFileEntryToTempFile(entry, fileNameWithPath);
                            LibraryExtractorHandleGcmFile(tempFileNameWithPath, zipFileNameWithPath, fileNameWithRelativePath);
                        }
                    }
                }
            }            
        }

        /// <summary>
        /// Extract function/procedure code as text. The last two strings are only for putting text info on location etc. if there are errors
        /// </summary>
        /// <param name="extractedFile"></param>
        private void LibraryExtractorHandleGcmFile(string extractedFile, string zipFileName, string fileNameWithRelativePath)
        {
            // ...
            // ...
            // function ...           --> or procedure
            // ...
            // end;
            // ...
            // function ...
            // ...
            // end;
            // ...

            // will be cut like this:

            //=========================================
            // ...
            // ...
            // function ...                                functionNames[0], functionCounter = 1
            // ...
            // end;
            // ...
            //=========================================
            // function ...                                functionNames[1], functionCounter = 2
            // ...
            // end;
            // ...
            //=========================================

            //It basically cuts before second "function" and so on.

            string pathToFileInsideZip = zipFileName + fileNameWithRelativePath;

            string s = Program.GetTextFromFileWithWait(extractedFile);
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
                        int i1 = i;
                        TokenHelper th = null;
                        if (th1 != null) th = th1.SiblingBefore(1, true);
                        if (th != null && G.Equal(th.s, "end"))
                        {
                            //searching for the "end" just before the ";" of the previous
                            //command (ignoring comments)
                            i1 = th1.id + 1;  //the node after the ";"
                        }

                        functionCounter++; 
                        
                        if (isFunction)
                        {
                            TokenHelper temp2 = t2.subnodes[i].SiblingAfter(2, true);  //skips comments, newlines, etc.
                            string name = temp2.s.ToLower();
                            functionNamesLower.Add(name);
                            if (Globals.gekkoInbuiltFunctions.ContainsKey(name))
                            {
                                using (Warning text = new Warning())
                                {
                                    O.SameFunctionOrProcedureNameWarning(text, name);
                                }
                            }
                        }
                        else
                        {
                            TokenHelper temp2 = t2.subnodes[i].SiblingAfter(1, true);  //skips comments, newlines, etc.
                            string name = temp2.s.ToLower();
                            functionNamesLower.Add(Globals.procedure + name);
                            foreach (string s5 in Globals.commandNames)
                            {
                                if (G.Equal(s5, name))
                                {
                                    //But you cannot even define a procedure with a Gekko command name...
                                    new Warning("Beware that user " + G.FromLibraryToFunctionProcedureName(name, 4) + " is also the name of a Gekko command. The Gekko command will take precedence.");
                                }
                            }
                        }

                        if (functionCounter >= 2)
                        {
                            this.LibraryExtractorGetFunctionCode(i0, i1, functionNamesLower[functionCounter - 2], t2.subnodes, pathToFileInsideZip);
                            i0 = i1;
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

            GekkoFunctionCode gfc = new GekkoFunctionCode();
            gfc.code = sb.ToString();
            gfc.fileNameWithPath = file;
            gfc.line= t[i0].line;
            function.overloads.Add(gfc);  //See also this: #08975389245253
        }
    }

    [ProtoContract]
    public class GekkoFunctionCode
    {
        [ProtoMember(1)]
        public string fileNameWithPath = null;

        [ProtoMember(2)]
        public int line = -12345;

        [ProtoMember(3)]
        public string code = null;

        /// <summary>
        /// Only for protobuf.
        /// </summary>
        public GekkoFunctionCode()
        {
        }
    }

    [ProtoContract]
    public class GekkoFunction
    {
        [ProtoMember(1)]
        private string name = null; //for instance 'f'

        [ProtoMember(2)]
        public string libraryName = null;  //name of the lib where the function is stored

        [ProtoMember(3)]
        public List<GekkoFunctionCode> overloads = new List<GekkoFunctionCode>();  //may contain code from several places, snippets of f(), f(...), f(..., ...)

        //Do not protobuf
        public bool hasBeenCompiled = false;

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
        /// Only for protobuf.
        /// </summary>
        public GekkoFunction()
        {
        }

        public GekkoFunction(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Name of function, irrespective of overloads.
        /// </summary>
        public string GetName()
        {
            return this.name;
        }
    }
}