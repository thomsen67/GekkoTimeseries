using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gekko
{
    /// <summary>The comparison of one series across the banks of a Venn area (2 or 3 banks).</summary>
    public sealed class SeriesComparison
    {
        public string Name { get; set; }

        /// <summary>0-based slots (list positions) that are compared. Bank number shown to the user is slot + 1.</summary>
        public int[] Slots { get; set; }

        /// <summary>Parallel to Slots.</summary>
        public CompareSeries[] Series { get; set; }

        public int Start { get; set; }
        public int End { get; set; }

        /// <summary>Index = period - Start. True when the observations are not all equal.</summary>
        public bool[] Deviates { get; set; }

        public int DeviatingPeriods { get; set; }

        public bool DeviatesAt(int period)
        {
            int i = period - Start;
            return i >= 0 && i < Deviates.Length && Deviates[i];
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
        public List<string> Names { get; } = new List<string>();
        public List<SeriesComparison> Deviations { get; } = new List<SeriesComparison>();
        public bool IsComparison => Slots.Length >= 2;
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

        /// <summary>Universal period: all observations of the selected frequency in the shown banks.</summary>
        public bool HasSpan { get; set; }
        public int SpanStart { get; set; }
        public int SpanEnd { get; set; }

        public int[] BankSpanStart { get; set; }
        public int[] BankSpanEnd { get; set; }
        public int[] BankSeriesCount { get; set; }

        public string SpanText
        {
            get { return HasSpan ? PeriodText.Format(Freq, SpanStart) + "\u2013" + PeriodText.Format(Freq, SpanEnd) : "no observations"; }
        }

        public string BankSpanText(int slot)
        {
            string freq = Freq.ToString().ToLowerInvariant();
            if (BankSeriesCount[slot] == 0) return "no " + freq + " series";
            string count = BankSeriesCount[slot].ToString(CultureInfo.InvariantCulture) + " series";
            if (BankSpanStart[slot] > BankSpanEnd[slot]) return count;
            return PeriodText.Format(Freq, BankSpanStart[slot]) + "\u2013" + PeriodText.Format(Freq, BankSpanEnd[slot]) + ", " + count;
        }
    }

    /// <summary>Text lines for the deviation list, plus which line shows which period.</summary>
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
        /// </summary>
        public static VennResult Compute(IList<CompareBank> banks, bool[] active, CompareFreq freq, Func<string, bool> filter, IEqualityCriterion criterion)
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
                BankSeriesCount = new int[n]
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
                area.Names.Add(kv.Key);
                if (!area.IsComparison) continue;
                SeriesComparison c = CompareSeriesInBanks(kv.Key, area.Slots, banks, freq, criterion);
                if (c.DeviatingPeriods > 0) area.Deviations.Add(c);
            }

            foreach (VennArea area in result.Areas.Values)
            {
                area.Names.Sort(StringComparer.OrdinalIgnoreCase);
                area.Deviations.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Name, b.Name));
            }
            return result;
        }

        static int[] SlotsOf(int mask, int slotCount)
        {
            var slots = new List<int>();
            for (int s = 0; s < slotCount; s++) if ((mask & (1 << s)) != 0) slots.Add(s);
            return slots.ToArray();
        }

        static SeriesComparison CompareSeriesInBanks(string name, int[] slots, IList<CompareBank> banks, CompareFreq freq, IEqualityCriterion criterion)
        {
            var series = new CompareSeries[slots.Length];
            int start = int.MaxValue, end = int.MinValue;
            for (int i = 0; i < slots.Length; i++)
            {
                series[i] = banks[slots[i]].Get(freq)[name];
                if (series[i].Data.Length == 0) continue;
                start = Math.Min(start, series[i].Start);
                end = Math.Max(end, series[i].End);
            }

            var c = new SeriesComparison { Name = series[0].Name, Slots = slots, Series = series };
            if (start > end)
            {
                c.Start = 0;
                c.End = -1;
                c.Deviates = new bool[0];
                return c;
            }

            c.Start = start;
            c.End = end;
            c.Deviates = new bool[end - start + 1];
            var values = new double?[series.Length];
            for (int p = start; p <= end; p++)
            {
                for (int i = 0; i < series.Length; i++) values[i] = series[i].Get(p);
                if (!AllEqual(values, criterion))
                {
                    c.Deviates[p - start] = true;
                    c.DeviatingPeriods++;
                }
            }
            return c;
        }

        /// <summary>For 2 banks: equal. For 3 banks: all three pairwise equal (area A).</summary>
        public static bool AllEqual(double?[] values, IEqualityCriterion criterion)
        {
            for (int i = 0; i < values.Length; i++)
                for (int j = i + 1; j < values.Length; j++)
                    if (!criterion.AreEqual(values[i], values[j])) return false;
            return true;
        }

        /// <summary>Number of deviating series per period, over the universal period.</summary>
        public static int[] Histogram(VennArea area, int spanStart, int spanEnd)
        {
            var counts = new int[Math.Max(0, spanEnd - spanStart + 1)];
            foreach (SeriesComparison c in area.Deviations)
            {
                for (int p = c.Start; p <= c.End; p++)
                {
                    if (!c.Deviates[p - c.Start]) continue;
                    int i = p - spanStart;
                    if (i >= 0 && i < counts.Length) counts[i]++;
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
            if (hidden.Count > 0) sb.Append(" (bank ").Append(JoinWords(hidden, "and")).Append(" hidden)");
            return sb.ToString();
        }

        static string JoinWords(List<int> numbers, string conjunction)
        {
            if (numbers.Count == 1) return numbers[0].ToString(Inv);
            return string.Join(", ", numbers.Take(numbers.Count - 1).Select(x => x.ToString(Inv)))
                + " " + conjunction + " " + numbers[numbers.Count - 1].ToString(Inv);
        }

        /// <summary>
        /// Text listing of one series. Two banks: period, x(1), x(2), abs dev., rel. dev.
        /// Three banks (area A): period, x(1), x(2), x(3). Deviating periods end with " *".
        /// </summary>
        public static DeviationListing FormatComparison(SeriesComparison c, CompareFreq freq, bool onlyDeviating)
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

            var values = new double?[n];
            for (int p = c.Start; p <= c.End; p++)
            {
                bool deviates = c.Deviates[p - c.Start];
                if (onlyDeviating && !deviates) continue;
                for (int i = 0; i < n; i++) values[i] = c.Series[i].Get(p);

                sb.Clear();
                sb.Append(PeriodText.Format(freq, p).PadRight(periodWidth));
                for (int i = 0; i < n; i++) sb.Append(FormatValue(values[i]).PadLeft(valueWidth));
                if (pair)
                {
                    double a, b;
                    if (TryFinite(values[0], out a) && TryFinite(values[1], out b))
                    {
                        double d = b - a;
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

        static bool TryFinite(double? v, out double x)
        {
            x = 0;
            if (!v.HasValue) return false;
            x = v.Value;
            return !double.IsNaN(x) && !double.IsInfinity(x);
        }

        /// <summary>"M" means no observation in the period.</summary>
        public static string FormatValue(double? v)
        {
            if (!v.HasValue) return "M";
            double x = v.Value;
            if (double.IsNaN(x)) return "NaN";
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
