/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

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

        public enum ESigilType
        {
            Scalar,
            Collection,
            Frequency,  //can be series
            None  //can be series            
        }

        public static string NL = "\r\n";  //official Windows, cf. https://stackoverflow.com/questions/3986093/in-c-whats-the-difference-between-n-and-r-n
        public static char NL2 = '\n';     //best for counting number of newlines, since Windows accepts both \r\n and \n as newline. Mac uses \r, hmm, never mind.

        /// <summary>
        /// Compares two strings, ignoring case (so "aBc" == "Abc").
        /// </summary>
        /// <param name="s1">First string</param>
        /// <param name="s2">Second string</param>
        /// <returns>True if equal</returns>
        public static bool Equal(string s1, string s2)
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
        /// In a double[] array, replaces missing values with 0
        /// </summary>
        /// <param name="temp"></param>
        public static void ReplaceNaNWith0(double[] temp)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                if (G.isNumericalError(temp[i])) temp[i] = 0d;
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
                    if (G.isNumericalError(temp[i, j])) temp[i, j] = 0d;
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
        /// Search for string inside string (case-insensitive)
        /// </summary>
        /// <param name="s1">String to search (e.g. 'peartree')</param>
        /// <param name="s2">Sub-string to search for (e.g. 'tree')</param>
        /// <returns>True if match</returns>
        public static bool Contains(string s1, string s2)
        {
            return s1.IndexOf(s2, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Parse a string into an integer
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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
        public static double ParseIntoDouble(string s, bool reportError)
        {
            double d = double.NaN;
            bool ok = G.TryParseIntoDouble(s, out d);
            if (ok) return d;
            if (reportError)
            {
                new Error("Cannot convert '" + s + "' into a value"); return double.NaN;
                //new GekkoException();
            }
            else
            {
                return double.NaN;
            }
        }

        /// <summary>
        /// Helper method
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HandleInternalIdentifyer1(string s)
        {
            s = s.Replace(Globals.internalColumnIdentifyer, "");
            s = s.Replace(Globals.internalSetIdentifyer, "#");
            return s;
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
        /// Helper method for DECOMP
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HandleInternalIdentifyer2(string s)
        {
            if (s.StartsWith("#")) s = Globals.internalSetIdentifyer + s.Substring(1);
            else s = Globals.internalColumnIdentifyer + s;
            return s;
        }


        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double ParseIntoDouble(string s)
        {
            return G.ParseIntoDouble(s, false);
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
        /// Add 's' to plural word. For instance "loaded 0 files", "loaded 1 file", "loaded 2 files", ... . 
        /// Here, we can use "loaded " + G.AddS(i, "file").
        /// </summary>
        /// <param name="s"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string AddS(int i, string s)
        {
            if (i == 1) return i + " " + s;  //"1 file"
            else
            {
                string s2 = s + "s";
                if (s == "library") s2 = "libraries";
                return i + " " + s2;        //"0 files" or "2 files" or ...
            }
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

        //will include lag indicator (¤)
        /// <summary>
        /// Helper function for models. Move to model part of code.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string FromBNumberToVarname2(int i)
        {
            string culprit = Program.model.modelGekko.varsBTypeInverted[i];
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
                //throw new GekkoException();
            }
            GekkoTime.ConvertFreqs(EFreq.Q, t1, t2, ref allFreqsHelper.t1Quarterly, ref allFreqsHelper.t2Quarterly);
            if (GekkoTime.Observations(allFreqsHelper.t1Quarterly, allFreqsHelper.t2Quarterly) < 1)
            {
                new Error("Start period must be <= end period");
                //throw new GekkoException();
            }
            GekkoTime.ConvertFreqs(EFreq.M, t1, t2, ref allFreqsHelper.t1Monthly, ref allFreqsHelper.t2Monthly);
            if (GekkoTime.Observations(allFreqsHelper.t1Monthly, allFreqsHelper.t2Monthly) < 1)
            {
                new Error("Start period must be <= end period");
                //throw new GekkoException();
            }
            GekkoTime.ConvertFreqs(EFreq.D, t1, t2, ref allFreqsHelper.t1Daily, ref allFreqsHelper.t2Daily);
            if (GekkoTime.Observations(allFreqsHelper.t1Daily, allFreqsHelper.t2Daily) < 1)
            {
                new Error("Start period must be <= end period");
                //throw new GekkoException();
            }
            GekkoTime.ConvertFreqs(EFreq.U, t1, t2, ref allFreqsHelper.t1Undated, ref allFreqsHelper.t2Undated);
            if (GekkoTime.Observations(allFreqsHelper.t1Undated, allFreqsHelper.t2Undated) < 1)
            {
                new Error("Start period must be <= end period");
                //throw new GekkoException();
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
                //throw new GekkoException();
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

            bool hasSigil = G.Chop_HasSigil(varname);

            string varnameWithFreq = varname;

            if ((isLeftSideVariable != O.ELookupType.LeftHandSide && !hasSigil) || (isLeftSideVariable == O.ELookupType.LeftHandSide && !hasSigil && (type == EVariableType.Var || type == EVariableType.Series)))
            {
                //Series has '!' added
                //In VAL v = 100, there will be no freq added.
                if (!varname.Contains(Globals.freqIndicator.ToString()))
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
        /// Helper method for GAMS. Move to GAMS part.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string S(int i)
        {
            if (i <= 1) return "";
            else return "s";
        }

        /// <summary>
        /// Overload
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        public static EFreq ConvertFreq(string freq)
        {
            return ConvertFreq(freq, false);
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
                //throw new GekkoException();
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
                //throw new GekkoException();
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

        /// <summary>
        /// Get index part of bankvarname, for instance x!q[a, b] returns ["a", "b"]
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
        /// Produces a bankvarname from chops/chunks/parts.
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="name"></param>
        /// <param name="freq"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        //See equivalent method in Functions.cs
        public static string Chop_GetFullName(string bank, string name, string freq, string[] index)
        {
            string s = O.UnChop(bank, name, freq, index);
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
                //throw new GekkoException();
            }
            bool hasSigil = false;
            if (varname2[0] == Globals.symbolScalar || varname2[0] == Globals.symbolCollection) hasSigil = true;
            return hasSigil;
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
        /// Convert freq from EFreq to string
        /// </summary>
        /// <param name="eFreq"></param>
        /// <returns></returns>
        public static string ConvertFreq(EFreq eFreq)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================
            string freq = "a";
            if (eFreq == EFreq.A)
            {
                //do nothing
            }
            else if (eFreq == EFreq.Q)
            {
                freq = "q";
            }
            else if (eFreq == EFreq.M)
            {
                freq = "m";
            }
            else if (eFreq == EFreq.D)
            {
                freq = "d";
            }
            else if (eFreq == EFreq.U)
            {
                freq = "u";
            }
            else
            {
                new Error("Strange error regarding freq");
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

        /// <summary>
        /// Ignore lag part in for instance x[-1]
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ExtractOnlyVariableIgnoreLag(string key, string code)
        {
            if (key == null) return null;
            string variable = null;
            int indx = key.LastIndexOf(code); //in decomp window, we may have x['a', 'z'][-1], so therefore we look for the last '['       
            if (indx != -1)
            {
                string rest = key.Substring(indx);
                if (rest.Contains("'") || rest.Contains(Globals.symbolCollection.ToString())) variable = key;  //if input is x['a', 'z'] or x[#i, #j], etc.
                else variable = key.Substring(0, indx - 0);
            }
            else variable = key;
            return variable;
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
        /// Get seconds elapsed since param1
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
        /// Show elapsed milliseconds in a readable format
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
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
            if (G.isNumericalError(level1))
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
                    //throw new GekkoException();
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
                    //throw new GekkoException();
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
        /// Return this number of blanks.
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
            newDatabank.yearStart = originalDatabank.yearStart;
            newDatabank.yearEnd = originalDatabank.yearEnd;
            newDatabank.info1 = originalDatabank.info1;
            newDatabank.date = originalDatabank.date;
            newDatabank.isDirty = true;
            //don't touch alias names: we are cloning the content of the databank, not altering its name.
            foreach (KeyValuePair<string, IVariable> kvp in originalDatabank.storage)
            {
                IVariable ivCopy = kvp.Value.DeepClone(null);
                newDatabank.AddIVariable(kvp.Key, ivCopy);
            }
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

        /// <summary>
        /// Helper class for natural file listing sorting (a8, a9, a10, a11 instead of a10, a11, a8, a9)
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
                        else if (char.IsDigit(mCurChar))
                        {
                            ParseNumericalValue();
                            return;
                        }
                        else if (char.IsLetter(mCurChar))
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
                    char NumberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
                    char NumberGroupSeparator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator[0];
                    do
                    {
                        NextChar();
                        if (mCurChar == NumberDecimalSeparator)
                        {
                            // parse digits after the Decimal Separator 
                            do
                            {
                                NextChar();
                                if (!char.IsDigit(mCurChar) && mCurChar != NumberGroupSeparator)
                                    break;

                            }
                            while (true);
                            break;
                        }
                        else
                        {
                            if (!char.IsDigit(mCurChar) && mCurChar != NumberGroupSeparator)
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
                        if (!char.IsLetter(mCurChar)) break;
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
                        result = string.Compare(mParser1.StringValue, mParser2.StringValue);
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
        /// Helper method for file access (writing)
        /// </summary>
        /// <param name="fs2"></param>
        /// <returns></returns>
        public static StreamWriter GekkoStreamWriter(FileStream fs2)
        {
            return new StreamWriter(fs2, Encoding.GetEncoding(1252));
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
        /// Are two double numbers equal?
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
            // So in Gekko, if there are missings, op = <, <=, >=, > are just kind of broke
            // but with missings, == and <> can be used.
            //
            //                              d2
            //               |   NaN    normal
            //-------------------------------------
            //   d1  NaN     |   true   false
            //      normal   |   false    ?
            //-------------------------------------
            //
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
            return G.isNumericalError(d1) && G.isNumericalError(d2);
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
                //throw new GekkoException();
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
                //throw new GekkoException();
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
                //throw new GekkoException();
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
                        //throw new GekkoException();
                    }
                }
                else
                {
                    new Error("Malformed name: '" + name + "'");
                    //throw new GekkoException();
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
        /// Convert a GekkoTime to floating point. Used for PLOT.
        /// </summary>
        /// <param name="gt"></param>
        /// <returns></returns>
        public static double FromDateToFloating(GekkoTime gt)
        {
            double d = double.NaN;
            if (gt.freq == EFreq.A || gt.freq == EFreq.U)
            {
                d = gt.super;
            }
            else if (gt.freq == EFreq.Q)
            {
                d = (double)gt.super + ((double)gt.sub - 1d) / 4d;
            }
            else if (gt.freq == EFreq.M)
            {
                d = (double)gt.super + ((double)gt.sub - 1d) / 12d;
            }
            return d;
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

        /// <summary>
        /// How many days does a certain month contain
        /// </summary>
        /// <param name="y"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int DaysInMonth(int y, int m)
        {
            if (y >= 1 && y <= 9999)
            {
                return DateTime.DaysInMonth(y, m);
            }
            else
            {
                return 30;  //does not make any sense anyway, but undated freq can have periods outside 1..9999
            }
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
        /// Another "interface" to the substring method, with start end end position, instead of using length. Indexes are 0-based.
        /// The positions are inclusive.
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
            return x;
        }

        /// <summary>
        /// In a string, skip to next non-space (tabs counted as spaces)
        /// </summary>
        /// <param name="c"></param>
        /// <param name="ii"></param>
        /// <returns></returns>
        public static int SkipSpaces(string c, int ii)
        {
            int i;
            //skip spaces (tab is included counted)
            for (i = ii; i < c.Length; i++)
            {
                if (c[i] == ' ' || c[i] == '\t')     //'\t' is tab
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
        /// <param name="version"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
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
        /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another 
        /// specified string according the type of search to use for the specified string.
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
                var ss2 = G.ExtractLinesFromText(s);
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
                G.Writeln(Globals.serviceMessage, Color.LightGray);
            }
            else
            {
                if (p.IsSimple())
                {
                    if (p.numberOfServiceMessages < 4)
                    {
                        G.Write2(s);
                        G.Writeln(Globals.serviceMessage, Color.LightGray);
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

                    if(Globals.runningOnTTComputer)
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

                }
                catch { };  //fail silently               
                

            }
            sb.AppendLine("==========================================================================");
            sb.AppendLine();
            if (!silent)
            {
                int widthRemember = Program.options.print_width;
                Program.options.print_width = int.MaxValue;
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
                }

                //Program.ShowPeriodInStatusField("");                
            }
            return sb;
        }        

        /// <summary>
        /// Pretty showing current frequency and period, like "Quarterly 2020q1-2024q4".
        /// </summary>
        /// <returns></returns>
        public static string FreqAndPeriodPretty(bool lowerCaseFirstChar)
        {
            string start = G.FromDateToString(Globals.globalPeriodStart);
            string end = G.FromDateToString(Globals.globalPeriodEnd);
            string f = G.GetFreqPretty();
            string ss2 = f + " " + start + "-" + end;
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
                rv = "Gekko " + Globals.gekkoVersion + " (" + Program.Get64Bitness(1) + "-bit), " + G.FreqAndPeriodPretty(true);
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
                rv = "Gekko " + Globals.gekkoVersion + " (" + Program.Get64Bitness(1) + "-bit), " + G.FreqAndPeriodPretty(true) + ", working folder = " + wf;
            }
            else
            {
                new Error("Illegal argument '" + s + "'");
            }

            return rv;
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
                new Error("Regarding year");
                return -12345;  //will never return anything
            }
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
            if (freq != null) freqPretty = " (" + G.GetFreqPretty(ConvertFreq(freq)) + ")";
            if(useQuotes) return "'" + varname + "'" + freqPretty;
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
        /// Get current frequency as pretty string
        /// </summary>
        /// <returns></returns>
        public static string GetFreqPretty()
        {            
            return GetFreqPretty(Program.options.freq);            
        }
               
        /// <summary>
        /// Convert EFreq to pretty string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetFreqPretty(EFreq input)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================
            string f = "";
            if ((input == EFreq.A))
            {
                f = "Annual";
            }
            else if ((input == EFreq.Q))
            {
                f = "Quarterly";
            }
            else if ((input == EFreq.M)) 
            {
                f = "Monthly";
            }
            else if ((input == EFreq.D))
            {
                f = "Daily";                
            }
            else if ((input == EFreq.U))
            {
                f = "Undated";
            }
            else
            {
                new Error("Strange error regarding freq");
            }

            return f;
        }

        /// <summary>
        /// Check that t1 is less than or equal to t2
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public static void CheckLegalPeriod(GekkoTime t1, GekkoTime t2)
        {
            int n = GekkoTime.Observations(t1, t2);
            if (n < 1)
            {
                new Error("Start date (" + t1.ToString() + ") should be same as or before end date (" + t2.ToString() + ")");
                //throw new GekkoException();
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
        /// Internal method. Before a .gcm file is parsed, some "glue" symbols are added to aid parsing. These are removed here.
        /// Use to transform parser errors containing parts of the .gcm code.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceGlueSymbols(string s)
        {
            //int length = s.Length;
            //TODO:
            //what about error position if glue stuff is removed?? Do some logic...
            if (s == null) return s;
            s = s.Replace(Globals.symbolGlueChar2, "");
            s = s.Replace(Globals.symbolGlueChar3, "");
            s = s.Replace(Globals.symbolGlueChar4, "");
            s = s.Replace(Globals.symbolGlueChar5, "<");  //--> fixme, this is a workaround
            s = s.Replace(Globals.symbolGlueChar6, "[");  //--> fixme, this is a workaround, #098523
            s = s.Replace(Globals.symbolGlueChar6a, "["); //--> is this necessary?
            s = s.Replace(Globals.symbolGlueChar7, "[");
            s = s.Replace(Globals.symbolGlueChar1.ToString(), "");  //must be after symbolGlueChar7
            s = Regex.Replace(s, "s___er", "ser", RegexOptions.IgnoreCase);  //#098275432874
            s = Regex.Replace(s, "s___eries", "series", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, "s____er", "ser", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, "s____eries", "series", RegexOptions.IgnoreCase);

            //s = s.Remove(
            //lengthDiff = length - s.Length;
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
        /// Overload.
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
        /// Converts a double into nearest int. Expects the input value to be very neaar to an integer, tolerance 0.000001 absolute.
        /// Will handle negative values ok.
        /// </summary>
        /// <param name="rounded"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ConvertToInt(out int rounded, double value)
        {            
            bool flag = true;
            rounded = Convert.ToInt32(value);  //this function rounds to nearest int, so -12.98 --> -13
            double decimals = value - rounded;            
            if (G.isNumericalError(value) || Math.Abs(decimals) > 0.000001)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// Converts a double into nearest int. Expects the input value to be very neaar to an integer, tolerance 0.000001 absolute.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ConvertToInt(double value)
        {
            //simpler method
            bool flag = false;
            int rounded = Convert.ToInt32(value);  //this function rounds to nearest int, so -12.98 --> -13
            double decimals = value - rounded;
            if (G.isNumericalError(value) || Math.Abs(decimals) > 0.000001)
            {
                new Error("Could not convert " + value + " into integer");
                //throw new GekkoException();
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
        /// Delete an entire folder, with omit.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="omitType"></param>
        public static void DeleteFolder(string s, string omitType)
        {
            if (!Directory.Exists(s)) return;
            DeleteFolderHelper(new DirectoryInfo(s), omitType);
        }

        /// <summary>
        /// Delete an entire folder.
        /// </summary>
        /// <param name="s"></param>
        public static void DeleteFolder(string s)
        {
            DeleteFolder(s, null);
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
        /// Helper for deleting folders.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="omitType"></param>
        //Seems it does not delete the folders, but only their content
        private static void DeleteFolderHelper(DirectoryInfo directoryInfo, string omitType)
        {
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (omitType != null && G.Equal("." + omitType, file.Extension)) continue;  //skip it
                file.Delete();  //hmm probably best not to use WaitForFileDelete() here, exceptions are typically caught in a wrapper on this method, and not critical if it fails (used for cleanup)
            }
            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                DeleteFolderHelper(subfolder, omitType);
            }
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
        /// Converts e.g. "fxo" to "fXo" is a model is loaded (using casing of model)
        /// </summary>
        /// <param name="var">Input string</param>
        /// <returns>Output string</returns>
        public static string GetUpperLowerCase(string var)
        {
            if (HasModelGekko() && Program.model.modelGekko.varsAType != null)
            {
                //a model is loaded
                //ATypeData temp = (ATypeData)Program.model.modelGekko.varsAType[var];
                ATypeData temp = null; Program.model.modelGekko.varsAType.TryGetValue(var, out temp);
                if (temp != null)
                {
                    //var is known from the model
                    var = temp.varName;  //with nicer upper/lowercase.
                }
            }
            return var;
        }

        /// <summary>
        /// Whether a Gekko model is loaded with MODEL
        /// </summary>
        /// <returns></returns>
        public static bool HasModelGekko()
        {
            return Program.model != null && Program.model.modelGekko != null;
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
        /// Use GekkoAction class instead
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
        /// Used in DECOMP.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool ContainsWord(string s, string word)
        {                       
            return Regex.Match(s, @"\b" + word + @"\b", RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// Used for JSON
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
        /// For developer use/debugging. Will also be true if ExcelDna is active.
        /// </summary>
        public static bool IsUnitTesting()  
        {
            if (Globals.excelDna) return true;            
            if ((Application.ExecutablePath.Contains("testhost.x86.exe") || Application.ExecutablePath.Contains("vstesthost.exe") || Application.ExecutablePath.Contains("QTAgent32_40.exe") || Application.ExecutablePath.Contains("QTAgent32.exe") || Application.ExecutablePath.Contains("vstest.executionengine.x86.exe"))) return true;
            else return false;
        }

        /// <summary>
        /// Not used at the moment
        /// </summary>
        /// <returns></returns>
        public static bool IsMuting()
        {
            return G.Equal(Program.options.interface_mute, "yes");
        }

        /// <summary>
        /// Not used at the moment
        /// </summary>
        /// <returns></returns>
        public static bool IsPiping()
        {
            return Globals.pipe || Globals.pipe2;
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

            bool mustAlsoPrintOnScreen = helper.mustAlsoPrintToScreen;            
            
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
            bool isMuting = false;

            if (G.Equal(Program.options.interface_mute, "yes")) isMuting = true;
            
            //Not piping to normal pipe file if there is a pipe to pipe2-file (eg. for "p fy file=output.txt")
            if (!Globals.pipe2 && Globals.pipe && Globals.pipeFileHelper.pipeFile != null)
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
                if (G.IsUnitTesting())
                {
                    if (newline)
                    {
                        if (Globals.excelDna)
                        {
                            if (Globals.excelDnaOutput != null) Globals.excelDnaOutput.AppendLine(s);
                        }
                        else
                        {
                            Globals.unitTestScreenOutput.AppendLine(s);
                            System.Diagnostics.Debug.WriteLine(s);
                        }
                    }
                    else
                    {
                        if (Globals.excelDna)
                        {
                            if (Globals.excelDnaOutput != null) Globals.excelDnaOutput.Append(s);
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
                if (G.IsUnitTesting())
                {
                    if (Globals.excelDna)
                    {
                        if (Globals.excelDnaOutput != null) Globals.excelDnaOutput.Append(s);
                    }
                    else
                    {
                        Globals.unitTestScreenOutput.Append(s);
                        System.Diagnostics.Debug.Write(s);
                    }
                }
                else
                {
                    //not piping, not muting, not ExcelDna'ing, not unit testing --> normal printing
                    if (link == null)
                    {
                        textBox.AppendText(s);
                    }
                    else
                    {
                        RichTextBoxEx textBoxEx = textBox as RichTextBoxEx;
                        if (textBoxEx == null) MessageBox.Show("*** ERROR: Cannot use links in this RichTextBox");
                        int position = textBoxEx.SelectionStart;
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
            if (!Globals.pipe2 && Globals.pipe && Globals.pipeFileHelper.pipeFile != null)
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
    }    
}
