using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gekko
{
    /// <summary>A drill-down filter on the "found" flag: no filter, found, or not found.</summary>
    public enum FlagFilter { Any, Yes, No }

    /// <summary>
    /// The three independent checks (the tabs at the bottom of the controls panel). Each check marks some
    /// series-periods as "found"; the red numbers, the histogram, the red names and the * in the values all
    /// follow the current check.
    ///   Differences: the area's banks do not all have the same value (beyond the threshold). M = M is equal.
    ///   Missings:    the value is M (double.NaN) in at least one of the banks.
    ///   Zeroes:      the value is zero (|value| &lt;= threshold) in at least one of the banks.
    /// </summary>
    public enum CheckKind { Differences, Missings, Zeroes }

    /// <summary>The thresholds of the checks.</summary>
    public sealed class CheckSettings
    {
        /// <summary>Differences: two numbers are equal when they differ by at most this much.</summary>
        public double DiffThreshold { get; set; }

        /// <summary>Differences: the threshold is in percent of the larger absolute value instead of absolute.</summary>
        public bool DiffRelative { get; set; }

        /// <summary>Zeroes: a value counts as zero when its absolute value is at most this.</summary>
        public double ZeroThreshold { get; set; }

        /// <summary>M = M counts as equal; M and a number are not equal; two numbers use the threshold.</summary>
        public bool AreEqual(double a, double b)
        {
            bool ma = double.IsNaN(a), mb = double.IsNaN(b);
            if (ma || mb) return ma && mb;
            if (a == b) return true;  // also equal infinities
            double d = Math.Abs(a - b);
            if (double.IsInfinity(d) || double.IsNaN(d)) return false;
            if (!DiffRelative) return d <= DiffThreshold;
            double scale = Math.Max(Math.Abs(a), Math.Abs(b));  // > 0, since a != b
            return d / scale * 100.0 <= DiffThreshold;
        }

        public bool IsZero(double v)
        {
            return !double.IsNaN(v) && Math.Abs(v) <= ZeroThreshold;
        }
    }

    /// <summary>Wording for the checks, so that all views say the same.</summary>
    public static class CheckText
    {
        /// <summary>"differ", "have missing values", "have zeroes".</summary>
        public static string Verb(CheckKind k)
        {
            switch (k)
            {
                case CheckKind.Missings: return "have missing values";
                case CheckKind.Zeroes: return "have zeroes";
                default: return "differ";
            }
        }

        /// <summary>"that differ", "with missing values", "with zeroes".</summary>
        public static string Phrase(CheckKind k)
        {
            switch (k)
            {
                case CheckKind.Missings: return "with missing values";
                case CheckKind.Zeroes: return "with zeroes";
                default: return "that differ";
            }
        }
    }

    /// <summary>
    /// Period texts in any frequency, as typed in the From/To boxes: "1990" (annual), "1990q3", "1990m7", or anything
    /// PeriodText.TryParse accepts for the frequency at hand. A simple stand-in for Gekko's own conversion between
    /// frequencies; replace the body of TryConvert with that if wanted:
    ///   to a lower frequency:  1990q3 becomes 1990, 1990m8 becomes 1990q3;
    ///   to a higher frequency: 1990 becomes 1990q1 as the first period and 1990q4 as the last,
    ///                          1990q3 becomes 1990m7 as the first period and 1990m9 as the last.
    /// </summary>
    public static class PeriodConvert
    {
        static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        /// <summary>Splits "1990" (letter '\0'), "1990q3" ('q') or "1990m7" ('m'). Case-insensitive.</summary>
        public static bool TrySplit(string text, out int year, out char letter, out int sub)
        {
            year = 0;
            letter = '\0';
            sub = 1;
            text = (text ?? "").Trim();
            int i = 0;
            while (i < text.Length && text[i] >= '0' && text[i] <= '9') i++;
            if (i == 0 || i > 5) return false;
            year = int.Parse(text.Substring(0, i), NumberStyles.None, Inv);
            if (i == text.Length) return true;

            char c = char.ToLowerInvariant(text[i]);
            if (c != 'q' && c != 'm') return false;
            string rest = text.Substring(i + 1);
            if (rest.Length == 0 || rest.Length > 2 || rest.Any(ch => ch < '0' || ch > '9')) return false;
            int s = int.Parse(rest, NumberStyles.None, Inv);
            if (s < 1 || s > PerYear(c)) return false;
            letter = c;
            sub = s;
            return true;
        }

        static int PerYear(char letter)
        {
            return letter == 'q' ? 4 : letter == 'm' ? 12 : 1;
        }

        static string Join(int year, char letter, int sub)
        {
            string y = year.ToString(Inv);
            return letter == '\0' ? y : y + letter + sub.ToString(Inv);
        }

        /// <summary>The first (isEnd false) or last (isEnd true) period of freq that the text covers.</summary>
        public static bool TryConvert(string text, CompareFreq freq, bool isEnd, out int period)
        {
            int year, sub;
            char letter;
            if (TrySplit(text, out year, out letter, out sub))
            {
                int target = PeriodText.PeriodsPerYear(freq);
                int source = PerYear(letter);
                if (target == 1 || target == 4 || target == 12)
                {
                    int t = isEnd ? (sub * target + source - 1) / source : (sub - 1) * target / source + 1;
                    period = PeriodText.ToIndex(freq, year, t);
                    return true;
                }
            }
            return PeriodText.TryParse((text ?? "").Trim(), freq, isEnd, out period);
        }

        /// <summary>One period later or earlier in the text's own frequency: 1990 -> 1991, 1990q4 -> 1991q1.</summary>
        public static bool TryStep(string text, int delta, out string result)
        {
            result = null;
            int year, sub;
            char letter;
            if (!TrySplit(text, out year, out letter, out sub)) return false;
            int per = PerYear(letter);
            int index = year * per + (sub - 1) + delta;
            if (index < 0) return false;
            result = Join(index / per, letter, index % per + 1);
            return true;
        }

        /// <summary>The text's first (or last) month counted from year 0, to check that From is not after To.</summary>
        public static bool TryMonth(string text, bool isEnd, out int month)
        {
            month = 0;
            int year, sub;
            char letter;
            if (!TrySplit(text, out year, out letter, out sub)) return false;
            int per = PerYear(letter);
            month = year * 12 + (isEnd ? sub * 12 / per - 1 : (sub - 1) * 12 / per);
            return true;
        }

        /// <summary>
        /// False for undated series: they have no calendar, so the period filter does not apply to them (they keep all
        /// their periods). Adjust if Gekko names the undated frequency differently.
        /// </summary>
        public static bool IsDated(CompareFreq freq)
        {
            return FrequencyName(freq) != "Undated";
        }

        /// <summary>"Annual", "Quarterly", "Monthly", ... for the frequency buttons.</summary>
        public static string FrequencyName(CompareFreq freq)
        {
            string s = freq.ToString();
            switch (s.ToLowerInvariant())
            {
                case "a": case "annual": return "Annual";
                case "q": case "quarterly": return "Quarterly";
                case "m": case "monthly": return "Monthly";
                case "w": case "weekly": return "Weekly";
                case "d": case "daily": return "Daily";
                case "u": case "undated": return "Undated";
                default: return s.Length > 0 ? char.ToUpperInvariant(s[0]) + s.Substring(1) : s;
            }
        }
    }

    /// <summary>
    /// The period filter (the From/To boxes, shared by both window states): two period texts in any frequency, null
    /// when open. Each frequency gets its own first and last period (PeriodConvert), so one filter serves them all.
    /// Use on the UI thread (the conversions are remembered per frequency).
    /// </summary>
    public sealed class PeriodRange
    {
        public static readonly PeriodRange All = new PeriodRange(null, null);
        readonly Dictionary<CompareFreq, int[]> bounds = new Dictionary<CompareFreq, int[]>();

        public PeriodRange(string fromText, string toText)
        {
            FromText = string.IsNullOrWhiteSpace(fromText) ? null : fromText.Trim();
            ToText = string.IsNullOrWhiteSpace(toText) ? null : toText.Trim();
        }

        public string FromText { get; }
        public string ToText { get; }
        public bool IsOpen => FromText == null && ToText == null;

        /// <summary>The first period of freq in the filter (int.MinValue when open, or when the text does not convert).</summary>
        public int From(CompareFreq freq)
        {
            return Bounds(freq)[0];
        }

        /// <summary>The last period of freq in the filter (int.MaxValue when open, or when the text does not convert).</summary>
        public int To(CompareFreq freq)
        {
            return Bounds(freq)[1];
        }

        int[] Bounds(CompareFreq freq)
        {
            int[] b;
            if (bounds.TryGetValue(freq, out b)) return b;
            int from = int.MinValue, to = int.MaxValue;
            if (PeriodConvert.IsDated(freq))  // undated series keep all their periods
            {
                if (FromText == null || !PeriodConvert.TryConvert(FromText, freq, false, out from)) from = int.MinValue;
                if (ToText == null || !PeriodConvert.TryConvert(ToText, freq, true, out to)) to = int.MaxValue;
            }
            bounds[freq] = b = new[] { from, to };
            return b;
        }

        /// <summary>" in 2000–2010", " from 2000", " until 2010", or "" when open.</summary>
        public string Suffix
        {
            get
            {
                if (FromText != null && ToText != null)
                    return " in " + (string.Equals(FromText, ToText, StringComparison.OrdinalIgnoreCase) ? FromText : FromText + "–" + ToText);
                if (FromText != null) return " from " + FromText;
                if (ToText != null) return " until " + ToText;
                return "";
            }
        }
    }

    /// <summary>Counts for one Venn area or one whole bank/folder.</summary>
    public sealed class AreaCounts
    {
        public int Total;  // all items (black number)
        public int Found;  // items found by the current check (red number)
    }

    /// <summary>
    /// What the Venn diagram shows, for banks (series inside) or folders (files inside). The diagram, its
    /// numbers, captions and boxes only use this, so the same VennView serves both window states.
    /// </summary>
    public interface IVennModel
    {
        int SlotCount { get; }
        bool[] Active { get; }
        int ActiveCount { get; }
        IEnumerable<int> AreaMasks { get; }
        /// <summary>Whether the area gets a red number (e.g. not for one-bank areas when looking for differences).</summary>
        bool ShowsFound(int mask);
        AreaCounts AreaCountsOf(int mask);
        string SlotWord { get; }      // "bank" / "folder"
        string ItemWord { get; }      // "series" / "files"
        string FoundPhrase { get; }   // "that differ", "with missing values", ...
        string SlotName(int slot);    // box, first line (shortened to 25 characters by the view)
        string SlotInfo(int slot);    // box, the lines under the name, separated by "\n"
        string SlotTip(int slot);     // box tooltip (full path)
        string FoundText(AreaCounts c);   // tooltip of a red number
        string Summarize(int mask);       // caption when hovering an area
        string SummarizeWhole(int slot);  // caption when hovering a box
    }

    /// <summary>
    /// One series in a Venn area, compared across the area's banks over the whole universal period.
    ///
    /// M means double.NaN, nothing else. A period outside a series' stored range is also M.
    ///   Deviates (per period): the area's values are not all equal (CheckSettings.AreEqual).
    ///   Missing  (per bank):   the bank's value in the period is M.
    ///   Zero     (per bank):   the bank's value in the period is zero (CheckSettings.IsZero).
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

        /// <summary>[index into Slots][period - Start]: true when that bank's value is zero.</summary>
        public bool[][] Zero { get; set; }

        // Prefix sums, so that "found anywhere in from..to" is O(1) per bank.
        int[] devCum;
        int[][] missCum;
        int[][] zeroCum;

        internal void BuildPrefixSums()
        {
            devCum = Cumulate(Deviates);
            missCum = Missing.Select(Cumulate).ToArray();
            zeroCum = Zero.Select(Cumulate).ToArray();
        }

        static int[] Cumulate(bool[] flags)
        {
            var cum = new int[flags.Length + 1];
            for (int i = 0; i < flags.Length; i++) cum[i + 1] = cum[i] + (flags[i] ? 1 : 0);
            return cum;
        }

        bool Clamp(int from, int to, out int lo, out int hi)
        {
            lo = Math.Max(from, Start) - Start;
            hi = Math.Min(to, End) - Start;
            return lo <= hi && Deviates.Length > 0;
        }

        bool[][] PerBank(CheckKind kind)
        {
            return kind == CheckKind.Zeroes ? Zero : Missing;
        }

        /// <summary>The value of the series in bank Slots[index], M (NaN) outside its stored range.</summary>
        public double Value(int index, int period)
        {
            double? v = Series[index].Get(period);
            return v.HasValue ? v.Value : double.NaN;
        }

        /// <summary>
        /// Found by the check in this period. For Missings and Zeroes only the banks in slotMask count
        /// (bit s = slot s): the area's banks, or one bank for "All of bank x".
        /// </summary>
        public bool FoundAt(CheckKind kind, int period, int slotMask)
        {
            int i = period - Start;
            if (i < 0 || i >= Deviates.Length) return false;
            if (kind == CheckKind.Differences) return Deviates[i];
            bool[][] flags = PerBank(kind);
            for (int s = 0; s < Slots.Length; s++)
                if ((slotMask & (1 << Slots[s])) != 0 && flags[s][i]) return true;
            return false;
        }

        /// <summary>Number of periods in from..to where the check finds something.</summary>
        public int FoundPeriods(CheckKind kind, int from, int to, int slotMask)
        {
            int lo, hi;
            if (!Clamp(from, to, out lo, out hi)) return 0;
            if (kind == CheckKind.Differences) return devCum[hi + 1] - devCum[lo];
            int count = 0;
            for (int i = lo; i <= hi; i++)
                if (FoundAt(kind, Start + i, slotMask)) count++;
            return count;
        }

        /// <summary>Whether the check finds something in at least one period in from..to.</summary>
        public bool FoundIn(CheckKind kind, int from, int to, int slotMask)
        {
            int lo, hi;
            if (!Clamp(from, to, out lo, out hi)) return false;
            if (kind == CheckKind.Differences) return devCum[hi + 1] - devCum[lo] > 0;
            int[][] cum = kind == CheckKind.Zeroes ? zeroCum : missCum;
            for (int s = 0; s < Slots.Length; s++)
                if ((slotMask & (1 << Slots[s])) != 0 && cum[s][hi + 1] - cum[s][lo] > 0) return true;
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

    /// <summary>The comparison of 2 or 3 databanks (the "Datafiles" state).</summary>
    public sealed class VennResult : IVennModel
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

        /// <summary>Set by CompareEngine.UpdateCounts for the current check and period filter.</summary>
        public CheckKind Check { get; set; }
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
                if (CountFrom > CountTo) return " in the chosen period (no " + PeriodConvert.FrequencyName(Freq).ToLowerInvariant() + " data there)";
                return " in " + PeriodText.Format(Freq, CountFrom) + (CountFrom == CountTo ? "" : "–" + PeriodText.Format(Freq, CountTo));
            }
        }

        public string SpanText
        {
            get { return HasSpan ? PeriodText.Format(Freq, SpanStart) + "–" + PeriodText.Format(Freq, SpanEnd) : "no observations"; }
        }

        /// <summary>For the box: the bank's span and its number of series, on two lines ("1990–2025\n260 series").</summary>
        public string BankSpanText(int slot)
        {
            string freq = PeriodConvert.FrequencyName(Freq).ToLowerInvariant();
            if (BankSeriesCount[slot] == 0) return "no " + freq + " series";
            string count = BankSeriesCount[slot].ToString(CultureInfo.InvariantCulture) + " series";
            if (BankSpanStart[slot] > BankSpanEnd[slot]) return count;
            return PeriodText.Format(Freq, BankSpanStart[slot]) + "–" + PeriodText.Format(Freq, BankSpanEnd[slot]) + "\n" + count;
        }

        // ---- IVennModel

        public IEnumerable<int> AreaMasks => Areas.Keys;

        public bool ShowsFound(int mask)
        {
            VennArea area;
            return Areas.TryGetValue(mask, out area) && (Check != CheckKind.Differences || area.IsComparison);
        }

        public AreaCounts AreaCountsOf(int mask)
        {
            AreaCounts c;
            if (Counts.TryGetValue(mask, out c)) return c;
            VennArea area;
            return new AreaCounts { Total = Areas.TryGetValue(mask, out area) ? area.Series.Count : 0 };
        }

        public string SlotWord => "bank";
        public string ItemWord => "series";
        public string FoundPhrase => "series " + CheckText.Phrase(Check);
        public string SlotName(int slot) => Banks[slot].Name;
        public string SlotInfo(int slot) => BankSpanText(slot);
        public string SlotTip(int slot) => Banks[slot].FilePath ?? (Banks[slot].Name + " (artificial data)");

        public string FoundText(AreaCounts c)
        {
            return c.Found + " of " + c.Total + " series " + CheckText.Verb(Check) + CountPeriodText;
        }

        public string Summarize(int mask)
        {
            return CompareEngine.DescribeArea(SlotCount, Active, mask, "bank") + ": " + DescribeCounts(AreaCountsOf(mask), ShowsFound(mask));
        }

        public string SummarizeWhole(int slot)
        {
            AreaCounts c = BankCounts != null && BankCounts[slot] != null ? BankCounts[slot] : new AreaCounts { Total = BankSeriesCount[slot] };
            bool shows = Check != CheckKind.Differences || ActiveCount > 1;
            return "All of bank " + (slot + 1) + " (" + Banks[slot].Name + "): " + DescribeCounts(c, shows);
        }

        /// <summary>E.g. "100 series, 17 that differ in 2000".</summary>
        public string DescribeCounts(AreaCounts c, bool showsFound)
        {
            string text = c.Total.ToString(CultureInfo.InvariantCulture) + " series";
            if (showsFound) text += ", " + c.Found.ToString(CultureInfo.InvariantCulture) + " " + CheckText.Phrase(Check);
            return text + CountPeriodText;
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
        /// All three checks are computed for the whole universal period; the current check and the period
        /// filter are applied later (UpdateCounts and the window), so switching them needs no recomputation.
        /// Changing a threshold does.
        /// </summary>
        public static VennResult Compute(IList<CompareBank> banks, bool[] active, CompareFreq freq, Func<string, bool> filter,
                                         CheckSettings settings)
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
                area.Series.Add(CompareSeriesInBanks(kv.Key, area.Slots, banks, freq, settings, result.SpanStart, result.SpanEnd));
            }

            foreach (VennArea area in result.Areas.Values)
                area.Series.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Name, b.Name));

            UpdateCounts(result, CheckKind.Differences, result.SpanStart, result.SpanEnd);
            return result;
        }

        public static int[] SlotsOf(int mask, int slotCount)
        {
            var slots = new List<int>();
            for (int s = 0; s < slotCount; s++) if ((mask & (1 << s)) != 0) slots.Add(s);
            return slots.ToArray();
        }

        static SeriesComparison CompareSeriesInBanks(string name, int[] slots, IList<CompareBank> banks, CompareFreq freq,
                                                     CheckSettings settings, int start, int end)
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
                Missing = new bool[slots.Length][],
                Zero = new bool[slots.Length][]
            };
            for (int i = 0; i < slots.Length; i++)
            {
                c.Missing[i] = new bool[length];
                c.Zero[i] = new bool[length];
            }

            var values = new double[slots.Length];
            for (int k = 0; k < length; k++)
            {
                int p = start + k;
                for (int i = 0; i < slots.Length; i++)
                {
                    values[i] = c.Value(i, p);
                    c.Missing[i][k] = double.IsNaN(values[i]);
                    c.Zero[i][k] = settings.IsZero(values[i]);
                }
                if (slots.Length >= 2) c.Deviates[k] = !AllEqual(values, settings);
            }
            c.BuildPrefixSums();
            return c;
        }

        /// <summary>For 2 banks: equal. For 3 banks: all three pairwise equal.</summary>
        public static bool AllEqual(double[] values, CheckSettings settings)
        {
            for (int i = 0; i < values.Length; i++)
                for (int j = i + 1; j < values.Length; j++)
                    if (!settings.AreEqual(values[i], values[j])) return false;
            return true;
        }

        /// <summary>Counts per area and per whole bank, for the check and the period filter from..to.</summary>
        public static void UpdateCounts(VennResult r, CheckKind check, int from, int to)
        {
            r.Check = check;
            r.CountFrom = from;
            r.CountTo = to;
            r.Counts.Clear();
            foreach (VennArea area in r.Areas.Values)
                r.Counts[area.Mask] = Count(area.Series, check, area.Mask, from, to);
            for (int s = 0; s < r.SlotCount; s++)
            {
                IEnumerable<SeriesComparison> all = r.Areas.Values.Where(a => (a.Mask & (1 << s)) != 0).SelectMany(a => a.Series);
                r.BankCounts[s] = r.Active[s] ? Count(all, check, 1 << s, from, to) : new AreaCounts();
            }
        }

        static AreaCounts Count(IEnumerable<SeriesComparison> series, CheckKind check, int slotMask, int from, int to)
        {
            var c = new AreaCounts();
            foreach (SeriesComparison s in series)
            {
                c.Total++;
                if (s.FoundIn(check, from, to, slotMask)) c.Found++;
            }
            return c;
        }

        /// <summary>Per period start..end: the number of the series found by the check (in the banks in slotMask).</summary>
        public static int[] FoundCounts(IEnumerable<SeriesComparison> series, CheckKind check, int slotMask, int start, int end)
        {
            var counts = new int[Math.Max(0, end - start + 1)];
            foreach (SeriesComparison c in series)
                for (int p = start; p <= end; p++)
                    if (c.FoundAt(check, p, slotMask)) counts[p - start]++;
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

        /// <summary>Plain-language description of an area, e.g. "In banks 1 and 2, not in bank 3" (word: bank/folder).</summary>
        public static string DescribeArea(int slotCount, bool[] active, int mask, string word)
        {
            var inside = new List<int>();
            var outside = new List<int>();
            var hidden = new List<int>();
            for (int s = 0; s < slotCount; s++)
            {
                if (!active[s]) hidden.Add(s + 1);
                else if ((mask & (1 << s)) != 0) inside.Add(s + 1);
                else outside.Add(s + 1);
            }

            var sb = new StringBuilder();
            if (inside.Count == 1)
            {
                sb.Append(outside.Count > 0 ? "Only in " + word + " " : "In " + word + " ").Append(inside[0]);
            }
            else
            {
                sb.Append("In " + word + "s ").Append(JoinWords(inside, "and"));
                if (outside.Count > 0) sb.Append(", not in " + word + " ").Append(JoinWords(outside, "or"));
            }
            if (hidden.Count > 0) sb.Append(hidden.Count == 1 ? " (" + word + " " : " (" + word + "s ").Append(JoinWords(hidden, "and")).Append(" hidden)");
            return sb.ToString();
        }

        public static string DescribeArea(VennResult r, int mask)
        {
            return DescribeArea(r.SlotCount, r.Active, mask, "bank");
        }

        static string JoinWords(List<int> numbers, string conjunction)
        {
            if (numbers.Count == 1) return numbers[0].ToString(Inv);
            return string.Join(", ", numbers.Take(numbers.Count - 1).Select(x => x.ToString(Inv)))
                + " " + conjunction + " " + numbers[numbers.Count - 1].ToString(Inv);
        }

        /// <summary>
        /// Text listing of one series for the periods from..to. Two banks: period, x(1), x(2), abs dev., rel. dev.
        /// One or three banks: period and the values. Periods found by the check end with " *".
        /// onlyFound: only those periods.
        /// </summary>
        public static DeviationListing FormatComparison(SeriesComparison c, CompareFreq freq, int from, int to,
                                                        CheckKind check, int slotMask, bool onlyFound)
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

            var values = new double[n];
            for (int p = Math.Max(from, c.Start); p <= Math.Min(to, c.End); p++)
            {
                bool found = c.FoundAt(check, p, slotMask);
                if (onlyFound && !found) continue;
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
                if (found) sb.Append("  *");
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

        /// <summary>Parses a threshold typed by the user (invariant or current culture). Must be a number &gt;= 0.</summary>
        public static bool TryParseThreshold(string text, out double value)
        {
            text = (text ?? "").Trim();
            if (text.Length == 0)
            {
                value = 0;
                return true;
            }
            if (!double.TryParse(text, NumberStyles.Float, Inv, out value) &&
                !double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value)) return false;
            return value >= 0 && !double.IsNaN(value) && !double.IsInfinity(value);
        }
    }
}
