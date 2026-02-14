using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
using System.Text.RegularExpressions;

namespace Gekko
{
    public enum ETraceType
    {
        Normal,
        GluedToSeries,
        Divider,
        Dangling  //not used?
    }

    public enum ETraceHelper
    {
        GetAllMetasAndTraces,
        OnlyGetMetas,
        TrimWithTimeShadowing,
        Scramble  //not actually used for traces
    }

    [ProtoContract]
    public class Precedents2
    {
        [ProtoMember(1)]
        public List<Trace2> storage = new List<Trace2>();

        /// <summary>
        /// Pretty innocuous: using this, we can set .storage = null before protobuf.
        /// </summary>
        [ProtoMember(2)]
        public List<TraceID2> storageIDTemporary = null;  //used to recreate connections after protobuf. Will not take up space in general. Same size as .storagePeriodsTemporary

        public void FromID(Dictionary<TraceID2, Trace2> dict2)
        {
            if (this.storageIDTemporary != null && this.storageIDTemporary.Count > 0)
            {
                this.storage = new List<Trace2>();
                for (int i = 0; i < this.storageIDTemporary.Count; i++)
                {
                    TraceID2 id = this.storageIDTemporary[i];
                    if (id.counter < 0) { G.Writeln2("This trace is not stored in the databank, but has been pruned off: " + id.ToString()); throw new GekkoException(); }
                    Trace2 trace = null; dict2.TryGetValue(id, out trace);
                    if (trace == null) { G.Writeln2("Could not find this trace in databank: " + id.ToString()); throw new GekkoException(); }
                    this.storage.Add(trace);
                }
            }
            this.storageIDTemporary = null;
        }

        public void ToID()
        {
            this.storageIDTemporary = new List<TraceID2>();
            //if (this.storage.Count() > 0)
            {
                foreach (Trace2 trace in this.storage)
                {
                    TraceID2 temp = null;
                    GekkoTimeSpansSimple temp2 = new GekkoTimeSpansSimple();  //protobuf cannot handle if an element is == null (for dividers)                    
                    temp = trace.GetId();                    
                    this.storageIDTemporary.Add(temp);
                }
            }
            //this.SetStorage(null);  //breaks the references
            this.storage = null;
        }

    }

    [Serializable]
    [ProtoContract]
    public class Trace2
    {

        [ProtoMember(1)]
        public Precedents2 precedents = new Precedents2();

        [ProtoMember(2)]
        public readonly ETraceType type = ETraceType.Normal;  //default

        [ProtoMember(3)]
        public readonly TraceContents2 traceContents = null;

        public Trace2()
        {
        }

        public Trace2(ETraceType type, GekkoTime t1, GekkoTime t2)
        {
            this.type = type;
            TraceContents2 traceContents = new TraceContents2(t1, t2);
            this.traceContents = traceContents;
        }

        public TraceID2 GetId()
        {
            return this.traceContents.id;
        }

        public static List<TimeSeries> PrecedentsFromGlobals()
        {
            return Globals.traceContainer.GetList().AsEnumerable().Reverse().ToList();
        }

        public static void PrecedentsNames(Trace2 trace, List<TimeSeries> tss)
        {
            if (Globals.traceContainer.Count() > 0)
            {
                trace.traceContents.precedentsNames = new List<string>();
                foreach (TimeSeries ts in tss)
                {
                    trace.traceContents.precedentsNames.Add(ts.GetNameAndParentDatabank());
                }
            }
        }

