using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Gekko
{
    /// <summary>
    /// Frequencies that can be compared. Series of different frequencies are never compared with each other.
    /// </summary>
    public enum CompareFreq { Annual, Quarterly, Monthly }

    /// <summary>
    /// Periods are plain integers: year * periodsPerYear + (subperiod - 1).
    /// So consecutive periods are consecutive integers, which keeps all loops trivial.
    /// </summary>
    public static class PeriodText
    {
        public static int PeriodsPerYear(CompareFreq freq)
        {
            switch (freq)
            {
                case CompareFreq.Quarterly: return 4;
                case CompareFreq.Monthly: return 12;
                default: return 1;
            }
        }

        public static int ToIndex(CompareFreq freq, int year, int subPeriod)
        {
            return year * PeriodsPerYear(freq) + (subPeriod - 1);
        }

        public static int Year(CompareFreq freq, int index)
        {
            return index / PeriodsPerYear(freq);
        }

        public static int SubPeriod(CompareFreq freq, int index)
        {
            return index % PeriodsPerYear(freq) + 1;
        }

        public static string Format(CompareFreq freq, int index)
        {
            string year = Year(freq, index).ToString(CultureInfo.InvariantCulture);
            string sub = SubPeriod(freq, index).ToString(CultureInfo.InvariantCulture);
            switch (freq)
            {
                case CompareFreq.Quarterly: return year + "q" + sub;
                case CompareFreq.Monthly: return year + "m" + sub;
                default: return year;
            }
        }
    }

    /// <summary>One timeseries. Data[0] belongs to period Start.</summary>
    public sealed class CompareSeries
    {
        public CompareSeries(string name, CompareFreq freq, int start, double[] data)
        {
            Name = name;
            Freq = freq;
            Start = start;
            Data = data ?? new double[0];
        }

        public string Name { get; }
        public CompareFreq Freq { get; }
        public int Start { get; }
        public double[] Data { get; }
        public int End => Start + Data.Length - 1;

        /// <summary>Returns null when the series has no observation in the period. NaN values are returned as NaN.</summary>
        public double? Get(int period)
        {
            int i = period - Start;
            if (i < 0 || i >= Data.Length) return null;
            return Data[i];
        }
    }

    /// <summary>A databank: series per frequency, looked up by name (case-insensitive).</summary>
    public sealed class CompareBank
    {
        static readonly Dictionary<string, CompareSeries> Empty = new Dictionary<string, CompareSeries>();
        readonly Dictionary<CompareFreq, Dictionary<string, CompareSeries>> byFreq = new Dictionary<CompareFreq, Dictionary<string, CompareSeries>>();

        public CompareBank(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        public string Name { get; }

        /// <summary>Null for banks with artificial data.</summary>
        public string FilePath { get; }

        public void Add(CompareSeries series)
        {
            Dictionary<string, CompareSeries> dict;
            if (!byFreq.TryGetValue(series.Freq, out dict))
            {
                dict = new Dictionary<string, CompareSeries>(StringComparer.OrdinalIgnoreCase);
                byFreq[series.Freq] = dict;
            }
            dict[series.Name] = series;
        }

        public IReadOnlyDictionary<string, CompareSeries> Get(CompareFreq freq)
        {
            Dictionary<string, CompareSeries> dict;
            return byFreq.TryGetValue(freq, out dict) ? dict : Empty;
        }

        public int Count(CompareFreq freq)
        {
            return Get(freq).Count;
        }

        public IEnumerable<CompareFreq> Frequencies
        {
            get { return byFreq.Where(kv => kv.Value.Count > 0).Select(kv => kv.Key).OrderBy(f => f); }
        }
    }

    /// <summary>
    /// Decides whether two observations are equal. a or b is null when there is no observation
    /// in that period, and NaN when the observation is NaN. Plug your own criterion in here.
    /// </summary>
    public interface IEqualityCriterion
    {
        bool AreEqual(double? a, double? b);
    }

    /// <summary>Placeholder criterion: absolute or relative tolerance, missing only equals missing, NaN only equals NaN.</summary>
    public sealed class SimpleEqualityCriterion : IEqualityCriterion
    {
        public double AbsoluteTolerance { get; set; } = 1e-9;
        public double RelativeTolerance { get; set; } = 1e-6;

        public bool AreEqual(double? a, double? b)
        {
            if (!a.HasValue || !b.HasValue) return !a.HasValue && !b.HasValue;
            double x = a.Value, y = b.Value;
            bool xNaN = double.IsNaN(x), yNaN = double.IsNaN(y);
            if (xNaN || yNaN) return xNaN && yNaN;
            if (x == y) return true;
            double diff = Math.Abs(x - y);
            if (double.IsInfinity(diff) || double.IsNaN(diff)) return false;
            return diff <= AbsoluteTolerance || diff <= RelativeTolerance * Math.Max(Math.Abs(x), Math.Abs(y));
        }
    }

    /// <summary>Where real databank files are read. Replace the body with your own reader.</summary>
    public static class DatabankLoader
    {
        public static CompareBank Load(string path)
        {
            // TODO: read the real databank file here and return all its series (all frequencies).
            // For now every dropped file gets artificial data, seeded by its file name.
            return ArtificialData.CreateBank(Path.GetFileName(path), path);
        }
    }

    /// <summary>
    /// Deterministic artificial data. All banks share the same underlying series, so most common
    /// series are equal. Each bank leaves out some series, revises some from a point onwards,
    /// has one-period glitches, NaNs and ragged ends, which gives realistic deviations.
    /// </summary>
    public static class ArtificialData
    {
        static readonly string[] Stems = { "gdp", "cp", "cg", "ip", "e", "m", "pcp", "pe", "pm", "lna", "q", "ul", "tw", "ys", "yd", "wcp", "bf", "fy", "fe", "fm" };
        static readonly string[] Sectors = { "agr", "man", "con", "ser", "pub" };
        static readonly string[] Countries = { "dk", "de", "se" };

        public static CompareBank CreateBank(string name, string filePath)
        {
            var bank = new CompareBank(name, filePath);
            int bankSeed = StableHash(name);
            AddSeries(bank, CompareFreq.Annual, PeriodText.ToIndex(CompareFreq.Annual, 1990, 1), PeriodText.ToIndex(CompareFreq.Annual, 2025, 1), bankSeed);
            AddSeries(bank, CompareFreq.Quarterly, PeriodText.ToIndex(CompareFreq.Quarterly, 2000, 1), PeriodText.ToIndex(CompareFreq.Quarterly, 2025, 4), bankSeed);
            AddSeries(bank, CompareFreq.Monthly, PeriodText.ToIndex(CompareFreq.Monthly, 2010, 1), PeriodText.ToIndex(CompareFreq.Monthly, 2025, 12), bankSeed);
            return bank;
        }

        static void AddSeries(CompareBank bank, CompareFreq freq, int start, int end, int bankSeed)
        {
            int periodsPerYear = PeriodText.PeriodsPerYear(freq);
            foreach (string name in MakeNames(freq))
            {
                int nameHash = StableHash(name + "!" + freq);
                var pick = new Random((nameHash ^ bankSeed) & 0x7FFFFFFF);
                if (pick.NextDouble() > 0.82) continue;  // this bank does not have the series

                double[] full = BaseValues(nameHash, end - start + 1, periodsPerYear);
                int length = full.Length;
                if (pick.NextDouble() < 0.02) length = Math.Max(1, length - 1 - pick.Next(4));  // ragged end
                var values = new double[length];
                Array.Copy(full, values, length);

                double u = pick.NextDouble();
                if (u < 0.06)
                {
                    // Revision from some period onwards
                    int from = pick.Next(length / 2, length);
                    double factor = 1 + (pick.NextDouble() - 0.5) * 0.04;
                    for (int i = from; i < length; i++) values[i] = Math.Round(values[i] * factor, 4);
                }
                else if (u < 0.075)
                {
                    // One-period glitch
                    int k = pick.Next(length);
                    values[k] = Math.Round(values[k] * (1 + 0.01 * (1 + pick.Next(10))), 2);
                }
                if (pick.NextDouble() < 0.01) values[pick.Next(length)] = double.NaN;

                bank.Add(new CompareSeries(name, freq, start, values));
            }
        }

        static double[] BaseValues(int nameHash, int count, int periodsPerYear)
        {
            var rnd = new Random(nameHash & 0x7FFFFFFF);
            double level = Math.Round(10 + rnd.NextDouble() * 990, 0);
            double growth = (rnd.NextDouble() - 0.3) * 0.04 / periodsPerYear;
            double noise = rnd.NextDouble() * 0.03 / Math.Sqrt(periodsPerYear);
            var values = new double[count];
            double x = level;
            for (int i = 0; i < count; i++)
            {
                x *= 1 + growth + (rnd.NextDouble() - 0.5) * noise;
                values[i] = Math.Round(x, 2);
            }
            return values;
        }

        static List<string> MakeNames(CompareFreq freq)
        {
            var names = new List<string>();
            int plain = freq == CompareFreq.Annual ? 260 : freq == CompareFreq.Quarterly ? 160 : 60;
            for (int i = 0; i < plain; i++)
            {
                int k = i / Stems.Length;
                names.Add(Stems[i % Stems.Length] + (k == 0 ? "" : k.ToString(CultureInfo.InvariantCulture)));
            }
            int xCount = freq == CompareFreq.Annual ? 40 : freq == CompareFreq.Quarterly ? 20 : 10;
            for (int i = 1; i <= xCount; i++) names.Add("x" + i.ToString(CultureInfo.InvariantCulture));

            if (freq != CompareFreq.Monthly)
            {
                foreach (string s in Sectors) names.Add("emp[" + s + "]");
                foreach (string s in Sectors)
                    foreach (string c in Countries) names.Add("prod[" + s + "," + c + "]");
            }
            if (freq == CompareFreq.Annual)
            {
                for (int age = 0; age <= 90; age += 10)
                    foreach (string sex in new[] { "m", "f" }) names.Add("pop[a" + age.ToString("00", CultureInfo.InvariantCulture) + "," + sex + "]");
            }
            if (freq == CompareFreq.Monthly)
            {
                foreach (string c in Countries)
                    foreach (string t in new[] { "3m", "1y", "5y", "10y" }) names.Add("rate[" + c + "," + t + "]");
            }
            return names;
        }

        /// <summary>FNV-1a hash, stable across runs and machines (unlike string.GetHashCode on newer runtimes).</summary>
        public static int StableHash(string s)
        {
            unchecked
            {
                uint h = 2166136261;
                foreach (char c in s)
                {
                    h ^= c;
                    h *= 16777619;
                }
                return (int)h;
            }
        }
    }
}
