using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
//using System.Windows.Forms;
//using static alglib;

namespace Gekko
{
    /// <summary>
    /// Used for the .trace field of timeseries
    /// </summary>    
    /// 
    public enum ETraceType
    {
        Parent,
        Child
    }

    public enum ETracePushType
    {
        Sibling,
        NewParent
    }

    public enum ETraceHelper
    {        
        GetAllMetasAndTraces,
        OnlyGetMetas
    }

    [ProtoContract]
    public class TraceContents
    {
        [ProtoMember(1)]
        private GekkoTime t1 = GekkoTime.tNull;

        [ProtoMember(2)]
        private GekkoTime t2 = GekkoTime.tNull;

        [ProtoMember(3)]
        public string bankAndVarnameWithFreq = null;

        [ProtoMember(4)]
        public string commandFileAndLine = null;

        /// <summary>
        /// An extra char in this .text will take up 2 bytes or 16 bits.
        /// </summary>
        [ProtoMember(5)]
        public string text = null;

        /// <summary>
        /// For instance the file from where data was imported. Will often be null.
        /// </summary>
        [ProtoMember(6)]
        public string dataFile = null;

        /// <summary>
        /// The "active" left-hand side periods for the current trace (that is, what the trace determines). 
        /// At some point, the periods inside should be compacted. For instance a list of GekkoTimes 1966-2022 is a
        /// waste of space. Some kind of interval logic could be implemented. For instance, updating 2000-2010 over the 
        /// 1966-2022 interval would break 1966-2022 into --> 1966-1999 and 2011-2022. Would save a lot of space.
        /// See maybe https://github.com/mbuchetics/RangeTree for ideas. But this tree is only for searching though.
        /// Perhaps allow combo of intervals (for > 3 dates) and single dates.
        /// </summary>
        [ProtoMember(7)]
        public Periods periods = new Periods();

        public GekkoTime GetT1()
        {
            return this.t1;
        }

        public GekkoTime GetT2()
        {
            return this.t2;
        }

        public TraceContents DeepClone()
        {
            TraceContents trace2 = new TraceContents();
            trace2.t1 = this.t1;
            trace2.t2 = this.t2;
            trace2.bankAndVarnameWithFreq = this.bankAndVarnameWithFreq;
            trace2.commandFileAndLine = this.commandFileAndLine;
            trace2.text = this.text;
            trace2.dataFile = this.dataFile;
            trace2.periods = new Periods();

            if (this.periods.Count() > 0)
            {
                trace2.periods.Initialize();
                foreach (GekkoTimeSpanSimple gtss in this.periods.GetStorage())
                {
                    GekkoTimeSpanSimple temp = new GekkoTimeSpanSimple(); //We use this constructor because .t1 and .t2 may or may not be null.
                    temp.t1 = gtss.t1;
                    temp.t2 = gtss.t2;
                    trace2.periods.Add(temp);
                }
            }

            return trace2;
        }

        public TraceContents()
        {
            //for protobuf
        }

        public TraceContents(GekkoTime t1, GekkoTime t2)
        {
            this.t1 = t1;
            this.t2 = t2;
            if (!this.t1.IsNull() && !this.t2.IsNull())
            {
                this.periods = new Periods();
                this.periods.Add(new GekkoTimeSpanSimple(this.t1, this.t2));
            }
        }

        public TraceContents(bool isNullTime)
        {
            if (isNullTime)
            {
                this.t1 = GekkoTime.tNull;
                this.t2 = GekkoTime.tNull;
                this.periods = new Periods();
                this.periods.Add(new GekkoTimeSpanSimple(isNullTime));
            }
            else new Error("TraceContents time error");
        }
    }

    /// <summary>    
    /// Trace
    /// x.id (TraceID), is a DateTime and random long.
    /// x.contents (TraceContents), has .t1 and .t2 and .periods, where periods is list of time ranges.
    /// x.precedents (TracePrecedents), basically a List&lt;Trace>
    /// We may have: 
    /// (1) contents != null and id.counter > 0 (normal).
    /// (2) contents == null and id.counter &lt; 0 (pruned off to external file).
    /// (3) contents == null and id.counter > 0 (temporarily prepared for protobuf).
    /// </summary>
    [ProtoContract]
    public class Trace2  //Trace2 because it is experimental
    {
        [ProtoMember(1)]
        public TraceID2 id = new TraceID2();

