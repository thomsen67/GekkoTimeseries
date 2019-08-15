using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using System.Drawing;

namespace Gekko
{
    public enum EFixedType
    {
        None, //no fixing or parameter (that is: endogenous)
        Parameter, //for series type normal or arraysuper, corresponds to GAMS parameter
        Normal, //for series type normal
        Timeless, //for series type timeless
    }

    [ProtoContract]
    //GekkoTime is an immutable struct for fast looping. Structs should be < 16 bytes to be effective (we have 3 x 4 = 12 bytes here)
    public struct GekkoTime_1_2
    {
        //use IsNull() to check for null. The fields are short, to reduce size since this is a struct (that also gets saved in GBK files).
        [ProtoMember(1)]
        public readonly short super;   //year, null object is emulated by setting super to -12345
        [ProtoMember(2)]
        public readonly short sub;     //quarter, month, week, etc. (not day)
        [ProtoMember(3)]
        public readonly short subsub;  //day, if sub is month
        [ProtoMember(4)]
        public readonly EFreq_1_2 freq;

        public static GekkoTime_1_2 tNull = new GekkoTime_1_2(EFreq_1_2.A, -12345, 1);  //think of it as a 'null' object (but it is a struct)

        public GekkoTime_1_2(EFreq_1_2 freq2, int super2, int sub2)
        {
            freq = freq2;
            super = (short)super2;
            sub = (short)sub2;
            //Sanity checks to follow
            //Problem is that TIME 2010m13 2012m0 can probably parse. If not, the check below is not necessary.            
            if (sub < 1)
            {
                G.Writeln2("*** ERROR: subperiod < 1");
                throw new GekkoException();
            }
            if (freq == EFreq_1_2.A)
            {
                if (sub > 1)
                {
                    G.Writeln2("*** ERROR: freq 'a' cannot have subperiod > 1");
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq_1_2.Q)
            {
                if (sub > 4)
                {
                    G.Writeln2("*** ERROR: freq 'q' cannot have subperiod > 4");
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq_1_2.M)
            {
                if (sub > 12)
                {
                    G.Writeln2("*** ERROR: freq 'm' cannot have subperiod > 12");
                    throw new GekkoException();
                }
            }
            else if (freq == EFreq_1_2.U)
            {
                if (sub > 1)
                {
                    G.Writeln2("*** ERROR: freq 'u' cannot have subperiod > 1");
                    throw new GekkoException();
                }
            }
            subsub = 0;
        }
    }

    public enum EFreq_1_2  //search for 'ttfreq' to see places with freq that should be enumerated
    {
        A,
        Q,
        M,
        U,      //also called 'u' in Eviews, called 'n' in TSP, but undated has no name in AREMOS (uses 'periodic')     
        None          //used to signal non-freq variable, for instance a VAL   
    }

    [ProtoContract]
    public class MapMultidim
    {
        [ProtoMember(1)]
        public Dictionary<MapMultidimItem, IVariable> storage = new Dictionary<MapMultidimItem, IVariable>();
        
        public MapMultidim()
        {
            //only for protobuf use
        }

        public bool TryGetValue(MapMultidimItem gmi, out IVariable iv)
        {
            return this.storage.TryGetValue(gmi, out iv);
        }
    }

    [ProtoContract]
    public class MapMultidimItem
    {
        [ProtoMember(1)]
        public string[] storage = null;

        public Series_1_2 parent = null;  //do not store in protobuf

        private MapMultidimItem()
        {
            //only because protobuf needs it, not for outside use
        }

        //Only used for lookup purposes, is going to be discarded afterwards
        public MapMultidimItem(string[] s)
        {
            this.storage = s;
        }

        //Used for permanent storage, so the mmi must point to its parent
        public MapMultidimItem(string[] s, Series_1_2 parent)
        {
            this.storage = s;
            this.parent = parent;
        }

        public override string ToString()
        {
            string first = null;
            foreach (string s in this.storage)
            {
                //first += "'" + s + "'" + ", ";
                first += s + ", ";
            }
            first = first.Substring(0, first.Length - ", ".Length);
            return first;
        }

        public string GetName()
        {
            return this.parent.name + "[" + this.ToString() + "]";
        }

    }

    [ProtoContract]
    public class Series_1_2
    {
        public enum ESeriesType
        {
            Normal,
            Light,
            Timeless,
            ArraySuper
        }

        [ProtoMember(1)]
        public SeriesMetaInformation meta = null;
        ///// <summary>
        ///// Indicates the frequency of the Series.
        ///// </summary>
        [ProtoMember(2)]
        public EFreq_1_2 freq;
        /// <summary>
        /// The name of the variable. In a databank, this name corresponds to the key that the Series is stored under,
        /// including frequency (for instance x!q for x with quarterly freq).        
        /// </summary>
        [ProtoMember(3)]
        public string name = null;
        /// <summary>
        /// The array containing the time series data. This array is initialized with NaN values, and the array may resize
        /// itself if necessary to store a particular observation.
        /// </summary>
        [ProtoMember(5)]
        public SeriesDataInformation data = new SeriesDataInformation();  //Must be born with this, unless ArraySuper       
        //[ProtoMember(7)]
        //private bool isTimeless = false; //a timeless variable is like a ScalarVal (VAL). A timeless variable puts the value in dataArray[0]        
        //[ProtoMember(8)]
        public MapMultidim dimensionsStorage = null;  //only active if it is an array-timeseries

        [ProtoMember(9)]
        //dimensions = gdxdimensions + (type == Timeless) - 1
        public int dimensions = 0;  //non-time dimensions: default is 0 which is same as normal timeseries, also used in IsArrayTimeseries()
        [ProtoMember(10)]
        public ESeriesType type = ESeriesType.Normal;  //default

        [ProtoMember(11)]
        //BEWARE: Be careful when using .dataOffsetLag! #772439872435
        private int dataOffsetLag = 0;  //Added in protobuf for ultra-safety, should not be necessary. Only used in Series Light, to create lags/leads, never stored in protobuf since Series Light are never stored there

        public MapMultidimItem mmi = null;  //only used for array-subseries, pointing to its indices, the 'a', 'b' in x['a', 'b'].
        
        private Series_1_2()
        {
            //This is ONLY because protobuf-net needs it! 
            //Empty timeseries should not be created that way.            
        }

        public Series_1_2(ESeriesType type, EFreq_1_2 freq)
        {
            //Creates minimal light series
            this.type = type;
            this.freq = freq;
            if (type != ESeriesType.Normal && type != ESeriesType.Light)
            {
                G.Writeln2("*** ERROR: Series constructor error");
                throw new GekkoException();
            }
        }        

        /// <summary>
        /// Puts a date stamp into the timeseries, see also .isDirty
        /// </summary>
        public void Stamp()
        {
            //See also #80927435209843
            this.meta.stamp = Globals.dateStamp;
        }

        private void FreqError(GekkoTime_1_2 t)
        {
            G.Writeln2("*** ERROR: Frequency mismatch: " + (this.freq.ToString()) + " versus " + (t.freq.ToString()));
            throw new GekkoException();
        }

        public double GetDataSimple(GekkoTime_1_2 t)
        {
            //for Normal or Timeless series, not Light
            //if out of bounds, a NaN is returned, no error is issued
            //this is fine if it is not an expression, for instance if it is taken directly from a databank
            return GetData(t);
        }

        public void TooSmallOrTooLarge(int index, out int tooSmall, out int tooLarge)
        {
            tooSmall = -index;
            tooLarge = index - (this.data.GetDataArray_ONLY_INTERNAL_USE().Length - 1);
        }

        /// <summary>
        /// Gets the timeseries value corresponding to the given period.
        /// </summary>
        /// <param name="t">The period.</param>
        /// <returns>The value (double.NaN if missing)</returns>
        /// <exception cref="GekkoException">Exception if frequency of timeseries and period do not match.</exception>
        //smpl so that tooSmall/tooLarge error can be raised (set to null if irrelevant)
        //set smpl = null if tooSmall/tooLarge is irrelevant (no light series used)
        public double GetData(GekkoTime_1_2 t)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetArrayIndex() which is safe
            // ----------------------------------------------------------------------------

            //Instead of GetData(null, t), please use GetDataNonLight(t)
            double rv = double.NaN;
            if (this.freq != t.freq)
            {
                FreqError(t);
            }
            if (this.data.GetDataArray_ONLY_INTERNAL_USE() == null)
            {
                //If no data has been added to the timeseries, NaN will always be returned.
                if (this.type == ESeriesType.ArraySuper)
                {
                    G.Writeln2("*** ERROR: The variable '" + this.name + "' is an array-timeseries,");
                    G.Writeln("           but is used as a normal timeseries here (without []-indexer)", Color.Red);
                    Program.ArrayTimeseriesTip(this.name);
                    throw new GekkoException();
                }
                else
                {
                    goto End;
                }
            }
            if (this.type == ESeriesType.Timeless)
            {
                rv = this.data.GetDataArray_ONLY_INTERNAL_USE()[0];
                goto End;
            }
            else
            {
                int index = GetArrayIndex(t);
                int tooSmall = 0; int tooLarge = 0;
                this.TooSmallOrTooLarge(index, out tooSmall, out tooLarge);
                if (tooSmall > 0 || tooLarge > 0)
                {                    
                    goto End;  //out of bounds, we return a missing value (NaN)                    
                }
                else
                {
                    rv = this.data.GetDataArray_ONLY_INTERNAL_USE()[index];
                    goto End;
                }
            }
        End:
            //if (MissingZero(this))
            //{
            //    if (G.isNumericalError(rv)) rv = 0d;
            //}
            return rv;
        }        

        public double GetTimelessData()
        {
            if (this.type != ESeriesType.Timeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #1009");
                throw new GekkoException();
            }
            if (this.data.GetDataArray_ONLY_INTERNAL_USE() == null)
            {
                return double.NaN;
            }
            return this.data.GetDataArray_ONLY_INTERNAL_USE()[0];
        }

        
        /// <summary>
        /// Gets the period (GekkoTime) corresponding to a particular index in the data array.
        /// </summary>
        /// <param name="indexInDataArray">The index in the data array.</param>
        /// <returns>The period (GekkoTime).</returns>
        public GekkoTime_1_2 GetPeriod(int indexInDataArray)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetAnchorPeriodPositionInArray()
            // ----------------------------------------------------------------------------

            if (this.type == ESeriesType.Timeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #7");
                throw new GekkoException();
            }
            //The inverse method is GetArrayIndex()
            //Should maybe be private method? But then how to unit-test?
            //see also AddToPeriod()
            //DimensionCheck();
            int subPeriods = 1;
            if (this.freq == EFreq_1_2.Q) subPeriods = 4;
            else if (this.freq == EFreq_1_2.M) subPeriods = 12;
            else if (this.freq == EFreq_1_2.U) subPeriods = 1;

            //Calculates the period by means of using the anchor. Uses integer division, so there is an
            //implicit modulo calculation here.
            int sub1 = this.data.anchorPeriod.sub + (indexInDataArray - this.GetAnchorPeriodPositionInArray());
            int addPer = (sub1 - 1) / subPeriods;
            int addSub = (indexInDataArray - this.GetAnchorPeriodPositionInArray()) - subPeriods * addPer;

            int resultSuperPer = this.data.anchorPeriod.super + addPer;
            int resultSubPer = this.data.anchorPeriod.sub + addSub;

            //This code below fixes a bug (1.4 suffers from it: only affects non-annual timeseries), bug fixed in 1.5.8
            if (resultSubPer < 1)  //this may happen, probaby because of "/" on integer not behaving as expected
            {
                resultSuperPer -= 1;
                resultSubPer += subPeriods;
            }
            GekkoTime_1_2 t = new GekkoTime_1_2(this.freq, resultSuperPer, resultSubPer);
            return t;
        }

