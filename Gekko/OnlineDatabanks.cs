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
            try
            {
                //In principle, it would be better to omit this and instead port Gekko for
                //the .NET Framework 4.6.
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2, must be used after 10/12 2019. This probably requires that the user has .NET 4.5 to run the DOWNLOAD.
            }
            catch
            {
                //no reason to fail on this
            }
            string file = Program.FindFile(o1.fileName, null, true, true);
            if (file == null) new Error("The file does not exist: " + o1.fileName);
            string jsonCode = Program.GetTextFromFileWithWait(file); //also removes some kinds of funny characters
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(o1.dbUrl);
            httpWebRequest.Timeout = 24 * 60 * 60 * 1000; //24 hours max        
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            httpWebRequest.UseDefaultCredentials = true;  //to be able to access sumdatabasen from inside DST
            httpWebRequest.Credentials = CredentialCache.DefaultNetworkCredentials;  //seems necessary together with the above
            httpWebRequest.UserAgent = "Gekko/" + Globals.gekkoVersion;  //Pelle Rossau von Hedemann (DST) skriver "Jeg kan i øvrigt anbefale at sætte UserAgent, fx Gekko/2.3.4, på request-objektet, således at denne kan genfindes i loggen. API’et returnerer i øvrigt en header med navnet ” StatbankAPI-Request-Id”, som indeholder et GUID for hvert eneste kald. Denne gør det muligt at identificere det specifikke kald i vores log. Man kan, hvis man ønsker det, opsamle denne id og præsentere den for brugeren på en eller anden måde"
            if (o1.opt_key != null) httpWebRequest.Headers.Add("Authorization", o1.opt_key);

            Dictionary<string, object> jsonTree = null;
            try
            {
                jsonTree = (Dictionary<string, object>)serializer.DeserializeObject(jsonCode);
            }
            catch (Exception e)
            {
                new Warning("The .json file does not seem correctly formatted. " + e.Message);
                //throw;
            }

            bool saved = false;
            int? saved2 = null;

            try
            {
                saved2 = (int)jsonTree["savedQueryId"];
            }
            catch { }
            if (saved2 != null) saved = true;
            

            string tableName = null;
            if (!saved)
            {

                try
                {
                    tableName = (string)jsonTree["table"];
                }
                catch { }
                if (tableName == null)
                {
                    new Warning("You should use \"table\": \"...\", in the .json file");
                }
            }
            
            string format = null;
            try
            {
                format = (string)jsonTree["format"];
            }
            catch { }
            if (format == null || !G.Equal(format, "px"))
            {
                new Warning("You should use \"format\": \"px\", in the .json file");
            }

            List<string> codesHeaderJson = null;
            if (!saved)
            {                
                try
                {
                    object[] o = (object[])jsonTree["variables"];                    
                    foreach (Dictionary<string, object> oo in o)
                    {
                        if (codesHeaderJson == null) codesHeaderJson = new List<string>();
                        codesHeaderJson.Add((string)oo["code"]);
                    }
                }
                catch
                {
                    new Warning("The \"variables\" field in the .json file seems malformed");
                }
            }

            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            }
            catch (Exception e)
            {
                //May get something like this: System.Net.WebException: Der kunne ikke oprettes forbindelse til fjernserveren ---> System.Net.Sockets.SocketException: Det blev forsøgt at få adgang til en socket på en måde, der er forbudt af den pågældende sockets adgangstilladelser 91.208.143.3:80
                new Error("Connection failed with the following error: " + e.Message);
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
                    bool isTransport = false; if (e.InnerException != null && e.InnerException.Message != null && (G.Contains(e.InnerException.Message, "transportforbindelsen") || G.Contains(e.InnerException.Message, "transport connection"))) isTransport = true;
                    //timeout errors and the like
                    using (Error error = new Error())
                    {
                        error.MainAdd("Download failed after " + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + " with the following error: ");
                        error.MainAdd(e.Message + ".");
                        if (e.InnerException != null && e.InnerException.Message != null) error.MainAdd(e.InnerException.Message + ".");
                        if (is405) error.MainAdd("This error type may indicate an erroneous path, for instance 'http://api.statbank.dk/v1' instead of 'http://api.statbank.dk/v1/data'.");
                        if (isTransport) error.MainAdd("The connection demands TSL 1.2, and therefore that Gekko runs on .NET Framework 4.5 or higher.");
                    }
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
                        fileName = Program.CreateFullPathAndFileName(G.AddExtension(o1.fileName2, "." + "px"));
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
                        new Warning("DOWNLOAD<file> failed: is '" + fileName + "' blocked?");
                    }
                }
                else
                {                    
                    int vars;
                    GekkoTime perStart;
                    GekkoTime perEnd;
                    string warning = null;
                    Program.ReadPx(Program.databanks.GetFirst(), o1.opt_array, true, source, tableName, codesHeaderJson, pxLinesText, out vars, out warning, out perStart, out perEnd);
                    if (warning != null) new Warning(warning);
                }
            }
        }

        public static void DownloadJobindsats(O.Download o1)
        {            
            try
            {
                //In principle, it would be better to omit this and instead port Gekko for
                //the .NET Framework 4.6.
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2, must be used after 10/12 2019. This probably requires that the user has .NET 4.5 to run the DOWNLOAD.
            }
            catch
            {
                //no reason to fail on this
            }
                        
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(o1.dbUrl);
            httpWebRequest.Timeout = 24 * 60 * 60 * 1000; //24 hours max        
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";            
            httpWebRequest.UserAgent = "Gekko/" + Globals.gekkoVersion;  //Pelle Rossau von Hedemann (DST) skriver "Jeg kan i øvrigt anbefale at sætte UserAgent, fx Gekko/2.3.4, på request-objektet, således at denne kan genfindes i loggen. API’et returnerer i øvrigt en header med navnet ” StatbankAPI-Request-Id”, som indeholder et GUID for hvert eneste kald. Denne gør det muligt at identificere det specifikke kald i vores log. Man kan, hvis man ønsker det, opsamle denne id og præsentere den for brugeren på en eller anden måde"            
            httpWebRequest.Headers.Add("Authorization", o1.opt_key);

            string jsonCode = null;
            if (o1.fileName != null)
            {
                //string input = Program.options.folder_working + "\\" + o1.fileName;     
                string file = Program.FindFile(o1.fileName, null, true, true);
                if (file == null) new Error("The file does not exist: " + o1.fileName);
                jsonCode = Program.GetTextFromFileWithWait(file); //also removes some kinds of funny characters
            }

            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
            }
            catch (Exception e)
            {
                //May get something like this: System.Net.WebException: Der kunne ikke oprettes forbindelse til fjernserveren ---> System.Net.Sockets.SocketException: Det blev forsøgt at få adgang til en socket på en måde, der er forbudt af den pågældende sockets adgangstilladelser 91.208.143.3:80
                new Error("Connection failed with the following error: " + e.Message);
            }

            using (streamWriter)
            {
                streamWriter.Write(jsonCode);
                streamWriter.Flush();
                streamWriter.Close();

                string outputLines = null;

                DateTime t0 = DateTime.Now;

                G.Writeln2("--> Download of data file start...");
                HttpWebResponse httpResponse = null;
                try
                {
                    //Actually downloading the json/csv file
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                }
                catch (Exception e)
                {
                    bool is405 = false; if (e.Message.Contains("405")) is405 = true;
                    bool isTransport = false; if (e.InnerException != null && e.InnerException.Message != null && (G.Contains(e.InnerException.Message, "transportforbindelsen") || G.Contains(e.InnerException.Message, "transport connection"))) isTransport = true;
                    //timeout errors and the like
                    using (Error error = new Error())
                    {
                        error.MainAdd("Download failed after " + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + " with the following error: ");
                        error.MainAdd(e.Message + ".");
                        if (e.InnerException != null && e.InnerException.Message != null) error.MainAdd(e.InnerException.Message + ".");
                        if (is405) error.MainAdd("This error type may indicate an erroneous path, for instance 'https://api.jobindsats.dk/v1/data/...' instead of 'https://api.jobindsats.dk/v1/data/...'.");
                        if (isTransport) error.MainAdd("The connection demands TSL 1.2, and therefore that Gekko runs on .NET Framework 4.5 or higher.");
                    }
                }

                Encoding encoding = System.Text.Encoding.GetEncoding("Windows-1252");

                //It seems the px file is in ANSI/win 1252: the file also reports this at the top: CHARSET="ANSI"; CODEPAGE = "Windows-1252";
                //Setting UTF8 here will fail!           
                //This encoding stuff is probably necessary
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), encoding))
                {
                    outputLines = streamReader.ReadToEnd();  //encoded as Windows-1252                    
                }

                double length = Math.Round((double)outputLines.Length / 1000000d, 1);
                string size = "size " + length + " MB, ";
                if (length == 0.0) size = "size < 0.1 MB, ";

                G.Writeln("--> Download of data file ended (" + size + G.SecondsFormat((DateTime.Now - t0).TotalMilliseconds) + ")");

                string source = o1.dbUrl + ", " + o1.fileName;

                using (FileStream fs = Program.WaitForFileStream(o1.fileName2, Program.GekkoFileReadOrWrite.Write))
                using (StreamWriter res = G.GekkoStreamWriter(fs))
                {
                    res.Write(outputLines);
                }
            }
        }
    }
}
