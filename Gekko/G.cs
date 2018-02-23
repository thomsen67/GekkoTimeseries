/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

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
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using Microsoft.Win32;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Gekko
{
    /// <summary>
    /// Class containing a library of functions used in many places
    /// </summary>
    public class G {

        public static string NL = "\r\n";

        /// <summary>
        /// Compares two strings, ignoring case (so "aBc" == "Abc").
        /// </summary>
        /// <param name="s1">First string</param>
        /// <param name="s2">Second string</param>
        /// <returns>True if equal</returns>
        public static bool equal(string s1, string s2)
        {
            //s1 or s2 may be null
            return (string.Compare(s1, s2, true) == 0);  //true for ignoreCase                
        }

        public static string Equal(string s1, List<string> s3)
        {
            string rv = null;
            foreach (string s2 in s3)
            {
                if (string.Compare(s1, s2, true) == 0)  //true for ignoreCase                
                {
                    rv = s2;
                    break;
                }
            }
            return rv;
        }

        /// <summary>
        /// Search for string inside string (case-insensitive)
        /// </summary>
        /// <param name="s1">String to search (e.g. 'peartree')</param>
        /// <param name="s2">Sub-string to search for (e.g. 'tree')</param>
        /// <returns>True if match</returns>
        public static bool Contains(string s1, string s2)
        {
            return s1.IndexOf(s2, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        //NOTE: only deals with ints like 123, 007, 5. No minus, delimiters etc.!
        //Returns -12345 if s is not such an integer
        public static int IntParse(string s)
        {
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

        
        public static string TrueFalse(bool x) {
            string s = "false";
            if (x) s = "true";
            return s;
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
        
        //TODO: should be done as a struct
        /// <summary>
        /// Extracts "fY" and "-2" from "fY¤-2"
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
        public static string FromBNumberToVarname(int i)
        {
            string culprit;
            EquationHelper eh = (EquationHelper)Program.model.equations[Program.model.m2.fromBNumberToEqNumber[i]];
            culprit = eh.lhsWithLagIndicator;
            return culprit;
        }
        
        //will include lag indicator (¤)
        public static string FromBNumberToVarname2(int i)
        {
            string culprit = Program.model.varsBTypeInverted[i];            
            return culprit;
        }

        /// Converts "fY¤-2" into "fY(-2)"
        public static string FormatVariableAndLag(string varName)
        {
            string variable = null;
            int lag = 0;
            G.ExtractVariableAndLag(varName, out variable, out lag);
            variable = G.PrettifyTimeseriesHash(variable, true, false);
            if (lag != 0) variable += "[" + lag + "]";
            return variable;
        }

        /// Extracts "fY" and "-2" from "fY¤-2"
        /// Extracts "fY" and "¤2000" from "fY¤¤2000"
        public static void ExtractVariableAndLag(string key, out string variable, out string lag)
        {
            //NOTE: NO parsing of the lag as integer here!
            //NOTE: OK with this: some vars are of this type: fy¤¤2001q3 = absolute time
            int indx = key.IndexOf(Globals.lagIndicator);
            variable = key.Substring(0, indx - 0);
            lag = key.Substring(indx + 1, key.Length - (indx + 1));            
        }        

        public static EFreq GetFreq(string freq)
        {
            EFreq eFreq = EFreq.Annual;
            if (G.equal(freq, "a"))
            {
                //do nothing
            }
            else if (G.equal(freq, "q"))
            {
                eFreq = EFreq.Quarterly;
            }
            else if (G.equal(freq, "m"))
            {
                eFreq = EFreq.Monthly;
            }
            else if (G.equal(freq, "u"))
            {
                eFreq = EFreq.Undated;
            }
            else
            {
                G.Writeln("*** ERROR regarding frequency: '" + freq + "' not recognized");
                throw new GekkoException();
            }
            return eFreq;
        }

        // #09832752
        public static string RemoveFreqFromKey(string s)
        {
            //This will become faster when A freq has %a attached!
            int i = s.IndexOf(Globals.freqIndicator);
            if (i == 0)
            {
                G.Writeln2("*** ERROR: freq problem");
                throw new GekkoException();
            }
            else if (i > 0)
            {
                return s.Substring(0, i);  //for instance, fy%q --> fy
            }
            return s;  //annual            
        }

        // #09832752
        public static EFreq GetFreqFromKey(string s)
        {
            //This will become faster when A freq has %a attached!
            int i = s.IndexOf(Globals.freqIndicator);
            if (i == 0)
            {
                G.Writeln2("*** ERROR: freq problem");
                throw new GekkoException();
            }
            else if (i > 0)
            {
                return G.GetFreq(s.Substring(i + 1));  //for instance, fy%q --> q --> .Quarterly
            }
            return EFreq.Annual;  //annual            
        }

        public static string GetFreq(EFreq eFreq)
        {
            string freq = "a";
            if (eFreq == EFreq.Annual)
            {
                //do nothing
            }
            else if (eFreq == EFreq.Quarterly)
            {
                freq = "q";
            }
            else if (eFreq == EFreq.Monthly)
            {
                freq = "m";
            }
            else if (eFreq == EFreq.Undated)
            {
                freq = "u";
            }
            else
            {
                G.Writeln2("*** ERROR regarding frequency: '" + eFreq.ToString() + "' not recognized");
                throw new GekkoException();
            }
            return freq;
        }

        /// <summary>
        /// Extracts "fY" from "fY¤-2"
        /// </summary>
        /// <param name="key">Input</param>        
        public static string ExtractOnlyVariableIgnoreLag(string key)
        {
            return ExtractOnlyVariableIgnoreLag(key, Globals.lagIndicator);
        }

        public static string ExtractOnlyVariableIgnoreLag(string key, string code)
        {
            string variable = null;
            int indx = key.LastIndexOf(code); //in decomp window, we may have x['a', 'z'][-1], so therefore we look for the last '['       
            if (indx != -1)
            {
                string rest = key.Substring(indx);
                if (rest.Contains("'") || rest.Contains(Globals.symbolList.ToString())) variable = key;  //if input is x['a', 'z'] or x[#i, #j], etc.
                else variable = key.Substring(0, indx - 0);
            }
            else variable = key;            
            return variable;
        }

        public static bool IsEnglishLetter(char c)
        {
            //Problem is that char.IsLetter accepts æøå and others
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        //Maybe allowTurtle should be removed
        private static bool IsSimpleToken(string varName, bool allowTurtle)
        {
            //must be like a38, f16, var2, _var3, x_y etc. Cannot start with digit.
            if (varName == null) return false;
            if (varName.Length == 0) return false;
            if (!G.IsLetterOrUnderscore(varName[0])) return false;
            for (int jj = 1; jj < varName.Length; jj++)
            {
                if (!allowTurtle)
                {
                    if (!G.IsLetterOrDigitOrUnderscore(varName[jj]))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!G.IsLetterOrDigitOrUnderscoreOrTurtle(varName[jj]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsSimpleToken(string varName)
        {
            return IsSimpleToken(varName, false);  //no turtle allowed, maybe remove that
        }

        public static string varFormat(string level1, int width)
        {
            return level1 + G.Blanks(width - level1.Length);
        }

        public static string varFormat(string level1)
        {
            return varFormat(level1, 12);
        }

        public static string Seconds(DateTime t0)
        {
            double milliseconds = (DateTime.Now - t0).TotalMilliseconds;
            string s = SecondsFormat(milliseconds);
            return s;
        }

        public static string SecondsFormat(double milliseconds)
        {            
            double total = milliseconds / 1000d;            
            string s = total.ToString("0.00") + " sec";
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

        public static string levelFormat(double level1, int width)
        {            
            string levelFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + width + ":0.0000}", level1);
            if (double.IsNaN(level1)) levelFormatted = Globals.printNaNIndicator;
            return G.Blanks(width - levelFormatted.Length) + levelFormatted;
        }

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

        public static string StringFormat(string input, int width)
        {
            return G.Blanks(width - input.Length) + input;  //right-aligned
        }

        //----------- used in prt statement end -----------------------------------


        public static bool LooksLikeSimpleList(string s)
        {            
            //Should looke like "#mylist2"
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (i == 0 && c != '#') return false;
                if (i == 1 && !(G.IsLetterOrUnderscore(c))) return false;
                if (i >= 2 && !(G.IsLetterOrDigitOrUnderscore(c))) return false;
            }
            return true;
        }        

        public static int CountCharsInString(string source, string cc)
        {            
            int count = 0;
            foreach (char c in source)
                if (c == cc[0]) count++;
            return count;
        }        

        public static string updprtFormat(double level1, int decimals)
        {
            string z = new string('0', decimals);
            string levelFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:0." + z + "}", level1);
            if (double.IsNaN(level1)) levelFormatted = "M";
            return levelFormatted;
        }
        
        public static string levelFormatOld(double level1)
        {
            return levelFormatOld(level1, 14);
        }

        public static string levelFormatOld(double level1, int width)
        {
            int widthM1 = width - 1;
            string levelFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + widthM1 + ":0.0000}", level1);
            if (double.IsNaN(level1)) levelFormatted = Globals.printNaNIndicator;
            return G.Blanks(width - levelFormatted.Length) + levelFormatted;
        }

        public static string IntFormat(int input, int width)
        {
            string formatted = input.ToString();            
            return G.Blanks(width - formatted.Length) + formatted;
        }

        public static string pchFormatOld(double pch1)
        {
            return pchFormatOld(pch1, 8);
        }

        public static string pchFormatOld(double pch1, int width)
        {
            int widthM1 = width - 1;
            int widthM2 = width - 2;
            string pchFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + widthM2 + ":0.00}", pch1);
            if (double.IsNaN(pch1) || pchFormatted.Length > widthM1) pchFormatted = "******";
            return G.Blanks(width - pchFormatted.Length) + pchFormatted;
        }        
        
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

        public static string dlogFormatOld(double input)
        {
            return dlogFormatOld(input, 8);
        }

        public static string dlogFormatOld(double input, int width)
        {
            int widthM1 = width - 1;
            int widthM2 = width - 2;
            string dlogFormatted = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0," + widthM2 + ":0.0000}", input);
            if (double.IsNaN(input) || dlogFormatted.Length > widthM1) dlogFormatted = "******";
            return G.Blanks(width - dlogFormatted.Length) + dlogFormatted;
        }

        public static string Blanks(int count)
        {
            if (count < 0) return "";
            return "".PadLeft(count);
        }

        //only for debugging purposes
        public static string BlanksDebug(int count)
        {
            if (count < 0) return "";
            string s = "";
            for (int i = 0; i < count; i++)
            {
                s = s + "'";
            }
            return s;
        }

        public static String TspUtilityFindToken(List<string> al, List<string> alType, int i, int relativePosition)
        {
            int ii = -12345;
            return TspUtilityFindWord(out ii, 0, al, alType, i, relativePosition);
        }
        public static String TspUtilityFindType(List<string> al, List<string> alType, int i, int relativePosition)
        {
            int ii = -12345;
            return TspUtilityFindWord(out ii, 1, al, alType, i, relativePosition);
        }

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

        public static int TspUtilitiesFindIndex(List<string> al, List<string> alType, int i, int relativePosition)
        {
            int ii = -12345;
            TspUtilityFindWord(out ii, 1, al, alType, i, relativePosition);
            return ii;
        }                

        public static void CloneDatabank(Databank newDatabank, Databank originalDatabank)
        {
            newDatabank.FileNameWithPath = originalDatabank.FileNameWithPath;
            newDatabank.yearStart = originalDatabank.yearStart;
            newDatabank.yearEnd = originalDatabank.yearEnd;
            newDatabank.info1 = originalDatabank.info1;
            newDatabank.date = originalDatabank.date;
            newDatabank.isDirty = true;
            //don't touch alias names: we are cloning the content of the databank, not altering its name.
            foreach (TimeSeries ts in originalDatabank.storage.Values) 
            {
                TimeSeries tsCopy = ts.Clone();
                newDatabank.AddVariable(ts.frequency, tsCopy, false);  //FIXME: what if already there. No variable name check -- just wastes time, and not good regarding GAMS variable names.
            }
        }

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

        //if there is a lag paranthesis starting at pos i
        //   0 1 2 3
        //   ( - 1 )
        //   ( + 1 )                
        //Also handles specific years
        //   0  1   2
        //   ( 2001 )
        //And broken lags, including leads
        //   1 2  3 4        
        //   ( - .5 )        
        //   ( + .5 )        
        //Will not accept this:
        //   ( .25 )
        //   because lags/leads should have +/-, and specific
        //   years cannot be broken.
        //And the above with a plus as well (broken leads)
        //also accepts [-1] (AREMOS)
        // i is located at the first lag parenthesis in fy(-1)

        public static string PrettifyTimeseriesHash(string s, bool isVarName, bool isInverse)
        {
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

                    string s3 = PrettifyTimeseriesHash(s2, false, false);

                    string[] ss = s3.Split(new string[] { "<LAGINDICATOR>" }, StringSplitOptions.None);
                    if (ss.Length == 1) return s1 + "[" + s3 + "]";
                    else
                    {
                        return s1 + "[" + ss[0] + "'][-" + ss[1].Substring(0, ss[1].Length - 1) + "]";
                    }
                }
                else return "'" + s.Replace(Globals.symbolTurtle, "', '") + "'";
            }
        }
        public static int CompareNatural(string strA, string strB)
        {
            return CompareNatural(strA, strB, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase);
        }

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

        public static StringBuilder ExtractTextFromLines(List<string> linesInput)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in linesInput)
            {
                sb.AppendLine(line);
            }            
            return sb;
        }

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

        public static StreamWriter GekkoStreamWriter(FileStream fs2)
        {
            return new StreamWriter(fs2, Encoding.GetEncoding(1252));
        }

        public static bool IsLetterOrDigitOrUnderscore(char c)
        {
            if (G.IsEnglishLetter(c) || char.IsDigit(c) || c == '_')
                return true;
            else return false;
        }

        public static bool IsLetterOrDigit(char c)
        {
            if (G.IsEnglishLetter(c) || char.IsDigit(c))
                return true;
            else return false;
        }

        public static bool IsLetterOrDigitOrUnderscoreOrTurtle(char c)
        {
            if (G.IsEnglishLetter(c) || char.IsDigit(c) || c == '_' || c == '¤')
                return true;
            else return false;
        }

        public static bool IsLetterOrUnderscore(char c)
        {
            if (G.IsEnglishLetter(c) || c == '_')
                return true;
            else return false;
        }

        public static double FromDateToFloating(GekkoTime gt)
        {
            double d = double.NaN;
            if (gt.freq == EFreq.Annual || gt.freq == EFreq.Undated)
            {
                d = gt.super;
            }
            else if (gt.freq == EFreq.Quarterly)
            {
                d = (double)gt.super + ((double)gt.sub - 1d) / 4d;
            }
            else if (gt.freq == EFreq.Monthly)
            {
                d = (double)gt.super + ((double)gt.sub - 1d) / 12d;
            }
            return d;
        }
        
        public static GekkoTime FromStringToDate(string s)
        {
            return FromStringToDate(s, false);
        }

        public static GekkoTime FromStringToDate(string s, bool allowKForQuarters)
        {
            //To do the reverse: see G.FromDateToString()            

            GekkoTime t;

            {
                int i = -12345;
                bool b = int.TryParse(s, out i);
                if (b)
                {
                    //happens often, so we do it fast
                    return new GekkoTime(EFreq.Annual, G.findYear(i), 1);
                }
            }

            if (s.EndsWith("a1", StringComparison.OrdinalIgnoreCase))
            {
                int i = -12345;                
                bool b = int.TryParse(s.Substring(0, s.Length - 2), out i);
                if (b)
                {
                    return new GekkoTime(EFreq.Annual, G.findYear(i), 1);
                }
                else
                {
                    G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                    throw new GekkoException();
                }
            }

            if (s.Contains("q") || s.Contains("Q"))
            {
                try
                {
                    string[] temp1 = s.Split(new char[] { 'q', 'Q' });
                    int y1 = G.findYear(int.Parse(temp1[0]));
                    int q1 = int.Parse(temp1[1]);
                    if (q1 < 1 || q1 > 4)
                    {
                        G.Writeln("*** ERROR: should have quarters from 1 to and including 4");
                        throw new GekkoException();
                    }
                    t = new GekkoTime(EFreq.Quarterly, y1, q1);
                }
                catch (Exception e)
                {
                    G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                    throw new GekkoException();
                }
            }
            else if (allowKForQuarters && (s.Contains("k") || s.Contains("K")))
            {
                try
                {
                    string[] temp1 = s.Split(new char[] { 'k', 'K' });
                    int y1 = G.findYear(int.Parse(temp1[0]));
                    int q1 = int.Parse(temp1[1]);
                    if (q1 < 1 || q1 > 4)
                    {
                        G.Writeln("*** ERROR: should have quarters from 1 to and including 4");
                        throw new GekkoException();
                    }
                    t = new GekkoTime(EFreq.Quarterly, y1, q1);
                }
                catch (Exception e)
                {
                    G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                    throw new GekkoException();
                }
            }
            else if (s.Contains("m") || s.Contains("M"))
            {
                try
                {
                    string[] temp1 = s.Split(new char[] { 'm', 'M' });
                    int y1 = G.findYear(int.Parse(temp1[0]));
                    int m1 = int.Parse(temp1[1]);
                    if (m1 < 1 || m1 > 12)
                    {
                        G.Writeln("*** ERROR: should have months from 1 to and including 12");
                        throw new GekkoException();
                    }
                    t = new GekkoTime(EFreq.Monthly, y1, m1);
                }
                catch (Exception e)
                {
                    G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                    throw new GekkoException();
                }
            }
            else if (s.Contains("u") || s.Contains("U"))  //ttfreq
            {
                string s2 = s;
                if (s.EndsWith("u1", StringComparison.OrdinalIgnoreCase))
                {
                    s2 = s.Substring(0, s.Length - 2);
                }
                try
                {
                    t = new GekkoTime(EFreq.Undated, int.Parse(s2), 1);
                }
                catch (Exception e)
                {
                    G.Writeln("*** ERROR: timeperiod " + s + " not valid");
                    throw new GekkoException();
                }
            }
            else
            {
                G.Writeln2("*** ERROR: Could not understand the timeperiod: " + s);
                throw new GekkoException();
            }
            return t;
        }

        public static int GekkoMin(int i1, int i2) {
            //if both are missing, a missing is returned
            if (i1 == -12345) return i2;
            if (i2 == -12345) return i1;
            return Math.Min(i1, i2);
        }

        public static int GekkoMax(int i1, int i2)
        {
            //if both are missing, a missing is returned
            //with positive inputs, this method is superflous, but we keep it for symmtery reasons (see GekkoMin())
            if (i1 == -12345) return i2;
            if (i2 == -12345) return i1;
            return Math.Max(i1, i2);
        }

        public static void SetWorkingFolder()
        {
            SetWorkingFolder(true);
        }

        public static bool SetWorkingFolder(bool throwException)
        {
            try
            {
                System.IO.Directory.SetCurrentDirectory(Program.options.folder_working);
            }
            catch (Exception e)
            {
                G.Writeln2("*** ERROR: It seems the folder '" + Program.options.folder_working + "' is blocked or does not exist");
                if (throwException) throw new GekkoException();
                else return true;  //problem
            }
            return false;
        }

        public static string GetProgramDir()
        {
            return System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        }
        public static string GetWorkingFolder()
        {
            return System.IO.Directory.GetCurrentDirectory();
        }


        //1.4.9 stuff
        public static string PrintVersion(string version, bool patch)
        {

            try
            {
                string[] versionSplit = version.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (versionSplit.Length == 3)
                {
                    string start = versionSplit[0];
                    string middle = versionSplit[1];
                    string end = versionSplit[2];
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
            if (Globals.isAlphaVersion) version += " ALPHA VERSION - NO GUARANTEES!";
            if (Globals.isBetaVersion) version += " BETA VERSION - NO GUARANTEES!";
            if (Globals.isGammaVersion) version += " GAMMA VERSION - NO GUARANTEES!";
            if (Globals.isPreviewVersion) version += " BETA VERSION - NO GUARANTEES!";
            return version;
        }

        public static void ServiceMessage()
        {
            G.Writeln(Globals.serviceMessage, Color.LightGray);
        }

        //1.4.9 stuff
        public static StringBuilder WriteDirs(string type, bool silent)  //"small" or "large"
        {
            StringBuilder sb = new StringBuilder();
            string workingFolder = GetWorkingFolder();
            sb.AppendLine("==========================================================================");
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

            if (type == "large") sb.AppendLine(" Program folder: ");
            if (type == "large") sb.AppendLine("   " + GetProgramDir());
            sb.AppendLine(" Working folder: ");
            sb.AppendLine("   " + workingFolder);

            if (type == "large")
            {
                sb.AppendLine(" User settings file (for window positions etc.): ");
                sb.AppendLine("   " + Globals.userSettingsPath);
                sb.AppendLine(" Temporary files (cached models, gnuplot data, etc.): ");
                sb.AppendLine("   " + System.Windows.Forms.Application.LocalUserAppDataPath);
                sb.AppendLine(" Excel version installed: Excel " + Program.GetExcelVersion(Program.eOfficeApp.eOfficeApp_Excel));                

                try
                {
                    RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
                    string[] version_names = installed_versions.GetSubKeyNames();
                    //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
                    double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);
                    int SP = Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1]).GetValue("SP", 0));
                    if (SP > 0) sb.AppendLine(" Microsoft .NET version: " + Framework + ", service pack " + SP);
                    else sb.AppendLine(" Microsoft .NET version: " + Framework);
                  
                }
                catch { };  //fail silently

                string file = Application.StartupPath + "\\buildInfo.xml";
                if (File.Exists(file))
                {
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.Load(file);
                    }
                    catch (Exception e)
                    {
                        //silent about this
                    }
                    XmlElement root = doc.DocumentElement;
                    XmlNodeList nodes = root.SelectNodes("/version"); // You can filter elements here using XPath
                    foreach (XmlNode node in nodes)
                    {
                        string revision = node["revision"].InnerText;
                        string revisionRange = node["revisionRange"].InnerText;
                        string revisionTime = node["revisionTime"].InnerText;
                        string modified = node["modified"].InnerText;
                        string buildTime = node["buildTime"].InnerText;
                        string url = node["url"].InnerText;
                        //sb.AppendLine(" SVN repository " + url);
                        //sb.AppendLine(" SVN revision #" + revision + " (" + revisionRange + ")" + " at " + revisionTime + " (" + modified + ")");
                        sb.AppendLine(" Build time of gekko.exe: " + buildTime);
                    }
                }
                

            }
            sb.AppendLine("==========================================================================");
            sb.AppendLine();
            if (!silent)
            {
                int widthRemember = Program.options.print_width;
                int fileWidthRemember = Program.options.print_filewidth;
                Program.options.print_width = int.MaxValue;
                Program.options.print_filewidth = int.MaxValue;
                try
                {
                    List<string> lines = G.ExtractLinesFromText(sb.ToString());
                    foreach (string s2 in lines)
                    {
                        G.Writeln(s2);
                    }
                }
                finally
                {
                    //resetting, also if there is an error
                    Program.options.print_width = widthRemember;
                    Program.options.print_filewidth = fileWidthRemember;
                }

                Program.ShowPeriodInStatusField("");                
            }
            return sb;
        }


        public static String oddX0000Hack(String val)
        {            
            String val1 = val.Replace('\x0000', '?');  //some odd hack regarding this odd character
            return val1;
        }

        public static bool IsSamePath(string path1, string path2)
        {
            return string.Compare(Path.GetFullPath(path1).TrimEnd('\\'), Path.GetFullPath(path2).TrimEnd('\\'), StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public static bool IsMissingVariableArtificialNumber(double val)
        {
            if (val > 2.999e300d && val < 3.001e300d)
                return true;
            else return false;
        }        

        /// <summary>
        /// Checks if input IsInfinity or IsNaN.
        /// </summary>
        /// <param name="f">Input</param>
        /// <returns>True if the input is problematic</returns>
        public static bool isNumericalError(double f)
        {
            if (Double.IsInfinity(f) || Double.IsNaN(f)) return true;
            else return false;
        }

        /// <summary>
        /// 1950 --> 1950.
        /// 50   --> 1950.
        /// 2010 --> 2010.
        /// 110  --> 2010.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int findYear(int x)
        {
            if (x >= 1500 && x <= 3000)
            {
                return x;
            }
            else if (x >= 0 && x <= 199)
            {
                return x + 1900;
            }
            else
            {
                G.Writeln("*** ERROR regarding year");
                throw new GekkoException();
            }
        }

        public static string GetFreqString()
        {            
            return GetFreqString(Program.options.freq);            
        }

        public static string GetFreqString(EFreq input)
        {
            string f = "";
            if ((input == EFreq.Annual))
            {
                f = "Annual";
            }
            else if ((input == EFreq.Quarterly))
            {
                f = "Quarterly";
            }
            else if ((input == EFreq.Monthly))  //ttfreq
            {
                f = "Monthly";
            }
            else if ((input == EFreq.Undated))
            {
                f = "Undated";
            }
            else
            {
                G.Writeln("*** ERROR: strange error regarding freq");
            }

            return f;
        }

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

        public static string FromDateToString(GekkoTime gt)
        {
            //The reverse: see FromStringToDate()
            string s = "";
            s = gt.super.ToString() + G.GetSubPeriodString(gt);
            return s;
        }

        public static string GetSubPeriodString(GekkoTime gt)
        {
            string subend = "";
            if (gt.freq == EFreq.Annual)
            {
            }
            else if (gt.freq == EFreq.Quarterly)
            {
                subend = "q" + gt.sub;
            }
            else if (gt.freq == EFreq.Monthly)
            {
                subend = "m" + gt.sub;
            }
            else if (gt.freq == EFreq.Undated)  //ttfreq
            {
            }
            else throw new GekkoException();
            return subend;
        }

        public static bool IsDotIdent(string bank)
        {
            //generalizes IsIdent(), handles abc.de
            string[] split = bank.Split('.');
            if (split.Length == 1)
            {
                return IsIdent(split[0]);
            }
            else if (split.Length == 2)
            {
                if (split[0].Length > 0 && split[1].Length > 0 && IsIdent(split[0]) && IsIdent(split[1]))
                    return true;
                else return false;
            }
            else return false;
        }
        
        public static string GetVariableType(int n)
        {
            if (n == 1) return "IVariable";            
            else return "GekkoTuple.Tuple" + n;
        }

        public static string ReplaceGlueNew(string s)
        {
            //int length = s.Length;
            //TODO:
            //what about error position if glue stuff is removed?? Do some logic...
            s = s.Replace(Globals.symbolGlueChar1.ToString(), "");
            s = s.Replace(Globals.symbolGlueChar2, "");
            s = s.Replace(Globals.symbolGlueChar3, "");
            s = s.Replace(Globals.symbolGlueChar4, "");
            s = s.Replace(Globals.symbolGlueChar5, "<");  //--> fixme, this is a workaround
            s = s.Replace(Globals.symbolGlueChar6, "[");  //--> fixme, this is a workaround, #098523
            s = s.Replace(Globals.symbolGlueChar7, "[");            
            s = Regex.Replace(s, "s___er", "ser", RegexOptions.IgnoreCase);  //#098275432874
            s = Regex.Replace(s, "s___eries", "series", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, "s____er", "ser", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, "s____eries", "series", RegexOptions.IgnoreCase);

            //s = s.Remove(
            //lengthDiff = length - s.Length;
            return s;
        }        
        
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

        public static bool IsIdentTranslate(string s)
        {
            if (s == null || s == "") return false;
            return IsIdent(s);
        }   

        public static bool IsInteger(string s)
        {            
            foreach (char c in s)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }

        public static bool IsIntegerTranslate(string s)
        {
            if (s == null || s == "") return false;
            return IsInteger(s);
        }      

        public static bool Round(out int rounded, double value)
        {
            //TODO: What about double.NaN????
            bool flag = false;
            rounded = Convert.ToInt32(value);  //this function rounds to nearest int, so -12.98 --> -13
            double decimals = value - rounded;
            if (Math.Abs(decimals) < 0.0001)
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// Converts e.g. "fxo" to "fXo" is a model is loaded
        /// </summary>
        /// <param name="var">Input string</param>
        /// <returns>Output string</returns>
        public static string GetUpperLowerCase(string var)
        {
            if (Program.model != null && Program.model.varsAType != null)
            {
                //a model is loaded
                //ATypeData temp = (ATypeData)Program.model.varsAType[var];
                ATypeData temp = null; Program.model.varsAType.TryGetValue(var, out temp);
                if (temp != null)
                {
                    //var is known from the model
                    var = temp.varName;  //with nicer upper/lowercase.
                }
            }
            return var;
        }

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

        public static void PrintListWithCommas(List<string> list, bool insertLinks)
        {
            PrintListWithCommas(list, insertLinks, false);            
            return;
        }

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

        public static string GetListWithCommas(List<string> list) 
        {
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
        /// For writing output to screen
        /// </summary>
        /// <param name="s"></param>
        public static void Write(string s)
        {
            WriteAbstract(s, null, false, Color.Empty, false, ETabs.Main);            
        }

        public static void Write(string s, ETabs tab)
        {
            WriteAbstract(s, null, false, Color.Empty, false, tab);
        }

        /// <summary>
        /// For writing output to screen in a color
        /// </summary>
        /// <param name="s"></param>
        public static void Write(string s, Color color)
        {
            WriteAbstract(s, null, false, color, false, ETabs.Main);
        }

        public static void Write(string s, Color color, ETabs tab)
        {
            WriteAbstract(s, null, false, color, false, tab);
        }
      
        /// <summary>
        /// For writing output to screen
        /// </summary>
        /// <param name="x"></param>
        public static void Write(int x)
        {
            WriteAbstract(x.ToString(), null, false, Color.Empty, false, ETabs.Main);            
        }

        public static void Write(int x, ETabs tab)
        {
            WriteAbstract(x.ToString(), null, false, Color.Empty, false, tab);
        }
        /// <summary>
        /// For writing output to screen
        /// </summary>
        /// <param name="x"></param>
        public static void Write(double x)
        {
            WriteAbstract(x.ToString(), null, false, Color.Empty, false, ETabs.Main);            
        }

        public static void Write(double x, ETabs tab)
        {
            WriteAbstract(x.ToString(), null, false, Color.Empty, false, tab);
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
            WriteAbstract(text, linktype, false, Color.Empty, true, ETabs.Main);  //Color is not used anyway -- gets blue underlined
        }

        public static void WriteLink(string text, string linktype, ETabs tab)
        {
            //for instance input0 = "output", "tab:output"
            //for instance input0 = "fY", "disp:fY"
            //for instance input0 = "sim", "help:sim"            
            //see Gui.textBox1_LinkClicked
            WriteAbstract(text, linktype, false, Color.Empty, true, tab);  //Color is not used anyway -- gets blue underlined
        }

        //Provides extra blank line
        public static void Writeln2(string s)
        {
            Writeln();
            Writeln(s);
        }

        //Provides extra blank line
        public static void Write2(string s)
        {
            Writeln();
            Write(s);
        }

        //Provides extra blank line
        public static void Writeln2(string s, Color color)
        {
            Writeln();
            Writeln(s, color);
        }

        //Easier to remember G.Writeln()
        /// <summary>
        /// For writing output to screen (with line feed)
        /// </summary>
        /// <param name="s"></param>
        public static void Writeln(string s)
        {
            WriteAbstract(s, null, true, Color.Empty, false, ETabs.Main);
        }

        public static void Writeln(string s, ETabs tab)
        {
            WriteAbstract(s, null, true, Color.Empty, false, tab);
        }
        /// <summary>
        /// For writing output to screen in a color
        /// </summary>
        /// <param name="s"></param>
        public static void Writeln(string s, Color color)
        {
            WriteAbstract(s, null, true, color, false, ETabs.Main);
        }

        public static void Writeln(string s, Color color, bool mustAlsoWriteToScreen)
        {
            WriteAbstractScroll(s, null, true, color, false, ETabs.Main, false, mustAlsoWriteToScreen);
        }

        public static void Writeln(string s, Color color, ETabs tab)
        {
            WriteAbstract(s, null, true, color, false, tab);
        }

        public static void WriteAbstract(string s, string linktype, bool newline, Color color, bool link, ETabs tab)
        {            
            WriteAbstractScroll(s, linktype, newline, color, link, tab, false, false);
        }

        public static void WriteAbstractScroll(string s, string linktype, bool newline, Color color, bool link, ETabs tab, bool mustScrollToEnd, bool mustAlsoPrintToScreen)
        {
            if (Globals.applicationIsInProcessOfDying)
            {
                //if (s.Trim() != "") MessageBox.Show(s);  //this just creates confusion
                return;
            }
            
            Program.WorkerThreadHelper2 wh = new Program.WorkerThreadHelper2();
            if (s == null)
            {
                s = "";
            }
            if (s.Trim().StartsWith("*** ERROR"))
            {
                color = Color.Red;
                Globals.numberOfErrors++;
            }
            else if (s.Trim().StartsWith("+++ WARNING"))
            {
                color = Globals.warningColor;
                Globals.numberOfWarnings++;
            }            
            wh.color = color;
            wh.s = s;
            wh.linktype = linktype;
            wh.newline = newline;
            wh.link = link;
            wh.tab = tab;
            wh.mustScrollToEnd = mustScrollToEnd;
            wh.mustAlsoPrintToScreen = mustAlsoPrintToScreen;
            if (Globals.workerThread == null)
            {
                //typically only just when program starts -- worker thread not yet created
                WriteAbstract2(wh);
            }
            else
            {
                Globals.workerThread.gekkoGui.Invoke(Globals.workerThread.gekkoGui.threadDelegateAddString, new Object[] { wh });
            }
        }

        public static void WritelnGray(string s)
        {
            if (!Globals.runningOnTTComputer) return;
            if (!Globals.printGrayLinesForDebugging) return;
            WriteAbstract(s, null, true, Color.Gray, false, ETabs.Main);
        }

        public static bool IsUnitTesting()
        {
            if ((Application.ExecutablePath.Contains("vstesthost.exe") || Application.ExecutablePath.Contains("QTAgent32.exe") || Application.ExecutablePath.Contains("vstest.executionengine.x86.exe"))) return true;
            else return false;
        }

        public static void WriteAbstract2(Object o)
        {            
            Program.WorkerThreadHelper2 wh = (Program.WorkerThreadHelper2)o;
            Color color = wh.color;
            string s = wh.s;            
            string linktype = wh.linktype;
            bool newline = wh.newline;
            bool link = wh.link;
            ETabs tab = wh.tab;
            bool mustScrollToEnd = wh.mustScrollToEnd;
            
            RichTextBoxEx textBox = null;            

            bool mustAlsoPrintOnScreen = wh.mustAlsoPrintToScreen;            
            
            if (s.Contains("*** ERROR"))
            {
                mustAlsoPrintOnScreen = true;  //so we get an error on screen even if piping
                if (Globals.errorMemory == null) Globals.errorMemory = new StringBuilder();
            }
            if (s.Contains("+++ WARNING")) mustAlsoPrintOnScreen = true;  //so we get an error on screen even if piping

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
            if (!Globals.pipe2 && Globals.pipe && Globals.pipeFileHelper.pipeFile != null)
            {
                try
                {
                    //if (Globals.pipeFileHelper.isPiping = true)  //this can be false with PIPE<pause>, and set with PIPE<continue>
                    {
                        isPiping = true;
                        if (newline) Globals.pipeFileHelper.pipeFile.WriteLine(s);
                        else Globals.pipeFileHelper.pipeFile.Write(s);
                        Globals.pipeFileHelper.pipeFile.Flush();
                    }
                }
                catch (Exception e)
                {
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
                if (newline) Globals.pipeFileHelper2.pipeFile.WriteLine(s);
                else Globals.pipeFileHelper2.pipeFile.Write(s);
                Globals.pipeFileHelper2.pipeFile.Flush();
                }
                catch (Exception e)
                {
                    G.Writeln2("*** ERROR: Could not PIPE to file: " + Globals.pipeFileHelper2.pipeFileFileWithPath);
                    throw new GekkoException();
                }
            }            

            if (!isPiping || mustAlsoPrintOnScreen)
            {
                if (G.IsUnitTesting())
                {
                    if (newline)
                    {
                        Globals.unitTestScreenOutput.AppendLine(s);
                        //Globals.unitTestWindow.WriteLine(s);
                        //Console.Out.WriteLine(s);
                        System.Diagnostics.Debug.WriteLine(s);
                        //System.Diagnostics.Trace.WriteLine(s);
                    }
                    else
                    {
                        Globals.unitTestScreenOutput.Append(s);
                        //Globals.unitTestWindow.Write(s);
                        //Console.Out.Write(s);
                        System.Diagnostics.Debug.Write(s);
                        //System.Diagnostics.Trace.Write(s);
                    }
                }
                else
                {
                    if (tab == ETabs.Main) textBox = Gui.gui.textBox1;
                    else if (tab == ETabs.Output) textBox = Gui.gui.textBoxTab2;
                    else if (tab == ETabs.Help) textBox = Gui.gui.textBoxTab3;
                    else throw new GekkoException();

                    int start = textBox.TextLength;
                    if (newline)
                    {
                        if (link)
                        {
                            G.Writeln("*** ERROR: link with newline not supported");
                            throw new GekkoException();
                        }
                        else
                        {
                            WriteAbstractClipHelper(s, textBox, true); //Globals.guiMainLinePosition is changed here                                     
                            if (tab == ETabs.Main || mustScrollToEnd) Gui.gui.ScrollToEnd(textBox);
                        }
                    }
                    else
                    {
                        if (link)
                        {
                            //see Gui.textBox1_LinkClicked                        
                            textBox.InsertLink(s, linktype);
                            Globals.guiMainLinePosition += s.Length;
                        }
                        else
                        {
                            WriteAbstractClipHelper(s, textBox, false); //Globals.guiMainLinePosition is changed here                        
                        }
                    }
                    int end = textBox.TextLength;

                    if (link == false)
                    {
                        textBox.Select(start, end - start);
                        {
                            textBox.SelectionColor = color; //could set box.SelectionBackColor, box.SelectionFont too.
                            if (color == Color.Red)  //hack, do an abstract method with bold (bold is ugly anyway...)
                            {
                                //Gekko.gui.textBox1.SelectionFont = new Font(Gekko.gui.textBox1.SelectionFont, FontStyle.Bold);
                            }
                        }
                        textBox.SelectionLength = 0; // clear     
                        textBox.SelectionStart = end;
                    }

                    //textBox.SelectionFont = new Font(textBox.SelectionFont, FontStyle.Regular);  //clear
                }
            }

        }

        private static void WriteAbstractClipHelper(string s, RichTextBoxEx textBox, bool newline)
        {
            string NL2 = "\r\n" + Globals.blankUsedAsPadding;
            while (s != null)
            {
                //int pos0 = textBox.GetFirstCharIndexOfCurrentLine();
                //int pos = textBox.SelectionStart - pos0 - 1;  //seems SelectionStart is 1-based
                //int rest = Program.options.print_width - pos;
                
                //Point pt;
                //int line, col, index;
                //// get the current line 
                //index = textBox.SelectionStart; line = textBox.GetLineFromCharIndex(index);
                //// get the caret position in pixel coordinates 
                //pt = textBox.GetPositionFromCharIndex(index);
                //// now get the character index at the start of the line, and 
                //// subtract from the current index to get the column 
                //pt.X = 0;
                //col = index - textBox.GetCharIndexFromPosition(pt);


                int col = Globals.guiMainLinePosition;
                int rest = Program.options.print_width - col;
                if (rest < 0) rest = 0;

                if (s.Length <= rest)
                {

                    if (newline)
                    {
                        textBox.AppendText(s + NL2);
                        Globals.guiMainLinePosition = 0;
                    }
                    else
                    {
                        textBox.AppendText(s);
                        Globals.guiMainLinePosition += s.Length;
                    }
                    s = null;
                }
                else
                {
                    for (int c = s.Length - 1; c >= 0; c--)
                    {
                        if (c == 0)
                        {
                            textBox.AppendText(s + NL2);  //has to write it, even if too long. If newline=false, we impose a newline anyway
                            s = null;
                            Globals.guiMainLinePosition = 0;
                            break;
                        }
                        if (s.Substring(c, 1) == " ")
                        {
                            if (c <= rest)
                            {
                                string s1 = s.Substring(0, c + 1);
                                s = s.Substring(c + 1, s.Length - c - 1);
                                textBox.AppendText(s1 + NL2);  //If newline=false, we impose a newline anyway
                                Globals.guiMainLinePosition = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// For writing output to screen
        /// </summary>
        public static void Writeln()
        {
            WriteAbstract("", null, true, Color.Empty, false, ETabs.Main);
        }

        public static void Writeln(ETabs tab)
        {
            WriteAbstract("", null, true, Color.Empty, false, tab);
        }
        /// <summary>
        /// For writing output to screen (with line feed)
        /// </summary>
        /// <param name="x"></param>
        public static void Writeln(int x)
        {
            WriteAbstract(x.ToString(), null, true, Color.Empty, false, ETabs.Main);
        }

        public static void Writeln(int x, ETabs tab)
        {
            WriteAbstract(x.ToString(), null, true, Color.Empty, false, tab);
        }
        /// <summary>
        /// For writing output to screen (with line feed)
        /// </summary>
        /// <param name="x"></param>
        public static void Writeln(double x)
        {
            WriteAbstract(x.ToString(), null, true, Color.Empty, false, ETabs.Main);
        }

        public static void Writeln(double x, ETabs tab)
        {
            WriteAbstract(x.ToString(), null, true, Color.Empty, false, tab);
        }

        public static bool FilenameIncludesPath(string filename)
        {
            return filename.Contains(":") || filename.Contains("\\");
        }
    }
}
