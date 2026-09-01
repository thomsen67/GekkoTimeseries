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
using System.Diagnostics;
using System.Threading;

namespace Gekko
{    
    public static class DlinkSetup
    {
        public enum EDlinkSetup
        {
            Activate,
            ActivateOnlyHooks,
            ActivateOnlySync,
            DeactivateHooks,
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
                // Note: -c core.quotepath=false --> without it, Git mangles זרו etc. With it, we get UTF8. Se #oowar7asdfj
                // Pre-commit only needs to sync the .dlink files that are actually part of
                // this commit -- "git diff --cached --name-only --diff-filter=ACMR" lists just the
                // Added/Copied/Modified/Renamed paths staged in the index (D=deleted is excluded on
                // purpose: nothing to sync for a file being removed). The other hooks
                // (post-checkout/post-merge/pre-push) are about "does the whole working copy match
                // reality", not "what's in this one commit", so they keep the full
                // "git ls-files --cached" scan below.
                //NOTE: It seems that if one uses raw Git commands and(1) creates a file with "117" inside, (2) adds it
                //      changes the file to "118" inside, (3) commits it --> it is "117" that ends up in the repo.
                //      Regarding TortoiseGit it seems like it re-stages the working-tree contents before committing, because
                //      using TortoiseGit GUI instead makes "118" end up in the repo.
                //      If users ever start using raw Git commands AND do an add and then changes the file before committing,
                //      the logic may break.

                string _common = @$"#!/bin/sh
ROOT_DIR=$(git rev-parse --show-toplevel 2>/dev/null)
rm -f ""${{ROOT_DIR}}""/.git/dlink_filelist_*.txt
if [ ""$1"" = ""pre-commit"" ]; then
  STAGED_FILES=$(git -C ""${{ROOT_DIR}}"" -c core.quotepath=false diff --cached --name-only --diff-filter=ACMR -- ':(icase)*.dlink')
else
  STAGED_FILES=$(git -C ""${{ROOT_DIR}}"" -c core.quotepath=false ls-files --cached -- ':(icase)*.dlink')
fi
DLINK_TMP_NAME=""dlink_filelist_$$.txt""
DLINK_TMP_PATH=""${{ROOT_DIR}}/.git/${{DLINK_TMP_NAME}}""
echo ""$1"" > ""$DLINK_TMP_PATH""
echo ""$STAGED_FILES"" >> ""$DLINK_TMP_PATH""
echo ""==> Syncing data files (please wait for popup window to show)""
cd /c/Windows
cmd.exe //c ""{gekkoExePath}"" ""-dlink:'$DLINK_TMP_NAME'"" ""-dlinkw:'$ROOT_DIR'""
GEKKO_EXIT_CODE=$?
rm -f ""$DLINK_TMP_PATH""
echo ""==> Syncing data files finished""
exit $GEKKO_EXIT_CODE
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

