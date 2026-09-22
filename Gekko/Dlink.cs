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
using ProtoBuf;
using System.Linq;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;

namespace Gekko
{
    public enum EDlinkSetup
    {
        Activate,
        ActivateOnlyHooks,
        ActivateOnlySync,
        DeactivateHooks,
    }

    public enum EDlinkHashKind
    {
        PhysicalFileHash,
        PxContentHash,
        GbkDataHash,
    }

    public enum EDlinkVersion
    {
        None,
        v1_0,
        v1_1
    }

    public static class DlinkSetup
    {        
        
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
                string gekkoPath = Path.Combine(G.CleanupFolderName(Program.options.databank_dlink_folder_storage, false), "_utilities", "Gekko").Replace("\\", "/");
                string gekkoExePath = Path.Combine(G.CleanupFolderName(Program.options.databank_dlink_folder_storage, false), "_utilities", "Gekko", "Gekko.exe").Replace("\\", "/");
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

    public static class DlinkHashKinds
    {        
        public static EDlinkHashKind Classify(string filePath)
        {
            return ClassifyByExtension(Path.GetExtension(filePath));
        }
        
        public static EDlinkHashKind ClassifyByExtension(string extension)
        {
            return DlinkCommon.ClassifyByExtension(DlinkCommon.CurrentVersion, extension);
        }

        /// <summary>
        /// True only for the one kind where persisting this file's byte count alongside its hash is
        /// safe.
        /// </summary>
        public static bool IsByteCountMeaningful(EDlinkHashKind kind)
        {
            return kind == EDlinkHashKind.PhysicalFileHash;
        }
    }

    public static class DlinkAutoDlinkFiles
    {
        
        /// <summary>
        /// Handles blobs, for .dlink.       
        /// </summary>
        /// <param name="dataFile"></param>
        public static void Blob(string dataFile, bool force)
        {
            string hash = null;
            long? bytes = null;
            if (force || Program.options.databank_dlink)
            {
                //Note: just because a .dlink file is constructed, this it not the same
                //      as that it has to go into blobs storage.

                string dlinkFile = DlinkCommon.Dlink_FromDataFileToDlinkFile(dataFile, false);

                if (dlinkFile == null)
                {
                    //Do nothing: may be a databank on some other drive
                }
                else
                {
                    if (File.Exists(dataFile))
                    {                        
                        hash = DlinkHooks.GetFileHash(dataFile);
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

                    EDlinkHashKind hashKind = DlinkHashKinds.Classify(dataFile);

                    if (!DlinkHashKinds.IsByteCountMeaningful(hashKind)) bytes = null; //Not meaningful for px or gbk
                    DlinkFile blobInfo = new DlinkFile(hash, bytes);
                    G.YamlWriter<DlinkFile>(blobInfo, dlinkFile);
                }
            }
        }        
    }    

    /// <summary>
    /// For instance if Python creates a csv file. Or already existing .gbk files are to be put into Git as .dlink files.        
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
                new Error(s2);
            }

            try
            {
                foreach (string dlinkFile2 in dlinkFiles) //Could probably be parallelized
                {
                    DlinkAutoDlinkFiles.Blob(dlinkFile2, true);
                }
            }
            catch
            {
                string s2 = "Producing " + dlinkFiles.Count + " dlink file" + G.S(dlinkFiles.Count) + " failed";
                new Error(s2);
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
            try
            {
                Globals.showErrorsAsMessageBox = true;
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
                        new Error("The folder '" + gitFolder + "' could not be found (parent of \\.git folder)");                        
                    }
                }
                if (G.NullOrBlanks(gitFolder))
                {
                    new Error("Could not get the path to the \\.git folder)");
                }

