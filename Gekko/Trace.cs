using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Text;
using Microsoft.Data.Analysis;

// Simplificed overview
//
//+ Timeseries meta object
//  + Trace2                                         <---- just a phoney object ("GluedToSeries")
//    + TraceContents2             
//    + List<TraceAndPeriods2>                       <---- precedents
//      + List<GekkoTimeSpanSimple>
//      + Trace2
//        + TraceContents2
//        + List <TraceAndPeriods2>                   <---- precedents

//So each trace has contents (like command line) and n precedents. Each precedent is a (trace, timespans), so a precedent
//is not "just" another trace, but a (trace, timespans) combination.Because the same previous trace may be time-shadowed
//in different ways in different places. In the precedents there may be dividers.

//When protobuffed, the precedents (List<TraceAndPeriods2>) are cut off and replaced with ID's. So if there are n precedents,
//.storageIDTemporary and .storagePeriodsTemporary will each get n elements, where the former is a traceID consisting of a
//combination (DateTime, long) and the latter is raw periods.


namespace Gekko
{
    /// 
    public enum ETraceType
    {
        Normal,
        GluedToSeries,
        Divider,
        Dangling  //not used?
    }

    /// <summary>
    /// Used for the .trace field of timeseries
    /// </summary>    
    /// 
    public enum ETraceParentOrChild
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
        TrimWithTimeShadowing,
        Scramble  //not actually used for traces
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

