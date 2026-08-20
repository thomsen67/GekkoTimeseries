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

        public enum EDlinkSetup
        {
            Activate,
            Deactivate,
            Sync
        }
        
        /// <summary>
        /// When called, parentPath will be the folder wherein the folder \.git resides, and
        /// rhs will be == "makrobk_grunddata/_utilities/githooks".
        /// </summary>
        /// <param name="parentOfGitFolder"></param>
        public static void DlinkFunction(string parentOfGitFolder, EDlinkSetup type)
        {
            if (G.DlinkDebug()) MessageBox.Show("GitHooks() called with " + parentOfGitFolder + ", type " + type.ToString());

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

                if (type == EDlinkSetup.Activate)
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
                else if (type == EDlinkSetup.Deactivate)
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
                else if (type == EDlinkSetup.Sync)
                {
                    new Writeln("Syncing should be automatic when for instance cloning a repo. Therefore this function is not implemented at the moment");
                }
                else new Error();
            }
            catch
            {
                if (type == EDlinkSetup.Activate) new Error("Failed to write Git hooks files in folder '" + hooksPath + "'");
                else if (type == EDlinkSetup.Deactivate) new Error("Failed to remove Git hooks files in folder '" + hooksPath + "'");
                else if (type == EDlinkSetup.Sync) new Error("Failed to sync .dlink files (Git folder: '" + hooksPath + "')");
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
        public static void Blob(string dataFile, long? nVariables, bool force)
        {
            string hash = null;
            long? size = null;
            DateTime? stamp = null;
            if (force || Program.options.databank_dlink)
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
                        hash = DlinkHooks.GetFileHash(dataFile); //TODO: WithWait or WaitFor...
                    }
                    else
                    {
                        new Error("The file '" + dataFile + "' does not exist for .dlink file construction");
                    }
                    // New: for "data hash" file types (see DlinkHooks.IsDataHashFileType), the byte
                    // size is not a reliable proxy for "the data is the same" -- two files holding
                    // the same data can have different byte sizes (e.g. embedded timestamps inside a
                    // .gbk, or different zip/compression settings). Recording a size that flips back
                    // and forth for data that has not really changed also means the .dlink file's
                    // content changes for no real reason -- which shows up in Git as a commit on a
                    // file that, in reality, is unchanged. Leaving it null avoids both problems.
                    size = DlinkHooks.IsDataHashFileType(dataFile) ? (long?)null : (new FileInfo(dataFile)).Length;
                    if (!Directory.Exists(Path.GetDirectoryName(dlinkFile)))
                    {
                        if (true)
                        {
                            //MessageBox.Show("The folder '" + Path.GetDirectoryName(dlinkFile) + "' is created");
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
            if (!G.NullOrBlanks(Program.options.databank_dlink_folder_remove1)) m3.Insert(2, Program.options.databank_dlink_folder_remove1); //hacky, in middle                        
            List<string> m4 = Stringlist.Path_ReplaceString(m3, Program.options.databank_dlink_folder_replace1a, Program.options.databank_dlink_folder_replace1b, 1);
            m4 = Stringlist.Path_ReplaceString(m4, Program.options.databank_dlink_folder_replace2a, Program.options.databank_dlink_folder_replace2b, 1);
            List<string> m5 = m4.ToList();
            m5[m5.Count - 1] += "." + Program.options.databank_dlink_name;
            List<string> m6 = m5.ToList();
            m6.InsertRange(0, dataStart2);
            return Stringlist.Path_FromListToString(m6, "\\");            
        }
    }    

    public static class DlinkHooks
    {

        public static void DLinkFilesCalledFromExe(string[] args, bool function)
        {
            List<string> dlinkFiles = new List<string>();
            if (function)
            {
                dlinkFiles = args.ToList();
            }
            else
            {
                if (G.DlinkDebug()) MessageBox.Show("Producing .dlink");
                string s2 = args[0].Substring("dlinkfiles:".Length);
                MatchCollection matches = Regex.Matches(s2, @"'([^']*)'");                
                for (int i = 0; i < matches.Count; i++)
                {
                    string s = matches[i].Groups[1].Value;
                    dlinkFiles.Add(s);
                }
            }

            if (dlinkFiles.Count == 0)
            {
                string s2 = "Producing 0 dlink files";
                if (function) new Error(s2);
                MessageBox.Show("*** ERROR: " + s2); //We want this to show
                return;
            }

            try
            {
                foreach (string dlinkFile2 in dlinkFiles) //Could probably be parallelized
                {
                    DlinkAutoDlinkFiles.Blob(dlinkFile2, null, true); //We do not know the number of variables, so it is set to null
                }
            }
            catch
            {
                string s2 = "Producing " + dlinkFiles.Count + " dlink file" + G.S(dlinkFiles.Count) + " failed";
                if (function) new Error(s2);
                else MessageBox.Show("*** ERROR: " + s2); //We want this to show
                return;
            }
            finally
            {
                DlinkHashCache.Save(); //Persist any hashes computed above, even if the batch failed partway through
            }
            string s3 = "Producing " + dlinkFiles.Count + " dlink file" + G.S(dlinkFiles.Count) + " succeeded";
            if (function) new Writeln(s3);
            else Console.WriteLine(s3); //This will probably not show in output, but never mind

        }
        
        /// <summary>
        /// DLink() must be fed with a list of .dlink files to update. The list comes from Git via a Git hook. In principle, Gekko
        /// could look at all .dlink files, but some of these may be irrelevant and not versioned.
        /// </summary>
        /// <param name="args"></param>
        public static void DLinkCalledFromGitHook(string[] args)
        {
            //MessageBox.Show("!?!");
            string gitFolder = null;
            if (args.Length >= 2 && args[1].StartsWith("-dlinkw:"))
            {
                gitFolder = G.StripQuotes(args[1].Substring("-dlinkw:".Length)); //The path to \.git is sent from the Git hook
                if (Globals.tthDebug) File.WriteAllText("c:\\b-tth\\test1", gitFolder);
                if (!G.NullOrBlanks(Program.options.databank_dlink_folder_replace3a))
                {
                    gitFolder = G.Replace(gitFolder, Program.options.databank_dlink_folder_replace3a, Program.options.databank_dlink_folder_replace3b, StringComparison.OrdinalIgnoreCase, 1);
                }
                if (Globals.tthDebug) File.WriteAllText("c:\\b-tth\\test2", gitFolder);
                if (!Directory.Exists(gitFolder))
                {
                    MessageBox.Show("*** Error: The folder '" + gitFolder + "' could not be found (parent of \\.git folder)");
                    new Error();
                }
            }
            if (G.NullOrBlanks(gitFolder))
            {
                MessageBox.Show("*** Error: Could not get the path to the \\.git folder)");
                new Error();
            }
            string s2 = args[0].Substring("dlink:".Length);
            MatchCollection matches = Regex.Matches(s2, @"'([^']*)'");            
            List<string> dlinkFiles = new List<string>();
            string type = matches[0].Groups[1].Value;
            for (int i = 1; i < matches.Count; i++)
            {
                string s = matches[i].Groups[1].Value;
                if (G.NullOrBlanks(s)) continue; //First time, it can have a '' as the first element
                dlinkFiles.Add(s);
            }            
            List<string> getFilesNew = new List<string>();
            List<string> getFilesOverwrite = new List<string>();
            List<string> putFiles = new List<string>();            
            
            //GekkoDictionary<string, bool> datafiles = new GekkoDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (string dlinkFile2 in dlinkFiles) //Could probably be parallelized
            {                
                string dLinkFileWithPath = Path.Combine(G.CleanupFolderName(gitFolder, false), G.CleanupFolderName(dlinkFile2, false));
                if (!File.Exists(dLinkFileWithPath))
                {
                    MessageBox.Show("This ." + Program.options.databank_dlink_name + " file does not exist: '" + dLinkFileWithPath + "'");
                    new Error();
                }
                DlinkFile dlinkFileData = G.YamlReader<DlinkFile>(dLinkFileWithPath);
                string dataFile = Dlink_FromDlinkFileToDataFile(dLinkFileWithPath);
                //datafiles.Add(dataFile, false); //for cleanup purposes
                if (G.NullOrBlanks(dataFile))
                {
                    MessageBox.Show("Datafile string is null"); new Error();
                }

                FileInfo fi1 = new FileInfo(dataFile); //File may not exist                
                bool exists = fi1.Exists;
                RealFile realFile = new RealFile(
                    fi1.FullName,
                    null,
                    exists ? fi1.Length : 0,
                    exists ? fi1.LastWriteTimeUtc : DateTime.MinValue,
                    exists
                );                

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
                
                //After this method call, realFile may change regarding .hash and .exists fields (and only those)
                bool isDataFileOk = IsDLlinkHelperFileOk(realFile.name, dlinkFileData, ref realFile); //regarding last two args: either both non-null or both null
                if (isDataFileOk)
                {
                    //Check that we have the file in blobs folder, else add it there
                    SyncBlobs(false, realFile.name, dlinkFileData.hash, G.CleanupFolderName(Program.options.databank_dlink_folder_blobs, false), getFilesNew, getFilesOverwrite, putFiles);                    
                }
                else
                {
                    //Get it from blobs (A or B)
                    SyncBlobs(true, realFile.name, dlinkFileData.hash, G.CleanupFolderName(Program.options.databank_dlink_folder_blobs, false), getFilesNew, getFilesOverwrite, putFiles);
                    FileInfo fi2 = new FileInfo(realFile.name);
                    //We update the realFile, because its contents have changed
                    realFile = new RealFile(realFile.name, dlinkFileData.hash, fi2.Length, fi2.LastWriteTimeUtc, true);                    
                }
            }            
            DlinkHashCache.Save(); //Persist any hashes computed while checking this batch of .dlink files
            DLinkCalledFromGitHookReporting(type, getFilesNew, getFilesOverwrite, putFiles);
        }

        private static string Dlink_FromDlinkFileToDataFile(string dlinkFile)
        {
            //m1                 K:\MAKROBK\tth\test\makrobk_grunddata\biver\_progs\_uddata_dlink\x.csv.dlink
            //m2                 tth\test\makrobk_grunddata\biver\_progs\_uddata_dlink\x.csv.dlink
            //m3                 tth\test\biver\_uddata_dlink\x.csv.dlink
            //m4                 tth\test\biver\_uddata\x.csv.dlink
            //m5                 tth\test\biver\_uddata\x.csv
            //m6                 k:\\MAKROBK_KILDE\\2025_10_01\tth\test\biver\_uddata\x.csv

            if (Globals.tthDlink1) dlinkFile = G.Replace(dlinkFile, "c:\\tools\\k", "K:", StringComparison.OrdinalIgnoreCase, 1);

            if (!Path.IsPathRooted(Program.options.databank_dlink_folder_progs)) new Error("Expected path '" + Program.options.databank_dlink_folder_progs + "' to be absolute");
            List<string> dataStart1 = Stringlist.Path_FromStringToList(Program.options.databank_dlink_folder_progs);
            if (!Path.IsPathRooted(Program.options.databank_dlink_folder_data)) new Error("Expected path '" + Program.options.databank_dlink_folder_data + "' to be absolute");
            List<string> dataStart2 = Stringlist.Path_FromStringToList(Program.options.databank_dlink_folder_data);            
            if (!Path.IsPathRooted(dlinkFile)) new Error("Expected path '" + dlinkFile + "' to be absolute");
            List<string> m1 = Stringlist.Path_FromStringToList(dlinkFile);
            List<string> m2 = Stringlist.Path_RemoveStart(m1, dataStart1);
            List<string> m3 = m2.ToList(); //copy
            if (!G.NullOrBlanks(Program.options.databank_dlink_folder_remove1)) m3 = Stringlist.Path_RemoveString(m3, Program.options.databank_dlink_folder_remove1, 1);
            if (!G.NullOrBlanks(Program.options.databank_dlink_folder_remove2)) m3 = Stringlist.Path_RemoveString(m3, Program.options.databank_dlink_folder_remove2, 1);
            List<string> m4 = Stringlist.Path_ReplaceString(m3, Program.options.databank_dlink_folder_replace1b, Program.options.databank_dlink_folder_replace1a, 1);
            m4 = Stringlist.Path_ReplaceString(m4, Program.options.databank_dlink_folder_replace2b, Program.options.databank_dlink_folder_replace2a, 1);            
            List<string> m5 = m4.ToList();
            m5[m5.Count - 1] = m5[m5.Count - 1].Replace("." + Program.options.databank_dlink_name, "");
            List<string> m6 = m5.ToList();
            m6.InsertRange(0, dataStart2);
            return Stringlist.Path_FromListToString(m6, "\\");
        }

        /// <summary>
        /// New: true for file types whose .dlink hash is a "data hash" -- computed from the actual
        /// values inside the file rather than from its raw bytes (see the isGbk branch in
        /// GetFileHash below). For these types, two files with different byte sizes (different
        /// embedded timestamps, different zip/compression settings, etc.) can legitimately produce
        /// the same hash, so byte size must NOT be used as a proxy for "this is the same/different
        /// data" -- see Blob() and IsDLlinkHelperFileOk(), which both consult this.
        /// </summary>
        public static bool IsDataHashFileType(string filePath)
        {
            return G.Equal(Path.GetExtension(filePath), ".gbk");
        }

        /// <summary>
        /// Returns true if file is ok, else it must be fetched from blobs
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="syncTimeUtc"></param>
        /// <param name="dlinkFileData"></param>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static bool IsDLlinkHelperFileOk(string dataFile, DlinkFile dlinkFileData, ref RealFile realFile)
        {
            //When this method is called, dataFile does not have a hash code because it is costly to compute
            //We try to take the hash code from cache
            if (!realFile.exists)
            {                
                return false; //In that case, realFile.stamp etc. are null too
            }
            // New: dlinkFileData.bytes is null for "data hash" file types (see IsDataHashFileType /
            // Blob()), where byte size is not a reliable proxy for "the data is the same". Only gate
            // on it when we actually have a comparable value recorded.
            if (dlinkFileData.bytes != null && realFile.bytes != dlinkFileData.bytes)
            {                
                return false;
            }            
            //HARD way
            //We now need to calc the sha256 physically.                
            string realHash = GetFileHash(dataFile);
            realFile = new RealFile(realFile.name, realHash, realFile.bytes, realFile.stamp, true);
            if (dlinkFileData.hash != realHash)
            {
                //MessageBox.Show("FALSE --> hash, dlink=" + dlinkFileData.hash + " just gotten realhash=" + realHash);
                return false;
            }
            return true;
        }

        private static void DLinkCalledFromGitHookReporting(string type, List<string> filesNew, List<string> filesOverwritten, List<string> putFiles)
        {
            string s = null;
            s += " ---------------------- DATA FOLDER SYNC --------------------------- ";
            s += G.NL + G.NL;
            if (filesNew.Count + filesOverwritten.Count > 0)
            {                         
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
                s += "No data files added or overwritten.";
            }

            s += G.NL + G.NL;
            s += " ---------------------- VERSIONS STORAGE --------------------------- ";
            s += G.NL + G.NL;

            if (putFiles.Count == 0)
            {
                s += "Nothing changed regarding long-term storage.";
            }
            else
            {
                
                s += putFiles.Count + " data file version" + G.S(putFiles.Count) + " added to long-term storage:";
                foreach (string f in putFiles)
                {
                    s += G.NL + f;
                }
            }

            s += G.NL + G.NL;
            s += " ------------------------- HASH CACHE ------------------------------ ";
            s += G.NL + G.NL;
            s += "Queries = " + DlinkHashCache.countAsk + ", hits = " + DlinkHashCache.countHit + ", size = " + DlinkHashCache.Count();

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

        public static string GetFileHash(string filePath)
        {

            if (G.DlinkDebug()) MessageBox.Show("Getting hash from " + filePath);

            // ---- LRU cache lookup ---------------------------------------------------------
            FileInfo fiForCache = new FileInfo(filePath);
            string cachedHash = DlinkHashCache.TryGet(filePath, fiForCache.Length, fiForCache.LastWriteTimeUtc);
            if (cachedHash != null)
            {
                if (G.DlinkDebug()) MessageBox.Show("Getting hash from LRU cache");
                return cachedHash;
            }            

            string hash = null;            
            bool isGbk = IsDataHashFileType(filePath); // New: was "G.Equal(Path.GetExtension(filePath), ".gbk")" inline; now shared with Blob() / IsDLlinkHelperFileOk()

            if (isGbk)
            {
                using (ZipArchive archive = ZipFile.OpenRead(filePath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (G.Equal(entry.Name, Globals.databankInfoName))
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
                            catch
                            {
                                MessageBox.Show("Could not extract data hash from inside .gbk file (" + Globals.databankInfoName + ").\nFile: " + filePath);
                                throw;
                            }
                        }
                    }
                }
            }

            if (hash == null)
            {
                //if .gbk, this means that data hash is not implemented for that file
                if (G.DlinkDebug()) MessageBox.Show("Getting hash from physical file");
                hash = G.GetSha256FromFile(filePath);
            }
            else
            {
                if (G.DlinkDebug()) MessageBox.Show("Getting hash from xml");
            }

            // ---- Remember this result for next time -----------------------------------------
            DlinkHashCache.Set(filePath, fiForCache.Length, fiForCache.LastWriteTimeUtc, hash);
            // ----------------------------------------------------------------------------------------

            return hash;
        }

        public static void SyncBlobs(bool isGet, string fileName, string sha256, string blobsFolder, List<string> getFilesNew, List<string> getFilesOverwrite, List<string> putFiles)
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

    public class DlinkFile
    {
        public readonly string version = "1.0";
        public string hash { get; private set; }
        public long? bytes { get; private set; }
        public DateTime? stamp { get; private set; }
        public long? variables { get; private set; }
        public string extra { get; private set; }

        public DlinkFile()
        {
        }

        public DlinkFile(string hash, long? bytes, DateTime? stamp, long? nVariables, string extra)
        {
            this.hash = hash;
            this.bytes = bytes;
            this.stamp = stamp;
            this.variables = nVariables;
            this.extra = extra;
        }
    }

    public class RealFile
    {
        public readonly string name = null;
        public readonly string hash = null;
        public readonly long? bytes = null;
        public readonly DateTime? stamp = null;
        public readonly bool exists = false;

        public RealFile(string name, string hash, long? bytes, DateTime? stamp, bool exists)
        {
            this.name = name;
            this.hash = hash;
            this.bytes = bytes;
            this.stamp = stamp;
            this.exists = exists;
        }
    }

    // ================================================================================================
    // On-disk LRU hash cache.
    //
    // GetFileHash() (below, in DlinkHooks) is called once per .dlink file on every single git hook
    // invocation (post-checkout, post-merge, pre-commit, pre-push all funnel through
    // DLinkCalledFromGitHook -> IsDLlinkHelperFileOk -> GetFileHash), even for files nothing touched.
    // This cache lets GetFileHash skip recomputing a SHA-256 (or, for .gbk files, re-extracting the
    // embedded metadata) when a file's size and last-write-time still match what was recorded the
    // last time it was hashed.
    //
    // It is a plain static cache: one instance per Gekko.exe run, loaded from disk on first use.
    // Because each hook invocation is its own process (see the "_common" hook script in DlinkSetup,
    // which shells out to Gekko.exe), the only way a cache can survive between hook runs is on disk --
    // that's the whole point of persisting it here rather than just keeping it in memory.
    //
    // Callers should call Set() per file as usual, then call Save() ONCE after a batch of files, not
    // once per file -- otherwise every hashed file costs a disk write and most of the benefit is lost.
    //
    // Deliberately NOT given the same atomic-write treatment as the blob store (see the TODO in
    // SyncBlobs below): this cache only ever holds derived data that can always be recomputed from
    // the file itself, so worst case on a corrupt or half-written cache file is a cold cache next run
    // (caught below and treated as empty), never a wrong or lost answer.
    // ================================================================================================
    public static class DlinkHashCache
    {
        public static int countAsk = 0;
        public static int countHit = 0;

        private const int Capacity = 1000;
        private static readonly long ToleranceTicks = TimeSpan.FromSeconds(0).Ticks; //Changed from TimeSpan.FromSeconds(2) to TimeSpan.FromSeconds(0).
        private static readonly DateTime TicksEpoch = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static readonly object _lock = new object(); //cheap insurance if the foreach loops above are ever parallelized
        private static Dictionary<string, LinkedListNode<HashCacheEntry>> _map;
        private static LinkedList<HashCacheEntry> _lru; //front = most recently used, back = least recently used
        private static bool _loaded = false;
        private static bool _dirty = false;

        private static string CacheFilePath
        {
            get
            {
                string folder = Path.Combine(G.CleanupFolderName(Program.options.databank_dlink_folder_blobs, false), "_utilities", "hashcache");
                //Per-machine file name: this folder is shared/network storage (databank_dlink_folder_blobs),
                //and giving each machine its own cache file avoids two machines racing on the same file.
                //If you'd rather have one shared cache, replace this with a fixed file name -- just be
                //aware Save() below is a plain overwrite, not an atomic one (see class comment above).
                return Path.Combine(folder, "hashcache_" + Environment.MachineName + ".yaml");
            }
        }

        private static long ToTicksSinceEpoch(DateTime utc)
        {
            return (utc - TicksEpoch).Ticks;
        }

        private static void EnsureLoaded()
        {
            //Caller must hold _lock
            if (_loaded) return;
            _map = new Dictionary<string, LinkedListNode<HashCacheEntry>>(StringComparer.OrdinalIgnoreCase);
            _lru = new LinkedList<HashCacheEntry>();
            try
            {
                if (File.Exists(CacheFilePath))
                {
                    HashCacheFile cf = G.YamlReader<HashCacheFile>(CacheFilePath);
                    if (cf != null && cf.entries != null)
                    {
                        //File is written oldest-first (see Save() below); AddFirst()'ing in that order
                        //rebuilds the same most-recently-used-at-front order we had before saving.
                        foreach (HashCacheEntry e in cf.entries)
                        {
                            LinkedListNode<HashCacheEntry> node = _lru.AddFirst(e);
                            _map[e.path] = node;
                        }
                    }
                }
            }
            catch
            {
                //A corrupt or unreadable cache file should not break the hook -- just start empty.
                _map = new Dictionary<string, LinkedListNode<HashCacheEntry>>(StringComparer.OrdinalIgnoreCase);
                _lru = new LinkedList<HashCacheEntry>();
            }
            _loaded = true;
        }

        /// <summary>
        /// Returns the cached hash for filePath if it is still fresh (same size, and last-write-time
        /// within +/- 2 seconds of what was recorded last time). Returns null on a miss.
        /// </summary>
        public static string TryGet(string filePath, long size, DateTime lastWriteUtc)
        {
            lock (_lock)
            {
                countAsk++;
                EnsureLoaded();
                LinkedListNode<HashCacheEntry> node;
                if (!_map.TryGetValue(filePath, out node)) return null;

                HashCacheEntry e = node.Value;
                if (e.bytes != size) return null;
                long ticksNow = ToTicksSinceEpoch(lastWriteUtc);
                if (Math.Abs(ticksNow - e.stamp) > ToleranceTicks) return null;

                //Hit: touch it so it counts as recently used
                countHit++;
                _lru.Remove(node);
                _lru.AddFirst(node);
                return e.hash;
            }
        }

        /// <summary>
        /// Records/refreshes the hash for filePath. Evicts the least-recently-used entry once the
        /// cache is over capacity (1000 entries). Does not touch disk -- call Save() once after a
        /// batch of files.
        /// </summary>
        public static void Set(string filePath, long bytes, DateTime lastWriteUtc, string hash)
        {
            lock (_lock)
            {
                EnsureLoaded();
                long stamp = ToTicksSinceEpoch(lastWriteUtc);

                LinkedListNode<HashCacheEntry> existing;
                if (_map.TryGetValue(filePath, out existing))
                {
                    existing.Value.bytes = bytes;
                    existing.Value.stamp = stamp;
                    existing.Value.hash = hash;
                    _lru.Remove(existing);
                    _lru.AddFirst(existing);
                }
                else
                {
                    HashCacheEntry entry = new HashCacheEntry { path = filePath, bytes = bytes, stamp = stamp, hash = hash };
                    LinkedListNode<HashCacheEntry> node = _lru.AddFirst(entry);
                    _map[filePath] = node;
                    if (_map.Count > Capacity)
                    {
                        LinkedListNode<HashCacheEntry> lruNode = _lru.Last;
                        _lru.RemoveLast();
                        _map.Remove(lruNode.Value.path);
                    }
                }
                _dirty = true;
            }
        }

        /// <summary>
        /// Persists the cache to disk if anything changed since the last Save(). Cheap no-op
        /// otherwise. Call this once after processing a batch of files, not once per file.
        /// </summary>
        public static void Save()
        {
            lock (_lock)
            {
                if (!_dirty || _lru == null) return;
                try
                {
                    HashCacheFile cf = new HashCacheFile();
                    cf.entries = new List<HashCacheEntry>();
                    //Walk least-recently-used -> most-recently-used (oldest first), so that re-loading
                    //via the AddFirst() loop in EnsureLoaded() reproduces this exact order.
                    for (LinkedListNode<HashCacheEntry> node = _lru.Last; node != null; node = node.Previous)
                    {
                        cf.entries.Add(node.Value);
                    }
                    Directory.CreateDirectory(Path.GetDirectoryName(CacheFilePath));
                    G.YamlWriter<HashCacheFile>(cf, CacheFilePath);
                    _dirty = false;
                }
                catch
                {
                    //Best-effort: a failed cache save should not break the hook. Worst case, next
                    //run recomputes a few more hashes than strictly necessary.
                }
            }
        }

        /// <summary>
        /// Returns the current number of cached items.
        /// </summary>
        public static int Count()
        {
            lock (_lock)
            {
                EnsureLoaded();
                return _map.Count;
            }
        }
    }

    // Plain data classes backing DlinkHashCache's on-disk file. Kept as simple public fields
    // (rather than DlinkFile's private-setter-property style) since DlinkHashCache mutates entries
    // in place on every cache hit/refresh.
    public class HashCacheEntry
    {
        public string path;
        public long bytes;
        public long stamp; //ticks (100ns units) since DlinkHashCache's fixed epoch; compared with a ~2 second tolerance
        public string hash;
    }

    public class HashCacheFile
    {
        public List<HashCacheEntry> entries = new List<HashCacheEntry>();
    }
}