        [ProtoMember(2)]
        public TraceContents contents = null;

        [ProtoMember(3)]
        public Precedents precedents = new Precedents();

        private Trace2()
        {
            //Only for protobuf and DeepClone()
        }

        /// <summary>
        /// Construct a parent trace. For this, .contents will be == null.
        /// </summary>
        /// <param name="type"></param>
        public Trace2(ETraceType type)
        {
            if (type == ETraceType.Child) new Error("Trace constructor problem");            
        }

        /// <summary>
        /// Child trace. Also sets stamp, traceversion and .t1 and .t2 (and fills .periods with this range).
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public Trace2(GekkoTime t1, GekkoTime t2)
        {
            if (t1.IsNull() || t2.IsNull()) 
                new Error("Trace time error");
            this.contents = new TraceContents(t1, t2);            
        }

        public Trace2(bool isNullTime)
        {
            if (isNullTime)
            {
                this.contents = new TraceContents(isNullTime);
            }
            else new Error("Trace period problem");
        }

        /// <summary>
        /// Test if a trace is "real" (false) or "invisible" (true). Invisible traces are directly linked to timeseries
        /// and have no contents. They are just an entry into the real traces.
        /// These should not count in statistics etc.
        /// Remember that a divider trace can be == null!
        /// </summary>
        /// <returns></returns>
        public static bool IsInvisibleTrace(Trace2 trace)
        {
            if (trace == null) return false;
            return trace.contents == null;
        }

        public string ToString()
        {
            string s = null;
            if (this.GetTraceType() == ETraceType.Parent) s = "------- meta parent entry: " + this.contents.bankAndVarnameWithFreq + " -------";
            else s = this.contents.GetT1() + "-" + this.contents.GetT2() + ": " + this.contents.text;
            return s;
        }        

        public void DeepTrace(TraceHelper th, Trace2 parent, int depth)
        {
            if (th.type == ETraceHelper.GetAllMetasAndTraces)
            {                
                th.unittestTraceCountIncludeInvisible++; //only for testing

                PrecedentsAndDepth temp = null; th.tracesDepth2.TryGetValue(this, out temp);
                if (temp == null)
                {
                    th.tracesDepth2.Add(this, new PrecedentsAndDepth() { precedents = this.precedents, depth = depth });
                }
                else
                {
                    //has been seen before
                    temp.depth = Math.Min(temp.depth, depth);
                    if (!Globals.traceWalkAllCombinations) return;
                }

                if (!Trace2.IsInvisibleTrace(this))
                {                    
                    if (!th.traces.ContainsKey(this)) th.traces.Add(this, this.precedents);                    
                }

                if (this.precedents.Count() > 0)
                {
                    foreach (Trace2 trace in this.precedents.GetStorage())
                    {
                        if (trace == null) continue;                        
                        trace.DeepTrace(th, this, depth + 1);
                    }
                }
            }            
        }        

        public Trace2 DeepClone(CloneHelper cloneHelper)
        {
            object known = null;
            Trace2 trace2 = null;
            if (cloneHelper != null)
            {
                cloneHelper.dict.TryGetValue(this, out known);
            }
            if (known == null)
            {
                trace2 = new Trace2();
                if (this.contents != null)
                {
                    trace2.contents = this.contents.DeepClone();
                }
                trace2.precedents = this.precedents.DeepClone(cloneHelper);
                if (cloneHelper != null)
                {
                    cloneHelper.dict.Add(this, trace2);
                }
            }
            else
            {
                trace2 = known as Trace2;
            }            
            return trace2;
        }

        //public void PrintRecursive(int depth, List<string> output)
        //{
        //    if (depth > 0)
        //    {
        //        string s = Text();
        //        output.Add("-" + G.Blanks(2 * (depth - 1)) + s);
        //    }
        //    if (this.precedents.Count() > 0)
        //    {
        //        foreach (Trace2 child in this.precedents.GetStorage())
        //        {
        //            if (child == null)
        //            {
        //                output.Add("-" + G.Blanks(2 * (depth - 1)) + "----------");
        //            }
        //            else
        //            {
        //                child.PrintRecursive(depth + 1, output);
        //            }
        //        }
        //    }            
        //    if (depth == 0 && output.Count == 0) new Writeln("[No trace found]");
        //}