        public TraceContents2(bool isNullTime)
        {
            if (isNullTime)
            {
                this.period = new GekkoTimeSpanSimple(GekkoTime.tNull, GekkoTime.tNull);
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
        private Precedents2 precedents = new Precedents2();  //be careful accessing it, use GetPrecedentsAndShadowedPeriods()
                                                             //
        [ProtoMember(2)]
        public readonly ETraceType type = ETraceType.Normal;  //default

        [ProtoMember(3)]
        public readonly TraceContents2 traceContents = null; 

        //Only for protobuf and DeepClone()
        private Trace2()
        {

        }

        public Trace2(ETraceType type, TraceContents2 traceContents)
        {
            this.type = type;
            this.traceContents = traceContents;
        }

        public Trace2(ETraceType type)
        {
            this.type = type;
            TraceContents2 traceContents = new TraceContents2();
            this.traceContents = traceContents;
        }

        /// <summary>
        /// Construct a parent trace. For this, .contents will be == null.
        /// </summary>
        /// <param name="childOrParentType"></param>
        public Trace2(ETraceType type, ETraceParentOrChild childOrParentType)
        {                      
            if (childOrParentType == ETraceParentOrChild.Child) new Error("Trace constructor problem");            
            this.type = type;
            TraceContents2 traceContents = new TraceContents2();
            this.traceContents = traceContents;
        }

        /// <summary>
        /// Child trace. Also sets stamp, traceversion and .t1 and .t2 (and fills .periods with this range).
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public Trace2(ETraceType type, GekkoTime t1, GekkoTime t2, bool nullPeriodAccepted)
        {            
            if (!nullPeriodAccepted && (t1.IsNull() || t2.IsNull())) new Error("Trace time error");            
            this.type = type;
            TraceContents2 traceContents = new TraceContents2(t1, t2);
            this.traceContents = traceContents;         
        }

        public Trace2(ETraceType type, GekkoTime t1, GekkoTime t2) : this(type, t1, t2, false)
        {
            //overload
        }

        /// <summary>
        /// Must be called with isNullTime == true.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isNullTime"></param>
        public Trace2(ETraceType type, bool isNullTime)
        {
            this.type = type;

            if (isNullTime)
            {
                TraceContents2 traceContents = new TraceContents2(isNullTime);
                this.traceContents = traceContents;
            }
            else new Error("Trace period problem");
                        
            if (type == ETraceType.Divider)
            {
                //This is done for size reasons (ram size, gbk size).
                //A bit inefficient since .id and .precedents objects are
                //first created (in the constructur) and since deleted.
                //We can live with that inefficiency.                
                this.precedents = null;
            }
        }

        public TraceContents2 GetContents()
        {
            return this.traceContents;
        }

        public TraceID2 GetId()
        {
            return this.traceContents.id;
        }

        /// <summary>
        /// Only for internal use.
        /// </summary>
        /// <returns></returns>
        public Precedents2 GetPrecedents_BewareOnlyInternalUse()
        {
            return this.precedents;
        }

        /// <summary>
        /// Get all traces from series rhs into trace (later put inside lhs series).
        /// The method is used for assignments, and assignments automatically identify all series "asked" on the rhs.
        /// When altering something regarding traces, make sure precedentsNames is also altered!
        /// Dividers are inserted to separate each rhs-trace-collection from the next.
        /// See also AddRangeFromSeries2().
        /// </summary>
        /// <param name="lhsTrace"></param>
        /// <param name="rhs"></param>
        public static void AddRangeFromSeries1(Trace2 lhsTrace, Series rhs)
        {                        
            bool hasTrace = true; if (rhs?.meta?.trace2 == null) hasTrace = false;
            if (lhsTrace.GetContents().precedentsNames == null) lhsTrace.GetContents().precedentsNames = new List<string>();
            lhsTrace.GetContents().precedentsNames.Add(TraceGetNameDecorated(rhs, hasTrace));

            if (hasTrace && rhs.meta.trace2.GetPrecedents_BewareOnlyInternalUse().Count() > 0)
            {
                int counter2 = -1;
                foreach (TraceAndPeriods2 kvp in rhs.meta.trace2.GetPrecedents_BewareOnlyInternalUse().GetStorage())
                {
                    //Looping through RHS traces, from RHS variable

                    TraceAndPeriods2 rhsTraceTap = kvp;
                    Trace2 rhsTrace = rhsTraceTap.trace;

                    bool similar = false;

                    if (Globals.traceEndoRhsFix1)
                    {
                        //This only deals with producing too many (deep) trace references for x[%t] = x[%t] + 2;
                        //inside a time loop. It is not the number of traces that is the problem, but how they
                        //cross-reference (and the depth of these references).

                        int n1 = 2;  //see comment below

                        for (int i1 = 0; i1 < n1; i1++)
                        {
                            //looping through previous traces at same depth on LHS
                            //
                            // We are trying to avoid this:
                            //
                            // x[2003] = x[2003] + 1;   trace #4
                            // x[2002] = x[2002] + 1;   trace #3
                            // x[2001] = x[2001] + 1;   trace #2
                            // x = 1;                   trace #1   
                            //
                            // where trace #4 refers to trace #3, trace #3 refers to trace #2, trace #2 refers to trace #1
                            // This creates a deep spiderweb of references, so when we get to trace #3, Gekko will detect
                            // that trace #2 is similar and skip the reference (trace #3 will still end up referencing trace #2).
                            // Normally looking 1 trace back is enough for a time loop, but then what about 
                            // x = 1; for val %t = 2001 to 2003; x[%t] = x[%t] + 1; x[%t] = x[%t] + 0; end;
                            // because there are 2 statements inside loop, n1 = 1 would not catch this. Therefore we set
                            // n > 1, but needs not be too large though, because n1 would only deal with the LHS variable
                            // appearing on the RHS, which usually is not done several times consecutively.
                            // So we set n1 = 2, which should be more than enough.
                            //
                            // HMM double loop RHS+LHS if n is large
                            //
                            if (i1 + 1 > lhsTrace.precedents.Count()) break;  //cannot get to n1
                            TraceAndPeriods2 previousLhsTraceTap = lhsTrace.precedents.GetStorage()[lhsTrace.precedents.Count() - (i1 + 1)];  //looks at the last one, then the second last one.
                            if (Object.ReferenceEquals(previousLhsTraceTap, rhsTraceTap)) goto LabelDoNotAddAsChild; //actually same, faster check. Can this even happen?
                            if (IsSimilarTrace(previousLhsTraceTap.trace, rhsTrace)) goto LabelDoNotAddAsChild;
                        }
                    }

                    if (Globals.traceEndoRhsFix2)
                    {
                        if (IsSimilarTrace(lhsTrace, rhsTrace) && lhsTrace.traceContents.period.t1.EqualsGekkoTime(rhsTrace.traceContents.period.t1) && lhsTrace.traceContents.period.t2.EqualsGekkoTime(rhsTrace.traceContents.period.t2))
                        {
                            //
                            //We are about to get something like this, for instance x = 1; x = x + 1; x = x + 1;
                            //
                            // x = x + 1;       trace #3    (lhsTrace, about to be added to)
                            //     x = x + 1;   trace #2    (rhsTrace)
                            //         x = 1;   trace #1    (could have siblings)
                            //
                            //But if produced by a loop, this may become very deep and unnecessary. So we cut trace #2 off,
                            //eliminating it, so that we insted get this:
                            //
                            // x = x + 1;       trace #3                            
                            //     x = 1;       trace #1   

                            // We will have a problem with this:
                            //
                            // reset; x = 1;
                            // x = x + 1; x = x + 0;
                            // x = x + 1; x = x + 0;
                            // x = x + 1; x = x + 0;
                            // x = x + 1; x = x + 0;
                            // x = x + 1; x = x + 0;
                            //
                            // because of the alternation. But that would take two consecutive x-with-lagged-endo, which would be rare.
                            
                            if (rhsTrace.precedents.Count() > 0)
                            {
                                foreach (TraceAndPeriods2 kvp2 in rhsTrace.GetPrecedents_BewareOnlyInternalUse().GetStorage())
                                {
                                    if (lhsTrace.precedents.GetStorage() == null) lhsTrace.precedents.InitWithEmptyList();
                                    lhsTrace.precedents.GetStorage().Add(kvp2);
                                }
                                goto LabelDoNotAddAsChild;
                            }
                        }
                    }

                    counter2++;
                    if (lhsTrace.precedents.GetStorage() == null) lhsTrace.precedents.InitWithEmptyList();                    
                    if (counter2 == 0 && lhsTrace.precedents.GetStorage().Count > 0 && lhsTrace.precedents.GetStorage()[lhsTrace.precedents.GetStorage().Count - 1] != null)
                    {
                        lhsTrace.precedents.GetStorage().Add(new TraceAndPeriods2(new Trace2(ETraceType.Divider, true), Globals.traceNullPeriods));  //divider  
                    }

                    // --------- clone start ----------------
                    //We must clone the period part of the trace+period, because otherwise it may be overwritten in a wrong way.
                    GekkoTimeSpansSimple tempSpans = null;
                    if (rhsTraceTap.periods != null)
                    {
                        tempSpans = new GekkoTimeSpansSimple();
                        tempSpans.AddRange(rhsTraceTap.periods);  //the timespans themselves are immutable
                    }
                    TraceAndPeriods2 childTrace2Clone = new TraceAndPeriods2(rhsTraceTap.trace, tempSpans);
                    // --------- clone end ----------------

                    lhsTrace.precedents.GetStorage().Add(childTrace2Clone);
                LabelDoNotAddAsChild:;
                }
            }
        }

        /// <summary>
        /// If we are
        /// (a) in same session (counters differ little) and
        /// (b) code is equal and
        /// (c) file + line is equal
        /// then --> we do not add this trace.
        /// For instance reset; x &lt;2014 2024> = 2; for val %t = 2014 to 2024; x[%t] = x[%t] + 2; end;
        /// This loop will produce a network of references, accumulating more and more for traces near 2024.
        /// The if here makes sure we do not get a lot of non-interesting dublets. But beware that something like copy b:*; for
        /// some timeseries will make their first traces look identical even though sub-traces under copy b:*; can be
        /// different because it is different timeseries. That is handled.
        /// </summary>
        /// <param name="lastTrace"></param>
        /// <param name="newTrace"></param>
        /// <returns></returns>
        private static bool IsSimilarTrace(Trace2 lastTrace, Trace2 newTrace)
        {
            //We cannot compare periods, because we want x[%t] to be able to prune out similar traces over different periods.
            if (!G.Equal(lastTrace.GetContents().name, newTrace.GetContents().name))
            {
                //cannot be a similar trace, if x{%i} == ... in two traces defines a differnet LHS variable!
                //Now even if "b:x!a" is the same in both traces, and the code line is the same, could it still be a
                //different series object? Yes, in principle, but it would be a bit weird, involving another "b" bank.
                //Traces do not point back to their series objects: if they did, object equality could be used.
                return false;  
            }
            if (Math.Abs(lastTrace.GetContents().id.GetCounter() - newTrace.GetContents().id.GetCounter()) > 1000000) return false;
            if (lastTrace.GetContents().text != newTrace.GetContents().text) return false;
            if (lastTrace.GetContents().commandFileAndLine != newTrace.GetContents().commandFileAndLine) return false;
            return true;
        }        

        /// <summary>
        /// Used in trace: .precedentsNames. For a series x!a in databank b, theres is a prefix {i}¤ on names, where i is an integer from 1 to 8.
        /// See code: it is combinations of has trace, is first-pos databank, and is current freq.
        /// </summary>
        /// <param name="rhs"></param>
        /// <param name="hasTrace"></param>
        /// <returns></returns>
        public static string TraceGetNameDecorated(Series rhs, bool hasTrace)  //See #9khsigra7ioau
        {            
            string prefix = null;            
            string databankName = rhs.GetParentDatabank()?.GetName();  //databank may be null, for instance an imported series
            bool isFirst = G.Equal(databankName, Program.databanks.GetFirst().GetName());
            bool isCurrentFreq = rhs.freq == Program.options.freq;
            if (hasTrace)
            {
                if (isFirst)
                {
                    if (isCurrentFreq) prefix = Globals.number1;
                    else prefix = Globals.number2;
                }
                else
                {
                    if (isCurrentFreq) prefix = Globals.number3;
                    else prefix = Globals.number4;
                }
            }
            else
            {
                if (isFirst)
                {
                    if (isCurrentFreq) prefix = Globals.number5;
                    else prefix = Globals.number6;
                }
                else
                {
                    if (isCurrentFreq) prefix = Globals.number7;
                    else prefix = Globals.number8;
                }
            }
            return prefix + Globals.tracePrecedentsTypeDelimiter + rhs.GetNameAndParentDatabank();
        }


        /// <summary>
        /// Get all traces from series rhs into lhs. Safer to use than AddRange(). Method does nothing if rhs == null, rhs.meta == null or rhs.meta.trace2 == null.
        /// When altering something regarding traces, make sure precedentsNames is also altered!
        /// See also AddRangeFromSeries1().
        /// </summary>
        /// <param name="rhs"></param>
        public void AddRangeFromSeries2(Series lhs, Series rhs)
        {
            if (rhs == null || Object.ReferenceEquals(lhs, rhs)) return; //do not point to your own trace!
            if (rhs.type == ESeriesType.ArraySuper) return; //do not do this for array-series parent
            bool hasTrace = true; if (rhs?.meta?.trace2 == null) hasTrace = false;
            if (this.GetContents().precedentsNames == null) this.GetContents().precedentsNames = new List<string>();
            this.GetContents().precedentsNames.Add(TraceGetNameDecorated(rhs, hasTrace));
            if (hasTrace)
            {
                if (true)
                {
                    //RIGHT WAY!

                    //Doing some cloning: else this fails: (see unit test: #p0fjad8fjd)
                    //
                    //reset;
                    //time 2001 2003;
                    //p1!a = 100;
                    //interpolate p2!q = p1!a repeat;
                    //p1!a = 200;
                    //disp p2!q;
                    //
                    // ---> Now the trace of p2!q shows it depends upon the trace p1!a = 200. Which is obviously BAD.
                    //      Probably because without cloning there is a pointing p1!a --> TraceAndPeriod which points to 
                    //      the same object as p2!q --> p1!a --> TraceAndPeriod.
                    //
                    //Somewhat ugly, but we just need to make it work. Perhaps do it more clean for 
                    //a rewrite of traces. Wonder what .DeepClone() can do regarding this?

                    if (rhs.meta.trace2.precedents.Count() > 0)
                    {                        
                        List<TraceAndPeriods2> taps_clone = new List<TraceAndPeriods2>();
                        foreach (TraceAndPeriods2 tap in rhs.meta.trace2.precedents.GetStorage())  //.GetStorage() cannot be null (because .Count() > 0)
                        {
                            TraceAndPeriods2 tap_clone = new TraceAndPeriods2();
                            tap_clone.trace = tap.trace;
                            if (true)
                            {
                                //This timeperiod cloning may not be necessary, but unsure precisely WHY it is not necessary --> anyway: not costly.
                                //Clone the list of timespans (not the timespans themselves)
                                tap_clone.periods = new GekkoTimeSpansSimple();
                                foreach (GekkoTimeSpanSimple gtss in tap.periods.GetStorage())  //.GetStorage() cannot be null
                                {
                                    tap_clone.periods.Add(gtss);
                                }
                            }
                            else
                            {
                                tap_clone.periods = tap.periods;
                            }
                            taps_clone.Add(tap_clone);
                        }
                        if (this.precedents == null)
                        {
                            this.precedents = new Precedents2();
                            this.precedents.SetStorage(new List<TraceAndPeriods2>());
                        }
                        Precedents2 precedents_clone = new Precedents2();
                        precedents_clone.SetStorage(taps_clone);
                        this.precedents.AddRange(precedents_clone);
                    }
                }
                else
                {
                    //WRONG WAY!
                    //This is too naive: Problem is that rhs.meta.trace2.precedents can change, since trace2 her is 
                    //of type GluedToSeries, and hence .precedents (which is really a List<TraceAndPeriods2> is
                    //dynamic and may change afterwards). Therefore, we need to create a new .precendents where
                    //each TraceAndPeriods2 gets added.
                    this.GetPrecedents_BewareOnlyInternalUse().AddRange(rhs.meta.trace2.GetPrecedents_BewareOnlyInternalUse()); //may come from an old Gekko databank where .trace2 == null.           
                }
            }
        }

        /// <summary>
        /// Only for internal use.
        /// </summary>
        /// <param name="x"></param>
        public void SetPrecedents_BewareOnlyInternalUse(Precedents2 x)
        {
            this.precedents = x;
        }

        /// <summary>
        /// Overload.
        /// </summary>
        /// <returns></returns>
        public List<TraceAndPeriods2> TimeShadow2()
        {
            if (Globals.traceWallTimeHandledSpecialWayFor1UnitTest) return TimeShadow2(false);
            else return TimeShadow2(true);
        }

        /// <summary>
        /// For the list of precedents in this.precedents, the method checks which traces are shadowed by later traces.
        /// With shadowedTracesAreRemoved == false, the count of the list returned will be the same as the count of the
        /// count of this.precedents (so we also get null-dividers). The included list includes period information, and
        /// if shadowedTracesAreRemoved == false, the period info may be empty.
        /// BEWARE: can return null!
        /// </summary>        
        public List<TraceAndPeriods2> TimeShadow2(bool invertWallTime)
        {
            if (invertWallTime && !Globals.traceWallTimeHandledSpecialWayFor1UnitTest)
            {
                List<TraceAndPeriods2> rv = InvertWallTime(this.precedents.GetStorage());
                int count = 0; if (rv != null) count = rv.Count;
                if (count != this.precedents.Count()) new Error("TimeShadow problem");
                if (count > 1 && rv[0].trace.type == ETraceType.Divider) new Error("TimeShadow problem");
                if (count > 1 && rv[rv.Count - 1].trace.type == ETraceType.Divider) new Error("TimeShadow problem");
                return rv;
            }
            else
            {
                return this.precedents.GetStorage();
            }
        }

        public static List<TraceAndPeriods2> InvertWallTime(List<TraceAndPeriods2> rv3)
        {
            //Could use the Divide() method?
            if (rv3 == null || rv3.Count <= 1) return rv3;  //no need to do anything
            List<TraceAndPeriods2> rv = new List<TraceAndPeriods2>(rv3.Count);
            List<TraceAndPeriods2> temp = new List<TraceAndPeriods2>();
            int n = 0;
            foreach (TraceAndPeriods2 tap in rv3)
            {
                n++;
                if (tap.trace.type == ETraceType.Divider)
                {
                    temp.Reverse();
                    rv.AddRange(temp);
                    temp.Clear();
                    rv.Add(tap);
                }
                else
                {
                    temp.Add(tap);
                    if (n == rv3.Count)
                    {
                        temp.Reverse();
                        rv.AddRange(temp);
                        temp.Clear();
                    }
                }
            }
            return rv;
        }

        /// <summary>
        /// Test if a trace is "real" (false) or "invisible" (true). Invisible traces are directly linked to timeseries
        /// and have no contents. They are just an entry into the real traces.
        /// These should not count in statistics etc.
        /// Remember that a divider trace can have .contents == null! (this will also return true and should also not be counted)
        /// </summary>
        /// <returns></returns>
        public bool IsInvisibleTrace()
        {
            if (this.type == ETraceType.Normal) return false;
            return true;
        }

        public string ToString()
        {
            string s = null;
            if (this.GetTraceType() == ETraceParentOrChild.Parent) s = "------- meta parent entry: " + this.GetContents().name + " -------";
            else s = this.GetContents().period.t1 + "-" + this.GetContents().period.t2 + ": " + this.GetContents().text;
            return s;
        }        

        public void DeepTrace(TraceHelper th, int depth)
        {
            if (th.specialAdamStuff)
            {
                List<string> temp1 = new List<string>() { "work:ADAM_as_test!", "work:ADAM_ad_test!", "work:ADAM_av_test!", "work:ADAM_ax_fejl!", "work:ADAM_ax_test!" };
                foreach (string s in temp1)
                {
                    if (G.StartsWith(this.traceContents.name, s)) return;
                }

                //List<string> temp2 = new List<string>() { "work:adambk_", "work:ADAM_s!", "work:ADAM_pension!", "work:ADAM_q!", "work:ADAM_bfr!", "work:adam_hq!", "work:ADAM_owp!", "work:ADAM_et!", "work:ADAM_IO!", "work:adam_tip!" };
                //foreach (string s in temp2)
                //{
                //    if (G.StartsWith(this.traceContents.name, s)) { depth--; break; }
                //}

                if (G.StartsWith(this.traceContents.name, "work:adam_") || G.StartsWith(this.traceContents.name, "work:adambk_")) depth--;
            }

            if (th.depthLimit != -12345 && depth >= th.depthLimit) return;            
            if (th.type == ETraceHelper.GetAllMetasAndTraces)  //0 corresponds to direct effect from bank variable (e.g. "adambk:"), not indirect effect.
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

                if (!this.IsInvisibleTrace())
                {                    
                    if (!th.traces.ContainsKey(this)) th.traces.Add(this, this.precedents);                    
                }

                if (this.precedents.Count() > 0)
                {
                    foreach (TraceAndPeriods2 traceAndPeriods in this.precedents.GetStorage())
                    {
                        if (traceAndPeriods.trace.type == ETraceType.Divider) continue;                        
                        traceAndPeriods.trace.DeepTrace(th, depth + 1);
                    }
                }
            }            
            else if (th.type == ETraceHelper.TrimWithTimeShadowing)
            {
                string temp = null; th.timeShadowing.TryGetValue(this, out temp);  //do not look at the same trace object > 1 time.
                if (temp == null)
                {
                    th.timeShadowing.Add(this, "");  //will be interned
                    this.PrecedentsShadowing(null); //tracetrim2()
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
                    foreach (TraceAndPeriods2 traceAndPeriods in this.precedents.GetStorage())
                    {
                        if (traceAndPeriods.trace.type == ETraceType.Divider) continue;
                        traceAndPeriods.trace.DeepTrace(th, depth + 1);
                    }
                }
            }
        }

        /// <summary>
        /// Used in PushIntoSeries, for a .GluedToSeries trace type.
        /// Not used when viewing/printing traces: for this TimeShadow2() is used.
        /// The traceThatIsGoingToBeAdded is about to be added, shadowing earlier traces.
        /// You may set traceThatIsGoingToBeAdded = null, and is so nothing is added, and shadowing is done. But why would you do that?
        /// </summary>
        private void PrecedentsShadowing(Trace2 traceThatIsGoingToBeAdded)
        {
            // This pushes traceThatIsGoingToBeAdded onto existing traces, potentially cutting 
            // the existing traces into halves etc.
            //
            // When we already have shadowing, we have stuff like this
            //
            //            ==========                                   always 1 piece
            //       =====          =====                              1 or 2 pieces
            //  =====                    =====                         1-3 pieces
            //
            //  Maybe do a top-down search of a new piece. If the new piece is equal to or inside an existing piece
            //  at depth d, nothing is touched lower than d.
            //              
            // NOTE: We are only doing shadowing at depth = 0, at .GluedToSeries
            //       At that level (possibly also deeper), the .precedents lists of TraceAndPeriods cannot have dublets
            //       regarding the trace inside each list item. Therefore, the trace.id equality check later on is probably
            //       correct (instead of using ReferenceEquals).
            //        
            //
            //  x1 <2001 2010> = 01;   // 
            //  x1 <2003 2004> = 02;   //
            //  x1 <2007 2008> = 03;   //
            //  x1 <2006 2009> = 04;   // -->
            //
            //      1   2   3   4   5   6   7   8   9  10     NON-sorted list at the moment before == is added
            // 01  --  --          --  --          --  --
            // 02          --  -- 
            // 03                          --  --
            // 04                      ==  ==  ==  ==
            //
            //
            //      1   2   3   4   5   6   7   8   9  10    Sorted list (inverse, 01 is element 1) at the moment before == is added. The X are used for sorting.
            // 02          --  -x                 
            // 03                          --  -x 
            // 01  --  --          --  --          --  -x
            // 04                      ==  ==  ==  ==
            //
            // The sorted key regarding 01 changes from 6 to 5
            // If == includes 2005, the sorted items 01 and 02 are swapped.            

            this.precedents.RecreateSorted();  //cannot be omitted, may be called directly from a databank read.

            if (traceThatIsGoingToBeAdded != null)
            {
                int n = this.precedents.Count();
                //Could perhaps also have logic that works if the previous trace has a *larger* period than the new.
                //Then the larger trace is cut in 2 (potentially), but no more shadowing is necessary.
                //Probably not worth it though, now we have .storageSorted.
                if (n > 0)
                {
                    TraceAndPeriods2 tapLast = this.precedents.GetStorage()[n - 1];
                    if (!traceThatIsGoingToBeAdded.GetContents().period.t1.IsNull() && !traceThatIsGoingToBeAdded.GetContents().period.t2.IsNull() && tapLast.trace.GetContents().period.t1.EqualsGekkoTime(traceThatIsGoingToBeAdded.GetContents().period.t1) && tapLast.trace.GetContents().period.t2.EqualsGekkoTime(traceThatIsGoingToBeAdded.GetContents().period.t2))
                    {
                        // --- Remove from SortedSet --- 
                        SortedBagItem sbi = new SortedBagItem(tapLast.LastPeriod(), new TraceAndPeriods2(tapLast.trace, tapLast.periods));
                        bool success = this.precedents.GetStorageSorted().Remove(sbi);  //has O(log n), where RemoveWhere() has O(n).                        
                        if (!success) new Error("Trace: sorted set problem");  //remove this check after some time

                        // --- Add to SortedSet --- 
                        TraceAndPeriods2 tap = new TraceAndPeriods2();
                        tap.trace = traceThatIsGoingToBeAdded;
                        tap.periods = new GekkoTimeSpansSimple(new List<GekkoTimeSpanSimple>() { traceThatIsGoingToBeAdded.GetContents().period });
                        this.precedents.GetStorageSorted().Add(new SortedBagItem(tap.LastPeriod(), tap));

                        // --- Replace in unsorted list.  --- 
                        tapLast.trace = traceThatIsGoingToBeAdded;

                        if (this.precedents.GetStorage().Count() != this.precedents.GetStorageSorted().Count) new Error("Trace: counts do not match");
                        return;
                    }
                }
            }                 
                        
            if (this.type != ETraceType.GluedToSeries) new Error("Internal error: expected ETraceType.GluedToSeries");
            if (traceThatIsGoingToBeAdded == null) return;  //Not possible now?? would normally perform shadowing, but now everything is always up to date
            if (this.precedents.Count() != this.precedents.CountSorted())
            {
                new Error("Trace logic problem");
            }            

            bool mustUpdateSorted = false;

            if (this.precedents.CountSorted() > 0)
            {
                List<TraceAndPeriods2> addToSorted = new List<TraceAndPeriods2>();
                List<TraceAndPeriods2> removeInUnsorted = new List<TraceAndPeriods2>();
                List<SortedBagItem> removeInSorted = new List<SortedBagItem>();
                foreach (SortedBagItem sbi in this.precedents.GetStorageSorted())
                {
                    //Look at each TraceAndPeriods2 in sorted order (last end period first)
                    if (sbi.t.IsNull()) break;  //no shadowing for null-times, they are put last in the sorted set
                    if (traceThatIsGoingToBeAdded.GetContents().period.t1.StrictlyLargerThan(sbi.t)) break;  //not neccessary to look any further!

                    //Shadow
                    GekkoTimeSpansSimple newSpans = new GekkoTimeSpansSimple();
                    foreach (GekkoTimeSpanSimple span in sbi.tap.periods.GetStorage())
                    {
                        //The existing precedent may have several active periods
                        newSpans.AddRange(Trace2.TimeShadow1(traceThatIsGoingToBeAdded.GetContents().period, span));
                    }

                    if (newSpans.Count() == 0)
                    {
                        //kvp.tap must be removed from both .storage and .storageSorted
                        //must be removed
                        mustUpdateSorted = true;
                        removeInSorted.Add(sbi);
                        removeInUnsorted.Add(sbi.tap);

                    }
                    else if (!newSpans.GetStorage()[newSpans.Count() - 1].t2.EqualsGekkoTime(sbi.tap.LastPeriod()))
                    {
                        //It must be removed and added again because the last t2 of the spans changes
                        mustUpdateSorted = true;
                        removeInSorted.Add(sbi);
                        addToSorted.Add(sbi.tap);
                    }
                    sbi.tap.periods = newSpans;  //Cannot be omitted here, this sbi is not sure to be removed later on.
                }

                if (removeInUnsorted.Count > 0)
                {
                    //This actually ought to be pretty fast
                    //The list removeInUnsorted is usually small, if not too many existing traces are shadowed.
                    //And because the list is small, it is pretty fast to see if it contains a TraceAndPeriods2 object.
                    //It uses object reference compare, comparing object identity. That is fast.
                    List<TraceAndPeriods2> temp = new List<TraceAndPeriods2>();

                    int count = 0;
                    foreach (TraceAndPeriods2 tap in this.precedents.GetStorage())  //loop through items that are filtered
                    {
                        bool filter = false;
                        foreach (TraceAndPeriods2 remove in removeInUnsorted)       //loop through filter items
                        {                            
                            //????????????????????????????????????????????????????????????????
                            // Is this ok for identity? What about the periods? ReferenceEquals will not do (may be written and read --> new objects)
                            //????????????????????????????????????????????????????????????????                         
                            if (tap.trace.GetId().Equals(remove.trace.GetId()))
                            {
                                filter = true;
                                break;
                            }
                        }

                        if (filter)
                        {
                            count++;  //we like to check there is only 1!!
                        }
                        else
                        {
                            temp.Add(tap);
                        }
                    }
                    if (count != removeInUnsorted.Count)
                    {
                        new Error("Precedents shadowing problem");
                    }
                    this.precedents.SetStorage(temp);
                }

                if (mustUpdateSorted)
                {                    
                    foreach (SortedBagItem sbi in removeInSorted)
                    {
                        this.precedents.GetStorageSorted().Remove(sbi);  //has O(log n), where RemoveWhere() has O(n).
                    }
                    
                    if (addToSorted.Count > 0)
                    {                        
                        foreach (TraceAndPeriods2 tap in addToSorted)
                        {
                            this.precedents.GetStorageSorted().Add(new SortedBagItem(tap.LastPeriod(), tap));
                        }
                    }
                }

                if (this.precedents.GetStorageSorted() != null && this.precedents.Count() != this.precedents.CountSorted())
                {
                    new Error("Trace logic problem");
                }
            }

            TraceAndPeriods2 tap5 = new TraceAndPeriods2();
            tap5.trace = traceThatIsGoingToBeAdded;            
            tap5.periods = new GekkoTimeSpansSimple(new List<GekkoTimeSpanSimple>() { traceThatIsGoingToBeAdded.GetContents().period });
            this.precedents.Add(tap5);
        }     

        public Trace2 DeepClone(int depth, CloneHelper cloneHelper)
        {
            if (cloneHelper == null) cloneHelper = new CloneHelper();  //often at depth==0, and if so, the dictionary resides here for all higher depths. That should be ok.
            
            object known = null;
            Trace2 trace2 = null;

            if (Program.options.bugfix_tracedepth != -1 && depth > Program.options.bugfix_tracedepth)
            {
                //do nothing: stop the possible infinite regress here
            }
            else
            {
                if (cloneHelper != null)
                {
                    cloneHelper.dict.TryGetValue(this, out known);
                }
                if (known == null)
                {
                    trace2 = new Trace2(this.type, this.traceContents);  //the .traceContents object is not cloned!                                
                    trace2.precedents = this.precedents?.DeepClone(depth + 1, cloneHelper);
                    if (cloneHelper != null)
                    {
                        if (cloneHelper.dict.ContainsKey(this))
                        {
                            //Should not normally happen unless cycles in graph
                            if (Globals.runningOnTTComputer) G.WarningInternal("TTH: Clone dict dublet problem");
                        }
                        else
                        {
                            cloneHelper.dict.Add(this, trace2);
                        }
                    }
                }
                else
                {
                    trace2 = known as Trace2;
                }
            }
            return trace2;
        }

        
        public string PrintStamp()
        {
            string s = null;
            //The stamp is in UTC time, so we ask for it in local time for printing on screen.
            s += this.GetId().StampInLocalTime().ToString("dd/MM/yyyy HH:mm:ss") + "|" + this.GetId().GetCounter();
            return s;
        }

        /// <summary>
        /// Returns .Parent if .contents == null.
        /// </summary>
        /// <returns></returns>
        public ETraceParentOrChild GetTraceType()
        {
            ETraceParentOrChild x = ETraceParentOrChild.Child;
            if (this.GetContents() == null) x = ETraceParentOrChild.Parent;
            return x;
        }

        public TwoStrings Text(int d)
        {            
            string s1 = null;
            string s2 = null;
            if (true)
            {
                s1 = this.GetContents().text;
                string period = this.GetContents().period.t1 + "-" + this.GetContents().period.t2;
                int len = "---".Length;
                if (s1 != null) len = s1.Length;
                s2 += G.Blanks(50 - len - 2 * d) + " --> period: " + period;
                //s2 += ", stamp: " + this.GetId().stamp.ToString("g", System.Globalization.CultureInfo.CreateSpecificCulture(Globals.languageDaDK));  //This is SLOOW!
                s2 += ", stamp: " + this.GetId().StampInLocalTime().ToString("g", System.Globalization.CultureInfo.GetCultureInfo(Globals.languageDaDK));
            }
            return new TwoStrings(s1, s2);
        }

        /// <summary>
        /// Type == NewParent ---> Puts the new trace on top of the series traces. Reconnects the existing series trace(s) to the new trace.
        /// Type == Sibling ---> Puts it among siblings. Removes the date(s) from its siblings.
        /// Param usesRealDataPeriod is not used for now, just a pointer for future fixes.
        /// </summary>
        /// <param name="ts"></param>
        public static void PushIntoSeries(Series ts, Trace2 traceThatIsGoingToBeAdded, ETracePushType type, bool usesRealDataPeriod)
        {
            //
            // !!!
            // !!! In the longer run, these IF's can be removed
            // !!!            
            if (traceThatIsGoingToBeAdded == null) new Error("Trace problem: trace == null");
            if (traceThatIsGoingToBeAdded.GetContents().text == null) new Error("Trace problem: trace.GetContents().text == null");
            if (ts.meta == null) new Error("Trace problem: ts.meta == null");
            
            if (ts.meta.trace2 == null) ts.meta.trace2 = new Trace2(ETraceType.GluedToSeries, ETraceParentOrChild.Parent);

            if (type == ETracePushType.Sibling)
            {
                //In something like "reset; y = 1; y = 2;" this is called 2 times.
                ts.meta.trace2.PrecedentsShadowing(traceThatIsGoingToBeAdded);
                //In unit tests, trace period (t1/t2) is always present here, so no null periods.
                if (traceThatIsGoingToBeAdded.traceContents.period.t1.IsNull()) G.WarningInternal("*** TTH: Trace problem #1: " + traceThatIsGoingToBeAdded.traceContents.text);                
            }
            else if (type == ETracePushType.NewParent)
            {
                //The idea regarding these probably is that it is a new object,
                //possibly copying/cloning stuff from another object.
                //For instance, where in the traces of x2 would you put the
                //fact that x2 has been renamed to x1 ??? Therefore, .NewParent for that, and null period.

                //In unit tests, trace period (t1/t2) is only null for:
                //  rename x1 as x2;
                //  copy x1 to x2;
                //  copy <2001 2002> x1 to x2;  //when after above. probably copy<respect>too --> is this root problem??
                //The others have periods, and typically also construct new objects.

                //reset; x = 1; rename x as y; rename y as z; trace2 z;   --> GOOD, no accumulation, 3 traces
                //reset; x = 1; copy x as y; copy x as y; trace2 y;       --> GOOD, no accumulation, 3 traces (but 2 of them are equal, so really only 2 traces, the first copy trace is gone)
                //reset; x = 1; copy < 2020 2021 > x as y; copy < 2020 2021 > x as y; trace2 y; --> also ok
                //reset; x = 1; copy<respect> x as y; copy<respect> x as y; trace2 y; --> BAD, ACCUMULATES
                //reset; x3 = 3; copy <2001 2002> x3 to x5; copy <2001 2002> x3 to x5; copy <2001 2002> x3 to x5; copy <2001 2002> x3 to x5; trace2 x5;  --> BAD ACCUMULATES 1 TIME too much

                //LOOK AT COPY, maybe in the cases where we copy PART of data (with <t1 t2> or <respect>) FROM a series into a NEW series or EXISTING series.
                //COPY<respect> accumulates worst, possibly because time is not detected --> should be, fix this first!
                
                //COPY x1 to x2; (period)
                //RENAME x1 as x2; (null)
                //COLLAPSE y!a = x1; (period)
                //INTERPOLATE... (period)
                //REBASE... (period)
                //SMOOTH... (period)
                //DOWNLOAD... (period)
                //TRUNCATE <2001q2 2002q3> y; (period)
                //SPLICE... (period)
                //READ <t1 t2> xx; (period)
                //READ xx; (period)

                traceThatIsGoingToBeAdded.AddRangeFromSeries2(null, ts);
                ts.meta.trace2.precedents = new Precedents2();
                TraceAndPeriods2 tap6 = new TraceAndPeriods2();
                tap6.trace = traceThatIsGoingToBeAdded;
                GekkoTimeSpansSimple gtss = new GekkoTimeSpansSimple();
                gtss.SetStorage(new List<GekkoTimeSpanSimple>() { tap6.trace.GetContents().period });  //should be ok to just add it here, because .GetContents().period never changes (is immutable anyway)
                tap6.periods = gtss;
                ts.meta.trace2.precedents.Add(tap6);
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
        public static GekkoTimeSpansSimple TimeShadow1(GekkoTimeSpanSimple newSpan, GekkoTimeSpanSimple oldSpan)
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

            GekkoTimeSpansSimple temp5 = new GekkoTimeSpansSimple();
            temp5.SetStorage(rv);
            return temp5;
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
                dict1Inverted[trace.GetId()] = trace;
                trace.precedents.ToID();  //remove links
            }
            foreach (SeriesMetaInformation meta in th.metas)
            {
                meta.ToID();
            }
            databank.traces = th.tracesDepth2.Keys.ToList();
        }        

        public static void PrintTraceHelper(Trace2 trace, bool all, Series ts)
        {
            int widthRemember = Program.options.print_width;
            Program.options.print_width = int.MaxValue;
            try
            {
                TraceHelper th = new TraceHelper();
                trace.DeepTrace(th, Globals.traceDeepStartDepth);
                int count2 = Trace2.CountWithoutInvisible(th.tracesDepth2);
                string s = "Traces";
                if (true)
                {
                    if (trace.precedents.Count() > 0)
                    {
                        Action<GAO> a = (gao) =>
                        {
                            TraceViewerInfo info = new TraceViewerInfo();
                            info.ts = ts;
                            info.n = count2;
                            CallTraceViewer(trace, int.MaxValue, info);
                        };
                        s += " (" + G.GetLinkAction("view " + count2, new GekkoAction(EGekkoActionTypes.Unknown, null, a)) + ")";
                    }
                }
                s += ":";
                G.Writeln(s);
                PrintTraceHelper(trace, 0);
            }
            finally
            {
                //resetting, also if there is an error
                Program.options.print_width = widthRemember;
            }
        }

        /// <summary>
        /// Starts up the data trace viewer. See also WalkTracesForHtml().
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="maxDepth"></param>
        /// <returns></returns>
        public static int CallTraceViewer(Trace2 trace, int maxDepth, TraceViewerInfo info)
        {
            // with graph = false: 2 --> 4, 3 --> 11, 4 --> 35, 5 --> 134, 6 --> 204, 7 --> 397, 8 --> 432, 9 --> 432
            // with graph = true:  2 --> 4, 3 --> 11, 4 --> 34, 5 --> 128, 6 --> 166, 7 --> 184, 8 --> 189, 9 --> 189

            // Items = disp = 188, new items = 432 (437)            

            int nn = 0;

            if (Globals.batchType == EBatchType.PyGekko || !G.IsUnitTestingOrNotShowingGUI())
            {
                Thread sta = new Thread(delegate ()
                {
                    WindowTreeViewWithTable w = CallTraceViewerHelper(trace, info);
                    w.Show();
                    System.Windows.Threading.Dispatcher.Run();
                });
                sta.SetApartmentState(ApartmentState.STA);
                sta.Start();
            }
            return nn;
        }

        private static WindowTreeViewWithTable CallTraceViewerHelper(Trace2 trace, TraceViewerInfo info)
        {
            Globals.itemCounter = 0;
            TreeGridModel model = new TreeGridModel();
            TraceItem temp = null;

            if (true)
            {
                TraceItem item = trace.FromTraceToTreeViewItem(null);
                //At startup, we need to get two levels in: depth=0 and depth=1.
                List<TraceAndPeriods2> taps1 = trace.TimeShadow2();
                if (taps1 != null && taps1.Count > 0)
                {
                    foreach (TraceAndPeriods2 tap1 in taps1)
                    {
                        if (!Program.options.databank_trace_divide && tap1.trace.type == ETraceType.Divider) continue;  //do not show dividers
                        Trace2 trace1 = tap1.trace;
                        TraceItem item1 = trace1.FromTraceToTreeViewItem(tap1.periods);
                        item.GetChildren().Add(item1);
                        ExpandTraceInTraceViewer(item1);

                        List<TraceAndPeriods2> taps2 = trace1.TimeShadow2();
                        if (taps2 != null && taps2.Count > 0)
                        {
                            foreach (TraceAndPeriods2 tap2 in taps2)
                            {
                                if (!Program.options.databank_trace_divide && tap2.trace.type == ETraceType.Divider) continue;  //do not show dividers
                                Trace2 trace2 = tap2.trace;
                                bool ignore = IgnoreNephew(item.trace.TimeShadow2(), trace1, trace2);
                                if (!ignore)
                                {
                                    TraceItem item2 = trace2.FromTraceToTreeViewItem(tap2.periods);
                                    item1.GetChildren().Add(item2);
                                    ExpandTraceInTraceViewer(item2);
                                }
                            }
                        }
                        if (item1.GetChildren().Count == 0) item1.HasChildren = false;
                        else item1.HasChildren = true;
                    }
                }
                temp = item;
            }

            foreach (TraceItem item in temp.GetChildren())
            {
                model.Add(item);
            }

            WindowTreeViewWithTable w = new WindowTreeViewWithTable(model, info);
            Globals.windowsTrace.Add(w);
            w.text.Background = new System.Windows.Media.SolidColorBrush(G.Lighter(Globals.GekkoModeYellow, 0.70));  //this.scrollViewerFind.Background = new SolidColorBrush(G.Lighter(Globals.GekkoModeYellow, 0.70));                    
            string v = null;
            if (trace.GetContents() != null && trace.GetContents().name != null) v = G.Chop_RemoveBank(trace.GetContents().name, Program.databanks.GetFirst().name) + " - ";
            w.Title = v + "Gekko data-trace";
            return w;
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
                if (trace2.IsInvisibleTrace()) continue;
                n++;
            }
            return n;
        }

