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

    /// <summary>
    /// The thresholds of the checks.
    ///
    /// Differences has two criteria, each with its own box: an absolute and a relative (%) threshold. Two values differ
    /// when their difference exceeds the threshold of each criterion that is on: with both on, it must exceed both
    /// (so e.g. absolute 0.0001 ignores tiny differences that are large in percent, while relative still applies).
    /// With neither on, any difference counts. A value against M always differs; M against M is equal.
    /// </summary>
    public sealed class CheckSettings
    {
        /// <summary>Differences, the absolute criterion: on/off and its threshold.</summary>
        public bool DiffAbsOn { get; set; } = true;
        public double DiffAbsThreshold { get; set; }

        /// <summary>Differences, the relative criterion: on/off and its threshold in percent.</summary>
        public bool DiffRelOn { get; set; }
        public double DiffRelThreshold { get; set; }

        /// <summary>Zeroes: a value counts as zero when its absolute value is at most this.</summary>
        public double ZeroThreshold { get; set; }

        /// <summary>
        /// The difference between x1 and x0 as the absolute and the relative (%) difference, signs dropped, by
        /// Program.CompareTwoValues (x1 compared to x0). This is the one rule used everywhere (both states, sorting):
        ///   M against M, or equal values: 0 and 0.
        ///   M against a number: +infinity and +infinity.
        ///   A difference that cannot be computed (e.g. % against 0): +infinity.
        /// </summary>
        public static void Measure(double x1, double x0, out double abs, out double rel)
        {
            //abs = 0;
            //rel = 0;
            //bool m1 = double.IsNaN(x1), m0 = double.IsNaN(x0);
            //if (m1 && m0) return;
            //if (m1 || m0)
            //{
            //    abs = rel = double.PositiveInfinity;
            //    return;
            //}
            //if (x1 == x0) return;  // also equal infinities
            double varDelta, varPch;
            Program.CompareTwoValues(out varDelta, out varPch, false, x1, x0);
            abs = Math.Abs(varDelta);
            rel = Math.Abs(varPch);
            if (double.IsNaN(abs)) abs = double.PositiveInfinity;
            if (double.IsNaN(rel)) rel = double.PositiveInfinity;
        }

        /// <summary>Whether a difference (as Measure gives it) counts, by the criteria that are on (see the class).</summary>
        public bool Exceeds(double abs, double rel)
        {
            bool a = abs > DiffAbsThreshold;
            bool r = rel > DiffRelThreshold;
            if (DiffAbsOn && DiffRelOn) return a && r;
            if (DiffAbsOn) return a;
            if (DiffRelOn) return r;
            return abs > 0 || rel > 0;  // no criterion: any difference
        }

        /// <summary>Equal for the Differences check: the difference does not count (see the class).</summary>
        public bool AreEqual(double x1, double x0)
        {
            double abs, rel;
            Measure(x1, x0, out abs, out rel);
            if (abs == 0 && rel == 0) return true;
            return !Exceeds(abs, rel);
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

    /// <summary>Text lines for the value listing (header, rule, one line per period), plus which line shows which period (by offset on the axis).</summary>
    public sealed class DeviationListing
    {
        public List<string> Lines { get; } = new List<string>();
        public Dictionary<int, int> LineOfPeriod { get; } = new Dictionary<int, int>();
    }

    /// <summary>
    /// The layout of the Values tab (see CompareEngine.FormatComparison), in one place: for each column type its width
    /// and the padding (spaces) to the next column. Numbers are right-aligned in their width. The first column (the
    /// series name over the periods) is left-aligned; its width is computed (FirstColumnWidth): the longest name or
    /// period shown, plus NamePad. With the defaults the text looks like this:
    ///
    ///     x123            (1)            (2)             ABS DIFF      % DIFF
    ///     -------------------------------------------------------------------
    ///     2003         6.0000         5.0000               1.0000       20.00         +++
    /// </summary>
    public static class ValuesLayout
    {
        /// <summary>A column type: its width, and the spaces after it when another column follows.</summary>
        public sealed class Column
        {
            public Column(int width, int pad)
            {
                Width = width;
                Pad = pad;
            }

            public int Width { get; set; }
            public int Pad { get; set; }
        }

        /// <summary>The first column: spaces after the longest name or period.</summary>
        public static int NamePad = 2;

        /// <summary>The values of the banks: (1), (2), (3).</summary>
        public static readonly Column Level = new Column(13, 2);

        /// <summary>ABS DIFF (two banks).</summary>
        public static readonly Column Abs = new Column(19, 2);

        /// <summary>% DIFF (two banks).</summary>
        public static readonly Column Pct = new Column(10, 2);

        /// <summary>The mark of a period found by the check, and the spaces between the last column and it.</summary>
        public const string Mark = "+++";
        public static int MarkGap = 9;

        /// <summary>The line under the header.</summary>
        public const char RuleChar = '-';

        /// <summary>The width of the first column, for these names (as shown) and periods (as text).</summary>
        public static int FirstColumnWidth(IEnumerable<string> names, IEnumerable<string> periods)
        {
            int w = 0;
            foreach (string s in names) if (s != null) w = Math.Max(w, s.Length);
            foreach (string s in periods) if (s != null) w = Math.Max(w, s.Length);
            return w + NamePad;
        }

        /// <summary>
        /// Appends text right-aligned in the column (trimmed first, so a formatter that pads on its own does not
        /// shift it), then the column's padding unless it is the last column of the line.
        /// </summary>
        public static void Append(StringBuilder sb, string text, Column column, bool lastColumn)
        {
            sb.Append((text ?? "").Trim().PadLeft(column.Width));
            if (!lastColumn) sb.Append(' ', column.Pad);
        }
    }

    /// <summary>
    /// The filter box, for series names or data files.
    ///   Wildcard mode: patterns separated by spaces (or ;), * and ? as usual, the whole name must match. A pattern
    ///   starting with - excludes. A name passes when it matches one of the other patterns (or there are none) and
    ///   none of the excluding ones: "p* q* -pa* -pd*" is all p... and q... names except pa... and pd...; "-x*" is all
    ///   names except x....
    ///   Regex mode: case-insensitive, matches anywhere unless anchored with ^ and $.
    /// </summary>
    public sealed class NameFilter
    {
        Regex regex;
        readonly List<Regex> include = new List<Regex>();
        readonly List<Regex> exclude = new List<Regex>();

        /// <summary>Null when the text is empty (no filter) or invalid (then error is set).</summary>
        public static NameFilter Parse(string text, bool isRegex, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(text)) return null;
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
            var f = new NameFilter();
            if (isRegex)
            {
                try
                {
                    f.regex = new Regex(text.Trim(), options);
                    return f;
                }
                catch (ArgumentException ex)
                {
                    error = "Invalid regular expression: " + ex.Message;
                    return null;
                }
            }
            foreach (string part in text.Split(new[] { ' ', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                bool negative = part.StartsWith("-", StringComparison.Ordinal);
                string pattern = negative ? part.Substring(1) : part;
                if (pattern.Length == 0) continue;  // a lone "-"
                var rx = new Regex("^" + Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", options);
                (negative ? f.exclude : f.include).Add(rx);
            }
            if (f.include.Count == 0 && f.exclude.Count == 0) return null;
            return f;
        }

        /// <summary>
        /// Whether one of the candidates (e.g. a file name and its relative path) matches an including pattern (or
        /// there are none), and none of them matches an excluding one. Regex mode: one of them matches.
        /// </summary>
        public bool Matches(params string[] candidates)
        {
            if (regex != null) return candidates.Any(c => c != null && regex.IsMatch(c));
            bool included = include.Count == 0 || candidates.Any(c => c != null && include.Any(rx => rx.IsMatch(c)));
            if (!included) return false;
            return !candidates.Any(c => c != null && exclude.Any(rx => rx.IsMatch(c)));
        }
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

        static SeriesComparison CompareSeriesInBanks(string name, int[] slots, IList<CompareBank> banks, EFreq freq, CheckSettings settings, PeriodAxis axis)
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
        /// The series filter (Datafiles): the name must match (see NameFilter). Null when there is no filter (or it is
        /// invalid, then error is set).
        /// </summary>
        public static Func<string, bool> BuildFilter(string text, bool isRegex, out string error)
        {
            NameFilter f = NameFilter.Parse(text, isRegex, out error);
            if (f == null) return null;
            return name => f.Matches(name);
        }

        /// <summary>
        /// The data file filter (Folders): tried on the file name and on the path relative to the folder (without the
        /// leading separator), so both "*.gbk" and "sub\*.gbk" work; "-old\*" leaves out a subfolder. The returned
        /// function takes the relative path. Null when there is no filter (or it is invalid, then error is set).
        /// </summary>
        public static Func<string, bool> BuildFileFilter(string text, bool isRegex, out string error)
        {
            NameFilter f = NameFilter.Parse(text, isRegex, out error);
            if (f == null) return null;
            return relativePath =>
            {
                string rel = (relativePath ?? "").TrimStart(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar);
                return f.Matches(System.IO.Path.GetFileName(rel), rel);
            };
        }

        /// <summary>
        /// Turns "x!q" or "x!q[a,b]" into "x" and "x[a,b]", because freqs are selected anyway.
        /// </summary>
        public static string Freqpretty(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            return G.Chop_RemoveFreq(name);
        }

        public static string StripDimensions(string name)
        {
            int i = name.IndexOf('[');
            return i < 0 ? name : name.Substring(0, i);
        }

        /// <summary>
        /// With ignoreDimensions, x[a,b] and x[a,c] both become x (listed once, sorted by name unless keepOrder: then in
        /// the order of their first element, e.g. sorted by deviation).
        /// </summary>
        public static List<string> NamesForDisplay(IEnumerable<string> names, bool ignoreDimensions, bool keepOrder = false)
        {
            if (!ignoreDimensions) return names.ToList();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var list = new List<string>();
            foreach (string name in names)
            {
                string stripped = StripDimensions(name);
                if (seen.Add(stripped)) list.Add(stripped);
            }
            if (!keepOrder) list.Sort(StringComparer.OrdinalIgnoreCase);
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
        /// Text listing of one series for the offsets lo..hi on its axis, laid out by ValuesLayout:
        ///
        ///     x123            (1)            (2)             ABS DIFF      % DIFF
        ///     -------------------------------------------------------------------
        ///     2003         6.0000         5.0000               1.0000       20.00         +++
        ///
        /// A column per bank of the series ((1), (2), (3): the bank numbers); with two banks also ABS DIFF and % DIFF
        /// ((1) compared to (2), as CheckSettings.Measure does it). Periods found by the check end with ValuesLayout.Mark.
        /// onlyFound: only those periods. displayName (may be null: c.Name) heads the first column, which is
        /// firstColumnWidth wide (ValuesLayout.FirstColumnWidth) so that all series of the text line up.
        /// </summary>
        public static DeviationListing FormatComparison(SeriesComparison c, int lo, int hi, CheckKind check, int slotMask,
                                                        bool onlyFound, string displayName, int firstColumnWidth)
        {
            var listing = new DeviationListing();
            int n = c.Series.Length;
            bool pair = n == 2;
            int first = Math.Max(firstColumnWidth, (displayName ?? c.Name).Length + ValuesLayout.NamePad);

            // The header and the rule under it (as long as the columns, without the mark).
            var sb = new StringBuilder();
            sb.Append((displayName ?? c.Name).PadRight(first));
            for (int i = 0; i < n; i++)
                ValuesLayout.Append(sb, "(" + (c.Slots[i] + 1).ToString(Inv) + ")", ValuesLayout.Level, i == n - 1 && !pair);
            if (pair)
            {
                ValuesLayout.Append(sb, "ABS DIFF", ValuesLayout.Abs, false);
                ValuesLayout.Append(sb, "% DIFF", ValuesLayout.Pct, true);
            }
            string header = sb.ToString();
            listing.Lines.Add(header);
            listing.Lines.Add(new string(ValuesLayout.RuleChar, header.Length));

            var values = new double[n];
            int last = Math.Min(hi, c.Deviates.Length - 1);
            for (int k = Math.Max(lo, 0); k <= last; k++)
            {
                bool found = c.FoundAt(check, k, slotMask);
                if (onlyFound && !found) continue;
                for (int i = 0; i < n; i++) values[i] = c.Value(i, k);

                sb.Clear();
                sb.Append(c.Axis.At(k).ToString().PadRight(first));
                for (int i = 0; i < n; i++)
                    ValuesLayout.Append(sb, G.levelFormatOld(values[i], ValuesLayout.Level.Width), ValuesLayout.Level, i == n - 1 && !pair);
                if (pair)
                {
                    // The same direction as the threshold test (CheckSettings.AreEqual(values[0], values[1])):
                    // (1) compared to (2), so 6 against 5 is 1.0000 and 20.00 %.
                    double varDelta, varPch;
                    Program.CompareTwoValues(out varDelta, out varPch, false, values[0], values[1]);
                    ValuesLayout.Append(sb, G.levelFormatOld(varDelta, ValuesLayout.Abs.Width), ValuesLayout.Abs, false);
                    ValuesLayout.Append(sb, G.pchFormatOld(varPch, ValuesLayout.Pct.Width), ValuesLayout.Pct, true);
                }
                if (found) sb.Append(' ', ValuesLayout.MarkGap).Append(ValuesLayout.Mark);
                listing.LineOfPeriod[k] = listing.Lines.Count;
                listing.Lines.Add(sb.ToString());
            }
            return listing;
        }

        /// <summary>
        /// The largest deviation of a series in two banks over the offsets lo..hi, (1) compared to (2), for sorting:
        /// the % difference when the relative criterion is on, else the absolute difference (CheckSettings.Measure).
        /// Only periods that count as differences (Deviates, by the current thresholds) are looked at, so differences
        /// that the thresholds ignore do not move a series up. 0 when there are none; -1 for a series that is not in
        /// exactly two banks (nothing to compare: sorted last).
        /// </summary>
        public static double MaxDeviation(SeriesComparison c, int lo, int hi, CheckSettings settings)
        {
            if (c.Series.Length != 2) return -1;
            double max = 0;
            int last = Math.Min(hi, c.Deviates.Length - 1);
            for (int k = Math.Max(lo, 0); k <= last; k++)
            {
                if (!c.Deviates[k]) continue;
                double abs, rel;
                CheckSettings.Measure(c.Value(0, k), c.Value(1, k), out abs, out rel);
                double d = settings.DiffRelOn ? rel : abs;
                if (d > max) max = d;
                if (double.IsPositiveInfinity(max)) break;
            }
            return max;
        }

        /// <summary>
        /// The series sorted by MaxDeviation in lo..hi, largest first. The sort is stable, so equal deviations keep
        /// their order (by name).
        /// </summary>
        public static List<SeriesComparison> SortByMaxDeviation(IEnumerable<SeriesComparison> series, int lo, int hi, CheckSettings settings)
        {
            return series.Select(c => new { c, d = MaxDeviation(c, lo, hi, settings) })
                         .OrderByDescending(x => x.d)
                         .Select(x => x.c)
                         .ToList();
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
            Name = filePath == null ? "" : System.IO.Path.GetFileName(filePath);
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
            oRead.trace = false; //No need to chew on traces here.
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
