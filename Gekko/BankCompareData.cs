using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Gekko
{
    /// <summary>
    /// A Gekko Databank as the compare window sees it: its series per frequency, looked up by name (case-insensitive),
    /// plus the first and last period with data per frequency. Built once when the file is read, so the window does not
    /// have to ask the Databank again. This is the only place that talks to the Databank, so if Gekko's API differs,
    /// only this class needs changing.
    /// </summary>
    public sealed class CompareBank
    {
        /// <summary>The frequencies looked for, in the order the frequency buttons show them.</summary>
        public static readonly EFreq[] AllFreqs = { EFreq.A, EFreq.Q, EFreq.M, EFreq.W, EFreq.D, EFreq.U };

        static readonly Dictionary<string, Series> Empty = new Dictionary<string, Series>();

        readonly Dictionary<EFreq, Dictionary<string, Series>> byFreq = new Dictionary<EFreq, Dictionary<string, Series>>();
        readonly Dictionary<EFreq, GekkoTime[]> spans = new Dictionary<EFreq, GekkoTime[]>();

        public CompareBank(Databank databank, string filePath)
        {
            Databank = databank;
            FilePath = filePath;
            Name = filePath == null ? "" : Path.GetFileName(filePath);
            foreach (EFreq freq in AllFreqs)
            {
                var dict = new Dictionary<string, Series>(StringComparer.OrdinalIgnoreCase);
                GekkoTime first = GekkoTime.tNull, last = GekkoTime.tNull;
                foreach (IVariable iv in databank.StorageFlattenedArrayTimeseries(freq, true).Values)
                {
                    Series ts = iv as Series;
                    if (ts == null) continue;
                    dict[ts.GetName()] = ts;
                    ComparePeriods.Widen(ref first, ref last, ts.GetRealDataPeriodFirst(), ts.GetRealDataPeriodLast());
                }
                if (dict.Count == 0) continue;
                byFreq[freq] = dict;
                spans[freq] = new[] { first, last };
            }
        }

        public Databank Databank { get; }
        public string FilePath { get; }
        public string Name { get; }

        /// <summary>The series of one frequency, by name. Empty if the bank has none.</summary>
        public IReadOnlyDictionary<string, Series> Get(EFreq freq)
        {
            Dictionary<string, Series> dict;
            return byFreq.TryGetValue(freq, out dict) ? dict : Empty;
        }

        public int Count(EFreq freq)
        {
            return Get(freq).Count;
        }

        /// <summary>The frequencies that have series, in the order of AllFreqs.</summary>
        public IEnumerable<EFreq> Frequencies
        {
            get { return AllFreqs.Where(f => byFreq.ContainsKey(f)); }
        }

        /// <summary>First and last period with data over all series of the frequency (tNull when there is none).</summary>
        public void Span(EFreq freq, out GekkoTime first, out GekkoTime last)
        {
            GekkoTime[] s;
            if (spans.TryGetValue(freq, out s))
            {
                first = s[0];
                last = s[1];
            }
            else
            {
                first = GekkoTime.tNull;
                last = GekkoTime.tNull;
            }
        }
    }

    /// <summary>
    /// The periods of one comparison, in one frequency: Start and the following Length - 1 periods. The flag arrays of
    /// the comparison are indexed by the offset k = 0..Length-1 from Start. GekkoTime is used wherever a period means
    /// something (labels, the period filter, reading values); offsets are only used to index arrays, and the two are only
    /// converted here, so frequencies never get mixed up.
    /// </summary>
    public sealed class PeriodAxis
    {
        readonly GekkoTime[] times;

        /// <summary>first..last inclusive, same frequency, both not null and first &lt;= last.</summary>
        public PeriodAxis(GekkoTime first, GekkoTime last)
        {
            Freq = first.freq;
            Start = first;
            int n = Math.Max(0, GekkoTime.Observations(first, last));
            times = new GekkoTime[n];
            GekkoTime t = first;
            for (int k = 0; k < n; k++)
            {
                times[k] = t;
                t = t.Add(1);
            }
        }

        public EFreq Freq { get; }
        public GekkoTime Start { get; }
        public int Length => times.Length;
        public GekkoTime End => times.Length > 0 ? times[times.Length - 1] : GekkoTime.tNull;

        /// <summary>The period at offset k (0..Length-1).</summary>
        public GekkoTime At(int k)
        {
            return times[k];
        }

        /// <summary>The offset of t, which may be &lt; 0 or &gt;= Length (outside the axis). t must have the axis' frequency.</summary>
        public int OffsetOf(GekkoTime t)
        {
            return GekkoTime.Observations(Start, t) - 1;
        }

        /// <summary>
        /// The period filter from..to as offsets, cut to the axis. A null end is open. False (and lo &gt; hi) if nothing
        /// of the axis is left.
        /// </summary>
        public bool Clamp(GekkoTime from, GekkoTime to, out int lo, out int hi)
        {
            lo = from.IsNull() ? 0 : Math.Max(0, OffsetOf(from));
            hi = to.IsNull() ? Length - 1 : Math.Min(Length - 1, OffsetOf(to));
            return lo <= hi;  // when false, loops over lo..hi do nothing
        }
    }

    /// <summary>GekkoTime helpers where tNull means "open" (no limit).</summary>
    public static class ComparePeriods
    {
        /// <summary>The later of two periods (same frequency). A null one is ignored.</summary>
        public static GekkoTime Later(GekkoTime a, GekkoTime b)
        {
            if (a.IsNull()) return b;
            if (b.IsNull()) return a;
            return a.StrictlyLargerThan(b) ? a : b;
        }

        /// <summary>The earlier of two periods (same frequency). A null one is ignored.</summary>
        public static GekkoTime Earlier(GekkoTime a, GekkoTime b)
        {
            if (a.IsNull()) return b;
            if (b.IsNull()) return a;
            return a.StrictlySmallerThan(b) ? a : b;
        }

        /// <summary>Widens first..last (tNull: nothing yet) to include t1..t2 (ignored if null).</summary>
        public static void Widen(ref GekkoTime first, ref GekkoTime last, GekkoTime t1, GekkoTime t2)
        {
            if (t1.IsNull() || t2.IsNull()) return;
            first = Earlier(first, t1);
            last = Later(last, t2);
        }

        /// <summary>a..b holds at least one period (both not null).</summary>
        public static bool IsRange(GekkoTime a, GekkoTime b)
        {
            return !a.IsNull() && !b.IsNull() && a.SmallerThanOrEqual(b);
        }
    }

    /// <summary>Where real databank files are read.</summary>
    public static class DatabankLoader
    {
        /// <summary>
        /// Files are read (and .gbk data hashes extracted) both on the UI thread and by the folder comparison's worker
        /// thread. Everything that goes through Gekko's readers takes this lock, so only one file is handled at a time,
        /// in case those readers use shared state.
        /// </summary>
        public static readonly object Lock = new object();

        public static Databank Load(string path)
        {
            CellOffset offset = new CellOffset();
            ReadOpenMulbkHelper oRead = new ReadOpenMulbkHelper();
            Program.ReadInfo readInfo = new Program.ReadInfo();
            P p = new P();
            string tsdxFile = null; string tempTsdxPath = null;
            int NaNCounter = 0;
            Databank databank = Program.GetDatabankFromFile(offset, oRead, readInfo, path, path, path, oRead.dateformat, oRead.datetype, oRead.bankName, p, ref tsdxFile, ref tempTsdxPath, ref NaNCounter);
            return databank;
        }

        /// <summary>Reads the file and wraps it for the compare window.</summary>
        public static CompareBank LoadForCompare(string path)
        {
            lock (Lock) return new CompareBank(Load(path), path);
        }
    }
}
