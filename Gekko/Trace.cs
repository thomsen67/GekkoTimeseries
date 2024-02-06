using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Threading;

namespace Gekko
{
    /// 
    public enum ETraceType
    {
        Normal,
        GluedToSeries,
        Divider,
        Dangling
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
        TrimWithTimeShadowing
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
        public List<string> precedentsNames = null; //Elements are with bank and freq, but also starts with a type like "4¤..." to indicate info on databank, freq, and if the name has traces.
                
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
        /// See also AddRangeFromSeries2().
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="rhs"></param>
        public static void AddRangeFromSeries1(Trace2 trace, Series rhs)
        {
            bool hasTrace = true; if (rhs?.meta?.trace2 == null) hasTrace = false;

            if (trace.GetContents().precedentsNames == null) trace.GetContents().precedentsNames = new List<string>();

            trace.GetContents().precedentsNames.Add(TraceGetNameDecorated(rhs, hasTrace));

            if (hasTrace && rhs.meta.trace2.GetPrecedents_BewareOnlyInternalUse().Count() > 0)
            {
                int counter2 = -1;
                foreach (TraceAndPeriods2 kvp in rhs.meta.trace2.GetPrecedents_BewareOnlyInternalUse().GetStorage())
                {
                    TraceAndPeriods2 childTrace2 = kvp;
                    bool known = false;
                    if (trace.precedents.GetStorage() != null)
                    {
                        foreach (TraceAndPeriods2 tempElement in trace.precedents.GetStorage())
                        {
                            if (Object.ReferenceEquals(childTrace2, tempElement))
                            {
                                known = true; break;
                            }
                        }
                    }

                    if (!known)
                    {
                        counter2++;
                        if (trace.precedents.GetStorage() == null)
                        {
                            trace.precedents.InitWithEmptyList();
                        }
                        if (counter2 == 0 && trace.precedents.GetStorage().Count > 0 && trace.precedents.GetStorage()[trace.precedents.GetStorage().Count - 1] != null)
                        {
                            trace.precedents.GetStorage().Add(new TraceAndPeriods2(new Trace2(ETraceType.Divider, true), Globals.traceNullPeriods));  //divider  
                        }

                        // --------- clone start ----------------
                        //We must clone the period part of the trace+period, because otherwise it may be overwritten in a wrong way.
                        GekkoTimeSpansSimple xx = null;
                        if (childTrace2.periods != null)
                        {
                            xx = new GekkoTimeSpansSimple();
                            xx.AddRange(childTrace2.periods);  //the timespans themselves are immutable
                        }
                        TraceAndPeriods2 childTrace2Clone = new TraceAndPeriods2(childTrace2.trace, xx);
                        // --------- clone end ----------------

                        trace.precedents.GetStorage().Add(childTrace2Clone);
                    }
                }
            }
        }

        /// <summary>
        /// Removes time-shadowed traces from a Gekko databank. Used just before writing the databank.
        /// Too costly to run all the time when traces change.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static TraceHelper TraceTrim(Databank db)
        {
            return Trace2.CollectAllTraces(db, ETraceHelper.TrimWithTimeShadowing);
        }