        private static void PrintTraceHelper(Trace2 trace, int d)
        {
            if (d > 1) return;
            string s = null;            
            s = "| ";

            List<TraceAndPeriods2> taps = trace.TimeShadow2();

            int max = 5;
            int n = 0;
            foreach (TraceAndPeriods2 tap in taps)
            {
                if (tap.trace.type == ETraceType.Divider) continue;
                n++;
                if (n > max)
                {
                    G.Writeln("| ...see older traces in trace viewer...", System.Drawing.Color.Gray);
                    break;
                }
                string code = null; string codeDetailed = null;
                Trace2.GetCodeAsString(tap.trace.GetContents().text, out code, out codeDetailed);
                string active = null; string activeDetailed = null;
                Trace2.GetActivePeriodsAsString(tap.periods, ref active, ref activeDetailed);
                string stamp = null; string stampDetailed = null;
                Trace2.GetStampAsString(tap.trace.GetId(), out stamp, out stampDetailed);
                G.Write("| " + code); G.Writeln(G.Blanks(50 - tap.trace.GetContents().text.Length) + " --> " + activeDetailed + ", " + stamp, Globals.MiddleGray);
            }            
        }

        /// <summary>
        /// From a TraceItem (which stems from a Trace), this extracts information (detailed) to show at the
        /// bottom of the trace viewer, or similarly in the html browser. In this string, there are no
        /// size restrictions, and the "detailed" fields of TraceItem are used.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string FromTraceItemToDetailedText(TraceItem item, bool html)
        {
            string text = null;
            text = item.CodeDetailed + G.NL;
            if (html) text += " -------------------------------------------------------------------------------------------------- " + G.NL;
            else text += " ---------------------------------------------- " + G.NL;
            text += "Name: " + item.NameDetailed;
            if (!G.NullOrBlanks(item.Label)) text += " ('" + item.Label + "')";
            text += G.NL;
            text += "Period: " + item.Period + ", Active: " + item.ActiveDetailed + G.NL;            
            text += "File: " + item.FileDetailed + G.NL;
            text += "Stamp: " + item.StampDetailed + G.NL;
            if (item.PrecedentsNames != null && item.PrecedentsNames.Count > 0) { text += "Vars: " + Stringlist.GetListWithCommas(item.PrecedentsNames); }
            return text;
        }