        public static void PushIntoSeries(Trace2 traceLhs, TimeSeries tsLhs, List<TimeSeries> tsRhss, bool newParent, bool mySelf)
        {
            bool useMySelf = false;
            if (mySelf && newParent && tsRhss.Count == 1 && object.ReferenceEquals(tsRhss[0], tsLhs)) useMySelf = true;

            if (useMySelf)
            {
                if (tsRhss[0].trace2 != null)
                {
                    traceLhs.precedents.storage.AddRange(tsRhss[0].trace2.precedents.storage);
                }
                if (Globals.runningOnTTComputer && tsLhs.trace2.type != ETraceType.GluedToSeries)
                {
                    G.Writeln2("*** ERROR: Glued problem"); throw new GekkoException();
                }
                tsLhs.trace2.precedents.storage.Clear();  //else it will be in two places
                tsLhs.trace2.precedents.storage.Add(traceLhs);
            }
            else
            {

                if (tsLhs.trace2 == null)
                {
                    tsLhs.trace2 = new Trace2(ETraceType.GluedToSeries, Globals.tNull, Globals.tNull);
                }
                else
                {
                    //try { traceLhs.precedents.storage.AddRange(tsLhs.trace2.precedents.storage); } catch { }
                }

                foreach (TimeSeries tsRhs in tsRhss)
                {
                    if (tsRhs.trace2 != null)
                    {
                        traceLhs.precedents.storage.AddRange(tsRhs.trace2.precedents.storage);
                    }
                }
                if (Globals.runningOnTTComputer && tsLhs.trace2.type != ETraceType.GluedToSeries)
                {
                    G.Writeln2("*** ERROR: Glued problem"); throw new GekkoException();
                }

                try { MaybeRemoveShadowedTrace(traceLhs, tsLhs); } catch { }
                tsLhs.trace2.precedents.storage.Add(traceLhs);
            }
        }

        private static void MaybeRemoveShadowedTrace(Trace2 traceLhs, TimeSeries tsLhs)
        {
            GekkoTime t1 = traceLhs.traceContents.period.t1;
            GekkoTime t2 = traceLhs.traceContents.period.t2;
            if (t1.IsNull() || t2.IsNull()) return;
            int hit = -12345;
            for (int i = 0; i < tsLhs.trace2.precedents.storage.Count; i++)
            {
                Trace2 traceExisting = tsLhs.trace2.precedents.storage[i];
                if (!traceExisting.traceContents.period.t1.IsNull() && !traceExisting.traceContents.period.t2.IsNull() && t1.IsSamePeriod(traceExisting.traceContents.period.t1) && t2.IsSamePeriod(traceExisting.traceContents.period.t2))
                {
                    hit = i;
                    break;
                }
                if (traceLhs.traceContents.text == traceExisting.traceContents.text
                    && traceLhs.traceContents.commandFileAndLine == traceExisting.traceContents.commandFileAndLine
                    && GekkoTime.Observations(traceLhs.traceContents.period.t1, traceLhs.traceContents.period.t2) == 1
                    && GekkoTime.Observations(traceExisting.traceContents.period.t1, traceExisting.traceContents.period.t2) == 1
                    && Math.Abs(GekkoTime.Observations(traceLhs.traceContents.period.t1, traceExisting.traceContents.period.t1)-1) == 1 //note: 2020,2019 gives 0, and 2019,2020 gives 2.
                    && traceLhs.traceContents.id.counter - traceExisting.traceContents.id.counter < 1000
                    )
                {
                    hit = i;
                    break;
                }
            }
            if (hit != -12345)
            {
                tsLhs.trace2.precedents.storage.RemoveAt(hit);
            }
        }