        /// <summary>
        /// Pretty print periods and stamp.
        /// </summary>
        /// <returns></returns>
        public string PrintPeriods()
        {
            string s = null;
            if (this.contents.periods.Count() > 0)
            {
                //s += "";
                foreach (GekkoTimeSpanSimple gtss in this.contents.periods.GetStorage()) s += gtss.t1 + "-" + gtss.t2 + ", ";
            }
            if (s.EndsWith(", ")) s = s.Substring(0, s.Length - 2);
            return s;
        }

        public string PrintStamp()
        {
            string s = null;            
            s += this.id.stamp.ToString("dd/MM/yyyy HH:mm:ss") + "|" + this.id.counter;
            return s;
        }

        /// <summary>
        /// Returns .Parent if .contents == null.
        /// </summary>
        /// <returns></returns>
        public ETraceType GetTraceType()
        {
            ETraceType x = ETraceType.Child;
            if (this.contents == null) x = ETraceType.Parent;
            return x;
        }

        public TwoStrings Text(bool unfold, int d)
        {            
            string s1 = null;
            string s2 = null;
            if (!unfold)
            {
                s1 = this.contents.text;
                string period = this.contents.GetT1() + "-" + this.contents.GetT2();
                int len = "---".Length;
                if (s1 != null) len = s1.Length;
                s2 += G.Blanks(50 - len - 2 * d) + " --> period:" + period;
                string active = this.PrintPeriods();
                if (!G.Equal(period, active))
                {
                    s2 += " (active:" + active + ")";
                }
                s2 += ", " + this.id.stamp.ToString("dd-MM-yyyy HH:mm:ss");
            }
            else
            {
                s2 += "" + this.contents.GetT1() + "-" + this.contents.GetT2() + "";
                s2 += " --> ";
                s1 += this.contents.text;
                s2 += "          ";
                s2 += " || " + this.PrintPeriods();
                if (this.contents.bankAndVarnameWithFreq != null) s2 += " || lhs=" + this.contents.bankAndVarnameWithFreq;
                if (this.contents.dataFile != null) s2 += " || data=" + this.contents.dataFile;
                if (this.contents.commandFileAndLine != null) s2 += " || gcm=" + this.contents.commandFileAndLine;
                s2 += " || " + this.PrintStamp();
            }
            return new TwoStrings(s1, s2);
        }