        public static void ExpandTraceInTraceViewer(TraceItem item)
        {
            //
            //             item
            //            /    \
            //           /      
            //       childItem              (childTrace)
            //          /    \
            //         /      
            //   grandChildTrace
            //
            // We are expanding the childItem. We want to see if -- inside the same divider block -- a sibling to childTrace
            // has same id as the grandChildTrace. If so, kill it.
            //

            foreach (TraceItem childItem in item.GetChildren()) //is already expanded, else .TimeShadow2() would be used.
            {                
                Trace2 childTrace = childItem.trace;
                if (childTrace.type == ETraceType.Divider) continue; //dividers are not shown                

                List<TraceAndPeriods2> grandChildrenTraces = childTrace.TimeShadow2();
                if (grandChildrenTraces != null && childItem.GetChildren().Count == 0) //.Count will be > 0 if it has been expanded already previously. If so, we avoid putting in dublets.
                {
                    foreach (TraceAndPeriods2 grandChildTrace in grandChildrenTraces)
                    {
                        if (!Program.options.databank_trace_divide && grandChildTrace.trace.type == ETraceType.Divider) continue; //dividers are not shown
                        bool ignore = IgnoreNephew(item.trace.TimeShadow2(), childTrace, grandChildTrace.trace);
                        if (!ignore)
                        {                            
                            TraceItem itemGrandChild = grandChildTrace.trace.FromTraceToTreeViewItem(grandChildTrace.periods);
                            childItem.GetChildren().Add(itemGrandChild);
                        }
                    }
                }

                if (childItem.GetChildren().Count == 0) childItem.HasChildren = false;
                else childItem.HasChildren = true;
            }
        }

