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
    /// Period texts as typed in the From/To boxes ("2010", "2010q3", "2010m7", "2010w12", "2010m7d15"), read with
    /// Gekko's own parser and converted to other frequencies with Gekko's own conversion:
    ///   to a lower frequency:  2010q3 becomes 2010, 2010m8 becomes 2010q3;
    ///   to a higher frequency: 2010 becomes 2010q1 as the first period and 2010q4 as the last.
    /// </summary>
    public static class PeriodConvert
    {
        /// <summary>The text as a GekkoTime in its own frequency. False if it is not a period.</summary>
        public static bool TryParse(string text, out GekkoTime t)
        {
            t = GekkoTime.tNull;
            text = (text ?? "").Trim();
            if (text.Length == 0) return false;
            try
            {
                t = GekkoTime.FromStringToGekkoTime(text, false, false);  // no error message: tNull if not a period
            }
            catch (Exception)
            {
                t = GekkoTime.tNull;
            }
            return !t.IsNull();
        }

        /// <summary>The first period of freq that the text covers (tNull if the text is not a period).</summary>
        public static GekkoTime First(string text, EFreq freq)
        {
            GekkoTime t;
            if (!TryParse(text, out t)) return GekkoTime.tNull;
            try
            {
                return GekkoTime.ConvertFreqsFirst(freq, t, null);
            }
            catch (Exception)
            {
                return GekkoTime.tNull;
            }
        }

        /// <summary>The last period of freq that the text covers (tNull if the text is not a period).</summary>
        public static GekkoTime Last(string text, EFreq freq)
        {
            GekkoTime t;
            if (!TryParse(text, out t)) return GekkoTime.tNull;
            try
            {
                return GekkoTime.ConvertFreqsLast(freq, t);
            }
            catch (Exception)
            {
                return GekkoTime.tNull;
            }
        }

        /// <summary>One period later or earlier in the text's own frequency: 1990 -> 1991, 1990q4 -> 1991q1.</summary>
        public static bool TryStep(string text, int delta, out string result)
        {
            result = null;
            GekkoTime t;
            if (!TryParse(text, out t)) return false;
            try
            {
                result = t.Add(delta).ToString();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>From does not start after To ends (compared in days, so any two frequencies can be compared).</summary>
        public static bool IsOrdered(string from, string to)
        {
            if (from == null || to == null) return true;
            GekkoTime a, b;
            if (!TryParse(from, out a) || !TryParse(to, out b)) return true;
            try
            {
                GekkoTime da = GekkoTime.ConvertFreqsFirst(EFreq.D, a, null);
                GekkoTime db = GekkoTime.ConvertFreqsLast(EFreq.D, b);
                return da.SmallerThanOrEqual(db);
            }
            catch (Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// False for undated series: they have no calendar, so the period filter does not apply to them (they keep all
        /// their periods).
        /// </summary>
        public static bool IsDated(EFreq freq)
        {
            return freq != EFreq.U;
        }

        /// <summary>"Annual", "Quarterly", "Monthly", ... for the frequency buttons.</summary>
        public static string FrequencyName(EFreq freq)
        {
            switch (freq)
            {
                case EFreq.A: return "Annual";
                case EFreq.Q: return "Quarterly";
                case EFreq.M: return "Monthly";
                case EFreq.W: return "Weekly";
                case EFreq.D: return "Daily";
                case EFreq.U: return "Undated";
                default: return freq.ToString();
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
        readonly Dictionary<EFreq, GekkoTime[]> bounds = new Dictionary<EFreq, GekkoTime[]>();

        public PeriodRange(string fromText, string toText)
        {
            FromText = string.IsNullOrWhiteSpace(fromText) ? null : fromText.Trim();
            ToText = string.IsNullOrWhiteSpace(toText) ? null : toText.Trim();
        }

        public string FromText { get; }
        public string ToText { get; }
        public bool IsOpen => FromText == null && ToText == null;

        /// <summary>The first period of freq in the filter (tNull when open, or when the text does not convert).</summary>
        public GekkoTime From(EFreq freq)
        {
            return Bounds(freq)[0];
        }

        /// <summary>The last period of freq in the filter (tNull when open, or when the text does not convert).</summary>
        public GekkoTime To(EFreq freq)
        {
            return Bounds(freq)[1];
        }

        GekkoTime[] Bounds(EFreq freq)
        {
            GekkoTime[] b;
            if (bounds.TryGetValue(freq, out b)) return b;
            GekkoTime from = GekkoTime.tNull, to = GekkoTime.tNull;
            if (PeriodConvert.IsDated(freq))  // undated series keep all their periods
            {
                if (FromText != null) from = PeriodConvert.First(FromText, freq);
                if (ToText != null) to = PeriodConvert.Last(ToText, freq);
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
    /// One series in a Venn area, compared across the area's banks over the whole universal period (Axis).
    /// All arrays are indexed by the offset k = 0..Axis.Length-1; Axis.At(k) is the period.
    ///
    /// M means double.NaN, nothing else. A period outside a series' stored range is also M (Series.GetValue gives NaN).
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
        public Series[] Series { get; set; }

        /// <summary>The universal period of the comparison (shared by all series of a VennResult).</summary>
        public PeriodAxis Axis { get; set; }

        /// <summary>[k]: the values differ in period Axis.At(k).</summary>
        public bool[] Deviates { get; set; }

        /// <summary>[index into Slots][k]: true when that bank's value is M.</summary>
        public bool[][] Missing { get; set; }

        /// <summary>[index into Slots][k]: true when that bank's value is zero.</summary>
        public bool[][] Zero { get; set; }

        // Prefix sums, so that "found anywhere in lo..hi" is O(1) per bank.
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

        /// <summary>Cuts the offsets lo..hi to the arrays. False if nothing is left.</summary>
        bool Clamp(int lo, int hi, out int a, out int b)
        {
            a = Math.Max(lo, 0);
            b = Math.Min(hi, Deviates.Length - 1);
            return a <= b;
        }

        bool[][] PerBank(CheckKind kind)
        {
            return kind == CheckKind.Zeroes ? Zero : Missing;
        }

        /// <summary>The value of the series in bank Slots[index] at offset k, M (NaN) outside its stored range.</summary>
        public double Value(int index, int k)
        {
            return Series[index].GetDataSimple(Axis.At(k));
        }

        /// <summary>
        /// Found by the check at offset k. For Missings and Zeroes only the banks in slotMask count
        /// (bit s = slot s): the area's banks, or one bank for "All of bank x".
        /// </summary>
        public bool FoundAt(CheckKind kind, int k, int slotMask)
        {
            if (k < 0 || k >= Deviates.Length) return false;
            if (kind == CheckKind.Differences) return Deviates[k];
            bool[][] flags = PerBank(kind);
            for (int s = 0; s < Slots.Length; s++)
                if ((slotMask & (1 << Slots[s])) != 0 && flags[s][k]) return true;
            return false;
        }

        /// <summary>Number of periods in the offsets lo..hi where the check finds something.</summary>
        public int FoundPeriods(CheckKind kind, int lo, int hi, int slotMask)
        {
            int a, b;
            if (!Clamp(lo, hi, out a, out b)) return 0;
            if (kind == CheckKind.Differences) return devCum[b + 1] - devCum[a];
            int count = 0;
            for (int k = a; k <= b; k++)
                if (FoundAt(kind, k, slotMask)) count++;
            return count;
        }

        /// <summary>Whether the check finds something in at least one period in the offsets lo..hi.</summary>
        public bool FoundIn(CheckKind kind, int lo, int hi, int slotMask)
        {
            int a, b;
            if (!Clamp(lo, hi, out a, out b)) return false;
            if (kind == CheckKind.Differences) return devCum[b + 1] - devCum[a] > 0;
            int[][] cum = kind == CheckKind.Zeroes ? zeroCum : missCum;
            for (int s = 0; s < Slots.Length; s++)
                if ((slotMask & (1 << Slots[s])) != 0 && cum[s][b + 1] - cum[s][a] > 0) return true;
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
        public EFreq Freq { get; set; }
        public int SlotCount { get; set; }
        public bool[] Active { get; set; }
        public int ActiveMask { get; set; }
        public int ActiveCount => Active.Count(a => a);
        public CompareBank[] Banks { get; set; }
        public Dictionary<int, VennArea> Areas { get; } = new Dictionary<int, VennArea>();

        /// <summary>Universal period: all observations of the selected frequency in the shown banks. Null when there are none.</summary>
        public PeriodAxis Axis { get; set; }
        public bool HasSpan => Axis != null && Axis.Length > 0;

        /// <summary>Per bank: its first and last period with data (tNull when it has none).</summary>
        public GekkoTime[] BankSpanStart { get; set; }
        public GekkoTime[] BankSpanEnd { get; set; }
        public int[] BankSeriesCount { get; set; }

        /// <summary>Set by CompareEngine.UpdateCounts for the current check and period filter.</summary>
        public CheckKind Check { get; set; }
        public Dictionary<int, AreaCounts> Counts { get; } = new Dictionary<int, AreaCounts>();
        public AreaCounts[] BankCounts { get; set; }

        /// <summary>The period filter as offsets on the Axis (CountFrom &gt; CountTo: no periods).</summary>
        public int CountFrom { get; set; }
        public int CountTo { get; set; }

        public bool IsPeriodRestricted => HasSpan && (CountFrom != 0 || CountTo != Axis.Length - 1);

        /// <summary>" in 2000" or " in 2000–2005" when the period filter is set, "" otherwise.</summary>
        public string CountPeriodText
        {
            get
            {
                if (!IsPeriodRestricted) return "";
                if (CountFrom > CountTo) return " in the chosen period (no " + PeriodConvert.FrequencyName(Freq).ToLowerInvariant() + " data there)";
                return " in " + Axis.At(CountFrom) + (CountFrom == CountTo ? "" : "–" + Axis.At(CountTo));
            }
        }

        public string SpanText
        {
            get { return HasSpan ? Axis.Start + "–" + Axis.End : "no observations"; }
        }

        /// <summary>For the box: the bank's span and its number of series, on two lines ("1990–2025\n260 series").</summary>
        public string BankSpanText(int slot)
        {
            string freq = PeriodConvert.FrequencyName(Freq).ToLowerInvariant();
            if (BankSeriesCount[slot] == 0) return "no " + freq + " series";
            string count = BankSeriesCount[slot].ToString(CultureInfo.InvariantCulture) + " series";
            if (!ComparePeriods.IsRange(BankSpanStart[slot], BankSpanEnd[slot])) return count;
            return BankSpanStart[slot] + "–" + BankSpanEnd[slot] + "\n" + count;
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
        public string SlotTip(int slot) => Banks[slot].FilePath ?? Banks[slot].Name;

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

    /// <summary>Text lines for the value listing, plus which line shows which period (by offset on the axis).</summary>
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
        public static VennResult Compute(IList<CompareBank> banks, bool[] active, EFreq freq, Func<string, bool> filter,
                                         CheckSettings settings)
        {
            int n = banks.Count;
            var result = new VennResult
            {
                Freq = freq,
                SlotCount = n,
                Active = active.Take(n).ToArray(),
                Banks = banks.ToArray(),
                BankSpanStart = new GekkoTime[n],
                BankSpanEnd = new GekkoTime[n],
                BankSeriesCount = new int[n],
                BankCounts = new AreaCounts[n]
            };
            for (int s = 0; s < n; s++) if (result.Active[s]) result.ActiveMask |= 1 << s;

            var masks = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            GekkoTime spanStart = GekkoTime.tNull, spanEnd = GekkoTime.tNull;

            for (int s = 0; s < n; s++)
            {
                GekkoTime start = GekkoTime.tNull, end = GekkoTime.tNull;
                int count = 0;
                foreach (KeyValuePair<string, Series> kv in banks[s].Get(freq))
                {
                    if (filter != null && !filter(kv.Key)) continue;
                    count++;
                    if (result.Active[s])
                    {
                        int m;
                        masks.TryGetValue(kv.Key, out m);
                        masks[kv.Key] = m | (1 << s);
                    }
                    ComparePeriods.Widen(ref start, ref end, kv.Value.GetRealDataPeriodFirst(), kv.Value.GetRealDataPeriodLast());
                }
                result.BankSpanStart[s] = start;
                result.BankSpanEnd[s] = end;
                result.BankSeriesCount[s] = count;
                if (result.Active[s]) ComparePeriods.Widen(ref spanStart, ref spanEnd, start, end);
            }
            result.Axis = ComparePeriods.IsRange(spanStart, spanEnd) ? new PeriodAxis(spanStart, spanEnd) : null;

            for (int mask = 1; mask < 8; mask++)
            {
                if ((mask & ~result.ActiveMask) != 0) continue;
                result.Areas[mask] = new VennArea { Mask = mask, Slots = SlotsOf(mask, n) };
            }

            foreach (KeyValuePair<string, int> kv in masks)
            {
                VennArea area = result.Areas[kv.Value];
                area.Series.Add(CompareSeriesInBanks(kv.Key, area.Slots, banks, freq, settings, result.Axis));
            }

            foreach (VennArea area in result.Areas.Values)
                area.Series.Sort((a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Name, b.Name));

            UpdateCounts(result, CheckKind.Differences, 0, result.HasSpan ? result.Axis.Length - 1 : -1);
            return result;
        }

        public static int[] SlotsOf(int mask, int slotCount)
        {
            var slots = new List<int>();
            for (int s = 0; s < slotCount; s++) if ((mask & (1 << s)) != 0) slots.Add(s);
            return slots.ToArray();
        }

        static SeriesComparison CompareSeriesInBanks(string name, int[] slots, IList<CompareBank> banks, EFreq freq,
                                                     CheckSettings settings, PeriodAxis axis)
        {
            var series = new Series[slots.Length];
            for (int i = 0; i < slots.Length; i++) series[i] = banks[slots[i]].Get(freq)[name];

            int length = axis == null ? 0 : axis.Length;
            var c = new SeriesComparison
            {
                Name = name,
                Slots = slots,
                Series = series,
                Axis = axis,
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
                GekkoTime t = axis.At(k);
                for (int i = 0; i < slots.Length; i++)
                {
                    values[i] = series[i].GetDataSimple(t);  // NaN outside the series' stored range
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

        /// <summary>Counts per area and per whole bank, for the check and the period filter (offsets lo..hi on the axis).</summary>
        public static void UpdateCounts(VennResult r, CheckKind check, int lo, int hi)
        {
            r.Check = check;
            r.CountFrom = lo;
            r.CountTo = hi;
            r.Counts.Clear();
            foreach (VennArea area in r.Areas.Values)
                r.Counts[area.Mask] = Count(area.Series, check, area.Mask, lo, hi);
            for (int s = 0; s < r.SlotCount; s++)
            {
                IEnumerable<SeriesComparison> all = r.Areas.Values.Where(a => (a.Mask & (1 << s)) != 0).SelectMany(a => a.Series);
                r.BankCounts[s] = r.Active[s] ? Count(all, check, 1 << s, lo, hi) : new AreaCounts();
            }
        }

        static AreaCounts Count(IEnumerable<SeriesComparison> series, CheckKind check, int slotMask, int lo, int hi)
        {
            var c = new AreaCounts();
            foreach (SeriesComparison s in series)
            {
                c.Total++;
                if (s.FoundIn(check, lo, hi, slotMask)) c.Found++;
            }
            return c;
        }

        /// <summary>Per offset 0..length-1: the number of the series found by the check (in the banks in slotMask).</summary>
        public static int[] FoundCounts(IEnumerable<SeriesComparison> series, CheckKind check, int slotMask, int length)
        {
            var counts = new int[Math.Max(0, length)];
            foreach (SeriesComparison c in series)
                for (int k = 0; k < counts.Length; k++)
                    if (c.FoundAt(check, k, slotMask)) counts[k]++;
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
        /// Text listing of one series for the offsets lo..hi on its axis. Two banks: period, x(1), x(2), abs dev., rel. dev.
        /// One or three banks: period and the values. Periods found by the check end with " *".
        /// onlyFound: only those periods.
        /// </summary>
        public static DeviationListing FormatComparison(SeriesComparison c, int lo, int hi,
                                                        CheckKind check, int slotMask, bool onlyFound)
        {
            var listing = new DeviationListing();
            int n = c.Series.Length;
            bool pair = n == 2;
            var heads = new string[n];
            for (int i = 0; i < n; i++) heads[i] = c.Name + "(" + (c.Slots[i] + 1).ToString(Inv) + ")";

            const int periodWidth = 12, absWidth = 14, relWidth = 12;  // 12: room for daily periods like 2020m12d31
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
            int last = Math.Min(hi, c.Deviates.Length - 1);
            for (int k = Math.Max(lo, 0); k <= last; k++)
            {
                bool found = c.FoundAt(check, k, slotMask);
                if (onlyFound && !found) continue;
                for (int i = 0; i < n; i++) values[i] = c.Value(i, k);

                sb.Clear();
                sb.Append(c.Axis.At(k).ToString().PadRight(periodWidth));
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
                listing.LineOfPeriod[k] = listing.Lines.Count;
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