        // -----------------------------------------------------------------------------
        // ----------------- private methods -------------------------------------------
        // -----------------------------------------------------------------------------

        
        //Not intended for outside use
        public int GetArrayIndex(GekkoTime_1_2 gt)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetAnchorPeriodPositionInArray()
            // ----------------------------------------------------------------------------

            int rv = FromGekkoTimeToArrayIndexAbstract(gt, new GekkoTime_1_2(this.freq, this.data.anchorPeriod.super, this.data.anchorPeriod.sub), this.GetAnchorPeriodPositionInArray());
            return rv;
        }

        //NOT for outside use, note that it is not safe regarding .dataOffsetLag!
        private static int FromGekkoTimeToArrayIndexAbstract(GekkoTime_1_2 gt, GekkoTime_1_2 anchorPeriod, int anchorPeriodPositionInArray)
        {
            // ----------------------------------------------------------------------------
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // !!! NOTE: OFFSET UNSAFE !!!!!!!!!
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // ----------------------------------------------------------------------------

            //Static method not relying on the Series object
            //NO .dataOffsetLag here, must be added afterwards

            if (gt.freq != anchorPeriod.freq)
            {
                G.Writeln2("*** ERROR: Frequency mismatch");
                throw new GekkoException();
            }
            //this.anchorPeriod.sub is always 1 at the moment, and will always be 1 for Annual.
            //but we cannot count on anchorSubPeriod being 1 forever (for instance for daily obs)   
            int rv = -12345;
            if (anchorPeriod.freq == EFreq_1_2.A)
            {
                //Special treatment in order to make it fast.
                //undated freq could return fast in the same way as this??
                rv = anchorPeriodPositionInArray + gt.super - anchorPeriod.super;
            }
            else
            {
                //Non-annual                
                int subPeriods = 1;
                if (anchorPeriod.freq == EFreq_1_2.Q) subPeriods = 4;
                else if (anchorPeriod.freq == EFreq_1_2.M) subPeriods = 12;
                else if (anchorPeriod.freq == EFreq_1_2.U) subPeriods = 1;
                //For quarterly data for instance, each super period amounts to 4 observations. Therefore the multiplication.
                int dif = subPeriods * (gt.super - anchorPeriod.super) + (gt.sub - anchorPeriod.sub);
                int index = anchorPeriodPositionInArray + dif;
                rv = index;
            }

            return rv;
        }