        /// <summary>
        /// If a newphew trace has an "uncle" (a sibling to a parent (or the parent) inside the same divider), it may be ignored since it is already shown.
        /// </summary>
        /// <param name="childTraceSiblings"></param>
        /// <param name="childTrace"></param>
        /// <param name="grandChildTrace"></param>
        /// <returns></returns>
        private static bool IgnoreNephew(List<TraceAndPeriods2> childTraceSiblings, Trace2 childTrace, Trace2 grandChildTrace)
        {
            if (Program.options.databank_trace_dublets) return false;
            bool ignore = false;
            if (childTraceSiblings == null) return false; //cannot evaluate
            if (!Program.options.databank_trace_dublets)
            {                
                List<List<TraceAndPeriods2>> xChildTracesDivided = Trace2.SplitDividers(childTraceSiblings);
                foreach (List<TraceAndPeriods2> xChildTracesChunk in xChildTracesDivided)
                {
                    bool isRightChunk = false;
                    bool isDublet = false;
                    foreach (TraceAndPeriods2 xChildTrace in xChildTracesChunk)
                    {
                        if (xChildTrace.trace.GetContents().id == childTrace.GetContents().id)
                        {
                            isRightChunk = true;
                        }
                        if (xChildTrace.trace.GetContents().id == grandChildTrace.GetContents().id)
                        {
                            isDublet = true;
                        }
                    }
                    if (isRightChunk && isDublet)
                    {
                        ignore = true;
                        break;
                    }
                    else if (isDublet)
                    {
                        G.WarningInternal("Trace problem #2: Invalid dublet!");
                    }
                }
            }
            return ignore;
        }

        /// <summary>
        /// Splits a list of TraceAndPeriods2 into portions according to dividers. Returning object has Count == 0 if input is null or has Count == 0.
        /// </summary>
        /// <param name="xChildTraces"></param>
        /// <returns></returns>
        public static List<List<TraceAndPeriods2>> SplitDividers(List<TraceAndPeriods2> xChildTraces)
        {
            List<List<TraceAndPeriods2>> divided = new List<List<TraceAndPeriods2>>();
            if (xChildTraces == null) return divided;
            List<TraceAndPeriods2> current = null;
            int counter = -1;
            int dividerCounter = 0;
            foreach (TraceAndPeriods2 xChildTrace in xChildTraces)
            {
                counter++;
                if (counter == 0) current = new List<TraceAndPeriods2>();
                if (xChildTrace.trace.type == ETraceType.Divider)
                {
                    dividerCounter++;
                    if (counter == 0 || counter == xChildTraces.Count - 1) new Error("Divider problem"); //TODO TODO TODO remove this check for Gekko 4.0
                    divided.Add(current);
                    current = new List<TraceAndPeriods2>();
                }
                else
                {
                    current.Add(xChildTrace);
                }
            }
            if (current != null) divided.Add(current);
            if (true)
            {                
                //TODO TODO TODO remove this check for Gekko 4.0
                int n = 0;
                foreach (List<TraceAndPeriods2> xx in divided)
                {
                    n += xx.Count;
                }
                if (xChildTraces.Count != n + dividerCounter) new Error("Divider problem");
            }
            return divided;
        }

