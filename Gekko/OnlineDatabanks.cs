using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Gekko
{
    
    public static class OnlineDatabanks
    {
        public static void Walk(string table, List<string> codesHeader, List<List<string>> codes, List<string> codesCombi, List<List<string>> values, List<string> valuesCombi, int depth, string sCodes, string sValues)
        {
            if (depth > codes.Count - 1)
            {                
                if (sCodes.EndsWith("_")) sCodes = sCodes.Substring(0, sCodes.Length - 1);
                if (sValues.StartsWith(", ")) sValues = sValues.Substring(2);
                codesCombi.Add(table + sCodes);
                valuesCombi.Add(sValues);
                return;                
            }                        

            for (int i = 0; i < codes[depth].Count; i++)
            {                
                string sCodesTemp = sCodes + "_" + codesHeader[depth] + "_" + codes[depth][i];
                string sValuesTemp = sValues + ", " + values[depth][i];

                Walk(table, codesHeader, codes, codesCombi, values, valuesCombi, depth + 1, sCodesTemp, sValuesTemp);
            }
            
        }        
        
        public static void Test(string url, string jsonName)
        {            
            string input = Program.options.folder_working + "\\" + jsonName;
            string jsonCode = Program.GetTextFromFileWithWait(input);            
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> jsonTree = (Dictionary<string, object>)serializer.DeserializeObject(jsonCode);            
            string table = (string)jsonTree["table"];
            List<string> codesHeaderJson = new List<string>();
            
            object[] o = (object[])jsonTree["variables"];
            foreach (Dictionary<string, object> oo in o)
            {
                //G.Writeln("" + oo["code"]);
                codesHeaderJson.Add((string)oo["code"]);                
            }         

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonCode);
                streamWriter.Flush();
                streamWriter.Close();
                string result = null;

                bool write = true;
                if (write)
                {
                    //using (FileStream fs = Program.WaitForFileStream(Program.options.folder_working + "\\pc-axis-data.px", Program.GekkoFileReadOrWrite.Write))
                    using (FileStream fs = Program.WaitForFileStream(Globals.localTempFilesLocation + "\\pc-axis-data.px", Program.GekkoFileReadOrWrite.Write))
                    //using (StreamWriter sw = G.GekkoStreamWriter(fs))
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        G.Writeln2("--> Download start...");
                        HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        //It seems the px file is in ANSI/win 1252: the file also reports this at the top: CHARSET="ANSI"; CODEPAGE = "Windows-1252";
                        //Setting UTF8 here will fail!
                        Encoding encoding = System.Text.Encoding.GetEncoding("Windows-1252");                        
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), encoding))
                        {
                            result = streamReader.ReadToEnd();
                            sw.Write(result);
                            sw.Flush();
                        }
                    }
                }
                                
                string freq = "a";
                
                List<string> dates = new List<string>();                
                bool start = false;
                List<string> lines2 = G.ExtractLinesFromText(result);
                string temp = null;
                List<string> lines = new List<string>();
                foreach (string line in lines2)
                {
                    temp += line;
                    if (line.TrimEnd().EndsWith(";"))
                    {
                        lines.Add(temp);
                        temp = null;
                    }
                }

                List<string> codesHeader = new List<string>();

                List<List<string>> codes = new List<List<string>>();
                List<List<string>> values = new List<List<string>>();                

                //int counter = 0;
                string codeTimeString = "CODES(\"tid\")=";
                string codeString = "CODES(";
                string valueTimeString = "VALUES(\"tid\")=";
                string valueString = "VALUES(";
                foreach (string line2 in lines)
                {
                    string line = line2.Trim();
                    if (line.StartsWith("DATA="))
                    {                        
                        List<string> codesCombi = new List<string>();
                        List<string> valuesCombi = new List<string>();
                        
                        //we are using codesHeaderJson instead of codesHeader (these are more verbose)
                        Walk(table, codesHeaderJson, codes, codesCombi, values, valuesCombi, 0, "", "");
                        string s = line;
                        s = s.Substring(5);
                        if (s.EndsWith(";")) s = s.Substring(0, s.Length - 1);                        
                        //read the data
                        string[] ss = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (ss.Length != codesCombi.Count * dates.Count)
                        {
                            G.Writeln2("*** ERROR: Gekko expected " + codesCombi.Count + " * " + dates.Count + " = " + (codesCombi.Count * dates.Count) + " values, but got " + ss.Length);
                            throw new GekkoException();
                        }
                        
                        for (int j = 0; j < codesCombi.Count; j++)
                        {
                            string name2 = codesCombi[j];
                            name2 = name2.Replace("Æ", "AE");
                            name2 = name2.Replace("Ø", "OE");
                            name2 = name2.Replace("Å", "AA");
                            name2 = name2.Replace("æ", "ae");
                            name2 = name2.Replace("ø", "oe");
                            name2 = name2.Replace("å", "aa");
                            name2 = name2.Replace("-", "h");  //h like hyphen
                            TimeSeries ts = new TimeSeries(G.GetFreq(freq), name2);
                            ts.label = valuesCombi[j];
                            ts.source = url + ", " + jsonName;
                            ts.stamp = Globals.dateStamp;
                            ts.isDirty = true; //for safety
                            GekkoTime gt0 = Globals.tNull;
                            GekkoTime gt1 = Globals.tNull;
                            for (int i = 0; i < dates.Count; i++)  //periods
                            {
                                GekkoTime gt = G.FromStringToDate(dates[i]);
                                double value = double.NaN;
                                string temp2 = ss[i + j * dates.Count];
                                try
                                {
                                    value = double.Parse(temp2);
                                }
                                catch
                                {
                                    if (temp2 == "\".\"" || temp2 == "\"..\"" || temp2 == "\"...\"" || temp2 == "\"....\"" || temp2 == "\":\"")
                                    {
                                        //See http://www.inside-r.org/packages/cran/pxr/docs/read.px
                                        //do nothing, "." and ".." and "..." and "...." and ":" will be missing value (these include the quotes in the Axis file)
                                    }
                                    else
                                    {
                                        G.Writeln2("*** ERROR: Could not convert '" + temp2 + "' into a number");
                                        throw new GekkoException();
                                    }
                                }

                                ts.SetData(gt, value);
                                if (gt0.IsNull()) gt0 = gt;
                                if (gt1.IsNull()) gt1 = gt;
                                if (gt.StrictlySmallerThan(gt0)) gt0 = gt;
                                if (gt.StrictlyLargerThan(gt1)) gt1 = gt;
                            }

                            if (Program.databanks.GetPrim().GetVariable(G.GetFreq(freq), ts.variableName) != null)
                            {
                                Program.databanks.GetPrim().RemoveVariable(G.GetFreq(freq), ts.variableName);
                            }
                            Program.databanks.GetPrim().AddVariable(freq, ts);
                            //if (j == 0) G.Writeln();
                            G.Writeln(ts.variableName + ", with freq " + freq.ToUpper() + ", " + G.FromDateToString(gt0) + "-" + G.FromDateToString(gt1));
                            //counter++;                        
                        }
                        G.Writeln("--> Downloaded " + codesCombi.Count + " timeseries in total");
                      
                    }
                    else if (line.StartsWith(codeTimeString))
                    {
                        string s = line.Substring(codeTimeString.Length);
                        if (s.EndsWith(";")) s = s.Substring(0, s.Length - 1);
                        string[] ss = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s2 in ss)
                        {
                            if (s2.Substring(1, s2.Length - 2).IndexOf("m", StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                freq = "m";
                            }
                            if (s2.Substring(1, s2.Length - 2).IndexOf("k", StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                freq = "q";
                            }
                            if (s2.Substring(1, s2.Length - 2).IndexOf("q", StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                freq = "q";
                            }
                            dates.Add(s2.Substring(1, s2.Length - 2));
                        }
                    }
                    else if (line.StartsWith(codeString))
                    {
                        //For instance:
                        //
                        //  CODES("ydelse, k?n og alder")="TOT","NET","LDP","LKT","AKI","ADP","AKT","MEN","KVR","U25","O25","O30","O40","O50","O60";
                        //  CODES("s?sonkorrigering og faktiske tal")="10";
                        //
                        int i = line.IndexOf("=");
                        if (i < 0)
                        {
                            G.Writeln2("*** ERROR: Expected a '=' in this line: " + line);
                            throw new GekkoException();
                        }
                        
                        string s3 = line.Substring(0, i); s3 = s3.Substring(7); s3 = s3.Substring(0, s3.Length - 2);
                        codesHeader.Add(s3);

                        string s = line.Substring(i + 1);
                        if (s.EndsWith(";")) s = s.Substring(0, s.Length - 1);
                        string[] ss = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> names2 = new List<string>();
                        foreach (string s2 in ss)
                        {
                            names2.Add(s2.Substring(1, s2.Length - 2));
                        }
                        if (names2.Count == 0)
                        {
                            G.Writeln2("*** ERROR: Expected 1 or more items in this line: " + line);
                            throw new GekkoException();
                        }
                        codes.Add(names2);
                    }
                    else if (line.StartsWith(valueTimeString))
                    { 
                        //ignore
                    }
                    else if (line.StartsWith(valueString))
                    {                        
                        int i = line.IndexOf("=");
                        if (i < 0)
                        {
                            G.Writeln2("*** ERROR: Expected a '=' in this line: " + line);
                            throw new GekkoException();
                        }
                        string s = line.Substring(i + 1);
                        if (s.EndsWith(";")) s = s.Substring(0, s.Length - 1);
                        string[] ss = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> values2 = new List<string>();
                        foreach (string s2 in ss)
                        {
                            values2.Add(s2.Substring(1, s2.Length - 2));
                        }
                        if (values2.Count == 0)
                        {
                            G.Writeln2("*** ERROR: Expected 1 or more items in this line: " + line);
                            throw new GekkoException();
                        }
                        values.Add(values2);
                    }
                }
            }
        }        
    }
}