        public int FromGekkoTimeToArrayIndex(GekkoTime_1_2 gt)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetAnchorPeriodPositionInArray()
            // ----------------------------------------------------------------------------
            return FromGekkoTimeToArrayIndexAbstract(gt, this.data.anchorPeriod, this.GetAnchorPeriodPositionInArray());
        }

        private int ResizeDataArray(GekkoTime_1_2 gt)
        {
            return ResizeDataArray(gt, true);
        }

        public void InitializeDataArray(double[] dataArray)
        {
            //Fill it with NaN's.
            if (Globals.initializeDataArrayWithNaN)
            {
                for (int i = 0; i < dataArray.Length; i++)
                {
                    dataArray[i] = double.NaN;
                }
            }
        }

        private int ResizeDataArray(GekkoTime_1_2 gt, bool adjustStartEndDates)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetArrayIndex() which is safe
            // ----------------------------------------------------------------------------

            int index = GetArrayIndex(gt);
            while (index < 0 || index >= this.data.GetDataArray_ONLY_INTERNAL_USE().Length)
            {
                //Resize data array
                //Keeps on going until the array is large enough.
                double n = Math.Max(this.data.GetDataArray_ONLY_INTERNAL_USE().Length, 4);  //the length could be 1 (or maybe even 0), so we translate 0, 1, 2, 3 into 4 which will become 6 with 1.5 times expandRate.
                double[] newDataArray = new double[(int)(n * Globals.defaultExpandRateForDataArrays)];
                InitializeDataArray(newDataArray);
                if (index >= this.data.GetDataArray_ONLY_INTERNAL_USE().Length)
                {
                    //new periods after end
                    System.Array.Copy(this.data.GetDataArray_ONLY_INTERNAL_USE(), newDataArray, this.data.GetDataArray_ONLY_INTERNAL_USE().Length);
                }
                else
                {
                    //new periods added before start
                    int diffSize = newDataArray.Length - this.data.GetDataArray_ONLY_INTERNAL_USE().Length;
                    System.Array.Copy(this.data.GetDataArray_ONLY_INTERNAL_USE(), 0, newDataArray, diffSize, this.data.GetDataArray_ONLY_INTERNAL_USE().Length);
                    this.data.anchorPeriodPositionInArray += diffSize;
                    if (adjustStartEndDates)  //only for setting data
                    {
                        if (this.meta != null)
                        {
                            if (this.meta.firstPeriodPositionInArray != Globals.firstPeriodPositionInArrayNull)
                            {
                                this.meta.firstPeriodPositionInArray += diffSize;
                            }
                            if (this.meta.lastPeriodPositionInArray != Globals.lastPeriodPositionInArrayNull)
                            {
                                this.meta.lastPeriodPositionInArray += diffSize;
                            }
                        }
                    }
                }

                //this.data.GetDataArray_ONLY_FOR_INTERNAL_USE() = newDataArray;
                //this.data.SetDataArray_ONLY_FOR_INTERNAL_USE(newDataArray);

                this.data.SetDataarray_ONLY_INTERNAL_USE(newDataArray);
                index = GetArrayIndex(gt);
            }
            return index;
        }

        private void InitDataArray(GekkoTime_1_2 t)
        {
            if (this.type == ESeriesType.Timeless)
            {
                G.Writeln2("*** ERROR: Timeless error #10");
                throw new GekkoException();
            }
            else
            {
                //The anchor is set in the middle of the array, and the anchor date is set to gt.
                this.data.SetDataarray_ONLY_INTERNAL_USE(new double[Globals.defaultPeriodsWhenCreatingTimeSeries]);
                this.data.anchorPeriodPositionInArray = Globals.defaultPeriodsWhenCreatingTimeSeries / 2;  //possible to simulate 100 years forwards, and have data 100 years back.
                InitializeDataArray(this.data.GetDataArray_ONLY_INTERNAL_USE());  //may fill it with NaN's
                                                                                  //the following two will always be fixed to what they
                                                                                  //were for the very first observation entering the double[] array (unless the array is resized).
                this.data.anchorPeriod = t;
            }
        }        


        private bool Type(ESeriesType type)
        {
            //type is not used here, just a decorator
            return this.name == null;  //then this.meta will also be null, but we only test .name
        }

        
        public int GetAnchorPeriodPositionInArray()
        {
            //this.data is not null when this is called
            //BEWARE: Be careful when using .dataOffsetLag! #772439872435
            return this.data.anchorPeriodPositionInArray + this.dataOffsetLag;  //.dataOffset changed from 0 to -1 is same as x[-1]
        }

        
    }

    [ProtoContract]
    public class SeriesDataInformation
    {
        public double[] GetDataArray_ONLY_INTERNAL_USE()
        {
            return this.dataArray;
        }

        public void SetDataarray_ONLY_INTERNAL_USE(double[] x)
        {
            this.dataArray = x;
        }

        //public double[] GetDataarray()
        //{
        //    return this.dataArray;
        //}

        [ProtoMember(1, IsPacked = true)]  //a bit faster, and a bit smaller file (also when zipped) 
        //BEWARE: Be careful about .dataOffsetLag when using the array! #772439872435
        private double[] dataArray = null;  //BEWARE: if altering directly, make sure that .protect in the databank is not set!!

        [ProtoMember(2)]
        public GekkoTime_1_2 anchorPeriod = GekkoTime_1_2.tNull;

        [ProtoMember(3)]
        //Do not access directly, use GetAnchorPeriodPositionInArray(), so the .lagOffset is included
        public int anchorPeriodPositionInArray = -123454321;

    }

    [ProtoContract]
    public class SeriesMetaInformation
    {
        [ProtoMember(1)]
        public string label;
        /// <summary>
        /// The source of the timeseries (meta-data), for instance 'Statistics Denmark, National Accounts'.
        /// </summary>
        [ProtoMember(2)]
        public string source;
        /// <summary>
        /// First data in timeseries: points to the index in the data array.
        /// </summary>        
        [ProtoMember(3)]
        public int firstPeriodPositionInArray = int.MaxValue;
        /// <summary>
        /// Last data in timeseries: points to the index in the data array.
        /// </summary>        
        [ProtoMember(4)]
        public int lastPeriodPositionInArray = int.MinValue;
        /// <summary>
        /// This field is only used when CLOSEing an OPEN bank, to see if
        /// the bank needs to be rewritten. Should not be put into protobuffer.
        /// </summary>        
        [ProtoMember(5)]
        public string stamp;
        [ProtoMember(6)]
        public string units;

        [ProtoMember(7)]
        public string[] domains = null;

        [ProtoMember(8)]
        public EFixedType fix = EFixedType.None;
        [ProtoMember(9)]
        public GekkoTimeSpans fixedNormal = null;  //only for EFixedType.Normal, setting the periods

        private bool isDirty = false;  //do not keep this in protobuf
        public Databank parentDatabank = null;  //do not keep this in protobuf        

        public void SetDirty(bool b1)
        {
            this.isDirty = b1;
        }


        //public void SetGhost(bool b2)
        //{
        //    this.isGhost = b2;
        //}

        public bool IsDirty()
        {
            return this.isDirty;
        }

        //public bool IsGhost()
        //{
        //    return this.isGhost;
        //}
    }

    class Databank_1_2
    {
    }
}
