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

        private static void DownloadNew(O.Download o1)
        {
            string input = Program.options.folder_working + "\\" + o1.fileName;
            string jsonCode = Program.GetTextFromFileWithWait(input); //also removes some kinds of funny characters
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