                //Reads file names from the temporary file created by the _common bash script/hook            
                string dlinkFileArg = args.FirstOrDefault(a => a.StartsWith("-dlink:"));
                if (G.NullOrBlanks(dlinkFileArg))
                {
                    new Error("Could not find the '-dlink:' argument");                    
                }
                string dlinkListFileName = G.StripQuotes(dlinkFileArg.Substring("-dlink:".Length));
                string dlinkListFilePath = Path.Combine(gitFolder, ".git", dlinkListFileName);
                if (!File.Exists(dlinkListFilePath))
                {
                    new Error("Could not find the .dlink file list '" + dlinkListFilePath + "'");
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
            finally
            {
                Globals.showErrorsAsMessageBox = false;
            }
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
                        new Error("'git ls-files' failed in '" + parentOfGitFolder + "':" + G.NL + stderr);
                    }
                    foreach (string line in stdout.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        result.Add(line);
                    }
                }
            }
            catch
            {
                new Error("Could not run 'git' from '" + parentOfGitFolder + "' -- is Git installed and on PATH?");
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
            string blobsFolder = G.CleanupFolderName(Program.options.databank_dlink_folder_storage, false);
            if (!Directory.Exists(blobsFolder))
            {
                new Error("Folder '" + blobsFolder + "' does not exist for file blobs/storage");
            }
            if (!File.Exists(Path.Combine(blobsFolder, "blobsroot.ini")))
            {
                new Error("File '" + Path.Combine(blobsFolder, "blobsroot.ini") + "' does not exist. This is a safety precaution: you may add an empty file with that name.");
            }

            List<string> getFilesNew = new List<string>();
            List<string> getFilesOverwrite = new List<string>();
            List<string> putFiles = new List<string>();
            List<string> backupFiles = new List<string>();
            List<string> errors = new List<string>();
            
            WindowDlinkGitHook progressWindow = new WindowDlinkGitHook("Data file sync (" + type + ")");

            Thread worker = new Thread(delegate ()
            {
                int currentFileIndex = 0; int lastReportedPercent = 0; //for the console-style progress line

                System.Diagnostics.Stopwatch progressStopwatch = System.Diagnostics.Stopwatch.StartNew();
                const int progressReportIntervalMs = 100; //don't push a window update more often than this

                foreach (string dlinkFile2 in dlinkFiles) //Could probably be parallelized, but maybe it is IO bound anyway
                {
                    G.PrintProgress(dlinkFiles.Count, ref currentFileIndex, ref lastReportedPercent, G.Equal(type, "activate"), "data file" + G.S(dlinkFiles.Count) + " synchronized", gap);
                    try
                    {
                        string dLinkFileWithPath = Path.Combine(G.CleanupFolderName(gitFolder, false), G.CleanupFolderName(dlinkFile2, false));
                        if (!File.Exists(dLinkFileWithPath))
                        {
                            new Error("This ." + Program.options.databank_dlink_name + " file does not exist: '" + dLinkFileWithPath + "'");                            
                        }
                        DlinkFile dlinkFileData = G.YamlReader<DlinkFile>(dLinkFileWithPath);
                        EDlinkVersion dlinkVersion = DlinkCommon.ParseDiskVersion(dlinkFileData.version);
                        if (!DlinkCommon.IsVersionSupported(dlinkVersion)) new Error("Dlink file '" + dlinkFile2 + "' has dlink version " + dlinkFileData.version + ", which is unsupported in this Gekko version");
                        string dataFile = DlinkCommon.Dlink_FromDlinkFileToDataFile(dLinkFileWithPath, true);
                        if (G.NullOrBlanks(dataFile))
                        {
                            new Error("Failed getting from .dlink file '" + dLinkFileWithPath + "' to data file name");
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
                        //  Note: we are obviously in A or B here, since we are handling a .dlink file.
                        // --------------------------------------------------------------------------------------------------                                

                        //After this method call, realFile may change regarding .hash and .exists fields (and only those)
                        bool doDlinkFileAndDataFileCorrespond = DoDlinkFileAndDataFileCorrespond(realFile.name, dlinkFileData, ref realFile); //regarding last two args: either both non-null or both null
                        if (doDlinkFileAndDataFileCorrespond)
                        {
                            //Check that we have the file in blobs folder, else add it there. This happens when making a brand new datafile
                            SyncBlobs(false, realFile.name, dlinkFileData.hash, blobsFolder, getFilesNew, getFilesOverwrite, putFiles, dlinkVersion);
                        }
                        else if (realFile.exists && G.Equal(type, "pre-push"))
                        {
                            //A push must never change the working copy. The data file differs from its .dlink file
                            //(probably an uncommitted local change), so leave it alone. All the push needs is that the
                            //blob the .dlink points to exists in storage.
                            if (ResolveExistingBlob(blobsFolder, dlinkFileData.hash, realFile.name) == null)
                            {
                                new Error("For '" + realFile.name + "', could not find a blob file for hash '" + dlinkFileData.hash + "' under '" + blobsFolder + "'");
                            }
                        }
                        else
                        {
                            //Get it from blobs (A or B)
                            //If a different version of the file is already there, it is about to be overwritten. Make sure that version is safe in
                            //blob storage first. (Only if the blob we restore from exists: else SyncBlobs(true) fails below and nothing is overwritten.)
                            if (realFile.exists && ResolveExistingBlob(blobsFolder, dlinkFileData.hash, realFile.name) != null)
                            {
                                string backupPath = ParkLocalVersion(realFile, dlinkVersion, blobsFolder);
                                if (backupPath != null)
                                {
                                    backupFiles.Add(backupPath);
                                }
                            }
                            SyncBlobs(true, realFile.name, dlinkFileData.hash, blobsFolder, getFilesNew, getFilesOverwrite, putFiles, dlinkVersion);
                            FileInfo fi2 = new FileInfo(realFile.name);
                            //We update the realFile, because its contents have changed
                            realFile = new RealFile(realFile.name, dlinkFileData.hash, fi2.Length, fi2.LastWriteTimeUtc, true);
                            //Hash cache remembers this for later
                            DlinkHashCache.Set(HashCacheKey(realFile.name, dlinkVersion), fi2.Length, fi2.LastWriteTimeUtc, dlinkFileData.hash);
                            DlinkHashCache.SetBlobConfirmed(HashCacheKey(realFile.name, dlinkVersion), dlinkFileData.hash);
                        }
                        if ((currentFileIndex == dlinkFiles.Count) || progressStopwatch.ElapsedMilliseconds >= progressReportIntervalMs)
                        {
                            progressWindow.ReportProgress(currentFileIndex, dlinkFiles.Count, dataFile);
                            progressStopwatch.Restart();
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(dlinkFile2 + ": " + ex.Message);
                    }                    
                }
                DlinkHashCache.Save(); //Persist any hashes computed while checking this batch of .dlink files

                string report = BuildSyncReportText(type, getFilesNew, getFilesOverwrite, putFiles, backupFiles);
                progressWindow.Finish(report); //fills the report in, enables OK, and lets ShowDialog() below return once the user dismisses it
            });
            worker.IsBackground = true;            
            worker.SetApartmentState(ApartmentState.STA);
            worker.Start();            
            progressWindow.ShowDialog();

            if (errors.Count > 0)
            {
                new Error("Dlink sync failed for " + errors.Count + " file" + G.S(errors.Count) + ":" + G.NL + string.Join(G.NL, errors));
            }
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
            //Hash the live file under THIS .dlink record's OWN version, not whatever is current --
            //an old v1.1 file must keep being checked by v1.1's rules even once a newer Gekko build's
            //CurrentVersion has moved on to v1.2/v1.3/etc.
            EDlinkVersion dlinkVersion = DlinkCommon.ParseDiskVersion(dlinkFileData.version);
            //We now need to calc the sha256 physically (or for newer .gbk files: fetch data hash).
            //The hash may be gotten from cache file though.
            string realHash = GetFileHash(dataFile, realFile.bytes.Value, realFile.stamp.Value, dlinkVersion);
            realFile = new RealFile(realFile.name, realHash, realFile.bytes, realFile.stamp, true);
            if (dlinkFileData.hash != realHash)
            {                
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Called right before a data file that differs from its .dlink file is about to be overwritten with the
        /// blob version. If the file's CURRENT content is not already recoverable some other way (i.e. no blob
        /// with its hash exists -- this is the case for a locally changed file whose .dlink was never regenerated,
        /// e.g. a csv written by Python), copies it to a sibling ".bakN" file next to it before it's lost, N being
        /// the next unused number (".bak1", ".bak2", ...) so earlier backups are never overwritten, and returns
        /// that path so the caller can list it in the sync report. If the current content's hash IS already a blob
        /// somewhere (e.g. this is an older, already-committed version reached by checking out an earlier commit --
        /// recoverable through that commit's own .dlink file), nothing is written and null is returned; a local
        /// backup would just duplicate what Git/blob storage already has. If a needed backup cannot be written this
        /// throws, and the caller must NOT overwrite the file.
        /// </summary>
        private static string ParkLocalVersion(RealFile realFile, EDlinkVersion dlinkVersion, string blobsFolder)
        {
            //realFile.hash is null if the byte-count check in DoDlinkFileAndDataFileCorrespond returned before hashing.
            string currentHash = realFile.hash ?? GetFileHash(realFile.name, realFile.bytes.Value, realFile.stamp.Value, dlinkVersion);
            if (ResolveExistingBlob(blobsFolder, currentHash, realFile.name) != null)
            {
                return null; //Already safely stored under its own hash -- no local backup needed.
            }

            string backupPath = NextBackupPath(realFile.name);
            try
            {
                File.Copy(realFile.name, backupPath, true);
                //Never leave the backup read-only -- it would make a LATER backup copy at this same path fail.
                FileAttributes attr = File.GetAttributes(backupPath);
                if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    File.SetAttributes(backupPath, attr & ~FileAttributes.ReadOnly);
                }
            }
            catch (Exception ex)
            {
                new Error("'" + realFile.name + "' differs from its .dlink file and would be overwritten, but a backup copy '" + backupPath + "' could not be written first (" + ex.Message + "). The file was NOT overwritten. Move or delete it manually, and sync again.");
            }
            return backupPath;
        }

        /// <summary>
        /// Next unused "fileName.bakN" path (.bak1, .bak2, ...) -- so ParkLocalVersion never overwrites an earlier
        /// backup that hasn't been dealt with yet. No upper bound: if these are never cleaned up they accumulate
        /// indefinitely, one per overwritten local edit.
        /// </summary>
        private static string NextBackupPath(string fileName)
        {
            int n = 1;
            string path;
            do
            {
                path = fileName + ".bak" + n;
                n++;
            } while (File.Exists(path));
            return path;
        }

        private static string BuildSyncReportText(string type, List<string> filesNew, List<string> filesOverwritten, List<string> putFiles, List<string> backupFiles)
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

            if (backupFiles.Count > 0)
            {
                s += G.NL + G.NL;
                s += " ----------------------- LOCAL BACKUPS ----------------------------- ";
                s += G.NL + G.NL;
                s += backupFiles.Count + " local file version" + G.S(backupFiles.Count) + " copied to *.bak{n} before being overwritten:";
                foreach (string f in backupFiles)
                {
                    s += G.NL + f;
                }
            }

            if (false && G.Equal(Environment.UserName, "tth"))
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
        /// Hashes filePath under DlinkCommon.CurrentVersion. Right for a BRAND NEW .dlink record --
        /// there's no existing version to respect yet. To re-verify a file against an EXISTING .dlink
        /// record, use the EDlinkVersion overload below with THAT record's own version instead (see
        /// DoDlinkFileAndDataFileCorrespond), so a build whose CurrentVersion has moved on to v1.2
        /// still hashes an untouched v1.1 file the v1.1 way.
        /// </summary>
        public static string GetFileHash(string filePath)
        {
            return GetFileHash(filePath, DlinkCommon.CurrentVersion);
        }

        public static string GetFileHash(string filePath, EDlinkVersion dlinkVersion)
        {
            FileInfo fi = new FileInfo(filePath);
            return GetFileHash(filePath, fi.Length, fi.LastWriteTimeUtc, dlinkVersion);
        }

        public static string GetFileHash(string filePath, long knownSize, DateTime knownLastWriteUtc)
        {
            return GetFileHash(filePath, knownSize, knownLastWriteUtc, DlinkCommon.CurrentVersion);
        }

        /// <summary>
        /// DlinkHashCache's entries are keyed by this rather than a bare filePath, everywhere in
        /// DlinkHooks that touches it (GetFileHash below, and SyncBlobs/DlinkSyncFiles's
        /// IsBlobConfirmed/SetBlobConfirmed/Set calls) -- so hashing, or confirming a blob for, the
        /// same untouched file under two different .dlink versions (e.g. right after bumping
        /// CurrentVersion from v1.1 to v1.2) can never read or write the wrong version's row. "|" is
        /// illegal in a real file path, so it can't collide with one. This only affects the cache's
        /// lookup key -- the real filePath is still what actually gets opened, hashed, or fetched.
        /// </summary>
        private static string HashCacheKey(string filePath, EDlinkVersion dlinkVersion)
        {
            return filePath + "|dlinkVersion=" + dlinkVersion;
        }

        public static string GetFileHash(string filePath, long knownSize, DateTime knownLastWriteUtc, EDlinkVersion dlinkVersion)
        {
            if (G.DlinkDebug()) MessageBox.Show("Getting hash from " + filePath);

            string cacheKey = HashCacheKey(filePath, dlinkVersion);

            // ---- LRU cache lookup ---------------------------------------------------------
            string cachedHash = DlinkHashCache.TryGet(cacheKey, knownSize, knownLastWriteUtc);
            if (cachedHash != null)
            {
                if (G.DlinkDebug()) MessageBox.Show("Getting hash from LRU cache");
                return cachedHash;
            }            

            string hash = ComputeHashUncached(filePath, dlinkVersion);

            // ---- Remember this result for next time -----------------------------------------            
            DlinkHashCache.Set(cacheKey, knownSize, knownLastWriteUtc, hash);
            // ----------------------------------------------------------------------------------------

            return hash;
        }        

        private static string ComputeHashUncached(string filePath, EDlinkVersion dlinkVersion, string forceFileType = null)
        {
            // Every ".px"/".gbk" special case -- including .gbk's data-hash-from-zip extraction, and
            // the "which extension is even valid as a forceFileType" check -- lives in exactly one
            // place: DlinkCommon.GetSha256FromFileWithDlink's VersionRegistry. This method doesn't need
            // to know either extension string, branch on EDlinkHashKind, or decide which version is
            // "current" itself -- the caller (GetFileHash / VerifyFetchedBlobHash) decides that.
            return DlinkCommon.GetSha256FromFileWithDlink(filePath, dlinkVersion, forceFileType);
        }

        /// <summary>
        /// Note: Globals.alreadyZipped may change in some future Gekko version (and even change back)
        /// </summary>
        /// <param name="dataFile"></param>
        /// <returns></returns>
        public static bool ShouldZipBlobByCurrentPolicy(string dataFile)
        {
            return !DlinkCommon.alreadyZipped.Contains(Path.GetExtension(dataFile), StringComparer.OrdinalIgnoreCase);
        }

        private class BlobLocation
        {
            public readonly string path;
            public readonly bool zipped; //how to interpret the file AT path -- from ITS suffix, a settled fact, never a fresh policy guess
            public BlobLocation(string path, bool zipped)
            {
                this.path = path;
                this.zipped = zipped;
            }
        }

        /// <summary>
        /// Locates sha256's blob on disk, trying, in order:
        ///   1. The two-level path (\xx\yy\hash_z or \xx\yy\\hash_r) with the suffix TODAY's policy
        ///      (ShouldZipBlobByCurrentPolicy) says fileNameAndPath's extension should have. Right
        ///      for the overwhelming majority of blobs, since whatever wrote this one presumably
        ///      used the same policy that's in effect now.
        ///   2. The SAME two-level path with the OTHER suffix -- in case Globals.alreadyZipped's
        ///      membership changed since this specific blob was written. A blob's own suffix is a
        ///      permanent, self-describing fact about that exact file; the policy check above is
        ///      only ever used to decide which suffix to try FIRST, never to decide how to actually
        ///      read a file once found -- that always comes from the suffix that's actually there.
        /// Returns null if the blob exists nowhere.
        /// </summary>
        private static BlobLocation ResolveExistingBlob(string blobsFolder, string sha256, string fileNameAndPath)
        {
            string level1 = sha256.Substring(0, 2);
            string level2 = sha256.Substring(2, 2);
            bool policyZipped = ShouldZipBlobByCurrentPolicy(fileNameAndPath);

            string primarySuffix = policyZipped ? "_z" : "_r";
            string primaryPath = Path.Combine(blobsFolder, level1, level2, sha256 + primarySuffix);
            if (File.Exists(primaryPath))
            {
                return new BlobLocation(primaryPath, policyZipped);
            }

            string otherSuffix = policyZipped ? "_r" : "_z";
            string otherPath = Path.Combine(blobsFolder, level1, level2, sha256 + otherSuffix);
            if (File.Exists(otherPath))
            {
                return new BlobLocation(otherPath, !policyZipped);
            }

            return null;
        }

        public static void SyncBlobs(bool isGet, string fileNameAndPath, string sha256, string blobsFolder, List<string> getFilesNew, List<string> getFilesOverwrite, List<string> putFiles, EDlinkVersion dlinkVersion = DlinkCommon.CurrentVersion)
        {
            // This uses atomic writes.

            if (isGet)
            {
                // ------------------------------------
                // Getting
                // ------------------------------------
                BlobLocation location = ResolveExistingBlob(blobsFolder, sha256, fileNameAndPath);
                if (location == null)
                {
                    new Error("For '" + fileNameAndPath + "', could not find a blob file for hash '" + sha256 + "' under '" + blobsFolder + "'");                    
                }
                else
                {
                    if (File.Exists(fileNameAndPath)) getFilesOverwrite.Add(fileNameAndPath);
                    else getFilesNew.Add(fileNameAndPath);
                    //dlinkVersion passed through so the re-fetched content is verified under the SAME
                    //version's hash rules this .dlink record was written with (defaults to
                    //DlinkCommon.CurrentVersion for any caller that doesn't know a specific version).
                    BlobsFileGet(fileNameAndPath, location.path, location.zipped, sha256, dlinkVersion); //New: sha256 passed through so BlobsFileGet can verify the fetched content
                }
            }
            else
            {
                // ------------------------------------
                // Putting --> when a brand new file is there
                // ------------------------------------
                                
                if (DlinkHashCache.IsBlobConfirmed(HashCacheKey(fileNameAndPath, dlinkVersion), sha256))
                {
                    return;
                }

                BlobLocation existingLocation = ResolveExistingBlob(blobsFolder, sha256, fileNameAndPath);
                if (existingLocation == null)
                {
                    //No need to copy it if we already have it anywhere (new two-level layout,
                    //either suffix) -- this is only reached when it's genuinely nowhere yet, so
                    //write it fresh, at the new two-level, suffixed location, using today's policy
                    //(the only sensible choice for content that's never been stored).                               
                    bool blobZipped = ShouldZipBlobByCurrentPolicy(fileNameAndPath);
                    string level1 = sha256.Substring(0, 2);
                    string level2 = sha256.Substring(2, 2);
                    string blobsFile = Path.Combine(blobsFolder, level1, level2, sha256 + (blobZipped ? "_z" : "_r"));
                    Directory.CreateDirectory(Path.Combine(blobsFolder, level1, level2));
                    BlobsFilePut(fileNameAndPath, blobsFile, blobZipped);
                    putFiles.Add(fileNameAndPath);
                }
                DlinkHashCache.SetBlobConfirmed(HashCacheKey(fileNameAndPath, dlinkVersion), sha256);
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
                //from (e.g. a blob file, which BlobsFilePut always marks read-only).
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
                    catch (Exception ex)
                    {                        
                        if (!File.Exists(finalPath))
                        {
                            new Error("Failed to write '" + finalPath + "' from temp file '" + tempPath + "': " + ex.Message);                            
                        }
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

        private static void BlobsFileGet(string fileNameAndPath, string blobsFile, bool blobZipped, string expectedHash, EDlinkVersion dlinkVersion)
        {
            //We always create the folder in case it does not already exist. For cloning this is obviously important.
            Directory.CreateDirectory(Path.GetDirectoryName(fileNameAndPath));
            
            AtomicWrite(fileNameAndPath, false, tempPath =>
            {
                if (!blobZipped)
                {
                    File.Copy(blobsFile, tempPath, true);
                }
                else
                {
                    using (ZipArchive archive = ZipFile.OpenRead(blobsFile))
                    {
                        ZipArchiveEntry entry = archive.GetEntry("storage");
                        if (entry == null)
                        {                            
                            new Error("File '" + blobsFile + "' is a zip archive but has no 'storage' entry which was expected -- the file may be corrupt.");
                            new Error();
                        }
                        entry.ExtractToFile(tempPath, true);
                    }
                }

                //Verify the just-fetched content actually hashes to what the .dlink file
                //expects, BEFORE AtomicWrite (running next). 
                VerifyFetchedBlobHash(tempPath, fileNameAndPath, expectedHash, dlinkVersion);
            });
            G.ReadOnlyRemove(fileNameAndPath);
        }
        
        private static void VerifyFetchedBlobHash(string tempPath, string originalFileNameAndPath, string expectedHash, EDlinkVersion dlinkVersion)
        {
            string extension = Path.GetExtension(originalFileNameAndPath);            
            string forceFileType = DlinkCommon.IsRecognizedForceFileType(dlinkVersion, extension) ? extension : null;
            string actualHash = ComputeHashUncached(tempPath, dlinkVersion, forceFileType);
            if (!G.Equal(actualHash, expectedHash))
            {
                new Error("Fetched blob for '" + originalFileNameAndPath + "' does not match its expected hash (expected '" + expectedHash + "', got '" + actualHash + "') -- the stored blob may be missing, corrupted, or have been replaced.");
            }
        }

        private static void BlobsFilePut(string fileName, string blobsFile, bool blobZipped)
        {            
            AtomicWrite(blobsFile, true, tempPath =>
            {
                if (!blobZipped) //New: was Globals.alreadyZipped.Contains(Path.GetExtension(fileName), ...) -- now decided once by SyncBlobs (ShouldZipBlobByCurrentPolicy) and passed in
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
        //Always DlinkCommon.CurrentVersion's disk string as of construction -- bumping CurrentVersion
        //is the ONE place that changes what NEW .dlink files get written as. Files already on disk
        //keep whatever version string they were written with (read back via DlinkCommon.ParseDiskVersion).
        public readonly string version = DlinkCommon.DiskVersionString(DlinkCommon.CurrentVersion);
        public string hash { get; private set; }
        public long? bytes { get; private set; }                

        public DlinkFile()
        {
        }

        public DlinkFile(string hash, long? bytes)
        {
            this.hash = hash;
            this.bytes = bytes;            
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
                string folder = Path.Combine(G.CleanupFolderName(Program.options.databank_dlink_folder_storage, false), "_utilities", "hashcache");
                //Per-machine file name: this folder is shared/network storage (databank_dlink_folder_storage),
                //and giving each machine its own cache file avoids two machines racing on the same file.                
                return Path.Combine(folder, "hashcache_" + Environment.MachineName + ".cache"); //New: was ".yaml" -- now a protobuf-net binary file
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
                    HashCacheFile cf;
                    using (FileStream stream = File.OpenRead(CacheFilePath))
                    using (GZipStream gzipStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        cf = ProtoBuf.Serializer.Deserialize<HashCacheFile>(gzipStream);
                    }
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
        /// of what was recorded last time). Returns null on a miss.
        /// </summary>
        public static string TryGet(string filePath, long size, DateTime lastWriteUtc)
        {
            lock (_lock)
            {
                countAsk++;
                EnsureLoaded();
                LinkedListNode<HashCacheEntry> node;
                if (!_map.TryGetValue(filePath, out node))
                {                    
                    return null;
                }

                HashCacheEntry e = node.Value;
                if (e.bytes != size)
                {                    
                    return null;
                }
                long ticksNow = ToTicksSinceEpoch(lastWriteUtc);
                if (Math.Abs(ticksNow - e.stamp) > ToleranceTicks)
                {                    
                    return null;
                }

                //Hit: touch it so it counts as recently used
                countHit++;
                _lru.Remove(node);
                _lru.AddFirst(node);                
                return e.hash;
            }
        }

        /// <summary>
        /// Records/refreshes the hash for filePath. Evicts the least-recently-used entry once the
        /// cache is over capacity. Does not touch disk -- call Save() once after a batch of files.
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
                    using (FileStream stream = File.Create(CacheFilePath))
                    using (GZipStream gzipStream = new GZipStream(stream, CompressionLevel.Fastest))
                    {
                        ProtoBuf.Serializer.Serialize(gzipStream, cf);
                    }
                    _dirty = false;
                }
                catch
                {
                    //No catastrophe is this happens
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
    
    [ProtoContract] //New
    public class HashCacheEntry
    {
        [ProtoMember(1)] public string path;
        [ProtoMember(2)] public long bytes; 
        [ProtoMember(3)] public long stamp; //ticks (100ns units) since DlinkHashCache's fixed epoch 
        [ProtoMember(4)] public string hash;        
        //True once we've confirmed (via SyncBlobs) that the blob for "hash" is present in blob storage.
        //Reset to false whenever hash changes.
        [ProtoMember(7)] public bool blobConfirmed; //New
    }

    [ProtoContract] 
    public class HashCacheFile
    {
        [ProtoMember(1)] public List<HashCacheEntry> entries = new List<HashCacheEntry>();
    }

    public static class DlinkCommon
    {
        public static List<string> alreadyZipped = new List<string>() { ".7z", ".docm", ".docx", ".gbk", ".gz", ".ods", ".odt", ".odp", ".parquet", ".pdf", ".pptm", ".pptx", ".rar", ".rds", ".xlsm", ".xlsx", ".zip" };

        // ============================================================================================
        // SINGLE SOURCE OF TRUTH for ".px" / ".gbk" / EDlinkHashKind / EDlinkVersion -- and now for
        // "which .dlink versions exist at all", "what does each one do", and "which one is current".
        //
        // TO ADD v1_2 (or v1_3, etc.), once its behavior is decided:
        //   1. Add the member to the EDlinkVersion enum (top of file).
        //   2. Add a VersionRegistry entry below for it -- DiskVersionString, IsSupported, and its
        //      ExtensionRules (reuse v1_1's rules/lambdas for anything that hasn't changed).
        //   3. If it's replacing v1_1 as the version NEW .dlink files get written as, change
        //      CurrentVersion below to point at it. That is the ONLY place that decides "current" --
        //      every existing v1_1 file keeps being read and re-hashed under v1_1's own rules forever,
        //      because DoDlinkFileAndDataFileCorrespond/VerifyFetchedBlobHash always look up the
        //      version recorded IN that file, not CurrentVersion.
        // Skip step 2, or mistype the DiskVersionString, and DlinkCommon's static constructor throws
        // immediately (see the bottom of this table) -- it is not possible to run Gekko with an
        // EDlinkVersion member that has no registered VersionSpec.
        // ============================================================================================

        /// <summary>One special-extension rule, for one .dlink version.</summary>
        private class ExtensionRule
        {
            public readonly string Extension;
            public readonly EDlinkHashKind HashKind;            
            public readonly Func<string, string> Hasher;

            public ExtensionRule(string extension, EDlinkHashKind hashKind, Func<string, string> hasher)
            {
                Extension = extension;
                HashKind = hashKind;
                Hasher = hasher;
            }
        }

        /// <summary>Everything DlinkCommon needs to know about one .dlink version.</summary>
        private class VersionSpec
        {
            // The exact string stored in / read from a .dlink file's "version" field for this version.
            public readonly string DiskVersionString;
            // False for a version that's recognized (so old files naming it can still be identified
            // and given a clear error) but that this Gekko build refuses to read/hash/write -- e.g.
            // v1_0. True for every version this build can actually operate on.
            public readonly bool IsSupported;
            // This version's .px/.gbk (etc.) rules. Empty for an unsupported version.
            public readonly ExtensionRule[] ExtensionRules;

            public VersionSpec(string diskVersionString, bool isSupported, ExtensionRule[] extensionRules)
            {
                DiskVersionString = diskVersionString;
                IsSupported = isSupported;
                ExtensionRules = extensionRules ?? new ExtensionRule[0];
            }
        }

        private static readonly List<string> CreationDatePrefixV1_1 = new List<string> { "CREATION-DATE=", "TIMEVAL(\"tid\")=" }; //Note: entries here won't match lower-case or with blanks around "=".

        /// <summary>
        /// Extracts the data hash embedded inside a .gbk file's databank-info entry, rather than
        /// hashing the .gbk's raw bytes. Returns null if that fails or isn't present, in which case
        /// the caller falls back to a plain physical-file hash of the .gbk itself.
        /// </summary>
        private static string ExtractGbkDataHashV1_1(string filePath)
        {
            string hash = null;
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
                            new Error("Could not extract data hash from inside .gbk file (" + Globals.databankInfoName + ").\nFile: " + filePath);
                        }
                    }
                }
            }
            return hash;
        }

        /// <summary>
        /// The version newly-created .dlink files are written as, and what "no specific version"
        /// callers (e.g. constructing a brand-new .dlink file from scratch) mean by "the current
        /// rules". Bumping this is the ONLY change needed to make a new version "the" version --
        /// reading/verifying old files of earlier versions keeps working unchanged, since they carry
        /// their own version string and are always looked up by THAT, never by CurrentVersion.
        /// Must be a version registered below with IsSupported == true (the static constructor below
        /// checks this too).
        /// </summary>
        public const EDlinkVersion CurrentVersion = EDlinkVersion.v1_1;

        // One entry per EDlinkVersion member (except None -- see the static constructor below).
        private static readonly Dictionary<EDlinkVersion, VersionSpec> VersionRegistry = new Dictionary<EDlinkVersion, VersionSpec>
        {
            [EDlinkVersion.v1_0] = new VersionSpec("1.0", isSupported: false, extensionRules: null),

            [EDlinkVersion.v1_1] = new VersionSpec("1.1", isSupported: true, extensionRules: new[]
            {
                new ExtensionRule(".px", EDlinkHashKind.PxContentHash,
                    hasher: filePath => G.FileHasher.GetSha256ExcludingLine(filePath, CreationDatePrefixV1_1)),
                new ExtensionRule(".gbk", EDlinkHashKind.GbkDataHash,
                    hasher: ExtractGbkDataHashV1_1),
            }),

            // Add v1_2, v1_3, etc. here as their own VersionSpec entry when the time comes -- see the
            // "TO ADD v1_2" comment at the top of this table.
        };

        /// <summary>
        /// Fails fast, at type-load time, if EDlinkVersion ever gains a member with no matching
        /// VersionSpec above -- so "add the enum member, forget the registry row" cannot silently
        /// compile-and-ship; it cannot even run. None is deliberately exempt: it is the "no/unspecified
        /// version" sentinel and is never meant to have a VersionSpec.
        /// </summary>
        static DlinkCommon()
        {
            foreach (EDlinkVersion v in Enum.GetValues(typeof(EDlinkVersion)))
            {
                if (v == EDlinkVersion.None) continue;
                if (!VersionRegistry.ContainsKey(v))
                {
                    new Error("EDlinkVersion." + v + " has no VersionSpec registered in DlinkCommon.VersionRegistry -- add one before this build can be used (see the comment above VersionRegistry).");
                }
            }
            if (!VersionRegistry.TryGetValue(CurrentVersion, out VersionSpec currentSpec) || !currentSpec.IsSupported)
            {
                new Error("DlinkCommon.CurrentVersion is set to '" + CurrentVersion + "', which is not a supported VersionSpec -- CurrentVersion must point at a version with IsSupported == true.");
            }
        }

        private static ExtensionRule FindRule(EDlinkVersion dlinkVersion, string extension)
        {
            if (extension != null && VersionRegistry.TryGetValue(dlinkVersion, out VersionSpec spec))
            {
                foreach (ExtensionRule rule in spec.ExtensionRules)
                {
                    if (G.Equal(extension, rule.Extension)) return rule;
                }
            }
            return null;
        }
        
        public static bool IsRecognizedForceFileType(EDlinkVersion dlinkVersion, string extension)
        {
            return FindRule(dlinkVersion, extension) != null;
        }

        /// <summary>
        /// True iff dlinkVersion is a version this build can actually read/hash/write (false for a
        /// recognized-but-retired version like v1_0).
        /// </summary>
        public static bool IsVersionSupported(EDlinkVersion dlinkVersion)
        {
            return VersionRegistry.TryGetValue(dlinkVersion, out VersionSpec spec) && spec.IsSupported;
        }

        /// <summary>
        /// The exact string this version is stored as in a .dlink file's "version" field.
        /// </summary>
        public static string DiskVersionString(EDlinkVersion dlinkVersion)
        {
            return VersionRegistry.TryGetValue(dlinkVersion, out VersionSpec spec) ? spec.DiskVersionString : dlinkVersion.ToString();
        }

        /// <summary>
        /// Reverse lookup: the EDlinkVersion whose DiskVersionString matches the "version" string
        /// found inside an existing .dlink file on disk. This is how an old v1.1 file keeps being
        /// hashed/verified under v1.1's own rules forever, even once CurrentVersion has moved on to
        /// v1.2/v1.3/etc. -- callers pass THIS result, not CurrentVersion, when working with a
        /// specific already-existing .dlink record. Reports a clear error (and returns
        /// EDlinkVersion.None) for a version string this build has never heard of -- e.g. a file
        /// written by a newer Gekko than the one reading it.
        /// </summary>
        public static EDlinkVersion ParseDiskVersion(string diskVersion)
        {
            foreach (KeyValuePair<EDlinkVersion, VersionSpec> kv in VersionRegistry)
            {
                if (G.Equal(diskVersion, kv.Value.DiskVersionString)) return kv.Key;
            }
            new Error("Dlink file has version '" + diskVersion + "', which is not recognized in this Gekko version.");
            return EDlinkVersion.None;
        }

        /// <summary>
        /// The hash strategy that applies to a file with this extension, under this .dlink version --
        /// decided purely by VersionRegistry above. This is the one place Blob() and GetFileHash()
        /// (via DlinkHashKinds) both ask this question -- change what counts as a .px/.gbk file for a
        /// version here, and both follow automatically.
        /// </summary>
        public static EDlinkHashKind ClassifyByExtension(EDlinkVersion dlinkVersion, string extension)
        {
            return FindRule(dlinkVersion, extension)?.HashKind ?? EDlinkHashKind.PhysicalFileHash;
        }

        public static string Dlink_FromDlinkFileToDataFile(string dlinkFile, bool reportError)
        {
            //m1                 K:\MAKROBK\tth\test\makrobk_grunddata\biver\_progs\_uddata_dlink\x.csv.dlink
            //m2                 tth\test\makrobk_grunddata\biver\_progs\_uddata_dlink\x.csv.dlink
            //m3                 tth\test\biver\_uddata_dlink\x.csv.dlink
            //m4                 tth\test\biver\_uddata\x.csv.dlink
            //m5                 tth\test\biver\_uddata\x.csv
            //m6                 k:\\MAKROBK_KILDE\\2025_10_01\tth\test\biver\_uddata\x.csv

            if (Globals.tthDlink1) dlinkFile = G.Replace(dlinkFile, "c:\\tools\\k", "K:", StringComparison.OrdinalIgnoreCase, 1);

            if (!Path.IsPathRooted(Program.options.databank_dlink_folder_progs))
            {
                if (reportError) new Error("Expected path '" + Program.options.databank_dlink_folder_progs + "' to be absolute (option databank dlink folder progs)");
                else return null;
            }
            List<string> dataStart1 = Stringlist.Path_FromStringToList(Program.options.databank_dlink_folder_progs);
            if (!Path.IsPathRooted(Program.options.databank_dlink_folder_data))
            {
                if (reportError) new Error("Expected path '" + Program.options.databank_dlink_folder_data + "' to be absolute (option databank dlink folder data)");
                else return null;
            }
            List<string> dataStart2 = Stringlist.Path_FromStringToList(Program.options.databank_dlink_folder_data);
            if (!Path.IsPathRooted(dlinkFile))
            {
                if (reportError) new Error("Expected path '" + dlinkFile + "' to be absolute (.dlink file)");
                else return null;
            }
            List<string> m1 = Stringlist.Path_FromStringToList(dlinkFile);
            List<string> m2 = Stringlist.Path_RemoveStart(m1, dataStart1, reportError);
            if (m2 == null) return null; //Can only be == null from the above if reportError is false
            List<string> m3 = m2.ToList(); //copy
            if (!G.NullOrBlanks(Program.options.databank_dlink_folder_remove1)) m3 = Stringlist.Path_RemoveString(m3, Program.options.databank_dlink_folder_remove1, 1);
            List<string> m4 = Stringlist.Path_ReplaceString(m3, Program.options.databank_dlink_folder_replace1b, Program.options.databank_dlink_folder_replace1a, 1);
            m4 = Stringlist.Path_ReplaceString(m4, Program.options.databank_dlink_folder_replace2b, Program.options.databank_dlink_folder_replace2a, 1);
            m4 = Stringlist.Path_ReplaceString(m4, Program.options.databank_dlink_folder_replace3b, Program.options.databank_dlink_folder_replace3a, 1);
            List<string> m5 = m4.ToList();
            m5[m5.Count - 1] = m5[m5.Count - 1].Replace("." + Program.options.databank_dlink_name, "");
            List<string> m6 = m5.ToList();
            m6.InsertRange(0, dataStart2);
            return Stringlist.Path_FromListToString(m6, "\\");
        }

        public static string Dlink_FromDataFileToDlinkFile(string dataFile, bool reportError)
        {
            // datastart1  k:\\MAKROBK_KILDE\\2025_10_01
            // datastart2  k:\\MAKROBK
            // m           K:\MAKROBK_KILDE\2025_10_01\tth\test\biver\_uddata\x.csv
            // m2          tth\test\biver\_uddata\x.csv
            // m3          tth\test\makrobk_grunddata\biver\_uddata\x.csv
            // m4          tth\test\makrobk_grunddata\biver\_uddata_dlink\x.csv
            // m5          tth\test\makrobk_grunddata\biver\_uddata_dlink\x.csv.dlink
            // m6          k:\\MAKROBK\tth\test\makrobk_grunddata\biver\_uddata_dlink\x.csv.dlink   (output)            

            if (!Path.IsPathRooted(Program.options.databank_dlink_folder_data))
            {
                if (reportError) new Error("Expected path '" + Program.options.databank_dlink_folder_data + "' to be absolute (options databank dlink folder data)");
                else return null;
            }
            List<string> dataStart1 = Stringlist.Path_FromStringToList(Program.options.databank_dlink_folder_data);
            if (!Path.IsPathRooted(Program.options.databank_dlink_folder_progs))
            {
                if (reportError) new Error("Expected path '" + Program.options.databank_dlink_folder_progs + "' to be absolute (option databank dlink folder progs)");
                else return null;
            }
            List<string> dataStart2 = Stringlist.Path_FromStringToList(Program.options.databank_dlink_folder_progs);
            if (!Path.IsPathRooted(dataFile))
            {
                if (reportError) new Error("Expected path '" + dataFile + "' to be absolute (datafile)");
                else return null;
            }
            List<string> m1 = Stringlist.Path_FromStringToList(dataFile);
            List<string> m2 = Stringlist.Path_RemoveStart(m1, dataStart1, reportError);
            if (m2 == null) return null; //Can only be == null from the above if reportError is false. We allow this for .dlink construction, so that an opened file in a "foreign" folder is ok to read in (no .dlink constucted in that case)            
            int i = 2; if (DlinkCommon.StagingOrMainError(m2)) i = 1;
            List<string> m3 = m2.ToList(); //copy
            if (!G.NullOrBlanks(Program.options.databank_dlink_folder_remove1)) m3.Insert(i, Program.options.databank_dlink_folder_remove1); //hacky, in middle
            List<string> m4 = Stringlist.Path_ReplaceString(m3, Program.options.databank_dlink_folder_replace1a, Program.options.databank_dlink_folder_replace1b, 1);
            m4 = Stringlist.Path_ReplaceString(m4, Program.options.databank_dlink_folder_replace2a, Program.options.databank_dlink_folder_replace2b, 1);
            m4 = Stringlist.Path_ReplaceString(m4, Program.options.databank_dlink_folder_replace3a, Program.options.databank_dlink_folder_replace3b, 1);
            List<string> m5 = m4.ToList();
            m5[m5.Count - 1] += "." + Program.options.databank_dlink_name;
            List<string> m6 = m5.ToList();
            m6.InsertRange(0, dataStart2);
            return Stringlist.Path_FromListToString(m6, "\\");
        }

        public static bool StagingOrMainError(List<string> m2)
        {
            //Sanity check, hacky for now
            List<string> m = new List<string>() { "staging", "main", "prod", "production", "test", "datatest" };
            foreach (string s in m)
            {
                if (G.Equal(m2[0], s)) return true;
            }
            return false;
        }

        /// <summary>
        /// Hashes a file under dlinkVersion's own rules. EVERYTHING about which extensions are special
        /// for that version, what EDlinkHashKind they map to, and how each is actually hashed
        /// (line-excluded content hash for .px, data-hash-from-zip for .gbk, under v1_1) comes from
        /// VersionRegistry above -- nothing version-specific is repeated here, so adding v1_2/v1_3/etc.
        /// needs ZERO changes to this method, only a new VersionRegistry entry.
        /// forceFileType may be == null, but for ".px" or ".gbk" it is used for Dlink blobs with particular name.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="dlinkVersion">Which version's rules to hash under -- the version recorded in the
        /// specific .dlink file being worked with, NOT necessarily DlinkCommon.CurrentVersion (that's
        /// only right for a brand-new file that has no existing version to respect).</param>
        /// <param name="forceFileType">null (default) to use filePath's own extension; otherwise an extension dlinkVersion treats specially (".px"/".gbk" for v1_1).</param>
        /// <returns></returns>
        public static string GetSha256FromFileWithDlink(string filePath, EDlinkVersion dlinkVersion, string forceFileType = null)
        {
            if (forceFileType != null && !IsRecognizedForceFileType(dlinkVersion, forceFileType))
            {
                new Error("forceFileType must be null, or an extension '" + dlinkVersion + "' treats specially -- got '" + forceFileType + "'.");
            }

            if (!IsVersionSupported(dlinkVersion))
            {
                // Covers v1_0 (registered but retired), EDlinkVersion.None, and anything else not
                // marked IsSupported == true -- a genuinely unregistered enum member can't reach this
                // line at all, since DlinkCommon's static constructor already refused to let the
                // program start in that case.
                new Error("Dlink version '" + dlinkVersion + "' is not supported for hashing.");
            }
            else
            {
                string effectiveExtension = forceFileType ?? Path.GetExtension(filePath);
                ExtensionRule rule = FindRule(dlinkVersion, effectiveExtension);
                if (rule != null)
                {
                    string specialHash = rule.Hasher(filePath);
                    if (specialHash != null)
                    {
                        if (G.DlinkDebug()) MessageBox.Show(rule.HashKind == EDlinkHashKind.GbkDataHash ? "Getting hash from xml" : "Getting hash from content hash");
                        return specialHash;
                    }
                    //else: e.g. a .gbk with no extractable data hash inside it -- fall through below,
                    //exactly as if this extension had no rule at all (a plain physical-file hash).
                }
            }
            if (G.DlinkDebug()) MessageBox.Show("Getting hash from physical file");
            string hash = G.FileHasher.GetSha256FromFile(filePath);
            return hash;
        }
    }
}