        /// <summary>
        /// Type == NewParent ---> Puts the new trace on top of the series traces. Reconnects the existing series trace(s) to the new trace.
        /// Type == Sibling ---> Puts it among siblings. Removes the date(s) from its siblings.
        /// </summary>
        /// <param name="ts"></param>
        public static void PushIntoSeries(Series ts, Trace2 trace, ETracePushType type)
        {
            if (trace.contents.text == null) new Error("PushIntoSeries problem");
            if (ts.meta.trace2 == null) ts.meta.trace2 = new Trace2(ETraceType.Parent);
            if (type == ETracePushType.NewParent)
            {                   
                trace.precedents.AddRange(ts.meta.trace2.precedents);
                ts.meta.trace2.precedents = new Precedents();
                ts.meta.trace2.precedents.Add(trace);
            }            
            else if (type == ETracePushType.Sibling)
            {
                if (ts.meta.trace2.precedents.Count() > 0)
                {
                    if (ts.meta.trace2.GetTraceType() != ETraceType.Parent) new Error("Trace type error");  //should never be possible huh???

                    if (trace.contents.periods.Count() != 1) new Error("Problem with time spans");
                    GekkoTimeSpanSimple thsSpan = trace.contents.periods[0];

                    if (!thsSpan.IsNull()) //if null --> cannot shadow anything
                    {
                        List<Trace2> siblingsToRemove = new List<Trace2>();

                        foreach (Trace2 sibling in ts.meta.trace2.precedents.GetStorage())
                        {
                            //We know that sibling's parent always has GetTraceType() == ETraceType.Parent
                            //So the siblings all belong to the same timeseries, and therefore it is ok
                            //to remove periods.
                            //trace is the new trace that is going to be added    

                            if (sibling.contents.periods.Count() > 0)
                            {
                                List<GekkoTimeSpanSimple> spansTemp = new List<GekkoTimeSpanSimple>();
                                foreach (GekkoTimeSpanSimple siblingSpan in sibling.contents.periods.GetStorage())
                                {
                                    // Four possibilities
                                    //
                                    //             =============                  === is siblingSpan (existing), --- is thsSpan (new one)
                                    //  -----                         -----       A. Outside (left or right)
                                    //          -----        -----                B. Cut from left or right                                
                                    //                 ----                       C. Separate in two
                                    //          -------------------               D. Shadow and remove
                                    //
                                    // The code below removes the --- from the ===, so removes periods from siblingSpan
                                    //

                                    if (siblingSpan.IsNull())
                                    {
                                        //will not be touched
                                        spansTemp.Add(siblingSpan);
                                    }
                                    else if (thsSpan.t2.StrictlySmallerThan(siblingSpan.t1) || thsSpan.t1.StrictlyLargerThan(siblingSpan.t2))
                                    {
                                        //A, nothing happens
                                        spansTemp.Add(siblingSpan);
                                    }
                                    else if (thsSpan.t1.SmallerThanOrEqual(siblingSpan.t1) && thsSpan.t2.LargerThanOrEqual(siblingSpan.t2))
                                    {
                                        //D, remove --> nothing added
                                    }
                                    else if (thsSpan.t1.SmallerThanOrEqual(siblingSpan.t1) && thsSpan.t2.StrictlySmallerThan(siblingSpan.t2))
                                    {
                                        //B left
                                        spansTemp.Add(new GekkoTimeSpanSimple(thsSpan.t2.Add(1), siblingSpan.t2));
                                    }
                                    else if (thsSpan.t1.StrictlyLargerThan(siblingSpan.t1) && thsSpan.t2.LargerThanOrEqual(siblingSpan.t2))
                                    {
                                        //B right
                                        spansTemp.Add(new GekkoTimeSpanSimple(siblingSpan.t1, thsSpan.t1.Add(-1)));
                                    }
                                    else if (thsSpan.t1.StrictlyLargerThan(siblingSpan.t1) && thsSpan.t2.StrictlySmallerThan(siblingSpan.t2))
                                    {
                                        //C cut in two
                                        spansTemp.Add(new GekkoTimeSpanSimple(siblingSpan.t1, thsSpan.t1.Add(-1)));
                                        spansTemp.Add(new GekkoTimeSpanSimple(thsSpan.t2.Add(1), siblingSpan.t2));
                                    }
                                    else new Error("Wrong logic regarding time spans");
                                }
                                if (spansTemp.Count() == 0)
                                {
                                    siblingsToRemove.Add(sibling);
                                }
                                sibling.contents.periods.SetStorage(spansTemp);
                            }
                        }
                        if (siblingsToRemove.Count > 0)
                        {
                            List<Trace2> tempTrace = new List<Trace2>();
                            foreach (Trace2 sibling in ts.meta.trace2.precedents.GetStorage())
                            {
                                if (sibling == null)
                                {
                                    if (tempTrace.Count > 0 && tempTrace[tempTrace.Count - 1] == null)
                                    {
                                        //do nothing, we do not want two nulls 
                                    }
                                    else
                                    {
                                        tempTrace.Add(null);
                                    }                                    
                                }
                                else
                                {
                                    if (!siblingsToRemove.Contains(sibling))
                                    {
                                        tempTrace.Add(sibling);
                                    }
                                }
                            }
                            if (tempTrace.Count == 0) tempTrace = null;
                            ts.meta.trace2.precedents.SetStorage(tempTrace);
                        }
                    }
                }
                ts.meta.trace2.precedents.Add(trace);
            }
            else new Error("Trace");
        }

        ///// <summary>
        ///// Removes a particular trace from ts.meta.trace in a Series. Happens when a new Trace shadows other older traces.
        ///// </summary>
        ///// <param name="ts"></param>
        //public static void RemoveFromSeries(Series ts, Trace ths)
        //{
        //    if (ts.meta.trace == null) return;
        //    if (ts.meta.trace.precedents.Count() > 0)
        //    {                
        //        ts.meta.trace.precedents.GetStorage().Remove(ths);
        //    }
        //}