        public static void WalkTraces(Trace2 parent, int depth, List<string>traceLines, int type, ref int counter) //0 for viewer, 1 for printing
        {            
            int widthRemember = Program.options.print_width;
            int fileWidthRemember = Program.options.print_filewidth;
            try
            {
                Program.options.print_width = int.MaxValue;
                Program.options.print_filewidth = int.MaxValue;

                //  -----------------------------
                
                string prec = null;
                if (parent.traceContents.precedentsNames != null)
                {
                    List<string> xx = new List<string>(parent.traceContents.precedentsNames);
                    //xx.RemoveAll(s => string.Equals(s, parent.traceContents.name, StringComparison.OrdinalIgnoreCase));
                    xx.Reverse();
                    prec = string.Join(", ", xx);                    
                }
                
                //These must be short
                string name = parent.traceContents.name;
                string period = null;
                if (parent.traceContents.period.t1.IsNull() || parent.traceContents.period.t2.IsNull())
                {
                    period = "<no period>";
                }
                else
                {
                    period = parent.traceContents.period.ToString().Split(' ')[0];
                }
                string code = RemoveNewlines(parent.traceContents.text);
                string file = null;
                string fileDetailed = null;

                if (!G.NullOrBlanks(parent.traceContents.commandFileAndLine))
                {
                    string[] ss = parent.traceContents.commandFileAndLine.Split('¤');
                    if (ss.Length == 2)
                    {
                        file = System.IO.Path.GetFileName(ss[0]) + " line " + ss[1];
                        fileDetailed = ss[0] + " line " + ss[1];
                    }
                    else
                    {
                        //fallback, should never happen
                        file = parent.traceContents.commandFileAndLine;
                        fileDetailed = parent.traceContents.commandFileAndLine;
                    }
                }

                if (file != null && file.Contains(":"))
                {
                    file = System.IO.Path.GetFileName(file);
                }
                string datafile = parent.traceContents.dataFile;
                if (datafile != null && datafile.Contains(":"))
                {
                    datafile = System.IO.Path.GetFileName(datafile);
                }
                string id = parent.traceContents.id.ToString().Split(' ')[0];

                if (type == 0 && depth > 0)
                {
                    string d = "{tce}";
                    traceLines.Add((depth - 1) + d + name + d + period + d + code + d + prec + d + file + d + datafile + d + id + d + parent.traceContents.name + d + parent.traceContents.period + d + parent.traceContents.text + d + prec + d + parent.traceContents.commandFileAndLine + d + parent.traceContents.dataFile + d + parent.traceContents.id);
                }
                else
                {
                    if (depth == 1)
                    {
                        int max = 3;
                        if (counter == max)
                        {
                            G.Writeln("| ...", System.Drawing.Color.Gray);
                        }
                        else if (counter >max)
                        {
                            //ignore
                        }
                        else
                        {
                            G.Writeln("| " + G.Blanks(2 * (depth - 1)) + code, System.Drawing.Color.Gray);
                        }
                        counter++;
                    }
                }

                //NOTE: list items are reversed!
                foreach (Trace2 child in parent.precedents.storage.AsEnumerable().Reverse().ToList())
                {
                    WalkTraces(child, depth + 1, traceLines, type, ref counter);
                }
            }
            finally
            {

                //resetting, also if there is an error
                Program.options.print_width = widthRemember;
                Program.options.print_filewidth = fileWidthRemember;
            }            
        }

        public static string RemoveNewlines(string input)
        {
            //return s.Replace(G.NL, " ").Replace("\r", " ").Replace("\n", " ").Replace("  ", " ").Replace("  ", " ");
            if (string.IsNullOrEmpty(input)) return input;
            // 1. \s+ matches any sequence of whitespace (tabs, newlines, spaces)
            // 2. We replace that entire sequence with a single space " "
            // 3. Trim() removes any leading or trailing spaces left over
            return Regex.Replace(input, @"\s+", " ").Trim();
        }

        public static string Truncate(string s)
        {
            if (s == null) return s;
            int n = 60;
            string s2 = RemoveNewlines(s);
            if (s2.Length > n)
            {
                s2 = s2.Substring(0, n) + " ...";
            }
            return s2;
        }

        public static TraceHelper CollectAllTraces(Databank databank, ETraceHelper type)
        {
            return CollectAllTraces(databank, type, double.NaN);
        }

        public static TraceHelper CollectAllTraces(Databank databank, ETraceHelper type, double scramble)
        {
            TraceHelper th1 = new TraceHelper();
            th1.type = type;
            th1.scramble = scramble;
            foreach (KeyValuePair<string, TimeSeries> kvp in databank.storage)
            {
                kvp.Value.DeepTrace(th1);
            }
            return th1;
        }