                if (type == EDlinkSetup.Activate || type == EDlinkSetup.ActivateOnlyHooks || type == EDlinkSetup.ActivateOnlySync)
                {
                    if (type == EDlinkSetup.Activate || type == EDlinkSetup.ActivateOnlyHooks)
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

                    if (type == EDlinkSetup.Activate || type == EDlinkSetup.ActivateOnlySync)
                    {
                        // Here we sync every tracked .dlink file, as if a Git hook had fired.
                        List<string> trackedDlinkFiles = DlinkHooks.ListTrackedDlinkFiles(parentOfGitFolder);
                        DlinkHooks.DlinkSyncFiles(parentOfGitFolder, "activate", trackedDlinkFiles);
                    }
                }
                else if (type == EDlinkSetup.DeactivateHooks)
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
                else new Error();
            }
            catch
            {
                if (type == EDlinkSetup.Activate) new Error("Failed to write Git hooks files in folder '" + hooksPath + "', and sync afterwards.");
                else if (type == EDlinkSetup.DeactivateHooks) new Error("Failed to remove Git hooks files in folder '" + hooksPath + "'");
                else if (type == EDlinkSetup.ActivateOnlyHooks) new Error("Failed to write Git hooks files in folder '" + hooksPath + "'");
                else if (type == EDlinkSetup.ActivateOnlySync) new Error("Failed to sync .dlink files (Git folder: '" + hooksPath + "')");
                new Error();
            }
        }
    }

    public static class DlinkAutoDlinkFiles
    {

        /// <summary>
        /// Handles blobs, for .dlink. All non-gbk files go through this.
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="force"></param>
        public static void Blob(string dataFile, bool force)
        {
            Blob(dataFile, null, null, force);
        }

        /// <summary>
        /// Handles blobs, for .dlink. Only gbk files go through this.
        /// </summary>
        /// <param name="dataFile"></param>
        public static void Blob(string dataFile, long? nVariables, long? nSeries, bool force)
        {
            string hash = null;
            long? bytes = null;
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

                    bytes = (new FileInfo(dataFile)).Length;
                    if (!Directory.Exists(Path.GetDirectoryName(dlinkFile)))
                    {                        
                        Directory.CreateDirectory(Path.GetDirectoryName(dlinkFile));
                    }

                    if (!G.Equal(Path.GetExtension(dataFile), ".gbk"))
                    {
                        nVariables = null; nSeries = null; //Even if present, we do not store these in .dlink file. We would like other software like Python be able to produce .dlink files that are compatible, without parsing/understanding the contents of the data file (for instance .csv file)
                    }
                    if (G.Equal(Path.GetExtension(dataFile), ".gbk") || G.Equal(Path.GetExtension(dataFile), ".px"))
                    {
                        bytes = null; //Bytes do not necessarily follow (data)hash for these types
                    }

                    DlinkFile blobInfo = new DlinkFile(hash, bytes, nVariables, nSeries, null);                    
                    G.YamlWriter<DlinkFile>(blobInfo, dlinkFile);
                }
            }
        }

        public static string Dlink_FromDataFileToDlinkFile(string dataFile)
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
            m4 = Stringlist.Path_ReplaceString(m4, Program.options.databank_dlink_folder_replace3a, Program.options.databank_dlink_folder_replace3b, 1);
            List<string> m5 = m4.ToList();
            m5[m5.Count - 1] += "." + Program.options.databank_dlink_name;
            List<string> m6 = m5.ToList();
            m6.InsertRange(0, dataStart2);
            return Stringlist.Path_FromListToString(m6, "\\");            
        }
    }    

    /// <summary>
    /// For instance if Python creates a csv file. Or already existing .gbk files are to be put into Git as .dlink files.    
    /// TODO: for Python etc. should it be possible to state number of variables?
    /// 
    /// </summary>
    public static class DlinkHooks
    {
        public static void CreateDlinkFilesManually(string[] args, bool function)
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
                    DlinkAutoDlinkFiles.Blob(dlinkFile2, true); //We do not know the number of variables, so it is set to null
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
                if (!G.NullOrBlanks(Program.options.databank_dlink_folder_replace4a))
                {
                    gitFolder = G.Replace(gitFolder, Program.options.databank_dlink_folder_replace4a, Program.options.databank_dlink_folder_replace4b, StringComparison.OrdinalIgnoreCase, 1);
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
            
            //Reads file names from the temporary file created by the _common bash script/hook            
            string dlinkFileArg = args.FirstOrDefault(a => a.StartsWith("-dlink:"));
            if (G.NullOrBlanks(dlinkFileArg))
            {
                MessageBox.Show("*** Error: Could not find the '-dlink:' argument");
                new Error();
            }
            string dlinkListFileName = G.StripQuotes(dlinkFileArg.Substring("-dlink:".Length));
            string dlinkListFilePath = Path.Combine(gitFolder, ".git", dlinkListFileName);
            if (!File.Exists(dlinkListFilePath))
            {
                MessageBox.Show("*** Error: Could not find the .dlink file list '" + dlinkListFilePath + "'");
                new Error();
            }
            string[] lines = File.ReadAllLines(dlinkListFilePath);
            string type = lines.Length >= 1 ? lines[0] : null;
            List<string> dlinkFiles = new List<string>();
            for (int i = 1; i < lines.Length; i++)
            {
                if (G.NullOrBlanks(lines[i])) continue; //Happens when there are no staged .dlink files at all
                dlinkFiles.Add(lines[i]);
            }
            DlinkSyncFiles(gitFolder, type, dlinkFiles);
        }

        /// <summary>
        /// runs "git ls-files --cached -- :(icase)*.dlink", same as 
        /// the "_common" hook script does. It is assumed that Git in on the PATH.
        /// </summary>
        public static List<string> ListTrackedDlinkFiles(string parentOfGitFolder)
        {
            List<string> result = new List<string>();
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "git",                    
                    // Note: -c core.quotepath=false --> without it, Git mangles זרו etc. With it, we get UTF8. Se #oowar7asdfj
                    Arguments = "-c core.quotepath=false ls-files --cached -- :(icase)*.dlink",
                    WorkingDirectory = parentOfGitFolder,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    //UTF8 because Git emits that
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    StandardErrorEncoding = System.Text.Encoding.UTF8,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (Process p = Process.Start(psi))
                {
                    string stdout = p.StandardOutput.ReadToEnd();
                    string stderr = p.StandardError.ReadToEnd();
                    p.WaitForExit();
                    if (p.ExitCode != 0)
                    {
                        MessageBox.Show("*** Error: 'git ls-files' failed in '" + parentOfGitFolder + "':" + G.NL + stderr);
                        new Error();
                    }
                    foreach (string line in stdout.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        result.Add(line);
                    }
                }
            }
            catch
            {
                MessageBox.Show("*** Error: could not run 'git' from '" + parentOfGitFolder + "' -- is Git installed and on PATH?");
                new Error();
            }
            return result;
        }

        /// <summary>
        /// Make data files correspond to .dlink files. type is just a label used in the reporting dialog (e.g. "post-checkout",
        /// "pre-commit", or "activate").
        /// </summary>  
        public static void DlinkSyncFiles(string gitFolder, string type, List<string> dlinkFiles)
        {
            int gap = 5; //5%
            if (G.Equal(type, "activate")) new Writeln("Synchronizing .dlink and data files");

            //Sanity check -- fast, so it stays on the calling thread, before any window is shown
            string blobsFolder = G.CleanupFolderName(Program.options.databank_dlink_folder_blobs, false);
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

            List<string> getFilesNew = new List<string>();
            List<string> getFilesOverwrite = new List<string>();
            List<string> putFiles = new List<string>();
            List<string> errors = new List<string>();
            
            WindowDlinkGitHook progressWindow = new WindowDlinkGitHook("Data file sync (" + type + ")");

            Thread worker = new Thread(delegate ()
            {
                int currentFileIndex = 0; int lastReportedPercent = 0; //for the console-style progress line

                System.Diagnostics.Stopwatch progressStopwatch = System.Diagnostics.Stopwatch.StartNew();
                const int progressReportIntervalMs = 50; //don't push a window update more often than this

                foreach (string dlinkFile2 in dlinkFiles) //Could probably be parallelized, but maybe it is IO bound anyway
                {
                    G.PrintProgress(dlinkFiles.Count, ref currentFileIndex, ref lastReportedPercent, G.Equal(type, "activate"), "data file" + G.S(dlinkFiles.Count) + " synchronized", gap);
                    if ((currentFileIndex == dlinkFiles.Count) || progressStopwatch.ElapsedMilliseconds >= progressReportIntervalMs)
                    {
                        progressWindow.ReportProgress(currentFileIndex, dlinkFiles.Count, Path.GetFileNameWithoutExtension(dlinkFile2));
                        progressStopwatch.Restart();
                    }
                    try
                    {
                        string dLinkFileWithPath = Path.Combine(G.CleanupFolderName(gitFolder, false), G.CleanupFolderName(dlinkFile2, false));
                        if (!File.Exists(dLinkFileWithPath))
                        {
                            MessageBox.Show("This ." + Program.options.databank_dlink_name + " file does not exist: '" + dLinkFileWithPath + "'");
                            new Error();
                        }
                        DlinkFile dlinkFileData = G.YamlReader<DlinkFile>(dLinkFileWithPath);
                        string dataFile = Dlink_FromDlinkFileToDataFile(dLinkFileWithPath);
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
                        bool doDlinkFileAndDataFileCorrespond = DoDlinkFileAndDataFileCorrespond(realFile.name, dlinkFileData, ref realFile); //regarding last two args: either both non-null or both null
                        if (doDlinkFileAndDataFileCorrespond)
                        {
                            //Check that we have the file in blobs folder, else add it there. This happens when making a brand new datafile
                            SyncBlobs(false, realFile.name, dlinkFileData.hash, blobsFolder, getFilesNew, getFilesOverwrite, putFiles);
                        }
                        else
                        {
                            //Get it from blobs (A or B)
                            SyncBlobs(true, realFile.name, dlinkFileData.hash, blobsFolder, getFilesNew, getFilesOverwrite, putFiles);
                            FileInfo fi2 = new FileInfo(realFile.name);
                            //We update the realFile, because its contents have changed
                            realFile = new RealFile(realFile.name, dlinkFileData.hash, fi2.Length, fi2.LastWriteTimeUtc, true);
                            //Hash cache remembers this for later
                            DlinkHashCache.Set(realFile.name, fi2.Length, fi2.LastWriteTimeUtc, dlinkFileData.hash);
                            DlinkHashCache.SetBlobConfirmed(realFile.name, dlinkFileData.hash);
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(dlinkFile2 + ": " + ex.Message);
                    }
                }
                DlinkHashCache.Save(); //Persist any hashes computed while checking this batch of .dlink files

                string report = BuildSyncReportText(type, getFilesNew, getFilesOverwrite, putFiles);
                progressWindow.Finish(report); //fills the report in, enables OK, and lets ShowDialog() below return once the user dismisses it
            });
            worker.IsBackground = true;
            //MessageBox.Show(...) above, and Clipboard access inside WindowDlinkGitHook's "Copy
            //text" button, are WinForms/COM and expect an STA thread, same as the app's main UI thread.
            worker.SetApartmentState(ApartmentState.STA);
            worker.Start();

            //Blocks the calling thread here (same as the old ShowDialog() call did), but this
            //window's own message pump keeps it responsive/repainting while "worker" does the
            //actual sync work above. Returns once the user clicks OK (see WindowDlinkGitHook,
            //which refuses to close early via its Closing handler).
            progressWindow.ShowDialog();

            if (errors.Count > 0)
            {
                new Error("Dlink sync failed for " + errors.Count + " file" + G.S(errors.Count) + ":" + G.NL + string.Join(G.NL, errors));
            }
        }   
                
        public static string Dlink_FromDlinkFileToDataFile(string dlinkFile)
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
            m4 = Stringlist.Path_ReplaceString(m4, Program.options.databank_dlink_folder_replace3b, Program.options.databank_dlink_folder_replace3a, 1);
            List<string> m5 = m4.ToList();
            m5[m5.Count - 1] = m5[m5.Count - 1].Replace("." + Program.options.databank_dlink_name, "");
            List<string> m6 = m5.ToList();
            m6.InsertRange(0, dataStart2);
            return Stringlist.Path_FromListToString(m6, "\\");
        }

        /// <summary>
        /// Returns true if .dlink and data files correspond: else data file must be fetched from blobs
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="syncTimeUtc"></param>
        /// <param name="dlinkFileData"></param>
        /// <param name="fi"></param>
        /// <returns></returns>
        public static bool DoDlinkFileAndDataFileCorrespond(string dataFile, DlinkFile dlinkFileData, ref RealFile realFile)
        {
            //When this method is called, dataFile does not have a hash code because it is costly to compute
            //We try to take the hash code from cache
            if (!realFile.exists)
            {                
                return false; //In that case, realFile.stamp etc. are null too
            }
            // For most .gbk files, byte size is not stored because data hash is used instead.
            if (dlinkFileData.bytes != null && realFile.bytes != dlinkFileData.bytes)
            {                
                return false;
            }                        
            //We now need to calc the sha256 physically (or for newer .gbk files: fetch data hash).
            //The hash may be gotten from cache file though.
            string realHash = GetFileHash(dataFile, realFile.bytes.Value, realFile.stamp.Value);
            realFile = new RealFile(realFile.name, realHash, realFile.bytes, realFile.stamp, true);
            if (dlinkFileData.hash != realHash)
            {                
                return false;
            }
            return true;
        }

        /// <summary>
        /// Builds the final summary text shown in the progress window once a sync finishes (see
        /// DlinkSyncFiles). Pure string building -- no window/UI code -- so it's safe to call from
        /// a background thread. (Previously this method was also responsible for creating and
        /// showing a WindowMessageBox itself, only once the whole sync was already done; that's
        /// now WindowDlinkGitHook's job, opened up-front by DlinkSyncFiles.)
        /// </summary>
        private static string BuildSyncReportText(string type, List<string> filesNew, List<string> filesOverwritten, List<string> putFiles)
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
                    s += filesNew.Count + " new file" + G.S(filesNew.Count) + " " + s2a + " added";
                }
                else if (filesNew.Count == 0 && filesOverwritten.Count > 0)
                {
                    s += filesOverwritten.Count + " file" + G.S(filesOverwritten.Count) + " " + s2b + " overwritten";
                }
                else
                {
                    s += filesNew.Count + " new file" + G.S(filesNew.Count) + " " + s2a + " added, " + filesOverwritten.Count + " file" + G.S(filesOverwritten.Count) + " " + s2b + " overwritten";
                }                
                s += " (" + ActivateText(type) + ")";
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

            if (G.Equal(Environment.UserName, "tth"))
            {
                s += G.NL + G.NL;
                s += " ------------------------- HASH CACHE ------------------------------ ";
                s += G.NL + G.NL;
                s += "TTH: Queries = " + DlinkHashCache.countAsk + ", hits = " + DlinkHashCache.countHit + ", size = " + DlinkHashCache.Count() + G.NL;                
            }

            return s;
        }

        private static string ActivateText(string type)
        {
            string typeTemp = type;
            if (G.Equal(type, "activate")) typeTemp = "dlink('activate')";
            return typeTemp;
        }

        /// <summary>
        /// Handles .gbk files to get datahash from metadata inside file, and handles .px
        /// files to omit the line with time stamp.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileHash(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return GetFileHash(filePath, fi.Length, fi.LastWriteTimeUtc);
        }
        
        /// <summary>
        /// Handles .gbk files to get datahash from metadata inside file, and handles .px
        /// files to omit the line with time stamp.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="knownSize"></param>
        /// <param name="knownLastWriteUtc"></param>
        /// <returns></returns>
        public static string GetFileHash(string filePath, long knownSize, DateTime knownLastWriteUtc)
        {
            if (G.DlinkDebug()) MessageBox.Show("Getting hash from " + filePath);

            // ---- LRU cache lookup ---------------------------------------------------------
            string cachedHash = DlinkHashCache.TryGet(filePath, knownSize, knownLastWriteUtc);
            if (cachedHash != null)
            {
                if (G.DlinkDebug()) MessageBox.Show("Getting hash from LRU cache");
                return cachedHash;
            }            

            string hash = null;
            int variables = 0; //default
            int series = 0; //default

            if (G.Equal(Path.GetExtension(filePath), ".gbk"))
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
                                variables = readInfo.variables;
                                series = readInfo.series;
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
                hash =  G.FileHasher.GetSha256FromFile(filePath);
            }
            else
            {
                if (G.DlinkDebug()) MessageBox.Show("Getting hash from xml");
            }

            // ---- Remember this result for next time -----------------------------------------
            DlinkHashCache.Set(filePath, knownSize, knownLastWriteUtc, hash);
            // ----------------------------------------------------------------------------------------

            return hash;
        }

        public static void SyncBlobs(bool isGet, string fileNameAndPath, string sha256, string blobsFolder, List<string> getFilesNew, List<string> getFilesOverwrite, List<string> putFiles)
        {
            // This uses atomic writes.
            
            string shapart1 = sha256.Substring(0, 2);
            string shapart2 = sha256; //We do not want file "abcdefg" to become "\ab\cdefg", but prefer it to become "\ab\abcdefg". Easier to search for etc. even though Git does the former.
            if (isGet)
            {
                // ------------------------------------
                // Getting
                // ------------------------------------
                if (!File.Exists(Path.Combine(blobsFolder, shapart1, shapart2)))
                {
                    MessageBox.Show("For '" + fileNameAndPath + "', could not find blob file '" + Path.Combine(blobsFolder, shapart1, shapart2) + "'");
                    new Error();
                }
                else
                {
                    if (File.Exists(fileNameAndPath)) getFilesOverwrite.Add(fileNameAndPath);
                    else getFilesNew.Add(fileNameAndPath);
                    BlobsFileGet(fileNameAndPath, Path.Combine(blobsFolder, shapart1, shapart2));
                }
            }
            else
            {
                // ------------------------------------
                // Putting --> when a brand new file is there
                // ------------------------------------
                                
                if (DlinkHashCache.IsBlobConfirmed(fileNameAndPath, sha256))
                {
                    return;
                }

                string blobsFile = Path.Combine(blobsFolder, shapart1, shapart2);
                if (!Directory.Exists(Path.Combine(blobsFolder, shapart1)))
                {
                    Directory.CreateDirectory(Path.Combine(blobsFolder, shapart1));
                    BlobsFilePut(fileNameAndPath, blobsFile);
                    putFiles.Add(fileNameAndPath);
                }
                else
                {
                    if (File.Exists(Path.Combine(blobsFolder, shapart1, shapart2)))
                    {
                        //No need to copy it: same file is already there
                        //TODO TODO TODO                        
                        //TODO TODO TODO ---> if a gbk is newer but with same datahash, we could add the new one (may have better meta information --> but we have now added meta info to data hash, so...)
                        //TODO TODO TODO                        
                    }
                    else
                    {
                        BlobsFilePut(fileNameAndPath, blobsFile);
                        putFiles.Add(fileNameAndPath);
                    }
                }                
                DlinkHashCache.SetBlobConfirmed(fileNameAndPath, sha256);
            }
        }

        /// <summary>
        /// Writes to finalPath atomically and crash-safely, so a concurrent reader can never
        /// observe a partially-written file. writeAction is given a path to a private temp file in
        /// the SAME folder as finalPath (same-volume, so the final publish step is a true rename,
        /// not a copy+delete across volumes) and must write the complete content there. finalPath
        /// only ever appears under its real name once writeAction has fully finished -- a crash or
        /// exception mid-write leaves only an orphaned temp file behind, never a corrupt finalPath.        
        /// </summary>
        private static void AtomicWrite(string finalPath, bool ifAbsentOnly, Action<string> writeAction)
        {
            string folder = Path.GetDirectoryName(finalPath);
            Directory.CreateDirectory(folder);
            //Unique temp name, same folder as finalPath, dot-prefixed so it doesn't look like a
            //real data/blob file if something lists the folder mid-write.
            string tempPath = Path.Combine(folder, "." + Path.GetFileName(finalPath) + "." + Guid.NewGuid().ToString("N") + ".tmp");
            try
            {
                writeAction(tempPath);
                if (!File.Exists(tempPath))
                {
                    //writeAction chose not to produce anything -- nothing to publish.
                    return;
                }

                //tempPath may have inherited the ReadOnly attribute from whatever writeAction copied it
                //from (e.g. a blob file, which BlobsFilePut always marks read-only). It's our own scratch
                //file, so strip it -- otherwise the move/delete below, or the cleanup in "finally", can
                //fail with UnauthorizedAccessException.
                FileAttributes tempAttr = File.GetAttributes(tempPath);
                if ((tempAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(tempPath, tempAttr & ~FileAttributes.ReadOnly);
                }

                if (ifAbsentOnly)
                {
                    if (File.Exists(finalPath))
                    {
                        return; //someone else already produced this exact (hash-addressed) content
                    }
                    try
                    {
                        File.Move(tempPath, finalPath);
                    }
                    catch (IOException)
                    {
                        //Lost a race between the check above and the move -- finalPath now exists
                        //with (by construction, same hash) the same content, so this is not an error.
                        if (!File.Exists(finalPath)) new Writeln("Dlink data file storage issue: " + finalPath);
                    }
                }
                else
                {
                    // File.Replace's underlying Win32 ReplaceFile call is not reliably supported on
                    // network/mapped drives. File.Move (MoveFileEx under
                    // the hood) is far more broadly supported. This trades
                    // strict atomicity for reliability: there is a brief window, between the delete and the
                    // move, where finalPath does not exist at all. A concurrent reader hitting that exact
                    // instant sees "file not found" rather than old or new content -- never a torn/partial
                    // read, and still a big improvement over reading a file mid-write.
                    if (File.Exists(finalPath))
                    {
                        FileAttributes finalAttr = File.GetAttributes(finalPath);
                        if ((finalAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            File.SetAttributes(finalPath, finalAttr & ~FileAttributes.ReadOnly);
                        }
                        File.Delete(finalPath);
                    }
                    File.Move(tempPath, finalPath);
                }
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    try { File.Delete(tempPath); }
                    catch
                    {
                        //We live with the temp file
                    }
                }
            }
        }

        private static void BlobsFileGet(string fileNameAndPath, string blobsFile)
        {
            //TODO
            //TODO
            //TODO Maybe check that the sha hash is correct after fetching the file.
            //TODO
            //TODO

            //We always create the folder in case it does not already exist. For cloning this is obviously important.
            Directory.CreateDirectory(Path.GetDirectoryName(fileNameAndPath));
            
            AtomicWrite(fileNameAndPath, false, tempPath =>
            {
                if (Globals.alreadyZipped.Contains(Path.GetExtension(fileNameAndPath), StringComparer.OrdinalIgnoreCase))
                {
                    File.Copy(blobsFile, tempPath, true);
                }
                else
                {
                    using (ZipArchive archive = ZipFile.OpenRead(blobsFile))
                    {
                        ZipArchiveEntry entry = archive.GetEntry("storage");
                        if (entry != null)
                        {
                            entry.ExtractToFile(tempPath, true);
                        }
                    }
                }
            });
            G.ReadOnlyRemove(fileNameAndPath);
        }

        private static void BlobsFilePut(string fileName, string blobsFile)
        {            
            AtomicWrite(blobsFile, true, tempPath =>
            {
                if (Globals.alreadyZipped.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase))
                {
                    File.Copy(fileName, tempPath);
                }
                else
                {
                    using (FileStream zipToOpen = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
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
            });
            G.ReadOnlySet(blobsFile);
        }
    }    

    public class DlinkFile
    {
        public readonly string version = "1.0";
        public string hash { get; private set; }
        public long? bytes { get; private set; }        
        public long? variables { get; private set; }        
        public long? series { get; private set; } //normal series + array-subseries
        public string extra { get; private set; }

        public DlinkFile()
        {
        }

        public DlinkFile(string hash, long? bytes, long? nVariables, long? nSeries, string extra)
        {
            this.hash = hash;
            this.bytes = bytes;
            this.variables = nVariables;
            this.series = nSeries;
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
    public static class DlinkHashCache
    {
        public static int countAsk = 0;
        public static int countHit = 0;
        
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
            var sw = System.Diagnostics.Stopwatch.StartNew();
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

            if (G.Equal(Environment.UserName, "tth"))
            {
                MessageBox.Show("TTH: Hash cache load took " + (double)sw.ElapsedMilliseconds / 1000d + " s for " + _map.Count + " entries");
            }
        }

        /// <summary>
        /// Returns the cached hash for filePath if it is still fresh (same size, and last-write-time
        /// of what was recorded last time). Returns null on a miss.
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
        /// cache is over capacity (10000 entries). Does not touch disk -- call Save() once after a
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
                    if (existing.Value.hash != hash)
                    {
                        existing.Value.blobConfirmed = false;
                    }
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
                    if (_map.Count > Program.options.databank_dlink_cache)
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
        /// This trusts that a blob, once written, is never deleted out from under it. Blob storage is expected to
        /// be read-only (write once). But if files are ever removed from blob storage, the system will crash with
        /// an error.
        /// </summary>
        public static bool IsBlobConfirmed(string filePath, string hash)
        {
            lock (_lock)
            {
                EnsureLoaded();
                LinkedListNode<HashCacheEntry> node;
                if (!_map.TryGetValue(filePath, out node)) return false;
                return node.Value.hash == hash && node.Value.blobConfirmed;
            }
        }        
        
        public static void SetBlobConfirmed(string filePath, string hash)
        {
            lock (_lock)
            {
                EnsureLoaded();
                LinkedListNode<HashCacheEntry> node;
                if (_map.TryGetValue(filePath, out node) && node.Value.hash == hash && !node.Value.blobConfirmed)
                {
                    node.Value.blobConfirmed = true;
                    _dirty = true;
                }
            }
        }

        /// <summary>
        /// Persists the cache to disk if anything changed since the last Save(). Cheap no-op
        /// otherwise. Call this once after processing a batch of files, not once per file.
        /// </summary>
        public static void Save()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
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
                    //No catastrophe is this happens
                }
            }
            if (G.Equal(Environment.UserName, "tth"))
            {
                MessageBox.Show("TTH: Hash cache save took " + (double)sw.ElapsedMilliseconds / 1000d + " s for " + _map.Count + " entries");
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
        public long stamp; //ticks (100ns units) since DlinkHashCache's fixed epoch
        public string hash;
        //True once we've confirmed (via SyncBlobs) that the blob for "hash" is present in blob storage.
        //Reset to false whenever hash changes.
        public bool blobConfirmed;
    }

    public class HashCacheFile
    {
        public List<HashCacheEntry> entries = new List<HashCacheEntry>();
    }
}