        public static TraceHelper CollectAllTraces(Databank databank, ETraceHelper type)
        {            
            TraceHelper th1 = new TraceHelper();
            th1.type = type;
            foreach (KeyValuePair<string, IVariable> kvp in databank.storage)
            {
                kvp.Value.DeepTrace(th1);
            }
            return th1;
        }

        /// <summary>
        /// After deserializing a protobuf gbk, this method restores trace connections from flat list (databank.traces).
        /// </summary>
        /// <param name="databank"></param>
        public static void HandleTraceRead1(Databank databank)
        {
            if (databank.traces != null && databank.traces.Count > 0)  //the .Count > 0 seems to be ok: why do anything if there are no traces?
            {
                TraceHelper th = Gekko.Trace2.CollectAllTraces(databank, ETraceHelper.OnlyGetMetas);
                Dictionary<TraceID2, Trace2> dictInverted = new Dictionary<TraceID2, Trace2>();
                foreach (Trace2 trace in databank.traces) dictInverted[trace.id] = trace;
                HandleTraceRead2(th.metas, dictInverted);
                databank.traces = null;  //important!
            }
        }

        /// <summary>
        /// After deserializing a protobuf gbk, this method restores trace connections from flat list (databank.traces).
        /// </summary>
        public static void HandleTraceRead2(List<SeriesMetaInformation> metas, Dictionary<TraceID2, Trace2> dict1Inverted)
        {                         
            foreach (SeriesMetaInformation meta in metas)
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
                dict1Inverted[trace.id] = trace;
                trace.precedents.ToID();  //remove links
            }
            foreach (SeriesMetaInformation meta in th.metas)
            {
                meta.ToID();
            }
            databank.traces = th.tracesDepth2.Keys.ToList();
            if (Globals.runningOnTTComputer) new Writeln("TTH: " + databank.traces.Count + " traces written");
        }

