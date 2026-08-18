/*
    Gekko Timeseries Software (www.t-t.dk/gekko)..
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

using System.Text.RegularExpressions;
using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Data;
using ProtoBuf;
using System.Linq;
using System.IO.Compression;

namespace Gekko
{
    public static class DlinkSetup
    {       
        
        /// <summary>
        /// When called, parentPath will be the folder wherein the folder \.git resides, and
        /// rhs will be == "makrobk_grunddata/_utilities/githooks".
        /// </summary>
        /// <param name="parentOfGitFolder"></param>
        public static void DlinkFunction(string parentOfGitFolder, bool activate)
        {
            if (G.DlinkDebug()) MessageBox.Show("GitHooks() called with " + parentOfGitFolder + ", activate " + activate);

            string hooksPath = Path.Combine(parentOfGitFolder, ".git", "hooks");
            if (!Directory.Exists(hooksPath)) new Error("Could not find folder '" + hooksPath + "'");

            try
            {
                // ----------------------------------------------------------------------------------------------------------                                            
                string gekkoPath = Path.Combine(G.CleanupFolderName(Program.options.databank_dlink_folder_blobs, false), "_utilities", "Gekko").Replace("\\", "/");
                string gekkoExePath = Path.Combine(G.CleanupFolderName(Program.options.databank_dlink_folder_blobs, false), "_utilities", "Gekko", "Gekko.exe").Replace("\\", "/");
                string _common = @$"#!/bin/sh
ROOT_DIR=$(git rev-parse --show-toplevel 2>/dev/null)
STAGED_FILES=$(git -C ""${{ROOT_DIR}}"" ls-files --cached -- ':(icase)*.dlink')
FORMATTED_FILES=$(echo ""$STAGED_FILES"" | sed ""s/^/'/;s/$/'/"" | paste -sd, -)
#powershell.exe -Command ""(New-Object -ComObject WScript.Shell).Popup('... ' + $FORMATTED_FILES, 0, 'Message', 64)""
#powershell.exe -Command ""(New - Object - ComObject WScript.Shell).Popup('.1. ' + $FORMATTED_FILES, 0, 'Message', 64)""
cmd.exe //c ""{gekkoExePath}"" ""-dlink:'$1',$FORMATTED_FILES"" ""-dlinkw:'$ROOT_DIR'""
";
                // ----------------------------------------------------------------------------------------------------------
                string post_checkout = $@"#!/bin/sh
bash ""$(dirname ""$0"")/_common"" ""post-checkout""
";
                // ----------------------------------------------------------------------------------------------------------
                string post_merge = $@"#!/bin/sh
bash ""$(dirname ""$0"")/_common"" ""post-merge""
";
                // ----------------------------------------------------------------------------------------------------------
                string pre_commit = $@"#!/bin/sh
bash ""$(dirname ""$0"")/_common"" ""pre-commit""
";
                // ----------------------------------------------------------------------------------------------------------
                string pre_push = $@"#!/bin/sh
# Only for extra safety, not strictly necessary
bash ""$(dirname ""$0"")/_common"" ""pre-push""
";
                // ----------------------------------------------------------------------------------------------------------

                var hooks = new Dictionary<string, string> { { "_common", _common }, { "post-checkout", post_checkout }, { "post-merge", post_merge }, { "pre-commit", pre_commit }, { "pre-push", pre_push } };
                
                if (activate)
                {
                    int counter = 0;
                    Directory.CreateDirectory(gekkoPath);                    
                    foreach (var hook in hooks)
                    {
                        string filePath = Path.Combine(hooksPath, hook.Key);
                        string contentToWrite = hook.Value;
                        bool b = G.WriteIfChanged(filePath, contentToWrite);
                        if (b) counter++;
                    }
                    if (counter == 0) new Writeln("Git hook files in folder '" + hooksPath + "' are already up to date");
                    else
                    {
                        new Writeln("Added or changed " + counter + " Git hook files in folder '" + hooksPath + "'");
                    }
                }
                else
                {                    
                    int c = 0;
                    foreach (var hook in hooks)
                    {
                        string filePath = Path.Combine(hooksPath, hook.Key);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            c++;
                        }                        
                    }
                    if (c == 0) new Writeln("Did not find any Git hook files to delete in folder '" + hooksPath + "'");
                    else new Writeln("Deleted " + c + " Git hook files from folder '" + hooksPath + "'");
                }
            }
            catch
            {
                if (activate) new Error("Failed to write Git hooks files in folder '"+ hooksPath + "'");
                else new Error("Failed to remove Git hooks files in folder '"+ hooksPath + "'");
                new Error();
            }
        }


    }

    public static class DlinkAutoDlinkFiles
    {        
        /// <summary>
        /// Handles blobs, for .dlink
        /// </summary>
        /// <param name="dataFile"></param>
        public static void Blob(string dataFile, long? nVariables, P p)
        {
            string hash = null;
            long? size = null;
            DateTime? stamp = null;
            if (Program.options.databank_dlink)
            {
                //Note: just because a .dlink file is constructed, this it not the same
                //      as that it has to go into blobs storage.

                string dlinkFile = Dlink_FromDataFileToDlinkFile(dataFile);

                if (dlinkFile == null)
                {
                    //Do nothing: may be a databank on some other drive
                }
                else
                {
                    if (File.Exists(dataFile))
                    {
                        hash = DlinkHooks.BlobsHash(dataFile); //TODO: WithWait or WaitFor...
                    }
                    size = (new FileInfo(dataFile)).Length;
                    if (!Directory.Exists(Path.GetDirectoryName(dlinkFile)))
                    {
                        if (true)
                        {
                            MessageBox.Show("The folder '" + Path.GetDirectoryName(dlinkFile) + "' is created");
                            Directory.CreateDirectory(Path.GetDirectoryName(dlinkFile));
                        }
                        else
                        {
                            MessageBox.Show("The folder '" + Path.GetDirectoryName(dlinkFile) + "' does not exist for ." + Program.options.databank_dlink_name + " file writing");
                            new Error();
                        }
                    }
                    DlinkFile blobInfo = new DlinkFile(hash, size, stamp, nVariables, null);
                    G.YamlWriter<DlinkFile>(blobInfo, dlinkFile);
                }
            }
        }

        private static string Dlink_FromDataFileToDlinkFile(string dataFile)
        {
            // datastart1  k:\\MAKROBK_KILDE\\2025_10_01
            // datastart2  k:\\MAKROBK
            // m           K:\MAKROBK_KILDE\2025_10_01\tth\test\biver\_uddata\x.csv
            // m2          tth\test\biver\_uddata\x.csv
            // m3          tth\test\makrobk_grunddata\biver\_uddata\x.csv
            // m4          tth\test\makrobk_grunddata\biver\_uddata_dlink\x.csv
            // m5          tth\test\makrobk_grunddata\biver\_uddata_dlink\x.csv.dlink
            // m6          k:\\MAKROBK\tth\test\makrobk_grunddata\biver\_uddata_dlink\x.csv.dlink   (output)            

            if (!Path.IsPathRooted(Program.options.databank_dlink_folder_data)) new Error("Expected path '" + Program.options.databank_dlink_folder_data + "' to be absolute");
            List<string> dataStart1 = Stringlist.Path_FromStringToList(Program.options.databank_dlink_folder_data);
            if (!Path.IsPathRooted(Program.options.databank_dlink_folder_progs)) new Error("Expected path '" + Program.options.databank_dlink_folder_progs + "' to be absolute");
            List<string> dataStart2 = Stringlist.Path_FromStringToList(Program.options.databank_dlink_folder_progs);            
            if (!Path.IsPathRooted(dataFile)) new Error("Expected path '" + dataFile + "' to be absolute");
            List<string> m1 = Stringlist.Path_FromStringToList(dataFile);
            List<string> m2 = Stringlist.Path_RemoveStart(m1, dataStart1);
            List<string> m3 = m2.ToList(); //copy
            if (!G.NullOrBlanks(Program.options.databank_dlink_folder_remove1))
            {
                //Add this in the middle...
                m3.Insert(2, Program.options.databank_dlink_folder_remove1);
            }            
            List<string> m4 = Stringlist.Path_ReplaceString(m3, Program.options.databank_dlink_folder_replace1a, Program.options.databank_dlink_folder_replace1b, 1);
            m4 = Stringlist.Path_ReplaceString(m4, Program.options.databank_dlink_folder_replace2a, Program.options.databank_dlink_folder_replace2b, 1);
            List<string> m5 = m4.ToList();
            m5[m5.Count - 1] += "." + Program.options.databank_dlink_name;
            List<string> m6 = m5.ToList();
            m6.InsertRange(0, dataStart2);
            return Stringlist.Path_FromListToString(m6, "\\");

            ////string s1 = G.Replace(dataFile, Program.options.databank_dlink_folder_data, "", 1);

            ////dataFile:          K:\MAKROBK_KILDE\2025_10_01\tth\test\biver\_uddata\x.csv
            ////f2:                K:\MAKROBK\tth\test\makrobk_grunddata\biver\_progs
            ////s2:                \tth\test\makrobk_grunddata\biver  
            ////s2a:               \tth\test\biver   
            ////s3:                K:\MAKROBK_KILDE\2025_10_01\tth\test\biver
            ////s4:                \_uddata\x.csv
            ////s5:                K:\MAKROBK\tth\test\makrobk_grunddata\biver\_progs\_uddata_dlink\x.csv.dlink
            //string f2 = G.CleanupFolderName(O.ConvertToString(Functions.Helper_Runfolder(new IVariable[0], p)), false); //.dlink file, c:\Thomas\Gekko\BlobsTest\tth\staging
            //if (G.NullOrBlanks(f2)) f2 = Program.options.folder_working; //Run directly: in that case we must assume the working folder
            //string s2 = DlinkCommon.Dlink_HandleProgsPath(f2);
            //string s2a = DlinkCommon.Dlink_HandleRemove(s2);
            //string s3 = Path.Combine(Program.options.databank_dlink_folder_data, s2a.TrimStart('\\'));
            //if (!dataFile.StartsWith(s3 + "\\", StringComparison.OrdinalIgnoreCase)) new Error("Problem with .dlink file path: based on the .gcm file path, the datafile path '" + dataFile + "' was expected to start with the path '" + s3 + "'");
            //string s4 = G.Replace(dataFile, s3, "", StringComparison.OrdinalIgnoreCase, 1);
            
            //string s5 = Path.Combine(Program.options.databank_dlink_folder_progs, s2.TrimStart('\\'), s4.TrimStart('\\'));
            //s5 = DlinkCommon.AddOrRemoveDlinkFromInddataOrUddata(s5, true);
            //s5 = s5 + "." + Program.options.databank_dlink_name;            
            //return s5;
        }
    }

    public static class DlinkHooks
    {
        /// <summary>
        /// DLink() must be fed with a list of .dlink files to update. The list comes from Git via a Git hook. In principle, Gekko
        /// could look at all .dlink files, but some of these may be irrelevant and not versioned.
        /// </summary>
        /// <param name="args"></param>
        public static void DLinkCalledFromGitHook(string[] args)
        {
            string cacheIndexDlinkFile = Path.Combine(Program.ProgramFolderGit(), ".git", "index_dlink");
            string gitConfigFile = Path.Combine(Program.ProgramFolderGit(), ".git", "config");
            string s2 = args[0].Substring("dlink:".Length);
            MatchCollection matches = Regex.Matches(s2, @"'([^']*)'");
            if (G.DlinkDebug()) MessageBox.Show("xxx " + Stringlist.GetListWithCommas(args));
            if (G.DlinkDebug()) MessageBox.Show("yyy " + matches.Count);
            List<string> dlinkFiles = new List<string>();
            string type = matches[0].Groups[1].Value;
            for (int i = 1; i < matches.Count; i++)
            {
                string s = matches[i].Groups[1].Value;
                if (G.NullOrBlanks(s)) continue; //First time, it can have a '' as the first element
                dlinkFiles.Add(s);
            }
            if (G.DlinkDebug()) MessageBox.Show("zzz " + dlinkFiles.Count);
            List<string> getFilesNew = new List<string>();
            List<string> getFilesOverwrite = new List<string>();
            List<string> putFiles = new List<string>();

            CacheIndexDlink cacheIndexDlink = new CacheIndexDlink(); //empty
            if (File.Exists(cacheIndexDlinkFile))
            {
                try
                {
                    cacheIndexDlink = Program.ProtobufRead<CacheIndexDlink>(cacheIndexDlinkFile);
                }
                catch
                {
                    if (G.DlinkDebug()) MessageBox.Show("Loading " + cacheIndexDlinkFile + " failed");
                }
            }

            GekkoDictionary<string, bool> datafiles = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string dlinkFile2 in dlinkFiles) //Could probably be parallelized
            {
                string dlinkFile = G.CleanupFolderName(dlinkFile2, false);
                string dLinkFileWithPath = Path.Combine(Program.ProgramFolderGit(), dlinkFile);
                if (!File.Exists(dLinkFileWithPath))
                {
                    MessageBox.Show("This ." + Program.options.databank_dlink_name + " file does not exist: '" + dLinkFileWithPath + "'");
                    new Error();
                }
                DlinkFile dlinkFileData = G.YamlReader<DlinkFile>(dLinkFileWithPath);
                string dataFile = Dlink_FromDlinkFileToDataFile(dLinkFileWithPath);
                datafiles.Add(dataFile, false); //for cleanup purposes
                if (G.NullOrBlanks(dataFile))
                {
                    MessageBox.Show("Datafile string is null"); new Error();
                }

                FileInfo fi1 = new FileInfo(dataFile); //File may not exist
                RealFile realFile = new RealFile(fi1.FullName, null, fi1.Length, fi1.LastWriteTimeUtc, fi1.Exists);

                // --------------------------------------------------------------------------------------------------
                //                              datafile exists
                //                             yes            no
                //  ----------------------------------------------------------
                //  .dlink exists    yes       A              B
                //                   no        C              D
                //  ----------------------------------------------------------
                //  A: Check that they correspond etc. --> but only if .dlink file has been already added/committed.
                //  B: Try to get file from blobs      --> but only if .dlink file has been already added/committed.
                //  C: May just be a datafile copied into datafiles, not being read by Gekko yet
                //  D: Not relevant
                //  Note: were are obvisously in A or B here, since we are handling a .dlink file.
                // --------------------------------------------------------------------------------------------------                
                CacheIndexDlinkElement cacheIndexDlinkElement = null;
                cacheIndexDlink.storage.TryGetValue(realFile.name, out cacheIndexDlinkElement);
                //After this method call, realFile may change regarding .hash and .exists fields (and only those)
                bool isDataFileOk = IsDLlinkHelperFileOk(realFile.name, dlinkFileData, cacheIndexDlinkElement, ref realFile); //regarding last two args: either both non-null or both null
                if (isDataFileOk)
                {
                    //Check that we have the file in blobs folder, else add it there
                    BlobsFile(false, realFile.name, dlinkFileData.hash, G.CleanupFolderName(Program.options.databank_dlink_folder_blobs, false), getFilesNew, getFilesOverwrite, putFiles);
                    //Force-update the cache entry
                    cacheIndexDlink.storage[realFile.name] = new CacheIndexDlinkElement(realFile.name, realFile.hash, realFile.size, realFile.stamp);
                }
                else
                {
                    //Get it from blobs (A or B)
                    BlobsFile(true, realFile.name, dlinkFileData.hash, G.CleanupFolderName(Program.options.databank_dlink_folder_blobs, false), getFilesNew, getFilesOverwrite, putFiles);
                    FileInfo fi2 = new FileInfo(realFile.name);
                    //We update the realFile, because its contents have changed
                    realFile = new RealFile(realFile.name, dlinkFileData.hash, fi2.Length, fi2.LastWriteTimeUtc, true);
                    //Force-update the cache entry              
                    cacheIndexDlink.storage[realFile.name] = new CacheIndexDlinkElement(realFile.name, dlinkFileData.hash, realFile.size, realFile.stamp);
                }
            }

            List<string> filesToRemove = cacheIndexDlink.storage.Keys.Where(key => !datafiles.ContainsKey(key)).ToList();
            foreach (var fileToRemove in filesToRemove) cacheIndexDlink.storage.Remove(fileToRemove); //Else the storage will always grow
            try
            {
                Program.ProtobufWrite(cacheIndexDlink, cacheIndexDlinkFile); //always refresh the file
            }
            catch
            {
                if (G.DlinkDebug()) MessageBox.Show("Writing " + cacheIndexDlinkFile + " failed");
            }
            DLinkCalledFromGitHookReporting(type, getFilesNew, getFilesOverwrite, putFiles);
        }

        private static string Dlink_FromDlinkFileToDataFile(string dlinkFile)
        {
            //dlinkFile:         K:\MAKROBK\tth\test\makrobk_grunddata\biver\_progs\_uddata_dlink\x.csv.dlink
            //s2a:               \tth\test\biver\_uddata_dlink\x.csv.dlink

            //fileNameAndPath:   K:\MAKROBK_KILDE\2025_10_01\tth\test\biver\_uddata\x.csv
            //f2:                K:\MAKROBK\tth\test\makrobk_grunddata\biver\_progs
            //s2:                \tth\test\makrobk_grunddata\biver  
            //s2a:               \tth\test\biver   
            //s3:                K:\MAKROBK_KILDE\2025_10_01\tth\test\biver
            //s4:                \_uddata\x.csv
            //s5:                

            string s2 = DlinkCommon.Dlink_HandleProgsPath(dlinkFile);
            string s2a = DlinkCommon.Dlink_HandleRemove(s2);
            string s3 = Path.Combine(Program.options.databank_dlink_folder_data, s2a.TrimStart('\\'));
            s3 = DlinkCommon.AddOrRemoveDlinkFromInddataOrUddata(s3, false);
            if (!s3.EndsWith("." + Program.options.databank_dlink_name, StringComparison.OrdinalIgnoreCase)) new Error("Expected dlink file to end with " + "." + Program.options.databank_dlink_name);
            string s4 = s3.Substring(0, s3.Length - ("." + Program.options.databank_dlink_name).Length);
            return s4;
        }

        /// <summary>
        /// Returns true if file is ok, else it must be fetched from blobs
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="syncTimeUtc"></param>
        /// <param name="dlinkFileData"></param>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static bool IsDLlinkHelperFileOk(string dataFile, DlinkFile dlinkFileData, CacheIndexDlinkElement cacheIndexDlinkElement, ref RealFile realFile)
        {
            if (!realFile.exists) return false; //In that case, realFile.stamp etc. are null too
            if (realFile.size != dlinkFileData.size) return false;
            //Here we know that the data file exists and is of the right size. Now we check stamp.
            double krit = 2d; //2s: Krit can be quite small: it is taken from the acutual timestamp in the user folder (with \.git folder), on the same server. If the files are copied somewhere else, some precision may be lost, so therefore 2s.
            string realHash;
            if (cacheIndexDlinkElement != null && cacheIndexDlinkElement.size == realFile.size && cacheIndexDlinkElement.stamp != null && Math.Abs(((DateTime)realFile.stamp - (DateTime)cacheIndexDlinkElement.stamp).TotalSeconds) < krit)
            {
                //EASY way
                //dataFileSha256 is ok as taken from index_dlink file, but the hash must still be checked against the .dlink file hash
                realHash = cacheIndexDlinkElement.hash;
            }
            else
            {
                //HARD way
                //We now need to calc the sha256 physically.                
                realHash = BlobsHash(dataFile);
            }
            realFile = new RealFile(realFile.name, realHash, realFile.size, realFile.stamp, true);
            if (dlinkFileData.hash != realHash) return false;
            return true;
        }

        private static void DLinkCalledFromGitHookReporting(string type, List<string> filesNew, List<string> filesOverwritten, List<string> putFiles)
        {
            string s = null;
            if (filesNew.Count + filesOverwritten.Count > 0)
            {
                s = "User data file folder: ";
                string s2a = "are"; if (filesNew.Count < 2) s2a = "is";
                string s2b = "are"; if (filesOverwritten.Count < 2) s2b = "is";
                if (filesNew.Count > 0 && filesOverwritten.Count == 0)
                {
                    s += filesNew.Count + " new file" + G.S(filesNew.Count) + " " + s2a + " added ";
                }
                else if (filesNew.Count == 0 && filesOverwritten.Count > 0)
                {
                    s += filesOverwritten.Count + " file" + G.S(filesOverwritten.Count) + " " + s2b + " overwritten ";
                }
                else
                {
                    s += filesNew.Count + " new file" + G.S(filesNew.Count) + " " + s2a + " added, " + filesOverwritten.Count + " file" + G.S(filesOverwritten.Count) + " " + s2b + " overwritten ";
                }
                s += " (" + type + ")";
                s += G.NL;
                foreach (string f in filesNew)
                {
                    s += G.NL + f + " (added)";
                }
                foreach (string f in filesOverwritten)
                {
                    s += G.NL + f + " (overwritten)";
                }                
            }
            else
            {
                s = "User data file folder: no data files added or overwritten.";
                //s += G.NL + G.NL;
                //s += "(For now, this message is kept --> may be omitted when data versioning has matured).";
            }

            if (putFiles.Count > 0)
            {
                s += G.NL + G.NL;
                s += "Versions storage: " + putFiles.Count + " new data file" + G.S(putFiles.Count) + " added to long-term storage:";
                foreach (string f in putFiles)
                {
                    s += G.NL + f;
                }
            }

            WindowMessageBox w = new WindowMessageBox(EMessageBox.Normal);
            w.Height = 300;
            w.Width = 600;
            w.textBox1.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            w.textBox1.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            w.textBox1.TextWrapping = System.Windows.TextWrapping.NoWrap;
            w.textBox1.Text = s;
            w.textBox1.FontFamily = new System.Windows.Media.FontFamily("Courier New");
            w.textBox1.FontSize = 11;
            w.ShowDialog();
        }

        public static string BlobsHash(string filePath)
        {
            //
            // TODO: here we could do datahash for .gbk files instead (and handle specialFlagForTraces too)
            // If we can loop through all IVariables, while sorting dict keys before calling children, we
            // can use an incremental sha256 engine. For series, we need to stamp/inject the first observation as
            // a freq + super + sub + subsub. We need to rempace G.IsNumericalError() with double.NaN.
            // Also, scalars and matrices and maps. Labels for matrices? Should we truncate precision?
            //
            string hash = null;
            bool hasTraces = false;
            bool isGbk = G.Equal(Path.GetExtension(filePath), ".gbk");

            if (isGbk)
            {
                try
                {
                    using (ZipArchive archive = ZipFile.OpenRead(filePath))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {                            
                            if (G.Equal(entry.Name, Globals.protobufFileName3))
                            {
                                hasTraces = true;
                            }
                            else if (G.Equal(entry.Name, Globals.databankInfoName))
                            {
                                try //So that hasTraces has a chance to become == true
                                {
                                    Program.ReadInfo readInfo = new Program.ReadInfo();
                                    string databankVersion = null;
                                    string traceVersion = null;
                                    string tempFileNameWithPath = Program.WaitForZipExtractFileEntryToTempFile(entry, filePath);
                                    Program.GetDatabankInfo(readInfo, tempFileNameWithPath, out databankVersion, out traceVersion);
                                    hash = readInfo.dataHashFull;
                                }
                                catch { }
                            }
                        }
                    }
                }
                catch
                {
                    //No failing
                }
            }

            // The trace hash is baked in now
            //if (isGbk)
            //{
            //    if (hash == null) hash = G.GetSha256FromFile(filePath); //Then we take the file hash instead
            //    if (specialFlagForTraces)
            //    {
            //        //Below it is ensured that two files with same datahash, but where
            //        //there are traces in one file and not in another will have different hashes.
            //        if (hasTraces)
            //        {
            //            hash = hash.Substring(0, hash.Length - 1) + "1"; //always ends with 1
            //        }
            //        else
            //        {
            //            hash = hash.Substring(0, hash.Length - 1) + "0"; //always ends with 0
            //        }
            //    }
            //}
            //else
            //{
            //    hash = G.GetSha256FromFile(filePath);
            //}

            if (hash == null) hash = G.GetSha256FromFile(filePath);

            return hash;
        }

        /// <summary>
        /// Returns true if 
        /// </summary>
        /// <param name="isGet"></param>
        /// <param name="fileName"></param>
        /// <param name="sha256"></param>
        /// <param name="blobsFolder"></param>
        /// <param name="getFilesNew"></param>
        /// <param name="getFilesOverwrite"></param>
        public static void BlobsFile(bool isGet, string fileName, string sha256, string blobsFolder, List<string> getFilesNew, List<string> getFilesOverwrite, List<string> putFiles)
        {
            // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
            // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
            // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO            
            // -----------------------------------------------------------
            // ==== Think about atomic writes and simultaneous threads
            // -----------------------------------------------------------
            // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
            // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO
            // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO

            if (!Directory.Exists(blobsFolder))
            {
                MessageBox.Show("Folder '" + blobsFolder + "' does not exist for file blobs/storage");
                new Error();
            }
            if (!File.Exists(Path.Combine(blobsFolder, "blobsroot.ini")))
            {
                MessageBox.Show("File '" + Path.Combine(blobsFolder, "blobsroot.ini") + "' does not exist. This is a safety precaution: you may add an empty file with that name.");
                new Error();
            }
            string shapart1 = sha256.Substring(0, 2);
            string shapart2 = sha256; //We do not want file "abcdefg" to become "\ab\cdefg", but prefer it to become "\ab\abcdefg". Easier to search for etc. even though Git does the former.
            if (isGet)
            {
                // ------------------------------------
                // Getting
                // ------------------------------------
                if (!File.Exists(Path.Combine(blobsFolder, shapart1, shapart2)))
                {
                    MessageBox.Show("For '" + fileName + "', could not find blob file '" + Path.Combine(blobsFolder, shapart1, shapart2) + "'");
                    new Error();
                }
                else
                {
                    if (File.Exists(fileName)) getFilesOverwrite.Add(fileName);
                    else getFilesNew.Add(fileName);
                    BlobsFileGet(fileName, Path.Combine(blobsFolder, shapart1, shapart2));
                }
            }
            else
            {
                // ------------------------------------
                // Putting
                // ------------------------------------
                string blobsFile = Path.Combine(blobsFolder, shapart1, shapart2);
                if (!Directory.Exists(Path.Combine(blobsFolder, shapart1)))
                {
                    Directory.CreateDirectory(Path.Combine(blobsFolder, shapart1));
                    BlobsFilePut(fileName, blobsFile);
                    putFiles.Add(fileName);
                }
                else
                {
                    if (File.Exists(Path.Combine(blobsFolder, shapart1, shapart2)))
                    {
                        //No need to copy it: same file is already there
                        //TODO TODO TODO
                        //TODO TODO TODO
                        //TODO TODO TODO ---> if a gbk is newer but with same datahash, we could add the new one (may have better meta information)
                        //TODO TODO TODO
                        //TODO TODO TODO
                    }
                    else
                    {
                        BlobsFilePut(fileName, blobsFile);
                        putFiles.Add(fileName);
                    }
                }
            }
        }

        private static void BlobsFileGet(string fileName, string blobsFile)
        {
            //TODO
            //TODO
            //TODO Maybe check that the sha hash is correct after fetching the file.
            //TODO
            //TODO
            if (Globals.alreadyZipped.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase))
            {
                File.Copy(blobsFile, fileName, true); //Allows overwrite, TODO UNZIPPING                    
            }
            else
            {
                using (ZipArchive archive = ZipFile.OpenRead(blobsFile))
                {
                    ZipArchiveEntry entry = archive.GetEntry("storage");
                    if (entry != null)
                    {
                        entry.ExtractToFile(fileName, true);
                    }
                }
            }
            G.ReadOnlyRemove(fileName);
        }

        private static void BlobsFilePut(string fileName, string blobsFile)
        {
            if (Globals.alreadyZipped.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase))
            {
                File.Copy(fileName, blobsFile);
            }
            else
            {
                using (FileStream zipToOpen = new FileStream(blobsFile, FileMode.Create, FileAccess.Write))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                    {
                        ZipArchiveEntry readmeEntry = archive.CreateEntry(Path.GetFileName("storage"));
                        using (Stream writer = readmeEntry.Open())
                        using (FileStream fs = File.OpenRead(fileName))
                        {
                            fs.CopyTo(writer);
                        }
                    }
                }
            }
            G.ReadOnlySet(blobsFile);
        }
    }

    public class DlinkCommon 
    {

        public static string Dlink_HandleProgsPath(string f2)
        {
            //K:\MAKROBK\tth\test\makrobk_grunddata\biver\_progs  -->  \tth\test\makrobk_grunddata\biver  
            if (Globals.tthDlink) f2 = G.Replace(f2, "c:\\Tools\\K\\MAKROBK", "K:\\MAKROBK", StringComparison.OrdinalIgnoreCase, 1);
            if (!f2.StartsWith(Program.options.databank_dlink_folder_progs.Trim(), StringComparison.OrdinalIgnoreCase)) new Error("Problem with .dlink: the data file path '" + f2 + "' was expected was expected to start with the path '" + Program.options.databank_dlink_folder_progs.Trim() + "'");
            string s2 = f2;
            s2 = G.Replace(s2, Program.options.databank_dlink_folder_progs.Trim(), "", StringComparison.OrdinalIgnoreCase, 1);
            s2 = G.Replace(s2, "\\" + Program.options.databank_dlink_folder_remove2 + "", "", StringComparison.OrdinalIgnoreCase, 1);
            return s2;
        }

        public static string Dlink_HandleRemove(string s2)
        {
            //\tth\test\makrobk_grunddata\biver  -->   \tth\test\biver   
            string s2a = s2;
            if (!G.NullOrBlanks(Program.options.databank_dlink_folder_remove1))
            {
                s2a = G.Replace(s2, "\\" + Program.options.databank_dlink_folder_remove1.Trim() + "\\", "\\", StringComparison.OrdinalIgnoreCase, 1);
            }
            return s2a;
        }

        public static string AddOrRemoveDlinkFromInddataOrUddata(string s, bool larger)
        {
            if (larger)
            {
                s = G.Replace(s, "\\" + Program.options.databank_dlink_folder_replace2a + "\\", "\\" + Program.options.databank_dlink_folder_replace2b + "\\", StringComparison.OrdinalIgnoreCase, 1);
                s = G.Replace(s, "\\" + Program.options.databank_dlink_folder_replace1a + "\\", "\\" + Program.options.databank_dlink_folder_replace1b + "\\", StringComparison.OrdinalIgnoreCase, 1);
            }
            else
            {
                s = G.Replace(s, "\\" + Program.options.databank_dlink_folder_replace2b + "\\", "\\" + Program.options.databank_dlink_folder_replace2a + "\\", StringComparison.OrdinalIgnoreCase, 1);
                s = G.Replace(s, "\\" + Program.options.databank_dlink_folder_replace1b + "\\", "\\" + Program.options.databank_dlink_folder_replace1a + "\\", StringComparison.OrdinalIgnoreCase, 1);
            }
            return s;
        }
    }

    [ProtoContract]
    public class CacheIndexDlink
    {
        [ProtoMember(1)]
        public string version = "1.0";
        [ProtoMember(2)]
        public GekkoDictionary<string, CacheIndexDlinkElement> storage = new GekkoDictionary<string, CacheIndexDlinkElement>(StringComparer.OrdinalIgnoreCase);
    }

    [ProtoContract]
    public class CacheIndexDlinkElement
    {
        [ProtoMember(1)]
        public readonly string name = null;
        [ProtoMember(2)]
        public readonly string hash = null;
        [ProtoMember(3)]
        public readonly long? size = null;
        [ProtoMember(4)]
        public readonly DateTime? stamp = null;

        public CacheIndexDlinkElement()
        {
            //For protobuf
        }

        public CacheIndexDlinkElement(string name, string hash, long? size, DateTime? stamp)
        {
            this.name = name;
            this.hash = hash;
            this.size = size;
            this.stamp = stamp;
        }
    }

    public class DlinkFile
    {
        public readonly string version = "1.0";
        public string hash { get; private set; }
        public long? size { get; private set; }
        public DateTime? stamp { get; private set; }
        public long? variables { get; private set; }
        public string extra { get; private set; }

        public DlinkFile()
        {
        }

        public DlinkFile(string hash, long? size, DateTime? stamp, long? nVariables, string extra)
        {
            this.hash = hash;
            this.size = size;
            this.stamp = stamp;
            this.variables = nVariables;
            this.extra = extra;
        }
    }

    public class RealFile
    {
        public readonly string name = null;
        public readonly string hash = null;
        public readonly long? size = null;
        public readonly DateTime? stamp = null;
        public readonly bool exists = false;

        public RealFile(string name, string hash, long? size, DateTime? stamp, bool exists)
        {
            this.name = name;
            this.hash = hash;
            this.size = size;
            this.stamp = stamp;
            this.exists = exists;
        }
    }

}