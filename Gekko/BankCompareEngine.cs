using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gekko
{
    /// <summary>A drill-down filter on one of the two flags: no filter, flag set, or flag not set.</summary>
    public enum FlagFilter { Any, Yes, No }

    /// <summary>
    /// One series in a Venn area, compared across the area's banks over the whole universal period.
    ///
    /// M means double.NaN, nothing else. A period outside a series' stored range is also M.
    ///   Missing (per bank):  the bank's value in the period is M.
    ///   Deviates (per period): the area's values are not all equal, where M = M counts as equal.
    ///   Examples for banks 1, 2, 3:  1,1,1 equal | 1,2,1 deviates | 1,1,M deviates, missing in 3 |
    ///                                M,M,M equal, missing in all three.
    /// A series in one bank only never deviates.
    /// </summary>
    public sealed class SeriesComparison
    {
        public string Name { get; set; }

        /// <summary>0-based slots (list positions) of the banks the series exists in. Bank number = slot + 1.</summary>
        public int[] Slots { get; set; }

        /// <summary>Parallel to Slots.</summary>
        public CompareSeries[] Series { get; set; }

        /// <summary>The universal period. Arrays are indexed by period - Start.</summary>
        public int Start { get; set; }
        public int End { get; set; }

        public bool[] Deviates { get; set; }

        /// <summary>[index into Slots][period - Start]: true when that bank's value is M.</summary>
        public bool[][] Missing { get; set; }

        // Prefix sums, so that "does it deviate / is it missing anywhere in from..to" is O(1).
        int[] devCum;
        int[][] missCum;

        internal void BuildPrefixSums()
        {
            int n = Deviates.Length;
            devCum = new int[n + 1];
            for (int i = 0; i < n; i++) devCum[i + 1] = devCum[i] + (Deviates[i] ? 1 : 0);
            missCum = new int[Missing.Length][];
            for (int s = 0; s < Missing.Length; s++)
            {
                missCum[s] = new int[n + 1];
                for (int i = 0; i < n; i++) missCum[s][i + 1] = missCum[s][i] + (Missing[s][i] ? 1 : 0);
            }
        }

        bool Clamp(int from, int to, out int lo, out int hi)
        {
            lo = Math.Max(from, Start) - Start;
            hi = Math.Min(to, End) - Start;
            return lo <= hi && Deviates.Length > 0;
        }

        /// <summary>The value of the series in bank Slots[index], M (NaN) outside its stored range.</summary>
        public double Value(int index, int period)
        {
            double? v = Series[index].Get(period);
            return v.HasValue ? v.Value : double.NaN;
        }

        public bool DeviatesAt(int period)
        {
            int i = period - Start;
            return i >= 0 && i < Deviates.Length && Deviates[i];
        }

        /// <summary>True when the value is M in at least one of the banks in slotMask (bit s = slot s).</summary>
        public bool MissingAt(int period, int slotMask)
        {
            int i = period - Start;
            if (i < 0 || i >= Deviates.Length) return false;
            for (int s = 0; s < Slots.Length; s++)
                if ((slotMask & (1 << Slots[s])) != 0 && Missing[s][i]) return true;
            return false;
        }

        public int DeviatingPeriods(int from, int to)
        {
            int lo, hi;
            return Clamp(from, to, out lo, out hi) ? devCum[hi + 1] - devCum[lo] : 0;
        }

        /// <summary>Number of periods in from..to where the value is M in at least one of the banks in slotMask.</summary>
        public int MissingPeriods(int from, int to, int slotMask)
        {
            int lo, hi;
            if (!Clamp(from, to, out lo, out hi)) return 0;
            int count = 0;
            for (int i = lo; i <= hi; i++)
                for (int s = 0; s < Slots.Length; s++)
                    if ((slotMask & (1 << Slots[s])) != 0 && Missing[s][i]) { count++; break; }
            return count;
        }

        public bool DeviatesIn(int from, int to)
        {
            return DeviatingPeriods(from, to) > 0;
        }

        public bool MissingIn(int from, int to, int slotMask)
        {
            int lo, hi;
            if (!Clamp(from, to, out lo, out hi)) return false;
            for (int s = 0; s < Slots.Length; s++)
                if ((slotMask & (1 << Slots[s])) != 0 && missCum[s][hi + 1] - missCum[s][lo] > 0) return true;
            return false;
        }

        public static bool Passes(FlagFilter filter, bool flag)
        {
            return filter == FlagFilter.Any || (filter == FlagFilter.Yes) == flag;
        }
    }

    /// <summary>
    /// One area of the Venn diagram. Mask bit s is set when the series exist in slot s.
    /// Only banks that are shown count: a hidden bank is simply ignored.
    /// </summary>
    public sealed class VennArea
    {
        public int Mask { get; set; }
        public int[] Slots { get; set; }
        public List<SeriesComparison> Series { get; } = new List<SeriesComparison>();
        public bool IsComparison => Slots.Length >= 2;
    }

    /// <summary>Counts for the Venn diagram, for the current period filter.</summary>
    public sealed class AreaCounts
    {
        public int Total;      // all series (black number)
        public int Deviating;  // deviating in at least one selected period (red number)
        public int Missing;    // M in at least one selected period (blue number)
    }

    public sealed class VennResult
    {
        public CompareFreq Freq { get; set; }
        public int SlotCount { get; set; }
        public bool[] Active { get; set; }
        public int ActiveMask { get; set; }
        public int ActiveCount => Active.Count(a => a);
        public CompareBank[] Banks { get; set; }
        public Dictionary<int, VennArea> Areas { get; } = new Dictionary<int, VennArea>();

        /// <summary>Universal period: all stored observations of the selected frequency in the shown banks.</summary>
        public bool HasSpan { get; set; }
        public int SpanStart { get; set; }
        public int SpanEnd { get; set; }

        public int[] BankSpanStart { get; set; }
        public int[] BankSpanEnd { get; set; }
        public int[] BankSeriesCount { get; set; }

        /// <summary>Set by CompareEngine.UpdateCounts for the current period filter.</summary>
        public Dictionary<int, AreaCounts> Counts { get; } = new Dictionary<int, AreaCounts>();
        public AreaCounts[] BankCounts { get; set; }
        public int CountFrom { get; set; }
        public int CountTo { get; set; }

        public bool IsPeriodRestricted => HasSpan && (CountFrom != SpanStart || CountTo != SpanEnd);

        /// <summary>" in 2000" or " in 2000–2005" when the period filter is set, "" otherwise.</summary>
        public string CountPeriodText
        {
            get
            {
                if (!IsPeriodRestricted) return "";
                return " in " + PeriodText.Format(Freq, CountFrom) + (CountFrom == CountTo ? "" : "–" + PeriodText.Format(Freq, CountTo));
            }
        }

        public string SpanText
        {
            get { return HasSpan ? PeriodText.Format(Freq, SpanStart) + "–" + PeriodText.Format(Freq, SpanEnd) : "no observations"; }
        }

        public string BankSpanText(int slot)
        {
            string freq = Freq.ToString().ToLowerInvariant();
            if (BankSeriesCount[slot] == 0) return "no " + freq + " series";
            string count = BankSeriesCount[slot].ToString(CultureInfo.InvariantCulture) + " series";
            if (BankSpanStart[slot] > BankSpanEnd[slot]) return count;
            return PeriodText.Format(Freq, BankSpanStart[slot]) + "–" + PeriodText.Format(Freq, BankSpanEnd[slot]) + ", " + count;
        }
    }

    /// <summary>Text lines for the value listing, plus which line shows which period.</summary>
    public sealed class DeviationListing
    {
        public List<string> Lines { get; } = new List<string>();
        public Dictionary<int, int> LineOfPeriod { get; } = new Dictionary<int, int>();
    }

    public static class CompareEngine
    {
        static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        /// <summary>
        /// Builds the Venn areas for one frequency. banks are the compared banks in list order (2 or 3),
        /// active says which of them are shown, filter (may be null) selects series by name.
        /// The flags are computed for the whole universal period; the period filter is applied later
        /// (UpdateCounts and the window), so changing it does not need a recomputation.
        /// </summary>
        public static VennResult Compute(IList<CompareBank> banks, bool[] active, CompareFreq freq, Func<string, bool> filter,
                                         IEqualityCriterion criterion)
        {
            int n = banks.Count;
            var result = new VennResult
            {
                Freq = freq,
                SlotCount = n,
                Active = active.Take(n).ToArray(),
                Banks = banks.ToArray(),
                BankSpanStart = new int[n],
                BankSpanEnd = new int[n],
                BankSeriesCount = new int[n],
                BankCounts = new AreaCounts[n]
            };
            for (int s = 0; s < n; s++) if (result.Active[s]) result.ActiveMask |= 1 << s;

            var masks = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            int spanStart = int.MaxValue, spanEnd = int.MinValue;

            for (int s = 0; s < n; s++)
            {
                int start = int.MaxValue, end = int.MinValue, count = 0;
                foreach (CompareSeries ts in banks[s].Get(freq).Values)
                {
                    if (filter != null && !filter(ts.Name)) continue;
                    count++;
                    if (result.Active[s])
                    {
                        int m;
                        masks.TryGetValue(ts.Name, out m);
                        masks[ts.Name] = m | (1 << s);
                    }
                    if (ts.Data.Length == 0) continue;
                    if (ts.Start < start) start = ts.Start;
                    if (ts.End > end) end = ts.End;
                }
                result.BankSpanStart[s] = start;
                result.BankSpanEnd[s] = end;
                result.BankSeriesCount[s] = count;
                if (result.Active[s] && start <= end)
                {
                    spanStart = Math.Min(spanStart, start);
                    spanEnd = Math.Max(spanEnd, end);
                }
            }
            result.HasSpan = spanStart <= spanEnd;
            result.SpanStart = result.HasSpan ? spanStart : 0;
            result.SpanEnd = result.HasSpan ? spanEnd : -1;

            for (int mask = 1; mask < 8; mask++)
            {
                if ((mask & ~result.ActiveMask) != 0) continue;
                result.Areas[mask] = new VennArea { Mask = mask, Slots = SlotsOf(mask, n) };
            }

            foreach (KeyValuePair<string, int> kv in masks)
            {
                VennArea area = result.Areas[kv.Value];
                area.Series.Add(CompareSeriesInBanks(kv.Key, area.Slots, banks, freq, criterion, result.SpanStart, result.SpanEnd));
            }

            foreach (VennArea area in result.Areas.Values)
                area.Series.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Name, b.Name));

            UpdateCounts(result, result.SpanStart, result.SpanEnd);
            return result;
        }

        static int[] SlotsOf(int mask, int slotCount)
        {
            var slots = new List<int>();
            for (int s = 0; s < slotCount; s++) if ((mask & (1 << s)) != 0) slots.Add(s);
            return slots.ToArray();
        }

        static SeriesComparison CompareSeriesInBanks(string name, int[] slots, IList<CompareBank> banks, CompareFreq freq,
                                                     IEqualityCriterion criterion, int start, int end)
        {
            var series = new CompareSeries[slots.Length];
            for (int i = 0; i < slots.Length; i++) series[i] = banks[slots[i]].Get(freq)[name];

            int length = Math.Max(0, end - start + 1);
            var c = new SeriesComparison
            {
                Name = series[0].Name,
                Slots = slots,
                Series = series,
                Start = start,
                End = end,
                Deviates = new bool[length],
                Missing = new bool[slots.Length][]
            };
            for (int i = 0; i < slots.Length; i++) c.Missing[i] = new bool[length];

            var values = new double[slots.Length];
            for (int k = 0; k < length; k++)
            {
                int p = start + k;
                for (int i = 0; i < slots.Length; i++)
                {
                    values[i] = c.Value(i, p);
                    c.Missing[i][k] = double.IsNaN(values[i]);
                }
                if (slots.Length >= 2) c.Deviates[k] = !AllEqual(values, criterion);
            }
            c.BuildPrefixSums();
            return c;
        }

        /// <summary>M = M counts as equal; M and a number are not equal; two numbers use the criterion.</summary>
        public static bool AreEqual(double a, double b, IEqualityCriterion criterion)
        {
            bool ma = double.IsNaN(a), mb = double.IsNaN(b);
            if (ma || mb) return ma && mb;
            return criterion.AreEqual(a, b);
        }

        /// <summary>For 2 banks: equal. For 3 banks: all three pairwise equal.</summary>
        public static bool AllEqual(double[] values, IEqualityCriterion criterion)
        {
            for (int i = 0; i < values.Length; i++)
                for (int j = i + 1; j < values.Length; j++)
                    if (!AreEqual(values[i], values[j], criterion)) return false;
            return true;
        }

        /// <summary>Counts per area and per whole bank, for the period filter from..to.</summary>
        public static void UpdateCounts(VennResult r, int from, int to)
        {
            r.CountFrom = from;
            r.CountTo = to;
            r.Counts.Clear();
            foreach (VennArea area in r.Areas.Values)
                r.Counts[area.Mask] = Count(area.Series, area.Mask, from, to);
            for (int s = 0; s < r.SlotCount; s++)
            {
                IEnumerable<SeriesComparison> all = r.Areas.Values.Where(a => (a.Mask & (1 << s)) != 0).SelectMany(a => a.Series);
                r.BankCounts[s] = r.Active[s] ? Count(all, 1 << s, from, to) : new AreaCounts();
            }
        }

        static AreaCounts Count(IEnumerable<SeriesComparison> series, int missingMask, int from, int to)
        {
            var c = new AreaCounts();
            foreach (SeriesComparison s in series)
            {
                c.Total++;
                if (s.DeviatesIn(from, to)) c.Deviating++;
                if (s.MissingIn(from, to, missingMask)) c.Missing++;
            }
            return c;
        }

        /// <summary>Per period start..end: the number of the series that deviate.</summary>
        public static int[] DeviationCounts(IEnumerable<SeriesComparison> series, int start, int end)
        {
            var counts = new int[Math.Max(0, end - start + 1)];
            foreach (SeriesComparison c in series)
                for (int p = start; p <= end; p++)
                    if (c.DeviatesAt(p)) counts[p - start]++;
            return counts;
        }

        /// <summary>
        /// Per period start..end: the number of the series that are M in at least one of the banks in slotMask.
        /// perSlot[s][period - start] counts bank s alone (a series M in two banks counts in both).
        /// </summary>
        public static int[] MissingCounts(IEnumerable<SeriesComparison> series, int slotMask, int start, int end, out int[][] perSlot)
        {
            int length = Math.Max(0, end - start + 1);
            var counts = new int[length];
            perSlot = new int[3][];
            for (int s = 0; s < 3; s++) perSlot[s] = new int[length];
            foreach (SeriesComparison c in series)
            {
                for (int k = 0; k < length; k++)
                {
                    int i = start + k - c.Start;
                    if (i < 0 || i >= c.Deviates.Length) continue;
                    bool any = false;
                    for (int j = 0; j < c.Slots.Length; j++)
                    {
                        if ((slotMask & (1 << c.Slots[j])) == 0 || !c.Missing[j][i]) continue;
                        perSlot[c.Slots[j]][k]++;
                        any = true;
                    }
                    if (any) counts[k]++;
                }
            }
            return counts;
        }

        /// <summary>
        /// Wildcard mode: patterns separated by spaces, * and ? as usual, whole name must match, any pattern may match.
        /// Regex mode: case-insensitive, matches anywhere unless anchored with ^ and $.
        /// Returns null when there is no filter (or it is invalid, then error is set).
        /// </summary>
        public static Func<string, bool> BuildFilter(string text, bool isRegex, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(text)) return null;
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
            if (isRegex)
            {
                try
                {
                    var rx = new Regex(text.Trim(), options);
                    return name => rx.IsMatch(name);
                }
                catch (ArgumentException ex)
                {
                    error = "Invalid regular expression: " + ex.Message;
                    return null;
                }
            }
            List<Regex> patterns = text.Split(new[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => new Regex("^" + Regex.Escape(p).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", options))
                .ToList();
            return name => patterns.Any(rx => rx.IsMatch(name));
        }

        public static string StripDimensions(string name)
        {
            int i = name.IndexOf('[');
            return i < 0 ? name : name.Substring(0, i);
        }

        /// <summary>With ignoreDimensions, x[a,b] and x[a,c] both become x (listed once).</summary>
        public static List<string> NamesForDisplay(IEnumerable<string> names, bool ignoreDimensions)
        {
            if (!ignoreDimensions) return names.ToList();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var list = new List<string>();
            foreach (string name in names)
            {
                string stripped = StripDimensions(name);
                if (seen.Add(stripped)) list.Add(stripped);
            }
            list.Sort(StringComparer.OrdinalIgnoreCase);
            return list;
        }

        /// <summary>Plain-language description of an area, e.g. "In banks 1 and 2, not in bank 3".</summary>
        public static string DescribeArea(VennResult r, int mask)
        {
            var inside = new List<int>();
            var outside = new List<int>();
            var hidden = new List<int>();
            for (int s = 0; s < r.SlotCount; s++)
            {
                if (!r.Active[s]) hidden.Add(s + 1);
                else if ((mask & (1 << s)) != 0) inside.Add(s + 1);
                else outside.Add(s + 1);
            }

            var sb = new StringBuilder();
            if (inside.Count == 1)
            {
                sb.Append(outside.Count > 0 ? "Only in bank " : "In bank ").Append(inside[0]);
            }
            else
            {
                sb.Append("In banks ").Append(JoinWords(inside, "and"));
                if (outside.Count > 0) sb.Append(", not in bank ").Append(JoinWords(outside, "or"));
            }
            if (hidden.Count > 0) sb.Append(hidden.Count == 1 ? " (bank " : " (banks ").Append(JoinWords(hidden, "and")).Append(" hidden)");
            return sb.ToString();
        }

        /// <summary>Counts as text, e.g. "100 series, 17 deviating, 5 with missing values in 2000".</summary>
        public static string DescribeCounts(VennResult r, AreaCounts c, bool isComparison)
        {
            string text = c.Total.ToString(Inv) + " series";
            if (isComparison) text += ", " + c.Deviating.ToString(Inv) + " deviating";
            text += ", " + c.Missing.ToString(Inv) + " with missing values";
            return text + r.CountPeriodText;
        }

        /// <summary>Description plus counts, e.g. "In banks 1 and 2, not in bank 3: 100 series, 17 deviating, 5 with missing values".</summary>
        public static string SummarizeArea(VennResult r, int mask)
        {
            VennArea area = r.Areas[mask];
            AreaCounts c;
            if (!r.Counts.TryGetValue(mask, out c)) c = new AreaCounts { Total = area.Series.Count };
            return DescribeArea(r, mask) + ": " + DescribeCounts(r, c, area.IsComparison);
        }

        static string JoinWords(List<int> numbers, string conjunction)
        {
            if (numbers.Count == 1) return numbers[0].ToString(Inv);
            return string.Join(", ", numbers.Take(numbers.Count - 1).Select(x => x.ToString(Inv)))
                + " " + conjunction + " " + numbers[numbers.Count - 1].ToString(Inv);
        }

        /// <summary>
        /// Text listing of one series for the periods from..to. Two banks: period, x(1), x(2), abs dev., rel. dev.
        /// One or three banks: period and the values. Deviating periods end with " *".
        /// onlyFlagged: only periods that deviate or have M in a bank.
        /// </summary>
        public static DeviationListing FormatComparison(SeriesComparison c, CompareFreq freq, int from, int to, bool onlyFlagged)
        {
            var listing = new DeviationListing();
            int n = c.Series.Length;
            bool pair = n == 2;
            var heads = new string[n];
            for (int i = 0; i < n; i++) heads[i] = c.Name + "(" + (c.Slots[i] + 1).ToString(Inv) + ")";

            const int periodWidth = 10, absWidth = 14, relWidth = 12;
            int valueWidth = Math.Max(13, heads.Max(h => h.Length) + 3);

            var sb = new StringBuilder();
            sb.Append("period".PadRight(periodWidth));
            foreach (string h in heads) sb.Append(h.PadLeft(valueWidth));
            if (pair)
            {
                sb.Append("abs dev.".PadLeft(absWidth));
                sb.Append("rel. dev.".PadLeft(relWidth));
            }
            listing.Lines.Add(sb.ToString());

            int all = (1 << 3) - 1;
            var values = new double[n];
            for (int p = Math.Max(from, c.Start); p <= Math.Min(to, c.End); p++)
            {
                bool deviates = c.DeviatesAt(p);
                if (onlyFlagged && !deviates && !c.MissingAt(p, all)) continue;
                for (int i = 0; i < n; i++) values[i] = c.Value(i, p);

                sb.Clear();
                sb.Append(PeriodText.Format(freq, p).PadRight(periodWidth));
                for (int i = 0; i < n; i++) sb.Append(FormatValue(values[i]).PadLeft(valueWidth));
                if (pair)
                {
                    if (IsNumber(values[0]) && IsNumber(values[1]))
                    {
                        double a = values[0], d = values[1] - a;
                        sb.Append(FormatNumber(d).PadLeft(absWidth));
                        sb.Append((a != 0 ? FormatPercent(d / Math.Abs(a) * 100.0) : "").PadLeft(relWidth));
                    }
                    else
                    {
                        sb.Append(new string(' ', absWidth + relWidth));
                    }
                }
                if (deviates) sb.Append("  *");
                listing.LineOfPeriod[p] = listing.Lines.Count;
                listing.Lines.Add(sb.ToString());
            }
            return listing;
        }

        static bool IsNumber(double x)
        {
            return !double.IsNaN(x) && !double.IsInfinity(x);
        }

        /// <summary>"M" is double.NaN.</summary>
        public static string FormatValue(double x)
        {
            if (double.IsNaN(x)) return "M";
            if (double.IsInfinity(x)) return x > 0 ? "Inf" : "-Inf";
            return FormatNumber(x);
        }

        static string FormatNumber(double x)
        {
            double a = Math.Abs(x);
            if (a != 0 && (a >= 1e12 || a < 1e-4)) return x.ToString("0.#####E+0", Inv);
            return x.ToString("0.00####", Inv);
        }

        static string FormatPercent(double pct)
        {
            string s = Math.Abs(pct) >= 1e5 ? pct.ToString("0.##E+0", Inv) : pct.ToString("0.0###", Inv);
            return s + "%";
        }
    }
}