        public TraceItem FromTraceToTreeViewItem(GekkoTimeSpansSimple periods)
        {           

            // =========================================================================
            // Settings for the data trace viewer
            // =========================================================================
            string showFreq = "maybe";  //"yes", "no", "maybe
            string showDatabank = "maybe";  //"yes", "no", "maybe"
            string nullName = "-----";  //does not work well...
            // Also Globals.showDividers and Program.options.databank_trace_trim;
            // =========================================================================
                                                                         
            bool hasChildren = false;
            if (this.precedents != null && this.precedents.Count() > 0) hasChildren = true;
            string name = nullName;
            string nameDetailed = nullName;
            string code = nullName;
            string codeDetailed = nullName;
            string period = null;
            string active = null;
            string activeDetailed = null;
            string file = null;
            string fileDetailed = null;
            string stamp = null;
            string stampDetailed = null;
            string label = null;
            List<string> precedentsNames = null;

            if (this.GetContents() != null)
            {
                //Note: we always remove bank name, since this is often irrelevant. Freq is removed if same as current freq.
                if (this.GetContents().name != null)
                {
                    name = G.Chop_RemoveFreq(G.Chop_RemoveBank(this.GetContents().name), Program.options.freq);
                    nameDetailed = G.Chop_RemoveBank(this.GetContents().name);
                }
                GetCodeAsString(this.GetContents().text, out code, out codeDetailed);
                GekkoTime t1 = GekkoTime.tNull;
                GekkoTime t2 = GekkoTime.tNull;
                if (this.GetContents().period != null)
                {
                    t1 = this.GetContents().period.t1;
                    t2 = this.GetContents().period.t2;
                }
                if (t1.IsNull() && t2.IsNull()) period = "[null]";
                else period = "" + t1.ToString() + "-" + t2.ToString() + "";
                GetActivePeriodsAsString(periods, ref active, ref activeDetailed);

                int counter = 0;
                if (!G.NullOrBlanks(this.GetContents().commandFileAndLine))
                {
                    string[] ss = this.GetContents().commandFileAndLine.Split('¤');
                    file = System.IO.Path.GetFileName(ss[0]) + " line " + ss[1];
                    fileDetailed = ss[0] + " line " + ss[1];
                }
                if (!G.NullOrBlanks(this.GetContents().dataFile)) file += " (data = " + System.IO.Path.GetFileName(this.GetContents().dataFile) + ")";
                if (!G.NullOrBlanks(this.GetContents().dataFile)) fileDetailed += " (data = " + this.GetContents().dataFile + ")";
                Trace2.GetStampAsString(this.GetId(), out stamp, out stampDetailed);
                if (this.type == ETraceType.Divider)
                {
                    stamp = null; stampDetailed = null;                    
                }
                else
                {
                    label = SearchForLabelInOpenDatabanks(nameDetailed);
                }
                if (this.GetContents().precedentsNames != null) precedentsNames = GetPrecedentsNames(showFreq, showDatabank);                                
            }

            TraceItem newItem = new TraceItem(name, nameDetailed, code, codeDetailed, period, active, activeDetailed, stamp, stampDetailed, file, fileDetailed, label, precedentsNames, hasChildren);
            newItem.trace = this;
            return newItem;
        }

        /// <summary>
        /// For a given variable name (without databank but possibly with freq), Gekko tries to find a timeseries in one
        /// of the open databanks (including Ref) that has as its direct trace children a trace with the same trace ID as
        /// the 'this' object. If so, the timeseries label is returned. May return null.
        /// 
        /// </summary>
        /// <param name="nameWithFreq"></param>
        /// <returns></returns>
        private string SearchForLabelInOpenDatabanks(string nameWithFreq)
        {
            string label = null;
            try
            {
                if (nameWithFreq != null)
                {
                    //Why should it ever be == null, and why try... (fix Gekko 4.0)                    
                    foreach (Databank db in Program.databanks.storage)
                    {
                        if (db == null) continue;  //ever so?
                        string name2 = G.Chop_SetBank(nameWithFreq, db.GetName());
                        Series ts = O.GetIVariableFromString(name2, O.ECreatePossibilities.NoneReturnNullAlways) as Series;
                        if (ts?.meta?.trace2 != null)
                        {
                            foreach (TraceAndPeriods2 tap in ts.meta.trace2.GetPrecedents_BewareOnlyInternalUse().GetStorage())
                            {
                                if (tap.trace.GetId() == this.GetId())
                                {
                                    // Object.ReferenceEquals(tap.trace, this) may return false if object has been cloned: testing Id's is best here
                                    label = ts.meta.label;
                                    return label;
                                }
                            }
                        }
                    }
                }
            }
            catch { };
            return label;
        }

        public static void GetCodeAsString(string text, out string code, out string codeDetailed)
        {
            codeDetailed = text;
            code = null;
            if (codeDetailed != null) code = System.Text.RegularExpressions.Regex.Replace(codeDetailed, @"\s+", " "); //https://stackoverflow.com/questions/206717/how-do-i-replace-multiple-spaces-with-a-single-space-in-c                
        }

        public static void GetStampAsString(TraceID2 id, out string stamp, out string stampDetailed)
        {
            //The .stamp is in UTC time, so needs to be converted for printing
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo(Globals.languageDaDK);
            stamp = id.StampInLocalTime().ToString("d", ci);
            try
            {
                //stampDetailed = id.StampInLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fffffff", System.Globalization.CultureInfo.GetCultureInfo(Globals.languageDaDK)) + ", #" + id.counter;  //7 digits is 100 ns, which is limit anyway                
                stampDetailed = id.StampInLocalTime().ToString($"{ci.DateTimeFormat.ShortDatePattern} HH:mm:ss.fffffff", System.Globalization.CultureInfo.GetCultureInfo(Globals.languageDaDK)) + ", #" + id.GetCounter();  //7 digits is 100 ns, which is limit anyway
                //
            }
            catch
            {
                stampDetailed = id.StampInLocalTime().ToString("G", System.Globalization.CultureInfo.GetCultureInfo(Globals.languageDaDK)) + ", #" + id.GetCounter();
            }
        }

        public static void GetActivePeriodsAsString(GekkoTimeSpansSimple periods, ref string active, ref string activeDetailed)
        {
            int n = -1;
            if (periods != null)
            {
                foreach (GekkoTimeSpanSimple gts in periods.GetStorage())
                {
                    n++;
                    if (n > 0) active += ", ";
                    if (n > 0) activeDetailed += ", ";
                    string s = gts.t1.ToString() + "-" + gts.t2.ToString();
                    if (s == "[null]-[null]") s = "[null]";
                    if (n <= 1)
                    {
                        active += s;
                    }
                    else
                    {
                        active += "...";
                    }
                    activeDetailed += s;
                }
            }
        }

        /// <summary>
        /// May return empty list, but not null. Args are "yes", "no" or "maybe"
        /// </summary>
        /// <param name="showFreq"></param>
        /// <param name="showDatabank"></param>
        /// <returns></returns>
        public List<string> GetPrecedentsNames(string showFreq, string showDatabank)
        {
            List<string> precedentsNames;
            List<string> list = new List<string>();

            if (this.GetContents().precedentsNames != null)
            {
                foreach (string s in this.GetContents().precedentsNames)
                {
                    string type = s.Substring(0, 1);
                    string name = s.Substring(2);
                    //See #9khsigra7ioau regarding 8 types

                    bool removeBank = false;
                    bool removeFreq = false;

                    //We are not currently using whether it has trace or not (1, 2, 5, 6).
                    //But that info may become useful later on.

                    if (type == "1")
                    {
                        //has trace
                        //is first-position databank  
                        //is current frequency
                        if (G.Equal(showDatabank, "no") || G.Equal(showDatabank, "maybe")) removeBank = true;
                        if (G.Equal(showFreq, "no") || G.Equal(showFreq, "maybe")) removeFreq = true;
                    }
                    else if (type == "2")
                    {
                        //has trace
                        //is first-position databank  
                        //is "other" frequency
                        if (G.Equal(showDatabank, "no") || G.Equal(showDatabank, "maybe")) removeBank = true;
                        if (G.Equal(showFreq, "no")) removeFreq = true;
                    }
                    else if (type == "3")
                    {
                        //has trace
                        //is "other" open databank
                        //is current frequency
                        if (G.Equal(showDatabank, "no")) removeBank = true;
                        if (G.Equal(showFreq, "no") || G.Equal(showFreq, "maybe")) removeFreq = true;
                    }
                    else if (type == "4")
                    {
                        //has trace
                        //is "other" open databank
                        //is "other" frequency
                        if (G.Equal(showDatabank, "no")) removeBank = true;
                        if (G.Equal(showFreq, "no")) removeFreq = true;
                    }
                    else if (type == "5")
                    {
                        //has no trace
                        //is first-position databank  
                        //is current frequency
                        if (G.Equal(showDatabank, "no") || G.Equal(showDatabank, "maybe")) removeBank = true;
                        if (G.Equal(showFreq, "no") || G.Equal(showFreq, "maybe")) removeFreq = true;
                    }
                    else if (type == "6")
                    {
                        //has no trace
                        //is first-position databank  
                        //is "other" frequency
                        if (G.Equal(showDatabank, "no") || G.Equal(showDatabank, "maybe")) removeBank = true;
                        if (G.Equal(showFreq, "no")) removeFreq = true;
                    }
                    else if (type == "7")
                    {
                        //has no trace
                        //is "other" open databank
                        //is current frequency
                        if (G.Equal(showDatabank, "no")) removeBank = true;
                        if (G.Equal(showFreq, "no") || G.Equal(showFreq, "maybe")) removeFreq = true;
                    }
                    else if (type == "8")
                    {
                        //has no trace
                        //is "other" open databank
                        //is "other" frequency
                        if (G.Equal(showDatabank, "no")) removeBank = true;
                        if (G.Equal(showFreq, "no")) removeFreq = true;
                    }

                    if (removeBank) name = G.Chop_RemoveBank(name);
                    if (removeFreq) name = G.Chop_RemoveFreq(name);

                    list.Add(name);
                }
            }
            precedentsNames = list;
            return precedentsNames;
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
        private readonly long counter = ++Globals.traceCounter;

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

        public long GetCounter()
        {
            return this.counter;
        }


        public override bool Equals(object o)
        {
            TraceID2 other = o as TraceID2;
            if (other != null && this.stamp == other.stamp && this.counter == other.counter) return true;
            return false;
        }
        public override string ToString()
        {
            return this.StampInLocalTime().ToString() + "|" + this.counter;  //We want this printed in local time, not UTC time.
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.stamp.GetHashCode();  //No need to use .ToLocalTime() here: we just hash the the global ("true" and common) UTC time.
            hash = hash * 31 + this.counter.GetHashCode();
            return hash;
        }
    }    