        public void DeepTrace(TraceHelper th, int depth)
        {            
            if (th.depthLimit != -12345 && depth >= th.depthLimit) return;
            if (th.type == ETraceHelper.GetAllMetasAndTraces)  //0 corresponds to direct effect from bank variable (e.g. "adambk:"), not indirect effect.
            {
                PrecedentsAndDepth temp = null; th.tracesDepth2.TryGetValue(this, out temp);
                if (temp == null)
                {
                    th.tracesDepth2.Add(this, new PrecedentsAndDepth() { precedents = this.precedents, depth = depth });
                }
                else
                {
                    //has been seen before
                    temp.depth = Math.Min(temp.depth, depth);
                    return;
                }

                if (this.precedents.storage.Count() > 0)
                {
                    foreach (Trace2 trace in this.precedents.storage)
                    {                        
                        trace.DeepTrace(th, depth + 1);
                    }
                }
            }            
        }

        /// <summary>
        /// After deserializing a protobuf gbk, this method restores trace connections from flat list (databank.traces).
        /// </summary>
        /// <param name="databank"></param>
        public static void HandleTraceRead1(Databank databank)
        {
            if (databank.traces != null && databank.traces.Count > 0)  //the .Count > 0 seems to be ok: why do anything if there are no traces?
            {
                try
                {
                    TraceHelper th = Gekko.Trace2.CollectAllTraces(databank, ETraceHelper.OnlyGetMetas);
                    Dictionary<TraceID2, Trace2> dictInverted = new Dictionary<TraceID2, Trace2>();
                    foreach (Trace2 trace in databank.traces) dictInverted[trace.GetId()] = trace;
                    HandleTraceRead2(th.metas, dictInverted);
                }
                finally
                {
                    if (databank != null) databank.traces = null;  //important!
                }
            }
        }

        /// <summary>
        /// After deserializing a protobuf gbk, this method restores trace connections from flat list (databank.traces).
        /// </summary>
        public static void HandleTraceRead2(List<TimeSeries> metas, Dictionary<TraceID2, Trace2> dict1Inverted)
        {
            foreach (TimeSeries meta in metas)
            {
                meta.FromID(dict1Inverted);
            }
            foreach (Trace2 trace in dict1Inverted.Values)
            {                
                trace.precedents.FromID(dict1Inverted);                
            }
        }

        /// <summary>
        /// Before serializing a protobuf gbk, this method removes trace connections, and kind of packs the connections into a flat list (databank.traces).
        /// </summary>
        /// <param name="databank"></param>
        /// <param name="th"></param>
        /// <param name="dict1Inverted"></param>
        public static void HandleTraceWrite(Databank databank, out TraceHelper th, out Dictionary<TraceID2, Trace2> dict1Inverted)
        {
            //gather lists
            th = Gekko.Trace2.CollectAllTraces(databank, ETraceHelper.GetAllMetasAndTraces);
            dict1Inverted = new Dictionary<TraceID2, Trace2>();
            foreach (Trace2 trace in th.tracesDepth2.Keys)
            {
                dict1Inverted[trace.GetId()] = trace;                
                trace.precedents.ToID();  //remove links                
            }
            foreach (TimeSeries meta in th.metas)
            {
                meta.ToID();
            }
            databank.traces = th.tracesDepth2.Keys.ToList();
        }

        public override string ToString()
        {
            return "<" + this.traceContents.period.ToString() + ">" + " " + this.traceContents.text;
        }
    }

    //public class Precedents2
    //{
    //    [ProtoMember(1)]
    //    private List<TraceAndPeriods2> storage = null;
    //}

    //public class TraceAndPeriods2
    //{
    //    //At the moment, periods are just == null here, but in the longer run we can store them.
    //    //Other fields like min and max period could also be added. But wait, that is just t1 from first period
    //    //and t2 from last period. The periods are successive, no?

