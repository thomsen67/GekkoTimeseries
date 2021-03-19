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
using System.Collections;
using System.Text;
using System.IO;

namespace Gekko
{
    class TspUtilities
    {
        
        // This should become obsolete soon

        //                 E2_P
        //1975           0.74815
        //1976           0.78258

        //                  S1S2
        //1975            .
        //1976           0.72799
        //1977           0.68516 
        public static void tspDataUtility(String dataFile, String tsdOutputFile)
        {
            int allCounter = 0;

            using (FileStream fs = Program.WaitForFileStream(tsdOutputFile, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter res = G.GekkoStreamWriter(fs))
            {
                List<string> al = new List<string>(5000);
                List<string> alType = new List<string>(5000);
                List<string> al1 = new List<string>(5000);  //contains output                        
                Program.TokensFromFileToArrayList(dataFile, true, false, al, alType);
                double[] data = null;
                String varName = "";
                int start = -12345;
                int end = -12345;
                int counter = 0;
                bool firstTime = true;
                bool minus = false;
                for (int i = 0; i < al.Count - Globals.extra; i++)
                {                    
                    if ((string)alType[i] == "Symbol" && (string)al[i] == "-")
                    {
                        minus = true;
                    }
                    if ((string)alType[i] == "Word")
                    {
                        if (firstTime == false)
                        {
                            Program.WriteTsdRecord2(start, end, res, -12345, data, varName);
                        }
                        data = new double[4000];   //slack, fix
                        varName = (string)al[i];
                        allCounter++;
                        start = int.Parse((string)al[i + 2]);
                        counter = 0;
                        firstTime = false;
                        minus = false;
                    }
                    if ((string)alType[i] == "Number" || (string)al[i] == ".")
                    {

                        if (counter % 2 == 0)
                        {
                            end = int.Parse((string)al[i]);
                        }
                        else
                        {
                            if (al[i] == ".")
                            {
                                data[end] = double.NaN;
                            }
                            else
                            {
                                string n1 = (string)al[i];
                                n1 = n1.Replace("d", "E");
                                n1 = n1.Replace("D", "E");
                                data[end] = G.ParseIntoDouble(n1);
                                if (minus) data[end] = -data[end];
                            }
                        }
                        counter++;
                        minus = false;
                    }
                }
                Program.WriteTsdRecord2(start, end, res, -12345, data, varName);
                res.Flush();
                res.Close();
                G.Writeln("Converted " + allCounter + " times series from TSP format to TSD.");
                G.Writeln("Output is in the file: output.tsd.");
                G.Writeln();
            }
        }


        //                 E2_P
        //1975           0.74815
        //1976           0.78258

        //                  S1S2
        //1975            .
        //1976           0.72799
        //1977           0.68516 
        public static void tspDataUtility(string dataFile, Databank databank, ReadOpenMulbkHelper oRead, Program.ReadInfo readInfo)
        {
            // Used in READ<tsp>            
                        
            int allCounter = 0;

            string databankName = databank.name;

            if (!oRead.Merge)
            {
                databank.Clear();
            }

            int min = int.MaxValue;
            int max = int.MinValue;

            if(true)
            {
                List<string> al = new List<string>(5000);
                List<string> alType = new List<string>(5000);
                List<string> al1 = new List<string>(5000);  //contains output                        
                Program.TokensFromFileToArrayList(dataFile, true, false, al, alType);
                //double[] data = null;
                string varName = "";
                //int start = -12345;
                int year = -12345;                
                int counter = 0;
                //bool firstTime = true;
                bool minus = false;
                Series ts = null;
                for (int i = 0; i < al.Count - Globals.extra; i++)
                {
                    if ((string)alType[i] == "Symbol" && (string)al[i] == "-")
                    {
                        minus = true;
                    }
                    else if ((string)alType[i] == "Word")
                    {
                        
                        //data = new double[4000];   //slack, fix
                        varName = (string)al[i];

                        //HMMM, maybe use much simpler way here... (and remove last parameter in method):
                        ts = O.GetIVariableFromString(G.Chop_AddBank(varName, databankName), O.ECreatePossibilities.Can, true) as Series;
                        allCounter++;

                        //try
                        //{
                        //    start = int.Parse((string)al[i + 2]);
                        //}
                        //catch
                        //{
                        //    G.Writeln2("*** Could not parse '" + (string)al[i + 2] + "' as an integer");
                        //    throw new GekkoException();
                        //}                                               

                        counter = 0;
                        //firstTime = false;
                        minus = false;
                    }
                    else if ((string)alType[i] == "Number" || (string)al[i] == ".")
                    {
                        if (counter % 2 == 0)
                        {
                            try
                            {
                                year = int.Parse((string)al[i]);
                                min = G.GekkoMin(min, year);
                                max = G.GekkoMax(max, year);
                            }
                            catch
                            {
                                G.Writeln2("*** Could not parse '" + (string)al[i] + "' as an integer");
                                throw new GekkoException();
                            }
                        }
                        else
                        {
                            if (al[i] == ".")
                            {
                                ts.SetData(new GekkoTime(EFreq.A, year, 1), double.NaN);
                                //data[end] = double.NaN;
                            }
                            else
                            {
                                string n1 = (string)al[i];
                                n1 = n1.Replace("d", "E");
                                n1 = n1.Replace("D", "E");                                

                                try
                                {
                                    double v = G.ParseIntoDouble(n1);
                                    if (minus) v = -v;
                                    ts.SetData(new GekkoTime(EFreq.A, year, 1), v);
                                }
                                catch
                                {
                                    G.Writeln2("*** Could not parse '" + n1 + "' as an floating point number");
                                    throw new GekkoException();
                                }                                
                            }
                        }
                        counter++;
                        minus = false;
                    }
                }                               
                
                G.Writeln2("Read " + allCounter + " times series from TSP output file.");
                //G.Writeln2("Note that this utility merges data with any existing data in the primary bank.");
                G.Writeln();
            }
            readInfo.startPerInFile = min;
            readInfo.endPerInFile = max;
            readInfo.variables = allCounter;

            if (oRead.Merge)  //See almost identical code in other reads (tsd, pcim, etc...)
            {
                readInfo.startPerResultingBank = G.GekkoMin(readInfo.startPerInFile, databank.yearStart);
                readInfo.endPerResultingBank = G.GekkoMax(readInfo.endPerInFile, databank.yearEnd);
            }
            else
            {
                readInfo.startPerResultingBank = readInfo.startPerInFile;
                readInfo.endPerResultingBank = readInfo.endPerInFile;
            }

            databank.Trim();  //should in principle do this after each time series read, but these files are typically small anyway.

        }

        //template.frm:

        //            frml _DJRD log(qJealw) = -log(dtqjeal) + log(fXal)
        //                         + e11*log(pqjeal/pxal/dtqjeal)
        //                         + e12*log(pqjoal/pxal/dtqjoal) + k1 $
        //frml _DJRD log(qJoalw) = -log(dtqjoal) + log(fXal)
        //                         + e12*bshal*log(pqjeal/pxal/dtqjeal)
        //                         + e22*log(pqjoal/pxal/dtqjoal)
        //                         + gradk2*log(graddag)  + k2 $
        //frml _DJRD log(qJoalw) = -log(dtqjoal) + log(fXal)
        //                         + e12*bshal*log(pqjeal/pxal/dtqjeal)
        //                         + (-e12*bshal+e11+e12)*log(pqjoal/pxal/dtqjoal)
        //                         + gradk2*log(graddag) + k2 $
        //frml _SJRD Dlog(qJeal) = v1*dlog(qJealw) + c1*log(qJealw(-1)/qJeal(-1)) $
        //frml _SJRD Dlog(qJoal) = v2*dlog(qJoalw) + c2*log(qJoalw(-1)/qJoal(-1))
        //                         + (1-v2)*gradk2*Dlog(graddag) $

        //combine with .out file from tsp.       
        
        public static void tspUtility(String dataFile, String templateFile, String frmOutputFile)
        {            
            CaseInsensitiveHashtable ht = new CaseInsensitiveHashtable();
            CaseInsensitiveHashtable htConst = new CaseInsensitiveHashtable();
            List<string> al = new List<string>(5000);
            List<string> alType = new List<string>(5000);
            List<string> al1 = new List<string>(5000);  //contains output                        
            Program.TokensFromFileToArrayList(dataFile, true, false, al, alType);
            List<string> aaName = new List<string>();
            List<string> aaValue = new List<string>();

            bool constFlag = false;
            bool constFlagVars = true;
            bool constFlagData = false;
            bool constFlagNegative = false;
            bool lsqFlag1 = false;
            bool lsqFlag2 = false;

            for (int i = 0; i < al.Count - Globals.extra; i++)
            {
                if (i >= 4 && al[i - 4] == "CONSTANTS" && al[i - 3] == ":" && al[i - 2] == "\r\n" && al[i - 1] == "\r\n")
                {
                    constFlag = true;
                }
                if (al[i] == "Log" && al[i + 1] == "likelihood")
                {
                    lsqFlag1 = true;
                }
                if (lsqFlag1 && al[i] == "Parameter" &&
                    al[i + 1] == "Estimate" &&
                    al[i + 2] == "Error" &&
                    al[i + 3] == "t" &&
                    al[i + 4] == "-" &&
                    al[i + 5] == "statistic" &&
                    al[i + 6] == "P" &&
                    al[i + 7] == "-" &&
                    al[i + 8] == "value")
                {
                    i += 9;
                    lsqFlag2 = true;
                }

                if (constFlag)
                {
                    //bool VALUEencountered = false;
                    if (constFlagVars)
                    {

                        for (int iii = i; iii < int.MaxValue; iii++)
                        {
                            if (al[iii] == "\r\n")
                            {
                                if (al[iii + 1] != "VALUE")
                                {
                                    constFlag = false;
                                }
                                break;
                            }
                        }
                        if (constFlag)
                        {
                            if (alType[i] == "Word")
                            {
                                aaName.Add(al[i]);
                            }
                            else if (al[i] == "\r\n")
                            {
                                constFlagVars = false;
                                constFlagData = true;
                                //i++;  //so that consFlagData part starts at the right place
                                continue;  //with i
                            }
                            else
                            {
                                new Error("Problem in TSP utility", false);
                            }
                        }
                    }
                    if (constFlagData)
                    {
                        if (al[i] == "-" && alType[i + 1] == "Number")
                        {
                            i++;  //jump to number
                            constFlagNegative = true;
                        }
                        if (alType[i] == "Word")
                        {
                            if (al[i] == "VALUE")
                            {
                                //ok!
                                //i++; //to jump to first data
                                continue; //with i
                            }
                            else
                            {
                                new Error("Problem in TSP utility", false);
                            }
                        }
                        if (alType[i] == "Number")
                        {
                            if (constFlagNegative)
                            {
                                aaValue.Add("-" + al[i]);
                            }
                            else
                            {
                                aaValue.Add(al[i]);
                            }
                            constFlagNegative = false;
                        }
                        else if (al[i] == "\r\n")
                        {
                            constFlagVars = true;
                            constFlagData = false;

                            for (int iii = i; iii < int.MaxValue; iii++)
                            {
                                if (al[iii] != "\r\n")
                                {
                                    i = iii - 1;  //to jump past line breaks, to first non-line break
                                    break;
                                }
                            }
                        }
                        else if (al[i] == "-")
                        {
                            //ok
                        }
                        else
                        {
                            new Error("Problem in TSP utility", false);
                        }
                    }
                }
                if (lsqFlag2)
                {

                    if (al[i] == "Standard" && al[i + 1] == "Errors" && al[i + 2] == "computed")
                    {
                        lsqFlag1 = false;
                        lsqFlag2 = false;
                    }
                    else if (alType[i] == "Word")
                    {
                        String par = (string)al[i];
                        String val = "";
                        if (al[i + 1] == "-")
                        {
                            val = "-" + al[i + 2];
                        }
                        else
                        {
                            val = (string)al[i + 1];
                        }
                        if (ht.ContainsKey(par))
                        {
                            ht.Remove(par);
                        }
                        ht.Add(par, val);
                    }
                }
            }

            for (int i = 0; i < aaName.Count; i++)
            {
                if (htConst.ContainsKey(aaName[i]))
                {
                    htConst.Remove(aaName[i]);
                }
                htConst.Add(aaName[i], aaValue[i]);
            }

            al = new List<string>(5000);
            alType = new List<string>(5000);

            Program.TokensFromFileToArrayList(templateFile, false, false, al, alType);

            //----------------------------------------------------
            //
            int allCounter = 0;
            int constCounter = 0;
            bool skipOne = false;
            for (int i = 0; i < al.Count - Globals.extra; i++)
            {
                if (G.TspUtilityFindType(al, alType, i, 0) == "Symbol" && G.TspUtilityFindType(al, alType, i, +1) == "Word")
                {
                    String varName = G.TspUtilityFindToken(al, alType, i, +1);
                    Object value1 = ht[varName];
                    Object value2 = htConst[varName];
                    Object value = value1;
                    if (value1 == null && value2 != null)
                    {
                        value = value2;
                        constCounter++;
                    }
                    if (value != null)
                    {

                        allCounter++;
                        //found in hash table, i.e. the value of this varName should be substituted in
                        skipOne = true;
                        //It would be strange to have a variable being a number as the first item in the template, so therefore i>=1 is ok.
                        //Even if so, the code should work fine.
                        if (i >= 1 && ((String)value).StartsWith("-"))
                        {
                            if (G.TspUtilityFindToken(al, alType, i, 0) == "+")
                            {
                                //... + a1 + ... ---> ... -0.1234 + ...                                
                                al1.Add(G.Add0Ifmissing((String)value));
                            }
                            else if (G.TspUtilityFindToken(al, alType, i, 0) == "=" || G.TspUtilityFindToken(al, alType, i, 0) == "(")
                            {
                                //... = a1 + ... ---> ... = -0.1234 + ...
                                //... ( a1 + ... ---> ... ( -0.1234 + ...
                                al1.Add(al[i]);
                                al1.Add(G.Add0Ifmissing((String)value));
                            }
                            else
                            {
                                //... * a1 + ... ---> ... * (-0.1234) + ...
                                al1.Add(al[i]);
                                al1.Add("(");
                                al1.Add(G.Add0Ifmissing((String)value));
                                al1.Add(")");
                            }
                        }
                        else
                        {

                            //val is positive
                            //val does not start with "-"
                            al1.Add(al[i]);
                            al1.Add(G.Add0Ifmissing((String)value));
                        }
                        //i++;
                    }
                    else
                    {
                        //Word is not a variable
                        al1.Add(al[i]);
                        al1.Add(varName);
                    }
                    i = G.TspUtilitiesFindIndex(al, alType, i, +1);  //jumps forward to the new i

                }
                else
                {
                    //current pos is not a symbol, and/or next non-whitespace token is not a word                    
                    al1.Add(al[i]);
                }

            }

            using (FileStream fs = Program.WaitForFileStream(frmOutputFile, Program.GekkoFileReadOrWrite.Write))
            using (StreamWriter res = G.GekkoStreamWriter(fs))
            {
                for (int i = 0; i < al1.Count; i++)
                {                    
                    res.Write(al1[i]);
                }
                res.Flush();
                res.Close();
                G.Writeln("Inserted " + allCounter + " numbers, hereof " + (allCounter - constCounter) + " parameter values and " + constCounter + " constant values.");
                G.Writeln("Output is in the file: output.frm.");
                G.Writeln();
            }
        }
    }
}