    public class TraceHelper
    {
        public ETraceHelper type = ETraceHelper.GetAllMetasAndTraces;
        public double scramble = double.NaN;  //for scramble() function
        public int seriesObjectCount = 0; //number of series found (probably often equal to meta count)
        public List<SeriesMetaInformation> metas = new List<SeriesMetaInformation>();
        public int depthLimit = -12345;
        
        // --- the following is for stats etc. ("real" traces)        
        public int unittestTraceCountIncludeInvisible = 0; //will include combinations, traces will not
        public Dictionary<Trace2, Precedents2> traces = new Dictionary<Trace2, Precedents2>();  //value is parent (may be null)
                
        // --- gbk write/read and other stuff
        //Hmm, isn't Precedents already a part of the key? Anyway, the depth needs to be inside an object anyway to be altered.
        public Dictionary<Trace2, PrecedentsAndDepth> tracesDepth2 = new Dictionary<Trace2, PrecedentsAndDepth>();
        //
        // --- this is for time-shadowing        
        public Dictionary<Trace2, string> timeShadowing = new Dictionary<Trace2, string>();

        // --- this is for traceadam2()
        public TraceBankHelpler helper = null;

        public bool specialAdamStuff = false;

        /// <summary>
        /// Depth of traces. Returns -1 if no traces are found.
        /// </summary>
        /// <returns></returns>
        public int MaxDepth()
        {
            int depth = -1;
            foreach (PrecedentsAndDepth pad in tracesDepth2.Values)
            {
                depth = Math.Max(depth, pad.depth);
            }
            return depth;
        }
    }

    public class PrecedentsAndDepth
    {
        public Precedents2 precedents = null;
        public int depth = 0;
    }

    /// <summary>
    /// Is basically a List&lt;TraceAndPeriods2>.
    /// </summary>
    [ProtoContract]
    public class Precedents2
    {        
        [ProtoMember(1)]
        private List<TraceAndPeriods2> storage = null;

        /// <summary>
        /// Pretty innocuous: using this, we can set .storage = null before protobuf.
        /// </summary>
        [ProtoMember(2)]
        public List<TraceID2> storageIDTemporary = null;  //used to recreate connections after protobuf. Will not take up space in general. Same size as .storagePeriodsTemporary

        /// <summary>
        /// Pretty innocuous: using this, we can set .storage = null before protobuf.
        /// </summary>
        [ProtoMember(3)]
        public List<GekkoTimeSpansSimple> storagePeriodsTemporary = null;  //used to recreate connections after protobuf. Will not take up space in general. Same size as .storageIDTemporary

        /// <summary>
        /// This is filled whenever the precedents are used.
        /// When not null, it has same size as .storage.
        /// Used to loop through to find traces with end dates >= new trace start date, so that the existing traces are possibly shadowed.
        /// Note: items are in reverse GekkoTime order, so traces with high ending dates are first.
        /// It seems the tree is a self-balancing red-black tree, with O(log n) for insert, delete, and lookup.        /// 
        /// </summary>        
        private SortedSet<SortedBagItem> storageSorted = null;

        /// <summary>
        /// The .storageSorted binary tree is not kept in protobuf, but is a "helper" tree that helps .storage identifying
        /// trace periods that are so small that they are not affected by a new trace. Using the binary tree avoids searching
        /// the whole .storage list in a linear fashion. This will become important when -- in time -- the tracing 
        /// lists grow in volume. So better to get it implemented and tested now.
        /// </summary>
        public void RecreateSorted()
        {
            if (this.storage != null && this.storageSorted == null)
            {
                this.storageSorted = new SortedSet<SortedBagItem>(new SortedBagComparer());
                foreach (TraceAndPeriods2 tap in this.storage)
                {
                    GekkoTime tMax = tap.LastPeriod();
                    this.storageSorted.Add(new SortedBagItem(tMax, tap));
                }
                if (this.Count() != this.storageSorted.Count)
                {
                    new Error("Trace logic problem");
                }
            }
        }        

        /// <summary>
        /// Add into precedents.storage. Be careful that something like trace.GetPrecedents_BewareOnlyInternalUse().AddRange(ts.meta.trace2.GetPrecedents_BewareOnlyInternalUse()) may
        /// fail if ts.meta.trace2 is == null. In cases like that, better to use trace.GetPrecedents_BewareOnlyInternalUse().AddRangeFromSeries(ts).
        /// </summary>
        /// <param name="precedents"></param>
        public void AddRange(Precedents2 precedents)
        {            
            if (precedents.storage != null && precedents.Count() > 0)
            {
                if (this.storage == null) this.storage = new List<TraceAndPeriods2>();
                foreach (TraceAndPeriods2 tap in precedents.storage)
                {
                    this.Add(tap); //also updates .storageSorted
                }
            }
        }        

        /// <summary>
        /// Add a Trace to precedents list. Cannot add a "meta entry" to a Trace. These can only be set for .trace in SeriesMetaInformation objects.
        /// </summary>
        /// <param name="traceAndPeriods"></param>
        /// <exception cref="GekkoException"></exception>
        public void Add(TraceAndPeriods2 traceAndPeriods)
        {            
            if (traceAndPeriods.trace.type != ETraceType.Divider && traceAndPeriods.trace.GetContents() == null) throw new GekkoException();
            if (this.storage == null)
            {
                //Does this ever happen? YES!
                this.storage = new List<TraceAndPeriods2>();
                this.storageSorted = new SortedSet<SortedBagItem>(new SortedBagComparer());
            }
            this.RecreateSorted();
            this.storage.Add(traceAndPeriods);
            this.storageSorted.Add(new SortedBagItem(traceAndPeriods.LastPeriod(), traceAndPeriods));
        }

        /// <summary>
        /// Only for iterators! REMEMBER to put an "if (xxx.precedents.Count() > 0) {... " before iterating!!
        /// </summary>
        /// <returns></returns>
        public List<TraceAndPeriods2> GetStorage()
        {
            return this.storage;
        }

        /// <summary>
        /// Only for iterators! REMEMBER to put an "if (xxx.precedents.CountSorted() > 0) {... " before iterating!!
        /// </summary>
        /// <returns></returns>
        public SortedSet<SortedBagItem> GetStorageSorted()
        {
            return this.storageSorted;
        }

        /// <summary>
        /// Use this with care. Beware that this method may set .storageSorted = null.
        /// </summary>
        /// <param name="m"></param>
        public void SetStorage(List<TraceAndPeriods2> m)
        {
            if (m != null && m.Count == 0) this.storage = null; //so it does not take up space
            else this.storage = m;
        }

        public void InitWithEmptyList()
        {
            this.storage = new List<TraceAndPeriods2>();
            this.storageSorted = null;
        }

        public  void ToID()
        {
            this.storageIDTemporary = new List<TraceID2>();
            this.storagePeriodsTemporary = new List<GekkoTimeSpansSimple>();
            if (this.Count() > 0)
            {
                foreach (TraceAndPeriods2 traceAndPeriods in this.GetStorage())
                {
                    TraceID2 temp = null;
                    GekkoTimeSpansSimple temp2 = new GekkoTimeSpansSimple();  //protobuf cannot handle if an element is == null (for dividers)
                    if (traceAndPeriods.trace.type == ETraceType.Divider)
                    {
                        temp = new TraceID2(DateTime.MinValue, long.MinValue);  //negative, signals null
                    }
                    else
                    {
                        temp = traceAndPeriods.trace.GetId();
                        temp2 = traceAndPeriods.periods;
                    }
                    this.storageIDTemporary.Add(temp);                    
                    this.storagePeriodsTemporary.Add(temp2);
                }
            }
            this.SetStorage(null);  //breaks the references
        }

        public void FromID(Dictionary<TraceID2, Trace2> dict2)
        {
            if (this.storageIDTemporary != null && this.storageIDTemporary.Count > 0)
            {
                this.storage = new List<TraceAndPeriods2>();
                for (int i = 0; i < this.storageIDTemporary.Count; i++)
                {
                    TraceID2 id = this.storageIDTemporary[i];
                    GekkoTimeSpansSimple periods = this.storagePeriodsTemporary[i];

                    if (id.GetCounter() == long.MinValue)
                    {
                        this.storage.Add(new TraceAndPeriods2(new Trace2(ETraceType.Divider, true), Globals.traceNullPeriods));
                    }
                    else
                    {
                        if (id.GetCounter() < 0) new Error("This trace is not stored in the databank, but has been pruned off: " + id.ToString());
                        Trace2 trace = null; dict2.TryGetValue(id, out trace);
                        if (trace == null) new Error("Could not find this trace in databank: " + id.ToString());
                        this.storage.Add(new TraceAndPeriods2(trace, periods));
                    }
                }
            }
            this.storageIDTemporary = null;
            this.storagePeriodsTemporary = null;
        }

        /// <summary>
        /// If null, will return 0.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            if (this.storage == null) return 0;
            return this.storage.Count;
        }

        /// <summary>
        /// If null will return 0. Should always return the same as Count().
        /// </summary>
        /// <returns></returns>
        public int CountSorted()
        {
            if (this.storageSorted == null) return 0;
            return this.storageSorted.Count;
        }

        public TraceAndPeriods2 this[int i]
        {
            get { return this.storage[i]; }
            set { this.storage[i] = value; }
        }