    //    [ProtoMember(1)]
    //    public Trace2 trace = null;

    //    [ProtoMember(2)]
    //    public GekkoTimeSpansSimple periods = null;
    //}

    [ProtoContract]
    public class TraceContents2  //Trace2 because it is experimental
    {
        [ProtoMember(1)]
        public readonly TraceID2 id = new TraceID2();

        //In principle, these fields could be made readonly, but it would take a bit of refactoring.

        /// <summary>
        /// 1 timespan. This object is immutable (just as GekkoTime)
        /// </summary>
        [ProtoMember(2)]
        public GekkoTimeSpanSimple period = null;

        /// <summary>
        /// An extra char in a text string here will take up 2 bytes or 16 bits.
        /// </summary>        
        [ProtoMember(3)]
        public string text = null;

        [ProtoMember(4)]
        public string name = null;  //with bank and freq

        [ProtoMember(5)]
        public string commandFileAndLine = null;

        /// <summary>
        /// For instance the file from where data was imported. Will often be null.
        /// </summary>
        [ProtoMember(6)]
        public string dataFile = null;

        [ProtoMember(7)]
        public List<string> precedentsNames = null; //Elements are with bank and freq, but also starts with a type like "4¤..." to indicate info on databank, freq, and if the name has traces. See #9khsigra7ioau.

        public TraceContents2()
        {
            //for protobuf
        }

        public TraceContents2(GekkoTime t1, GekkoTime t2)
        {
            this.period = new GekkoTimeSpanSimple(t1, t2);
        }
    }

    [ProtoContract]
    /// <summary>
    /// For use with Trace. Like GekkoTime it is immutable, but it is not a struct. Should it be??
    /// </summary>
    public class GekkoTimeSpanSimple
    {
        [ProtoMember(1)]
        public readonly GekkoTime t1;
        [ProtoMember(2)]
        public readonly GekkoTime t2;

        public GekkoTimeSpanSimple()
        {
            //only because of protobuf
        }

        public GekkoTimeSpanSimple(GekkoTime t1, GekkoTime t2)
        {
            this.t1 = t1;
            this.t2 = t2;
        }

        public override string ToString()
        {
            return this.t1.ToString() + "-" + this.t2.ToString();
        }
    }

    [ProtoContract]
    public class TraceID2 //TraceID2 because it is experimental
    {
        /// <summary>
        /// Note: resolution is about 0.01 s.
        /// Switched from .Now to .UtcNow 5/9 2024, because .Now counts ticks since local time new Year 1900, but .UtcNow counts ticks
        /// since British New Year 1900. Local ticks will just confuse, with users in different time zones.
        /// And also, .UtcNow runs 3-4x faster than .Now (because .UtcNow is closer to the metal and does not have to look up which
        /// time zone the user happens to be in right now in this second).
        /// The change from .Now to .UtcNow will make older data traces 2 hours off for Danish users. Probably ok.
        /// </summary>
        [ProtoMember(1)]
        private readonly DateTime stamp = DateTime.UtcNow;  //Use .StampInLocalTime() when printing etc.!!! Faster than .Now and also more universal since it counts "tics" from the same Coordinated Universal Time.

        /// <summary>
        /// Used to distinguish traces, especially if these are pruned off. Will be numerically > 0, and when counter is < 0 it means that the trace is stored in en external file (pruned off).
        /// When Gekko starts up, the counter starts at a random position between 1 and 99% of long.MaxValue (9e18) and augments by 1 for each new trace.
        /// If the same Gekko session is used, there can be no collisions realistically, neither any overflow (that would demand > 9e16 calculations).
        /// With multiple Gekkos running at the same time, collisions would demand same stamp (unlikely) AND same counter (probability around 1e-19).
        /// Should never happen.
        /// </summary>
        [ProtoMember(2)]
        public readonly long counter = ++Globals.traceCounter;

