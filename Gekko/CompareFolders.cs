using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace Gekko
{
    /// <summary>
    /// Per frequency: summary of the (filtered) series in one copy of a data file, per period of the copy's span
    /// (Start..End: all its observations). Enough to answer Missings and Zeroes for any period and threshold.
    /// The arrays are indexed by the offset from Start.
    /// </summary>
    public sealed class FreqStats
    {
        public int Count;                                                // number of series
        public GekkoTime Start = GekkoTime.tNull, End = GekkoTime.tNull;  // the copy's span; tNull when no series has data

        /// <summary>[offset from Start]: number of series that are M there (also outside their own range); null if none.</summary>
        public int[] Missing;

        /// <summary>[offset from Start]: the smallest |value| that is not M (PositiveInfinity when all are M).</summary>
        public double[] MinAbs;

        public bool HasSpan => ComparePeriods.IsRange(Start, End);

        int Offset(GekkoTime t)
        {
            return GekkoTime.Observations(Start, t) - 1;
        }

        /// <summary>
        /// Some series is M in some period of lo..hi (both not null). lo..hi must lie inside the file's span for this
        /// frequency (outside the copy's own span, every series of the copy is M).
        /// </summary>
        public bool HasMissingIn(GekkoTime lo, GekkoTime hi)
        {
            if (Count == 0 || !ComparePeriods.IsRange(lo, hi)) return false;
            if (!HasSpan || lo.StrictlySmallerThan(Start) || hi.StrictlyLargerThan(End)) return true;
            if (Missing == null) return false;
            for (int k = Offset(lo), b = Offset(hi); k <= b; k++) if (Missing[k] > 0) return true;
            return false;
        }

        /// <summary>The smallest |value| in lo..hi (a null end is open; PositiveInfinity if none).</summary>
        public double MinAbsIn(GekkoTime lo, GekkoTime hi)
        {
            double m = double.PositiveInfinity;
            if (!HasSpan || MinAbs == null) return m;
            lo = ComparePeriods.Later(lo, Start);
            hi = ComparePeriods.Earlier(hi, End);
            if (!ComparePeriods.IsRange(lo, hi)) return m;
            for (int k = Offset(lo), b = Offset(hi); k <= b; k++) if (MinAbs[k] < m) m = MinAbs[k];
            return m;
        }
    }

    /// <summary>
    /// Summary of one copy of a data file (one folder), over all frequencies and the series passing the name filter.
    /// Enough to answer Missings and Zeroes for any period, threshold and set of copies, without reading the file again.
    /// </summary>
    public sealed class CopyStats
    {
        public string Error;   // the file could not be read
        public bool NotRead;   // the series were not read: not a file type that is read (FolderCompareEngine.IsReadable)
        public Dictionary<EFreq, FreqStats> Freqs = new Dictionary<EFreq, FreqStats>();
        public int SeriesCount => Freqs.Values.Sum(f => f.Count);
    }

    /// <summary>
    /// Per frequency and period, the differences between the common series of two copies, as (absolute, relative %)
    /// pairs (CheckSettings.Measure). Only the pairs that no other pair beats in both are kept (the Pareto front):
    /// that is all it takes to tell, for any thresholds, whether some series differs, also when both the absolute and
    /// the relative criterion must be exceeded (the largest absolute and the largest relative difference may belong to
    /// different series). Usually there are only a few pairs per period.
    /// </summary>
    public sealed class PairFreqStats
    {
        /// <param name="front">[offset from start]: the pairs as a1, r1, a2, r2, ... (null: no difference there).</param>
        public PairFreqStats(GekkoTime start, List<double>[] front)
        {
            Start = start;
            End = start.Add(front.Length - 1);
            Front = new double[front.Length][];
            Abs = new double[front.Length];
            Rel = new double[front.Length];
            for (int k = 0; k < front.Length; k++)
            {
                if (front[k] == null) continue;
                Front[k] = front[k].ToArray();
                for (int i = 0; i < Front[k].Length; i += 2)
                {
                    if (Front[k][i] > Abs[k]) Abs[k] = Front[k][i];
                    if (Front[k][i + 1] > Rel[k]) Rel[k] = Front[k][i + 1];
                }
            }
        }

        public readonly GekkoTime Start, End;
        public readonly double[][] Front;  // [offset from Start]: a1, r1, a2, r2, ... or null
        public readonly double[] Abs;      // [offset from Start]: the largest absolute difference (M against a number: infinity)
        public readonly double[] Rel;      // the same, relative (%)

        /// <summary>Adds the pair (abs, rel) to a front (see the class), unless another pair beats it in both.</summary>
        public static void AddToFront(ref List<double> front, double abs, double rel)
        {
            if (front == null) front = new List<double>(2);
            for (int i = 0; i < front.Count; i += 2)
                if (front[i] >= abs && front[i + 1] >= rel) return;
            for (int i = front.Count - 2; i >= 0; i -= 2)
                if (abs >= front[i] && rel >= front[i + 1]) front.RemoveRange(i, 2);
            front.Add(abs);
            front.Add(rel);
        }

        /// <summary>Clamps lo..hi (a null end is open) to the offsets of this span. False if nothing is left.</summary>
        bool Offsets(GekkoTime lo, GekkoTime hi, out int a, out int b)
        {
            a = 0;
            b = -1;
            lo = ComparePeriods.Later(lo, Start);
            hi = ComparePeriods.Earlier(hi, End);
            if (!ComparePeriods.IsRange(lo, hi)) return false;
            a = GekkoTime.Observations(Start, lo) - 1;
            b = GekkoTime.Observations(Start, hi) - 1;
            return true;
        }

        /// <summary>Widens abs and rel with the largest differences in lo..hi (a null end is open).</summary>
        public void Largest(GekkoTime lo, GekkoTime hi, ref double abs, ref double rel)
        {
            int a, b;
            if (!Offsets(lo, hi, out a, out b)) return;
            for (int k = a; k <= b; k++)
            {
                if (Abs[k] > abs) abs = Abs[k];
                if (Rel[k] > rel) rel = Rel[k];
            }
        }

        /// <summary>
        /// The largest difference in lo..hi that counts by the settings (CheckSettings.Exceeds); "largest" by the %
        /// when the relative criterion is on, else absolute. False (and 0, 0) when no difference counts.
        /// </summary>
        public bool LargestCounting(CheckSettings s, GekkoTime lo, GekkoTime hi, ref double abs, ref double rel)
        {
            int a, b;
            bool any = false;
            if (!Offsets(lo, hi, out a, out b)) return false;
            for (int k = a; k <= b; k++)
            {
                double[] f = Front[k];
                if (f == null) continue;
                for (int i = 0; i < f.Length; i += 2)
                {
                    if (!s.Exceeds(f[i], f[i + 1])) continue;
                    any = true;
                    if (s.DiffRelOn ? f[i + 1] > rel : f[i] > abs)
                    {
                        abs = f[i];
                        rel = f[i + 1];
                    }
                }
            }
            return any;
        }
    }

    /// <summary>
    /// Summary of the differences between two copies of a data file, over all frequencies and the series passing the
    /// name filter. Enough to answer Differences for any period and threshold without reading the files again.
    /// </summary>
    public sealed class PairStats
    {
        public string Error;

        /// <summary>For some frequency, a series is in one copy but not the other (an outer area of the file's Venn is not empty).</summary>
        public bool StructureDiffers;

        /// <summary>
        /// The two copies have different content hashes, and their series were not read (not a file type that is read,
        /// see FolderCompareEngine.IsReadable). They count as different whatever the threshold and period.
        /// </summary>
        public bool ContentDiffers;

        /// <summary>The frequencies in which some common series differs in some period (absent: no differences at all).</summary>
        public Dictionary<EFreq, PairFreqStats> Freqs = new Dictionary<EFreq, PairFreqStats>();

        public static readonly PairStats Identical = new PairStats();

        /// <summary>The largest absolute and relative (%) difference of a common series in the period, over all frequencies.</summary>
        public void Largest(PeriodRange range, out double abs, out double rel)
        {
            abs = 0;
            rel = 0;
            foreach (KeyValuePair<EFreq, PairFreqStats> kv in Freqs)
                kv.Value.Largest(range.From(kv.Key), range.To(kv.Key), ref abs, ref rel);
        }

        /// <summary>
        /// The largest difference of a common series in the period that counts by the settings (see
        /// PairFreqStats.LargestCounting), over all frequencies. False when none counts.
        /// </summary>
        public bool LargestCounting(CheckSettings s, PeriodRange range, out double abs, out double rel)
        {
            abs = 0;
            rel = 0;
            bool any = false;
            foreach (KeyValuePair<EFreq, PairFreqStats> kv in Freqs)
                if (kv.Value.LargestCounting(s, range.From(kv.Key), range.To(kv.Key), ref abs, ref rel)) any = true;
            return any;
        }

        /// <summary>
        /// Not the same series, or a common series differs in the period by the settings (CheckSettings.Exceeds: with
        /// both criteria on, one and the same series must exceed both thresholds in one period).
        /// </summary>
        public bool Differs(CheckSettings s, PeriodRange range)
        {
            if (Error != null || StructureDiffers || ContentDiffers) return true;
            double abs, rel;
            return LargestCounting(s, range, out abs, out rel);
        }
    }

    /// <summary>
    /// One data file, found by its path relative to the compared folders (e.g. "\sub\datafile1.gbk"),
    /// in the folders given by Slots.
    /// </summary>
    public sealed class FileComparison
    {
        /// <summary>Path relative to the folder, starting with a separator, e.g. "\sub\datafile1.gbk".</summary>
        public string RelativePath { get; set; }

        /// <summary>0-based slots of the folders the file exists in. Folder number = slot + 1.</summary>
        public int[] Slots { get; set; }

        /// <summary>Parallel to Slots.</summary>
        public string[] FullPaths { get; set; }
        public long[] Sizes { get; set; }
        public DateTime[] Modified { get; set; }      // local time, for showing
        public DateTime[] ModifiedUtc { get; set; }   // for the hash cache
        public CopyStats[] Copies { get; set; }

        /// <summary>[i, j] for i &lt; j, indices into Slots.</summary>
        public PairStats[,] Pairs { get; set; }

        /// <summary>
        /// Copies that hold the same data as an earlier copy point to it (index into Slots); else their own index. Same
        /// data means the same content hash as Dlink computes it (DlinkCommon.GetContentHash): the data hash
        /// inside a .gbk file, the content of a .px file apart from its creation date, else the bytes.
        /// </summary>
        public int[] SameAs { get; set; }

        /// <summary>
        /// Parallel to Slots: the copy's content hash, and what kind of hash it is. Null when the copy was not hashed:
        /// the file is in one folder only, could not be hashed (HashErrors), or is compared by its bytes and no other
        /// copy has its size (so it cannot be equal to one).
        /// </summary>
        public string[] Hashes { get; set; }
        public EDlinkHashKind?[] HashKinds { get; set; }
        public string[] HashErrors { get; set; }

        /// <summary>The series of the file are read (only .gbk files for now), see FolderCompareEngine.IsReadable.</summary>
        public bool IsReadable => FolderCompareEngine.IsReadable(RelativePath);

        /// <summary>"same data hash", "same .px content" or "same bytes", for two copies that are equal.</summary>
        public static string SameText(EDlinkHashKind? kind)
        {
            switch (kind)
            {
                case EDlinkHashKind.GbkDataHash: return "same data hash";
                case EDlinkHashKind.PxContentHash: return "same .px content";
                default: return "same bytes";
            }
        }

        /// <summary>Set by Evaluate: found by the current check (for the file's copies together, i.e. its Venn area).</summary>
        public bool Found { get; set; }

        /// <summary>Set by Evaluate: found in this copy (parallel to Slots), used for "All of folder x".</summary>
        public bool[] FoundInCopy { get; set; }

        /// <summary>Set by Evaluate: why the file is found, for tooltips (empty if not found).</summary>
        public string Why { get; set; }

        public bool IsComparison => Slots.Length >= 2;

        public string Error
        {
            get
            {
                string e = Copies?.Select(c => c?.Error).FirstOrDefault(x => x != null);
                if (e != null || Pairs == null) return e;
                foreach (PairStats p in Pairs) if (p != null && p.Error != null) return p.Error;
                return null;
            }
        }

        /// <summary>
        /// Applies the check in the period (each frequency gets its own first and last period from the range).
        /// The rule mirrors the Venn diagram of the file's series, per frequency:
        ///   Differences: the copies are equal only if every series is in all copies and no common series differs
        ///                beyond the threshold in the period (only the innermost area has series, and its red number is 0).
        ///   Missings:    a copy has M in the period, inside the file's universal period (as in the Datafiles state,
        ///                a series shorter than the file's longest one is M at the ends).
        ///   Zeroes:      a copy has a value with |value| &lt;= threshold in the period.
        /// A file that could not be read is always found.
        /// </summary>
        public void Evaluate(CheckKind check, CheckSettings settings, PeriodRange range)
        {
            int n = Slots.Length;
            FoundInCopy = new bool[n];
            Why = "";
            string error = Error;
            if (error != null)
            {
                Found = true;
                for (int i = 0; i < n; i++) FoundInCopy[i] = true;
                Why = "could not be read: " + error;
                return;
            }

            if (check == CheckKind.Differences)
            {
                Found = false;
                var reasons = new List<string>();
                for (int i = 0; i < n; i++)
                    for (int j = i + 1; j < n; j++)
                    {
                        PairStats p = Pairs[i, j];
                        if (p == null || !p.Differs(settings, range)) continue;
                        Found = true;
                        string pair = "folders " + (Slots[i] + 1) + " and " + (Slots[j] + 1);
                        if (p.ContentDiffers)
                        {
                            reasons.Add(pair + ": the contents differ (the series are only read in .gbk files)");
                            continue;
                        }
                        if (p.StructureDiffers)
                        {
                            reasons.Add(pair + ": not the same series");
                            continue;
                        }
                        double abs, rel;
                        p.LargestCounting(settings, range, out abs, out rel);
                        reasons.Add(pair + ": " + (double.IsInfinity(abs) ? "a value against M"
                                                   : "largest difference " + FormatDiff(abs, false)
                                                     + (double.IsInfinity(rel) ? "" : " (" + FormatDiff(rel, true) + ")")));  // % against 0: left out
                    }
                for (int i = 0; i < n; i++) FoundInCopy[i] = Found;
                Why = string.Join("; ", reasons);
                return;
            }

            if (check == CheckKind.Zeroes)
            {
                for (int i = 0; i < n; i++)
                    FoundInCopy[i] = Copies[i].Freqs.Any(kv => kv.Value.MinAbsIn(range.From(kv.Key), range.To(kv.Key)) <= settings.ZeroThreshold);
                Found = FoundInCopy.Any(x => x);
                Why = Found ? "zero in folder " + string.Join(", ", Enumerable.Range(0, n).Where(i => FoundInCopy[i]).Select(i => Slots[i] + 1)) : "";
                return;
            }

            // Missings: the file's universal period per frequency (over its copies), cut to the period filter.
            Dictionary<EFreq, GekkoTime[]> spans = Spans();
            for (int i = 0; i < n; i++)
            {
                foreach (KeyValuePair<EFreq, FreqStats> kv in Copies[i].Freqs)
                {
                    GekkoTime[] span;
                    if (!spans.TryGetValue(kv.Key, out span)) continue;  // no periods at all
                    GekkoTime lo = ComparePeriods.Later(span[0], range.From(kv.Key));
                    GekkoTime hi = ComparePeriods.Earlier(span[1], range.To(kv.Key));
                    if (kv.Value.HasMissingIn(lo, hi))
                    {
                        FoundInCopy[i] = true;
                        break;
                    }
                }
            }
            Found = FoundInCopy.Any(x => x);
            Why = Found ? "M in folder " + string.Join(", ", Enumerable.Range(0, n).Where(i => FoundInCopy[i]).Select(i => Slots[i] + 1)) : "";
        }

        /// <summary>Per frequency: the first and last period with data in any copy (the file's universal period).</summary>
        public Dictionary<EFreq, GekkoTime[]> Spans()
        {
            var spans = new Dictionary<EFreq, GekkoTime[]>();
            foreach (CopyStats c in Copies)
            {
                if (c == null) continue;
                foreach (KeyValuePair<EFreq, FreqStats> kv in c.Freqs)
                {
                    if (!kv.Value.HasSpan) continue;
                    GekkoTime[] span;
                    if (!spans.TryGetValue(kv.Key, out span)) spans[kv.Key] = span = new[] { GekkoTime.tNull, GekkoTime.tNull };
                    ComparePeriods.Widen(ref span[0], ref span[1], kv.Value.Start, kv.Value.End);
                }
            }
            return spans;
        }

        public static string FormatDiff(double d, bool relative)
        {
            if (double.IsInfinity(d)) return "a value against M";
            return d.ToString("G4", CultureInfo.InvariantCulture) + (relative ? "%" : "");
        }
    }

    /// <summary>One area of the folder Venn diagram: the files found in exactly these folders.</summary>
    public sealed class FolderArea
    {
        public int Mask { get; set; }
        public int[] Slots { get; set; }
        public List<FileComparison> Files { get; } = new List<FileComparison>();
        public bool IsComparison => Slots.Length >= 2;
    }

    /// <summary>
    /// The comparison of 1, 2 or 3 folders (the "Folders" state). Files are matched by relative path; the checks
    /// (Differences, Missings, Zeroes, with the same thresholds, period and name filter as the Datafiles state) are
    /// applied to the series inside each file, in all frequencies.
    /// </summary>
    public sealed class FolderResult : IVennModel
    {
        public int SlotCount { get; set; }
        public bool[] Active { get; set; }
        public int ActiveMask { get; set; }
        public int ActiveCount => Active.Count(a => a);

        /// <summary>Full paths of the compared folders.</summary>
        public string[] Folders { get; set; }

        /// <summary>Number of data files in each folder, also for hidden folders.</summary>
        public int[] FileCount { get; set; }

        public Dictionary<int, FolderArea> Areas { get; } = new Dictionary<int, FolderArea>();
        public Dictionary<int, AreaCounts> Counts { get; } = new Dictionary<int, AreaCounts>();
        public AreaCounts[] FolderCounts { get; set; }
        public CheckKind Check { get; set; }

        /// <summary>The period filter used by UpdateCounts.</summary>
        public PeriodRange Period { get; set; } = PeriodRange.All;

        /// <summary>True when the comparison was stopped before all files were read.</summary>
        public bool Cancelled { get; set; }

        /// <summary>Applies the check, thresholds and period to every file (no reading: uses the stored summaries).</summary>
        public void UpdateCounts(CheckKind check, CheckSettings settings, PeriodRange range)
        {
            Check = check;
            Period = range ?? PeriodRange.All;
            foreach (FolderArea area in Areas.Values)
                foreach (FileComparison f in area.Files) f.Evaluate(check, settings, Period);

            Counts.Clear();
            foreach (FolderArea area in Areas.Values)
                Counts[area.Mask] = new AreaCounts { Total = area.Files.Count, Found = area.Files.Count(f => f.Found) };
            FolderCounts = new AreaCounts[SlotCount];
            for (int s = 0; s < SlotCount; s++)
            {
                List<FileComparison> all = FilesOfFolder(s);
                FolderCounts[s] = new AreaCounts { Total = all.Count, Found = all.Count(f => FoundInFolder(f, s)) };
            }
        }

        /// <summary>Found for "All of folder x": Differences as for the file; Missings/Zeroes in that folder's copy.</summary>
        public static bool FoundInFolder(FileComparison f, int slot)
        {
            int i = Array.IndexOf(f.Slots, slot);
            return i >= 0 && f.FoundInCopy != null && f.FoundInCopy[i];
        }

        /// <summary>All files of a shown folder, sorted (the union of the areas inside its circle).</summary>
        public List<FileComparison> FilesOfFolder(int slot)
        {
            return Areas.Values.Where(a => (a.Mask & (1 << slot)) != 0)
                        .SelectMany(a => a.Files)
                        .OrderBy(f => f.RelativePath, StringComparer.OrdinalIgnoreCase)
                        .ToList();
        }

        bool yearsKnown;
        int firstYear, lastYear;

        /// <summary>The first and last year with data in the files read (shown in grey in the empty period boxes).</summary>
        public bool TryGetYears(out int first, out int last)
        {
            if (!yearsKnown)
            {
                yearsKnown = true;
                firstYear = int.MaxValue;
                lastYear = int.MinValue;
                foreach (FileComparison f in Areas.Values.SelectMany(a => a.Files))
                {
                    if (f.Copies == null) continue;
                    foreach (CopyStats c in f.Copies)
                    {
                        if (c == null) continue;
                        foreach (KeyValuePair<EFreq, FreqStats> kv in c.Freqs)
                        {
                            if (!kv.Value.HasSpan || !PeriodConvert.IsDated(kv.Key)) continue;
                            firstYear = Math.Min(firstYear, kv.Value.Start.super);
                            lastYear = Math.Max(lastYear, kv.Value.End.super);
                        }
                    }
                }
            }
            first = firstYear;
            last = lastYear;
            return firstYear <= lastYear;
        }

        public static string FolderName(string path)
        {
            string trimmed = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string name = Path.GetFileName(trimmed);
            return string.IsNullOrEmpty(name) ? trimmed : name;
        }

        // ---- IVennModel

        public IEnumerable<int> AreaMasks => Areas.Keys;

        public bool ShowsFound(int mask)
        {
            FolderArea area;
            return Areas.TryGetValue(mask, out area) && (Check != CheckKind.Differences || area.IsComparison);
        }

        public AreaCounts AreaCountsOf(int mask)
        {
            AreaCounts c;
            return Counts.TryGetValue(mask, out c) ? c : new AreaCounts();
        }

        public string SlotWord => "folder";
        public string ItemWord => "files";
        public string FoundPhrase => "files " + CheckText.Phrase(Check);
        public string SlotName(int slot) => FolderName(Folders[slot]);
        public string SlotInfo(int slot) => FileCount[slot].ToString(CultureInfo.InvariantCulture) + " files";
        public string SlotTip(int slot) => Folders[slot];

        public string FoundText(AreaCounts c)
        {
            return c.Found + " of " + c.Total + " files " + CheckText.Verb(Check) + Period.Suffix + " (in their series, all frequencies)";
        }

        public string Summarize(int mask)
        {
            AreaCounts c = AreaCountsOf(mask);
            return CompareEngine.DescribeArea(SlotCount, Active, mask, "folder") + ": " + c.Total + " files"
                   + (ShowsFound(mask) ? ", " + c.Found + " " + CheckText.Phrase(Check) + Period.Suffix : "");
        }

        public string SummarizeWhole(int slot)
        {
            AreaCounts c = FolderCounts != null && FolderCounts[slot] != null ? FolderCounts[slot] : new AreaCounts();
            bool shows = Check != CheckKind.Differences || ActiveCount > 1;
            return "All of folder " + (slot + 1) + " (" + SlotName(slot) + "): " + c.Total + " files"
                   + (shows ? ", " + c.Found + " " + CheckText.Phrase(Check) + Period.Suffix : "");
        }
    }

    /// <summary>
    /// The compare window's hash cache: Dlink's LruHashCache (the same protobuf file format and LRU logic, and the same
    /// hashing via DlinkCommon.GetFileHashCached), but its own file, in the local folder for temporary files, so content
    /// hashes survive between sessions. Not shared with Dlink's cache, and no .dlink files are involved.
    /// </summary>
    public static class CompareHashCache
    {
        /// <summary>Maximum number of files remembered (the least recently used are dropped).</summary>
        public static int Capacity = 100000;
        public static string CacheFilePath => Path.Combine(System.Windows.Forms.Application.LocalUserAppDataPath, "CompareFolders.cache");
        public static readonly LruHashCache Store = new LruHashCache(() => CacheFilePath, () => Capacity);
    }

    public static class FolderCompareEngine
    {
        /// <summary>
        /// The file types whose series are read (lower case, with the dot). Other files are only compared by their content
        /// hash (see FileComparison.SameAs): equal or not.
        /// </summary>
        public static string[] ReadableExtensions = { ".gbk" };

        /// <summary>Whether the series of a file of this name are read (its extension is in ReadableExtensions).</summary>
        public static bool IsReadable(string path)
        {
            string ext = Path.GetExtension(path ?? "");
            return ReadableExtensions.Any(e => string.Equals(e, ext, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// What the filter box of the Folders state starts with. Empty: all files are compared (.gbk and .px files by
        /// their data, other files by their bytes, as Dlink does). E.g. "*.gbk" for only .gbk files.
        /// </summary>
        public static string DefaultFileFilter => "";

        /// <summary>
        /// Whether a file is compared. fileFilter (null: all files) gets the path relative to the folder and tries it and
        /// the file name (CompareEngine.BuildFileFilter), so both "*.gbk" and "sub\*.gbk" work, also to exclude ("-sub\*").
        /// </summary>
        public static bool IsDataFile(string relativePath, Func<string, bool> fileFilter)
        {
            return fileFilter == null || fileFilter(relativePath);
        }

        /// <summary>
        /// Lists the data files (those passing fileFilter, null: all) in a folder and its subfolders, as relative paths
        /// starting with a separator. Subfolders that cannot be read are skipped.
        /// </summary>
        public static List<string> ListDataFiles(string folder, Func<string, bool> fileFilter, CancellationToken token)
        {
            var result = new List<string>();
            string root = folder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            // A drive root ("D:\") must keep its separator: "D:" means the current directory on drive D.
            string start = root.EndsWith(":") ? root + Path.DirectorySeparatorChar : root;
            var pending = new Stack<string>();
            pending.Push(start);
            while (pending.Count > 0)
            {
                token.ThrowIfCancellationRequested();
                string dir = pending.Pop();
                try
                {
                    foreach (string file in Directory.EnumerateFiles(dir))
                    {
                        string rel = file.Substring(root.Length);
                        if (IsDataFile(rel, fileFilter)) result.Add(rel);
                    }
                    foreach (string sub in Directory.EnumerateDirectories(dir)) pending.Push(sub);
                }
                catch (UnauthorizedAccessException) { }
                catch (IOException) { }
            }
            return result;
        }

        /// <summary>
        /// Compares 1, 2 or 3 folders: lists their files, matches them by relative path, finds the copies that hold the
        /// same data (Dlink's content hash, see CompareHashCache), and reads the series of the .gbk files (see CopyStats and
        /// PairStats). Copies with the same data are read once, and their pair is not compared series by series.
        ///   fileFilter (may be null: all files): which files are compared (the filter box of the Folders state).
        ///   seriesFilter (may be null): only these series count; filterKey identifies it.
        ///   load: reads a data file (DatabankLoader.LoadForCompare).
        ///   progress (may be null): status texts; called on the worker thread.
        ///   cache: summaries from earlier runs in this window (keyed by path, size, time and filter), so hiding a folder
        ///          or comparing again only reads files that changed. Content hashes are cached on disk, between
        ///          sessions, in CompareHashCache.
        /// If token is cancelled while reading, the result is returned with Cancelled set (unread files have no summary
        /// and count as equal, without missing values or zeroes).
        /// </summary>
        public static FolderResult Compute(IList<string> folders, bool[] active, Func<string, bool> fileFilter,
                                           Func<string, bool> seriesFilter, string filterKey,
                                           Func<string, CompareBank> load, Action<string> progress, CancellationToken token,
                                           ConcurrentDictionary<string, object> cache)
        {
            int n = folders.Count;
            var result = new FolderResult
            {
                SlotCount = n,
                Active = active.Take(n).ToArray(),
                Folders = folders.ToArray(),
                FileCount = new int[n]
            };
            for (int s = 0; s < n; s++) if (result.Active[s]) result.ActiveMask |= 1 << s;

            var masks = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int s = 0; s < n; s++)
            {
                progress?.Invoke("Listing the files in " + folders[s] + " ...");
                List<string> files = ListDataFiles(folders[s], fileFilter, token);
                result.FileCount[s] = files.Count;
                if (!result.Active[s]) continue;
                foreach (string rel in files)
                {
                    int m;
                    masks.TryGetValue(rel, out m);
                    masks[rel] = m | (1 << s);
                }
            }

            for (int mask = 1; mask < 8; mask++)
            {
                if ((mask & ~result.ActiveMask) != 0) continue;
                result.Areas[mask] = new FolderArea { Mask = mask, Slots = CompareEngine.SlotsOf(mask, n) };
            }

            foreach (KeyValuePair<string, int> kv in masks)
            {
                FolderArea area = result.Areas[kv.Value];
                int k = area.Slots.Length;
                var fc = new FileComparison
                {
                    RelativePath = kv.Key,
                    Slots = area.Slots,
                    FullPaths = area.Slots.Select(s => folders[s].TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + kv.Key).ToArray(),
                    Sizes = new long[k],
                    Modified = new DateTime[k],
                    ModifiedUtc = new DateTime[k],
                    Copies = new CopyStats[k],
                    Pairs = new PairStats[k, k],
                    SameAs = Enumerable.Range(0, k).ToArray(),
                    Hashes = new string[k],
                    HashKinds = new EDlinkHashKind?[k],
                    HashErrors = new string[k]
                };
                for (int i = 0; i < k; i++)
                {
                    try
                    {
                        var info = new FileInfo(fc.FullPaths[i]);
                        fc.Sizes[i] = info.Length;
                        fc.Modified[i] = info.LastWriteTime;
                        fc.ModifiedUtc[i] = info.LastWriteTimeUtc;
                    }
                    catch (Exception ex)
                    {
                        fc.Copies[i] = new CopyStats { Error = ex.Message };
                    }
                }
                area.Files.Add(fc);
            }
            foreach (FolderArea area in result.Areas.Values)
                area.Files.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.RelativePath, b.RelativePath));

            // Read and summarize. Files in several folders first: they matter for Differences.
            List<FileComparison> all = result.Areas.Values.OrderByDescending(a => a.Slots.Length).SelectMany(a => a.Files).ToList();
            try
            {
                SummarizeAll(all, result, seriesFilter, filterKey, load, progress, token, cache);
            }
            finally
            {
                CompareHashCache.Store.Save();  // the hashes computed now are there next time (also in a later session)
            }
            // Files not reached (cancelled) get empty summaries, so they count as equal.
            foreach (FileComparison fc in all)
            {
                int k = fc.Slots.Length;
                for (int i = 0; i < k; i++)
                {
                    if (fc.Copies[i] == null) fc.Copies[i] = fc.Copies[fc.SameAs[i]] ?? new CopyStats();
                    for (int j = i + 1; j < k; j++) if (fc.Pairs[i, j] == null) fc.Pairs[i, j] = PairStats.Identical;
                }
            }
            return result;
        }

        /// <summary>Reads and summarizes the files in this order (see Summarize). Stops (Cancelled) when the token is cancelled.</summary>
        static void SummarizeAll(List<FileComparison> all, FolderResult result, Func<string, bool> seriesFilter, string filterKey,
                                 Func<string, CompareBank> load, Action<string> progress, CancellationToken token,
                                 ConcurrentDictionary<string, object> cache)
        {
            for (int index = 0; index < all.Count; index++)
            {
                if (token.IsCancellationRequested)
                {
                    result.Cancelled = true;
                    break;
                }
                FileComparison fc = all[index];
                progress?.Invoke("Comparing file " + (index + 1) + " of " + all.Count + ": " + fc.RelativePath);
                try
                {
                    Summarize(fc, seriesFilter, filterKey, load, cache, token);
                }
                catch (OperationCanceledException)
                {
                    result.Cancelled = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Fills fc.Copies and fc.Pairs. First the copies are grouped by content hash (as Dlink does it); copies with the
        /// same data share one summary, and their pair is identical. Then, for .gbk files only, each distinct copy is read
        /// and summarized, and each pair of distinct copies is compared series by series (summaries from the cache where
        /// possible). Other files that differ get PairStats.ContentDiffers.
        /// </summary>
        static void Summarize(FileComparison fc, Func<string, bool> filter, string filterKey, Func<string, CompareBank> load,
                              ConcurrentDictionary<string, object> cache, CancellationToken token)
        {
            int k = fc.Slots.Length;
            bool readable = fc.IsReadable;

            HashCopies(fc, token);
            for (int i = 1; i < k; i++)
            {
                if (fc.Hashes[i] == null) continue;
                for (int j = 0; j < i; j++)
                {
                    if (fc.SameAs[j] != j || fc.Hashes[j] == null) continue;
                    if (fc.Hashes[j] == fc.Hashes[i] && fc.HashKinds[j] == fc.HashKinds[i])
                    {
                        fc.SameAs[i] = j;
                        break;
                    }
                }
            }

            var banks = new Dictionary<int, CompareBank>();
            Func<int, CompareBank> bankOf = i =>
            {
                CompareBank b;
                if (!banks.TryGetValue(i, out b)) banks[i] = b = load(fc.FullPaths[i]);
                return b;
            };

            for (int i = 0; i < k; i++)
            {
                token.ThrowIfCancellationRequested();
                if (fc.Copies[i] != null) continue;  // file info error
                int r = fc.SameAs[i];
                if (r != i)
                {
                    fc.Copies[i] = fc.Copies[r];
                    continue;
                }
                if (!readable)
                {
                    fc.Copies[i] = new CopyStats { NotRead = true };
                    continue;
                }
                string key = "copy|" + CacheKey(fc, i) + "|" + filterKey;
                object cached;
                if (cache != null && cache.TryGetValue(key, out cached))
                {
                    fc.Copies[i] = (CopyStats)cached;
                    continue;
                }
                CopyStats stats;
                try
                {
                    stats = CopyStatsOf(bankOf(i), filter);
                }
                catch (Exception ex)
                {
                    stats = new CopyStats { Error = ex.Message };
                }
                fc.Copies[i] = stats;
                if (cache != null && stats.Error == null) cache[key] = stats;
            }

            for (int i = 0; i < k; i++)
                for (int j = i + 1; j < k; j++)
                {
                    token.ThrowIfCancellationRequested();
                    int ri = fc.SameAs[i], rj = fc.SameAs[j];
                    if (ri == rj)
                    {
                        fc.Pairs[i, j] = PairStats.Identical;
                        continue;
                    }
                    if (fc.Copies[ri].Error != null || fc.Copies[rj].Error != null)
                    {
                        fc.Pairs[i, j] = new PairStats { Error = fc.Copies[ri].Error ?? fc.Copies[rj].Error };
                        continue;
                    }
                    if (!readable)
                    {
                        // Not read: different hashes are all there is to go by. A copy that could not be hashed is an error.
                        string hashError = fc.HashErrors[ri] ?? fc.HashErrors[rj];
                        fc.Pairs[i, j] = hashError != null ? new PairStats { Error = hashError } : new PairStats { ContentDiffers = true };
                        continue;
                    }
                    string ka = CacheKey(fc, ri), kb = CacheKey(fc, rj);
                    string key = "pair|" + (string.CompareOrdinal(ka, kb) <= 0 ? ka + "\n" + kb : kb + "\n" + ka) + "|" + filterKey;
                    object cached;
                    if (cache != null && cache.TryGetValue(key, out cached))
                    {
                        fc.Pairs[i, j] = (PairStats)cached;
                        continue;
                    }
                    PairStats stats;
                    try
                    {
                        stats = PairStatsOf(bankOf(ri), bankOf(rj), filter);
                    }
                    catch (Exception ex)
                    {
                        stats = new PairStats { Error = ex.Message };
                    }
                    fc.Pairs[i, j] = stats;
                    if (cache != null && stats.Error == null) cache[key] = stats;
                }
        }

        /// <summary>The (filtered) series of one frequency, by name (case-insensitive). Empty if the bank lacks the frequency.</summary>
        static Dictionary<string, Series> SeriesOf(CompareBank bank, EFreq freq, Func<string, bool> filter)
        {
            var d = new Dictionary<string, Series>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, Series> kv in bank.Get(freq))
                if (filter == null || filter(kv.Key)) d[kv.Key] = kv.Value;
            return d;
        }

        /// <summary>
        /// Per frequency: the copy's span, and per period how many series are M and the smallest |value|.
        /// M means double.NaN, also outside a series' own range (inside the copy's span).
        /// </summary>
        public static CopyStats CopyStatsOf(CompareBank bank, Func<string, bool> filter)
        {
            var cs = new CopyStats();
            foreach (EFreq freq in bank.Frequencies)
            {
                List<Series> series = SeriesOf(bank, freq, filter).Values.ToList();
                if (series.Count == 0) continue;
                var fs = new FreqStats { Count = series.Count };
                var first = new GekkoTime[series.Count];
                var last = new GekkoTime[series.Count];
                for (int i = 0; i < series.Count; i++)
                {
                    first[i] = series[i].GetRealDataPeriodFirst();
                    last[i] = series[i].GetRealDataPeriodLast();
                    ComparePeriods.Widen(ref fs.Start, ref fs.End, first[i], last[i]);
                }
                cs.Freqs[freq] = fs;
                if (!fs.HasSpan) continue;

                var axis = new PeriodAxis(fs.Start, fs.End);
                int length = axis.Length;
                var outside = new int[length + 1];  // difference array: series that are M because the period is outside their range
                var missing = new int[length];
                var minAbs = new double[length];
                for (int k = 0; k < length; k++) minAbs[k] = double.PositiveInfinity;
                for (int i = 0; i < series.Count; i++)
                {
                    if (!ComparePeriods.IsRange(first[i], last[i]))
                    {
                        outside[0]++;
                        outside[length]--;
                        continue;
                    }
                    int a = axis.OffsetOf(first[i]), b = axis.OffsetOf(last[i]);  // its own range, inside 0..length-1
                    if (a > 0)
                    {
                        outside[0]++;
                        outside[a]--;
                    }
                    if (b < length - 1)
                    {
                        outside[b + 1]++;
                        outside[length]--;
                    }
                    Series ts = series[i];
                    for (int k = a; k <= b; k++)
                    {
                        double x = ts.GetDataSimple(axis.At(k));
                        if (double.IsNaN(x)) missing[k]++;
                        else if (Math.Abs(x) < minAbs[k]) minAbs[k] = Math.Abs(x);
                    }
                }
                bool any = false;
                int run = 0;
                for (int k = 0; k < length; k++)
                {
                    run += outside[k];
                    missing[k] += run;
                    if (missing[k] > 0) any = true;
                }
                fs.Missing = any ? missing : null;
                fs.MinAbs = minAbs;
            }
            return cs;
        }

        /// <summary>
        /// Compares two copies series by series, like the Datafiles Venn would: per frequency, are the same series in
        /// both, and per period the largest difference of a common series (M outside their ranges, M = M is equal).
        /// </summary>
        public static PairStats PairStatsOf(CompareBank a, CompareBank b, Func<string, bool> filter)
        {
            var ps = new PairStats();
            foreach (EFreq freq in a.Frequencies.Union(b.Frequencies))
            {
                Dictionary<string, Series> da = SeriesOf(a, freq, filter);
                Dictionary<string, Series> db = SeriesOf(b, freq, filter);
                if (da.Count != db.Count || da.Keys.Any(name => !db.ContainsKey(name))) ps.StructureDiffers = true;

                // The common series, each with the union of the two series' ranges.
                var common = new List<Series[]>();
                var ranges = new List<GekkoTime[]>();
                GekkoTime start = GekkoTime.tNull, end = GekkoTime.tNull;
                foreach (KeyValuePair<string, Series> kv in da)
                {
                    Series other;
                    if (!db.TryGetValue(kv.Key, out other)) continue;
                    var pair = new[] { kv.Value, other };
                    var range = new[] { GekkoTime.tNull, GekkoTime.tNull };
                    foreach (Series s in pair) ComparePeriods.Widen(ref range[0], ref range[1], s.GetRealDataPeriodFirst(), s.GetRealDataPeriodLast());
                    if (!ComparePeriods.IsRange(range[0], range[1])) continue;  // no observations in either: M against M everywhere
                    common.Add(pair);
                    ranges.Add(range);
                    ComparePeriods.Widen(ref start, ref end, range[0], range[1]);
                }
                if (!ComparePeriods.IsRange(start, end)) continue;

                var axis = new PeriodAxis(start, end);
                List<double>[] front = null;
                for (int p = 0; p < common.Count; p++)
                {
                    Series[] pair = common[p];                    
                    int lo = axis.OffsetOf(ranges[p][0]), hi = axis.OffsetOf(ranges[p][1]);
                    for (int k = lo; k <= hi; k++)
                    {
                        GekkoTime t = axis.At(k);
                        double d, r;
                        CheckSettings.Measure(pair[0].GetDataSimple(t), pair[1].GetDataSimple(t), out d, out r);
                        if (d <= 0 && r <= 0) continue;  // equal
                        if (front == null) front = new List<double>[axis.Length];
                        PairFreqStats.AddToFront(ref front[k], d, r);
                    }
                }
                if (front != null) ps.Freqs[freq] = new PairFreqStats(start, front);
            }
            return ps;
        }

        /// <summary>
        /// Sets fc.Hashes, fc.HashKinds and fc.HashErrors: the content hash of each copy, as Dlink computes it, taken
        /// from CompareHashCache when the file has not changed since it was hashed (also in an earlier session). A file
        /// in one folder only is not hashed (nothing to compare it with), and neither is a copy compared by its bytes
        /// that no other copy has the size of.
        /// </summary>
        static void HashCopies(FileComparison fc, CancellationToken token)
        {
            int k = fc.Slots.Length;
            fc.Hashes = new string[k];
            fc.HashKinds = new EDlinkHashKind?[k];
            fc.HashErrors = new string[k];
            if (k < 2) return;
            bool bytesOnly = DlinkHashKinds.Classify(fc.RelativePath) == EDlinkHashKind.PhysicalFileHash;
            for (int i = 0; i < k; i++)
            {
                if (fc.Copies[i] != null) continue;  // a file info error
                if (bytesOnly)
                {
                    bool sameSize = false;
                    for (int j = 0; j < k; j++)
                        if (j != i && fc.Copies[j] == null && fc.Sizes[j] == fc.Sizes[i]) sameSize = true;
                    if (!sameSize) continue;
                }
                token.ThrowIfCancellationRequested();
                EDlinkHashKind kind;
                string error;
                string hash;
                lock (DatabankLoader.Lock)
                    hash = DlinkCommon.GetContentHash(CompareHashCache.Store, fc.FullPaths[i], fc.Sizes[i], fc.ModifiedUtc[i], out kind, out error);
                fc.Hashes[i] = hash;
                fc.HashKinds[i] = hash != null ? kind : (EDlinkHashKind?)null;
                fc.HashErrors[i] = error;
            }
        }

        static string CacheKey(FileComparison fc, int i)
        {
            return fc.FullPaths[i].ToLowerInvariant() + "|" + fc.Sizes[i] + "|" + fc.ModifiedUtc[i].Ticks;
        }
    }
}
