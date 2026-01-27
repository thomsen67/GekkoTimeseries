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

    [Serializable]
    [ProtoContract]
    public class Trace2
    {

        [ProtoMember(1)]
        public List<Trace2> precedents = null;

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

        public static void PushIntoSeries(Trace2 traceLhs, TimeSeries tsLhs, List<TimeSeries> tsRhss)
        {
            foreach (TimeSeries tsRhs in tsRhss)
            {
                if (tsRhs.trace != null)
                {
                    if (traceLhs.precedents == null) traceLhs.precedents = new List<Trace2>();
                    traceLhs.precedents.Add(tsRhs.trace);
                }
            }
            tsLhs.trace = traceLhs;
        }

        public static void WalkTraces(Trace2 parent, int depth, List<string>traceLines)
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
                    xx.RemoveAll(s => string.Equals(s, parent.traceContents.name, StringComparison.OrdinalIgnoreCase));
                    prec = string.Join(", ", xx);
                }
                
                //These must be short
                string name = parent.traceContents.name;
                string period = parent.traceContents.period.ToString().Split(' ')[0];
                string code = RemoveNewlines(parent.traceContents.text);
                string file = parent.traceContents.commandFileAndLine;
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

                traceLines.Add(depth + "¤" + name + "¤" + period + "¤" + code + "¤" + prec + "¤" + file + "¤" + datafile + "¤" + id + "¤" + parent.traceContents.name + "¤" + parent.traceContents.period + "¤" + parent.traceContents.text + "¤" + prec + "¤" + parent.traceContents.commandFileAndLine + "¤" + parent.traceContents.dataFile + "¤" + parent.traceContents.id);

                //G.Writeln("| " + G.Blanks(2 * depth) + parent.traceContents.name + " -- " + parent.traceContents.period + " -- " + Truncate(parent.traceContents.text) + " -- " + Truncate(prec) + " -- " + parent.traceContents.commandFileAndLine + " -- " + parent.traceContents.dataFile + " -- " + parent.traceContents.id, System.Drawing.Color.Gray);                

                if (parent.precedents != null)
                {
                    foreach (Trace2 child in parent.precedents)
                    {
                        WalkTraces(child, depth + 1, traceLines);
                    }
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
    }

    public class Precedents2
    {
        [ProtoMember(1)]
        private List<TraceAndPeriods2> storage = null;
    }

    public class TraceAndPeriods2
    {
        //At the moment, periods are just == null here, but in the longer run we can store them.
        //Other fields like min and max period could also be added. But wait, that is just t1 from first period
        //and t2 from last period. The periods are successive, no?

        [ProtoMember(1)]
        public Trace2 trace = null;

        [ProtoMember(2)]
        public GekkoTimeSpansSimple periods = null;
    }

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
        //trace.GetContents().commandFileAndLine = p?.GetExecutingGcmFile(ERunningGcm.IncludeProcFunc);

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

    

}