        public Precedents2 DeepClone(int depth, CloneHelper cloneHelper)
        {
            if (cloneHelper == null) cloneHelper = new CloneHelper();  //often at depth==0, and if so, the dictionary resides here for all higher depths. That should be ok.
            Precedents2 precedents = new Precedents2();            
            if (this.storage != null)
            {
                precedents.storage = new List<TraceAndPeriods2>();
                foreach (TraceAndPeriods2 traceAndPeriods in this.storage)
                {
                    precedents.storage.Add(traceAndPeriods.DeepClone(depth + 1, cloneHelper));
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
        private GekkoTimeSpansSimple storage = null;

        /// <summary>
        /// Beware: only for iterating.
        /// </summary>
        /// <returns></returns>
        public GekkoTimeSpansSimple GetStorage()
        {
            return this.storage;
        }

        public GekkoTimeSpanSimple this[int i]
        {
            get { return this.storage.GetStorage()[i]; }
            set { this.storage.GetStorage()[i] = value; }
        }

        /// <summary>
        /// Use with care
        /// </summary>
        /// <param name="storage"></param>
        public void SetStorage(GekkoTimeSpansSimple storage)
        {
            this.storage = storage;
        }

        /// <summary>
        /// Beware: use with care.
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            this.storage = new GekkoTimeSpansSimple();
        }

        public void Add(GekkoTimeSpanSimple x)
        {
            if (storage == null) storage = new GekkoTimeSpansSimple();
            this.storage.Add(x);
        }

        public int Count()
        {
            if (this.storage == null) return 0;
            return this.storage.Count();
        }
    }

    /// <summary>
    /// Only used for reporting, to know how periods shadow each other.
    /// </summary>
    [ProtoContract]
    public class TraceAndPeriods2
    {
        //At the moment, periods are just == null here, but in the longer run we can store them.
        //Other fields like min and max period could also be added. But wait, that is just t1 from first period
        //and t2 from last period. The periods are successive, no?
        
        [ProtoMember(1)]
        public Trace2 trace = null;

        [ProtoMember(2)]
        public GekkoTimeSpansSimple periods = null;

        //public long id = 0;  //just used to identify objects between sorted and unsorted list

        public TraceAndPeriods2()
        {
            //for protobuf
            //this.id = Globals.traceCounter2++;            
        }

        public TraceAndPeriods2(Trace2 trace, GekkoTimeSpansSimple periods)
        {
            this.trace = trace;
            this.periods = periods;
            //this.id = Globals.traceCounter2++;
        }

        /// <summary>
        /// Returns that last GekkoTime convered in all the periods. May return GekkoTime.tNull.
        /// </summary>
        /// <returns></returns>
        public GekkoTime LastPeriod()
        {
            if (this.periods == null || this.periods.Count() == 0) return GekkoTime.tNull;
            return this.periods.GetStorage()[this.periods.Count() - 1].t2;
        }

        public TraceAndPeriods2 DeepClone(int depth, CloneHelper cloneHelper)
        {
            if (cloneHelper == null) cloneHelper = new CloneHelper();  //often at depth==0, and if so, the dictionary resides here for all higher depths. That should be ok.
            GekkoTimeSpansSimple gtss = null;
            if (this.periods != null)
            {
                gtss = new GekkoTimeSpansSimple();
                gtss.AddRange(this.periods);  //the timespans themselves are immutable
            }
            return new TraceAndPeriods2(this.trace.DeepClone(depth + 1, cloneHelper), gtss);
        }
    }

    public class SortedBagItem  //Comparer: #kjhahaiuoslkfd
    {
        public GekkoTime t = GekkoTime.tNull;
        public TraceAndPeriods2 tap;

        public SortedBagItem(GekkoTime t, TraceAndPeriods2 tap) 
        {
            this.t = t;
            this.tap = tap;
        }    
    }

    public class SortedBagComparer : IComparer<SortedBagItem>  //Object: #kjhahaiuoslkfd
    {
        public int Compare(SortedBagItem x, SortedBagItem y)
        {
            if (Object.ReferenceEquals(x, y))
            {
                return 0;
            }
            else if (x.t.EqualsGekkoTime(y.t))
            {
                //add some salt
                if (x.tap.trace.GetId().GetCounter() == y.tap.trace.GetId().GetCounter()) return 0; //will probably not happen because of ReferenceEquals() at the top
                if (x.tap.trace.GetId().GetCounter() > y.tap.trace.GetId().GetCounter()) return 1; //just random, could just as well be inverse
                else return -1; //just random, could just as well be inverse
            }
            else
            {
                if (x.t.IsNull()) return 1;  //will put x later than y
                if (y.t.IsNull()) return -1; //will put x before y
                int i = x.t.CompareTo(y.t);
                if (i == 0) new Error("Compare error");
                return -i; //GekkoTimes are in reverse order
            }
        }
    }


    /// <summary>
    /// Column based so we can later use Microsoft.data.analysis
    /// </summary>
    public class TraceFrame
    {
        //Must correspond to #qwldak7dad
        public List<long> counter = new List<long>();
        public List<DateTime> stamp = new List<DateTime>();
        public List<string> period_start = new List<string>();
        public List<string> period_end = new List<string>();
        public List<DateTime?> date_start = new List<DateTime?>();
        public List<DateTime?> date_end = new List<DateTime?>();
        public List<string> name = new List<string>();
        public List<string> text = new List<string>();
        public List<string> precedentsNames = new List<string>();
        public List<string> commandFile = new List<string>();
        public List<int> commandLine = new List<int>();
        public List<string> dataFile = new List<string>();
        public List<string> databankFile = new List<string>();
        public List<int> databankFileCounter = new List<int>();
        public List<int> depth = new List<int>();

        public void AddRange(TraceFrame x)
        {
            //Must correspond to #qwldak7dad
            this.counter.AddRange(x.counter);
            this.stamp.AddRange(x.stamp);
            this.period_start.AddRange(x.period_start);
            this.period_end.AddRange(x.period_end);
            this.date_start.AddRange(x.date_start);
            this.date_end.AddRange(x.date_end);
            this.name.AddRange(x.name);
            this.text.AddRange(x.text);
            this.precedentsNames.AddRange(x.precedentsNames);
            this.commandFile.AddRange(x.commandFile);
            this.commandLine.AddRange(x.commandLine);
            this.dataFile.AddRange(x.dataFile);
            this.databankFile.AddRange(x.databankFile);
            this.databankFileCounter.AddRange(x.databankFileCounter);
            this.depth.AddRange(x.depth);
        }
        public void Add(TraceFrame traceFrame, int i)
        {
            //Must correspond to #qwldak7dad
            this.counter.Add(traceFrame.counter[i]);
            this.stamp.Add(traceFrame.stamp[i]);
            this.period_start.Add(traceFrame.period_start[i]);
            this.period_end.Add(traceFrame.period_end[i]);
            this.date_start.Add(traceFrame.date_start[i]);
            this.date_end.Add(traceFrame.date_end[i]);
            this.name.Add(traceFrame.name[i]);
            this.text.Add(traceFrame.text[i]);
            this.precedentsNames.Add(traceFrame.precedentsNames[i]);
            this.commandFile.Add(traceFrame.commandFile[i]);
            this.commandLine.Add(traceFrame.commandLine[i]);
            this.dataFile.Add(traceFrame.dataFile[i]);
            this.databankFile.Add(traceFrame.databankFile[i]);
            this.databankFileCounter.Add(traceFrame.databankFileCounter[i]);
            this.depth.Add(traceFrame.depth[i]);
        }
    }

    public class TraceDict
    {
        public GekkoDictionary<string, int> dict_commandFileAndLine = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public GekkoDictionary<string, int> dict_names = new GekkoDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public GekkoDictionary<int, int> dict_t1 = new GekkoDictionary<int, int>();
        public GekkoDictionary<int, int> dict_t2 = new GekkoDictionary<int, int>();

        public void Add1(GekkoDictionary<string, int> dict, string s)
        {
            if (!dict.ContainsKey(s)) dict.Add(s, 1);
            else dict[s]++;
        }

        public void Add1(GekkoDictionary<int, int> dict, int i)
        {
            if (!dict.ContainsKey(i)) dict.Add(i, 1);
            else dict[i]++;
        }
    }

    public class TraceFlow
    {
        public static TraceFrame Analyze(Dictionary<Trace2_1_1, PrecedentsAndDepth_1_1> traces, string fileNameWithPath)
        {
            TraceFrame df = new TraceFrame();
            foreach (KeyValuePair<Trace2_1_1, PrecedentsAndDepth_1_1> kvp in traces)
            {
                //Must correspond to #qwldak7dad
                Trace2_1_1 trace = kvp.Key;
                int depth = kvp.Value.depth;
                if (trace.type == ETraceType.GluedToSeries) continue;
                df.counter.Add(trace.traceContents.id.GetCounter());
                df.stamp.Add(trace.traceContents.id.GetStamp());
                string[] ss = trace.traceContents.commandFileAndLine.Split('¤');
                df.commandFile.Add(ss[0]);
                if (ss.Length > 1) df.commandLine.Add(int.Parse(ss[1]));
                else df.commandLine.Add(-1);
                df.name.Add(trace.traceContents.name);
                DateTime? pq_date_starts; string pq_period_starts;
                ParquetHelper.WriteParquetPeriodStart(trace.traceContents.period.t1, out pq_date_starts, out pq_period_starts);
                DateTime? pq_date_ends; string pq_period_ends;
                ParquetHelper.WriteParquetPeriodEnd(trace.traceContents.period.t2, out pq_date_ends, out pq_period_ends);
                df.period_start.Add(pq_period_starts);
                df.period_end.Add(pq_period_ends);
                df.date_start.Add(pq_date_starts);
                df.date_end.Add(pq_date_ends);
                df.text.Add(trace.traceContents.text);
                df.dataFile.Add(trace.traceContents.dataFile);
                df.databankFile.Add(fileNameWithPath);
                df.precedentsNames.Add(Stringlist.GetListWithCommas(trace.traceContents.precedentsNames));
                df.depth.Add(depth);
            }
            return df;
        }
    }

    public class TraceFlowElement
    {
        public readonly TraceID2 id = new TraceID2();
        public GekkoTimeSpanSimple period = null;
        public string text = null;
        public string name = null;  //with bank and freq
        public string commandFileAndLine = null;
        public string dataFile = null;
        public List<string> precedentsNames = null; //Elements are with bank and freq, but also starts with a type like "4¤..." to indicate info on databank, freq, and if the name has traces. See #9khsigra7ioau
        public List<TraceFlowDatabankStamp> foundInWhichDatabanks = new List<TraceFlowDatabankStamp>();
    }

    public class TraceFlowDatabankStamp
    {
        public string databankName = null;
        public DateTime utcTime = DateTime.MinValue;
    }
}