        public TraceID2()
        {
        }
        public TraceID2(DateTime stamp, long counter)
        {
            this.stamp = stamp;
            this.counter = counter;
        }

        public DateTime StampInLocalTime()
        {
            return this.stamp.ToLocalTime();
        }

        public override bool Equals(object o)
        {
            TraceID2 other = o as TraceID2;
            if (other != null && this.stamp == other.stamp && this.counter == other.counter) return true;
            return false;
        }
        public override string ToString()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo(Globals.languageDaDK);
            string stamp = this.StampInLocalTime().ToString("d", ci);
            string stampDetailed = null;
            try
            {                
                stampDetailed = this.StampInLocalTime().ToString($"{ci.DateTimeFormat.ShortDatePattern} HH:mm:ss.fffffff", System.Globalization.CultureInfo.GetCultureInfo(Globals.languageDaDK)) + ", #" + this.counter;  //7 digits is 100 ns, which is limit anyway
                //
            }
            catch
            {
                stampDetailed = this.StampInLocalTime().ToString("G", System.Globalization.CultureInfo.GetCultureInfo(Globals.languageDaDK)) + ", #" + this.counter;
            }
            return stampDetailed;
            //return this.StampInLocalTime().ToString("d'/'M yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + ", #" + this.counter;
            //return this.StampInLocalTime().ToString("d/M yyyy HH:mm:ss", new System.Globalization.CultureInfo("da-DK")) + "|" + this.counter;
            //return this.StampInLocalTime().ToString() + "|" + this.counter;  //We want this printed in local time, not UTC time.
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.stamp.GetHashCode();  //No need to use .ToLocalTime() here: we just hash the the global ("true" and common) UTC time.
            hash = hash * 31 + this.counter.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// Basically a List of GekkoTimeSpanSimple's, where the latter is GekkoTime t1, t2.
    /// </summary>
    [ProtoContract]
    public class GekkoTimeSpansSimple
    {
        [ProtoMember(1)]
        private List<GekkoTimeSpanSimple> storage = new List<GekkoTimeSpanSimple>();

        public GekkoTimeSpansSimple()
        {
        }
        public GekkoTimeSpansSimple(List<GekkoTimeSpanSimple> x)
        {
            this.storage = x;
        }

        public GekkoTimeSpanSimple this[int i]
        {
            get { return this.storage[i]; }
        }

        public int Count()
        {
            return this.storage.Count;
        }

        public void Add(GekkoTimeSpanSimple gts)
        {
            this.storage.Add(gts);
        }

        public void AddRange(GekkoTimeSpansSimple gtss)
        {
            this.storage.AddRange(gtss.storage);
        }

        /// <summary>
        /// Use for iterators only.
        /// </summary>
        /// <returns></returns>
        public List<GekkoTimeSpanSimple> GetStorage()
        {
            return this.storage;
        }

        public void SetStorage(List<GekkoTimeSpanSimple> input)
        {
            this.storage = input;
        }

        public override string ToString()
        {
            string s = null;
            foreach (GekkoTimeSpanSimple x in this.storage)
            {
                s += x.ToString() + ", ";
            }
            if (s.EndsWith(", ")) s = s.Substring(0, s.Length - ", ".Length);
            return s;
        }
    }

    public class TraceHelper
    {
        public ETraceHelper type = ETraceHelper.GetAllMetasAndTraces;
        public double scramble = double.NaN;  //for scramble() function
        public int seriesObjectCount = 0; //number of series found (probably often equal to meta count)
        public List<TimeSeries> metas = new List<TimeSeries>();
        public int depthLimit = -12345;
        public Dictionary<Trace2, Precedents2> traces = new Dictionary<Trace2, Precedents2>();  //value is parent (may be null)
        public Dictionary<Trace2, PrecedentsAndDepth> tracesDepth2 = new Dictionary<Trace2, PrecedentsAndDepth>();

    }

    public class PrecedentsAndDepth
    {
        public Precedents2 precedents = null;
        public int depth = 0;
    }



}