        public static void PrintTraceHelper(Trace2 trace, bool all)
        {
            int widthRemember = Program.options.print_width;
            Program.options.print_width = int.MaxValue;
            try
            {
                //using (Writeln txt = new Writeln())
                {
                    //txt.lineWidth = int.MaxValue;
                    TraceHelper th = new TraceHelper();
                    trace.DeepTrace(th, null, 0);                    
                    int count2 = Trace2.CountWithoutInvisible(th.tracesDepth2);
                    string s = "Traces";
                    if (all) s = count2 + " " + "traces (click [] to see more info)";
                    if (!all)
                    {
                        if (trace.precedents[0].precedents.Count() > 0)
                        {
                            Action<GAO> a = (gao) =>
                            {
                                PrintTraceHelper(trace, true);
                            };
                            s += " (" + G.GetLinkAction("show " + count2, new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ")";
                        }
                    }
                    s += ":";
                    if (all) G.Writeln();
                    G.Writeln(s);
                    //txt.MainAdd(s);
                    //txt.MainNewLineTight();
                }
                PrintTraceHelper(trace, 0, all);
            }
            finally
            {
                //resetting, also if there is an error
                Program.options.print_width = widthRemember;
            }
        }

        /// <summary>
        /// Omit traces that are null (dividers) or traces with .content == null (glued to series objects)
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static int CountWithoutInvisible(Dictionary<Trace2, PrecedentsAndDepth> dict)
        {
            int n = 0;
            foreach (Trace2 trace2 in dict.Keys)
            {
                if (Trace2.IsInvisibleTrace(trace2)) continue;
                n++;
            }
            return n;
        }

        private static void PrintTraceHelper(Trace2 trace, int d, bool all)
        {
            if (!all && d > 1) return;
            string s = null;

            if (all)
            {
                if (!Trace2.IsInvisibleTrace(trace))
                {
                    Action<GAO> a = (gao) =>
                    {
                        G.Writeln();
                        G.Writeln("Trace:     " + trace.contents.text);
                        G.Writeln("Period:    " + trace.contents.GetT1() + "-" + trace.contents.GetT2() + "");
                        G.Writeln("Active:    " + trace.PrintPeriods());
                        if (trace.contents.bankAndVarnameWithFreq != null) G.Writeln("LHS var:   " + trace.contents.bankAndVarnameWithFreq);
                        if (trace.contents.dataFile != null) G.Writeln("Data file: " + trace.contents.dataFile);
                        if (trace.contents.commandFileAndLine != null) G.Writeln("Gcm file:  " + trace.contents.commandFileAndLine.Replace("¤", " line ").Trim());
                        G.Writeln("Stamp:     " + trace.id.stamp.ToString("dd-MM-yyyy HH:mm:ss"));
                        G.Writeln("ID:        " + trace.id.counter);
                    };
                    string more = G.GetLinkAction("[]", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + " ";
                    G.Write(more);
                }
                s = G.Blanks(d * 2 - 2);
            }
            else
            {
                s = "| ";
            }
            
            if (trace == null)
            {
                G.Write(s);
                G.Writeln("---", Globals.MiddleGray);
            }
            else
            {
                if (!Trace2.IsInvisibleTrace(trace))
                {
                    TwoStrings s2 = trace.Text(false, d);
                    G.Write(s + s2.s1);
                    G.Writeln(s2.s2, Globals.MiddleGray);
                }
                if (trace.precedents.Count() > 0)
                {
                    foreach (Trace2 child in trace.precedents.GetStorage())
                    {
                        PrintTraceHelper(child, d + 1, all);
                    }
                }
            }
        }        
    }

    [ProtoContract]
    public class TraceID2 //TraceID2 because it is experimental
    {
        /// <summary>
        /// Note: resolution is about 0.01 s.
        /// </summary>
        [ProtoMember(1)]
        public DateTime stamp = DateTime.Now;

        /// <summary>
        /// Used to distinguish traces, especially if these are pruned off. Will be numerically > 0, and when counter is < 0 it means that the trace is stored in en external file (pruned off).
        /// When Gekko starts up, the counter starts at a random position between 1 and 99% of long.MaxValue (9e18) and augments by 1 for each new trace.
        /// If the same Gekko session is used, there can be no collisions realistically, neither any overflow (that would demand > 9e16 calculations).
        /// With multiple Gekkos running at the same time, collisions would demand same stamp (unlikely) AND same counter (probability around 1e-19).
        /// Should never happen.
        /// </summary>
        [ProtoMember(2)]
        public long counter = ++Globals.traceCounter;

        public override bool Equals(object o)
        {
            TraceID2 other = o as TraceID2;
            if (other != null && this.stamp == other.stamp && this.counter == other.counter) return true;
            return false;
        }
        public override string ToString()
        {
            return this.stamp.ToString() + "|" + this.counter;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.stamp.GetHashCode();
            hash = hash * 31 + this.counter.GetHashCode();
            return hash;
        }
    }

    public class TraceHelper
    {
        public ETraceHelper type = ETraceHelper.GetAllMetasAndTraces;
        public int seriesObjectCount = 0; //number of series found (probably often equatl to meta count)
        public List<SeriesMetaInformation> metas = new List<SeriesMetaInformation>();
        // ----------
        // --- the following is for stats etc. ("real" traces)        
        public int unittestTraceCountIncludeInvisible = 0; //will include combinations, traces will not
        public Dictionary<Trace2, Precedents> traces = new Dictionary<Trace2, Precedents>();  //value is parent (may be null)
                
        // --- gbk write/read and other stuff
        //Hmm, isn't Precedents already a part of the key? Anyway, the depth needs to be inside an object anyway to be altered.
        public Dictionary<Trace2, PrecedentsAndDepth> tracesDepth2 = new Dictionary<Trace2, PrecedentsAndDepth>();
    }

    public class PrecedentsAndDepth
    {
        public Precedents precedents = null;
        public int depth = 0;
    }

    /// <summary>
    /// Is basically a List&lt;Trace>.
    /// </summary>
    [ProtoContract]
    public class Precedents
    {
        [ProtoMember(1)]
        private List<Trace2> storage = null;

        /// <summary>
        /// Pretty innocuous: using this, we can set .storage = null before protobuf.
        /// </summary>
        [ProtoMember(2)]
        public List<TraceID2> storageIDTemporary = null;  //used to recreate connections after protobuf. Will not take up space in general.
        
        public void AddRange(Precedents precedents)
        {            
            if (precedents.storage != null)
            {
                if (this.storage == null) this.storage = new List<Trace2>();
                this.storage.AddRange(precedents.storage);
            }
        }

        /// <summary>
        /// Add a Trace to precedents list. Cannot add a "meta entry" to a Trace. These can only be set for .trace in SeriesMetaInformation objects.
        /// </summary>
        /// <param name="trace"></param>
        /// <exception cref="GekkoException"></exception>
        public void Add(Trace2 trace)
        {
            if (this.storage == null) this.storage = new List<Trace2>();
            if (trace != null && trace.contents == null) throw new GekkoException();
            this.storage.Add(trace);
        }

        /// <summary>
        /// Only for iterators! REMEMBER to put an "if (xxx.precedents.Count() > 0) {... " before iterating!!
        /// </summary>
        /// <returns></returns>
        public List<Trace2> GetStorage()
        {
            return this.storage;
        }

        /// <summary>
        /// Use this with care
        /// </summary>
        /// <param name="m"></param>
        public void SetStorage(List<Trace2> m)
        {
            if (m != null && m.Count == 0) this.storage = null; //so it does not take up space
            else this.storage = m;
        }

        public  void ToID()
        {
            this.storageIDTemporary = new List<TraceID2>();
            if (this.Count() > 0)
            {
                foreach (Trace2 trace in this.GetStorage())
                {
                    TraceID2 temp = null;
                    if (trace == null)
                    {
                        temp = new TraceID2();
                        temp.counter = long.MinValue;  //negative, signals null
                        temp.stamp = DateTime.MinValue;
                    }
                    else
                    {
                        temp = trace.id;
                    }
                    this.storageIDTemporary.Add(temp);
                }
            }
            this.SetStorage(null);
        }

        public void FromID(Dictionary<TraceID2, Trace2> dict2)
        {
            if (this.storageIDTemporary != null && this.storageIDTemporary.Count > 0)
            {
                this.storage = new List<Trace2>();
                foreach (TraceID2 id in this.storageIDTemporary)
                {
                    if (id.counter == long.MinValue)
                    {
                        this.storage.Add(null);
                    }
                    else
                    {
                        if (id.counter < 0) new Error("This trace is not stored in the databank, but has been pruned off: " + id.ToString());
                        Trace2 trace = null; dict2.TryGetValue(id, out trace);
                        if (trace == null) new Error("Could not find this trace in databank: " + id.ToString());
                        this.storage.Add(trace);
                    }
                }
            }
            this.storageIDTemporary = null;
        }

        public int Count()
        {
            if (this.storage == null) return 0;
            return this.storage.Count;
        }

        public Trace2 this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }

        public Precedents DeepClone(CloneHelper cloneHelper)
        {
            Precedents precedents = new Precedents();            
            if (this.storage != null)
            {
                precedents.storage = new List<Trace2>();
                foreach (Trace2 trace in this.storage)
                {
                    if (trace == null)
                    {
                        precedents.Add(null);
                    }
                    else
                    {
                        precedents.storage.Add(trace.DeepClone(cloneHelper));
                    }
                }
            }
            return precedents;
        }

        public string ToString()
        {
            return "Traces = " + this.Count();
        }
    }

    [ProtoContract]
    public class Periods
    {
        [ProtoMember(1)]
        private List<GekkoTimeSpanSimple> storage = null;

        /// <summary>
        /// Beware: only for iterating.
        /// </summary>
        /// <returns></returns>
        public List<GekkoTimeSpanSimple> GetStorage()
        {
            return this.storage;
        }

        public GekkoTimeSpanSimple this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }

        /// <summary>
        /// Use with care
        /// </summary>
        /// <param name="storage"></param>
        public void SetStorage(List<GekkoTimeSpanSimple> storage)
        {
            this.storage = storage;
        }

        /// <summary>
        /// Beware: use with care.
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            this.storage = new List<GekkoTimeSpanSimple>();
        }

        public void Add(GekkoTimeSpanSimple x)
        {
            if (storage == null) storage = new List<GekkoTimeSpanSimple>();
            this.storage.Add(x);
        }

        public int Count()
        {
            if (this.storage == null) return 0;
            return this.storage.Count;
        }
    }
}
