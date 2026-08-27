/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2025, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using Microsoft.Win32;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Security.Policy;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Gekko
{

    /// <summary>
    /// Class containing a library of functions used in many places
    /// </summary>
    public class G {

        public enum ESigilType
        {
            Scalar,
            Collection,
            Frequency,  //can be series
            None  //can be series            
        }

        public const string NL = "\r\n";  //official Windows, cf. https://stackoverflow.com/questions/3986093/in-c-whats-the-difference-between-n-and-r-n
        public const char NL2 = '\n';     //best for counting number of newlines, since Windows accepts both \r\n and \n as newline. Mac uses \r, hmm, never mind.
        public const string NL_ToolTip = "&#x0a;";  //This can be used directly in XAML if neede, just put here for remembrance.

        // ------------------------------------------------------------------------------------------------
        // Compare strings start
        // ------------------------------------------------------------------------------------------------

        /// <summary>
        /// Compares two strings, ignoring case (so "aBc" == "Abc"). If one but not the other
        /// is null, it returns false. If both are null, it returns true.
        /// </summary>
        /// <param name="s1">First string</param>
        /// <param name="s2">Second string</param>
        /// <returns>True if equal</returns>
        public static bool Equal(string s1, string s2)
        {
            //s1 or s2 may be null
            return (string.Compare(s1, s2, true) == 0);  //true for ignoreCase
            //For Gekko 3.2 maybe use this, probably faster:
            //return (string.Compare(s1, s2, StringComparison.OrdinalIgnoreCase) == 0);
        }

        /// <summary>
        /// True if at least one element is EqualHandleBlanks(). Practical to see if "x[a, b]" is 
        /// contained in the list ("x[a,b]", ...).
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="names2"></param>
        /// <returns></returns>
        public static bool Equal(DName name1, List<DName> names2)
        {
            string rv = null;
            foreach (DName s2 in names2)
            {
                if (G.Equal(name1, s2))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Case-insensitive pairwise compare of two lists of strings. Returns false if either list is null, or if different number of elements.
        /// </summary>
        /// <param name="names1"></param>
        /// <param name="names2"></param>
        /// <returns></returns>
        public static bool Equal(List<DName> names1, List<DName> names2)
        {
            if (names1 == null || names2 == null) return false;
            if (names1.Count != names2.Count) return false;
            for (int i = 0; i < names1.Count; i++)
            {
                if (!G.Equal(names1[i], names2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Equal(DName name1, DName name2)
        {
            Multidim2Comparer comparer = Multidim2Comparer.IgnoreCase;
            return comparer.Equals(name1, name2);
        }

        /// <summary>
        /// /// Compares two strings, ignoring case (so "aBc" == "Abc"). If one but not the other
        /// is null, it returns false. If both are null, it returns true. Blanks are ignored,
        /// so for instance "x[a, b]" will match "x[a,b]" ... and "X[A,B]". But blanks in quotes are not ignored.
        /// </summary>
        public static bool EqualHandleBlanks(string str1, string str2)
        {
            if (str1 == null || str2 == null)
            {
                return str1 == null && str2 == null;
            }

            int len1 = str1.Length;
            int len2 = str2.Length;
            int i = 0;
            int j = 0;
            bool inSingleQuotes1 = false;
            bool inSingleQuotes2 = false;

            while (i < len1 && j < len2)
            {
                char char1 = str1[i];
                char char2 = str2[j];

                // Handle single quotes for str1
                if (char1 == '\'')
                {
                    inSingleQuotes1 = !inSingleQuotes1;
                    i++;
                    continue;
                }

                // Handle single quotes for str2
                if (char2 == '\'')
                {
                    inSingleQuotes2 = !inSingleQuotes2;
                    j++;
                    continue;
                }

                // Skip blanks if not inside single quotes for str1
                if (!inSingleQuotes1 && char1 == ' ')
                {
                    i++;
                    continue;
                }

                // Skip blanks if not inside single quotes for str2
                if (!inSingleQuotes2 && char2 == ' ')
                {
                    j++;
                    continue;
                }

                // Compare characters (non-case sensitive)
                if (char.ToUpperInvariant(char1) != char.ToUpperInvariant(char2))
                {
                    return false;
                }

                i++;
                j++;
            }

            // Handle trailing blanks (outside quotes)
            while (i < len1)
            {
                if (str1[i] != ' ')
                {
                    return false;
                }
                i++;
            }

            while (j < len2)
            {
                if (str2[i] != ' ')
                {
                    return false;
                }
                j++;
            }

            return true;
        }

        /// <summary>
        /// True if at least one element is EqualHandleBlanks(). Practical to see if "x[a, b]" is 
        /// contained in the list ("x[a,b]", ...).
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s3"></param>
        /// <returns></returns>
        public static bool EqualHandleBlanks(string s1, List<string> s3)
        {
            string rv = null;
            foreach (string s2 in s3)
            {
                if (G.EqualHandleBlanks(s1, s2))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Could just as well return a boolean.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s3"></param>
        /// <returns></returns>
        public static string Equal(string s1, List<string> s3)
        {
            string rv = null;
            foreach (string s2 in s3)
            {
                //For Gekko 3.2 maybe use this, probably faster:
                //if (string.Compare(s1, s2, StringComparison.OrdinalIgnoreCase) == 0);
                if (string.Compare(s1, s2, true) == 0)  //true for ignoreCase                
                {
                    rv = s2;
                    break;
                }
            }
            return rv;
        }

        /// <summary>
        /// Case-insensitive pairwise compare of two lists of strings. Returns false if either list is null, or if different number of elements.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool Equal(List<string> m1, List<string> m2)
        {
            if (m1 == null || m2 == null) return false;
            if (m1.Count != m2.Count) return false;
            bool good = true;
            for (int i = 0; i < m1.Count; i++)
            {
                if (!G.Equal(m1[i], m2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets files only in the specified directory (no subfolders).
        /// </summary>
        public static List<string> GetFiles(string path, string searchPattern)
        {
            try
            {
                // Directory.GetFiles defaults to TopDirectoryOnly
                if (searchPattern == null) return Directory.GetFiles(path).ToList();
                else return Directory.GetFiles(path, searchPattern).ToList();
            }
            catch (Exception ex)
            {
                new Error($"Error accessing path: {ex.Message}");
                return null; //will never happen
            }
        }

        /// <summary>
        /// Gets immediate subdirectories only (no nested folders).
        /// </summary>
        public static List<string> GetFolders(string path, string searchPattern)
        {
            try
            {
                if (searchPattern == null) return Directory.GetDirectories(path).ToList();
                return Directory.GetDirectories(path, searchPattern).ToList();
            }
            catch (Exception ex)
            {
                new Error($"Error accessing path: {ex.Message}");
                return null; //will never happen
            }
        }

        /// <summary>
        /// Case-insensitive pairwise compare of two lists of strings. Returns false if either list is null, or if different number of elements. Blanks are removed before compare.
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool EqualHandleBlanks(List<string> m1, List<string> m2)
        {
            if (m1 == null || m2 == null) return false;
            if (m1.Count != m2.Count) return false;
            bool good = true;
            for (int i = 0; i < m1.Count; i++)
            {
                if (!G.EqualHandleBlanks(m1[i], m2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Fastest version of StartsWith(), use if case-insensitive is not required (the method is like 5x faster than G.StartsWith()).
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool StartsWithCaseSensitiveFast(string s1, string s2)
        {
            //This is like 10x faster than s1.StartsWith(s2), which looks for current culture first (which is slow). Here we are just comparing bytes, not worrying if "ae" is same as "æ".
            return s1.StartsWith(s2, StringComparison.Ordinal);
        }

        /// <summary>
        /// Ignores case. Like 5x slower than G.StartsWithCaseSensitiveFast(), but still ok fast, not looking for current culture.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool StartsWith(string s1, string s2)
        {
            if (s1 == null) return false;
            return s1.StartsWith(s2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Search for string inside string (case-insensitive). For instance Contains('Peartree', 'TREE') == true.
        /// </summary>
        /// <param name="s1">String to search (e.g. 'peartree')</param>
        /// <param name="s2">Sub-string to search for (e.g. 'tree')</param>
        /// <returns>True if match</returns>
        public static bool Contains(string s1, string s2)
        {
            if (s1 == null) return false;
            return s1.IndexOf(s2, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        // ------------------------------------------------------------------------------------------------
        // Compare strings end
        // ------------------------------------------------------------------------------------------------

        /// <summary>
        /// Helper method to keep stuff together. For Gekko 4.0 we should only have 1 of these calls from Decomp.cs, not
        /// a lot from different places, and the levels (Quo / QuoRef) should be stored in the decomp objects at the
        /// beginning.
        /// </summary>
        /// <param name="missingAsZero"></param>
        /// <returns></returns>
        public static ESeriesMissing DecompShouldHandleMissings(bool missingAsZero, bool wholeSeriesExistence)
        {
            if (missingAsZero) return ESeriesMissing.Zero;
            if (wholeSeriesExistence)
            {
                return Program.options.decomp_array_calc_missing;
            }
            else
            {
                return Program.options.decomp_data_missing;
            }
        }

        /// <summary>
        /// In a double[] array, replaces missing values with 0
        /// </summary>
        /// <param name="temp"></param>
        public static void ReplaceNaNWith0(double[] temp)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                if (G.IsNumericalError(temp[i])) temp[i] = 0d;
            }
        }

        /// <summary>
        ///  In a double[,] array, replaces missing values with 0
        /// </summary>
        /// <param name="temp"></param>

        public static void ReplaceNaNWith0(double[,] temp)
        {
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    if (G.IsNumericalError(temp[i, j])) temp[i, j] = 0d;
                }
            }
        }

        /// <summary>
        /// Helper method
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceTurtle(string s)
        {
            return s.Replace("¤[0]", "").Replace("¤", "");
        }

        /// <summary>
        /// Fast parse of a simple string into an integer. Strings like '123', '007', no minus, delimiters. ...
        /// NOTE: only deals with ints like 123, 007, 5. No minus, delimiters etc.!
        /// Returns -12345 if s is not such an integer
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int IntParse(string s)
        {
            if (s == null) return -12345;
            int y = 0;
            for (int i = 0; i < s.Length; i++)
            {
                int x = s[i] - '0';
                if (x < 0 || x > 9)
                {
                    return -12345;
                }
                y = y * 10 + x;
            }
            return y;
        }

        /// <summary>
        /// Parse a string into a double
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool TryParseIntoDouble(string s, out double d)
        {
            //NumberStyles.AllowDecimalPoint|NumberStyles.AllowExponent|NumberStyles.AllowLeadingSign
            return double.TryParse(s, NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out d);
        }

        /// <summary>
        /// Parse a string into a double
        /// </summary>
        /// <param name="s"></param>
        /// <param name="reportError"></param>
        /// <returns></returns>
        public static double ParseIntoDouble(string s, bool reportError, out bool ok)
        {
            ok = false;
            double d = double.NaN;
            bool ok2 = G.TryParseIntoDouble(s, out d);
            if (ok2)
            {
                ok = true;
                return d;
            }
            if (reportError)
            {
                new Error("Cannot convert '" + s + "' into a value");
                ok = false;
                return double.NaN;
            }
            else
            {
                ok = false;
                return double.NaN;
            }
        }

        /// <summary>
        /// For instance, 5 --> 0..9, 12 --> 10..19
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GroupBy10(int i)
        {
            string s2;
            int i1 = i / 10;
            s2 = (i1 * 10) + Globals.ageHierarchyDivider + ((i1 + 1) * 10 - 1);
            return s2;
        }


        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double ParseIntoDouble(string s, out bool ok)
        {
            return G.ParseIntoDouble(s, false, out ok);
        }

        /// <summary>
        /// Sets all elements of an array to NaN
        /// </summary>
        /// <param name="x"></param>
        public static void SetNaN(double[] x)
        {
            for (int i = 0; i < x.Length; i++) x[i] = double.NaN;
        }

        /// <summary>
        /// Creates a double[n] array with all elements set to NaN
        /// </summary>
        /// <param name="x"></param>
        public static double[] CreateNaN(int n)
        {
            double[] x = new double[n];
            G.SetNaN(x);
            return x;
        }

        /// <summary>
        /// Converts bool true/false into string
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string TrueFalse(bool x)
        {
            string s = "false";
            if (x) s = "true";
            return s;
        }

        /// <summary>
        /// Splits a string by commas, but ignores commas found inside single quotes.
        /// </summary>
        /// <param name="input">The string to split (e.g., "a,bb,'x,y',c").</param>
        /// <returns>A List of strings (e.g., "a", "bb", "x,y", "c").</returns>
        public static List<string> SplitIgnoringQuotedCommas(string input, bool sqlStyle)
        {
            List<string> result = null;
            if (sqlStyle)
            {
                result = new List<string>();
                if (string.IsNullOrWhiteSpace(input)) return result;

                // Strip the outer brackets x[...] if present
                int start = input.IndexOf('[') + 1;
                int end = input.LastIndexOf(']');
                if (start > 0 && end > start)
                {
                    input = input.Substring(start, end - start);
                }

                StringBuilder currentElement = new StringBuilder();
                bool inQuotes = false;
                char? quoteChar = null;

                for (int i = 0; i < input.Length; i++)
                {
                    char c = input[i];

                    if (!inQuotes)
                    {
                        if (c == '\'' || c == '\"')
                        {
                            inQuotes = true;
                            quoteChar = c;
                        }
                        else if (c == ',')
                        {
                            result.Add(currentElement.ToString().Trim());
                            currentElement.Clear();
                        }
                        else
                        {
                            currentElement.Append(c);
                        }
                    }
                    else // We are inside quotes
                    {
                        // Check for escaped quotes ('' or "")
                        if (c == quoteChar && i + 1 < input.Length && input[i + 1] == quoteChar)
                        {
                            currentElement.Append(c);
                            i++; // Skip the second quote
                        }
                        // Check for the closing quote
                        else if (c == quoteChar)
                        {
                            inQuotes = false;
                            quoteChar = null;
                        }
                        else
                        {
                            currentElement.Append(c);
                        }
                    }
                }

                // Add the final element
                result.Add(currentElement.ToString().Trim());
            }
            else
            {
                bool fixQuotesProblem = true;
                result = new List<string>();
                // StringBuilder to build the current token/field
                var currentToken = new StringBuilder();
                // Flag to track if we are inside a single-quoted section
                bool inQuotes = false;

                foreach (char c in input)
                {
                    if (c == '\'')
                    {
                        // Toggle the inQuotes state when a single quote is encountered
                        inQuotes = !inQuotes;
                        currentToken.Append(c);
                        continue;
                    }

                    if (c == ',' && !inQuotes)
                    {
                        // If we hit a comma *outside* of quotes, the current token is complete.
                        // 1. Add the trimmed token to the result list.
                        result.Add(currentToken.ToString().Trim());
                        // 2. Clear the StringBuilder for the next token.
                        currentToken.Clear();
                    }
                    else
                    {
                        // Otherwise (if it's not a quote, or it's a comma *inside* quotes, or any other character),
                        // just append the character to the current token.
                        currentToken.Append(c);
                    }
                }

                // After the loop, the last token needs to be added to the list.
                if (currentToken.Length > 0 || result.Count == 0)
                {
                    result.Add(currentToken.ToString().Trim());
                }
            }

            return result;
        }

        /// <summary>
        /// Add 's' to plural word. For instance "0 files", "1 file", "2 files", ... . 
        /// If addIsAre is active, it will return "are 0 files", "is 1 file", "are 2 files", ...
        /// Some words are known, like library --> libraries.
        /// for instance "
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string AddS(int i, string s, bool addIsAre)
        {
            if (i == 1)
            {
                if (addIsAre) return "is " + i + " " + s;      //"is 1 file"
                else return i + " " + s;                       //"1 file"
            }
            else
            {
                string s2 = s + "s";
                if (s == "library") s2 = "libraries";
                if (addIsAre) return "are " + i + " " + s2;    //"are 0 files" or "are 2 files" or ...
                else return i + " " + s2;                      //"0 files" or "2 files" or ...
            }
        }

        /// <summary>
        /// Add 's' to plural word. For instance "0 files", "1 file", "2 files", ... .         
        /// Some words are known, like library --> libraries.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string AddS(int i, string s)
        {
            return AddS(i, s, false);
        }

        /// <summary>
        /// Adds a "0" to e.g. -.4327 so it turns into -0.4327
        /// </summary>
        /// <param name="val">Input string</param>
        /// <returns>Output string</returns>
        public static string Add0Ifmissing(string val)
        {
            string val1 = "";
            if (val.Length >= 1 && val[0] == '.')
            {
                //.123 --> 0.123
                val1 = "0" + val;
            }
            else if (val.Length >= 2 && val[0] == '-' && val[1] == '.')
            {
                //-.123 --> -0.123
                val1 = val.Remove(0, 1);
                val1 = "-0" + val1;
            }
            else
            {
                val1 = val;
            }
            return val1;
        }

        //
        /// <summary>
        /// If input is "b[1234]", 1234 is returned
        /// </summary>
        /// <param name="xxx">Input</param>
        /// <returns>Number</returns>
        public static int parseBrackets(String xxx)
        {

            String yyy = xxx.Substring(2, xxx.Length - 3);
            int zzz = int.Parse(yyy);
            return zzz;
        }

        /// <summary>
        /// Extracts "fY" and "-2" from "fY¤-2". Not so much used in Gekko 3.0 anymore
        /// </summary>
        /// <param name="key">Input</param>
        /// <param name="variable">Variable name</param>
        /// <param name="lag">Lag</param>
        public static void ExtractVariableAndLag(string key, out string variable, out int lag)
        {
            //NOTE: some vars are of this type: @fy¤¤2001q3    = absolute time (in base)
            int indx = key.IndexOf(Globals.lagIndicator);
            variable = key.Substring(0, indx - 0);
            string lag1 = key.Substring(indx + 1, key.Length - (indx + 1));
            lag = int.Parse(lag1);  //TODO: error handling
        }

        //will include lag indicator (¤)
        //WHY not just always use FromBNumberToVarname2()??? Because of ENDO/EXO stuff??
        /// <summary>
        /// Helper function for models. Move to model part of code.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string FromBNumberToVarname(int i)
        {
            string culprit;
            EquationHelper eh = (EquationHelper)Program.model.modelGekko.equations[Program.model.modelGekko.m2.fromBNumberToEqNumber[i]];
            culprit = eh.lhsWithLagIndicator;
            return culprit;
        }

        // Converts "fY¤-2" into "fY(-2)"
        /// <summary>
        /// Helper not used much in Gekko 3.0
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public static string FormatVariableAndLag(string varName)
        {
            string variable = null;
            int lag = 0;
            G.ExtractVariableAndLag(varName, out variable, out lag);
            variable = G.PrettifyTimeseriesHash(variable, true, false);
            if (lag != 0) variable += "[" + lag + "]";
            return variable;
        }


        /// <summary>
        // Extracts "fY" and "-2" from "fY¤-2". Extracts "fY" and "¤2000" from "fY¤¤2000". Not used much in Gekko 3.0.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="variable"></param>
        /// <param name="lag"></param>
        public static void ExtractVariableAndLag(string key, out string variable, out string lag)
        {
            //NOTE: NO parsing of the lag as integer here!
            //NOTE: OK with this: some vars are of this type: fy¤¤2001q3 = absolute time
            int indx = key.IndexOf(Globals.lagIndicator);
            variable = key.Substring(0, indx - 0);
            lag = key.Substring(indx + 1, key.Length - (indx + 1));
        }

        /// <summary>
        /// Convert a GekkoTime period into corresponding period for all frequencies. Used for instance when printing 
        /// a quarterly series over a monthly time period. Important method.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static AllFreqsHelper ConvertDateFreqsToAllFreqs(GekkoTime t1, GekkoTime t2)
        {
            //Also see #345632473

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            if (t1.IsNull()) return null;

            AllFreqsHelper allFreqsHelper = new Gekko.AllFreqsHelper();

            GekkoTime.ConvertFreqs(EFreq.A, t1, t2, ref allFreqsHelper.t1Annual, ref allFreqsHelper.t2Annual);
            if (GekkoTime.Observations(allFreqsHelper.t1Annual, allFreqsHelper.t2Annual) < 1)
            {
                new Error("Start period must be <= end period");
            }
            GekkoTime.ConvertFreqs(EFreq.Q, t1, t2, ref allFreqsHelper.t1Quarterly, ref allFreqsHelper.t2Quarterly);
            if (GekkoTime.Observations(allFreqsHelper.t1Quarterly, allFreqsHelper.t2Quarterly) < 1)
            {
                new Error("Start period must be <= end period");
            }
            GekkoTime.ConvertFreqs(EFreq.M, t1, t2, ref allFreqsHelper.t1Monthly, ref allFreqsHelper.t2Monthly);
            if (GekkoTime.Observations(allFreqsHelper.t1Monthly, allFreqsHelper.t2Monthly) < 1)
            {
                new Error("Start period must be <= end period");
            }
            GekkoTime.ConvertFreqs(EFreq.W, t1, t2, ref allFreqsHelper.t1Weekly, ref allFreqsHelper.t2Weekly);
            if (GekkoTime.Observations(allFreqsHelper.t1Weekly, allFreqsHelper.t2Weekly) < 1)
            {
                new Error("Start period must be <= end period");
            }
            GekkoTime.ConvertFreqs(EFreq.D, t1, t2, ref allFreqsHelper.t1Daily, ref allFreqsHelper.t2Daily);
            if (GekkoTime.Observations(allFreqsHelper.t1Daily, allFreqsHelper.t2Daily) < 1)
            {
                new Error("Start period must be <= end period");
            }
            GekkoTime.ConvertFreqs(EFreq.U, t1, t2, ref allFreqsHelper.t1Undated, ref allFreqsHelper.t2Undated);
            if (GekkoTime.Observations(allFreqsHelper.t1Undated, allFreqsHelper.t2Undated) < 1)
            {
                new Error("Start period must be <= end period");
            }
            return allFreqsHelper;
        }

        /// <summary>
        /// Pick out the "right" converted period from AllFreqsHelper. A bit of a waste of effort to do it like this, but usually not
        /// part of speed-critical code.
        /// </summary>
        /// <param name="dates"></param>
        /// <param name="freqHere"></param>
        /// <param name="gt1"></param>
        /// <param name="gt2"></param>
        public static void PickFromAllFreqs(AllFreqsHelper dates, EFreq freqHere, out GekkoTime gt1, out GekkoTime gt2)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            if (freqHere == EFreq.A)
            {
                gt1 = dates.t1Annual;
                gt2 = dates.t2Annual;
            }
            else if (freqHere == EFreq.Q)
            {
                gt1 = dates.t1Quarterly;
                gt2 = dates.t2Quarterly;
            }
            else if (freqHere == EFreq.M)
            {
                gt1 = dates.t1Monthly;
                gt2 = dates.t2Monthly;
            }
            else if (freqHere == EFreq.W)
            {
                gt1 = dates.t1Weekly;
                gt2 = dates.t2Weekly;
            }
            else if (freqHere == EFreq.D)
            {
                gt1 = dates.t1Daily;
                gt2 = dates.t2Daily;
            }
            else if (freqHere == EFreq.U)
            {
                gt1 = dates.t1Undated;
                gt2 = dates.t2Undated;
            }
            else
            {
                new Error("Freq error"); gt1 = GekkoTime.tNull; gt2 = GekkoTime.tNull;
            }
        }

        /// <summary>
        /// Add frequency to a varname, if it is missing (x --> x!q, if frequency is quarterly ("q")).
        /// </summary>
        /// <param name="varname"></param>
        /// <param name="freq"></param>
        /// <param name="type"></param>
        /// <param name="isLeftSideVariable"></param>
        /// <returns></returns>
        public static string AddFreq(string varname, string freq, EVariableType type, O.ELookupType isLeftSideVariable)
        {
            //freq is added for all no-sigil rhs
            //freq is added for lhs if it is no-sigil AND the type is SERIES or VAR

            //For a simple timeseries loop, the below gives 17% more speed.
            //if (varname.StartsWith("%")) return varname;
            //else return varname + "!a";+

            bool hasSigil = G.Chop_HasSigil(varname);

            string varnameWithFreq = varname;

            if ((isLeftSideVariable != O.ELookupType.LeftHandSide && !hasSigil) || (isLeftSideVariable == O.ELookupType.LeftHandSide && !hasSigil && (type == EVariableType.Var || type == EVariableType.Series)))
            {
                //Series has '!' added
                //In VAL v = 100, there will be no freq added.
                if (!G.Contains(varname, Globals.freqIndicatorString))
                {
                    if (freq != null) varnameWithFreq = varname + Globals.freqIndicator + freq;
                    else varnameWithFreq = varname + Globals.freqIndicator + G.ConvertFreq(Program.options.freq);
                }
            }
            return varnameWithFreq;
        }

        /// <summary>
        /// Add frequency to a varname. Will replace AddFreq() method.
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static string AddFreqToName(string varName, string freq)
        {
            //Only used internally, when dealing with databanks. Not relevant for
            //outside use.
            if (freq == null) return G.Chop_AddFreq(varName, Program.options.freq);
            else return G.Chop_AddFreq(varName, freq);
        }

        /// <summary>
        /// Lighter color: factor=0 is no change, factor=1 is pure white. See also Darker()
        /// </summary>
        /// <param name="color"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color Lighter(System.Windows.Media.Color color, double factor)
        {
            System.Windows.Media.Color rv = System.Windows.Media.Color.FromArgb(255, (byte)((double)color.R + factor * (255d - (double)color.R)), (byte)((double)color.G + factor * (255d - (double)color.G)), (byte)((double)color.B + factor * (255d - (double)color.B)));
            return rv;
        }

        /// <summary>
        /// Darker color: factor=0 is no change, factor=1 is pure black. See also Darker()
        /// </summary>
        /// <param name="color"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static System.Windows.Media.Color Darker(System.Windows.Media.Color color, double factor)
        {
            System.Windows.Media.Color rv = System.Windows.Media.Color.FromArgb(255, (byte)((1 - factor) * color.R), (byte)((1 - factor) * color.G), (byte)((1 - factor) * color.B));
            return rv;
        }

        /// <summary>
        /// Helper method for plural "s", like 0 elefants, 1 elefant, 2 elefants, 3 elefants.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string S(int count)
        {
            if (count == 1) return "";
            else return "s";
        }

        /// <summary>
        /// For current freq, returns 4 for !q, 12 for !m, else 1. So 1 for !a and !u (but also 1 for !d and !w...).
        /// </summary>
        /// <returns></returns>
        public static int CurrentSubperiods()
        {
            return Subperiods(Program.options.freq);
        }

        /// <summary>
        /// Returns 4 for !q, 12 for !m, else 1. So 1 for !a and !u (but also 1 for !d and !w...).
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static int Subperiods(EFreq freq)
        {
            int x = 1;
            if (freq == EFreq.Q) x = Globals.freqQSubperiods;
            else if (freq == EFreq.M) x = Globals.freqMSubperiods;
            return x;
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static EFreq ConvertFreq(string freq)
        {
            EFreq f = EFreq.None;
            if (freq == null) return f;
            try
            {
                f = Globals.freqFromStringToEnum[freq.ToLower()];
            }
            catch
            {
                new Error("Frequency '" + freq + "' not recognized");
            }
            return f;
        }

        /// <summary>
        /// Convert from string to EFreq. Can optionally return current freq if string == null.
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="nullIsCurrent"></param>
        /// <returns></returns>
        public static EFreq ConvertFreq(string freq, bool nullIsCurrent)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================
            EFreq eFreq = EFreq.A;
            if (G.Equal(freq, "a"))
            {
                //do nothing
            }
            else if (G.Equal(freq, "q"))
            {
                eFreq = EFreq.Q;
            }
            else if (G.Equal(freq, "m"))
            {
                eFreq = EFreq.M;
            }
            else if (G.Equal(freq, "w"))
            {
                eFreq = EFreq.W;
            }
            else if (G.Equal(freq, "d"))
            {
                eFreq = EFreq.D;
            }
            else if (G.Equal(freq, "u"))
            {
                eFreq = EFreq.U;
            }
            else
            {
                if (nullIsCurrent)
                {
                    eFreq = Program.options.freq;
                }
                else
                {
                    new Error("Regarding frequency: '" + freq + "' not recognized");
                }
            }
            return eFreq;
        }

        /// <summary>
        /// Get the freq part of a name (for instance q in x!q) and return it as EFreq.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static EFreq GetFreqFromName(string s)
        {
            string f = G.Chop_GetFreq(s);
            if (f == null)
            {
                new Error("freq problem"); return EFreq.None;
            }
            else
            {
                return G.ConvertFreq(f);
            }
        }

        /// <summary>
        /// Test if variable type is null
        /// </summary>
        /// <param name="x1"></param>
        /// <returns></returns>
        public static bool IsGekkoNull(IVariable x1)
        {
            return x1.Type() == EVariableType.Null;
        }

        /// <summary>
        /// Helper method, return the number of fields in GekkoTime object. Technical use.
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static int FreqType(Series ts)
        {
            if (ts.freq == EFreq.A || ts.freq == EFreq.U) return 1;
            else if (ts.freq == EFreq.M || ts.freq == EFreq.Q) return 2;
            else if (ts.freq == EFreq.D) return 3;
            else
            {
                new Error("Internal error #726326283"); return -12345;
            }
        }


        // ===========================================================================================================================
        // ========================= functions to manipulate bankvarnames with indexes start =========================================
        // ===========================================================================================================================


        public static bool Chop_HasFreq(string bankvarname)
        {
            if (G.Chop_GetFreq(bankvarname) != null) return true;
            return false;
        }

        /// <summary>
        /// Whether or not a bankvarname has an index (like array-series x[a, b]).
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <returns></returns>
        public static bool Chop_HasIndex(string bankvarname)
        {
            if (G.Chop_GetIndex(bankvarname).Count > 0) return true;
            return false;
        }

        /// <summary>
        /// Get bank part of bankvarname. Returns blank if no bank.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_GetBank(string bankvarname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            string ss = "";
            if (bank != null) ss = bank;
            return ss;
        }

        /// <summary>
        /// Get name part of bankvarname. Freq is not included.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_GetName(string bankvarname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            return name;
        }

        /// <summary>
        /// Get freq part of bankvarname. Returns blank if no freq.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_GetFreq(string bankvarname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            if (freq == null) return "";
            else return freq;
        }

        /// <summary>
        /// Get name + freq from bankvarname.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_GetNameAndFreq(string bankvarname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            if (freq == null) return name;
            else return name + Globals.freqIndicator + freq;
        }

        public static string Chop_GetNameAndFreqAndIndex(string bankvarname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            return O.UnChop(null, name, freq, index);
        }

        /// <summary>
        /// Get index part of bankvarname, for instance x!q[a, b] returns ["a", "b"]. May return empty list.
        /// </summary>
        /// <param name="s1"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static List<string> Chop_GetIndex(string s1)
        {
            string bank, name, freq; string[] index;
            O.Chop(s1, out bank, out name, out freq, out index);
            if (index == null) return new List<string>();
            else return new List<string>(index);
        }

        /// <summary>
        /// Constructs a bankvarname with freq and indexes from its chunks/parts. Choose if blanks between index elements like [a, b, c] (" ") or [a,b,c] (null)
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="name"></param>
        /// <param name="freq"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_GetFullName(string bank, string name, string freq, string[] index, string listBlanks)
        {
            string s = O.UnChop(bank, name, freq, index, listBlanks);
            return s;
        }

        /// <summary>
        /// Constructs a bankvarname with freq and indexes from its chunks/parts. Index elements contain blanks like [a, b, c], not [a,b,c].
        /// Else see overload.
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="name"></param>
        /// <param name="freq"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string Chop_GetFullName(string bank, string name, string freq, string[] index)
        {
            string s = O.UnChop(bank, name, freq, index, " ");
            return s;
        }

        /// <summary>
        /// Add bank to bankvarname, if not already there.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bank"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_AddBank(string name, string bank)
        {
            string bank0, name0, freq; string[] index;
            O.Chop(name, out bank0, out name0, out freq, out index);
            if (bank0 == null)
            {
                return O.UnChop(bank, name0, freq, index);
            }
            else
            {
                return name;
            }
        }

        /// <summary>
        /// Set bank in bankvarname (will override existing bank)
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="bankname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_SetBank(string bankvarname, string bankname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            return O.UnChop(bankname, name, freq, index);
        }

        /// <summary>
        /// Remove bank from bankvarname.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_RemoveBank(string bankvarname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            return O.UnChop(null, name, freq, index);
        }

        /// <summary>
        /// Remove particular bank from bankvarname. Keep if different.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="bankname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_RemoveBank(string bankvarname, string bankname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            string bankRemove = bankname;
            if (G.Equal(bankRemove, bank)) bank = null;
            return O.UnChop(bank, name, freq, index);
        }

        /// <summary>
        /// Replace bankname in bankvarname
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="bankname1"></param>
        /// <param name="bankname2"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_ReplaceBank(string bankvarname, string bankname1, string bankname2)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            if (G.Equal(bankname1, bank)) bank = bankname2;
            return O.UnChop(bank, name, freq, index);
        }

        /// <summary>
        /// Add freq to bankvarname, if not already there
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="freqname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_AddFreq(string bankvarname, string freqname)
        {
            //only adds a freq if there is no freq already
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            if (G.Chop_HasSigil(name)) return bankvarname;
            if (freq == null)
            {
                return O.UnChop(bank, name, freqname, index);
            }
            else
            {
                return bankvarname;
            }
        }

        /// <summary>
        /// Variant
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static string Chop_AddFreq(string bankvarname, EFreq freq)
        {
            return Chop_AddFreq(bankvarname, G.ConvertFreq(freq));
        }

        /// <summary>
        /// Set freq in bankvarname, will override.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="freqname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_SetFreq(string bankvarname, string freqname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            if (G.Chop_HasSigil(name)) return bankvarname;
            return O.UnChop(bank, name, freqname, index);
        }

        /// <summary>
        /// Variant.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static string Chop_SetFreq(string bankvarname, EFreq freq)
        {
            return Chop_SetFreq(bankvarname, G.ConvertFreq(freq));
        }

        /// <summary>
        /// Remove index part of varname, for instance x[a, b] --> x.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_RemoveIndex(string bankvarname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            if (G.Chop_HasSigil(name)) return bankvarname;
            return O.UnChop(bank, name, freq, null);
        }

        public static string Chop_GetNameAndIndex(string bankvarname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            return O.UnChop(null, name, null, index);
        }

        /// <summary>
        /// Remove freq part of varnamne.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_RemoveFreq(string bankvarname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            if (G.Chop_HasSigil(name)) return bankvarname;
            return O.UnChop(bank, name, null, index);
        }

        /// <summary>
        /// Remove freq part of name, if the freq part is equal to second argument.
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="freqname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_RemoveFreq(string bankvarname, string freqname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            if (G.Chop_HasSigil(name)) return bankvarname;
            string freqRemove = freqname;
            if (G.Equal(freqRemove, freq)) freq = null;
            return O.UnChop(bank, name, freq, index);
        }

        /// <summary>
        /// Variant
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static string Chop_RemoveFreq(string bankvarname, EFreq freq)
        {
            return Chop_RemoveFreq(bankvarname, G.ConvertFreq(freq));
        }

        /// <summary>
        /// Replace a certain freq with some other
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="freq1"></param>
        /// <param name="freq2"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_ReplaceFreq(string bankvarname, string freq1, string freq2)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            if (G.Chop_HasSigil(name)) return bankvarname;
            if (G.Equal(freq1, freq)) freq = freq2;
            return O.UnChop(bank, name, freq, index);
        }

        /// <summary>
        /// Variant
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="freq2"></param>
        /// <param name="freq3"></param>
        /// <returns></returns>
        public static string Chop_ReplaceFreq(string s1, EFreq freq2, EFreq freq3)
        {
            return Chop_ReplaceFreq(s1, G.ConvertFreq(freq2), G.ConvertFreq(freq3));
        }

        /// <summary>
        /// Set name part of bankvarname
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="newname"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_SetName(string bankvarname, string newname)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            name = newname;
            return O.UnChop(bank, name, freq, index);
        }

        /// <summary>
        /// Insert a prefix in a name in a bankvarname
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string Chop_SetNamePrefix(string bankvarname, string prefix)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            name = prefix + name;
            return O.UnChop(bank, name, freq, index);
        }

        /// <summary>
        /// Insert a suffix in a name in a bankvarname
        /// </summary>
        /// <param name="bankvarname"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string Chop_SetNameSuffix(string bankvarname, string suffix)
        {
            string bank, name, freq; string[] index;
            O.Chop(bankvarname, out bank, out name, out freq, out index);
            name = name + suffix;
            return O.UnChop(bank, name, freq, index);
        }

        /// <summary>
        /// True if varname starts with % or #.
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static bool Chop_HasSigil(string varname)
        {
            if (Program.IsListfileArtificialName(varname)) return true;  //otherwise Chop_GetName gets it wrong below
            string varname2 = Chop_GetName(varname);
            if (varname2 == null || varname2.Length == 0)
            {
                new Error("Variable name with zero length");
            }
            bool hasSigil = false;
            if (varname2[0] == Globals.symbolScalar || varname2[0] == Globals.symbolCollection) hasSigil = true;
            return hasSigil;
        }

        /// <summary>
        /// Returns null or '%' or '#' depending on varname. Null for series.
        /// </summary>
        /// <param name="varname"></param>
        /// <returns></returns>
        public static string Chop_GetSigil(string varname)
        {
            if (Program.IsListfileArtificialName(varname)) return Globals.symbolCollection.ToString();  //otherwise Chop_GetName gets it wrong below
            string varname2 = Chop_GetName(varname);
            if (varname2 == null || varname2.Length == 0)
            {
                new Error("Variable name with zero length");
            }
            if (varname2[0] == Globals.symbolScalar) return Globals.symbolScalar.ToString();
            else if (varname2[0] == Globals.symbolCollection) return Globals.symbolCollection.ToString();
            else return null;
        }

        /// <summary>
        /// Ignore lag part in for instance x¤-1 or x[-1]. 
        /// For something like [-1] or [+1] use the method Chop_RemoveLagOrLead() instead -- faster and more reliable!
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Chop_RemoveLagOrLead_OLD(string key, string code)
        {
            string variable = null;
            if (key == null) return null;
            if (key.Contains("|"))
            {
                //Total hack here
                string[] ss = key.Split('|');
                if (ss.Length >= 1 && G.IsIdent(ss[0].Trim())) variable = ss[0].Trim();
            }
            else
            {
                int indx = key.LastIndexOf(code); //in decomp window, we may have x['a', 'z'][-1], so therefore we look for the last '['       
                if (indx != -1)
                {
                    string rest = key.Substring(indx);
                    if (rest.Contains("'") || rest.Contains(Globals.symbolCollection.ToString())) variable = key;  //if input is x['a', 'z'] or x[#i, #j], etc.
                    else variable = key.Substring(0, indx - 0);
                }
                else variable = key;
            }
            return variable;
        }

        /// <summary>
        /// Removes [-1], [+1], even [-0] or [+0], for instance x[-1] --> x, or x[a, b][-1] --> x[a, b].
        /// The method is pretty fast, and it does some sanity checks that [...] is really a +/- int.
        /// Note: x[0], x[2] etc. will not count as lag, x[+2] must be used.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Chop_RemoveLagOrLead(string name)
        {
            List<int> m = null;
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '[')
                {
                    if (m == null) m = new List<int>();
                    m.Add(i);
                }
            }
            if (m == null) return name;
            if (m.Count > 2) new Error("Expected at most 2 '[' in variable name");
            int last = m[m.Count - 1];
            string sub = name.Substring(last).Trim(); //now something like "[-1]"
            if (sub[sub.Length - 1] != ']') return name;
            string sub2 = G.Substring(sub, 1, sub.Length - 2).Trim();
            if (sub2.Length < 2) return name;
            if (!(sub2[0] == '-' || sub2[0] == '+')) return name;
            string sub3 = sub2.Substring(1);
            if (!G.IsInteger(sub3, false, true)) return name;  //We accept x[-0] or x[+0]
            return G.Substring(name, 0, last - 1);
        }

        /// <summary>
        /// /// Chops up into all components
        /// </summary>
        /// <param name="input2"></param>
        /// <param name="dbName"></param>
        /// <param name="varName"></param>
        /// <param name="freq"></param>
        /// <param name="indexes"></param>
        public static void Chop_Chop(string input2, out string dbName, out string varName, out string freq, out string[] indexes)
        {
            O.Chop(input2, out dbName, out varName, out freq, out indexes);
        }

        /// <summary>
        /// For something like x[a,b][c,d] or x[i][-1] chops up into all components
        /// </summary>
        /// <param name="input2"></param>
        /// <param name="dbName"></param>
        /// <param name="varName"></param>
        /// <param name="freq"></param>
        /// <param name="indexes"></param>
        public static void Chop_Chop_Jagged(string input2, out string dbName, out string varName, out string freq, out string[] indexes1, out string[] indexes2)
        {
            O.Chop_Jagged(input2, out dbName, out varName, out freq, out indexes1, out indexes2);
        }

        /// <summary>
        /// Constructs a bankvarname with freq and indexes from its chunks/parts. Index elements contain blanks like [a, b, c], not [a,b,c].
        /// Else see overload.
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="name"></param>
        /// <param name="freq"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string Chop_Unchop(string bank, string name, string freq, string[] index)
        {
            return O.UnChop(bank, name, freq, index);
        }

        /// <summary>
        /// Constructs a bankvarname with freq and indexes from its chunks/parts. Choose if blanks between index elements like [a, b, c] (" ") or [a,b,c] (null)
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="name"></param>
        /// <param name="freq"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string Chop_Unchop(string bank, string name, string freq, string[] index, string listBlanks)
        {
            return O.UnChop(bank, name, freq, index, listBlanks);
        }

        /// <summary>
        /// Adds a dimension to a name. If inputName = "x" and inputIndex = "40", the result
        /// will be x[40]. If inputName is "x[a, b]", the result will be "x[a, b, 40]".
        /// If inputname is "x[]", it will be treated as if it was "x".
        /// Last argument " " means blanks between commas (normal).
        /// Dimension is always added last.
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="inputIndex"></param>
        /// <returns></returns>
        public static string Chop_DimensionAddLast(string inputName, string inputIndex, string listBlanks)
        {
            string bank = null; string name = null; string freq = null; string[] indexes = null;
            G.Chop_Chop(inputName, out bank, out name, out freq, out indexes);
            string[] indexes2 = null;
            if (indexes == null || (indexes.Length == 1 && G.NullOrBlanks(indexes[0])))
            {
                indexes2 = new string[1];
                indexes2[0] = inputIndex;
            }
            else
            {
                indexes2 = new string[indexes.Length + 1];
                Array.Copy(indexes, indexes2, indexes2.Length - 1);
                indexes2[indexes2.Length - 1] = inputIndex;
            }
            return G.Chop_Unchop(bank, name, freq, indexes2, listBlanks);
        }

        public static string Chop_DimensionAddLast(string inputName, string inputIndex)
        {
            return Chop_DimensionAddLast(inputName, inputIndex, "");
        }

        /// <summary>
        /// Removes last dimension. For instance, "x[40]" becomes "x", and "x[a, b, 40]" becomes "x[a, b]".
        /// If no dimension like "x", the result will also be "x".
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="inputIndex"></param>
        /// <returns></returns>
        public static string Chop_DimensionRemoveLast(string inputName, string listBlanks)
        {
            string bank = null; string name = null; string freq = null; string[] indexes = null;
            G.Chop_Chop(inputName, out bank, out name, out freq, out indexes);
            string[] indexes2 = null;
            if (indexes == null || indexes.Length == 1)
            {
                //indexes2 will be null
            }
            else
            {
                indexes2 = new string[indexes.Length - 1];
                Array.Copy(indexes, indexes2, indexes2.Length);
            }
            return G.Chop_Unchop(bank, name, freq, indexes2, listBlanks);
        }

        /// <summary>
        /// This method is simple and fast, but is it bug-free?? Use it for Gekko 4.0.
        /// No trimming done at the end, should be superfluous.
        /// </summary>
        /// <param name="inputName"></param>
        /// <returns></returns>
        public static string Chop_DimensionRemoveLast_FASTER(string inputName)
        {
            int i3 = inputName.LastIndexOf(']');
            if (i3 == -1) return inputName;
            int i2 = inputName.LastIndexOf(',');
            int i1 = inputName.LastIndexOf('[');
            if (i2 != -1 && i1 < i2 && i2 < i3)
            {
                //Comma
                string s = G.Substring(inputName, 0, i2 - 1) + "]";
                return s;
            }
            else if (i1 < i3)
            {
                //No comma
                string s = G.Substring(inputName, 0, i1 - 1);
                return s;
            }
            else return inputName;
        }

        /// <summary>
        /// Handles a lag or lead like for instance "x[+1]".
        /// If name = "x", t0 = 2001 and t = 2002, it will return "x[+1]".
        /// With merge=false, "x[a]" will become "x[a][+1]", whereas with merge=true,
        /// we get "x[a, +1]". The former is normal.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="t0"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Chop_DimensionAddLag(string name, GekkoTime t0, GekkoTime t, bool merge, bool showT, string listBlanks)
        {
            string name2;
            string slag = GekkoTime.GetLagString(t0, t);
            if (slag == null)
            {
                if (showT) name2 = G.Chop_DimensionAddLast(name, "t", listBlanks); // name + "[t]";
                else name2 = name;
            }
            else
            {
                if (merge)
                {
                    if (showT) name2 = G.Chop_DimensionAddLast(name, "t" + slag, listBlanks);
                    else name2 = G.Chop_DimensionAddLast(name, slag, listBlanks);
                }
                else
                {
                    if (showT) name2 = name + "[" + "t" + slag + "]";
                    else name2 = name + "[" + slag + "]";
                }
            }
            return name2;
        }

        /// <summary>
        /// Sets lag when there is an existing time, like "x[a, 2002]", which for t0=2001 would be
        /// changed into "x[a][+1]" if merge=false (which is normal), or else "x[a, +1]".
        /// </summary>
        /// <param name="name"></param>
        /// <param name="t0"></param>
        /// <param name="t"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        public static string Chop_DimensionConvertToLag(string name, GekkoTime t0, bool merge, bool showT, string listBlanks)
        {
            return Chop_DimensionAddLag(Chop_DimensionRemoveLast(name, listBlanks), t0, Chop_DimensionGetPeriod(name), merge, showT, listBlanks);
        }

        /// <summary>
        /// In a name like "x[a, 2002]" or "x[a,2002]" the method returns the GekkoTime 2002.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static GekkoTime Chop_DimensionGetPeriod(string name)
        {
            List<string> ss = Chop_GetIndex(name);
            if (ss == null) new Error("No index found");
            string time = ss[ss.Count - 1];
            GekkoTime t = GekkoTime.FromStringToGekkoTime(time, false, false, false);  //does not report error, for instance if an equation like E_tIOy_tBase[d,s] does not have a time index. In that case, GekkoTime.tNull is returned.            
            return t;
        }

        // ===========================================================================================================================
        // ========================= functions to manipulate bankvarnames with indexes end ===========================================
        // ===========================================================================================================================

        /// <summary>
        /// Helper method for missings handling
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static ESeriesMissing GetMissing(string s)
        {
            if (G.Equal(s, "error")) return ESeriesMissing.Error;
            else if (G.Equal(s, "m")) return ESeriesMissing.M;
            else if (G.Equal(s, "zero")) return ESeriesMissing.Zero;
            else if (G.Equal(s, "skip")) return ESeriesMissing.Skip;
            else if (G.Equal(s, "ignore")) return ESeriesMissing.Ignore;
            else
            {
                new Error("Expected missing = error, m, zero, skip or ignore"); return ESeriesMissing.Error;
                //throw new GekkoException();
            }

        }

        /// <summary>
        /// Extracts "fY" from "fY¤-2"
        /// </summary>
        /// <param name="key">Input</param>        
        public static string ExtractOnlyVariableIgnoreLag(string key)
        {
            return Chop_RemoveLagOrLead_OLD(key, Globals.lagIndicator);
        }

        /// <summary>
        /// True if between a..z or A..Z
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsEnglishLetter(char c)
        {
            //Problem is that char.IsLetter accepts æøå and others
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        /// <summary>
        /// For a string like " [a,b, c , 'd',e ]" it splits up into array "a", "b", "c", "d", "e". Quite robust.
        /// Can also handle for instance "[-1]" or "[+1]".
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] SplitIndexerIntoParts(string s)
        {
            return s.Trim().Trim('[', ']').Split(',').Select(x => G.StripQuotes(x.Trim())).ToArray();
        }

        /// <summary>
        /// Remove single quotes from string: 'ab' --> ab.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StripQuotes(string s)
        {
            if (s == null) return null;
            if (s.StartsWith("'") && s.EndsWith("'"))
            {
                s = s.Substring(1, s.Length - 2);
            }
            return s;
        }

        /// <summary>
        /// Remove double quotes from string: "ab" --> ab
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StripQuotes2(string s)
        {
            if (s == null) return null;
            if (s.StartsWith("\"") && s.EndsWith("\""))
            {
                s = s.Substring(1, s.Length - 2);
            }
            return s;
        }

        /// <summary>
        /// Checks if a name is "simple", a38, f16, var2, _var3, x_y etc. Cannot start with digit.
        /// Option to allow frequency.
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="allowFreqIndicator"></param>
        /// <returns></returns>
        public static bool IsSimpleToken(string varName, bool allowFreqIndicator)
        {
            //must be like a38, f16, var2, _var3, x_y etc. Cannot start with digit.
            if (varName == null) return false;
            if (varName.Length == 0) return false;
            if (!G.IsLetterOrUnderscore(varName[0])) return false;
            for (int jj = 1; jj < varName.Length; jj++)
            {
                if (!allowFreqIndicator)
                {
                    if (!G.IsLetterOrDigitOrUnderscore(varName[jj]))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!G.IsLetterOrDigitOrUnderscoreOrExclamation(varName[jj]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public static bool IsSimpleToken(string varName)
        {
            return IsSimpleToken(varName, false);  //no turtle allowed, maybe remove that
        }

        /// <summary>
        /// Appends blanks so width is met.
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string varFormat(string level1, int width)
        {
            return level1 + G.Blanks(width - level1.Length);
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="level1"></param>
        /// <returns></returns>
        public static string varFormat(string level1)
        {
            return varFormat(level1, 12);
        }

        /// <summary>
        /// Get seconds elapsed since param1. See also SecondsUtc().
        /// </summary>
        /// <param name="t0"></param>
        /// <returns></returns>
        public static string Seconds(DateTime t0)
        {
            double milliseconds = (DateTime.Now - t0).TotalMilliseconds;
            string s = SecondsFormat(milliseconds);
            return s;
        }

        /// <summary>
        /// Get seconds elapsed since param1, but where param1 is in UTC time. See also Seconds().
        /// </summary>
        /// <param name="t0"></param>
        /// <returns></returns>
        public static string SecondsUtc(DateTime t0)
        {
            double milliseconds = (DateTime.UtcNow - t0).TotalMilliseconds;
            string s = SecondsFormat(milliseconds);
            return s;
        }

        /// <summary>
        /// Show elapsed milliseconds in a readable format
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static string SecondsFormat(double milliseconds)
        {
            double total = milliseconds / 1000d;
            string s = null;
            if (total < 1d)
            {
                s = total.ToString("0.0000") + " sec";
            }
            else
            {
                s = total.ToString("0.00") + " sec";
            }

            if (total >= 60d)
            {
                string min = "";
                string sec = "";
                int minutes = (int)total / 60;
                min += minutes;
                int seconds = (int)total % 60;
                if (seconds <= 9) sec += "0";
                sec += seconds;
                s += " (" + min + ":" + sec + " min)";
            }
            return s;
        }

        //----------- used in prt statement start -----------------------------------

        /// <summary>
        /// Pretty prints a number with blanks appended so width is met.
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string levelFormat(double level1, int width)
        {
            string levelFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + width + ":0.0000}", level1);
            if (double.IsNaN(level1)) levelFormatted = Globals.printNaNIndicator;
            return G.Blanks(width - levelFormatted.Length) + levelFormatted;
        }

        /// <summary>
        /// Pretty prints a percentage with blanks appended so width is met.
        /// </summary>
        /// <param name="pch1"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string pchFormat(double pch1, int width)
        {
            string pchFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + width + ":0.00}", pch1);
            if (double.IsNaN(pch1) || pchFormatted.Length > width)
            {
                pchFormatted = "";
                int width2 = Math.Min(6, width);
                for (int i = 0; i < width2; i++) pchFormatted += "*";
            }
            return G.Blanks(width - pchFormatted.Length) + pchFormatted;
        }

        /// <summary>
        /// Pretty prints a dlog with blanks appended so width is met.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string dlogFormat(double input, int width)
        {
            string dlogFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + width + ":0.0000}", input);
            if (double.IsNaN(input) || dlogFormatted.Length > width)
            {
                dlogFormatted = "";
                int width2 = Math.Min(6, width);
                for (int i = 0; i < width2; i++) dlogFormatted += "*";
            }
            return G.Blanks(width - dlogFormatted.Length) + dlogFormatted;
        }

        /// <summary>
        /// Preprends blanks to a string so width is met.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string StringFormat(string input, int width)
        {
            return G.Blanks(width - input.Length) + input;  //right-aligned
        }

        //----------- used in prt statement end -----------------------------------


        /// <summary>
        /// Pretty print of double with decimals.
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="decimals"></param>
        /// <param name="missFunction"></param>
        /// <returns></returns>
        public static string UpdprtFormat(double level1, int decimals, bool missFunction)
        {
            string z = new string('0', decimals);
            string levelFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0." + z + "}", level1);
            if (G.IsNumericalError(level1))
            {
                if (missFunction) levelFormatted = "m()";
                else levelFormatted = "M";
            }
            return levelFormatted;
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="level1"></param>
        /// <returns></returns>
        public static string levelFormatOld(double level1)
        {
            return levelFormatOld(level1, 14);
        }

        /// <summary>
        /// Pretty print a double so a width is met
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string levelFormatOld(double level1, int width)
        {
            int widthM1 = width - 1;
            string levelFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + widthM1 + ":0.0000}", level1);
            if (double.IsNaN(level1)) levelFormatted = Globals.printNaNIndicator;
            return G.Blanks(width - levelFormatted.Length) + levelFormatted;
        }

        /// <summary>
        /// Pretty print an it so a width is met
        /// </summary>
        /// <param name="input"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string IntFormat(int input, int width)
        {
            string formatted = input.ToString();
            return G.Blanks(width - formatted.Length) + formatted;
        }

        /// <summary>
        /// Variant
        /// </summary>
        /// <param name="pch1"></param>
        /// <returns></returns>
        public static string pchFormatOld(double pch1)
        {
            return pchFormatOld(pch1, 8);
        }

        /// <summary>
        /// Pretty print a percentage so a width is met
        /// </summary>
        /// <param name="pch1"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string pchFormatOld(double pch1, int width)
        {
            int widthM1 = width - 1;
            int widthM2 = width - 2;
            string pchFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + widthM2 + ":0.00}", pch1);
            if (double.IsNaN(pch1) || pchFormatted.Length > widthM1) pchFormatted = "******";
            return G.Blanks(width - pchFormatted.Length) + pchFormatted;
        }

        /// <summary>
        /// Left-adjust a string so width is met
        /// </summary>
        /// <param name="date"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string dateFormat(string date, int width)
        {
            //this format is left-adjusted, so we get this:
            //
            //  2011q9
            //  2011q10
            //  2011q11
            //
            return date + G.Blanks(width - date.Length);
        }

        /// <summary>
        /// Pretty print dlog
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string dlogFormatOld(double input)
        {
            return dlogFormatOld(input, 8);
        }

        /// <summary>
        /// Pretty print dlog
        /// </summary>
        /// <param name="input"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string dlogFormatOld(double input, int width)
        {
            int widthM1 = width - 1;
            int widthM2 = width - 2;
            string dlogFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + widthM2 + ":0.0000}", input);
            if (double.IsNaN(input) || dlogFormatted.Length > widthM1) dlogFormatted = "******";
            return G.Blanks(width - dlogFormatted.Length) + dlogFormatted;
        }

        /// <summary>
        /// Number formatting
        /// </summary>
        /// <param name="number"></param>
        /// <param name="format"></param>
        /// <param name="numberShouldShowAsN"></param>
        /// <param name="isTable"></param>
        /// <returns></returns>
        //Used for PRT, TABLE, etc.
        //See also #83490837432, these should be merged/fusioned
        public static string FormatNumber(double number, string format, bool numberShouldShowAsN, bool isTable)
        {
            int maxLength = 15; //default
            int decimals = 4; //default

            string format2 = format.ToLower();  //F --> f or S --> s

            try
            {

                if (!format2.StartsWith("f") && !format2.StartsWith("s"))
                {
                    new Error("Number format should start with 'f' or 's', e.g. 'f10.2': " + format);
                }
                string format3 = format.Substring(1);
                string[] format4 = format3.Split('.');
                if (format4.Length != 2)
                {
                    new Error("Number format should contain a '.', e.g. 'f10.2': " + format);
                }
                string f0 = format4[0];
                string f1 = format4[1];
                int ff0 = -12345;
                if (int.TryParse(f0, out ff0))
                {
                    maxLength = ff0;
                }
                else
                {
                    new Error("Number format should contain numbers, e.g. 'f10.2': " + format);
                }
                int ff1 = -12345;
                if (int.TryParse(f1, out ff1))
                {
                    decimals = ff1;
                }
                else
                {
                    new Error("Number format should contain numbers, e.g. 'f10.2': " + format);
                }
            }
            catch
            {
                new Error("Number format: could not parse: " + format);
            }

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();

            if (isTable)
            {
                if (G.Equal(Program.options.table_decimalseparator, "comma"))
                {
                    nfi.NumberGroupSeparator = ".";
                    nfi.NumberDecimalSeparator = ",";
                }
                else
                {
                    nfi.NumberGroupSeparator = ",";
                    nfi.NumberDecimalSeparator = ".";
                }
            }

            string s = "";
            if (isTable && Program.options.table_thousandsseparator)
            {
                if (format2.StartsWith("f"))
                {
                    //the comma below does NOT control how decimal comma is displayed: see nfi.NumberGroupSeparator above 
                    if (decimals > 0) s = number.ToString("#,0." + new string('0', decimals), nfi);
                    else if (decimals < 0) s = (Math.Round(number / Math.Pow(10d, -decimals), 0, MidpointRounding.AwayFromZero) * Math.Pow(10d, -decimals)).ToString("#,0", nfi);
                    else s = number.ToString("#,0", nfi);
                }
                else if (format2.StartsWith("s"))
                {
                    s = number.ToString("0." + new string('0', decimals) + "e+00", nfi);
                }
                else
                {
                    new Error("Table format error");
                }
            }
            else
            {
                if (format2.StartsWith("f"))
                {
                    if (decimals > 0) s = number.ToString("0." + new string('0', decimals), nfi);
                    else if (decimals < 0) s = (Math.Round(number / Math.Pow(10d, -decimals), 0, MidpointRounding.AwayFromZero) * Math.Pow(10d, -decimals)).ToString("0", nfi);
                    else s = number.ToString("0", nfi);
                }
                else if (format2.StartsWith("s"))
                {
                    s = number.ToString("0." + new string('0', decimals) + "e+00", nfi);
                }
                else
                {
                    new Error("Table format error");
                }
            }

            if (s.Length > maxLength)
                s = new string('*', maxLength);
            if (s.Length < maxLength)
                s = new string(' ', maxLength - s.Length) + s;

            if (double.IsNaN(number) && numberShouldShowAsN)
            {
                s = new string(' ', s.Length - 1) + "N";  //non-existing
            }
            if (s.Trim() == "NaN")
            {
                s = new string(' ', s.Length - 1) + "M";
            }

            return s;
        }

        /// <summary>
        /// True if the current thread is a DECOMP or FIND thread (these threads are used for DECOMP or FIND windows).
        /// </summary>
        /// <returns></returns>
        public static bool IsDecompOrFindThread()
        {
            return Thread.CurrentThread.Name == "Find" || Thread.CurrentThread.Name == "Decomp" || Thread.CurrentThread.Name == "Flow" || Thread.CurrentThread.Name == "Plot";
        }

        /// <summary>
        /// Thin wrapper on RemoveLagOrLead(), see that.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasLagOrLead(string name)
        {
            if (name == G.Chop_RemoveLagOrLead(name)) return true;
            return false;
        }

        /// <summary>
        /// With reverse=false, removes blank characters in a string fast. Maybe a factor 2-3 faster than .Replace(" ", "").
        /// But not tested. Fast return if input has no blanks (the input string is returned).
        /// Beware that blanks inside single-quoted strings are preserved, so with input
        /// "a, b, c, 'd, e, f', h" we get --> "a,b,c,'d, e, f',h".
        /// With reverse=true, "a,b,c,'d,e,f',h" becomes --> "a, b, c, 'd,e,f', h"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string HandleBlanks(string s, bool inverse)
        {
            if (s == null) return null;
            if (!inverse)
            {
                bool hit = false; //maybe using this is a bit faster?
                foreach (char c in s)
                {
                    if (c == ' ')
                    {
                        hit = true;
                        break;
                    }
                }
                if (!hit) return s;  //fast without object construction if no blanks at all in input            
            }
            StringBuilder sb = new StringBuilder(s.Length);
            bool insidePling = false;
            foreach (char c in s)
            {
                if (inverse)
                {
                    if (c == ',')
                    {
                        if (insidePling) sb.Append(c); //do not touch inside single quotes.
                        else sb.Append(c).Append(' ');
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                else
                {
                    if (c == ' ')
                    {
                        if (insidePling) sb.Append(c); //allow blanks inside '...' single quotes.
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }

                if (c == '\'')
                {
                    if (insidePling == false)
                    {
                        insidePling = true;
                    }
                    else
                    {
                        insidePling = false;
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Removes blank characters in a string fast. Maybe a factor 2-3 faster than .Replace(" ", "").
        /// But not tested. Fast return if input has no blanks (the input string is returned).
        /// Beware that blanks inside single-quoted strings are preserved, so with input
        /// "a, b, c, 'd, e, f', h" we get --> "a,b,c,'d, e, f',h". See also HandleBlanksResurrect().
        /// </summary>
        /// <param name="s"></param>
        /// <param name="inverse"></param>
        /// <returns></returns>
        public static string HandleBlanksRemove(string s)
        {
            return G.HandleBlanks(s, false);
        }

        public static string HandleBlanksHacky(string s)
        {
            return s.Replace(", ", ",");
        }

        /// <summary>
        /// Removes a string like for instance "x[i, j]" in the list of strings "a", "b", "X[i,j]", "c", because
        /// blanks are ignored and case does not matter.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static void HandleBlanksRemove(List<string> m, string s)
        {
            List<string> m2 = new List<string>();
            bool hit = false;
            foreach (string x in m)
            {
                if (G.EqualHandleBlanks(s, x))
                {
                    hit = true;
                }
                else
                {
                    m2.Add(x);
                }
            }
            if (hit)
            {
                m.Clear();
                foreach (string x in m2)
                {
                    m.Add(x);
                }
            }
        }

        /// <summary>
        /// Resurrects blank characters around commas, cf. HandleBlanksRemove().
        /// Note "a,b,c,'d,e,f',h" becomes --> "a, b, c, 'd,e,f', h". So inside quotes is not touched.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="inverse"></param>
        /// <returns></returns>
        public static string HandleBlanksResurrect(string s)
        {
            return G.HandleBlanks(s, true);
        }

        /// <summary>
        /// Return this number of blanks. Will return "" if count <= 0.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Blanks(int count)
        {
            if (count <= 0) return "";
            return "".PadLeft(count);
        }

        /// <summary>
        /// TSP helper method
        /// </summary>
        /// <param name="al"></param>
        /// <param name="alType"></param>
        /// <param name="i"></param>
        /// <param name="relativePosition"></param>
        /// <returns></returns>
        public static String TspUtilityFindToken(List<string> al, List<string> alType, int i, int relativePosition)
        {
            int ii = -12345;
            return TspUtilityFindWord(out ii, 0, al, alType, i, relativePosition);
        }

        /// <summary>
        /// TSP helper method
        /// </summary>
        /// <param name="al"></param>
        /// <param name="alType"></param>
        /// <param name="i"></param>
        /// <param name="relativePosition"></param>
        /// <returns></returns>
        public static String TspUtilityFindType(List<string> al, List<string> alType, int i, int relativePosition)
        {
            int ii = -12345;
            return TspUtilityFindWord(out ii, 1, al, alType, i, relativePosition);
        }

        /// <summary>
        /// TSP helper method.
        /// </summary>
        /// <param name="ii"></param>
        /// <param name="type"></param>
        /// <param name="al"></param>
        /// <param name="alType"></param>
        /// <param name="i"></param>
        /// <param name="relativePosition"></param>
        /// <returns></returns>
        public static String TspUtilityFindWord(out int ii, int type, List<string> al, List<string> alType, int i, int relativePosition)
        {
            if (relativePosition == 0)
            {
                ii = 0;
                if (type == 0) return (String)al[i];
                else return (String)alType[i];
            }
            int counter = 0;
            do
            {
                if (relativePosition > 0) i++;
                if (relativePosition < 0) i--;
                if (i < 0 || i > al.Count - 1)
                {
                    ii = -12345;
                    return "";
                }
                String token = (String)al[i];
                String tokenType = (String)alType[i];
                if (tokenType == "WhiteSpace") continue;
                counter++;
                if (counter == Math.Abs(relativePosition))
                {
                    ii = i;
                    if (type == 0) return token;
                    else return tokenType;
                }
            }
            while (true);
        }

        /// <summary>
        /// TSP helper method
        /// </summary>
        /// <param name="al"></param>
        /// <param name="alType"></param>
        /// <param name="i"></param>
        /// <param name="relativePosition"></param>
        /// <returns></returns>
        public static int TspUtilitiesFindIndex(List<string> al, List<string> alType, int i, int relativePosition)
        {
            int ii = -12345;
            TspUtilityFindWord(out ii, 1, al, alType, i, relativePosition);
            return ii;
        }

        /// <summary>
        /// Make an exact copy of a databank
        /// </summary>
        /// <param name="newDatabank"></param>
        /// <param name="originalDatabank"></param>                
        public static void CloneDatabank(Databank newDatabank, Databank originalDatabank)
        {
            newDatabank.FileNameWithPath = originalDatabank.FileNameWithPath;
            newDatabank.FileNameWithPathPretty = originalDatabank.FileNameWithPathPretty;
            newDatabank.yearStart = originalDatabank.yearStart;
            newDatabank.yearEnd = originalDatabank.yearEnd;
            newDatabank.info1 = originalDatabank.info1;
            newDatabank.date = originalDatabank.date;
            newDatabank.isDirty = true;

            CloneHelper cloneHelper = new CloneHelper();
            //don't touch alias names: we are cloning the content of the databank, not altering its name.
            foreach (KeyValuePair<string, IVariable> kvp in originalDatabank.storage)
            {
                IVariable ivCopy = kvp.Value.DeepClone(0, null, cloneHelper);  //uses that CloneHelper deeper down, which is ok
                newDatabank.AddIVariable(kvp.Key, ivCopy);
            }
        }

        /// <summary>
        /// True for a path like lib1.zip\data\x.csv, also works for full paths.
        /// </summary>
        /// <param name="pathAndFilename"></param>
        /// <returns></returns>
        public static bool ContainsZipPath(string pathAndFilename)
        {
            return pathAndFilename.ToLower().Contains(Globals.zip + "\\");
        }

        /// <summary>
        /// With "xx(aa,'xx(aa)',AA)" and "aa" and "zz", it will become "xx(zz,'xx(aa)',zz)", not touching the inside.
        /// The method is case-insensitive
        /// </summary>
        /// <param name="input"></param>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string ReplaceIgnoreCaseIgnoreQuoted(string input, string s1, string s2)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(s1))
                return input;

            // Pattern: Matches anything in '...' (Group: ignore) OR the target s1
            // Regex.Escape(s1) ensures the search string is treated as literal text
            string pattern = $@"(?<ignore>'[^']*')|{Regex.Escape(s1)}";

            return Regex.Replace(input, pattern, m =>
            {
                // If the match was caught by the 'ignore' group, return it exactly as-is
                if (m.Groups["ignore"].Success)
                {
                    return m.Value;
                }

                // Otherwise, it's a case-insensitive match for s1; replace with s2
                return s2;
            }, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Only replace first occurrence
        /// </summary>
        /// <param name="original"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceFirstOccurrence(string original, string oldValue, string newValue)
        {
            if (String.IsNullOrEmpty(original))
                return String.Empty;
            if (String.IsNullOrEmpty(oldValue))
                return original;
            if (String.IsNullOrEmpty(newValue))
                newValue = String.Empty;
            int loc = original.IndexOf(oldValue);
            if (loc == -1)
                return original;
            return original.Remove(loc, oldValue.Length).Insert(loc, newValue);
        }

        /// <summary>
        /// Helper function: fills an array with specified value
        /// </summary>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int[] CreateArray(int size, int value)
        {
            int[] temp = new int[size];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = value;
            }
            return temp;
        }

        /// <summary>
        /// Helper function: fills an array with specified value
        /// </summary>
        /// <param name="size"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double[] CreateArrayDouble(int size, double value)
        {
            double[] temp = new double[size];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = value;
            }
            return temp;
        }

        /// <summary>
        /// Create a double[,] array with a specific value
        /// </summary>
        /// <param name="size1"></param>
        /// <param name="size2"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double[,] CreateArrayDouble(int size1, int size2, double value)
        {
            double[,] temp = new double[size1, size2];
            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    temp[i, j] = value;
                }
            }
            return temp;
        }

        /// <summary>
        /// Near obsolete method.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="isVarName"></param>
        /// <param name="isInverse"></param>
        /// <returns></returns>
        public static string PrettifyTimeseriesHash(string s, bool isVarName, bool isInverse)
        {
            //This is most probably obsolete now: it transforms x___a into x[a]

            if (s == null) return null;
            if (!isVarName && isInverse) throw new GekkoException();
            if (isInverse)
            {
                string ss = s.Replace(Globals.leftParenthesisIndicator, Globals.symbolTurtle);
                ss = ss.Replace(Globals.rightParenthesisIndicator, "");
                ss = ss.Replace(",", Globals.symbolTurtle);
                ss = ss.Replace("'", "");
                ss = ss.Replace(" ", "");
                ss = ss.Trim();
                return ss;
            }
            else
            {
                if (isVarName)
                {
                    int i = s.IndexOf(Globals.symbolTurtle);
                    if (i <= 0) return s;
                    string s1 = s.Substring(0, i);
                    string s2 = s.Substring(i + Globals.symbolTurtle.Length, s.Length - (i + Globals.symbolTurtle.Length));
                    return s1 + "[" + PrettifyTimeseriesHash(s2, false, false) + "]";
                }
                else return "'" + s.Replace(Globals.symbolTurtle, "', '") + "'";
            }
        }

        /// <summary>
        /// Counts occurrences of string 'inside' inside 's'. Case-insensitive.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="inside"></param>
        /// <returns></returns>
        public static int Count(string s, string inside)
        {
            if (G.NullOrBlanks(s) == null) return 0;
            if (G.NullOrBlanks(inside) == null) return 0;
            int n = s.ToLower().Split(new string[] { inside.ToLower() }, StringSplitOptions.None).Length - 1;
            return n;
        }

        /// <summary>
        /// Helper method for natural file listing sorting (a8, a9, a10, a11 instead of a10, a11, a8, a9)
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <returns></returns>
        public static int CompareNaturalIgnoreCase(string strA, string strB)
        {
            return CompareNatural(strA, strB, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
        }
        /// <summary>
        /// Helper method for natural file listing sorting (a8, a9, a10, a11 instead of a10, a11, a8, a9)
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <param name="culture"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static int CompareNatural(string strA, string strB, CultureInfo culture, CompareOptions options)
        {
            CompareInfo cmp = culture.CompareInfo;
            int iA = 0;
            int iB = 0;
            int softResult = 0;
            int softResultWeight = 0;
            while (iA < strA.Length && iB < strB.Length)
            {
                bool isDigitA = Char.IsDigit(strA[iA]);
                bool isDigitB = Char.IsDigit(strB[iB]);
                if (isDigitA != isDigitB)
                {
                    return cmp.Compare(strA, iA, strB, iB, options);
                }
                else if (!isDigitA && !isDigitB)
                {
                    int jA = iA + 1;
                    int jB = iB + 1;
                    while (jA < strA.Length && !Char.IsDigit(strA[jA])) jA++;
                    while (jB < strB.Length && !Char.IsDigit(strB[jB])) jB++;
                    int cmpResult = cmp.Compare(strA, iA, jA - iA, strB, iB, jB - iB, options);
                    if (cmpResult != 0)
                    {
                        // Certain strings may be considered different due to "soft" differences that are
                        // ignored if more significant differences follow, e.g. a hyphen only affects the
                        // comparison if no other differences follow
                        string sectionA = strA.Substring(iA, jA - iA);
                        string sectionB = strB.Substring(iB, jB - iB);
                        if (cmp.Compare(sectionA + "1", sectionB + "2", options) ==
                            cmp.Compare(sectionA + "2", sectionB + "1", options))
                        {
                            return cmp.Compare(strA, iA, strB, iB, options);
                        }
                        else if (softResultWeight < 1)
                        {
                            softResult = cmpResult;
                            softResultWeight = 1;
                        }
                    }
                    iA = jA;
                    iB = jB;
                }
                else
                {
                    char zeroA = (char)(strA[iA] - (int)Char.GetNumericValue(strA[iA]));
                    char zeroB = (char)(strB[iB] - (int)Char.GetNumericValue(strB[iB]));
                    int jA = iA;
                    int jB = iB;
                    while (jA < strA.Length && strA[jA] == zeroA) jA++;
                    while (jB < strB.Length && strB[jB] == zeroB) jB++;
                    int resultIfSameLength = 0;
                    do
                    {
                        isDigitA = jA < strA.Length && Char.IsDigit(strA[jA]);
                        isDigitB = jB < strB.Length && Char.IsDigit(strB[jB]);
                        int numA = isDigitA ? (int)Char.GetNumericValue(strA[jA]) : 0;
                        int numB = isDigitB ? (int)Char.GetNumericValue(strB[jB]) : 0;
                        if (isDigitA && (char)(strA[jA] - numA) != zeroA) isDigitA = false;
                        if (isDigitB && (char)(strB[jB] - numB) != zeroB) isDigitB = false;
                        if (isDigitA && isDigitB)
                        {
                            if (numA != numB && resultIfSameLength == 0)
                            {
                                resultIfSameLength = numA < numB ? -1 : 1;
                            }
                            jA++;
                            jB++;
                        }
                    }
                    while (isDigitA && isDigitB);
                    if (isDigitA != isDigitB)
                    {
                        // One number has more digits than the other (ignoring leading zeros) - the longer
                        // number must be larger
                        return isDigitA ? 1 : -1;
                    }
                    else if (resultIfSameLength != 0)
                    {
                        // Both numbers are the same length (ignoring leading zeros) and at least one of
                        // the digits differed - the first difference determines the result
                        return resultIfSameLength;
                    }
                    int lA = jA - iA;
                    int lB = jB - iB;
                    if (lA != lB)
                    {
                        // Both numbers are equivalent but one has more leading zeros
                        return lA > lB ? -1 : 1;
                    }
                    else if (zeroA != zeroB && softResultWeight < 2)
                    {
                        softResult = cmp.Compare(strA, iA, 1, strB, iB, 1, options);
                        softResultWeight = 2;
                    }
                    iA = jA;
                    iB = jB;
                }
            }
            if (iA < strA.Length || iB < strB.Length)
            {
                return iA < strA.Length ? 1 : -1;
            }
            else if (softResult != 0)
            {
                return softResult;
            }
            return 0;
        }

        public static int RoundUpToNearest32(int value)
        {
            int d = (value + 31) & ~31;
            return d;
        }

        /// <summary>
        /// Helper class for natural file listing sorting 
        /// (a8, a9, a10, a11 instead of a10, a11, a8, a9). Will ignore case, and
        /// considers '_' a character. Does some internal tokenizing. Numbers can start with - or +.
        /// </summary>
        public class NaturalComparer : IComparer<string>, IComparer
        {
            private StringParser mParser1;
            private StringParser mParser2;
            private NaturalComparerOptions mNaturalComparerOptions;

            private enum TokenType
            {
                Nothing,
                Numerical,
                String
            }

            private class StringParser
            {
                private TokenType mTokenType;
                private string mStringValue;
                private decimal mNumericalValue;
                private int mIdx;
                private string mSource;
                private int mLen;
                private char mCurChar;
                private NaturalComparer mNaturalComparer;

                public StringParser(NaturalComparer naturalComparer)
                {
                    mNaturalComparer = naturalComparer;
                }

                public void Init(string source)
                {
                    if (source == null)
                        source = string.Empty;
                    mSource = source;
                    mLen = source.Length;
                    mIdx = -1;
                    mNumericalValue = 0;
                    NextChar();
                    NextToken();
                }

                public TokenType TokenType
                {
                    get { return mTokenType; }
                }

                public decimal NumericalValue
                {
                    get
                    {
                        if (mTokenType == NaturalComparer.TokenType.Numerical)
                        {
                            return mNumericalValue;
                        }
                        else
                        {
                            throw new NaturalComparerException("Internal Error: NumericalValue called on a non numerical value.");
                        }
                    }
                }

                public string StringValue
                {
                    get { return mStringValue; }
                }

                public void NextToken()
                {
                    do
                    {
                        //CharUnicodeInfo.GetUnicodeCategory 
                        if (mCurChar == '\0')
                        {
                            mTokenType = NaturalComparer.TokenType.Nothing;
                            mStringValue = null;
                            return;
                        }
                        else if (char.IsDigit(mCurChar) || mCurChar == '-' || mCurChar == '+')
                        {
                            ParseNumericalValue();
                            return;
                        }
                        else if (G.IsLetterOrUnderscore(mCurChar))
                        {
                            ParseString();
                            return;
                        }
                        else
                        {
                            //ignore this character and loop some more 
                            NextChar();
                        }
                    }
                    while (true);
                }

                private void NextChar()
                {
                    mIdx += 1;
                    if (mIdx >= mLen)
                    {
                        mCurChar = '\0';
                    }
                    else
                    {
                        mCurChar = mSource[mIdx];
                    }
                }

                private void ParseNumericalValue()
                {
                    int start = mIdx;
                    char NumberDecimalSeparator = '.';
                    do
                    {
                        NextChar();
                        if (mCurChar == NumberDecimalSeparator)
                        {
                            // parse digits after the Decimal Separator 
                            do
                            {
                                NextChar();
                                if (!char.IsDigit(mCurChar))
                                    break;
                            }
                            while (true);
                            break;
                        }
                        else
                        {
                            if (!char.IsDigit(mCurChar))
                                break;
                        }
                    }
                    while (true);
                    mStringValue = mSource.Substring(start, mIdx - start);
                    if (decimal.TryParse(mStringValue, out mNumericalValue))
                    {
                        mTokenType = NaturalComparer.TokenType.Numerical;
                    }
                    else
                    {
                        // We probably have a too long value 
                        mTokenType = NaturalComparer.TokenType.String;
                    }
                }

                private void ParseString()
                {
                    int start = mIdx;
                    bool roman = (mNaturalComparer.mNaturalComparerOptions & NaturalComparerOptions.RomanNumbers) != 0;
                    int romanValue = 0;
                    int lastRoman = int.MaxValue;
                    int cptLastRoman = 0;
                    do
                    {
                        NextChar();
                        if (!G.IsLetterOrUnderscore(mCurChar)) break;
                    }
                    while (true);
                    mStringValue = mSource.Substring(start, mIdx - start);
                    if (roman)
                    {
                        mNumericalValue = romanValue;
                        mTokenType = NaturalComparer.TokenType.Numerical;
                    }
                    else
                    {
                        mTokenType = NaturalComparer.TokenType.String;
                    }
                }

            }

            public NaturalComparer(NaturalComparerOptions NaturalComparerOptions)
            {
                mNaturalComparerOptions = NaturalComparerOptions;
                mParser1 = new StringParser(this);
                mParser2 = new StringParser(this);
            }

            public NaturalComparer()
               : this(NaturalComparerOptions.Default)
            {
            }

            int System.Collections.Generic.IComparer<string>.Compare(string string1, string string2)
            {
                mParser1.Init(string1);
                mParser2.Init(string2);
                int result;
                do
                {
                    if (mParser1.TokenType == TokenType.Numerical & mParser2.TokenType == TokenType.Numerical)
                    {
                        // both string1 and string2 are numerical 
                        result = decimal.Compare(mParser1.NumericalValue, mParser2.NumericalValue);
                    }
                    else
                    {
                        result = string.Compare(mParser1.StringValue, mParser2.StringValue, true);
                    }
                    if (result != 0)
                    {
                        return result;
                    }
                    else
                    {
                        mParser1.NextToken();
                        mParser2.NextToken();
                    }
                }
                while (!(mParser1.TokenType == TokenType.Nothing & mParser2.TokenType == TokenType.Nothing));
                //identical 
                return 0;
            }

            public int RomanValue(string string1)
            {
                mParser1.Init(string1);

                if (mParser1.TokenType == TokenType.Numerical)
                {
                    return (int)mParser1.NumericalValue;
                }
                else
                {
                    return 0;
                }
            }

            int IComparer.Compare(object x, object y)
            {
                return ((System.Collections.Generic.IComparer<string>)this).Compare((string)x, (string)y);
            }
        }

        /// <summary>
        /// Helper class for natural file listing sorting (a8, a9, a10, a11 instead of a10, a11, a8, a9)
        /// </summary>
        public class NaturalComparerException : System.Exception
        {

            public NaturalComparerException(string msg)
               : base(msg)
            {
            }
        }

        /// <summary>
        /// Helper class for natural file listing sorting (a8, a9, a10, a11 instead of a10, a11, a8, a9)
        /// </summary>
        [System.Flags()]
        public enum NaturalComparerOptions
        {
            None,
            RomanNumbers,
            //DecimalValues <- we could put this as an option 
            //IgnoreSpaces <- we could put this as an option 
            //IgnorePunctuation <- we could put this as an option 
            Default = None
        }

        /// <summary>
        /// Helper class for natural file listing sorting (a8, a9, a10, a11 instead of a10, a11, a8, a9)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class CustomComparer<T> : IComparer<T>
        {
            private Comparison<T> _comparison;

            public CustomComparer(Comparison<T> comparison)
            {
                _comparison = comparison;
            }

            public int Compare(T x, T y)
            {
                return _comparison(x, y);
            }
        }


        /// <summary>
        /// Helper method for file access (writing). Writes in ANSI (code 1252), so the for instance æøå look nicer in old text editors like Kedit.
        /// </summary>
        /// <param name="fs2"></param>
        /// <returns></returns>
        public static StreamWriter GekkoStreamWriter(FileStream fs2, Encoding encoding)
        {
            return new StreamWriter(fs2, encoding);
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="fs2"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static StreamWriter GekkoStreamWriter(FileStream fs2)
        {
            return GekkoStreamWriter(fs2, G.GetEncoding());
        }

        //To make sure it is utf-8 with bom, best when writing html for instance
        public static StreamWriter GekkoStreamWriterUtf8Bom(FileStream fs2)
        {
            return GekkoStreamWriter(fs2, new UTF8Encoding(true));
        }

        public static StreamWriter GekkoStreamWriterUtf8(FileStream fs2)
        {
            return GekkoStreamWriter(fs2, new UTF8Encoding(false));
        }

        /// <summary>
        /// Get ANSI or UTF8 encoding depending upon 'option system character encoding'.
        /// </summary>
        /// <returns></returns>
        public static Encoding GetEncoding()
        {
            Encoding encoding = null;
            if (G.Equal(Program.options.system_write_encoding, "ansi"))
            {
                //For emitting html for MAKRO, this seems to be the only one that gives ok æ ø å,
                //just not for the dynamic JavaScript.
                encoding = Encoding.GetEncoding("Windows-1252");
            }
            else if (G.Equal(Program.options.system_write_encoding, "utf8"))
            {
                if (Program.options.system_write_utf8_bom) encoding = new UTF8Encoding(true);
                else encoding = new UTF8Encoding(false);
            }
            else throw new GekkoException();
            return encoding;
        }

        /// <summary>
        /// Get all IndexOf() from a string, cf. https://stackoverflow.com/questions/15993357/how-to-get-all-indexof-instances-of-string-in-another-string-c-sharp. Can be empty, but will never contain an element = -1.
        /// See also G.Match() and G.IsDelimited().
        /// </summary>        
        /// <param name="input"></param>
        /// <param name="substring"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static List<int> AllIndexOf(string input, string substring, StringComparison comparisonType)
        {
            List<int> allIndexOf = new List<int>();
            int index = input.IndexOf(substring, comparisonType);
            while (index != -1)
            {
                allIndexOf.Add(index);
                index = input.IndexOf(substring, index + 1, comparisonType);
            }
            return allIndexOf;
        }

        public static int FirstNonBlankIndexOf(string text, int start)
        {
            if (string.IsNullOrEmpty(text))
            {
                return -1; // Or throw an exception, depending on your needs
            }

            // Iterate through the string character by character
            for (int i = start; i < text.Length; i++)
            {
                // checks for spaces, tabs, newlines, etc.
                if (!char.IsWhiteSpace(text[i]))
                {
                    return i; // Found the index of the first non-blank character
                }
            }

            return -1; // No non-blank character found (string is all whitespace)
        }

        public static int FirstBlankIndexOf(string text, int start)
        {
            if (string.IsNullOrEmpty(text))
            {
                return -1; // Or throw an exception, depending on your needs
            }

            // Iterate through the string character by character
            for (int i = start; i < text.Length; i++)
            {
                // checks for spaces, tabs, newlines, etc.
                if (char.IsWhiteSpace(text[i]))
                {
                    return i; // Found the index of the first non-blank character
                }
            }

            return -1; // No non-blank character found (string is all whitespace)
        }

        /// <summary>
        /// Normal letters + digigs + _
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetterOrDigitOrUnderscore(char c)
        {
            if (G.IsEnglishLetter(c) || char.IsDigit(c) || c == '_')
                return true;
            else return false;
        }

        /// <summary>   
        /// English letters + digits, not underscore.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetterOrDigit(char c)
        {
            if (G.IsEnglishLetter(c) || char.IsDigit(c))
                return true;
            else return false;
        }

        /// <summary>
        /// letters, digits, _ or !
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetterOrDigitOrUnderscoreOrExclamation(char c)
        {
            if (G.IsEnglishLetter(c) || char.IsDigit(c) || c == '_' || c == Globals.freqIndicator)
                return true;
            else return false;
        }

        /// <summary>
        /// Are two double numbers equal? Handles missings Gekko-correctly.
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool Equals(double d1, double d2)
        {
            // ---------------------------------------------
            // #890345340857                        
            //
            // In both Python and C#, if one or 
            // both of x and y are NaN, the following is the case:                    
            // x == y --> false
            // x != y --> true
            // x <op> x --> false, for op = <, <=, >=, >                  
            // So only != is true if any operand is NaN (Python: math.nan)                                                         
            //
            // Gekko has same behavior regarding op = <, <=, >=, > 
            // But in Gekko we have:
            //
            // m() == m() --> 1
            // m() == 2 --> 0
            // 2 == m() --> 0
            // m() <> m() --> 0
            // m() <> 2 --> 1
            // 2 <> m() --> 1
            //
            // So in Gekko, if there are missings, op <, <=, >=, > are just kind of broke
            // but with missings, == and <> can be used.
            //
            //                        d2
            //               |   NaN    normal
            //-------------------------------------
            //   d1  NaN     |   true   false
            //      normal   |   false    ?
            //-------------------------------------
            //
            //bool d1 = G.Equals(1d, 1d);                    --> true
            //bool d2 = G.Equals(double.NaN, 1d);            --> false
            //bool d3 = G.Equals(1d, double.NaN);            --> false
            //bool d4 = G.Equals(double.NaN, double.NaN);    --> true

            if (G.IsBothNumericalError(d1, d2)) return true;  //see also #87342543534
            if (d1 == d2) return true;  //can only be true if neither is NaN
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static bool IsBothNumericalError(double d1, double d2)
        {
            return G.IsNumericalError(d1) && G.IsNumericalError(d2);
        }

        /// <summary>
        /// True if string starts with % or #.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool StartsWithSigil(string s)
        {
            if (s == null) return false;
            if (s.Length == 0) return false;
            if (s[0] == Globals.symbolScalar || s[0] == Globals.symbolCollection)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Helper method to check validity fo varname
        /// </summary>
        /// <param name="x"></param>
        /// <param name="sigilType"></param>
        //Use together with CheckIVariableName()
        //See also G.AddSigil()
        public static void CheckIVariableNameAndType(IVariable x, G.ESigilType sigilType)
        {
            //Fortunately these checks are only when putting things in, and injecting will avoid it
            if (sigilType == G.ESigilType.Scalar)
            {
                if (x.Type() == EVariableType.Val || x.Type() == EVariableType.String || x.Type() == EVariableType.Date)
                {
                    //good
                }
            }
            else if (sigilType == G.ESigilType.Collection)
            {
                if (x.Type() == EVariableType.List || x.Type() == EVariableType.Matrix || x.Type() == EVariableType.Map)
                {
                    //good
                }
            }
            else if (sigilType == G.ESigilType.Frequency)
            {
                if (x.Type() == EVariableType.Series)
                {
                    //good
                }
            }
            else
            {
                //bad, also including a series with name 'x'
                new Error("Variable name and type do not conform");
            }
        }

        /// <summary>
        /// Check validity of varname
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        //Use together with CheckIVariableNameAndType()
        //See also G.AddSigil()
        public static ESigilType CheckIVariableName(string name)
        {
            //Fortunately these checks are only when putting things in, and injecting will avoid it
            bool hasFreqIndicator = false;
            int hasSigil = 0;
            ESigilType rv = ESigilType.None;

            if (name == null || name.Length == 0)
            {
                new Error("Name has zero length");
            }

            if (name[0] == Globals.symbolScalar)
            {
                rv = ESigilType.Scalar;
                hasSigil = 1;
            }
            else if (name[0] == Globals.symbolCollection)
            {
                rv = ESigilType.Collection;
                hasSigil = 1;
            }

            if (hasSigil == 1 && name.Length == 1)
            {
                new Error("Name is naked % or #");
            }

            for (int i = hasSigil; i < name.Length; i++)
            {
                char c = name[i];
                //The good thing is that this is only checked when putting stuff INTO the databank, and not
                //when retrieving from the databank. A ScalarVal will for instance just have its contents replaced,
                //if inside a loop.

                if (i > hasSigil && G.IsLetterOrDigitOrUnderscore(c))
                {
                    //good
                }
                else if (i == hasSigil && G.IsLetterOrUnderscore(c))
                {
                    //good, will not allow %117industries or #117industries, probably best to disallow this
                }
                else if (i < name.Length - 1 && c == Globals.freqIndicator)
                {
                    //good
                    hasFreqIndicator = true;
                    if (hasSigil == 1)
                    {
                        new Error("Cannot combine '%', '#' and '!'");
                    }
                }
                else
                {
                    if (c == '[')
                    {
                        break; //We accept x!q[... where something follows [.
                    }
                    new Error("Malformed name: '" + name + "'");
                }
            }

            if (hasSigil == 0)
            {
                if (hasFreqIndicator) rv = ESigilType.Frequency;
            }

            return rv;
        }

        /// <summary>
        /// Converts from string to EVariableType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static EVariableType GetVariableType(string type)
        {
            type = type.Trim();
            EVariableType etype = EVariableType.Var;
            if (G.Equal(type, "val")) etype = EVariableType.Val;
            else if (G.Equal(type, "string")) etype = EVariableType.String;
            else if (G.Equal(type, "date")) etype = EVariableType.Date;
            else if (G.Equal(type, "series")) etype = EVariableType.Series;
            else if (G.Equal(type, "ser")) etype = EVariableType.Series;
            else if (G.Equal(type, "list")) etype = EVariableType.List;
            else if (G.Equal(type, "matrix")) etype = EVariableType.Matrix;
            else if (G.Equal(type, "map")) etype = EVariableType.Map;
            else if (G.Equal(type, "var")) etype = EVariableType.Var;
            else if (G.Equal(type, "name")) etype = EVariableType.Name;
            else if (type == null || type == "") etype = EVariableType.Var;
            else
            {
                new Error("Could not recognize variable type '" + type + "'");
                //throw new GekkoException();
            }
            return etype;
        }

        /// <summary>
        /// True if a string is null or "" or " " or "   ", etc.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool NullOrBlanks(string x)
        {
            return !(x != null && x.Trim() != "");
        }

        /// <summary>
        /// True if a string is null or "".
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool NullOrEmpty(string x)
        {
            return !(x != null && x != "");
        }

        /// <summary>
        /// True if a..z or A..Z or _.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsLetterOrUnderscore(char c)
        {
            if (G.IsEnglishLetter(c) || c == '_')
                return true;
            else return false;
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HandleQuoteInQuote(string s)
        {
            return HandleQuoteInQuote(s, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="s"></param>
        /// <param name="special"></param>
        /// <returns></returns>
        public static string HandleQuoteInQuote(string s, bool special)
        {
            if (special) s = s.Replace("\"", "\\\"");  //inside js in html
            else s = s.Replace("\"", "\"\"");
            return s;
        }

        public static string HandleQuoteInQuote2(string s)
        {
            if (s == null) return null;
            //return s.Replace("\"", "\\\"").Replace("\\", "\\\\");            
            return s.Replace("\"", "'").Replace("\\", "-");
        }

        /// <summary>
        /// Helper method.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateHelper3(string format, DateTime dt)
        {
            string s = dt.ToString(format.ToLower().Replace("m", "M"));
            return s;
        }

        /// <summary>
        /// Min function. Special handling of -12345 input.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static int GekkoMin(int i1, int i2) {
            //if both are missing, a missing is returned
            if (i1 == -12345) return i2;
            if (i2 == -12345) return i1;
            return Math.Min(i1, i2);
        }

        /// <summary>
        /// Max function. Special handling of -12345 input.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static int GekkoMax(int i1, int i2)
        {
            //if both are missing, a missing is returned
            //with positive inputs, this method is superflous, but we keep it for symmtery reasons (see GekkoMin())
            if (i1 == -12345) return i2;
            if (i2 == -12345) return i1;
            return Math.Max(i1, i2);
        }

        /// <summary>
        /// Max function for GekkoTime.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static GekkoTime GekkoMax(GekkoTime t1, GekkoTime t2)
        {
            if (t1.IsNull() || t2.IsNull()) new Error("On or more Gekko periods are null");
            if (t1.freq != t2.freq) new Error("Mismatch of frequencies");
            GekkoTime t = t1;
            if (t2.StrictlyLargerThan(t1)) t = t2;
            return t;
        }

        /// <summary>
        /// Min function for GekkoTime.
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static GekkoTime GekkoMin(GekkoTime t1, GekkoTime t2)
        {
            if (t1.IsNull() || t2.IsNull()) new Error("On or more Gekko periods are null");
            if (t1.freq != t2.freq) new Error("Mismatch of frequencies");
            GekkoTime t = t1;
            if (t2.StrictlySmallerThan(t1)) t = t2;
            return t;
        }

        /// <summary>
        /// BEWARE: can return null (= error)!! Another "interface" to the substring method, with start end end position, instead of using length. 
        /// Indexes are 0-based. The positions are inclusive.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <returns></returns>
        public static string Substring(string s, int position1, int position2)
        {
            string x = null;
            try
            {
                x = s.Substring(position1, position2 - position1 + 1);
            }
            catch (Exception e) { };
            if (x == "") x = null;  //happens with Substring(s, 5, 4) for instance --> length 0.
            return x;
        }

        /// <summary>
        /// In a string, skip to next non-space (tabs counted as spaces)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ii"></param>
        /// <returns></returns>
        public static int SkipSpaces(string s, int ii)
        {
            if (ii < 0) return -12345;  //should not happen, but just in case...                        
            //skip spaces (tab is included counted)
            for (int i = ii; i < s.Length; i++)
            {
                if (s[i] == ' ' || s[i] == '\t')     //'\t' is tab
                {
                    //do nothing
                }
                else return i;
            }
            return -12345;
        }


        /// <summary>
        /// Overload.
        /// </summary>
        public static void SetWorkingFolder()
        {
            SetWorkingFolder(true);
        }

        /// <summary>
        /// Try to switch to working folder.
        /// </summary>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public static bool SetWorkingFolder(bool throwException)
        {
            try
            {
                System.IO.Directory.SetCurrentDirectory(Program.options.folder_working);
            }
            catch (Exception e)
            {
                new Error("It seems the folder '" + Program.options.folder_working + "' is blocked or does not exist", false);
                if (throwException) throw new GekkoException();
                else return true;  //problem
            }
            return false;
        }

        /// <summary>
        /// Get location of gekko.exe
        /// </summary>
        /// <returns></returns>
        public static string GetProgramDir()
        {
            return System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        }

        /// <summary>
        /// Is dir2 a subfolder of dir1? Note: the two dirs must not end with "\".
        /// </summary>
        /// <param name="dir1"></param>
        /// <param name="dir2"></param>
        /// <returns></returns>
        public static bool IsSubFolder(string dir1, string dir2)
        {
            DirectoryInfo di1 = new DirectoryInfo(dir1);
            DirectoryInfo di2 = new DirectoryInfo(dir2);
            bool isParent = false;
            while (di2.Parent != null)
            {
                if (di2.Parent.FullName == di1.FullName)
                {
                    isParent = true;
                    break;
                }
                else di2 = di2.Parent;
            }

            return isParent;
        }

        /// <summary>
        /// Get locatino of working folder
        /// </summary>
        /// <returns></returns>
        public static string GetWorkingFolder()
        {
            return System.IO.Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Get Gekko version as string
        /// </summary>
        public static string PrintVersion(string version, bool patch)
        {
            string start = "";
            string middle = "";
            string end = "";
            try
            {
                string[] versionSplit = version.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (versionSplit.Length == 3)
                {
                    start = versionSplit[0];
                    middle = versionSplit[1];
                    end = versionSplit[2];
                    int number0 = int.Parse(start);
                    int number1 = int.Parse(middle);
                    int number2 = int.Parse(end);
                    if (!(number0 == 1 && number1 == 0) && number1 % 2 == 0)  //dont do this for 1.0.xx versions
                    {
                        //1.4.9 stuff
                        version = start + "." + middle;  //don't use the last one, which is a patch
                        if (patch)
                        {
                            if (number2 != 0) version = version + " (patch #" + end + ")";  //if it is 1.2.17 print the patch #
                        }
                    }
                }
            }
            catch
            {
                version = Globals.gekkoVersion;
            };  //fail silently                        

            return version;
        }

        /// <summary>
        /// Tastes a file to see if it is (likely) binary, cf. https://stackoverflow.com/questions/4744890/c-sharp-check-if-file-is-text-based
        /// Practically uses no time.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="requiredConsecutiveNul"></param>
        /// <returns></returns>
        public static bool IsBinary(string filePath, int requiredConsecutiveNul = 1)
        {
            const int charsToCheck = 8000;
            const char nulChar = '\0';

            int nulCount = 0;

            using (var streamReader = new StreamReader(filePath))
            {
                for (var i = 0; i < charsToCheck; i++)
                {
                    if (streamReader.EndOfStream)
                        return false;

                    if ((char)streamReader.Read() == nulChar)
                    {
                        nulCount++;

                        if (nulCount >= requiredConsecutiveNul)
                            return true;
                    }
                    else
                    {
                        nulCount = 0;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another 
        /// specified string according the type of search to use for the specified string. Set checkBoundary if you do not want
        /// "bc" to match inside "abcd", but match inside "+bc+" (for instance) --> good for replacing variable names, also names
        /// like x[i,j]. Set maxtimes2=0 to indicate infinite.
        /// </summary>
        /// <param name="str">The string performing the replace method.</param>
        /// <param name="oldValue">The string to be replaced.</param>
        /// <param name="newValue">The string replace all occurrences of <paramref name="oldValue"/>. 
        /// If value is equal to <c>null</c>, than all occurrences of <paramref name="oldValue"/> will be removed from the <paramref name="str"/>.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the search.</param>
        /// <returns>A string that is equivalent to the current string except that all instances of <paramref name="oldValue"/> are replaced with <paramref name="newValue"/>. 
        /// If <paramref name="oldValue"/> is not found in the current instance, the method returns the current instance unchanged.</returns>        
        public static string Replace(string str, string oldValue, string @newValue, StringComparison comparisonType, int maxtimes2)
        {
            int maxtimes = maxtimes2;
            if (maxtimes2 <= 0) maxtimes = int.MaxValue;

            // Check inputs.
            if (str == null)
            {
                // Same as original .NET C# string.Replace behavior.
                throw new ArgumentNullException(nameof(str));
            }
            if (str.Length == 0)
            {
                // Same as original .NET C# string.Replace behavior.
                return str;
            }
            if (oldValue == null)
            {
                // Same as original .NET C# string.Replace behavior.
                throw new ArgumentNullException(nameof(oldValue));
            }
            if (oldValue.Length == 0)
            {
                // Same as original .NET C# string.Replace behavior.
                G.Writeln2("String cannot be of zero length.");
                throw new GekkoException();
            }

            // Prepare string builder for storing the processed string.
            // Note: StringBuilder has a better performance than String by 30-40%.
            StringBuilder resultStringBuilder = new StringBuilder(str.Length);

            // Analyze the replacement: replace or remove.
            bool isReplacementNullOrEmpty = string.IsNullOrEmpty(@newValue);

            // Replace all values.
            const int valueNotFound = -1;
            int foundAt;
            int startSearchFromIndex = 0;

            int counter = 0;

            while ((foundAt = str.IndexOf(oldValue, startSearchFromIndex, comparisonType)) != valueNotFound)
            {
                // Append all characters until the found replacement.
                int @charsUntilReplacment = foundAt - startSearchFromIndex;
                bool isNothingToAppend = @charsUntilReplacment == 0;
                if (!isNothingToAppend)
                {
                    resultStringBuilder.Append(str, startSearchFromIndex, @charsUntilReplacment);
                }

                // Process the replacement.
                if (!isReplacementNullOrEmpty)
                {
                    resultStringBuilder.Append(@newValue);
                }

                // Prepare start index for the next search.
                // This needed to prevent infinite loop, otherwise method always start search 
                // from the start of the string. For example: if an oldValue == "EXAMPLE", newValue == "example"
                // and comparisonType == "any ignore case" will conquer to replacing:
                // "EXAMPLE" to "example" to "example" to "example" … infinite loop.
                startSearchFromIndex = foundAt + oldValue.Length;
                if (startSearchFromIndex == str.Length)
                {
                    // It is end of the input string: no more space for the next search.
                    // The input string ends with a value that has already been replaced. 
                    // Therefore, the string builder with the result is complete and no further action is required.
                    return resultStringBuilder.ToString();
                }
                counter++;  //1, 2, 3, ...

                if (counter != 0 && counter >= maxtimes) break;
            }

            // Append the last part to the result.
            int @charsUntilStringEnd = str.Length - startSearchFromIndex;
            resultStringBuilder.Append(str, startSearchFromIndex, @charsUntilStringEnd);

            return resultStringBuilder.ToString();
        }

        /// <summary>
        /// Helper method regarding the #(listfile x) syntax for lists
        /// </summary>
        /// <param name="varnameWithFreq"></param>
        /// <returns></returns>
        public static string TransformListfileName(string varnameWithFreq)
        {
            if (!varnameWithFreq.Contains("___")) return varnameWithFreq;
            string fileName2 = varnameWithFreq.Substring((Globals.symbolCollection + Globals.listfile + "___").Length);
            string listfileName = Globals.symbolCollection + "(" + "listfile" + " " + fileName2 + ")";
            return listfileName;
        }

        /// <summary>
        /// Count lines in a string. With precise=true, the line count is more precise but slower. Difference
        /// between precise or not is normally within one line.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int CountLines(string s, bool precise)
        {
            //Just counting lines, fast, no splitting etc.
            if (s == null) return 0;
            if (s == string.Empty) return 0;

            if (precise)
            {
                var ss2 = Stringlist.ExtractLinesFromText(s);
                return ss2.Count;
            } else
            {
                //!!The below is imprecise sometimes
                int index = -1;
                int count = 0;
                while (-1 != (index = s.IndexOf(G.NL2, index + 1)))
                    count++;
                return count + 1;
            }
        }

        /// <summary>
        /// Helper method for splitting csv lines
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        //This: 111,222,"33,44,55",666,"77,88","99"
        //returns this: 
        //  111  
        //  222  
        //  33,44,55  
        //  666  
        //  77,88  
        //  99  
        //Note: the elements may need a .Trim() afterwards to remove superfluous blanks!
        public static List<string> SplitCsv(string line)
        {
            List<string> result = new List<string>();
            if (line == null) return result;
            StringBuilder currentStr = new StringBuilder("");
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++) // For each character
            {
                if (line[i] == '\"') // Quotes are closing or opening
                    inQuotes = !inQuotes;
                else if (line[i] == ',') // Comma
                {
                    if (!inQuotes) // If not in quotes, end of current string, add it to result
                    {
                        result.Add(currentStr.ToString());
                        currentStr.Clear();
                    }
                    else
                        currentStr.Append(line[i]); // If in quotes, just add it 
                }
                else // Add any other character to current string
                    currentStr.Append(line[i]);
            }
            result.Add(currentStr.ToString());
            return result;
        }

        /// <summary>
        /// Method for printing out "service messages" in the Gekko GUI,
        /// for instance when assigning z = x + y;
        /// </summary>
        /// <param name="s"></param>
        /// <param name="p"></param>
        public static void ServiceMessage(string s, P p)
        {
            if (p == null)
            {
                G.Write2(s);
                G.Writeln(" " + Globals.serviceMessage, Color.LightGray);
            }
            else
            {
                if (p.IsSimple())
                {
                    if (p.numberOfServiceMessages < 4)
                    {
                        G.Write2(s);
                        G.Writeln(" " + Globals.serviceMessage, Color.LightGray);
                        p.numberOfServiceMessages++;
                    }
                    else if (p.numberOfServiceMessages == 4)
                    {
                        G.Write2(s);
                        G.Writeln(Globals.serviceMessageTruncated, Color.LightGray);
                        p.numberOfServiceMessages++;
                    }
                    else
                    {
                        //do nothing
                    }
                }
            }
        }

        /// <summary>
        /// Write info on Gekko version etc.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="silent"></param>
        /// <returns></returns>
        public static StringBuilder WriteDirs(string type, bool silent)  //"small" or "large"
        {
            StringBuilder sb = new StringBuilder();
            string workingFolder = GetWorkingFolder();
            string branch = Program.GetBranch();
            string sBranch = null;
            if (!G.NullOrBlanks(branch))
            {
                if (branch.Contains(":")) sBranch = " (" + branch + ")";
                else sBranch = " (branch: " + branch + ")";
            }
            string wf = workingFolder + sBranch;
            sb.AppendLine(new string('=', Math.Max(60, wf.Length + 4))); //See #77afakjhf
            if (type == "large")
            {
                sb.AppendLine(" Gekko Timeseries Software -- timeseries handling and modeling");
            }

            string version = G.PrintVersion(Globals.gekkoVersion, false);  //here, 1.2.17 --> 1.2
            string version1 = Globals.gekkoVersion;  //here, 1.2.17 --> 1.2.17
            string version2 = G.PrintVersion(Globals.gekkoVersion, true); //here, 1.2.17 --> 1.2 (patch #17)            
            bool stable = false;
            if (version2.Split('.').Length == 2)
            {
                stable = true;
            }
            string s = version;
            if (stable) s += " (stable)";
            else s += " (develop)";
            if (type != "large")  //i.e., == "small"
            {
                sb.AppendLine(" Gekko version " + s + "  " + Globals.versionInternal);
            }
            else
            {
                sb.AppendLine(" Gekko version " + s + "  " + Globals.versionInternal);
                if (stable)
                {
                    string ss = "";
                    if (version2.Contains("patch")) ss += ". Patches only fix minor bugs etc.";
                    sb.AppendLine(" More precisely: version " + version2 + ", i.e. " + version1 + ss);
                }
            }

            if (type == "large")
            {
                string pd = GetProgramDir();
                string exe = null;
                try
                {
                    string pd2 = Path.Combine(pd, "gekko.exe");
                    DateTime modification = File.GetLastWriteTime(pd2);
                    exe = " (gekko.exe: " + modification.ToString("g", CultureInfo.GetCultureInfo(Globals.languageDaDK)) + ")";
                    //exe = " (gekko.exe: " + modification.ToString("g", CultureInfo.CreateSpecificCulture(Globals.languageDaDK)) + ")";  //This is SLOOW!
                }
                catch { }

                sb.AppendLine(" Program folder: ");
                sb.AppendLine("   " + pd + exe);
            }

            //string branch = Program.GetBranch();
            //string sBranch = null;
            //if (!G.NullOrBlanks(branch))
            //{
            //    if (branch.Contains(":")) sBranch = " (" + branch + ")";
            //    else sBranch = " (branch: " + branch + ")";
            //}

            sb.AppendLine(" Working folder: ");
            sb.AppendLine("   " + wf);

            if (type == "large")
            {
                sb.AppendLine(" User settings file (for window positions etc.): ");
                sb.AppendLine("   " + Globals.userSettingsPath);
                sb.AppendLine(" Temporary files (cached models, restore info, gnuplot data, etc.): ");
                sb.AppendLine("   " + System.Windows.Forms.Application.LocalUserAppDataPath);
                sb.AppendLine(" Excel version installed: Excel " + Program.GetExcelVersion(Program.eOfficeApp.eOfficeApp_Excel));

                try
                {
                    if (false)
                    {
                        RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
                        string[] version_names = installed_versions.GetSubKeyNames();
                        //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
                        double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
                        int SP = Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1]).GetValue("SP", 0));
                        if (SP > 0) sb.AppendLine(" Microsoft .NET version: " + Framework + ", service pack " + SP);
                        else sb.AppendLine(" Microsoft .NET version: " + Framework);
                    }
                    else
                    {
                        List<string> ss1 = GetVersionFromRegistry();
                        List<string> ss2 = Get45PlusFromRegistry();
                        sb.AppendLine(" Microsoft .NET framework versions installed:");
                        foreach (string ss in ss1) sb.AppendLine("   " + ss.Trim());
                        foreach (string ss in ss2) sb.AppendLine("   " + ss.Trim());
                    }

                    sb.AppendLine(" Bitness: " + Program.Get64Bitness(0));

                    if (false && Globals.runningOnTTComputer)
                    {
                        sb.AppendLine(Program.IsJit());
                    }

                    foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
                    {
                        sb.AppendLine(" Number of physical processors: " + item["NumberOfProcessors"]);
                    }

                    int coreCount = 0;
                    foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                    {
                        coreCount += int.Parse(item["NumberOfCores"].ToString());
                    }
                    sb.AppendLine(" Number of cores: " + coreCount);
                    sb.AppendLine(" Screen dpi scale x = " + Globals.screenDpiScaleX + "%, y = " + Globals.screenDpiScaleY + "%");
                    long size = Program.CacheFilesSize();
                    double pct = (double)size / (double)Globals.cacheFileMax * 100d;
                    sb.AppendLine(" Cache files size: " + G.UpdprtFormat((double)size / 1000000000d, 2, false) + " GB (" + G.UpdprtFormat(pct, 2, false) + "% of allocated " + G.UpdprtFormat((double)Globals.cacheFileMax / 1000000000d, 2, false) + " GB)");
                }
                catch { };  //fail silently               


            }            

            sb.AppendLine(new string('=', Math.Max(60, wf.Length + 4))); //See #77afakjhf

            sb.AppendLine();
            if (!silent)
            {
                int widthRemember = Program.options.print_width;
                Program.options.print_width = int.MaxValue;
                try
                {
                    List<string> lines = Stringlist.ExtractLinesFromText(sb.ToString());
                    foreach (string s2 in lines)
                    {
                        G.Writeln(s2);
                    }
                }
                finally
                {
                    //resetting, also if there is an error
                    Program.options.print_width = widthRemember;
                }
            }
            return sb;
        }

        /// <summary>
        /// Pretty showing current frequency and period, like "Quarterly 2020q1-2024q4".
        /// </summary>
        /// <returns></returns>
        public static string FreqAndPeriodPretty(bool lowerCaseFirstChar, bool includeObservations)
        {
            string observations = null;
            if (includeObservations) observations = " (" + GekkoTime.Observations(Globals.globalPeriodStart, Globals.globalPeriodEnd) + " periods)";
            string start = G.FromDateToString(Globals.globalPeriodStart);
            string end = G.FromDateToString(Globals.globalPeriodEnd);
            string f = Program.options.freq.Pretty();
            string ss2 = f + " " + start + "-" + end + observations;
            if (lowerCaseFirstChar) ss2 = char.ToLower(ss2[0]) + ss2.Substring(1);
            return ss2;
        }

        /// <summary>
        /// Used for the gekkoInfo() in-built function
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GekkoInfo(string s)
        {
            string rv = null;
            if (G.Equal(s, "short1"))
            {
                //Gekko 3.0.1
                rv = "Gekko " + Globals.gekkoVersion;
            }
            else if (G.Equal(s, "short2"))
            {
                //Gekko 3.0.1 (64-bit)
                rv = "Gekko " + Globals.gekkoVersion + " (" + Program.Get64Bitness(1) + "-bit)";
            }
            else if (G.Equal(s, "short3"))
            {
                //Gekko 3.0.1 (64-bit), working folder = g:\temp\gekko
                string wf = "[empty]";
                if (!string.IsNullOrEmpty(wf)) wf = Program.options.folder_working;
                rv = "Gekko " + Globals.gekkoVersion + " (" + Program.Get64Bitness(1) + "-bit), " + G.FreqAndPeriodPretty(true, false);
            }
            else if (G.Equal(s, "short4"))
            {
                //Gekko 3.0.1 (64-bit), working folder = g:\temp\gekko
                string wf = "[empty]";
                if (!string.IsNullOrEmpty(wf)) wf = Program.options.folder_working;
                rv = "Gekko " + Globals.gekkoVersion + " (" + Program.Get64Bitness(1) + "-bit), working folder = " + wf;
            }
            else if (G.Equal(s, "short5"))
            {
                //Gekko 3.0.1 (64-bit), working folder = g:\temp\gekko
                string wf = "[empty]";
                if (!string.IsNullOrEmpty(wf)) wf = Program.options.folder_working;
                rv = "Gekko " + Globals.gekkoVersion + " (" + Program.Get64Bitness(1) + "-bit), " + G.FreqAndPeriodPretty(true, false) + ", working folder = " + wf;
            }
            else
            {
                new Error("Illegal argument '" + s + "'");
            }

            return rv;
        }

        public static string ConvertFreq(EFreq freq)
        {
            string s = null;
            try
            {
                s = Globals.freqFromEnumToString[freq];
            }
            catch
            {
                new Error("Cannot recognize frequency '" + freq.ToString() + "'");
            }
            return s;
        }

        /// <summary>
        /// Helper for getting installed .NET versions
        /// </summary>
        /// <returns></returns>
        private static List<string> Get45PlusFromRegistry()
        {
            List<string> ss = new List<string>();

            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    ss.Add($"{CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}");
                }
                else
                {
                    ss.Add("4.5 or later is not detected.");
                }
            }
            return ss;
        }

        /// <summary>
        /// Helper for getting installed .NET versions
        /// </summary>
        /// <param name="releaseKey"></param>
        /// <returns></returns>
        private static string CheckFor45PlusVersion(int releaseKey)
        {
            //see https://github.com/dotnet/docs/blob/master/docs/framework/migration-guide/how-to-determine-which-versions-are-installed.md
            if (releaseKey >= 528040)
                return "4.8 or later";
            if (releaseKey >= 461808)
                return "4.7.2";
            if (releaseKey >= 461308)
                return "4.7.1";
            if (releaseKey >= 460798)
                return "4.7";
            if (releaseKey >= 394802)
                return "4.6.2";
            if (releaseKey >= 394254)
                return "4.6.1";
            if (releaseKey >= 393295)
                return "4.6";
            if (releaseKey >= 379893)
                return "4.5.2";
            if (releaseKey >= 378675)
                return "4.5.1";
            if (releaseKey >= 378389)
                return "4.5";
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }

        /// <summary>
        /// Find a WPF parent of specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T FindParent<T>(System.Windows.DependencyObject child) where T : System.Windows.DependencyObject
        {
            //get parent item
            System.Windows.DependencyObject parentObject = System.Windows.Media.VisualTreeHelper.GetParent(child);
            //we've reached the end of the tree
            if (parentObject == null) return null;
            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        public static bool DlinkDebug() 
        {            
            if (Directory.Exists("p:\\tth\\ny\\dlinkdebug.txt")) return true;
            else return false;
        }

        public static bool WriteIfChanged(string filePath, string content)
        {
            bool shouldWrite = true;
            if (File.Exists(filePath))
            {
                try
                {
                    string existingContent = File.ReadAllText(filePath);
                    if (existingContent == content) shouldWrite = false;
                }
                catch
                {
                }
            }
            if (shouldWrite)
            {
                using (FileStream fs = Program.WaitForFileStream(filePath, null, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter file = G.GekkoStreamWriter(fs))
                {
                    file.Write(content);
                }
            }
            return shouldWrite;
        }

        /// <summary>
        /// Helper for getting installed .NET versions
        /// </summary>
        /// <returns></returns>
        private static List<string> GetVersionFromRegistry()
        {
            List<string> ss = new List<string>();
            // Opens the registry key for the .NET Framework entry.
            using (RegistryKey ndpKey =
                    RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).
                    OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                foreach (var versionKeyName in ndpKey.GetSubKeyNames())
                {
                    // Skip .NET Framework 4.5 version information.
                    if (versionKeyName == "v4")
                    {
                        continue;
                    }

                    if (versionKeyName.StartsWith("v"))
                    {

                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        // Get the .NET Framework version value.
                        var name = (string)versionKey.GetValue("Version", "");
                        // Get the service pack (SP) number.
                        var sp = versionKey.GetValue("SP", "").ToString();

                        // Get the installation flag, or an empty string if there is none.
                        var install = versionKey.GetValue("Install", "").ToString();
                        if (string.IsNullOrEmpty(install)) // No install info; it must be in a child subkey.
                            ss.Add($"{versionKeyName}  {name}");
                        else
                        {
                            if (!(string.IsNullOrEmpty(sp)) && install == "1")
                            {
                                ss.Add($"{versionKeyName}  {name}  SP{sp}");
                            }
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                            continue;
                        }
                        foreach (var subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (!string.IsNullOrEmpty(name))
                                sp = subKey.GetValue("SP", "").ToString();

                            install = subKey.GetValue("Install", "").ToString();
                            if (string.IsNullOrEmpty(install)) //No install info; it must be later.
                                ss.Add($"{versionKeyName}  {name}");
                            else
                            {
                                if (!(string.IsNullOrEmpty(sp)) && install == "1")
                                {
                                    ss.Add($"{subKeyName}  {name}  SP{sp}");
                                }
                                else if (install == "1")
                                {
                                    ss.Add($"  {subKeyName}  {name}");
                                }
                            }
                        }
                    }
                }
            }
            return ss;
        }

        /// <summary>
        /// Helper for PCIM databank reading
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static String oddX0000Hack(String val)
        {
            String val1 = val.Replace('\x0000', '?');  //some odd hack regarding this odd character
            return val1;
        }

        /// <summary>
        /// Compare path names
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static bool IsSamePath(string path1, string path2)
        {
            return string.Compare(Path.GetFullPath(path1).TrimEnd('\\'), Path.GetFullPath(path2).TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        /// <summary>
        /// Helper method for TABLE
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsMissingVariableArtificialNumber(double val)
        {
            if (val > Globals.missingVariableArtificialNumberLow && val < Globals.missingVariableArtificialNumberHigh)
                return true;
            else return false;
        }

        /// <summary>
        /// Checks if input IsInfinity or IsNaN.
        /// </summary>
        /// <param name="f">Input</param>
        /// <returns>True if the input is problematic</returns>
        public static bool IsNumericalError(double f)
        {
            if (Double.IsInfinity(f) || Double.IsNaN(f)) return true;
            else return false;
        }

        /// <summary>
        /// Change any plus or minus infinity into NaN.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static double HandleNumericalError(double f)
        {
            if (Double.IsInfinity(f)) return double.NaN;
            return f;
        }

        /// <summary>
        /// First argument always "w{x}.{y}"! Used for the warning pool. Second argument may be null.
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="s"></param>
        public static void Warning(string typeId, string s)
        {
            bool discard;
            if (s == null) s = "";
            Globals.warningPool.WAdd(typeId, s, false, out discard);
        }

        /// <summary>
        /// Internal Gekko system warnings
        /// </summary>
        /// <param name="s"></param>
        public static void WarningInternal(string s)
        {
            G.Warning(Globals.INTERNAL, s);
        }

        /// <summary>
        /// Check if the week number is legal (some years have 52 weeks, some have 53 weeks). If reportError = false, 
        /// the method returns true if there is a problem/error.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="week"></param>
        /// <param name="reportError"></param>
        /// <returns></returns>
        public static bool CheckWeekNumberAndMaybePrintErrorMessage(int year, int week, bool reportError)
        {
            int maxWeeks = ISOWeek.GetWeeksInYear(year);
            if (week < 1 || week > maxWeeks)
            {
                if (reportError)
                {
                    using (Error txt = new Error())
                    {
                        txt.MainAdd("Freq 'w' has an illegal week number: " + week + ". ");
                        txt.MainAdd("The year " + year + " only contains week numbers 1-" + maxWeeks + " (inclusive). ");
                        txt.MainAdd("See the {a{ISO 8601 standard¤https://en.wikipedia.org/wiki/ISO_8601}a} regarding week numbering.");
                    }
                }
                else return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if x is between 1500 and 3000
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsYear(int x)
        {
            if (x >= Globals.possibleYearStart && x <= Globals.possibleYearEnd) return true;
            return false;
        }

        /// <summary>
        /// 1950 --> 1950.
        /// 50   --> 1950.
        /// 2010 --> 2010.
        /// 110  --> 2010.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int FindYear(int x, bool allowTwoDigits)
        {

            if (G.IsYear(x))
            {
                return x;
            }
            else if (allowTwoDigits && (x >= 0 && x <= 199))
            {
                return x + 1900;
            }
            else
            {
                string s = null;
                if (allowTwoDigits) s = " or [0 - 199] -- the latter has 1900 added";
                new Error("A year with value " + x + " was input. The allowable range for years is [" + Globals.possibleYearStart + "-" + Globals.possibleYearEnd + "]" + s + ".");
                return -12345;  //will never return anything
            }
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int FindYear(int x)
        {
            return FindYear(x, true);
        }

        /// <summary>
        /// Deep clone of object -- beware that it may be SLOW! Needs [Serializable] decoration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCloneSlow<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Returns a random long from min (inclusive) to max (exclusive)
        /// </summary>
        /// <param name="random">The given random instance</param>
        /// <param name="min">The inclusive minimum bound</param>
        /// <param name="max">The exclusive maximum bound.  Must be greater than min</param>
        public static long NextLong(Random random, long min, long max)
        {
            if (max <= min)
                throw new ArgumentOutOfRangeException("max", "max must be > min!");

            //Working with ulong so that modulo works correctly with values > long.MaxValue
            ulong uRange = (ulong)(max - min);

            //Prevent a modolo bias; see https://stackoverflow.com/a/10984975/238419
            //for more information.
            //In the worst case, the expected number of calls is 2 (though usually it's
            //much closer to 1) so this loop doesn't really hurt performance at all.
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return (long)(ulongRand % uRange) + min;
        }

        /// <summary>
        /// Pretty print name and frequency
        /// </summary>
        /// <param name="input"></param>
        /// <param name="useQuotes"></param>
        /// <returns></returns>
        public static string GetNameAndFreqPretty(string input, bool useQuotes)
        {
            //returns '%s' or 'x' or 'x' (Annual)
            string freq = null; string varname = null;
            O.ChopFreq(input, ref freq, ref varname);
            string freqPretty = null;
            if (freq != null) freqPretty = " (" + ConvertFreq(freq).Pretty() + ")";
            if (useQuotes) return "'" + varname + "'" + freqPretty;
            else return varname + freqPretty;
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetNameAndFreqPretty(string input)
        {
            return GetNameAndFreqPretty(input, true);
        }

        /// <summary>
        /// An array-series must have a name, also when it is created as part of an expression.
        /// The name has no significance, but must end with !-freq.
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static string GetArraySeriesTempName(EFreq freq)
        {
            return G.Chop_AddFreq(Globals.seriesArraySuperName, freq);
        }

        /// <summary>
        /// Check that t1 is less than or equal to t2
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public static void CheckLegalPeriod(GekkoTime t1, GekkoTime t2)
        {
            if (t1.IsNull() || t2.IsNull()) return;  //we accept this
            int n = GekkoTime.Observations(t1, t2);
            if (n < 1)
            {
                new Error("Start date (" + t1.ToString() + ") should be same as or before end date (" + t2.ToString() + ")");
            }
        }

        /// <summary>
        /// Get type of variable as string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetTypeString(IVariable input)
        {
            return input.Type().ToString().ToLower();
        }

        /// <summary>
        /// Replace string inside string, case insensitive
        /// </summary>
        /// <param name="str"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="onlyFirst"></param>
        /// <returns></returns>
        static public string ReplaceString(string str, string oldValue, string newValue, bool onlyFirst)
        {
            //Is always case-insensitive!
            StringComparison comparison = StringComparison.OrdinalIgnoreCase;
            StringBuilder sb = new StringBuilder();
            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            int counter = 0;
            while (index != -1)
            {
                if (onlyFirst && counter > 0) break;
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;
                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
                counter++;
            }
            sb.Append(str.Substring(previousIndex));
            return sb.ToString();
        }

        /// <summary>
        /// Convert from GekkoTime to string, cf. FromStringToDate()
        /// </summary>
        /// <param name="gt"></param>
        /// <returns></returns>
        public static string FromDateToString(GekkoTime gt)
        {
            //The reverse: see FromStringToDate()
            string s = "";
            s = gt.super.ToString() + G.GetSubPeriodString(gt);
            return s;
        }

        /// <summary>
        /// For a date like 2020m10d2 gets "m10d2".
        /// </summary>
        /// <param name="gt"></param>
        /// <returns></returns>
        public static string GetSubPeriodString(GekkoTime gt)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================
            string subend = "";
            if (gt.freq == EFreq.A)
            {
            }
            else if (gt.freq == EFreq.Q)
            {
                subend = "q" + gt.sub;
            }
            else if (gt.freq == EFreq.M)
            {
                subend = "m" + gt.sub;
            }
            else if (gt.freq == EFreq.W)
            {
                subend = "w" + gt.sub;
            }
            else if (gt.freq == EFreq.D)
            {
                subend = "m" + gt.sub + "d" + gt.subsub;
            }
            else if (gt.freq == EFreq.U)
            {
            }
            else throw new GekkoException();
            return subend;
        }

        /// <summary>
        /// Find last occurence of find in s, replace it with replace.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="find"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static string ReplaceLastOccurrence(string s, string find, string replace)
        {
            if (s == null) return s;
            int place = s.LastIndexOf(find);
            if (place == -1)
                return s;
            return s.Remove(place, find.Length).Insert(place, replace);
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="handleDoubleQuote"></param>
        /// <returns></returns>
        public static string ReplaceGlueSymbols(string s)
        {
            return ReplaceGlueSymbols(s, false);
        }

        /// <summary>
        /// Internal method. Before a .gcm file is parsed, some "glue" symbols are added to aid parsing. These are removed here.
        /// Use to transform parser errors containing parts of the .gcm code.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceGlueSymbols(string s, bool handleDoubleQuote)
        {
            //See replacement in new errors: #9j5n34jererjn
            if (s == null) return s;
            s = s.Replace(Globals.symbolGlueChar2, "");
            s = s.Replace(Globals.symbolGlueChar3, "");
            s = s.Replace(Globals.symbolGlueChar4, "");
            s = s.Replace(Globals.symbolGlueChar5, "<");  //--> fixme, this is a workaround
            s = s.Replace(Globals.symbolGlueChar6, "[");  //--> fixme, this is a workaround, #098523            
            s = s.Replace(Globals.symbolGlueChar7, "[");
            s = s.Replace(Globals.symbolGlueChar1.ToString(), "");  //must be after symbolGlueChar7

            //the following are probably obsolete in Gekko 3.0
            s = Regex.Replace(s, "s___er", "ser", RegexOptions.IgnoreCase);  //#098275432874
            s = Regex.Replace(s, "s___eries", "series", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, "s____er", "ser", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, "s____eries", "series", RegexOptions.IgnoreCase);

            if (handleDoubleQuote)
            {
                s = G.HandleQuoteInQuote(s);
            }

            return s;
        }

        /// <summary>
        /// Accepts names like a38, _xy, that is, "variable names".
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsIdent(string s)
        {
            bool simple = true;
            bool first = true;
            foreach (char c in s)
            {
                if (first)
                {
                    if (!G.IsLetterOrUnderscore(c))
                    {
                        simple = false;
                        break;
                    }
                }
                else
                {
                    if (!G.IsLetterOrDigitOrUnderscore(c))
                    {
                        simple = false;
                        break;
                    }
                }
                first = false;
            }
            return simple;
        }

        /// <summary>
        /// Checks if a substring inside input starting at i and with given length is word-like. That is, there are no alphanumeric (or '_'
        /// just before or after the substring. 
        /// For instance: good for for making sure "c:\bank1" does not match input "c:\bank1a", but matches input "c:\bank1a\bank2".
        /// See _Test_WordMatch.        
        /// See also G.Match() and G.AllIndexOf().
        /// </summary>
        /// <param name="input"></param>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool IsDelimited(string input, int i, int length)
        {
            if (i > 0 && G.IsLetterOrDigitOrUnderscore(input[i - 1])) return false;
            if (i + length < input.Length && G.IsLetterOrDigitOrUnderscore(input[i + length])) return false;
            return true;
        }

        /// <summary>
        /// Looks for the string input inside the elements. BEWARE: special logic so "c:\bank1" does not match "c:\bank1a", but matches "c:\bank1\bank2".
        /// See also G.AllIndexOf() and G.IsDelimited().
        /// </summary>
        /// <param name="input"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static bool Match(string input, List<string> elements)
        {
            foreach (string s in elements)
            {
                List<int> allIndexOf = G.AllIndexOf(input, s, StringComparison.OrdinalIgnoreCase);
                foreach (int i in allIndexOf)
                {
                    bool match = G.IsDelimited(input, i, s.Length);
                    if (match) return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Cf. IsIdent()
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsIdentTranslate(string s)
        {
            if (s == null || s == "") return false;
            return IsIdent(s);
        }

        /// <summary>
        /// Check if a string is an integer, with special options.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="canHaveMinus"></param>
        /// <param name="canBeZero"></param>
        /// <returns></returns>
        public static bool IsInteger(string s, bool canHaveMinus, bool canBeZero)
        {
            //if canBeZero is false, the integer has to be <> 0 when evaluated
            bool first = true;
            foreach (char c in s)
            {
                if (canHaveMinus)
                {
                    if (first)
                    {
                        if ((c != '-') && !char.IsDigit(c)) return false;
                    }
                    else
                    {
                        if (!char.IsDigit(c)) return false;
                    }
                }
                else
                {
                    if (!char.IsDigit(c)) return false;
                }
                first = false;
            }
            if (canBeZero == false)
            {
                int i = int.Parse(s);
                if (i == 0) return false;  //will catch "0" and "-0"
            }
            return true;
        }

        /// <summary>
        /// Overload. Can have minus = false, can be 0 = true. This is the fastest of the argument possibilities of the called method.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsInteger(string s)
        {
            return IsInteger(s, false, true);
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsIntegerTranslate(string s)
        {
            if (s == null || s == "") return false;
            return IsInteger(s);
        }

        /// <summary>
        /// Check if variable is series or value of 1x1 matrix. Used for printing/plotting.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsValueType(IVariable x)
        {
            if (x.Type() == EVariableType.Series)
            {
                return true;
            }
            else if (x.Type() == EVariableType.Val)
            {
                return true;
            }
            else if (x.Type() == EVariableType.Matrix && ((Matrix)x).data.Length == 1)  //an 1x1 matrix
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Converts a double into nearest int. Expects the input value to be very near to an integer, tolerance 0.000001 absolute.
        /// Will handle negative values ok.
        /// </summary>
        /// <param name="rounded"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConvertToInt(out int rounded, double value)
        {
            bool flag = true;
            try
            {
                rounded = Convert.ToInt32(value);  //this function rounds to nearest int, so -12.98 --> -13
            }
            catch
            {
                new Error("Could not convert the value '" + value + "' into an integer (32-bit integer at most)");
                throw;  //will not happen
            }
            double decimals = value - rounded;
            if (G.IsNumericalError(value) || Math.Abs(decimals) > 0.000001)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Converts a double into nearest int. Expects the input value to be very near to an integer, tolerance 0.000001 absolute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ConvertToInt(double value)
        {
            //simpler method
            bool flag = false;
            int rounded;
            try
            {
                rounded = Convert.ToInt32(value);  //this function rounds to nearest int, so -12.98 --> -13
            }
            catch
            {
                new Error("Could not convert the value '" + value + "' into an integer (32-bit integer at most)");
                throw;  //will not happen
            }
            double decimals = value - rounded;
            if (G.IsNumericalError(value) || Math.Abs(decimals) > 0.000001)
            {
                new Error("Could not convert " + value + " into integer");
            }
            return rounded;
        }

        /// <summary>
        /// Converts a double into nearest long. Expects the input value to be very near to a long, tolerance 0.000001 absolute.
        /// Will handle negative values ok.
        /// </summary>
        /// <param name="rounded"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConvertToLong(out long rounded, double value)
        {
            bool flag = true;
            try
            {
                rounded = Convert.ToInt64(value);  //this function rounds to nearest int, so -12.98 --> -13
            }
            catch
            {
                new Error("Could not convert the value '" + value + "' into an integer (64-bit integer at most)");
                throw;  //will not happen
            }
            double decimals = value - rounded;
            if (G.IsNumericalError(value) || Math.Abs(decimals) > 0.000001)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Converts a double into nearest long. Expects the input value to be very near to a long, tolerance 0.000001 absolute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ConvertToLong(double value)
        {
            //simpler method
            bool flag = false;
            long rounded;
            try
            {
                rounded = Convert.ToInt64(value);  //this function rounds to nearest int, so -12.98 --> -13
            }
            catch
            {
                new Error("Could not convert the value '" + value + "' into an integer (64-bit integer at most)");
                throw;  //will not happen
            }
            double decimals = value - rounded;
            if (G.IsNumericalError(value) || Math.Abs(decimals) > 0.000001)
            {
                new Error("Could not convert " + value + " into 64-bit integer");
            }
            return rounded;
        }


        /// <summary>
        /// Converts a string into an integer. Returns int.MaxValue if fail.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ConvertToInt(string input)
        {
            int output = int.MaxValue;
            if (input == null) return output;
            try
            {
                output = int.Parse(input);
            }
            catch (Exception e) { };
            return output;

        }

        /// <summary>
        /// Delete an entire folder, with omit.        /// 
        /// If total == true, BEWARE that you are pointing to the RIGHT folder always. Otherwise,
        /// a lot may be damaged. Not sure how this works for nested folders. 
        /// omitType is without dot, for instance "css". To delete everything,
        /// it is perhaps best to simply use the C# Directory.Delete(topPath, true).
        /// </summary>
        /// <param name="s"></param>
        /// <param name="omitType"></param>
        public static void DeleteFolder(string s, string omitType, bool total)
        {
            if (omitType != null && total) new Error("DeleteFolder() wrong mix");
            if (!Directory.Exists(s)) return;
            DeleteFolderHelper(new DirectoryInfo(s), omitType);
            if (total)
            {
                try
                {
                    Directory.Delete(s);
                }
                catch
                {
                    //we may survive if this fails
                }
            }
        }

        /// <summary>
        /// Delete an entire folder. 
        /// If total == true, BEWARE that you are pointing to the RIGHT folder always. Otherwise,
        /// a lot may be damaged.
        /// </summary>
        /// <param name="s"></param>
        public static void DeleteFolder(string s, bool total)
        {
            DeleteFolder(s, null, total);
        }

        /// <summary>
        /// Adds an extension to a filename, if no extension already given. For instance AddExtension("demo", ".gcm") --> "demo.gcm".
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="endingIncludingDot"></param>
        /// <returns></returns>
        public static string AddExtension(string fileName, string endingIncludingDot)
        {
            if (Path.GetExtension(fileName) == "") fileName += endingIncludingDot;  //ignore case
            return fileName;
        }

        /// <summary>
        /// Replaces all whitespace like normal spaces, tabs, newlines etc. with 1 blank.
        /// </summary>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static string ReplaceWhitespaceWith1Blank(string s2) {
            if (s2 == null) return s2;  //regex below does not accept null
            string s = Regex.Replace(s2, @"\s+", " ");
            return s;
        }

        /// <summary>
        /// Split a string into two parts, splitting at a given position.
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s"></param>
        /// <param name="i"></param>
        public static void SplitString(out string s1, out string s2, string s, int i)
        {
            s1 = s.Substring(0, i);
            s2 = s.Substring(i, s.Length - i);
        }

        /// <summary>
        /// Helper for deleting folders. Total to also delete structure.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="omitType"></param>
        private static void DeleteFolderHelper(DirectoryInfo directoryInfo, string omitType)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (omitType != null && G.Equal("." + omitType, file.Extension)) continue;  //skip it
                try
                {
                    file.Delete();  //hmm probably best not to use WaitForFileDelete() here, exceptions are typically caught in a wrapper on this method, and not critical if it fails (used for cleanup)
                }
                catch
                {
                    //we may survive if this fails
                }
            }
            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                DeleteFolderHelper(subfolder, omitType);
            }
        }

        /// <summary>
        /// Convert from "f", and from "procedure__g". 
        /// With type == 1, we return "f()" or "g".
        /// With type == 2, we return "function f()" or "procedure g".
        /// With type == 3, we return "'f()'" or "'g'".
        /// With type == 4, we return "function 'f()'" or "procedure 'g'".
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FromLibraryToFunctionProcedureName(string s, int type)
        {
            if (type < 1 || type > 4) new Error("Wrong type");
            string ss = null;
            if (s.StartsWith(Globals.procedure)) ss = s.Substring(Globals.procedure.Length);
            else ss = s + "()";
            if (type == 3 || type == 4) ss = "'" + ss + "'";
            if (type == 2 || type == 4)
            {
                if (s.StartsWith(Globals.procedure)) ss = "procedure " + ss;
                else ss = "function " + ss;
            }
            return ss;
        }


        /// <summary>
        /// Calendar function.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            if (year < 1 || year > 9999)
            {
                new Error("year must be between 1-9999");
                //throw new GekkoException();
            }
            return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
        }

        /// <summary>
        /// Transforms for instance "the car is red" into "The car is red".
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: new Error("Null string for FirstCharToUpper()"); break;
                case "": new Error("Empty string for FirstCharToUpper()"); break;
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
            return null;
        }

        /// <summary>
        /// Converts e.g. "fxo" to "fXo" is a model is loaded (using casing of model)
        /// </summary>
        /// <param name="var">Input string</param>
        /// <returns>Output string</returns>
        public static string GetUpperLowerCase(string var)
        {
            if (G.GetModelSourceType() == EModelType.Gekko && Program.model.modelGekko.varsAType != null)
            {
                //a Gekko model is loaded
                //ATypeData temp = (ATypeData)Program.model.modelGekko.varsAType[var];
                ATypeData temp = null; Program.model.modelGekko.varsAType.TryGetValue(new DNameSimplest(var), out temp);
                if (temp != null)
                {
                    //var is known from the model
                    var = temp.varName;  //with nicer upper/lowercase.
                }
            }
            return var;
        }

        /// <summary>
        /// Where did the model come from? This is not always the same as DecompType().
        /// Operates on global loaded model.
        /// </summary>
        /// <returns></returns>
        public static EModelType GetModelSourceType()
        {
            return Program.model.modelCommon.GetModelSourceType();
        }

        /// <summary>
        /// Prints list of strings to file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="list"></param>
        public static void PrintListWithCommasToFile(StreamWriter file, List<string> list)
        {
            bool first = true;
            foreach (string s in list)
            {
                if (first) file.Write(s);
                else file.Write(", " + s);
                first = false;
            }
            file.Write("");
            return;
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="list"></param>
        /// <param name="insertLinks"></param>
        public static void PrintListWithCommas(List<string> list, bool insertLinks)
        {
            PrintListWithCommas(list, insertLinks, false);
            return;
        }

        /// <summary>
        /// Print list of strings with commas, possibly with links
        /// </summary>
        /// <param name="list"></param>
        /// <param name="insertLinks"></param>
        /// <param name="nocr"></param>
        public static void PrintListWithCommas(List<string> list, bool insertLinks, bool nocr)
        {
            int counter = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string s = list[i];
                counter += s.Length + 2;

                if (insertLinks)
                {
                    {
                        G.WriteLink(s, "disp:" + s);
                        if (i < list.Count - 1) G.Write(", ");
                    }
                }
                else
                {
                    string s2 = s;
                    if (i < list.Count - 1) s2 += ", ";
                    G.Write(s2);  //best to write string in one go, gives better line wrapping with commas
                }
            }
            if (!nocr) G.Writeln();
            return;
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>        
        public static void Write(string s)
        {
            WriteAbstract(EWrapType.Writeln, s, null, false, Color.Empty, false, ETabs.Main);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>        
        public static void Write(string s, ETabs tab)
        {
            WriteAbstract(EWrapType.Writeln, s, null, false, Color.Empty, false, tab);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        /// <param name="s"></param>
        public static void Write(string s, Color color)
        {
            WriteAbstract(EWrapType.Writeln, s, null, false, color, false, ETabs.Main);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Write(string s, Color color, ETabs tab)
        {
            WriteAbstract(EWrapType.Writeln, s, null, false, color, false, tab);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>        
        public static void Write(int x)
        {
            WriteAbstract(EWrapType.Writeln, x.ToString(), null, false, Color.Empty, false, ETabs.Main);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Write(int x, ETabs tab)
        {
            WriteAbstract(EWrapType.Writeln, x.ToString(), null, false, Color.Empty, false, tab);
        }
        /// <summary>
        /// For writing output to screen
        /// </summary>        
        public static void Write(double x)
        {
            WriteAbstract(EWrapType.Writeln, x.ToString(), null, false, Color.Empty, false, ETabs.Main);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Write(double x, ETabs tab)
        {
            WriteAbstract(EWrapType.Writeln, x.ToString(), null, false, Color.Empty, false, tab);
        }

        /// <summary>
        /// Convert a GekkoAction call ino a string that will transform into a link in Gekko.
        /// </summary>
        public static string GetLinkAction(string s, GekkoAction a)
        {
            string s2 = Globals.linkActionStart + s + Globals.linkActionDelimiter + ++Globals.linkActionCounter + Globals.linkActionEnd;
            Globals.linkAction.Add(Globals.linkActionCounter, a);
            return s2;
        }

        /// <summary>
        /// For writing link to screen (without line feed)
        /// </summary>
        /// <param name="s"></param>
        public static void WriteLink(string text, string linktype)
        {
            //for instance input0 = "output", "tab:output"
            //for instance input0 = "fY", "disp:fY"
            //for instance input0 = "sim", "help:sim"            
            //see Gui.textBox1_LinkClicked
            WriteAbstract(EWrapType.Writeln, text, linktype, false, Color.Empty, true, ETabs.Main);  //Color is not used anyway -- gets blue underlined
        }

        public static void WriteLink(string text, string linktype, ETabs tab)
        {
            //for instance input0 = "output", "tab:output"
            //for instance input0 = "fY", "disp:fY"
            //for instance input0 = "sim", "help:sim"            
            //see Gui.textBox1_LinkClicked
            WriteAbstract(EWrapType.Writeln, text, linktype, false, Color.Empty, true, tab);  //Color is not used anyway -- gets blue underlined
        }

        /// <summary>
        /// Printing, with extra blank line before
        /// </summary>
        public static void Writeln2(string s)
        {
            Writeln();
            Writeln(s);
        }

        /// <summary>
        /// Printing, with extra blank line before
        /// </summary>
        public static void Write2(string s)
        {
            Writeln();
            Write(s);
        }

        /// <summary>
        /// Printing, with extra blank line before
        /// </summary>
        public static void Writeln2(string s, Color color)
        {
            Writeln();
            Writeln(s, color);
        }


        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln(string s)
        {
            WriteAbstract(EWrapType.Writeln, s, null, true, Color.Empty, false, ETabs.Main);
        }

        /// <summary>
        /// For writing output to screen, with a type/indent like error/warning/note
        /// </summary>
        public static void Writeln(EWrapType type, string s)
        {
            WriteAbstract(type, s, null, true, Color.Empty, false, ETabs.Main);
        }

        /// <summary>
        /// For writing output to screen, with a type/indent like error/warning/note.
        /// </summary>
        public static void Writeln2(EWrapType type, string s)
        {
            G.Writeln();
            WriteAbstract(type, s, null, true, Color.Empty, false, ETabs.Main);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln(string s, ETabs tab)
        {
            WriteAbstract(EWrapType.Writeln, s, null, true, Color.Empty, false, tab);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln(string s, Color color)
        {
            WriteAbstract(EWrapType.Writeln, s, null, true, color, false, ETabs.Main);
        }


        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln(string s, Color color, bool mustAlsoWriteToScreen)
        {
            WriteAbstractScroll(EWrapType.Writeln, s, null, null, true, color, false, ETabs.Main, false, mustAlsoWriteToScreen);
        }


        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln(string s, Color color, ETabs tab)
        {
            WriteAbstract(EWrapType.Writeln, s, null, true, color, false, tab);
        }


        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void WriteAbstract(EWrapType type, string s, string linktype, bool newline, Color color, bool link, ETabs tab)
        {
            WriteAbstractScroll(type, s, linktype, null, newline, color, link, tab, false, false);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void WriteAbstractScroll(EWrapType type, string s, string linktype, Action a, bool newline, Color color, bool link, ETabs tab, bool mustScrollToEnd, bool mustAlsoPrintToScreen)
        {
            if (Globals.applicationIsInProcessOfDying)
            {
                //if (s.Trim() != "") MessageBox.Show(s);  //this just creates confusion
                return;
            }

            Program.WorkerThreadHelper2 helper = new Program.WorkerThreadHelper2();
            if (s == null)
            {
                s = "";
            }
            if (s.Trim().StartsWith(Globals.errorString))
            {
                color = Color.Red;
                Globals.numberOfErrors++;
            }
            else if (s.Trim().StartsWith(Globals.warningString))
            {
                color = Globals.warningColor;
                Globals.numberOfWarnings++;
            }
            helper.color = color;
            helper.s = s;
            helper.linktype = linktype;
            helper.newline = newline;
            helper.link = link;
            helper.tab = tab;
            helper.mustScrollToEnd = mustScrollToEnd;
            helper.mustAlsoPrintToScreen = mustAlsoPrintToScreen;
            helper.type = type;
            helper.parentOfAll = true;  //only true here, not when cloning!

            if (Globals.workerThread == null)
            {
                //typically only just when program starts -- worker thread not yet created
                WriteAbstract2(helper);
            }
            else
            {
                Globals.workerThread.gekkoGui.Invoke(Globals.workerThread.gekkoGui.threadDelegateAddString, new Object[] { helper });
            }
        }

        /// <summary>
        /// The precision of the last modification time being stored in a file system varies between different file systems (VFAT, FAT, NTFS). 
        /// So, its best to use an epsillon environment for this and either let the user choose a sensible value or choose a value based 
        /// on the involved file systems. https://superuser.com/questions/937380/get-creation-time-of-file-in-milliseconds
        /// </summary>
        /// <returns></returns>
        public static bool FilesHaveSameWriteTime(string f1, string f2)
        {
            double epsilon = 2.0;  //can be around 2 sec for FAT/VFAT
            DateTime lastUpdateA = File.GetLastWriteTime(f1);
            DateTime lastUpdateB = File.GetLastWriteTime(f2);
            if (Math.Abs(Math.Round((lastUpdateA - lastUpdateB).TotalSeconds)) > epsilon) return false;
            return true;
        }

        /// <summary>
        /// Compares two files, returns true if they are identical.
        /// If unequal size --> false. 
        /// Else if option_strict == false and the dates are within 2 seconds --> true.
        /// Else two md5 hashes are computed and are used to return true or false.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="option_strict"></param>
        /// <returns></returns>
        public static bool CompareFiles(string p1, string p2, bool option_strict, bool option_date)
        {
            if (option_date)
            {
                if (G.FilesHaveSameWriteTime(p1, p2)) return true;
                else return false;
            }

            if ((new FileInfo(p1)).Length != (new FileInfo(p2)).Length) return false;
            if (!option_strict && G.FilesHaveSameWriteTime(p1, p2)) return true;
            //This is about 4x slower than the other way below
            //File.ReadAllBytes(p1).SequenceEqual(File.ReadAllBytes(p2)) is 2x slower than (GetMd5FromFile()+GetMd5FromFile())
            //So here we get a 4x speedup all in all. And we could report dublets in each folder.
            //identical = File.ReadAllBytes(p1).SequenceEqual(File.ReadAllBytes(p2));           
            //Now the hard way
            List<int> numbers = new List<int>() { 0, 1 };
            string md5_1 = null;
            string md5_2 = null;
            System.Threading.Tasks.Parallel.ForEach(numbers, number =>   //TODO: could test chunks...?
            {
                if (number == 0) md5_1 = G.GetMd5FromFile(p1, null);
                else md5_2 = G.GetMd5FromFile(p2, null);
            });
            if (md5_1 == md5_2) return true;  //almost certainly identical            
            return false;
        }

        /// <summary>
        /// Gets a MD5 hash from a file. Seems to be the fastest reasonable hash available (faster than SHA). Not parallel though. See GetMd5FromText().
        /// </summary>
        /// <param name="fileNameWithPath"></param>
        /// <returns></returns>
        public static string GetMd5FromFile(string fileNameWithPath, string extraSalt)
        {
            //tried physically splitting file in n chunks --> 
            //has about same speed as MD5 itself... (0.6 s for a 176 MB file)                
            //also, copying the file with File.Copy is not that much slower than MD5 itself.
            //So we need to use something that operates on the file itself, also cannot put it in
            //byte[] array and operate on this.
            //Maybe just accept it, or wait until a suitable parallel implementation of SHA3.
            //Cannot use xxHash and similar directly, they produce a ulong suitable for Dictionary
            //hashing.
            //In general, allowing READ <type> xx.zip, where file.type is inside the zip would be nice,
            //because then the hashing would be faster. User would have to zip gdx files though.
            //
            //!! actually if xxHash returns 128 bits (uint128), that is actually the same as
            //   MD5. Then the question is about collisions... Maybe when this:
            //   https://github.com/uranium62/xxHash adds stream support for 128 bit hashes.
            //
            //Conclusion: using md5 to compare files is 2x faster than SequenceEqual().
            //But with a lot of different files this may not be true.
            //Typically for compareFolders() most files are unchanged, and if not they would typically differ in size anyway.
            //Two files with same exact size are often identical.
            //As a benefit we get to tell number of dublets.

            string hash = null;
            using (MD5 md5Instance = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(fileNameWithPath))
                {

                    string salt = extraSalt;
                    if (salt == null) salt = "";
                    byte[] saltBytes = System.Text.Encoding.UTF8.GetBytes(salt);
                    byte[] buffer = new byte[4096]; // Read file in 4KB chunks
                    int bytesRead;

                    // 1. Feed the Salt into the hash
                    md5Instance.TransformBlock(saltBytes, 0, saltBytes.Length, null, 0);

                    // 2. Feed the File into the hash in chunks
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        md5Instance.TransformBlock(buffer, 0, bytesRead, null, 0);
                    }

                    // 3. Finalize the hash (must call TransformFinalBlock with an empty array or the last chunk)
                    md5Instance.TransformFinalBlock(new byte[0], 0, 0);

                    // 4. Get the resulting hash
                    byte[] hash2 = md5Instance.Hash;

                    // Your custom Base64 formatting
                    hash = System.Convert.ToBase64String(hash2)
                                 .Replace("=", "")
                                 .Replace("+", "a")
                                 .Replace("/", "b");
                }
            }
            return hash;
        }

        /// <summary>
        /// /// Gets a MD5 hash from text. Seems to be the fastest reasonable hash available (faster than SHA). Not parallel though. See G.GetMd5FromFile().
        /// </summary>
        /// <param name="inputText"></param>
        /// <returns></returns>
        public static string GetMd5FromText(string inputText, string extraSalt)
        {
            string hash = null;
            string salt = extraSalt;
            if (salt == null) salt = "";
            // step 1, calculate MD5 hash from input            
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputText + salt);  //UTF8 seems best choice
            byte[] hash2 = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string            
            hash = System.Convert.ToBase64String(hash2).Replace("=", "").Replace("+", "a").Replace("/", "b");
            //We remove empty indicator (=), and replace the two non-alphanumeric as well for simplicity.
            //a Base64-encoding can put 6 bits in each symbol, so that 128 bits become 23 symbols.
            //This is a little better than hex (32 symbols).
            return hash;
        }

        public static string GetSha256FromFile(string filePath)
        {
            string hash = null;
            using (var stream = File.OpenRead(filePath))
            {
                using (var sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    // Convert bytes to a hex string
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    hash = sb.ToString();
                }
            }

            return hash;
        }

        /// <summary>
        /// Checks if a file is blocked by the filesystem/Windows. This method may exist somewhere else, something
        /// like it was possibly being used in Gekko 2.x.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static bool IsBlocked(string FileName)
        {
            bool isBlocked = false;
            if (System.IO.File.Exists(FileName))
            {
                try
                {
                    using (Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                    }
                }
                catch (Exception ex)
                {
                    isBlocked = true;
                }
            }
            return isBlocked;
        }        

        /// <summary>
        /// Used in DECOMP. Not case sensitive.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool ContainsWord(string s, string word)
        {
            return Regex.Match(s, @"\b" + word + @"\b", RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// Normalizes a folder name so it only uses backslashes and does not end with backslash.
        /// Also optionally checks if the folder exists.
        /// </summary>
        /// <param name="f1"></param>
        /// <returns></returns>
        public static string CleanupFolderName(string f1, bool check)
        {
            if (f1 == null) return f1;
            f1 = f1.Trim();
            f1 = f1.Replace("/", "\\");
            if (f1.EndsWith("\\")) f1 = f1.Substring(0, f1.Length - 1);
            if (check && !Directory.Exists(f1)) new Error("Folder '" + f1 + "' does not seem to exist");
            return f1;
        }

        /// <summary>
        /// Helper for printing percentage progress on long jobs
        /// </summary>
        /// <param name="total"></param>
        /// <param name="current"></param>
        /// <param name="lastReportedPercent"></param>
        /// <param name="show"></param>
        /// <param name="message"></param>
        /// <param name="gap"></param>
        public static void PrintProgress(int total, ref int current, ref int lastReportedPercent, bool show, string message, int gap)
        {
            current++;
            if (show && total > 0)
            {
                int currentPercent = (int)((double)current / total * 100);
                if (currentPercent >= lastReportedPercent + gap || current == total)
                {
                    int displayPercent = (currentPercent / gap) * gap;
                    if (displayPercent > lastReportedPercent)
                    {
                        using (Writeln txt = new Writeln())
                        {
                            txt.MainOmitVeryFirstNewLine();
                            txt.color = System.Drawing.Color.Gray;
                            txt.MainAdd(displayPercent + "% of " + total + " " + message);
                        }
                        lastReportedPercent = displayPercent;
                    }
                }
            }
        }

        public static void ReadOnlyRemove(string fileName)
        {
            //Remove read-only
            FileAttributes attributes = File.GetAttributes(fileName);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                attributes = attributes & ~FileAttributes.ReadOnly;
                File.SetAttributes(fileName, attributes);
            }
        }

        public static void ReadOnlySet(string fileName)
        {
            FileAttributes attributes = File.GetAttributes(fileName);
            if ((attributes & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
            {
                attributes |= FileAttributes.ReadOnly;
                File.SetAttributes(fileName, attributes);
            }
        }

        public static T YamlReader<T>(string fileName)
        {
            T output = default(T);
            try
            {
                string s = File.ReadAllText(fileName);
                var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
                    .IgnoreUnmatchedProperties()
                    .Build();
                output = deserializer.Deserialize<T>(s);
            }
            catch
            {
                new Error("Yaml reader problem");
            }
            return output;
        }

        public static void YamlWriter<T>(object? obj, string fileName)
        {
            try
            {
                T input = (T)obj;
                var serializer = new YamlDotNet.Serialization.SerializerBuilder().ConfigureDefaultValuesHandling(YamlDotNet.Serialization.DefaultValuesHandling.OmitNull).Build();
                string s = serializer.Serialize(input);
                File.WriteAllText(fileName, s);
            }
            catch
            {
                new Error("Yaml writer problem");
            }
        }

        /// <summary>
        /// Used to remove comments inside a JSON file (they officially do not allow comments, even though comments in JavaScript are as in C#)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveComments(string input)
        {
            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)\r?\n";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";
            string noComments = Regex.Replace(input,
                blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                me => {
                    if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                        return me.Value.StartsWith("//") ? Environment.NewLine : "";
                    // Keep the literal strings
                    return me.Value;
                },
                RegexOptions.Singleline);
            return noComments;
        }

        /// <summary>
        /// For developer use/debugging
        /// </summary>        
        public static void WritelnGray(string s)
        {
            if (!Globals.runningOnTTComputer) return;
            if (!Globals.printGrayLinesForDebugging) return;
            WriteAbstract(EWrapType.Writeln, s, null, true, Color.Gray, false, ETabs.Main);
        }

        /// <summary>
        /// Helper class to show GUI progress. See CheckFractions().
        /// </summary>
        /// <param name="count"></param>
        /// <param name="fractions"></param>
        /// <param name="fractions2"></param>
        public static void GetFractions(int count, out List<int> fractions, out List<double> fractions2)
        {
            fractions = new List<int>();
            fractions2 = new List<double>();
            for (double dd = 0.05; dd <= 1.0; dd = dd + 0.05)
            {
                fractions.Add((int)(dd * (double)count));
                fractions2.Add(dd);
            }
        }

        /// <summary>
        /// See GetFractions(). Set threshold so it does not activate for small jobs.
        /// </summary>
        /// <param name="lines2"></param>
        /// <param name="fractions"></param>
        /// <param name="fractions2"></param>
        /// <param name="lineCounter"></param>
        public static void CheckFractions(int lineCounter, int numberOfLines, int threshold, List<int> fractions, List<double> fractions2)
        {
            if (numberOfLines >= threshold)
            {
                for (int i = 0; i < fractions.Count; i++)
                {
                    if (fractions[i] == lineCounter)
                    {
                        G.Writeln("    Progress: " + (int)(Math.Round(100 * fractions2[i])) + "% of " + numberOfLines + " elements", Color.Gray);
                    }
                }
            }
        }

        /// <summary>
        /// Reasonably fast check regarding even numbers not being prime, and that divisors only run up to sqrt(num).
        /// See comments to NextPrimt().
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static bool IsPrime(int num)
        {
            if (num <= 1) return false; // Not prime if less than or equal to 1
            if (num == 2) return true; // 2 is prime            
            for (int i = 2; i * i <= num; i++) // Start checking from 2
                if (num % i == 0) return false; // Not prime if divisible by any i
            return true; // Return true if it is prime
        }

        /// <summary>
        /// Not particularly fast. Used for pivot tables.
        /// Will not check num itself, so will not return num, even if num is prime.
        /// Even if num = 1 000 000 000, generating 1000 of these takes &lt; 0.05 seconds.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int NextPrime(int num)
        {
            for (int i = num + 1; i < int.MaxValue; i++)
            {
                if (G.IsPrime(i)) return i;
            }
            return -12345;  //hmm, probably never arrives here...!
        }

        /// <summary>
        /// Folder name, cf. GekkoExePath(). This ought to be a bullet proof way to find the folder of Gekko.exe, regardless of called from
        /// Python, Excel, etc. etc.
        /// </summary>
        /// <returns></returns>
        public static string GekkoExeFolder()
        {
            return Path.GetDirectoryName(G.GekkoExePath());
        }

        public static bool LooksLikeYear(string s)
        {
            bool good = false;
            if (s.Length == 4 && (s[0] == '1' || s[0] == '2'))  //Must be 1xxx or 2xxx
            {
                good = true;
                for (int i2 = 1; i2 < s.Length; i2++)
                {
                    if (!Char.IsDigit(s[i2])) return false;
                }
            }
            return good;
        }

        /// <summary>
        /// Something like 2020, 2020q4, 2020m12. Will not allow 3020 or 4020 etc. but will
        /// allow 2020q9 or 2020m99. But these still "look like dates".
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool LooksLikeYearOrQuarterOrMonth(string s)
        {
            int start1 = -12345;
            int end1 = -12345;
            if (!(s[0] == '1' || s[0] == '2')) return false; //Must be 1xxx or 2xxx
            if (s.Length == 4)
            {
                if (!G.AreDigits(s, 1, 3)) return false;
                return true;
            }
            else if (s.Length == 6)
            {
                if (!(s[4] == 'q' || s[4] == 'Q' || s[4] == 'm' || s[4] == 'M')) return false;
                if (!G.AreDigits(s, 1, 3)) return false;
                if (!G.AreDigits(s, 5, 5)) return false;
                return true;
            }
            else if (s.Length == 7)
            {
                if (!(s[4] == 'm' || s[4] == 'M')) return false;
                if (!G.AreDigits(s, 1, 3)) return false;
                if (!G.AreDigits(s, 5, 6)) return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks in string if index chars from i1 to i2 (both inclusive) are all 0..9. No bounds checks, beware.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static bool AreDigits(string s, int i1, int i2)
        {
            for (int i = i1; i <= i2; i++)
            {
                if (!Char.IsDigit(s[i])) return false;
            }
            return true;
        }

        public static bool LooksLikeQuarter(string s)
        {
            bool good = false;
            if (s.Length == 6 && (s[0] == '1' || s[0] == '2'))  //Must be 1xxxqx or 2xxxqx
            {
                good = true;
                for (int i2 = 1; i2 < 4; i2++)
                {
                    if (!Char.IsDigit(s[i2])) return false;
                }
                if (s[4] != 'q' && s[4] != 'Q') return false;
                else if (s[5] != '1' && s[5] != '2' && s[5] != '3' && s[5] != '4') return false;
            }
            return good;
        }

        /// <summary>
        /// This ought to be a bullet proof way to find the path of Gekko.exe, regardless of called from
        /// Python, Excel, etc. etc.
        /// </summary>
        /// <returns></returns>
        public static string GekkoExePath()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        /// <summary>
        /// For developer use/debugging. Will also be true if ExcelDna or hiding GUI is active.
        /// Beware: It seems the when running unit tests, Globals.runningOnTTComputer is false.
        /// </summary>
        public static bool IsUnitTestingOrNotShowingGUI()
        {
            if (Globals.batchType == EBatchType.Hide || Globals.batchType == EBatchType.Gekcel || Globals.batchType == EBatchType.PyGekko) return true;
            if (IsUnitTesting()) return true;
            return false;
        }

        /// <summary>
        /// True when unit testing
        /// </summary>
        /// <returns></returns>
        public static bool IsUnitTesting()
        {
            return Application.ExecutablePath.Contains("testhost.net48.exe") || Application.ExecutablePath.Contains("testhost.x86.exe") || Application.ExecutablePath.Contains("vstesthost.exe") || Application.ExecutablePath.Contains("QTAgent32_40.exe") || Application.ExecutablePath.Contains("QTAgent32.exe") || Application.ExecutablePath.Contains("vstest.executionengine.x86.exe") || Application.ExecutablePath.Contains("testhost.exe");
        }

        /// <summary>
        /// This is the "real" method actually doing the printing
        /// </summary>
        public static void WriteAbstract2(Object o)
        {
            Program.WorkerThreadHelper2 helper = (Program.WorkerThreadHelper2)o;
            
            Color color = helper.color;
            string s = helper.s;            
            string linktype = helper.linktype;
            bool newline = helper.newline;
            bool link = helper.link;
            ETabs tab = helper.tab;
            bool mustScrollToEnd = helper.mustScrollToEnd;

            if (s.Contains(Globals.linkActionStart)) // Use GekkoAction class instead
            {
                LinkAction action = FindAction(s); 
                if (action != null)
                {
                    Program.WorkerThreadHelper2 helper1 = helper.Clone();
                    helper1.s = action.chop1;
                    helper1.newline = false;
                    helper1.mustScrollToEnd = false;
                    helper1.parentOfAll = helper.parentOfAll;  //first time the line is split regarding links
                    WriteAbstract2(helper1);

                    Program.WorkerThreadHelper2 helper2 = helper.Clone();
                    helper2.newline = false;
                    helper2.link = true;
                    helper2.linktype = "action:" + action.ss2[1];
                    helper2.s = action.ss2[0];
                    helper2.mustScrollToEnd = false;
                    WriteAbstract2(helper2);
                    
                    Program.WorkerThreadHelper2 helper3 = helper.Clone();
                    helper3.s = action.chop3;
                    WriteAbstract2(helper3);

                    return;
                }

                for (int i = 0; i < s.Length; i++)
                {
                    if (s.Substring(i, Globals.linkActionStart.Length) == Globals.linkActionStart)
                    {
                        for (int j = 0; i + 1 < s.Length; j++)
                        {
                            if (s.Substring(j, Globals.linkActionEnd.Length) == Globals.linkActionEnd)
                            {
                                // Use GekkoAction class instead

                                int start = i + Globals.linkActionStart.Length;
                                int end = j;
                                string chop1 = s.Substring(0, start - Globals.linkActionStart.Length);
                                string chop2 = s.Substring(start, end - start);
                                string chop3 = s.Substring(end + Globals.linkActionEnd.Length, s.Length - end - Globals.linkActionEnd.Length);
                                string[] ss2 = chop2.Split(Globals.linkActionDelimiter);

                                Program.WorkerThreadHelper2 wh1 = helper.Clone();
                                wh1.s = chop1;
                                wh1.newline = false;
                                wh1.mustScrollToEnd = false;
                                WriteAbstract2(wh1);

                                Program.WorkerThreadHelper2 wh2 = helper.Clone();
                                wh2.newline = false;
                                wh2.link = true;
                                wh2.linktype = "action:" + ss2[1];
                                wh2.s = ss2[0];
                                wh2.mustScrollToEnd = false;
                                WriteAbstract2(wh2);

                                Program.WorkerThreadHelper2 wh3 = helper.Clone();
                                wh3.s = chop3;
                                WriteAbstract2(wh3);

                                return;
                            }
                        }
                    }
                }
                return;
            }

            RichTextBoxEx textBox = null;

            bool isMuting = false; if (G.Equal(Program.options.interface_mute, "yes")) isMuting = true;
            bool mustAlsoPrintOnScreen = helper.mustAlsoPrintToScreen;
            if (Globals.pipe.echo && !isMuting) mustAlsoPrintOnScreen = true; //Will always trump unless muting, see also #6356d83kpp

            if (helper.type == EWrapType.Error)
            {
                mustAlsoPrintOnScreen = true;  //so we get an error on screen even if piping or muting
                if (Globals.errorMemory == null) Globals.errorMemory = new StringBuilder();
            }
            else if (helper.type == EWrapType.Warning) mustAlsoPrintOnScreen = true;  //so we get an error on screen even if piping

            if (Globals.errorMemory != null)
            {
                //used in stack trace error message
                if (newline)
                {
                    Globals.errorMemory.AppendLine(s);  
                }
                else
                {
                    Globals.errorMemory.Append(s);  
                }
            }

            bool isPiping = false;            
            
            //Not piping to normal pipe file if there is a pipe to pipe2-file (eg. for "p fy file=output.txt")
            if (!Globals.pipe2 && Globals.pipe.isPiping && Globals.pipeFileHelper.pipeFile != null)
            {
                try
                {
                    //if (Globals.pipeFileHelper.isPiping = true)  //this can be false with PIPE<pause>, and set with PIPE<continue>
                    {
                        isPiping = true;
                        try
                        {
                            if (!isMuting) //will also mute in the pipe file
                            {
                                if (newline) Globals.pipeFileHelper.pipeFile.WriteLine(s);
                                else Globals.pipeFileHelper.pipeFile.Write(s);
                            }
                        }
                        catch (IOException)
                        {
                            MessageBox.Show("*** ERROR: I-o Problem with writing a line to pipe file");
                            throw;
                        }
                        Globals.pipeFileHelper.pipeFile.Flush();  ////#80435243075235 flushing turned off here
                    }
                }
                catch (Exception e)
                {
                    //#80435243075235
                    MessageBox.Show("*** ERROR: Could not PIPE to file: " + Globals.pipeFileHelper.pipeFileFileWithPath);                    
                    throw new GekkoException();
                }
            }

            //Always piping here if pipe2 is on
            if (Globals.pipe2 && Globals.pipeFileHelper2.pipeFile != null)
            {
                try
                {
                    isPiping = true;
                    if (!isMuting)
                    {
                        if (newline) Globals.pipeFileHelper2.pipeFile.WriteLine(s);
                        else Globals.pipeFileHelper2.pipeFile.Write(s);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("*** ERROR: Could not PIPE to file: " + Globals.pipeFileHelper2.pipeFileFileWithPath);
                    throw new GekkoException();
                }
            }            

            if (!(isPiping || isMuting) || mustAlsoPrintOnScreen)
            {                
                if (G.IsUnitTestingOrNotShowingGUI())
                {
                    if (newline)
                    {
                        if (Globals.batchType == EBatchType.PyGekko)
                        {
                            if (Globals.pyGekkoStdout)
                            {
                                Console.WriteLine(s);                                
                            }
                            else
                            {
                                if (Globals.gekkoOutputRecorder != null) Globals.gekkoOutputRecorder.AppendLine(s);
                            }
                        }
                        else if (Globals.batchType == EBatchType.Gekcel || Globals.batchType == EBatchType.Hide)
                        {
                            if (Globals.gekkoOutputRecorder != null) Globals.gekkoOutputRecorder.AppendLine(s);
                        }
                        else
                        {
                            Globals.unitTestScreenOutput.AppendLine(s);
                            System.Diagnostics.Debug.WriteLine(s);
                        }
                    }
                    else
                    {
                        if (Globals.batchType == EBatchType.PyGekko)
                        {
                            if (Globals.pyGekkoStdout)
                            {
                                Console.Write(s);                                
                            }
                            else
                            {
                                if (Globals.gekkoOutputRecorder != null) Globals.gekkoOutputRecorder.Append(s);
                            }
                        }
                        else if (Globals.batchType == EBatchType.Gekcel || Globals.batchType == EBatchType.Hide)
                        {
                            if (Globals.gekkoOutputRecorder != null) Globals.gekkoOutputRecorder.Append(s);
                        }
                        else
                        {
                            Globals.unitTestScreenOutput.Append(s);
                            System.Diagnostics.Debug.Write(s);
                        }
                    }
                }
                else
                {                    
                    if (tab == ETabs.Main) textBox = Gui.gui.textBoxMainTabUpper;
                    else if (tab == ETabs.Output) textBox = Gui.gui.textBoxOutputTab;
                    else throw new GekkoException();

                    int start = textBox.TextLength;
                    if (newline)
                    {
                        //has newline
                        if (link)
                        {
                            MessageBox.Show("*** ERROR: link with newline not supported");
                            throw new GekkoException();
                        }
                        else
                        {
                            WriteAbstractHelper(helper.type, helper.parentOfAll, s, textBox, true); //Globals.guiMainLinePosition is changed here                                     
                            if (tab == ETabs.Main || mustScrollToEnd) Gui.gui.ScrollToEnd(textBox);
                        }
                    }
                    else
                    {
                        //does not have newline
                        if (link)
                        {
                            //see Gui.textBox1_LinkClicked                        
                            textBox.InsertLink(s, linktype);
                            Globals.guiMainLinePosition += s.Length;
                        }
                        else
                        {
                            WriteAbstractHelper(helper.type, helper.parentOfAll, s, textBox, false); //Globals.guiMainLinePosition is changed here                        
                        }
                    }
                    int end = textBox.TextLength;

                    if (link == false)
                    {
                        //set color etc.

                        if (helper.type == EWrapType.Error) color = Color.Red;  //overrides any color given
                        else if (helper.type == EWrapType.Warning) color = Globals.warningColor;  //overrides any color given

                        textBox.Select(start, end - start);
                        {
                            textBox.SelectionColor = color; //could set box.SelectionBackColor, box.SelectionFont too.                            
                        }
                        textBox.SelectionLength = 0; // clear     
                        textBox.SelectionStart = end;
                    }
                }
            }
            if (helper.type == EWrapType.Error) throw new GekkoException();  //so that we do not have to do this manually after printing an error.
        }

        /// <summary>
        /// Used for TABLE links
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ExtraLinkLength(string s)
        {
            //see also #jkahfdasify7 
            int extra = 0;
            string sRest = s;

            LinkAction action = FindAction(s);  // Use GekkoAction class instead
            if (action != null)
            {
                extra += Globals.linkActionStart.Length;
                extra += 1; // Globals.linkActionDelimiter
                extra += action.ss2[1].Length;
                extra += Globals.linkActionEnd.Length;
                
                extra += ExtraLinkLength(action.chop3);
            }         

            return extra;
        }

        /// <summary>
        /// Used for GekkoAction links
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static LinkAction FindAction(string s)
        {
            //Will only return the first link found
            LinkAction action = null;
            for (int i = 0; i < s.Length - Globals.linkActionEnd.Length; i++)
            {
                if (s.Substring(i, Globals.linkActionStart.Length) == Globals.linkActionStart)
                {
                    for (int j = 0; i + 1 < s.Length; j++)
                    {
                        if (s.Substring(j, Globals.linkActionEnd.Length) == Globals.linkActionEnd)
                        {
                            action = new LinkAction();
                            action = new LinkAction();
                            action.start = i + Globals.linkActionStart.Length;
                            action.end = j;
                            action.chop1 = s.Substring(0, action.start - Globals.linkActionStart.Length);
                            action.chop2 = s.Substring(action.start, action.end - action.start);
                            action.chop3 = s.Substring(action.end + Globals.linkActionEnd.Length, s.Length - action.end - Globals.linkActionEnd.Length);
                            action.ss2 = action.chop2.Split(Globals.linkActionDelimiter);                            
                            return action;
                        }
                    }
                }
            }
            return action;  //will be null
        }        
        
        /// <summary>
        /// Low-level part of printing on screen.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentOfAll"></param>
        /// <param name="s"></param>
        /// <param name="textBox"></param>
        /// <param name="newline"></param>
        private static void WriteAbstractHelper(EWrapType type, bool parentOfAll, string s, RichTextBoxEx textBox, bool newline)
        {
            //not sure exactly how this code works in all details, but it has been battle-tested a lot,
            //and is probably robust.

            //Should be redone properly for WPF, with tokenizer

            string start = "";  //this is the blank-indent used on each line except the first.
            if (type == EWrapType.Error)
            {
                if (parentOfAll) start = Globals.errorString;
                else start = G.Blanks(Globals.errorString.Length);
                Globals.numberOfErrors++;
            }
            else if (type == EWrapType.Warning)
            {
                if (parentOfAll) start = Globals.warningString;
                else start = G.Blanks(Globals.warningString.Length);
                Globals.numberOfWarnings++;
            }

            while (s != null)
            {
                int colPosition = Globals.guiMainLinePosition;                   //Often 0. Globals.guiMainLinePosition handles some inserts (like links) that there must be room for. But it also handles G.Write() without newline, so that this position is remembered (and no blank-indent is inserted)
                int indent2 = 0; if (colPosition == 0) indent2 = start.Length;   //indent2 is 11 if start = "* * * ERROR" and we are after a newline.
                string start2 = start; if (indent2 == 0) start2 = "";            //hmm
                int rest = Program.options.print_width - colPosition - indent2;
                int restRemember = rest;
                if (rest < 0) rest = 0;

                if (s.Length <= rest)  //easy: we can fit what we are going to write inside the right margin
                {
                    if (newline)
                    {
                        textBox.AppendText(start2 + s + G.NL);
                        Globals.guiMainLinePosition = 0;
                    }
                    else
                    {
                        textBox.AppendText(start2 + s);
                        Globals.guiMainLinePosition += indent2 + s.Length;
                    }
                    s = null;
                }
                else
                {
                    //wrapping
                    for (int c = s.Length - 1; c >= 0; c--)
                    {
                        if (c == 0)
                        {
                            string extra = null;
                            if (newline) extra = G.NL;

                            //Now we are hacking!
                            //We are pretty sure that this does not contain any links.
                            //but it may be too long, like 400 chars rather than < 100.
                            //it will happen pretty seldom, probably only when a link is right at
                            //the right margin.
                                                        
                            while (true)
                            {
                                if (Program.options.print_width - start.Length - s.Length < 0)
                                {                                    
                                    string s1 = s.Substring(0, Program.options.print_width - start.Length);
                                    s = G.Substring(s, Program.options.print_width - start.Length + 1 - 1, s.Length - 1);
                                    s1 = s1.TrimStart();
                                    textBox.AppendText(G.NL + start + s1);
                                    Globals.guiMainLinePosition = start.Length + s1.Length;
                                    continue;
                                }
                                else
                                {
                                    s = s.TrimStart();
                                    textBox.AppendText(G.NL + start + s + extra);
                                    Globals.guiMainLinePosition += s.Length;
                                }
                                break;
                            }
                            
                            if (newline) Globals.guiMainLinePosition = 0;
                            else Globals.guiMainLinePosition = start.Length + s.Length;
                            s = null;
                            break;
                        }
                        if (s.Substring(c, 1) == " ")
                        {
                            if (c <= rest)
                            {
                                string s1 = s.Substring(0, c + 1);
                                s = s.Substring(c + 1, s.Length - c - 1);
                                textBox.AppendText(start2 + s1 + G.NL);  //If newline=false, we impose a newline anyway                                
                                Globals.guiMainLinePosition = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }        

        /// <summary>
        /// Is it a full path like 'c:\xx\yy.zz'? With drive letter and colon (localhost ok too).
        /// This method is probably not completely watertight.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAbsolutePath(string input)
        {
            if (G.NullOrBlanks(input)) return false;
            return input.Trim().Contains(":") || input.Trim().StartsWith("\\\\localhost\\", StringComparison.OrdinalIgnoreCase);
            //for .NET Core use: return Path.IsPathRooted(input) && Path.IsPathFullyQualified(input);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln()
        {
            WriteAbstract(EWrapType.Writeln, "", null, true, Color.Empty, false, ETabs.Main);
        }
        
        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln(ETabs tab)
        {
            WriteAbstract(EWrapType.Writeln, "", null, true, Color.Empty, false, tab);
        }
        
        /// <summary>
        /// For writing output to screen (with line feed)
        /// </summary>
        /// <param name="x"></param>
        public static void Writeln(int x)
        {
            WriteAbstract(EWrapType.Writeln, x.ToString(), null, true, Color.Empty, false, ETabs.Main);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln(int x, ETabs tab)
        {
            WriteAbstract(EWrapType.Writeln, x.ToString(), null, true, Color.Empty, false, tab);
        }

        /// <summary>
        /// For writing output to screen (with line feed)
        /// </summary>
        /// <param name="x"></param>
        public static void Writeln(double x)
        {
            WriteAbstract(EWrapType.Writeln, x.ToString(), null, true, Color.Empty, false, ETabs.Main);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln(double x, ETabs tab)
        {
            WriteAbstract(EWrapType.Writeln, x.ToString(), null, true, Color.Empty, false, tab);
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static bool FilenameIncludesPath(string filename)
        {
            return filename.Contains(":") || filename.Contains("\\");
        }

        /// <summary>
        /// Set color of a section of text in a RichTextBox
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="textLengthStart"></param>
        /// <param name="color"></param>
        public static void PrintLowLevelSetColor(RichTextBoxEx textBox, int textLengthStart, Color color)
        {
            textBox.Select(textLengthStart, textBox.TextLength);
            textBox.SelectionColor = color;
        }

        /// <summary>
        /// Helper method for adding text to the GUI. Not intended for use outside of Wrap.cs.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="s"></param>
        public static void PrintLowLevelAppendText(RichTextBox textBox, string s, EWrapType type, bool mustAlsoPrintOnScreen)
        {
            G.PrintLowLevelAppendTextAbstract(textBox, s, null, type, mustAlsoPrintOnScreen);  //no link
        }

        /// <summary>
        /// Main method for adding text to the GUI. The idea is that -- in the longer run -- all change to GUI text runs through
        /// this method (at the moment, only Wrap text does this). See also Gui.gui.LinkClicked().
        /// Not intended for use outside of Wrap.cs.
        /// </summary>
        /// <param name="textBox">The GUI text box</param>
        /// <param name="s">String to show</param>
        /// <param name="link">Link url, else null if no link</param>
        public static void PrintLowLevelAppendTextAbstract(RichTextBox textBox, string s, string link, EWrapType type, bool mustAlsoPrintOnScreen)
        {            
            if (type == EWrapType.Error || type == EWrapType.Warning) mustAlsoPrintOnScreen = true;  //so that errors and warnings get seen. The argument can be true if set in Writeln(...)

            bool isMuting = false;
            if (G.Equal(Program.options.interface_mute, "yes")) isMuting = true;

            bool isPiping = false;
            isPiping = AppendTextMaybePipe(s, isMuting, isPiping);


            if (!(isPiping || isMuting) || mustAlsoPrintOnScreen)
            {                
                if (G.IsUnitTestingOrNotShowingGUI())
                {
                    if (Globals.batchType == EBatchType.PyGekko)
                    {
                        if (Globals.pyGekkoStdout)
                        {
                            Console.Write(s);                            
                        }
                        else
                        {
                            if (Globals.gekkoOutputRecorder != null) Globals.gekkoOutputRecorder.Append(s);
                        }
                    }
                    else if (Globals.batchType == EBatchType.Gekcel || Globals.batchType == EBatchType.Hide)
                    {
                        if (Globals.gekkoOutputRecorder != null) Globals.gekkoOutputRecorder.Append(s);
                    }
                    else
                    {
                        Globals.unitTestScreenOutput.Append(s);
                        System.Diagnostics.Debug.Write(s);
                    }
                }
                else
                {
                    //not piping, not muting, not ExcelDna'ing, not unit testing, not pythoning --> normal printing
                    if (link == null)
                    {
                        textBox.AppendText(s);
                    }
                    else
                    {
                        RichTextBoxEx textBoxEx = textBox as RichTextBoxEx;
                        if (textBoxEx == null) MessageBox.Show("*** ERROR: Cannot use links in this RichTextBox");
                        int position = textBoxEx.SelectionStart;                        
                        //This is apparently a hack
                        textBoxEx.SelectedRtf = @"{\rtf1\ansi " + s + @"\v #" + link + @"\v0}";                        
                        textBoxEx.Select(position, s.Length + link.Length + 1);                        
                        textBoxEx.SetSelectionLink(true);
                        textBoxEx.Select(position + s.Length + link.Length + 1, 0);
                    }
                }
            }            

            if (Globals.errorMemory != null)
            {
                //used in stack trace error message  
                //ok to record this even if piping, muting, etc.
                Globals.errorMemory.Append(s);
            }
        }

        private static bool AppendTextMaybePipe(string s, bool isMuting, bool isPiping)
        {
            //Not piping to normal pipe file if there is a pipe to pipe2-file (eg. for "p fy file=output.txt")
            if (!Globals.pipe2 && Globals.pipe.isPiping && Globals.pipeFileHelper.pipeFile != null)
            {
                isPiping = true;
                try
                {
                    if (!isMuting) //will also mute in the pipe file
                    {
                        Globals.pipeFileHelper.pipeFile.Write(s);
                    }
                    Globals.pipeFileHelper.pipeFile.Flush();  ////#80435243075235 flushing turned off here
                }
                catch (Exception e)
                {
                    //#80435243075235
                    MessageBox.Show("*** ERROR: Could not PIPE to file: " + Globals.pipeFileHelper.pipeFileFileWithPath);
                    throw new GekkoException();
                }
            }

            //Always piping here if pipe2 is on
            if (Globals.pipe2 && Globals.pipeFileHelper2.pipeFile != null)
            {
                try
                {
                    isPiping = true;
                    if (!isMuting)
                    {
                        Globals.pipeFileHelper2.pipeFile.Write(s);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("*** ERROR: Could not PIPE to file: " + Globals.pipeFileHelper2.pipeFileFileWithPath);
                    throw new GekkoException();
                }
            }

            return isPiping;
        }

        public static double ArithmeticsPower2(double x1, double x2)
        {
            if (x2 == Globals.eps)
            {
                if (x1 == Globals.eps) return double.NaN; //eps ^ eps
                else return double.NaN; //eps ^ x1
            }
            else
            {
                if (x1 == Globals.eps) return double.NaN; //x2 ^ eps
                else return Math.Pow(x2, x1); //x2 ^ x1
            }
        }

        public static double ArithmeticsPower(double x1, double x2)
        {
            //More restrictive here than using 0, to avoid surprises
            //Could be loosened later on.
            if (x1 == Globals.eps)
            {
                if (x2 == Globals.eps) return double.NaN; //eps ^ eps
                else return double.NaN; //eps ^ x2
            }
            else
            {
                if (x2 == Globals.eps) return double.NaN; //x1 ^ eps
                else return Math.Pow(x1, x2); //x1 ^ x2
            }
        }

        public static double ArithmeticsDivide2(double x1, double x2)
        {
            if (x2 == Globals.eps)
            {
                if (x1 == Globals.eps) return double.NaN; //eps / eps
                else return Globals.eps;  //eps / x1
            }
            else
            {
                if (x1 == Globals.eps) return double.NaN; //x2 / eps
                else return x2 / x1; //x2 / x1
            }
        }

        public static double ArithmeticsDivide(double x1, double x2)
        {
            if (x1 == Globals.eps)
            {
                if (x2 == Globals.eps) return double.NaN; //eps / eps
                else return Globals.eps;  //eps / x2
            }
            else
            {
                if (x2 == Globals.eps) return double.NaN; //x1 / eps
                else return x1 / x2; //x1 / x2
            }
        }

        public static double ArithmeticsMultiply2(double x1, double x2)
        {
            if (x2 == Globals.eps)
            {
                if (x1 == Globals.eps) return Globals.eps; //eps * eps
                else return Globals.eps;  //eps * x1
            }
            else
            {
                if (x1 == Globals.eps) return Globals.eps; //x2 * eps
                else return x2 * x1; //x2 * x1
            }
        }

        public static double ArithmeticsMultiply(double x1, double x2)
        {
            if (x1 == Globals.eps)
            {
                if (x2 == Globals.eps) return Globals.eps; //eps * eps
                else return Globals.eps;  //eps * x2
            }
            else
            {
                if (x2 == Globals.eps) return Globals.eps; //x1 * eps
                else return x1 * x2; //x1 * x2
            }
        }

        public static double ArithmeticsSubtract2(double x1, double x2)
        {
            if (x2 == Globals.eps)
            {
                if (x1 == Globals.eps) return Globals.eps; //eps - eps
                else return -x1;  //eps - x1
            }
            else
            {
                if (x1 == Globals.eps) return x2; //x2 - eps
                else return x2 - x1; //x2 - x1
            }
        }

        public static double ArithmeticsSubtract(double x1, double x2)
        {
            if (x1 == Globals.eps)
            {
                if (x2 == Globals.eps) return Globals.eps; //eps - eps
                else return -x2;  //eps - x2
            }
            else
            {
                if (x2 == Globals.eps) return x1; //x1 - eps
                else return x1 - x2; //x1 - x2
            }
        }

        public static double ArithmeticsAdd2(double x1, double x2)
        {
            if (x2 == Globals.eps)
            {
                if (x1 == Globals.eps) return Globals.eps; //eps + eps
                else return x1;  //eps + x1
            }
            else
            {
                if (x1 == Globals.eps) return x2; //x2 + eps
                else return x2 + x1; //x2 + x1
            }
        }

        public static double ArithmeticsAdd(double x1, double x2)
        {
            if (x1 == Globals.eps)
            {
                if (x2 == Globals.eps) return Globals.eps; //eps + eps
                else return x2;  //eps + x2
            }
            else
            {
                if (x2 == Globals.eps) return x1; //x1 + eps
                else return x1 + x2; //x1 + x2
            }
        }

        public static double ArithmeticsNegate(double x1)
        {
            if (x1 == Globals.eps) return Globals.eps;
            else return -x1;
        }

        public static double ArithmeticsTanh(double x1)
        {
            if (x1 == Globals.eps) return Globals.eps;
            else return Math.Tanh(x1);
        }

        public static double ArithmeticsSqrt(double x1)
        {
            if (x1 == Globals.eps) return Globals.eps;
            else return Math.Sqrt(x1);
        }

        public static double ArithmeticsExp(double x1)
        {
            if (x1 == Globals.eps) return 1d;
            else return Math.Exp(x1);
        }

        public static double ArithmeticsLog(double x1)
        {
            if (x1 == Globals.eps) return double.NaN;
            else return Math.Log(x1);
        }

    }    
}
