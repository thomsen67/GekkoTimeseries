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
        public static void Download(O.Download o1)
        {
            if (Program.options.bugfix_download) DownloadNew(o1);
            else DownloadOld(o1);
        }

        private static void SLETMIG(O.Download o1)
        {
            string dbUrl = "url til databanken";
            string jsonCode = "json code fra json-fil";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(dbUrl);
            httpWebRequest.Timeout = 24 * 60 * 60 * 1000; //24 hours max        
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            bool xxxx = httpWebRequest.UseDefaultCredentials;

            StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            
            using (streamWriter)
            {
                streamWriter.Write(jsonCode);
                streamWriter.Flush();
                streamWriter.Close();                
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Encoding encoding = System.Text.Encoding.GetEncoding("Windows-1252");                                
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), encoding))
                {
                    string pxLinesText = streamReader.ReadToEnd();  //encoded as Windows-1252    
                    
                    //pxLinesText er den px-fil, som returneres.
                    //denne inlæses i det følgende                                    
                }                
            }
        }

        private static void DownloadNew(O.Download o1)
        {
            string input = Program.options.folder_working + "\\" + o1.fileName;
            string jsonCode = Program.GetTextFromFileWithWait(input); //also removes some kinds of funny characters
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(o1.dbUrl);
            httpWebRequest.Timeout = 24 * 60 * 60 * 1000; //24 hours max        
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            httpWebRequest.UseDefaultCredentials = true;  //to be able to access sumdatabasen from inside DST
            httpWebRequest.Credentials = CredentialCache.DefaultNetworkCredentials;  //seems necessary together with the above
            httpWebRequest.UserAgent = "Gekko/" + Globals.gekkoVersion;  //Pelle Rossau von Hedemann (DST) skriver "Jeg kan i øvrigt anbefale at sætte UserAgent, fx Gekko/2.3.4, på request-objektet, således at denne kan genfindes i loggen. API’et returnerer i øvrigt en header med navnet ” StatbankAPI-Request-Id”, som indeholder et GUID for hvert eneste kald. Denne gør det muligt at identificere det specifikke kald i vores log. Man kan, hvis man ønsker det, opsamle denne id og præsentere den for brugeren på en eller anden måde"

            Dictionary<string, object> jsonTree = null;
            try
            {
                jsonTree = (Dictionary<string, object>)serializer.DeserializeObject(jsonCode);
            }
            catch (Exception e)
            {
                G.Writeln2("*** ERROR: The .json file does not seem correctly formatted.");
                G.Writeln("           Message: " + e.Message);
                throw;
            }

            string tableName = null;
            try
            {
                tableName = (string)jsonTree["table"];
            }
            catch { }
            if (tableName == null)
            {
                G.Writeln2("*** ERROR: You should use \"table\": \"...\", in the .json file");
                throw new GekkoException();
            }
            
            string format = null;
            try
            {
                format = (string)jsonTree["format"];
            }
            catch { }
            if (format == null || !G.equal(format, "px"))
            {
                G.Writeln2("*** ERROR: You should use \"format\": \"px\", in the .json file");
                throw new GekkoException();
            }
            
            List<string> codesHeaderJson = new List<string>();
            try
            {
                object[] o = (object[])jsonTree["variables"];
                foreach (Dictionary<string, object> oo in o)
                {
                    codesHeaderJson.Add((string)oo["code"]);
                }
            }
            catch
            {
                G.Writeln2("*** ERROR: The \"variables\" field in the .json file seems malformed");
                throw new GekkoException();
            }

            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            }
            catch (Exception e)
            {
                //May get something like this: System.Net.WebException: Der kunne ikke oprettes forbindelse til fjernserveren ---> System.Net.Sockets.SocketException: Det blev forsøgt at få adgang til en socket på en måde, der er forbudt af den pågældende sockets adgangstilladelser 91.208.143.3:80
                G.Writeln2("*** ERROR: Connection failed with the following error:");
                G.Writeln("           " + e.Message);
                throw;
            }

            using (streamWriter)
            {
                streamWriter.Write(jsonCode);
                streamWriter.Flush();
                streamWriter.Close();
                string pxLinesText = null;

                DateTime t0 = DateTime.Now;

                G.Writeln2("--> Download of data file start...");
                HttpWebResponse httpResponse = null;
                try
                {
                    //Actually downloading the px file
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
                catch (Exception e)
                {
                    bool is405 = false; if (e.Message.Contains("405")) is405 = true;                    
                    //timeout errors and the like
                    G.Writeln2("*** ERROR: Download failed after " + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + " with the following error:");
                    G.Writeln("           " + e.Message);
                    if (is405) G.Writeln("           This error type may indicate an erroneous path, for instance 'http://api.statbank.dk/v1' instead of 'http://api.statbank.dk/v1/data'");
                    throw;
                }

                Encoding encoding = System.Text.Encoding.GetEncoding("Windows-1252");

                //-------- is it possible to make it into a file like this?
                //using (Stream output = File.OpenWrite("file.dat"))
                //using (Stream input = http.Response.GetResponseStream())
                //{
                //    input.CopyTo(output);
                //}

                //It seems the px file is in ANSI/win 1252: the file also reports this at the top: CHARSET="ANSI"; CODEPAGE = "Windows-1252";
                //Setting UTF8 here will fail!           
                //This encoding stuff is probably necessary
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), encoding))
                {
                    pxLinesText = streamReader.ReadToEnd();  //encoded as Windows-1252                    
                }

                double length = Math.Round((double)pxLinesText.Length / 1000000d, 1);
                string size = "size " + length + " MB, ";
                if (length == 0.0) size = "size < 0.1 MB, ";

                G.Writeln("--> Download of data file ended (" + size + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + ")");

                string source = o1.dbUrl + ", " + o1.fileName;

                if (o1.fileName2 != null)
                {
                    string fileName = null;
                    try
                    {
                        fileName = Program.CreateFullPathAndFileName(Program.AddExtension(o1.fileName2, "." + "px"));
                        using (FileStream fs = Program.WaitForFileStream(fileName, Program.GekkoFileReadOrWrite.Write))
                        using (StreamWriter sw = G.GekkoStreamWriter(fs))
                        {
                            sw.Write(pxLinesText);
                            sw.Flush();
                            sw.Close();
                        }
                        G.Writeln2("Downloaded px file: " + fileName);
                        G.Writeln("The file can be read with IMPORT<px>");
                    }
                    catch (Exception e)
                    {
                        G.Writeln2("+++ WARNING: DOWNLOAD<file> failed: is '" + fileName + "' blocked?");
                    }
                }
                else
                {                    
                    int vars;
                    GekkoTime perStart;
                    GekkoTime perEnd;
                    Program.ReadPx(o1.opt_array, true, null, source, tableName, codesHeaderJson, pxLinesText, out vars, out perStart, out perEnd);
                }
            }
        }

        private static void DownloadOld(O.Download o1)
        {
            string input = Program.options.folder_working + "\\" + o1.fileName;
            string jsonCode = Program.GetTextFromFileWithWait(input);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(o1.dbUrl);
            httpWebRequest.Timeout = 24 * 60 * 60 * 1000; //24 hours max        
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            Dictionary<string, object> jsonTree = null;
            try
            {
                jsonTree = (Dictionary<string, object>)serializer.DeserializeObject(jsonCode);
            }
            catch (Exception e)
            {
                G.Writeln2("*** ERROR: The .json file does not seem correctly formatted.");
                G.Writeln("           Message: " + e.Message);
                throw;
            }

            string tableName = (string)jsonTree["table"];
            List<string> codesHeaderJson = new List<string>();

            object[] o = (object[])jsonTree["variables"];
            foreach (Dictionary<string, object> oo in o)
            {
                codesHeaderJson.Add((string)oo["code"]);
            }

            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            }
            catch (Exception e)
            {
                //May get something like this: System.Net.WebException: Der kunne ikke oprettes forbindelse til fjernserveren ---> System.Net.Sockets.SocketException: Det blev forsøgt at få adgang til en socket på en måde, der er forbudt af den pågældende sockets adgangstilladelser 91.208.143.3:80
                G.Writeln2("*** ERROR: Connection failed with the following error:");
                G.Writeln("           " + e.Message);
                throw;
            }

            using (streamWriter)
            {
                streamWriter.Write(jsonCode);
                streamWriter.Flush();
                streamWriter.Close();
                string pxLinesText = null;

                string path = Globals.localTempFilesLocation + "\\pc-axis-data.px";
                DateTime t0 = DateTime.Now;

                using (FileStream fs = Program.WaitForFileStream(path, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    G.Writeln2("--> Download of data file start...");
                    HttpWebResponse httpResponse = null;
                    try
                    {
                        //Actually downloading the px file
                        httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    }
                    catch (Exception e)
                    {
                        //timeout errors and the like
                        G.Writeln2("*** ERROR: Download failed after " + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + " with the following error:");
                        G.Writeln("           " + e.Message);
                        throw;
                    }

                    Encoding encoding = System.Text.Encoding.GetEncoding("Windows-1252");

                    //-------- is it possible to make it into a file like this?
                    //using (Stream output = File.OpenWrite("file.dat"))
                    //using (Stream input = http.Response.GetResponseStream())
                    //{
                    //    input.CopyTo(output);
                    //}

                    //It seems the px file is in ANSI/win 1252: the file also reports this at the top: CHARSET="ANSI"; CODEPAGE = "Windows-1252";
                    //Setting UTF8 here will fail!           
                    //Putting the temporary file into a string             
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), encoding))
                    {
                        pxLinesText = streamReader.ReadToEnd();  //encoded as Windows-1252
                        sw.Write(pxLinesText);
                        sw.Flush();
                    }
                }

                string size = null;
                if (File.Exists(path))
                {
                    double length = Math.Round((double)new System.IO.FileInfo(path).Length / 1000000d, 1);
                    size = "size " + length + " MB, ";
                }

                G.Writeln("--> Download of data file ended (" + size + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + ")");

                string source = o1.dbUrl + ", " + o1.fileName;

                if (o1.fileName2 != null)
                {
                    string fileName = null;
                    try
                    {
                        fileName = Program.CreateFullPathAndFileName(Program.AddExtension(o1.fileName2, "." + "px"));
                        using (FileStream fs = Program.WaitForFileStream(fileName, Program.GekkoFileReadOrWrite.Write))
                        using (StreamWriter sw = G.GekkoStreamWriter(fs))
                        {
                            sw.Write(pxLinesText);
                            sw.Flush();
                            sw.Close();
                        }
                        G.Writeln2("Downloaded px file: " + fileName);
                        G.Writeln("The file can be read with IMPORT<px>");
                    }
                    catch (Exception e)
                    {
                        G.Writeln2("+++ WARNING: DOWNLOAD<file> failed: is '" + fileName + "' blocked?");
                    }
                }
                else
                {
                    int vars;
                    GekkoTime perStart;
                    GekkoTime perEnd;
                    Program.ReadPx(o1.opt_array, true, null, source, tableName, codesHeaderJson, pxLinesText, out vars, out perStart, out perEnd);                    
                }
            }
        }
    }
}
