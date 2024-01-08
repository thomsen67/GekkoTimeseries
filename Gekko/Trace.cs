﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

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
        OnlyGetMetas,
        GetTimeShadowInfo
    }

    [ProtoContract]
    public class TraceContents
    {
        /// <summary>
        /// An extra char in this .text will take up 2 bytes or 16 bits.
        /// </summary>        

        [ProtoMember(1)]
        public GekkoTimeSpanSimple span = null;

        [ProtoMember(2)]
        public string text = null;

        [ProtoMember(3)]
        public string bankAndVarnameWithFreq = null;

        [ProtoMember(4)]
        public string commandFileAndLine = null;
                
        /// <summary>
        /// For instance the file from where data was imported. Will often be null.
        /// </summary>
        [ProtoMember(5)]
        public string dataFile = null;               

        [ProtoMember(6)]
        public List<string> precedentsNames = null;

        public TraceContents DeepClone()
        {            
            TraceContents trace2 = new TraceContents();
            trace2.span = this.span;  //it is immutable
            trace2.bankAndVarnameWithFreq = this.bankAndVarnameWithFreq;
            trace2.commandFileAndLine = this.commandFileAndLine;
            trace2.text = this.text;
            trace2.dataFile = this.dataFile;
            return trace2;
        }

        public TraceContents()
        {
            //for protobuf
        }

        public TraceContents(GekkoTime t1, GekkoTime t2)
        {
            this.span = new GekkoTimeSpanSimple(t1, t2);
        }

        public TraceContents(bool isNullTime)
        {
            if (isNullTime)
            {
                this.span = new GekkoTimeSpanSimple(GekkoTime.tNull, GekkoTime.tNull);
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
        private Precedents precedents = new Precedents();  //be careful accessing it, use GetPrecedentsAndShadowedPeriods()        

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
            if (t1.IsNull() || t2.IsNull()) new Error("Trace time error");
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
        /// Only for internal use.
        /// </summary>
        /// <returns></returns>
        public Precedents GetPrecedents_BewareOnlyInternalUse()
        {
            return this.precedents;
        }

        /// <summary>
        /// Get all traces from series ts. Safer to use than AddRange(). Method does nothing if rhs == null, rhs.meta == null or rhs.meta.trace2 == null.
        /// </summary>
        /// <param name="rhs"></param>
        public void AddRangeFromSeries(Series lhs, Series rhs)
        {
            if (rhs == null || Object.ReferenceEquals(lhs, rhs)) return; //do not point to your own trace!
            bool hasTrace = true; if (rhs?.meta?.trace2 == null) hasTrace = false;
            if (this.contents.precedentsNames == null) this.contents.precedentsNames = new List<string>();
            this.contents.precedentsNames.Add((hasTrace ? "+" : null) + rhs.GetNameAndParentDatabank());
            if (hasTrace) this.GetPrecedents_BewareOnlyInternalUse().AddRange(rhs.meta.trace2.GetPrecedents_BewareOnlyInternalUse()); //may come from an old Gekko databank where .trace2 == null.           
        }

        /// <summary>
        /// Only for internal use.
        /// </summary>
        /// <param name="x"></param>
        public void SetPrecedents_BewareOnlyInternalUse(Precedents x)
        {
            this.precedents = x;
        }

        /// <summary>
        /// For the list of precedents in this.precedents, the method checks which traces are shadowed by later traces.
        /// With shadowedTracesAreRemoved == false, the count of the list returned will be the same as the count of the
        /// count of this.precedents (so we also get null-dividers). The included list includes period information, and
        /// if shadowedTracesAreRemoved == false, the period info may be empty.
        /// </summary>
        /// <param name="shadowedTracesAreRemoved"></param>
        /// <returns></returns>
        public List<TraceAndPeriods> TimeShadow2(bool shadowedTracesAreRemoved)
        {            
            List<TraceAndPeriods> rv = new List<TraceAndPeriods>();
            if (this.precedents.Count() > 0)
            {
                //Remove the if below at some point, just for sanity now            
                if (this.precedents[0] == null || this.precedents[this.precedents.Count() - 1] == null) new Error("Unexpected");
                List<List<GekkoTimeSpanSimple>> spansList = new List<List<GekkoTimeSpanSimple>>();  //is inverted, newest first
                int lastNull = this.precedents.Count();
                int counterI = -1;
                for (int i = this.precedents.Count() - 1; i >= 0; i--)
                {
                    counterI++;
                    Trace2 traceNew = this.precedents[i];

                    if (counterI == 0)
                    {
                        //To get the first one going.
                        List<GekkoTimeSpanSimple> tmp = new List<GekkoTimeSpanSimple>();
                        tmp.Add(traceNew.contents.span);
                        spansList.Add(tmp);
                    }

                    if (i == 0 || traceNew == null)
                    {
                        int count = -1;
                        for (int k = 0; k < spansList.Count; k++)
                        {
                            count++;
                            List<GekkoTimeSpanSimple> m = spansList[k];
                            if (!shadowedTracesAreRemoved || m.Count > 0)
                            {
                                TraceAndPeriods tap = new TraceAndPeriods();
                                tap.trace = this.precedents[lastNull - count - 1];
                                //The two below are a bit wasteful. Maybe represent contents.t1|t2 via GekkoTimeSpanSimple instead.
                                tap.periods = m;
                                rv.Add(tap);
                            }
                        }
                        if (i > 0) rv.Add(null); //divider
                        counterI = -1;
                        spansList.Clear();
                        lastNull = i;
                        continue;
                    }

                    int counterJ = -1;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        counterJ++;
                        Trace2 traceOld = this.precedents[j];
                        if (traceOld == null) break;

                        if (counterI == 0)
                        {
                            List<GekkoTimeSpanSimple> spans = Trace2.TimeShadow1(traceNew.contents.span, traceOld.contents.span);
                            spansList.Add(spans);
                        }
                        else
                        {
                            int k2 = counterI + counterJ + 1;
                            List<GekkoTimeSpanSimple> newList = new List<GekkoTimeSpanSimple>();
                            foreach (GekkoTimeSpanSimple spanTemp in spansList[k2])
                            {
                                List<GekkoTimeSpanSimple> spans = Trace2.TimeShadow1(traceNew.contents.span, spanTemp);
                                newList.AddRange(spans);
                            }
                            spansList[k2] = newList;
                        }
                    }
                }
            }
            rv.Reverse();
            return rv;
        }

        public List<TraceAndPeriods> TimeShadow2()
        {
            return this.TimeShadow2(true);
        }

        /// <summary>
        /// Test if a trace is "real" (false) or "invisible" (true). Invisible traces are directly linked to timeseries
        /// and have no contents. They are just an entry into the real traces.
        /// These should not count in statistics etc.
        /// Remember that a divider trace can have .contents == null! (this will also return true and should also not be counted)
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
            else s = this.contents.span.t1 + "-" + this.contents.span.t2 + ": " + this.contents.text;
            return s;
        }        

        public void DeepTrace(TraceHelper th, int depth)
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
                        trace.DeepTrace(th, depth + 1);
                    }
                }
            }            
            else if (th.type == ETraceHelper.GetTimeShadowInfo)
            {
                ScalarVal temp = null; th.timeShadowing.TryGetValue(this, out temp);
                if (temp == null)
                {
                    th.timeShadowing.Add(this, Globals.scalarVal1);
                    if (this.precedents.Count() > 0)
                    {
                        th.input += this.precedents.Count();
                        List<TraceAndPeriods> shadow = this.TimeShadow2(true);
                        th.output += shadow.Count;
                        if (shadow.Count > precedents.Count())
                        {
                            new Error("Hov!");
                        }
                        else if (shadow.Count != precedents.Count())
                        {
                            this.precedents = new Precedents();
                            foreach (TraceAndPeriods temp2 in shadow)
                            {
                                if (temp2 == null) this.precedents.Add(null);
                                else this.precedents.Add(temp2.trace);
                            }
                        }
                    }
                }
                else
                {
                    //has been seen before
                    //temp.lighted++;
                    //temp.shadowed++;
                    return;
                }                
                
                if (this.precedents.Count() > 0)
                {
                    foreach (Trace2 trace in this.precedents.GetStorage())
                    {
                        if (trace == null) continue;
                        trace.DeepTrace(th, depth + 1);
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

        public TwoStrings Text(int d)
        {            
            string s1 = null;
            string s2 = null;
            if (true)
            {
                s1 = this.contents.text;
                string period = this.contents.span.t1 + "-" + this.contents.span.t2;
                int len = "---".Length;
                if (s1 != null) len = s1.Length;
                s2 += G.Blanks(50 - len - 2 * d) + " --> period: " + period;
                s2 += ", stamp: " + this.id.stamp.ToString("g", System.Globalization.CultureInfo.CreateSpecificCulture(Globals.languageDaDK));
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
            //
            // !!!
            // !!! In the longer run, these IF's can be removed
            // !!!            
            if (trace == null) new Error("Trace problem: trace == null");
            if (trace.contents.text == null) new Error("Trace problem: trace.contents.text == null");
            if (ts.meta == null) new Error("Trace problem: ts.meta == null");
            
            if (ts.meta.trace2 == null) ts.meta.trace2 = new Trace2(ETraceType.Parent);            
            if (type == ETracePushType.NewParent)
            {                   
                trace.AddRangeFromSeries(null, ts);
                ts.meta.trace2.precedents = new Precedents();
                ts.meta.trace2.precedents.Add(trace);
            }            
            else if (type == ETracePushType.Sibling)
            {                
                ts.meta.trace2.precedents.Add(trace);
            }
            else new Error("Trace");
        }        

        /// <summary>
        /// For the newSpan, it removes these periods from the oldSpan. Returns a list of GekkoTimeSpanSimple with 0, 1 or 2 elements.
        /// Used in TimeShadow2().
        /// </summary>
        /// <param name="newSpan"></param>
        /// <param name="oldSpan"></param>
        /// <param name="periodsContainer"></param>
        public static List<GekkoTimeSpanSimple> TimeShadow1(GekkoTimeSpanSimple newSpan, GekkoTimeSpanSimple oldSpan)
        {
            // The code below removes the --- from the ===, so newSpan removes periods from oldSpan
            // Four possibilities
            //
            //             =============                  === is oldSpan, --- is newSpan
            //  -----                         -----       A. Outside (left or right)
            //          -----        -----                B. Cut from left or right                                
            //                 ----                       C. Separate in two
            //          -------------------               D. Shadow and remove
            //            
            //
                        
            List<GekkoTimeSpanSimple> rv = new List<GekkoTimeSpanSimple>();  //this construction is pretty fast

            if (newSpan.IsNull() || oldSpan.IsNull())
            {
                //One of the spans is null, no shadowing then...
                rv.Add(oldSpan);
            }
            else if (newSpan.t2.StrictlySmallerThan(oldSpan.t1) || newSpan.t1.StrictlyLargerThan(oldSpan.t2))
            {
                //A, nothing happens
                rv.Add(oldSpan);
            }            
            else if (newSpan.t1.SmallerThanOrEqual(oldSpan.t1) && newSpan.t2.StrictlySmallerThan(oldSpan.t2))
            {
                //B left
                rv.Add(new GekkoTimeSpanSimple(newSpan.t2.Add(1), oldSpan.t2));
            }
            else if (newSpan.t1.StrictlyLargerThan(oldSpan.t1) && newSpan.t2.LargerThanOrEqual(oldSpan.t2))
            {
                //B right
                rv.Add(new GekkoTimeSpanSimple(oldSpan.t1, newSpan.t1.Add(-1)));
            }
            else if (newSpan.t1.StrictlyLargerThan(oldSpan.t1) && newSpan.t2.StrictlySmallerThan(oldSpan.t2))
            {
                //C cut in two
                rv.Add(new GekkoTimeSpanSimple(oldSpan.t1, newSpan.t1.Add(-1)));
                rv.Add(new GekkoTimeSpanSimple(newSpan.t2.Add(1), oldSpan.t2));
            }
            else if (newSpan.t1.SmallerThanOrEqual(oldSpan.t1) && newSpan.t2.LargerThanOrEqual(oldSpan.t2))
            {
                //D, remove --> nothing added
            }
            else new Error("Wrong logic regarding time spans");
            return rv;
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
                    trace.DeepTrace(th, 0);                    
                    int count2 = Trace2.CountWithoutInvisible(th.tracesDepth2);
                    string s = "Traces";
                    //if (all) s = count2 + " " + "traces (click [] to see more info)";
                    if (true)
                    {
                        if (trace.precedents.Count() > 0)
                        {
                            Action<GAO> a = (gao) =>
                            {
                                TreeGridModel model = new TreeGridModel();

                                Item temp = trace.CopyToItems(0, 0);
                                foreach (Item item in temp.Children)
                                {
                                    model.Add(item);
                                }

                                WindowTreeViewWithTable w = new WindowTreeViewWithTable(model); 
                                string v = null;
                                if (trace.contents != null) v = G.Chop_RemoveBank(trace.contents.bankAndVarnameWithFreq, Program.databanks.GetFirst().name) + " - ";
                                w.Title = v + "Gekko trace";
                                w.ShowDialog();
                            };
                            s += " (" + G.GetLinkAction("view " + count2, new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ")";
                        }
                    }
                    s += ":";
                    //if (all) G.Writeln();
                    G.Writeln(s);
                    //txt.MainAdd(s);
                    //txt.MainNewLineTight();
                }
                PrintTraceHelper(trace, 0);
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

        private static void PrintTraceHelper(Trace2 trace, int d)
        {
            if (d > 1) return;
            string s = null;            
            s = "| ";            
            
            if (trace == null)
            {
                G.Write(s);
                G.Writeln("---", Globals.MiddleGray);
            }
            else
            {
                if (!Trace2.IsInvisibleTrace(trace))
                {
                    TwoStrings s2 = trace.Text(d);
                    G.Write(s + s2.s1);
                    G.Writeln(s2.s2, Globals.MiddleGray);
                }

                int max = 5;
                int start = 0;
                if (trace.precedents.Count() > max)
                {
                    start = trace.precedents.Count() - max;
                    if (d < 1) G.Writeln("...omitted " + start + " older traces...", System.Drawing.Color.Gray);
                }                
                                
                if (trace.precedents.Count() > 0)
                {
                    int counter = -1;
                    foreach (Trace2 child in trace.precedents.GetStorage())
                    {
                        counter++;
                        if (counter >= start) PrintTraceHelper(child, d + 1);
                    }
                }                
            }
        }

        //  d=0, c=1, null         -->  aa
        //  d=1, c=3, x3=x1+x2     -->  ---
        //  d=2, c=0, x1=...       --> x3=x1+x2
        //  d=2, c=0, DIVIDER      --> x3=x1+x2
        //  d=2, c=0, x2=...       --> x3=x1+x2

        public Item CopyToItems(int depth, int cnt)
        {            
            bool hasChildren = false;
            if (this.precedents.Count() > 0) hasChildren = true;
            string text = "null";
            string code = "null";
            string period = null;
            string file = null;
            string fileDetailed = null;
            string stamp = null;
            string stampDetailed = null;
            List<string> precedentsNames = null;
            if (this.contents != null)
            {
                text = G.Chop_RemoveFreq(G.Chop_RemoveBank(this.contents.bankAndVarnameWithFreq, Program.databanks.GetFirst().name), Program.options.freq);                
                code = this.contents.text;
                GekkoTime t1 = this.contents.span.t1;
                GekkoTime t2 = this.contents.span.t2;
                if (t1.IsNull() && t2.IsNull()) period = "";
                else period = "" + t1.ToString() + "-" + t2.ToString() + "";
                int counter = 0;
                if (!G.NullOrBlanks(this.contents.commandFileAndLine))
                {
                    string[] ss = this.contents.commandFileAndLine.Split('¤');
                    file = System.IO.Path.GetFileName(ss[0]) + " line " + ss[1];
                    fileDetailed = ss[0] + " line " + ss[1];
                }
                if (!G.NullOrBlanks(this.contents.dataFile)) file += " (data = " + System.IO.Path.GetFileName(this.contents.dataFile) + ")";
                if (!G.NullOrBlanks(this.contents.dataFile)) fileDetailed += " (data = " + this.contents.dataFile + ")";
                stamp = this.id.stamp.ToString("g", System.Globalization.CultureInfo.CreateSpecificCulture(Globals.languageDaDK));
                stampDetailed = this.id.stamp.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture(Globals.languageDaDK));
                precedentsNames = this.contents.precedentsNames;
            }
            
            Item newNode = new Item(text, code, period, stamp, stampDetailed, file, fileDetailed, precedentsNames, hasChildren);
            if (depth < 4)
            {
                if (this.precedents.GetStorage() != null)
                {
                    int n = -1;
                    foreach (Trace2 child in this.precedents.GetStorage())
                    {
                        n++;
                        Item newChild = null;
                        if (child != null)
                        {
                            //G.Writeln("depth " + depth + " alternative " + n + " cnt " + cnt);
                            newChild = child.CopyToItems(depth + 1, cnt + 1);
                            newNode.Children.Add(newChild);
                        }
                        else
                        {
                            //newChild = new Item("----------", "---", false);
                        }
                    }
                }
            }
            return newNode;
        }


        public static Item ViewerTraceHelper(Trace2 trace, int d, bool all, Item parent)
        {
            
            
            Item copy = trace.CopyToItems(0, 0);
            return copy;
            
            
            //{

            //    if (!all && d > 1) return;
            //    string s = null;

            //    if (all)
            //    {
            //        if (!Trace2.IsInvisibleTrace(trace))
            //        {
            //            Action<GAO> a = (gao) =>
            //            {
            //                G.Writeln();
            //                G.Writeln("Trace:     " + trace.contents.text);
            //                G.Writeln("Period:    " + trace.contents.GetT1() + "-" + trace.contents.span.t2 + "");
            //                G.Writeln("Active:    " + trace.PrintPeriods());
            //                if (trace.contents.bankAndVarnameWithFreq != null) G.Writeln("LHS var:   " + trace.contents.bankAndVarnameWithFreq);
            //                if (trace.contents.dataFile != null) G.Writeln("Data file: " + trace.contents.dataFile);
            //                if (trace.contents.commandFileAndLine != null) G.Writeln("Gcm file:  " + trace.contents.commandFileAndLine.Replace("¤", " line ").Trim());
            //                G.Writeln("Stamp:     " + trace.id.stamp.ToString("dd-MM-yyyy HH:mm:ss"));
            //                G.Writeln("ID:        " + trace.id.counter);
            //            };
            //            string more = G.GetLinkAction("[]", new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + " ";
            //            G.Write(more);
            //        }
            //        s = G.Blanks(d * 2 - 2);
            //    }
            //    else
            //    {
            //        s = "| ";
            //    }

            //    if (trace == null)
            //    {
            //        G.Write(s);
            //        G.Writeln("---", Globals.MiddleGray);
            //    }
            //    else
            //    {
            //        bool hasChildren = trace.precedents.Count() > 0;
            //        Item child = null;

            //        if (true || !Trace2.IsInvisibleTrace(trace))
            //        {
            //            parent.HasChildren = hasChildren;
            //            string txt = "---";
            //            if (!Trace2.IsInvisibleTrace(trace))
            //            {
            //                TwoStrings s2 = trace.Text(false, d);
            //                txt = s + s2.s1;
            //            }


            //            child = new Item(txt, 12321, false);
            //            //Item child1 = new Item(s + s2.s1, 12321, false);
            //            //Item child2 = new Item(s + s2.s1, 12321, false);
            //            //root.Children.Add(child1);
            //            //root.Children.Add(child2);
            //            parent.Children.Add(child);
            //        }
            //        if (trace.precedents.Count() > 0)
            //        {
            //            foreach (Trace2 tchild in trace.precedents.GetStorage())
            //            {
            //                ViewerTraceHelper(tchild, d + 1, all, child);
            //            }
            //        }
            //    }
            //}
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
        public int seriesObjectCount = 0; //number of series found (probably often equal to meta count)
        public List<SeriesMetaInformation> metas = new List<SeriesMetaInformation>();
        
        // --- the following is for stats etc. ("real" traces)        
        public int unittestTraceCountIncludeInvisible = 0; //will include combinations, traces will not
        public Dictionary<Trace2, Precedents> traces = new Dictionary<Trace2, Precedents>();  //value is parent (may be null)
                
        // --- gbk write/read and other stuff
        //Hmm, isn't Precedents already a part of the key? Anyway, the depth needs to be inside an object anyway to be altered.
        public Dictionary<Trace2, PrecedentsAndDepth> tracesDepth2 = new Dictionary<Trace2, PrecedentsAndDepth>();
        //
        // --- this is for time-shadowing
        public int input = 0;
        public int output = 0;
        public Dictionary<Trace2, ScalarVal> timeShadowing = new Dictionary<Trace2, ScalarVal>();

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

        /// <summary>
        /// Add into precedents.storage. Be careful that something like trace.GetPrecedents_BewareOnlyInternalUse().AddRange(ts.meta.trace2.GetPrecedents_BewareOnlyInternalUse()) may
        /// fail if ts.meta.trace2 is == null. In cases like that, better to use trace.GetPrecedents_BewareOnlyInternalUse().AddRangeFromSeries(ts).
        /// </summary>
        /// <param name="precedents"></param>
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

    /// <summary>
    /// Only used for reporting, to know how periods shadow each other. Never stored in databanks etc.
    /// </summary>
    public class TraceAndPeriods
    {
        public Trace2 trace = null;
        public List<GekkoTimeSpanSimple> periods = null;
    }

    //public class TraceShadowingHelper
    //{
    //    public int lighted = 0; //was retained in a TimeShadow sweep
    //    public int shadowed = 0; //was removed in a TimeShadow sweep
    //}
}
