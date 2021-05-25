using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        /// Transform a list of strings to a comma-separated string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetListWithCommas(List<string> list)
        {
            if (list == null) return null;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                string s = list[i];
                sb.Append(s);
                if (i < list.Count - 1) sb.Append(", ");
            }
            return sb.ToString();
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
        /// typically strings or values (integers). But an element may also be a 1-element sub-list,
        /// where this one element is of string or integer type. The integers may contain leading zeroes.
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
                    int ii = O.ConvertToInt(iv, false);
                    if (ii != int.MaxValue)
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
                            int ii = O.ConvertToInt(singleton, false);
                            if (ii != int.MaxValue)
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



    }
}
