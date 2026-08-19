using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Gekko
{
    /// <summary>
    /// Tries to assemble all methods that convert to/from a list of strings here,
    /// so that they are easier to locate. Also includes converting from list of strings
    /// to/from list of IVariables.
    /// </summary>
    public static class Stringlist
    {
        /// <summary>
        /// Transform a string into a list of strings
        /// </summary>
        /// <param name="textInput"></param>
        /// <returns></returns>
        public static List<string> ExtractLinesFromText(string textInput)
        {
            StringReader inputFileStringReader = new StringReader(textInput);
            List<string> output = new List<string>();
            while (true)
            {
                string aLine = inputFileStringReader.ReadLine();
                if (aLine != null)
                {
                    output.Add(aLine);
                }
                else
                {
                    break;
                }
            }
            return output;
        }

        /// <summary>
        /// Transforms a list of strings into a string
        /// </summary>
        /// <param name="linesInput"></param>
        /// <returns></returns>
        public static StringBuilder ExtractTextFromLines(List<string> linesInput)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in linesInput)
            {
                sb.AppendLine(line);
            }
            return sb;
        }

        /// <summary>
        /// A list like ("a", "middle element", "c") is turned into "[a, 'middle element', c]".
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetIndexWithCommas(string[] list)
        {
            string txt = null;
            foreach (string ss in list)
            {
                if (G.IsIdent(ss)) txt += ss + ",";
                else txt += "'" + ss + "'" + ",";
            }
            if (txt.Length > 0) txt = "[" + txt.Substring(0, txt.Length - 1) + "]";
            return txt;
        }


        /// <summary>
        /// Transform a list of DNames to a comma-separated string.
        /// Choose blanks between for instance elements, "a, b, c" (listBlanks = " ") or "a,b,c" (listBlanks = null or "").
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetListWithCommas(List<DName> list, string listBlanks)
        {
            if (list == null) return null;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string s = list[i].ToString();
                sb.Append(s);
                if (i < list.Count - 1)
                {
                    sb.Append("," + listBlanks);
                }
            }
            return sb.ToString();
        }


        public static string GetListWithCommas(List<DName> list)
        {
            return GetListWithCommas(list, " ");
        }

        /// <summary>
        /// Chops up a part as a list of strings.         
        /// For "xx\root.ini" or "\xx\root.ini\", it will return ["xx", "root.ini"].
        /// For "c:\xx\root.ini", it will return ["c:", "xx", "root.ini"].
        /// For "\\localhost\b$\xx\root.ini", it will return ["\\localhost\b$", "xx", "root.ini"].
        /// Gemini says there are some bugs in this that ought to be fixed... (we keep it for now, has been battletested).
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> Path_FromStringToList(string path)
        {
            // Check if the path is rooted (starts with C:\, \\server, etc.)
            if (G.NullOrBlanks(path)) return new List<string>();
            bool isRooted = Path.IsPathRooted(path);
            // Get the root part (e.g., "c:\", "\\server\share\")
            string root = Path.GetPathRoot(path);
            // List to hold the final parts
            List<string> finalParts = new List<string>();
            if (isRooted && !string.IsNullOrEmpty(root))
            {
                // 1. Handle Rooted Paths (e.g., "c:\a\b" or "\\server\share\a\b")
                // Add the clean root component (e.g., "c:" or "\\server\share")
                // Use TrimEnd to ensure the separator is removed from the root
                string rootComponent = root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                // Handle UNC paths which look like "\\server\share"
                if (root.StartsWith(@"\\"))
                {
                    // For UNC, the root is usually the server and share
                    // We use the original root because UNC stripping is complex
                    rootComponent = root.TrimEnd(Path.DirectorySeparatorChar);
                }
                else
                {
                    // For drive letters, just take the first part
                    rootComponent = root.Split(Path.DirectorySeparatorChar)[0];
                }
                finalParts.Add(rootComponent);
                // Remove the root part from the path string
                path = path.Substring(root.Length);
            }
            // 2. Split the remaining path (which may be the whole original path if relative)
            // The replace handles mixed separators like 'a/b\c'
            string normalizedPath = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            string[] directoryParts = normalizedPath.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            // 3. Add the rest of the segments
            finalParts.AddRange(directoryParts);
            return finalParts;
        }        

        public static string Path_FromListToString(List<string> m, string separator)
        {
            List<string> m2 = null;
            if (separator == "\\")
            {
                //Mostly for unc part, first element
                m2 = m.Select(s => s?.Replace('/', '\\')).ToList();
            }
            else if (separator == "/")
            {
                //Mostly for unc part, first element
                m2 = m.Select(s => s?.Replace('\\', '/')).ToList();
            }
            else if (separator == "<separator>")
            {
                //Only For testing purposes
                m2 = m;
            }
            else new Error("Expected separator '\\' or '/' or '<separator>'");
            return string.Join(separator, m2.ToArray());
        }

        public static bool Path_IsOPathRooted(List<string> m)
        {
            bool isRooted = Path.IsPathRooted(Path_FromListToString(m, "\\"));
            return isRooted;
        }

        public static List<string> Path_RemoveStart(List<string> source, List<string> prefix)
        {
            if (source == null || prefix == null) new Error("Null list");
            // Check if source has enough items and matches prefix case-insensitively
            bool startsWithPrefix = source.Count >= prefix.Count && source.Take(prefix.Count).SequenceEqual(prefix, StringComparer.OrdinalIgnoreCase);
            if (Globals.tthDebug) File.WriteAllText("c:\\b-tth\\gitbug111.txt", Path_FromListToString(source, "<separator>") + " === " + Path_FromListToString(prefix, "<separator>"));
            if (!startsWithPrefix)
            {
                if (Globals.tthDebug) File.WriteAllText("c:\\b-tth\\gitbug.txt2222", "The path '" + Path_FromListToString(source, "<separator>") + " does not start with " + Path_FromListToString(prefix, "<separator>"));
                new Error("The path '" + Path_FromListToString(source, "<separator>") + " does not start with " + Path_FromListToString(prefix, "<separator>"));
            }
            return source.Skip(prefix.Count).ToList();
        }

        public static List<string> Path_RemoveString(List<string> m, string target, int maxToRemove)
        {
            if (m == null || target == null || maxToRemove <= 0) return m;
            m = new List<string>(m); //clone
            int removedCount = 0;
            for (int i = 0; i < m.Count; i++)
            {
                if (string.Equals(m[i], target, StringComparison.OrdinalIgnoreCase))
                {
                    m.RemoveAt(i);
                    i--; // Step back to evaluate the new element at this index
                    removedCount++;
                    if (removedCount >= maxToRemove) break;
                }
            }
            return m;
        }

        public static List<string> Path_ReplaceString(List<string> m, string target, string replacement, int maxToReplace)
        {
            if (m == null || target == null || maxToReplace <= 0) return m;
            m = new List<string>(m); //clone
            int replacedCount = 0;
            for (int i = 0; i < m.Count; i++)
            {
                if (string.Equals(m[i], target, StringComparison.OrdinalIgnoreCase))
                {
                    m[i] = replacement;
                    replacedCount++;
                    if (replacedCount >= maxToReplace) break;
                }
            }
            return m;
        }

        /// <summary>
        /// Transform a list of strings to a comma-separated string.
        /// Choose blanks between for instance elements, "a, b, c" (listBlanks = " ") or "a,b,c" (listBlanks = null or "").
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetListWithCommas(List<string> list, string listBlanks)
        {
            if (list == null) return null;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string s = list[i];
                sb.Append(s);
                if (i < list.Count - 1)
                {                    
                    sb.Append("," + listBlanks);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Overload, with blank and comma between elements ("a, b, c" not "a,b,c")
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetListWithCommas(List<string> list)
        {
            return GetListWithCommas(list, " ");
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetListWithCommas(string[] list, string listBlanks)
        {
            if (list == null) return null;
            return GetListWithCommas(new List<string>(list), listBlanks);
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetListWithCommas(string[] list)
        {
            if (list == null) return null;
            return GetListWithCommas(new List<string>(list));
        }

        /// <summary>
        /// Removes empty lines in a list of strings
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<string> RemoveEmptyLines(List<string> s)
        {
            List<string> xx = new List<string>();
            foreach (string s2 in s)
            {
                if (s2.Trim() == "") continue;
                xx.Add(s2);
            }
            return xx;
        }



        /// <summary>
        /// Reads a file of string lines into a list of strings
        /// </summary>
        /// <param name="inputFile"></param>
        /// <returns></returns>
        public static List<string> CreateListOfStringsFromFile(string inputFile)
        {
            List<string> inputFileLines = new List<string>();
            StringReader inputFileStringReader = new StringReader(inputFile);
            while (true)
            {
                string aLine = inputFileStringReader.ReadLine();
                if (aLine != null)
                {
                    inputFileLines.Add(aLine);
                }
                else
                {
                    break;
                }
            }
            return inputFileLines;
        }

        /// <summary>
        /// Tries to convert a list of IVariables into a C# list of strings. The elements of the list are
        /// typically strings or values (integers up to 64-bit integer). But an element may also be a 1-element sub-list,
        /// where this one element is of string or integer type. The integers may contain leading zeroes (these zeroes are stored in the ScalarVal variables).
        /// More complicated nested lists are probably not supported.
        /// How does this relate to GetListOfStringsFromList()?
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static string[] GetListOfStringsFromListOfIvariables(IVariable[] elements)
        {
            string[] keys = new string[elements.Length];
            int stringCount = 0;
            int i = -1;
            foreach (IVariable iv in elements)
            {
                i++;
                if (iv.Type() == EVariableType.String)
                {
                    //note: see same kind of code just below, //#98073245243875
                    stringCount++;
                    ScalarString ss = iv as ScalarString;
                    keys[i] = ss.string2;
                }
                else if (iv.Type() == EVariableType.Val)  //will handle 007 in x[a, 007], will become x['a', '007']
                {
                    //note: see same kind of code just below, //#98073245243875
                    long ii = O.ConvertToLong(iv, false);
                    if (ii != long.MaxValue)
                    {
                        stringCount++;
                        byte b = (iv as ScalarVal).numberOfLeadingZeroes;
                        HandleLeadingZeroes(keys, i, b, ii);
                    }
                }
                else if (iv.Type() == EVariableType.List)
                {
                    List iv_list = iv as List;
                    if (iv_list.Count() == 1)
                    {
                        //Singleton list is allowed as a scalar
                        IVariable singleton = iv_list.list[0];
                        if (singleton.Type() == EVariableType.String)
                        {
                            //note: see same kind of code just above, //#98073245243875
                            stringCount++;
                            ScalarString ss = singleton as ScalarString;
                            keys[i] = ss.string2;
                        }
                        else if (singleton.Type() == EVariableType.Val)  //will not handle 007 in x[a, 007], must be x[a, '007']
                        {
                            //note: see same kind of code just above, //#98073245243875
                            long ii = O.ConvertToLong(singleton, false);
                            if (ii != long.MaxValue)
                            {
                                stringCount++;
                                keys[i] = ii.ToString();
                                byte b = (singleton as ScalarVal).numberOfLeadingZeroes;
                                HandleLeadingZeroes(keys, i, b, ii);
                            }
                        }
                    }
                }
            }
            if (elements.Length != stringCount)
            {
                keys = null;  //signals a problem
            }

            return keys;
        }

        /// <summary>
        /// Tries to convert IVariable to a C# list of strings. The IVariable is expected to be a Gekko list,
        /// containing strings. However, you may also input a single Gekko string to the method.
        /// How does this relate to GetListOfStringsFromListOfIvariables()?
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static List<string> GetListOfStringsFromList(IVariable a)
        {
            if (a.Type() == EVariableType.String)
            {
                List<string> mm = new List<string>();
                mm.Add(a.ConvertToString());
                return mm;
            }
            else if (a.Type() == EVariableType.List)
            {
                List<IVariable> m = a.ConvertToList();
                List<string> mm = new List<string>();
                foreach (IVariable iv in m)
                {
                    string s = O.ConvertToString(iv);
                    mm.Add(s);
                }
                return mm;
            }
            else
            {
                new Error("Input must be a string or list of strings"); return null;
            }
        }

        /// <summary>
        /// Converts a C# list of strings to a list of IVariables (Gekko strings). Simple method.
        /// </summary>
        /// <param name="indexes"></param>
        /// <returns></returns>
        public static IVariable[] GetListOfIVariablesFromListOfStrings(string[] indexes)
        {
            IVariable[] keys = new IVariable[indexes.Length];
            int stringCount = 0;
            int i = -1;
            foreach (string s in indexes)
            {
                i++;
                keys[i] = new ScalarString(indexes[i]);
            }
            return keys;            
        }

        /// <summary>
        /// Creates a Gekko list of strings from a C# list of strings
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List CreateListFromStrings(string[] input)
        {
            List m = new List(new List<string>(input));
            return m;
        }

        /// <summary>
        /// Why is this method so special, does it duplicate some other method?
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static List<string> GetListOfStringsFromIVariable(IVariable x)
        {
            if (x.Type() == EVariableType.String)
            {
                return new List<string>() { x.ConvertToString() };
            }
            else if (x.Type() == EVariableType.List)
            {
                return Stringlist.GetListOfStringsFromList(x);
            }
            else
            {
                G.Writeln2("*** Expected string of list of strings");
                throw new GekkoException();
            }
        }


        /// <summary>
        /// Helper method.
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="i"></param>
        /// <param name="b"></param>
        /// <param name="ii"></param>
        private static void HandleLeadingZeroes(string[] keys, int i, byte b, int ii)
        {
            string z = null;
            if (b > 0)
            {
                z = new string('0', b);
            }
            if (ii < 0)
            {
                //This should never happen: Gekko will not parse x[-0007] as a something that can
                //be parsed into a string --> it will always be interpreted as a lag, because the
                //first char after '[' is a '-'. So x[-...] or x[+...] are always lags or leads.
                //This is to avoid x[5] being interpreted as x leaded 5 periods, rather than, say,
                //five-year olds in the population.
                keys[i] = "-" + z + (-ii).ToString(); //b = 3, ii = -7 --> "-0007"
            }
            else
            {
                keys[i] = z + ii.ToString(); //b = 3, ii = 7 --> "0007"
            }
        }

        /// <summary>
        /// Overload for 64-bit ii. See same method for int 32-bit ii.
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="i"></param>
        /// <param name="b"></param>
        /// <param name="ii"></param>
        private static void HandleLeadingZeroes(string[] keys, int i, byte b, long ii)
        {
            string z = null;
            if (b > 0)
            {
                z = new string('0', b);
            }
            if (ii < 0)
            {                
                keys[i] = "-" + z + (-ii).ToString(); //b = 3, ii = -7 --> "-0007"
            }
            else
            {
                keys[i] = z + ii.ToString(); //b = 3, ii = 7 --> "0007"
            }
        }
    }


}