        /// <summary>
        /// Used in trace: .precedentsNames. For a series x!a in databank b, theres is a prefix {i}¤ on names, where i is an integer from 1 to 8.
        /// See code: it is combinations of has trace, is first-pos databank, and is current freq.
        /// </summary>
        /// <param name="rhs"></param>
        /// <param name="hasTrace"></param>
        /// <returns></returns>
        private static string TraceGetNameDecorated(Series rhs, bool hasTrace)  //See #9khsigra7ioau
        {
            string prefix = null;            
            string databankName = rhs.GetParentDatabank()?.GetName();  //databank may be null, for instance an imported series
            bool isFirst = G.Equal(databankName, Program.databanks.GetFirst().GetName());
            bool isCurrentFreq = rhs.freq == Program.options.freq;
            if (hasTrace)
            {
                if (isFirst)
                {
                    if (isCurrentFreq) prefix = "1";
                    else prefix = "2";
                }
                else
                {
                    if (isCurrentFreq) prefix = "3";
                    else prefix = "4";
                }
            }
            else
            {
                if (isFirst)
                {
                    if (isCurrentFreq) prefix = "5";
                    else prefix = "6";
                }
                else
                {
                    if (isCurrentFreq) prefix = "7";
                    else prefix = "8";
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
            if (hasTrace) this.GetPrecedents_BewareOnlyInternalUse().AddRange(rhs.meta.trace2.GetPrecedents_BewareOnlyInternalUse()); //may come from an old Gekko databank where .trace2 == null.           
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
            if (Globals.traceWallTimeHandledSpecialWayFor1UnitTest) return TimeShadow2(true, false);
            else return TimeShadow2(true, true);
        }

        /// <summary>
        /// For the list of precedents in this.precedents, the method checks which traces are shadowed by later traces.
        /// With shadowedTracesAreRemoved == false, the count of the list returned will be the same as the count of the
        /// count of this.precedents (so we also get null-dividers). The included list includes period information, and
        /// if shadowedTracesAreRemoved == false, the period info may be empty.
        /// BEWARE: can return null!
        /// </summary>        
        public List<TraceAndPeriods2> TimeShadow2(bool shadowedTracesAreRemoved, bool invertWallTime)
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
                        //this.precedents.UpdateSorted();
                        mustUpdateSorted = true;
                        //sbi.mustBeRemoved = true;
                        removeInSorted.Add(sbi);
                        removeInUnsorted.Add(sbi.tap);

                    }
                    else if (!newSpans.GetStorage()[newSpans.Count() - 1].t2.EqualsGekkoTime(sbi.tap.LastPeriod()))
                    {
                        //It must be removed and added again because the last t2 of the spans changes
                        mustUpdateSorted = true;
                        //sbi.mustBeRemoved = true;
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
                if (Globals.traceFix)
                {
                    trace2 = this;
                    trace2.precedents = this.precedents?.DeepClone(cloneHelper);
                }
                else
                {
                    if (true)
                    {
                        trace2 = new Trace2(this.type, this.traceContents);  //the .traceContents object is not cloned!
                        trace2.precedents = this.precedents?.DeepClone(cloneHelper);
                    }
                    else
                    {
                        trace2 = new Trace2(this.type, this.GetContents());
                        trace2.precedents = this.precedents?.DeepClone(cloneHelper);
                    }                   
                }

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
            s += this.GetId().stamp.ToString("dd/MM/yyyy HH:mm:ss") + "|" + this.GetId().counter;
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
                s2 += ", stamp: " + this.GetId().stamp.ToString("g", System.Globalization.CultureInfo.CreateSpecificCulture(Globals.languageDaDK));
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
            if (trace.GetContents().text == null) new Error("Trace problem: trace.GetContents().text == null");
            if (ts.meta == null) new Error("Trace problem: ts.meta == null");
            
            if (ts.meta.trace2 == null) ts.meta.trace2 = new Trace2(ETraceType.GluedToSeries, ETraceParentOrChild.Parent);

            if (type == ETracePushType.Sibling)
            {
                //In something like "reset; y = 1; y = 2;" this is called 2 times.
                ts.meta.trace2.PrecedentsShadowing(trace);
            }
            else if (type == ETracePushType.NewParent)
            {                   
                trace.AddRangeFromSeries2(null, ts);
                ts.meta.trace2.precedents = new Precedents2();
                TraceAndPeriods2 tap6 = new TraceAndPeriods2();
                tap6.trace = trace;
                GekkoTimeSpansSimple xx = new GekkoTimeSpansSimple();
                xx.SetStorage(new List<GekkoTimeSpanSimple>() { tap6.trace.GetContents().period });  //should be ok to just add it here, because .GetContents().period never changes (is immutable anyway)
                tap6.periods = xx;
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
                foreach (Trace2 trace in databank.traces) dictInverted[trace.GetId()] = trace;
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
                dict1Inverted[trace.GetId()] = trace;
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
                TraceHelper th = new TraceHelper();
                trace.DeepTrace(th, 0);
                int count2 = Trace2.CountWithoutInvisible(th.tracesDepth2);
                string s = "Traces";
                if (true)
                {
                    if (trace.precedents.Count() > 0)
                    {
                        Action<GAO> a = (gao) =>
                        {
                            CallTraceViewer(trace, int.MaxValue);
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

        public static int CallTraceViewer(Trace2 trace, int maxDepth)
        {
            // with graph = false: 2 --> 4, 3 --> 11, 4 --> 35, 5 --> 134, 6 --> 204, 7 --> 397, 8 --> 432, 9 --> 432
            // sith graph = true:  2 --> 4, 3 --> 11, 4 --> 34, 5 --> 128, 6 --> 166, 7 --> 184, 8 --> 189, 9 --> 189

            // Items = disp = 188, new items = 432 (437)

            int nn = 0;

            if (!G.IsUnitTesting())
            {               

                Thread sta = new Thread(delegate ()
                {
                    Globals.itemCounter = 0;

                    TreeGridModel model = new TreeGridModel();
                    
                    Item temp = null;

                    //if (lazy) temp = trace.precedents.GetStorage()[0].Get1Item(new List<GekkoTimeSpanSimple>());
                    int maxDepth2 = int.MaxValue;
                    if (Globals.isWindowTreeViewWithTableLazy) maxDepth2 = 2;
                    temp = trace.FromTraceToTreeViewItemsTree(0, 0, null, maxDepth2, Globals.traceShowDividers, ref nn);
                    foreach (Item item in temp.GetChildren())
                    {
                        model.Add(item);
                    }
                    WindowTreeViewWithTable w = new WindowTreeViewWithTable(model);
                    string v = null;
                    if (trace.GetContents() != null && trace.GetContents().name != null) v = G.Chop_RemoveBank(trace.GetContents().name, Program.databanks.GetFirst().name) + " - ";
                    w.Title = v + "Gekko data trace";
                    //w.ShowDialog();
                    w.Show();
                    System.Windows.Threading.Dispatcher.Run();
                });
                sta.SetApartmentState(ApartmentState.STA);
                sta.Start();
            }

            if (Globals.runningOnTTComputer) new Writeln("TTH: items " + Globals.itemCounter);
            return nn;
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

        
        public Item FromTraceToTreeViewItemsTree(int depth, int cnt, GekkoTimeSpansSimple periods, int max, bool showDividers, ref int nn)
        {            
            Item item = FromTraceToTreeViewItem(periods, showDividers);
            nn++;            
            if (depth < max)
            {
                List<TraceAndPeriods2> taps = this.TimeShadow2();
                if (taps != null && taps.Count > 0)
                {
                    foreach (TraceAndPeriods2 tap in taps)
                    {
                        if (!showDividers && tap.trace.type == ETraceType.Divider) continue;  //do not show dividers
                        Item itemChild = null;
                        itemChild = tap.trace.FromTraceToTreeViewItemsTree(depth + 1, cnt + 1, tap.periods, max, showDividers, ref nn);
                        item.GetChildren().Add(itemChild);
                    }
                }
            }
            return item;
        }

        public Item FromTraceToTreeViewItem(GekkoTimeSpansSimple periods, bool showDividers)
        {           

            // =========================================================================
            // Settings for the data trace viewer
            // =========================================================================
            string showFreq = "maybe";  //"yes", "no", "maybe
            string showDatabank = "maybe";  //"yes", "no", "maybe"
            // Also Globals.showDividers and Program.options.databank_trace_trim;
            // =========================================================================
                                                                         
            bool hasChildren = false;
            if (this.precedents != null && this.precedents.Count() > 0) hasChildren = true;
            string text = "null";
            string code = "null";
            string codeDetailed = "null";
            string period = null;
            string active = null;
            string activeDetailed = null;
            string file = null;
            string fileDetailed = null;
            string stamp = null;
            string stampDetailed = null;
            List<string> precedentsNames = null;

            if (this.GetContents() != null)
            {
                //Note: we always remove bank name, since this is often irrelevant. Freq is removed if same as current freq.
                if (this.GetContents().name != null) text = G.Chop_RemoveFreq(G.Chop_RemoveBank(this.GetContents().name), Program.options.freq);
                GetCodeAsString(this.GetContents().text, out code, out codeDetailed);
                GekkoTime t1 = GekkoTime.tNull;
                GekkoTime t2 = GekkoTime.tNull;
                if (this.GetContents().period != null)
                {
                    t1 = this.GetContents().period.t1;
                    t2 = this.GetContents().period.t2;
                }
                if (t1.IsNull() && t2.IsNull()) period = "";
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
                if (this.GetContents().precedentsNames != null) precedentsNames = GetPrecedentsNames(showFreq, showDatabank);
            }

            Item newItem = new Item(text, code, codeDetailed, period, active, activeDetailed, stamp, stampDetailed, file, fileDetailed, precedentsNames, hasChildren);
            newItem.trace = this;
            return newItem;
        }

        public static void GetCodeAsString(string text, out string code, out string codeDetailed)
        {
            codeDetailed = text;
            code = null;
            if (codeDetailed != null) code = System.Text.RegularExpressions.Regex.Replace(codeDetailed, @"\s+", " "); //https://stackoverflow.com/questions/206717/how-do-i-replace-multiple-spaces-with-a-single-space-in-c                
        }

        public static void GetStampAsString(TraceID2 id, out string stamp, out string stampDetailed)
        {
            stamp = id.stamp.ToString("g", System.Globalization.CultureInfo.CreateSpecificCulture(Globals.languageDaDK));
            stampDetailed = id.stamp.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture(Globals.languageDaDK)) + ", #" + id.counter;
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
                    if (n <= 1)
                    {
                        active += gts.t1.ToString() + "-" + gts.t2.ToString();
                    }
                    else
                    {
                        active += "...";
                    }
                    activeDetailed += gts.t1.ToString() + "-" + gts.t2.ToString();
                }
            }
        }

        private List<string> GetPrecedentsNames(string showFreq, string showDatabank)
        {
            List<string> precedentsNames;
            List<string> list = new List<string>();
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
            precedentsNames = list;
            return precedentsNames;
        }

        //public static bool MustBeRemoved(SortedBagItem s)
        //{
        //    return true;
        //    //return s.mustBeRemoved;
        //}
    }

    [ProtoContract]
    public class TraceID2 //TraceID2 because it is experimental
    {       
        /// <summary>
        /// Note: resolution is about 0.01 s.
        /// </summary>
        [ProtoMember(1)]
        public readonly DateTime stamp = DateTime.Now;

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
        public Dictionary<Trace2, Precedents2> traces = new Dictionary<Trace2, Precedents2>();  //value is parent (may be null)
                
        // --- gbk write/read and other stuff
        //Hmm, isn't Precedents already a part of the key? Anyway, the depth needs to be inside an object anyway to be altered.
        public Dictionary<Trace2, PrecedentsAndDepth> tracesDepth2 = new Dictionary<Trace2, PrecedentsAndDepth>();
        //
        // --- this is for time-shadowing        
        public Dictionary<Trace2, string> timeShadowing = new Dictionary<Trace2, string>();
        //public int timeShadowingCuts = 0;

    }

    public class PrecedentsAndDepth
    {
        public Precedents2 precedents = null;
        public int depth = 0;
    }

    /// <summary>
    /// Is basically a List&lt;Trace>.
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
            this.SetStorage(null);
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

                    if (id.counter == long.MinValue)
                    {
                        this.storage.Add(new TraceAndPeriods2(new Trace2(ETraceType.Divider, true), Globals.traceNullPeriods));
                    }
                    else
                    {
                        if (id.counter < 0) new Error("This trace is not stored in the databank, but has been pruned off: " + id.ToString());
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

        public Precedents2 DeepClone(CloneHelper cloneHelper)
        {
            Precedents2 precedents = new Precedents2();            
            if (this.storage != null)
            {
                precedents.storage = new List<TraceAndPeriods2>();
                foreach (TraceAndPeriods2 traceAndPeriods in this.storage)
                {
                    precedents.storage.Add(traceAndPeriods.DeepClone(cloneHelper));
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

        public TraceAndPeriods2 DeepClone(CloneHelper cloneHelper)
        {
            GekkoTimeSpansSimple gtss = null;
            if (this.periods != null)
            {
                gtss = new GekkoTimeSpansSimple();
                gtss.AddRange(this.periods);  //the timespans themselves are immutable
            }
            return new TraceAndPeriods2(this.trace.DeepClone(cloneHelper), gtss);
        }
    }

    public class SortedBagItem  //Comparer: #kjhahaiuoslkfd
    {
        public GekkoTime t = GekkoTime.tNull;
        public TraceAndPeriods2 tap;
        //public bool mustBeRemoved = false;

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
                if (x.tap.trace.GetId().counter == y.tap.trace.GetId().counter) return 0; //will probably not happen because of ReferenceEquals() at the top
                if (x.tap.trace.GetId().counter > y.tap.trace.GetId().counter) return 1; //just random, could just as well be inverse
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
}
