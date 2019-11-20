/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2016, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/


using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;
using System.Drawing;
using System.Linq;

namespace Gekko
{
    public enum ESeriesType
    {
        Normal,
        Light,
        Timeless,
        ArraySuper        
    }

    public enum EFixedType
    {
        None, //no fixing or parameter (that is: endogenous)
        Parameter, //for series type normal or arraysuper, corresponds to GAMS parameter
        Normal, //for series type normal
        Timeless, //for series type timeless
    }

    //                 name        meta        dataArray     dimensions      dimensionsArray
    // ---------------------------------------------------------------------------------------------
    // Normal             x           x                x             0               null
    // Light           null        null          small x             0               null
    // Timeless           x           x              1 x             0               null
    // ArraySuper         x           x             null             n                  x    
    // ---------------------------------------------------------------------------------------------
    // ArraySub has no name, else like Normal
    //

    /// <summary>
    /// The Series class is a class designed for storing and retrieving timeseries (vectors/arrays of consecutive time data) 
    /// in a fast and reliable way. Timeseries can be of different frequencies (for instance annual, quarterly or monthly). 
    /// The internal representation of the data is an auto-resizing double[] array for speed and compactness.
    /// </summary>
    /// <remarks>
    /// The Series class is typically used in conjunction with a Databank, which can be thought of as a container that
    /// stores the individual timeseries by their string names. Several databanks may be open at the same time (in RAM).
    /// </remarks>
    /// <example>
    /// A stand-alone Series may be created and filled with data like this:
    /// <code>
    /// Series ts = new Series(EFreq.Q, "gdp");
    /// GekkoTime t1 = new GekkoTime(EFreq.Q, 2000, 1);
    /// GekkoTime t2 = new GekkoTime(EFreq.Q, 2002, 4);
    /// ts.SetData(t1.Add(-1), 323490d);
    /// foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
    /// {
    ///     double lagValue = ts.GetData(t.Add(-1));
    ///     ts.SetData(t, 1.01d * lagValue);
    /// }    
    /// </code>
    /// The code first creates a new Series 'ts' with quarterly frequency and the name 'gdp' (this name is used if the
    /// timeseries is later put into a databank). Next, start and end periods are defined (2000q1 and 2002q4), and the period
    /// before the start period has its value set to 323490 (that is, for the quarter 1999q4). Next, all the
    /// quarters are looped via the iterator in the next line (that is, t = 2000q1, 2000q2, ... , 2002q4 = 12 observations
    /// in all). Inside the loop, lagValue is given the value of the lagged observations (for t = 2000q1 that will be the
    /// value of the timeseries in 1999q4). This value is augmented with 1% and set for the current observation. This way,
    /// the 'gdp' timeseries will obtain a quarterly growth rate of 1% for the period 2000q1-2002q4.
    /// </example>
    /// <example>
    /// A Series object can be put into a Databank object and retrieved again by name (builds upon the previous example):    
    /// <code>    
    /// Databanks databanks = new Databanks();
    /// databanks.AddDatabank(new Databank("Work");    
    /// databanks.GetDatabank("Work").AddVariable(ts);
    /// Series ts1 = databanks.GetDatabank("Work").GetVariable("gdp");
    /// </code>
    /// The 'Work' databank will contain the 'gdp' time series (the name indicated when the timeseries was created), 
    /// and 'ts' and 'ts1' will point to the same Series object. 
    /// You may consult the Databanks and Databank classes for more info on storing timeseries.
    /// </example>    
    /// <seealso cref="Databank"/>
    /// <seealso cref="Databanks"/>
    [ProtoContract]
    public class Series : IVariable
    {
        [ProtoMember(1)]
        public SeriesMetaInformation meta = null;
        ///// <summary>
        ///// Indicates the frequency of the Series.
        ///// </summary>
        [ProtoMember(2)]
        public EFreq freq;
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
        [ProtoMember(8)]
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
        public ESeriesMissing isNotFoundArraySubSeries = ESeriesMissing.Error; //used when for instance x['a'] does not hit anything

        private Series()
        {
            //This is ONLY because protobuf-net needs it! 
            //Empty timeseries should not be created that way.            
        }

        public Series(ESeriesType type, EFreq freq)
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

        public string GetNameAndFreqPretty(bool useQuotes)
        {
            string s = this.GetName();
            string ss = G.GetNameAndFreqPretty(s, useQuotes);
            return ss;
        }

        public string GetName()
        {            
            if (this.name == null || this.name.StartsWith(Globals.seriesArraySubName))
            {
                if (this.mmi == null)
                {
                    return null;
                }
                return this.mmi.GetName();
            }
            else return this.name;
        }

        public Databank GetParentDatabank()
        {
            //also works for array-subseries
            if (this.name == null && Globals.runningOnTTComputer)
            {
                G.Writeln2("*** ERROR: Parent db error");
                throw new GekkoException();
            }
            if (this.name == null || this.name.StartsWith(Globals.seriesArraySubName))
            {
                return this.mmi.parent.meta.parentDatabank;
            }
            else return this.meta.parentDatabank;
        }

        public string GetNameWithoutCurrentFreq(bool onlyRemoveCurrentFreq)
        {
            if (onlyRemoveCurrentFreq)
            {
                return G.Chop_RemoveFreq(this.GetName(), G.GetFreq(Program.options.freq));
            }
            else
            {
                return G.Chop_RemoveFreq(this.GetName());
            }            
        }

        //public Series(ETimeSeriesType type, GekkoSmpl smpl)
        //{
        //    // ------------------------------
        //    //Constructing a SeriesLight
        //    //type is just a decorator (not used), so that it is easier to 
        //    //see when a light timeseries is created.
        //    // ------------------------------
        //    this.freq = smpl.t0.freq;  //same as for t1, t2 or t3
        //    this.name = null; //light
        //    this.meta = null; //light
        //    int n = smpl.Observations03();
        //    this.dataArray = new double[n];  //we make the array as compact as possible --> faster
        //    InitializeDataArray(this.dataArray);
        //    this.anchorPeriod = smpl.t0;            
        //    this.anchorPeriodPositionInArray = 0;
        //}

        //public Series(ESeriesType type, GekkoTime t1, GekkoTime t2) : this(type, t1, t2)
        //{
        //}

        //public Series(ESeriesType type, EFreq freq) : this(type, GekkoTime.tNull, GekkoTime.tNull, true, freq)
        //{
        //}

        public Series(ESeriesType type, GekkoTime t1, GekkoTime t2)
        {
            // --------------------------------------------
            // ONLY for Light
            // --------------------------------------------

            this.type = type;
            if (type == ESeriesType.Light)
            {

                this.freq = t1.freq;  //same as for t1, t2 or t3  
                int n = GekkoTime.Observations(t1, t2);
                if (n < 1)
                {
                    G.Writeln2("*** ERROR: Attempt to create SERIES with " + n + " observation");
                    throw new GekkoException();
                }
                this.data.SetDataarray_ONLY_INTERNAL_USE(new double[n]);  //we make the array as compact as possible --> faster
                InitializeDataArray(this.data.GetDataArray_ONLY_INTERNAL_USE());
                this.data.anchorPeriod = t1;
                this.data.anchorPeriodPositionInArray = 0;

            }
            else
            {
                G.Writeln2("*** ERROR: SERIES constructor 1");
                throw new GekkoException();
            }
        }

        public Series(ESeriesType type, EFreq freq, string variableName, int dimensions)
        {
            // --------------------------------------------
            // ONLY for ArraySuper
            // --------------------------------------------

            this.type = type;
            if (type == ESeriesType.ArraySuper)
            {
                this.freq = freq;
                this.name = variableName;
                this.meta = new SeriesMetaInformation();
                this.dimensions = dimensions;
                this.dimensionsStorage = new MapMultidim();
                this.data = null; //for safety this is killed off                
            }
            else
            {
                G.Writeln2("*** ERROR: SERIES constructor 2");
                throw new GekkoException();
            }
        }

        public Series(ESeriesType type, EFreq freq, string variableName, double val)
        {
            // --------------------------------------------
            // ONLY for Timeless
            // --------------------------------------------

            this.type = type;
            if (type == ESeriesType.Timeless)
            {
                this.freq = freq;
                this.name = variableName;
                this.meta = new SeriesMetaInformation();
                this.data.SetDataarray_ONLY_INTERNAL_USE(new double[1]);
                this.data.GetDataArray_ONLY_INTERNAL_USE()[0] = val;
            }
            else
            {
                G.Writeln2("*** ERROR: SERIES constructor 4");
                throw new GekkoException();
            }
        }

        /// <summary>
        /// Constructor that creates a new Series object with a particular frequency and variable name.
        /// </summary>
        /// <param name="frequency">The frequency of the timeseries</param>
        /// <param name="variableName">The variable name of the timeseries</param>
        public Series(EFreq frequency, string variableName) : this(ESeriesType.Normal, frequency, variableName)
        {
        }

        public Series(ESeriesType type, EFreq frequency, string variableName)
        {
            // -------------------------------------------------------------------------------------------
            // ONLY for Normal (includes array sub-series that must have name Globals.seriesArraySubName
            // -------------------------------------------------------------------------------------------

            if (this.type != ESeriesType.Normal)
            {
                G.Writeln2("*** ERROR: SERIES constructor 3");
                throw new GekkoException();
            }

            this.type = type;

            this.freq = frequency;

            this.name = variableName;  //Note: the variableName does contain a '!'. If the name is null, it is a array sub-series

            if (this.name != null)
            {
                if (!this.name.Contains(Globals.freqIndicator))
                {
                    G.Writeln2("*** ERROR: Missing freq indicator, see G.AddFreqToName()");
                    throw new GekkoException();
                }
            }
            this.meta = new SeriesMetaInformation();
            //ok that dataArray is null to start out with
        }

        //used in dynamic code for list loops
        public void SetZero(GekkoSmpl smpl)
        {
            foreach (GekkoTime t in smpl.Iterate03())
            {
                this.SetData(t, 0d);
            }
        }

        public void Truncate(AllFreqsHelper dates)
        {
            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

            //Also see #345632473
            if (dates == null) return;
            if (this.freq == EFreq.A)
            {
                this.Truncate(dates.t1Annual, dates.t2Annual);
            }
            else if (this.freq == EFreq.Q)
            {
                this.Truncate(dates.t1Quarterly, dates.t2Quarterly);
            }
            else if (this.freq == EFreq.M)
            {
                this.Truncate(dates.t1Monthly, dates.t2Monthly);
            }
            else if (this.freq == EFreq.D)
            {
                this.Truncate(dates.t1Daily, dates.t2Daily);
            }
            else
            {
                G.Writeln2("***: Freq error");
                throw new GekkoException();
            }
        }

        /// <summary>
        /// Truncates the Series object, so that the starting period
        /// and ending period are as given. Beware: this will usually mean that data is deleted. Used to truncate
        /// a databank to a particular time period. Note: You may wish to use Trim() after a Truncate(). Note: only works for annual timeseries.
        /// </summary>
        /// <param name="start">The start period.</param>
        /// <param name="end">The end period.</param>
        /// <exception cref="GekkoException">
        /// </exception>
        public void Truncate(GekkoTime start, GekkoTime end)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetArrayIndex() which is safe
            // ----------------------------------------------------------------------------

            if (start.freq != end.freq)
            {
                G.Writeln2("*** ERROR: Truncate start and end have different frequencies");
                throw new GekkoException();
            }
            if (this.freq != start.freq)
            {
                G.Writeln2("*** ERROR: Series is freq: " + G.GetFreqString(this.freq) + ", which is different from truncate freq: " + G.GetFreqString(start.freq));
                throw new GekkoException();
            }
            if (this.type == ESeriesType.Timeless) return;
            if (this.meta.parentDatabank != null && !this.meta.parentDatabank.editable) Program.ProtectError("You cannot truncate a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
            int indexStart = this.GetArrayIndex(start);
            int indexEnd = this.GetArrayIndex(end);

            int newFirst = Math.Max(this.meta.firstPeriodPositionInArray, indexStart);
            int newLast = Math.Min(this.meta.lastPeriodPositionInArray, indexEnd);

            if (this.data.GetDataArray_ONLY_INTERNAL_USE() == null)
            {
                //do nothing, could be a series defined like x = m(); 
                //in that case, this.meta.firstPeriodPositionInArray and this.meta.lastPeriodPositionInArray will be strange too
            }
            else
            {
                if (newFirst > newLast)
                {
                    //the truncate window is completely before or after the data window
                    //wipe all the data, and set the sample to 1 length
                    
                    for (int i = 0; i < this.data.GetDataArray_ONLY_INTERNAL_USE().Length; i++)
                    {
                        this.data.GetDataArray_ONLY_INTERNAL_USE()[i] = double.NaN;
                    }

                    SetNullPeriod();
                }
                else
                {
                    //the following two must be inside existing timeseries array
                    this.meta.firstPeriodPositionInArray = newFirst;
                    this.meta.lastPeriodPositionInArray = newLast;

                    //NOTE: after this, the anchor position may be outside first/lastPeriodPositionInArray. 
                    //But this should not be a problem: it is only a hook
                    //that translates a date into an index and vice versa.

                    //When Truncate() is used for writing databanks, the timeseries
                    //will be Trim()'ed anyway, so these missings will disappear. But since the method
                    //could be used for other purposes later on, we set the values to missings explicitly
                    //The time loss is very small, and usually WRITE is not time-truncated anyway.

                    for (int i = 0; i < this.meta.firstPeriodPositionInArray; i++)
                    {
                        this.data.GetDataArray_ONLY_INTERNAL_USE()[i] = double.NaN;
                    }
                    for (int i = this.meta.lastPeriodPositionInArray + 1; i < this.data.GetDataArray_ONLY_INTERNAL_USE().Length; i++)
                    {
                        this.data.GetDataArray_ONLY_INTERNAL_USE()[i] = double.NaN;
                    }
                }
                this.SetDirty(true);
            }
        }

        private void SetNullPeriod()
        {
            this.meta.firstPeriodPositionInArray = Globals.firstPeriodPositionInArrayNull;
            this.meta.lastPeriodPositionInArray = Globals.lastPeriodPositionInArrayNull;
        }

        public bool IsNullPeriod()
        {
            return this.meta.firstPeriodPositionInArray == Globals.firstPeriodPositionInArrayNull && this.meta.lastPeriodPositionInArray == Globals.lastPeriodPositionInArrayNull;
        }

        public void SetArrayTimeseries(int dimensionsIncludingTimeDimension, bool hasTimeDimension)
        {
            int tDim = 0;
            if (hasTimeDimension) tDim = 1;
            this.dimensionsStorage = new MapMultidim();
            this.dimensions = dimensionsIncludingTimeDimension - tDim;
            this.type = ESeriesType.ArraySuper;
            //if (!hasTimeDimension) this.type = ESeriesType.Timeless;
        }

        /// <summary>
        /// Puts a date stamp into the timeseries, see also .isDirty
        /// </summary>
        public void Stamp()
        {
            //See also #80927435209843
            this.meta.stamp = Globals.dateStamp;
        }

        /// <summary>
        /// Trims the internal data array in the object, so that the sequence of data fits exactly inside the array. 
        /// This will save some RAM, and is also used when writing databanks via protobuf serializer.
        /// </summary>
        public void Trim()
        {
            // ----------------------------------------------------------------------------
            // OFFSET: does not use GekkoTime at all, so no problem. This is simple array trimming.
            // ----------------------------------------------------------------------------
            
            //DimensionCheck();
            if (this.data.GetDataArray_ONLY_INTERNAL_USE() == null) return;
            if (this.IsNullPeriod()) return;  //could actually trim this, but oh well
            if (!(this.meta.firstPeriodPositionInArray == 0 && this.meta.lastPeriodPositionInArray == this.data.GetDataArray_ONLY_INTERNAL_USE().Length - 1))  //already trimmed                
            {
                int size = this.meta.lastPeriodPositionInArray - this.meta.firstPeriodPositionInArray + 1;
                double[] temp = new double[size];
                Array.Copy(this.data.GetDataArray_ONLY_INTERNAL_USE(), this.meta.firstPeriodPositionInArray, temp, 0, size);
                int first = this.meta.firstPeriodPositionInArray;
                //Correct these pointers accordingly
                this.data.anchorPeriodPositionInArray += -first;
                this.meta.firstPeriodPositionInArray += -first; // --> 0
                this.meta.lastPeriodPositionInArray += -first;  // --> size-1
                this.data.SetDataarray_ONLY_INTERNAL_USE(temp);  //point to this array
            }
        }

        public double GetDataSimple(GekkoTime t)
        {
            //for Normal or Timeless series, not Light
            //if out of bounds, a NaN is returned, no error is issued
            //this is fine if it is not an expression, for instance if it is taken directly from a databank
            return GetData(null, t);
        }

        /// <summary>
        /// Gets the timeseries value corresponding to the given period.
        /// </summary>
        /// <param name="t">The period.</param>
        /// <returns>The value (double.NaN if missing)</returns>
        /// <exception cref="GekkoException">Exception if frequency of timeseries and period do not match.</exception>
            //smpl so that tooSmall/tooLarge error can be raised (set to null if irrelevant)
            //set smpl = null if tooSmall/tooLarge is irrelevant (no light series used)
        public double GetData(GekkoSmpl smpl, GekkoTime t)
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
                    if (this.type == ESeriesType.Light)
                    {
                        if (smpl == null)
                        {
                            //ignore error
                            if (Globals.runningOnTTComputer)
                            {
                                //for instance, printing montly data ending in m10, where m11 and m12 are also shown
                                G.Writeln("+++ WARNING: TT error: tooSmallTooLarge with no smpl");
                            }
                        }
                        else
                        {
                            if (smpl.gekkoError == null) smpl.gekkoError = new GekkoError(tooSmall, tooLarge);
                        }
                    }
                    goto End;  //out of bounds, we return a missing value (NaN)                    
                }
                else
                {
                    rv = this.data.GetDataArray_ONLY_INTERNAL_USE()[index];
                    goto End;
                }
            }
        End:
            if (MissingZero(this))
            {
                if (G.isNumericalError(rv)) rv = 0d;
            }
            return rv;
        }

        public static bool MissingZero()
        {
            return MissingZero(null);
        }

        public static bool MissingZero(Series x)
        {
            //changing NaN to 0 only works if the option is set, and it is not a light series
            if (x == null) return Program.options.series_data_missing == ESeriesMissing.Zero;
            else return x.type != ESeriesType.Light && Program.options.series_data_missing == ESeriesMissing.Zero;
        }

        private void FreqError(GekkoTime t)
        {
            G.Writeln2("*** ERROR: Frequency mismatch: " + G.GetFreqString(this.freq) + " versus " + G.GetFreqString(t.freq));
            throw new GekkoException();
        }

        public IVariable Concat(GekkoSmpl t, IVariable x)
        {
            G.Writeln2("*** ERROR: Type error regarding concat and SERIES");
            throw new GekkoException();
        }

        public void SetTimelessData(double value)
        {
            if (this.type != ESeriesType.Timeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #100");
                throw new GekkoException();
            }
            if (this.type != ESeriesType.Light && this.meta.parentDatabank != null && !this.meta.parentDatabank.editable) Program.ProtectError("You cannot change an observation in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");

            if (this.data.GetDataArray_ONLY_INTERNAL_USE() == null)
            {
                this.data.SetDataarray_ONLY_INTERNAL_USE(new double[1]);
            }
            this.data.GetDataArray_ONLY_INTERNAL_USE()[0] = value;
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
        /// This sets the observation (period) to the given value.
        /// </summary>
        /// <param name="t">The period.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="GekkoException">Exception if frequency of timeseries and period do not match.</exception>
        public void SetData(GekkoTime t, double value)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in ResizeDataArray() which is safe
            // ----------------------------------------------------------------------------
            
            if (this.type == ESeriesType.Timeless)
            {
                //Should not normally be used.
                //But this may be called from for instance DECOMP, calling it with a time period
                //Normally timeless variables should be called via the SetData(double value) method
                this.data.GetDataArray_ONLY_INTERNAL_USE()[0] = value;
            }
           
            if (this.type != ESeriesType.Light && this.meta != null && this.meta.parentDatabank != null && !this.meta.parentDatabank.editable) Program.ProtectError("You cannot change an observation in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
            
            if (this.freq != t.freq)
            {
                //See comment to GetData()
                FreqError(t);
            }
            if (this.data.GetDataArray_ONLY_INTERNAL_USE() == null)
            {
                InitDataArray(t);
            }

            if (this.type == ESeriesType.Timeless)
            {
                this.data.GetDataArray_ONLY_INTERNAL_USE()[0] = value;
            }
            else
            {
                //Get the array index corresponding to the period. If this index is out of array bounds, the array will
                //be resized (1.5 times larger).
                int index = ResizeDataArray(t);
                //the index is offset safe
                this.data.GetDataArray_ONLY_INTERNAL_USE()[index] = value;
                //Start and end date for observations are adjusted.
                //for the first obs put into a new timeseries, both the if's should trigger.
                if (this.type != ESeriesType.Light && this.meta != null)
                {
                    if (index > this.meta.lastPeriodPositionInArray)
                    {
                        this.meta.lastPeriodPositionInArray = index;
                    }
                    if (index < this.meta.firstPeriodPositionInArray)
                    {
                        this.meta.firstPeriodPositionInArray = index;
                    }
                }
            }
            if (this.type != ESeriesType.Light) this.SetDirty(true);
        }

        /// <summary>
        /// Gets a pointer to the data array of the timeseries corresponding to the given time period.
        /// </summary>
        /// <param name="index1">The array index corresponding to the start of the period</param>
        /// <param name="index2">The array index corresponding to the end of the period</param>
        /// <param name="gt1">The start of the period.</param>
        /// <param name="gt2">The end of the period.</param>
        /// <param name="setStartEndPeriods">Normally false.</param>
        /// <returns>A pointer to the data array that contains the data. This is a pointer to the REAL data, so
        /// DO NOT change the array values unless you know what you are doing!</returns>
        /// <exception cref="GekkoException">Exception if frequency of timeseries and periods differ.</exception>
        private double[] GetDataSequenceAbstract(out int index1, out int index2, GekkoTime gt1, GekkoTime gt2, bool setStartEndPeriods, bool clone)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetArrayIndex() which is safe
            // ----------------------------------------------------------------------------

            // -------------------------------------------------------------------------------------------------------------------------------
            // ----------------------------- OVERVIEW ofer GetDataSequence... methods            //
            //  + GetDataSequenceAbstract(out int index1, out int index2, GekkoTime gt1, GekkoTime gt2, bool setStartEndPeriods, bool clone)
            // => Not for outside use.
            //
            //  + GetDataSequenceBEWARE(out int index1, out int index2, GekkoTime per1, GekkoTime per2)
            // => Safe method that returns a *copy* of the dataArray, optionally with NaN changed to 0. May resize, safe to change dataArray if convenient. Will not set dirty. Use it for reading if you want to be safe regarding side-effects. Else if sure about side-effects, GetDataSequenceUnsafePointerReadOnlyBEWARE() can be used
            //
            //  + GetDataSequenceUnsafePointerAlterBEWARE()
            // => Used if no period is given, and something needs to be done on the dataArray. Will set dirty, do not use for light timeseries.
            //
            //  + GetDataSequenceUnsafePointerAlterBEWARE(out int index1, out int index2, GekkoTime per1, GekkoTime per2)
            // => Used to alter the dataArray directly. Will set dirty.Similar to below.Similar to above, but with periods and may resize.
            //            
            //  + GetDataSequenceUnsafePointerReadOnlyBEWARE()
            // => Used when no period is given, but something should be read. Will not set dirty, can be used for light timeseries.
            //
            //  + GetDataSequenceUnsafePointerReadOnlyBEWARE(out int index1, out int index2, GekkoTime per1, GekkoTime per2)
            // => Used to access the dataArray directly. May resize, should not change data, will not set dirty. Similar to above.
            //--------------------------------------------------------------------------------   //
            // -------------------------------------------------------------------------------------------------------------------------------

            //

            //All GetDataSequence... except one go through here
            //generic method, not for outside use

            if (this.type == ESeriesType.Timeless)
            {
                int n = GekkoTime.Observations(gt1, gt2);
                double[] numbers = new double[n];
                double d = this.data.GetDataArray_ONLY_INTERNAL_USE()[0];
                if (MissingZero(this))
                {
                    if (G.isNumericalError(d)) d = 0d;
                }
                for (int i = 0; i < n; i++) numbers[i] = d;
                index1 = 0;
                index2 = n - 1;
                return numbers;                
            }

            if (this.freq != gt1.freq)
            {
                //This check: better safe than sorry!
                //See comment to GetData()                
                FreqError(gt1);
            }

            if (gt1.freq != gt2.freq)
            {
                //See comment to GetData()                
                throw new GekkoException();  //should be rare...
            }

            if (this.data.GetDataArray_ONLY_INTERNAL_USE() == null)
            {
                this.InitDataArray(gt1);  //fills a new dataArray with NaN's so no risk regarding offset etc.
            };

            index1 = GetArrayIndex(gt1);
            index2 = GetArrayIndex(gt2);
            int tooSmall, tooLarge;
            TooSmallOrTooLarge(index1, index2, out tooSmall, out tooLarge);

            bool tooSmallOrTooLarge = tooSmall > 0 || tooLarge > 0;
            if (tooSmallOrTooLarge)
            {
                index1 = ResizeDataArray(gt1);
                index2 = ResizeDataArray(gt2);  //this would never change index1                                                
            }

            if (setStartEndPeriods)  //only relevant if the returned arrays is actually tampered with, which is normally NOT the case (only for a[,] and b[] array stuff in simulation)
            {
                if (this.meta != null)
                {
                    if (index2 > this.meta.lastPeriodPositionInArray)
                    {
                        this.meta.lastPeriodPositionInArray = index2;
                    }
                    if (index1 < this.meta.firstPeriodPositionInArray)
                    {
                        this.meta.firstPeriodPositionInArray = index1;
                    }
                }
            }
            if (clone)
            {
                int size = index2 - index1 + 1;
                double[] temp = new double[size];
                Array.Copy(this.data.GetDataArray_ONLY_INTERNAL_USE(), index1, temp, 0, size);
                index1 = 0;
                index2 = temp.Length - 1;  //TT changed 24-9-2018
                if (MissingZero(this)) G.ReplaceNaNWith0(temp);
                return temp;
            }
            else
            {
                return this.data.GetDataArray_ONLY_INTERNAL_USE();
            }
        }

       

        public double[] GetDataSequenceUnsafePointerAlterBEWARE()
        {
            //Regarding the GetDataSequence... methods, see the overview in GetDataSequenceAbstract()

            //Will set dirty, cannot be used for light timeseries for that reason

            //When using this to alter data, or to copy or transform it,
            //beware that this is the actual data array, not a copy of it.
            //Also, if using this, beware of OPTION series data missing.
            //You may have to transform NaN to 0 if this option is set.
            //See also #87943523987543

          
            this.SetDirty(true); //we have to mark dirty manually

            return this.data.GetDataArray_ONLY_INTERNAL_USE();
        }

        public double[] GetDataSequenceUnsafePointerReadOnlyBEWARE()
        {
            //Regarding the GetDataSequence... methods, see the overview in GetDataSequenceAbstract()

            //Will not set dirty, can be used for light timeseries

            //When using this to alter data, or to copy or transform it,
            //beware that this is the actual data array, not a copy of it.
            //Also, if using this, beware of OPTION series data missing.
            //You may have to transform NaN to 0 if this option is set.
            //See also #87943523987543
                                 

            return this.data.GetDataArray_ONLY_INTERNAL_USE();
        }

        public void TooSmallOrTooLarge(int index1, int index2, out int tooSmall, out int tooLarge)
        {
            tooSmall = -index1;
            tooLarge = index2 - (this.data.GetDataArray_ONLY_INTERNAL_USE().Length - 1);
        }

        public void TooSmallOrTooLarge(int index, out int tooSmall, out int tooLarge)
        {
            tooSmall = -index;
            tooLarge = index - (this.data.GetDataArray_ONLY_INTERNAL_USE().Length - 1);
        }

        /// <summary>
        /// Overload with setStartEndPeriods = false.
        /// </summary>
        /// <param name="index1">The array index corresponding to the start of the period</param>
        /// <param name="index2">The array index corresponding to the end of the period</param>
        /// <param name="per1">The start of the period.</param>
        /// <param name="per2">The end of the period.</param>
        /// <returns></returns>
        public double[] GetDataSequenceBEWARE(out int index1, out int index2, GekkoTime per1, GekkoTime per2)
        {
            //Regarding the GetDataSequence... methods, see the overview in GetDataSequenceAbstract()

            //BEWARE: If using this, beware of OPTION series data missing.
            //You may have to transform NaN to 0 if this option is set.
            //See also #87943523987543

            return GetDataSequenceAbstract(out index1, out index2, per1, per2, false, true);
        }

        public double[] GetDataSequenceUnsafePointerReadOnlyBEWARE(out int index1, out int index2, GekkoTime per1, GekkoTime per2)
        {
            //Regarding the GetDataSequence... methods, see the overview in GetDataSequenceAbstract()

            //BEWARE: When using this to alter data, or to copy or transform it,
            //beware that this is the actual data array, not a copy of it.
            //Also, if using this, beware of OPTION series data missing.
            //You may have to transform NaN to 0 if this option is set.
            //See also #87943523987543

            //will also set metadata regarding max-min periods       
            if (this.type == ESeriesType.Timeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #2");
                throw new GekkoException();
            }
            return GetDataSequenceAbstract(out index1, out index2, per1, per2, true, false);
        }

        public double[] GetDataSequenceUnsafePointerAlterBEWARE(out int index1, out int index2, GekkoTime per1, GekkoTime per2)
        {
            //Regarding the GetDataSequence... methods, see the overview in GetDataSequenceAbstract()

            //Just like GetDataSequenceUnsafePointerReadOnlyBEWARE, but setting dirty

            //BEWARE: When using this to alter data, or to copy or transform it,
            //beware that this is the actual data array, not a copy of it.
            //Also, if using this, beware of OPTION series data missing.
            //You may have to transform NaN to 0 if this option is set.
            //See also #87943523987543

            //will also set metadata regarding max-min periods
            if (this.type == ESeriesType.Timeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #2");
                throw new GekkoException();
            }
            this.SetDirty(true); //we have to mark dirty manually
            return GetDataSequenceAbstract(out index1, out index2, per1, per2, true, false);
        }

        public void SetDataSequence(GekkoTime gt1, GekkoTime gt2, double[] input, int inputOffset)
        {
            this.SetDataSequence(gt1, gt2, input, inputOffset, false);
        }

        /// <summary>
        /// For the given timeseries, this sets the data of the given period to array to the values given in the input array. An offset may be used.
        /// </summary>
        /// <param name="gt1">Start of the time period.</param>
        /// <param name="gt2">End of the time period</param>
        /// <param name="input">The input.</param>
        /// <param name="inputOffset">The input offset.</param>
        /// <exception cref="GekkoException">Exception if frequency of timeseries and periods differ.</exception>
        public void SetDataSequence(GekkoTime gt1, GekkoTime gt2, double[] input, int inputOffset, bool replaceNaNWith0)
        {
            // ------------------------------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetArrayIndex() and ResizeDataArray() which are safe
            // ------------------------------------------------------------------------------------------------
                                 
            if (this.type == ESeriesType.Timeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #3");
                throw new GekkoException();
            }
            if (this.meta.parentDatabank != null && !this.meta.parentDatabank.editable) Program.ProtectError("You cannot change observations in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");

            if (this.freq != gt1.freq)
            {
                //This check: better safe than sorry!
                //See comment to GetData()                
                FreqError(gt1);
            }

            if (gt1.freq != gt2.freq)
            {
                //See comment to GetData()                
                throw new GekkoException();  //should be rare...
            }

            if (this.data.GetDataArray_ONLY_INTERNAL_USE() == null)
            {
                //slack: init could be more precise regarding size, for non-annual frequencies.
                GekkoTime gtTemp = new GekkoTime(gt1.freq, (gt1.super + gt2.super) / 2, 1);
                InitDataArray(gtTemp);  //assumes subperiod 1
            }
            int index1 = -12345;
            int index2 = -12345;
            index1 = GetArrayIndex(gt1);
            index2 = GetArrayIndex(gt2);
            if (index1 < 0 || index1 >= this.data.GetDataArray_ONLY_INTERNAL_USE().Length || index2 < 0 || index2 >= this.data.GetDataArray_ONLY_INTERNAL_USE().Length)
            {
                index1 = ResizeDataArray(gt1);
                index2 = ResizeDataArray(gt2); //this would never change index1, since slots are added at the end                            
            }
            System.Array.Copy(input, inputOffset, this.data.GetDataArray_ONLY_INTERNAL_USE(), index1, index2 - index1 + 1);

            if (replaceNaNWith0)
            {
                for (int i = index1; i <= index2; i++)  //=3, i<4, 
                {
                    if (G.isNumericalError(this.data.GetDataArray_ONLY_INTERNAL_USE()[i])) this.data.GetDataArray_ONLY_INTERNAL_USE()[i] = 0d;
                }
            }

            //Adjust start and end data positions.
            if (index2 > this.meta.lastPeriodPositionInArray)
            {
                this.meta.lastPeriodPositionInArray = index2;
            }
            if (index1 < this.meta.firstPeriodPositionInArray)
            {
                this.meta.firstPeriodPositionInArray = index1;
            }
            this.SetDirty(true);

        }

        //public void SetDirtyGhost(bool b1, bool b2)
        //{
        //    this.SetDirty(b1);
        //    this.SetGhost(b2);
        //}        

        //public bool IsTimeless()
        //{
        //    return this.type == ESeriesType.Timeless;
        //}

        //public void SetTimeless()
        //{
        //    //this.isTimeless = true;
        //    this.type = ESeriesType.Timeless
        //    this.dataArray = new double[1];  //wipe anything existing out
        //    this.dataArray[0] = double.NaN;
        //}

        /// <summary>
        /// Overload with no offset.
        /// </summary>
        /// <param name="gt1">Start of the time period.</param>
        /// <param name="gt2">End of the time period</param>
        /// <param name="input">The input.</param>
        public void SetDataSequence(GekkoTime gt1, GekkoTime gt2, double[] input)
        {
            SetDataSequence(gt1, gt2, input, 0);
        }

        /// <summary>
        /// Gets the first observation/data as a period (GekkoTime).
        /// </summary>
        /// <returns>
        /// Period
        /// </returns>
        public GekkoTime GetPeriodFirst()
        {
            //TODO: Implement for array-series
            if (this.type == ESeriesType.Timeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #4");
                throw new GekkoException();
            }
            return GetPeriod(this.meta.firstPeriodPositionInArray);
        }

        /// <summary>
        /// Gets the last observation/data as a period (GekkoTime).
        /// </summary>
        /// <returns>
        /// Period
        /// </returns>
        public GekkoTime GetPeriodLast()
        {
            //TODO: Implement for array-series
            if (this.type == ESeriesType.Timeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #5");
                throw new GekkoException();
            }
            return GetPeriod(this.meta.lastPeriodPositionInArray);
        }
               

        public GekkoTime GetRealDataPeriodFirst()
        {
            //TODO: Implement for array-series
            //Takes some time for large non-trimmed arrays, but is more precise than GetPeriodFirst()
            GekkoTime rv = GekkoTime.tNull;
            if (this.type == ESeriesType.Timeless)
            {
                //do nothing
            }
            else
            {
                if (this.data.GetDataArray_ONLY_INTERNAL_USE() != null)
                {
                    for (int i = 0; i < this.data.GetDataArray_ONLY_INTERNAL_USE().Length; i++)
                    {
                        if (!G.isNumericalError(this.data.GetDataArray_ONLY_INTERNAL_USE()[i]))
                        {
                            rv = GetPeriod(i);
                            break;
                        }
                    }
                }                
            }            
            return rv;
        }

        public GekkoTime GetRealDataPeriodLast()
        {
            //TODO: Implement for array-series
            //Takes some time for large non-trimmed arrays, but is more precise than GetPeriodLast()
            GekkoTime rv = GekkoTime.tNull;
            if (this.type == ESeriesType.Timeless)
            {
                //do nothing
            }
            else
            {
                if (this.data.GetDataArray_ONLY_INTERNAL_USE() != null)
                {
                    for (int i = this.data.GetDataArray_ONLY_INTERNAL_USE().Length - 1; i >= 0; i--)
                    {
                        if (!G.isNumericalError(this.data.GetDataArray_ONLY_INTERNAL_USE()[i]))
                        {
                            rv = GetPeriod(i);
                            break;
                        }
                    }
                }
            }            
            return rv;
        }

        /// <summary>
        /// Gets the period (GekkoTime) corresponding to a particular index in the data array.
        /// </summary>
        /// <param name="indexInDataArray">The index in the data array.</param>
        /// <returns>The period (GekkoTime).</returns>
        public GekkoTime GetPeriod(int indexInDataArray)
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
            if (this.freq == EFreq.Q) subPeriods = 4;
            else if (this.freq == EFreq.M) subPeriods = 12;
            else if (this.freq == EFreq.U) subPeriods = 1;

            if (this.freq == EFreq.D)
            {
                int offset = indexInDataArray - this.GetAnchorPeriodPositionInArray();
                return this.data.anchorPeriod.Add(offset);
            }
            else
            {
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
                return new GekkoTime(this.freq, resultSuperPer, resultSubPer);
            }
            
        }

        // -----------------------------------------------------------------------------
        // ----------------- private methods -------------------------------------------
        // -----------------------------------------------------------------------------

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

        //Not intended for outside use
        public int GetArrayIndex(GekkoTime gt)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetAnchorPeriodPositionInArray()
            // ----------------------------------------------------------------------------

            int rv = FromGekkoTimeToArrayIndexAbstract(gt, new GekkoTime(this.freq, this.data.anchorPeriod.super, this.data.anchorPeriod.sub, this.data.anchorPeriod.subsub), this.GetAnchorPeriodPositionInArray());
            return rv;
        }

        //NOT for outside use, note that it is not safe regarding .dataOffsetLag!
        private static int FromGekkoTimeToArrayIndexAbstract(GekkoTime gt, GekkoTime anchorPeriod, int anchorPeriodPositionInArray)
        {
            // ----------------------------------------------------------------------------
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // !!! NOTE: OFFSET UNSAFE !!!!!!!!!
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // ----------------------------------------------------------------------------

            //========================================================================================================
            //                          FREQUENCY LOCATION, indicates where to implement more frequencies
            //========================================================================================================

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
            if (anchorPeriod.freq == EFreq.A)
            {
                //Special treatment in order to make it fast.
                //undated freq could return fast in the same way as this??
                rv = anchorPeriodPositionInArray + gt.super - anchorPeriod.super;
            }
            else if (anchorPeriod.freq == EFreq.D)
            {
                //this cannot be fast, converts implicitly to C# DateTime
                int dif = GekkoTime.Observations(anchorPeriod, gt) - 1;
                rv = anchorPeriodPositionInArray + dif;
            }
            else
            {
                //Non-annual and non-daily                
                int subPeriods = 1;
                if (anchorPeriod.freq == EFreq.Q) subPeriods = 4;
                else if (anchorPeriod.freq == EFreq.M) subPeriods = 12;
                else if (anchorPeriod.freq == EFreq.U) subPeriods = 1;
                //For quarterly data for instance, each super period amounts to 4 observations. Therefore the multiplication.
                int dif = subPeriods * (gt.super - anchorPeriod.super) + (gt.sub - anchorPeriod.sub);
                int index = anchorPeriodPositionInArray + dif;
                rv = index;
            }

            return rv;
        }        

        public int FromGekkoTimeToArrayIndex(GekkoTime gt)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetAnchorPeriodPositionInArray()
            // ----------------------------------------------------------------------------
            return FromGekkoTimeToArrayIndexAbstract(gt, this.data.anchorPeriod, this.GetAnchorPeriodPositionInArray());
        }        

        private int ResizeDataArray(GekkoTime gt)
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

                    if (IsOffsettedNormalSeries())
                    {
                        //TODO:
                        //
                        // Another and faster way of doing this is that the "shell" object of x[-1] has a
                        // pointer pointing back to the "real" series.
                        
                        //There was the following problem (see unit test here: #79873242834)
                        //a series x was defined, bank was written and read
                        //so now the series array is exactly over 66-2018
                        //Then x was used with a lag which triggerede a resize.
                        //Problem is that the x[-1] variable is a special lagged variable,
                        //using .dataOffsetLag. In a sense this x[-1] object is an empty shell, using 
                        //the same dataarray as x. So when the array is resized and the .anchorPeriodPositionInArray
                        //is changed (because data is added before what the original array spans), this should be
                        //transmitted back to the original x object.
                        //To do this, we would have to keep a pointer back to the x object, but what if we have x[-1] lagged later
                        //on? To avoid such confusion, if the .anchorPeriodPositionInArray is altered in such an "empty shell" object,
                        //we clone the dataarray for it. This will happen rarely anyway.   

                        this.data = this.data.DeepClone();                        

                    }

                    int diffSize = newDataArray.Length - this.data.GetDataArray_ONLY_INTERNAL_USE().Length;
                    System.Array.Copy(this.data.GetDataArray_ONLY_INTERNAL_USE(), 0, newDataArray, diffSize, this.data.GetDataArray_ONLY_INTERNAL_USE().Length);
                    this.data.anchorPeriodPositionInArray += diffSize;

                    if (this.meta != null)  //should never happen after the #79873242834 fix
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

                //this.data.GetDataArray_ONLY_FOR_INTERNAL_USE() = newDataArray;
                //this.data.SetDataArray_ONLY_FOR_INTERNAL_USE(newDataArray);

                this.data.SetDataarray_ONLY_INTERNAL_USE(newDataArray);
                index = GetArrayIndex(gt);
            }
            return index;
        }

        private bool IsOffsettedNormalSeries()
        {
            return this.name == null && this.meta == null;
        }

        private void InitDataArray(GekkoTime t)
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
        
        public static string GetHashCodeFromIvariables(IVariable[] indexes)
        {
            string hash = null;
            for (int i = 0; i < indexes.Length; i++)
            {
                if (indexes[i].Type() != EVariableType.String)
                {
                    G.Writeln2("*** ERROR: Expected [] indexer element #" + (i + 1) + " to be STRING");
                    throw new GekkoException();
                }
                hash += ((ScalarString)indexes[i]).string2;
                if (i < indexes.Length - 1) hash += Globals.symbolTurtle; //ok as delimiter
            }
            return hash;
        }

        public static string GetHashCodeFromIvariables(string[] indexes)
        {
            string hash = null;
            for (int i = 0; i < indexes.Length; i++)
            {
                hash += indexes[i];
                if (i < indexes.Length - 1) hash += Globals.symbolTurtle; //ok as delimiter
            }
            return hash;
        }

        //minus, abs(), log(), exp(), sqrt()
        public static Series ArithmeticsSeries(GekkoSmpl smpl, Series x1_series, Func<double, double> a)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in ResizeDataArray() which is safe
            // ----------------------------------------------------------------------------

            Series rv_series;
            if (x1_series.type == ESeriesType.Normal || x1_series.type == ESeriesType.Timeless)
            {
                rv_series = new Series(ESeriesType.Light, smpl.t0, smpl.t3);

                if (Globals.bugfix_speedup && x1_series.type != ESeriesType.Timeless)
                {
                    GekkoTime window1 = smpl.t0;
                    GekkoTime window2 = smpl.t3;

                    int ia1 = rv_series.ResizeDataArray(window1); //t0
                    int ia2 = rv_series.ResizeDataArray(window2);  //t3
                    int ib1 = x1_series.ResizeDataArray(window1);  //t0
                    int ib2 = x1_series.ResizeDataArray(window2);  //t3
                    double[] arraya = rv_series.data.GetDataArray_ONLY_INTERNAL_USE();
                    double[] arrayb = x1_series.data.GetDataArray_ONLY_INTERNAL_USE();
                    bool b = MissingZero(x1_series);
                    for (int i = 0; i < GekkoTime.Observations(window1, window2); i++)
                    {
                        double d = arrayb[i + ib1];
                        if (b && G.isNumericalError(d)) d = 0d;
                        arraya[i + ia1] = a(d);
                    }
                }
                else
                {
                    foreach (GekkoTime t in smpl.Iterate03())
                    {
                        rv_series.SetData(t, a(x1_series.GetData(smpl, t)));
                    }
                }
            }
            else if (x1_series.type == ESeriesType.Light)
            {
                //safe to alter the object itself, since it is temporary
                bool b = MissingZero(x1_series);
                for (int i = 0; i < x1_series.data.GetDataArray_ONLY_INTERNAL_USE().Length; i++)
                {
                    double d = x1_series.data.GetDataArray_ONLY_INTERNAL_USE()[i];
                    if (b && G.isNumericalError(d)) d = 0d;
                    x1_series.data.GetDataArray_ONLY_INTERNAL_USE()[i] = a(d);
                }
                rv_series = x1_series;
            }
            else
            {
                G.Writeln2("*** ERROR: Internal error #4598243755");
                throw new GekkoException();
            }

            return rv_series;
        }

        //pch(), dlog(), dif()
        public static Series ArithmeticsSeriesLag(GekkoSmpl smpl, Series x1_series, Func<double, double, double> a, int lag)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in ResizeDataArray() which is safe
            // ----------------------------------------------------------------------------
            
            //#9083245058

            int xx = -lag;
            if (Globals.lagfix) xx = 0;  //the return should have no lags, it is only the internals that are lagged

            //Functions like d() and pch() where lag is used
            Series rv_series;
            rv_series = new Series(ESeriesType.Light, smpl.t0.Add(xx), smpl.t3); //should return a seies corresponding to t0-t3

            if (x1_series.type == ESeriesType.Normal || x1_series.type == ESeriesType.Timeless)
            {
                if (Globals.bugfix_speedup && x1_series.type != ESeriesType.Timeless)
                {
                    GekkoTime window1 = smpl.t0.Add(xx); //should return a seies corresponding to t0-t3                    
                    GekkoTime window2 = smpl.t3;

                    int ia1 = rv_series.ResizeDataArray(window1);  //t0
                    int ia2 = rv_series.ResizeDataArray(window2);  //t3 -----------> note: this cannot change ia1, array is enlarged not moved around
                    int ib1 = x1_series.ResizeDataArray(window1);  //t0
                    int ib2 = x1_series.ResizeDataArray(window2);  //t3 -----------> note: this cannot change ib1, array is enlarged not moved around
                    double[] arraya = rv_series.data.GetDataArray_ONLY_INTERNAL_USE();
                    double[] arrayb = x1_series.data.GetDataArray_ONLY_INTERNAL_USE();
                    bool b = MissingZero(x1_series);
                    for (int i = 0; i < GekkoTime.Observations(window1, window2); i++)
                    {
                        double d1 = arrayb[i + ib1];
                        double d2 = arrayb[i + ib1 - lag];
                        if (b)
                        {
                            if (G.isNumericalError(d1)) d1 = 0d;
                            if (G.isNumericalError(d2)) d2 = 0d;                            
                        }
                        arraya[i + ia1] = a(d1, d2);
                    }
                }
                else
                {
                    foreach (GekkoTime t in smpl.Iterate03())
                    {
                        //timeless will just be 0 for dif()?
                        rv_series.SetData(t, a(x1_series.GetData(smpl, t), x1_series.GetData(smpl, t.Add(-lag))));
                    }
                }
            }
            else if (x1_series.type == ESeriesType.Light)
            {
                //safe to alter the object itself, since it is temporary                
                double[] temp = new double[x1_series.data.GetDataArray_ONLY_INTERNAL_USE().Length];
                for (int i = 0; i < lag; i++)
                {
                    temp[i] = double.NaN;
                }
                bool b = MissingZero(x1_series);
                for (int i = 0 + lag; i < x1_series.data.GetDataArray_ONLY_INTERNAL_USE().Length; i++)
                {

                    double d1 = x1_series.data.GetDataArray_ONLY_INTERNAL_USE()[i];
                    double d2 = x1_series.data.GetDataArray_ONLY_INTERNAL_USE()[i - lag];
                    if (b)
                    {
                        if (G.isNumericalError(d1)) d1 = 0d;
                        if (G.isNumericalError(d2)) d2 = 0d;                        
                    }
                    temp[i] = a(d1, d2);
                }
                x1_series.data.SetDataarray_ONLY_INTERNAL_USE(temp);  //has same size and same anchors            
                rv_series = x1_series;
            }
            else
            {
                G.Writeln2("*** ERROR: Internal error #4598243756");
                throw new GekkoException();
            }
            return rv_series;
        }

        public static Series ArithmeticsSeriesVal(GekkoSmpl smpl, Series x1_series, double x2_val, Func<double, double, double> a)
        {            
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in ResizeDataArray() and GetStartEndPeriod() which are safe
            // ----------------------------------------------------------------------------

            if (x1_series.type == ESeriesType.ArraySuper)
            {
                return ArithmeticsArraySeriesVal(smpl, x1_series, x2_val, a);
            }
            else
            {
                Series rv_series;
                GekkoTime window1, window2, windowNew1, windowNew2;
                InitWindows(out window1, out window2, out windowNew1, out windowNew2);

                GetStartEndPeriod(smpl, x1_series, ref window1, ref window2); //if light series, the returned period corresponds to array size, else smpl window is used

                rv_series = new Series(ESeriesType.Light, window1, window2);  //also checks that nobs > 0            

                // ---------------------------
                // x2 is a VAL or MATRIX 1x1
                // ---------------------------

                if (Globals.bugfix_speedup && x1_series.type != ESeriesType.Timeless)
                {
                    int ia1 = rv_series.ResizeDataArray(window1);
                    int ia2 = rv_series.ResizeDataArray(window2);

                    int ib1 = x1_series.ResizeDataArray(window1);
                    int ib2 = x1_series.ResizeDataArray(window2);

                    double[] arraya = rv_series.data.GetDataArray_ONLY_INTERNAL_USE();
                    double[] arrayb = x1_series.data.GetDataArray_ONLY_INTERNAL_USE();

                    bool b = MissingZero(x1_series);
                    for (int i = 0; i < GekkoTime.Observations(window1, window2); i++)
                    {
                        double d = arrayb[i + ib1];
                        if (b && G.isNumericalError(d)) d = 0d;
                        arraya[i + ia1] = a(d, x2_val);
                    }
                }
                else
                {
                    foreach (GekkoTime t in new GekkoTimeIterator(window1, window2))
                    {
                        rv_series.SetData(t, a(x1_series.GetData(smpl, t), x2_val));
                    }
                }

                return rv_series;
            }
        }

        private static Series ArithmeticsSeriesSeries(GekkoSmpl smpl, Series x1_series, Series x2_series, Func<double, double, double> a)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in ResizeDataArray() and GetStartEndPeriod() which are safe
            // ----------------------------------------------------------------------------
            
            if (x1_series.type == ESeriesType.ArraySuper && x2_series.type == ESeriesType.ArraySuper)
            {
                return ArithmeticsArraySeriesArraySeries(smpl, x1_series, x2_series, a);                
            }
            else if (x1_series.type == ESeriesType.ArraySuper)
            {
                //first array, last normal
                return ArithmeticsArraySeriesSeries(smpl, x1_series, x2_series, a);
            }
            else if (x2_series.type == ESeriesType.ArraySuper)
            {
                //first normal, last arary
                return ArithmeticsSeriesArraySeries(smpl, x1_series, x2_series, a);
            }
            else
            {
                //both normal series
                Series rv_series;
                GekkoTime window1, window2, windowNew1, windowNew2;
                InitWindows(out window1, out window2, out windowNew1, out windowNew2);

                //if smpl freq and x1/x2_series freq are the same,
                //these windows will just be smpl.t0 to smpl.t3
                //both for normal and light series. So common
                //window will also be .t0 to .t3.
                //-------------------------------------
                GetStartEndPeriod(smpl, x1_series, ref window1, ref window2); //if light series, the returned period corresponds to array size, else smpl window is used
                GetStartEndPeriod(smpl, x2_series, ref windowNew1, ref windowNew2); //if light series, the returned period corresponds to array size, else smpl window is used
                FindCommonWindow(ref window1, ref window2, windowNew1, windowNew2);
                //-------------------------------------

                rv_series = new Series(ESeriesType.Light, window1, window2);  //also checks that nobs > 0   ---> this will most often be smpl.t0 to smpl.t3         

                // -------------------
                // x2 is a SERIES
                // -------------------

                //Using Func<> instead of for instance raw '+' uses a bit more than double the time for simple double addition.
                //But when dealing with GetData(), SetData() etc. the difference can not be seen.
                //So for practical purposes, Func<> here does not cost performance.
                //If raw arrays were being used over large samples, perhaps the difference would manifest.

                if (Globals.bugfix_speedup && x1_series.type != ESeriesType.Timeless && x2_series.type != ESeriesType.Timeless)
                {
                    int ia1 = rv_series.ResizeDataArray(window1);
                    int ia2 = rv_series.ResizeDataArray(window2);

                    int ib1 = x1_series.ResizeDataArray(window1);
                    int ib2 = x1_series.ResizeDataArray(window2);

                    int ic1 = x2_series.ResizeDataArray(window1);
                    int ic2 = x2_series.ResizeDataArray(window2);

                    double[] arraya = rv_series.data.GetDataArray_ONLY_INTERNAL_USE();
                    double[] arrayb = x1_series.data.GetDataArray_ONLY_INTERNAL_USE();
                    double[] arrayc = x2_series.data.GetDataArray_ONLY_INTERNAL_USE();

                    bool b1 = MissingZero(x1_series);
                    bool b2 = MissingZero(x2_series);

                    for (int i = 0; i < GekkoTime.Observations(window1, window2); i++)
                    {
                        double d1 = arrayb[i + ib1];
                        double d2 = arrayc[i + ic1];
                        if (b1)
                        {
                            if (G.isNumericalError(d1)) d1 = 0d;
                        }
                        if (b2)
                        {                            
                            if (G.isNumericalError(d2)) d2 = 0d;
                        }
                        arraya[i + ia1] = a(d1, d2);
                    }
                }
                else
                {
                    foreach (GekkoTime t in new GekkoTimeIterator(window1, window2))
                    {
                        rv_series.SetData(t, a(x1_series.GetData(smpl, t), x2_series.GetData(smpl, t)));
                    }
                }

                return rv_series;
            }
        }

        private static Series ArithmeticsArraySeriesVal(GekkoSmpl smpl, Series x1_series, double x2_val, Func<double, double, double> a)
        {
            Series temp = new Series(ESeriesType.ArraySuper, x1_series.freq, G.Chop_AddFreq("temp", G.GetFreq(x1_series.freq)), x1_series.dimensions);
            temp.meta = new SeriesMetaInformation();
            temp.data = new SeriesDataInformation();

            List<MapMultidimItem> keys1 = x1_series.dimensionsStorage.storage.Keys.ToList();            

            keys1.Sort(Program.CompareMapMultidimItems);            

            for (int i = 0; i < keys1.Count; i++)
            {
                MapMultidimItem mm1 = keys1[i];                
                
                Series sub1 = x1_series.dimensionsStorage.storage[mm1] as Series;
                
                Series sub = new Series(ESeriesType.Normal, sub1.freq, Globals.seriesArraySubName + Globals.freqIndicator + G.GetFreq(sub1.freq));
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    sub.SetData(t, a(sub1.GetData(smpl, t), x2_val));
                }
                temp.dimensionsStorage.AddIVariableWithOverwrite(mm1, sub);

                //For safety, we clone the domain array of strings.
                //We first try to steal domains from x1, then from x2
                //If these do not have domains, the resulting series is domain-less too
                if (x1_series.meta != null && x1_series.meta.domains != null)
                {
                    temp.meta.domains = new string[x1_series.meta.domains.Length];
                    for (int ii = 0; ii < x1_series.meta.domains.Length; ii++)
                    {
                        temp.meta.domains[ii] = x1_series.meta.domains[ii];
                    }
                }                
            }
            temp.SetDirty(true);
            return temp;
        }

        private static Series ArithmeticsArraySeriesSeries(GekkoSmpl smpl, Series x1_series, Series x2_series, Func<double, double, double> a)
        {
            Series temp = new Series(ESeriesType.ArraySuper, x1_series.freq, G.Chop_AddFreq("temp", G.GetFreq(x1_series.freq)), x1_series.dimensions);
            temp.meta = new SeriesMetaInformation();
            temp.data = new SeriesDataInformation();

            List<MapMultidimItem> keys1 = x1_series.dimensionsStorage.storage.Keys.ToList();

            keys1.Sort(Program.CompareMapMultidimItems);

            for (int i = 0; i < keys1.Count; i++)
            {
                MapMultidimItem mm1 = keys1[i];

                Series sub1 = x1_series.dimensionsStorage.storage[mm1] as Series;

                Series sub = new Series(ESeriesType.Normal, sub1.freq, Globals.seriesArraySubName + Globals.freqIndicator + G.GetFreq(sub1.freq));
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    sub.SetData(t, a(sub1.GetData(smpl, t), x2_series.GetData(smpl, t)));
                }
                temp.dimensionsStorage.AddIVariableWithOverwrite(mm1, sub);

                //For safety, we clone the domain array of strings.
                //We first try to steal domains from x1, then from x2
                //If these do not have domains, the resulting series is domain-less too
                if (x1_series.meta != null && x1_series.meta.domains != null)
                {
                    temp.meta.domains = new string[x1_series.meta.domains.Length];
                    for (int ii = 0; ii < x1_series.meta.domains.Length; ii++)
                    {
                        temp.meta.domains[ii] = x1_series.meta.domains[ii];
                    }
                }
            }
            temp.SetDirty(true);
            return temp;
        }

        private static Series ArithmeticsSeriesArraySeries(GekkoSmpl smpl, Series x1_series, Series x2_series, Func<double, double, double> a)
        {
            Series temp = new Series(ESeriesType.ArraySuper, x2_series.freq, G.Chop_AddFreq("temp", G.GetFreq(x2_series.freq)), x2_series.dimensions);
            temp.meta = new SeriesMetaInformation();
            temp.data = new SeriesDataInformation();

            List<MapMultidimItem> keys1 = x2_series.dimensionsStorage.storage.Keys.ToList();

            keys1.Sort(Program.CompareMapMultidimItems);

            for (int i = 0; i < keys1.Count; i++)
            {
                MapMultidimItem mm1 = keys1[i];

                Series sub1 = x2_series.dimensionsStorage.storage[mm1] as Series;

                Series sub = new Series(ESeriesType.Normal, sub1.freq, Globals.seriesArraySubName + Globals.freqIndicator + G.GetFreq(sub1.freq));
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    sub.SetData(t, a(x1_series.GetData(smpl, t), sub1.GetData(smpl, t)));
                }
                temp.dimensionsStorage.AddIVariableWithOverwrite(mm1, sub);

                //For safety, we clone the domain array of strings.
                //We first try to steal domains from x1, then from x2
                //If these do not have domains, the resulting series is domain-less too
                if (x2_series.meta != null && x2_series.meta.domains != null)
                {
                    temp.meta.domains = new string[x2_series.meta.domains.Length];
                    for (int ii = 0; ii < x2_series.meta.domains.Length; ii++)
                    {
                        temp.meta.domains[ii] = x2_series.meta.domains[ii];
                    }
                }
            }
            temp.SetDirty(true);
            return temp;
        }

        private static Series ArithmeticsArraySeriesArraySeries(GekkoSmpl smpl, Series x1_series, Series x2_series, Func<double, double, double> a)
        {
            //SOMETHING FISHY HERE, when domains do not match
            //Make better check of matching domains, and how to use #default list.
            //#894543543543

            //This is typically used for printing differences
            if (x1_series.dimensions != x2_series.dimensions)
            {
                G.Writeln2("*** ERROR: The two array-series have different number of dimensions (" + x1_series.dimensions + " vs " + x2_series.dimensions + ")");
                throw new GekkoException();
            }

            Series temp = new Series(ESeriesType.ArraySuper, x1_series.freq, G.Chop_AddFreq("temp", G.GetFreq(x1_series.freq)), x1_series.dimensions);
            temp.meta = new SeriesMetaInformation();
            temp.data = new SeriesDataInformation();

            List<MapMultidimItem> keys1 = x1_series.dimensionsStorage.storage.Keys.ToList();
            List<MapMultidimItem> keys2 = x2_series.dimensionsStorage.storage.Keys.ToList();

            keys1.Sort(Program.CompareMapMultidimItems);
            keys2.Sort(Program.CompareMapMultidimItems);

            List m0 = new List(); //subseries first
            List m1 = new List(); //subseries ref

            //#98075grane

            if (keys1.Count != keys2.Count)
            {
                G.Writeln2("*** ERROR: The two array-series have different number of elements (" + keys1.Count + " vs " + keys2.Count + ")");
                throw new GekkoException();
            }

            for (int i = 0; i < keys1.Count; i++)
            {
                MapMultidimItem mm1 = keys1[i];
                MapMultidimItem mm2 = keys2[i];
                if (!mm1.Equals(mm2))
                {
                    G.Writeln2("*** ERROR: Non-corresponding elements [" + mm1.ToString() + "] and [" + mm2.ToString() + "]");
                    throw new GekkoException();
                }
                Series sub1 = x1_series.dimensionsStorage.storage[mm1] as Series;
                Series sub2 = x2_series.dimensionsStorage.storage[mm2] as Series;
                Series sub = new Series(ESeriesType.Normal, sub1.freq, Globals.seriesArraySubName + Globals.freqIndicator + G.GetFreq(sub1.freq));
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    sub.SetData(t, a(sub1.GetData(smpl, t), sub2.GetData(smpl, t)));
                }
                temp.dimensionsStorage.AddIVariableWithOverwrite(mm1, sub);

                //For safety, we clone the domain array of strings.
                //We first try to steal domains from x1, then from x2
                //If these do not have domains, the resulting series is domain-less too
                if (x1_series.meta != null && x1_series.meta.domains != null)
                {
                    temp.meta.domains = new string[x1_series.meta.domains.Length];
                    for (int ii = 0; ii < x1_series.meta.domains.Length; ii++)
                    {
                        temp.meta.domains[ii] = x1_series.meta.domains[ii];
                    }
                }
                else if (x2_series.meta != null && x2_series.meta.domains != null)
                {
                    temp.meta.domains = new string[x2_series.meta.domains.Length];
                    for (int ii = 0; ii < x2_series.meta.domains.Length; ii++)
                    {
                        temp.meta.domains[ii] = x2_series.meta.domains[ii];
                    }
                }
            }
            temp.SetDirty(true);
            return temp;
        }

        private static void InitWindows(out GekkoTime window1, out GekkoTime window2, out GekkoTime windowNew1, out GekkoTime windowNew2)
        {
            window1 = GekkoTime.tNull;
            window2 = GekkoTime.tNull;
            windowNew1 = GekkoTime.tNull;
            windowNew2 = GekkoTime.tNull;
        }

        private static void FindCommonWindow(ref GekkoTime window1, ref GekkoTime window2, GekkoTime windowNew1, GekkoTime windowNew2)
        {
            if (windowNew1.StrictlySmallerThan(window1)) window1 = windowNew1;
            if (windowNew2.StrictlyLargerThan(window2)) window2 = windowNew2;
        }

        private void PrepareInput(GekkoSmpl smpl, IVariable input, out Series x1, out Series x2_series, out double x2_val)
        {
            x1 = this;
            IVariable x2 = input;  //unknown type
            x2_val = double.NaN;
            x2_series = input as Series;
            if (x2_series == null)
            {
                if (x2.Type() == EVariableType.List)
                {
                    //For instance y = x + (1, 2, 3), returns a series light for (1, 2, 3), over local sample, and with same freq as sample
                    List x2_list = x2 as List;
                    if (x2_list.Count() != smpl.Observations12())
                    {
                        G.Writeln2("*** ERROR: List with " + x2_list.Count() + " elements, expected " + smpl.Observations12() + " elements corresponding to " + smpl.t1.ToString() + "-" + smpl.t2.ToString());
                        throw new GekkoException();
                    }
                    Series ts = new Series(ESeriesType.Light, smpl.t1.Add(-Globals.smplOffset), smpl.t2); //new series light
                    int i = -1;
                    foreach (GekkoTime t in smpl.Iterate12())
                    {
                        i++;
                        double d = x2_list.list[i].ConvertToVal();
                        ts.SetData(t, d);
                    }
                    x2_series = ts;
                }
                else if (x2.Type() == EVariableType.Matrix && ((Matrix)x2).data.Length > 1)
                {
                    //a matrix, not 1x1 (caught below)
                    //For instance y = x + [1; 2; 3], returns a series light for [1; 2; 3], over local sample, and with same freq as sample
                    Matrix x2_matrix = x2 as Matrix;
                    if (x2_matrix.data.GetLength(0) != smpl.Observations12() || x2_matrix.data.GetLength(1) != 1)
                    {
                        G.Writeln2("*** ERROR: " + x2_matrix.DimensionsAsString() + " elements, expected a " + smpl.Observations12() + " x 1 matrix corresponding to " + smpl.t1.ToString() + "-" + smpl.t2.ToString());
                        throw new GekkoException();
                    }
                    Series ts = new Series(ESeriesType.Light, smpl.t1.Add(-Globals.smplOffset), smpl.t2);  //new series light
                    
                    int i = -1;
                    foreach (GekkoTime t in smpl.Iterate12())
                    {
                        i++;
                        double d = x2_matrix.data[i, 0];
                        ts.SetData(t, d);
                    }
                    x2_series = ts;
                }
                else
                {
                    x2_val = x2.ConvertToVal();  //VAL or 1x1 MATRIX is ok
                }
            }
            else
            {
                if (x1.freq != x2_series.freq)
                {
                    G.Writeln2("*** ERROR: Frequencies do not match: " + G.GetFreqString(x1.freq) + " vs " + G.GetFreqString(x2_series.freq));
                    throw new GekkoException();
                }
            }
        }

        private static void GetStartEndPeriod(GekkoSmpl smpl, Series x1, ref GekkoTime window1, ref GekkoTime window2)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled in GetAnchorPeriodPositionInArray()
            // ----------------------------------------------------------------------------
            
            if (x1.type == ESeriesType.Light)
            {
                window1 = x1.data.anchorPeriod.Add(-x1.GetAnchorPeriodPositionInArray());
                window2 = window1.Add(x1.data.GetDataArray_ONLY_INTERNAL_USE().Length - 1);

                if (Globals.runningOnTTComputer)
                {
                    //#08753205743
                    GekkoTime wwwindow1 = GekkoTime.tNull;
                    GekkoTime wwwindow2 = GekkoTime.tNull;
                    GekkoTime.ConvertFreqs(x1.freq, smpl.t0, smpl.t3, ref wwwindow1, ref wwwindow2);
                    if (window1.IsSamePeriod(wwwindow1) && window2.IsSamePeriod(wwwindow2))
                    {
                        //G.Writeln("----> window test ok", Color.Gray);
                    }
                    else
                    {
                        G.Writeln("*** ERROR: TT NOTE: Window test, #08753205743");
                        //throw new GekkoException();
                    }
                }

            }
            else
            {
                GekkoTime.ConvertFreqs(x1.freq, smpl.t0, smpl.t3, ref window1, ref window2);
            }
        }

        public IVariable Add(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;

            //-------------------------------------
            //x1 = SERIES (this)
            //x2 = SERIES or VAL or MATRIX 1x1 (input)
            //-------------------------------------

            Series x1_series, x2_series; double x2_val;
            PrepareInput(smpl, input, out x1_series, out x2_series, out x2_val);

            Func<double, double, double> a = Globals.arithmentics[0];  //(x1, x2) => x1 + x2;

            Series rv_series = null;
            if (x2_series != null) rv_series = ArithmeticsSeriesSeries(smpl, x1_series, x2_series, a);
            else rv_series = ArithmeticsSeriesVal(smpl, x1_series, x2_val, a);
            return rv_series;
        }

        public IVariable Subtract(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;

            //-------------------------------------
            //x1 = SERIES (this)
            //x2 = SERIES or VAL or MATRIX 1x1 (input)
            //-------------------------------------

            Series x1_series, x2_series; double x2_val;
            PrepareInput(smpl, input, out x1_series, out x2_series, out x2_val);

            Func<double, double, double> a = Globals.arithmentics[2]; //(x1, x2) => x1 - x2;

            Series rv_series = null;
            if (x2_series != null) rv_series = ArithmeticsSeriesSeries(smpl, x1_series, x2_series, a);
            else rv_series = ArithmeticsSeriesVal(smpl, x1_series, x2_val, a);
            return rv_series;
        }

        public IVariable Multiply(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;

            //-------------------------------------
            //x1 = SERIES (this)
            //x2 = SERIES or VAL or MATRIX 1x1 (input)
            //-------------------------------------

            Series x1_series, x2_series; double x2_val;
            PrepareInput(smpl, input, out x1_series, out x2_series, out x2_val);

            Func<double, double, double> a = Globals.arithmentics[4]; //(x1, x2) => x1 * x2;

            Series rv_series = null;
            if (x2_series != null) rv_series = ArithmeticsSeriesSeries(smpl, x1_series, x2_series, a);
            else rv_series = ArithmeticsSeriesVal(smpl, x1_series, x2_val, a);
            return rv_series;
        }

        public IVariable Divide(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;
            //-------------------------------------
            //x1 = SERIES (this)
            //x2 = SERIES or VAL or MATRIX 1x1 (input)
            //-------------------------------------

            Series x1_series, x2_series; double x2_val;
            PrepareInput(smpl, input, out x1_series, out x2_series, out x2_val);

            Func<double, double, double> a = Globals.arithmentics[6]; //(x1, x2) => x1 / x2;

            Series rv_series = null;
            if (x2_series != null) rv_series = ArithmeticsSeriesSeries(smpl, x1_series, x2_series, a);
            else rv_series = ArithmeticsSeriesVal(smpl, x1_series, x2_val, a);
            return rv_series;
        }

        public IVariable Power(GekkoSmpl smpl, IVariable input)
        {
            if (G.IsGekkoNull(input)) return input;

            //-------------------------------------
            //x1 = SERIES (this)
            //x2 = SERIES or VAL or MATRIX 1x1 (input)
            //-------------------------------------

            Series x1_series, x2_series; double x2_val;
            PrepareInput(smpl, input, out x1_series, out x2_series, out x2_val);

            Func<double, double, double> a = Globals.arithmentics[8]; //(x1, x2) => Math.Pow(x1, x2);

            Series rv_series = null;
            if (x2_series != null) rv_series = ArithmeticsSeriesSeries(smpl, x1_series, x2_series, a);
            else rv_series = ArithmeticsSeriesVal(smpl, x1_series, x2_val, a);
            return rv_series;
        }

        public IVariable Negate(GekkoSmpl smpl)
        {            
            return ArithmeticsSeries(smpl, this, Globals.arithmentics1[0]); // (x1) => -x1;
        }
        
        public static void FindLagLeadOrFixedPeriod(ref int i, ref GekkoTime t, params IVariable[] indexes)
        {
            if (indexes.Length == 1)
            {
                if (indexes[0].Type() == EVariableType.Val)
                {
                    i = O.ConvertToInt(indexes[0]);
                }
                else if (indexes[0].Type() == EVariableType.Date)
                {
                    t = ((ScalarDate)indexes[0]).date;
                }                
            }
            return;
        }

        public IVariable Indexer(GekkoSmpl smpl, O.EIndexerType indexerType, params IVariable[] indexes)
        {
            // ----------------------------------------------------------------------------
            // OFFSET SAFE: dataOffsetLag is handled directly
            // ----------------------------------------------------------------------------

            IVariable rv = null;

            if (this.type == ESeriesType.ArraySuper)
            {
                if (indexerType == O.EIndexerType.None)
                {
                    rv = this.FindArraySeries(smpl, indexes, false, false, null);  //last arg. not used
                }
                else
                {
                    G.Writeln2("*** ERROR: Direct lagging/leading of an arrayseries not yet possible");
                    throw new GekkoException();
                }
            }
            else {

                //normal series

                int i = -12345; GekkoTime t = GekkoTime.tNull;
                Series.FindLagLeadOrFixedPeriod(ref i, ref t, indexes);  //issues error if for instance x[-2.4] or x[+3.1]

                if (indexerType == O.EIndexerType.IndexerLag || indexerType == O.EIndexerType.IndexerLead || (indexerType == O.EIndexerType.Dot && i != -12345))
                {
                    //This is a lag or lead, [-...] or [+...]
                    //TODO: Broken lags!!

                    if (!IsLagOrLead(i))
                    {
                        G.Writeln2("*** ERROR: lags or leads should be in the interval [-99, 99].");
                        throw new GekkoException();
                    }

                    if (this.type == ESeriesType.Timeless)
                    {
                        rv = this;  //no effect of lag/lead
                    }
                    else
                    {
                        Series temp = new Series(this.type, this.freq);  //This series gets the same type, so if it is Normal and access is outside dataArray, it can safely return a NaN.
                        //The two below correspond to just moving pointers
                        temp.data = this.data;
                        int ii = i;
                        if (indexerType == O.EIndexerType.Dot) ii = -i;
                        //BEWARE: Be careful when using .dataOffsetLag! #772439872435
                        temp.dataOffsetLag = this.dataOffsetLag + ii;
                        rv = temp;
                    }
                }
                else
                {
                    //not a lag or lead, [-...] or [+...]

                    if (!t.IsNull())
                    {
                        //x[2020a], x[2020u], x[2020q2]
                        double d = this.GetData(smpl, t);
                        rv = new ScalarVal(d);
                    }
                    else if (i != -12345)
                    {
                        //x[2020]
                        if (this.freq == EFreq.A || this.freq == EFreq.U)
                        {
                            double d = this.GetData(smpl, new GekkoTime(this.freq, i, 1));
                            rv = new ScalarVal(d);
                        }
                        else
                        {
                            G.Writeln2("*** ERROR: You cannot index " + G.GetFreqString(this.freq) + " series with value " + i);
                            throw new GekkoException();
                        }
                    }
                    else
                    {
                        //not a single-dimensional time index (or lag/lead)
                        var temp = Program.GetListOfStringsFromListOfIvariables(indexes);
                        if (temp != null)
                        {
                            G.Writeln2("*** ERROR: Could not understand index " + this.name + "[" + G.GetListWithCommas(temp) + "]");
                        }
                        else
                        {
                            G.Writeln2("*** ERROR: Could not understand index " + this.name + "[...]");
                        }
                        throw new GekkoException();
                    }
                }                
            }  //end of non-arraysuper
            
            return rv;
        }

        private bool Type(ESeriesType type)
        {
            //type is not used here, just a decorator
            return this.name == null;  //then this.meta will also be null, but we only test .name
        }

        public IVariable FindArraySeries(GekkoSmpl smpl, IVariable[] indexes, bool isLhs, bool rhsIsTimeless, LookupSettings settings)
        {
            if (indexes.Length == 0)
            {
                G.Writeln2("*** ERROR: Indexer has 0 length");
                throw new GekkoException();
            }
            IVariable rv = null;

            string[] keys = Program.GetListOfStringsFromListOfIvariables(indexes);

            if (keys == null)
            {
                //FAIL
                string s = null;
                foreach (IVariable iv in indexes)
                {
                    s += iv.Type().ToString() + ", ";
                }
                G.Writeln2("*** ERROR: Series []-index with these argument types: " + s.Substring(0, s.Length - (", ").Length));
                throw new GekkoException();
            }

            rv = FindArraySeriesHelper(smpl, isLhs, keys, rhsIsTimeless, settings);

            return rv;
        }

        private IVariable FindArraySeriesHelper(GekkoSmpl smpl, bool isLhs, string[] keys, bool rhsIsTimeless, LookupSettings settings)
        {
            IVariable rv = null;
            if (true)
            {

                if (this.dimensionsStorage == null)
                {
                    string txt = null; foreach (string ss in keys) txt += "'" + ss + "', ";
                    G.Writeln2("*** ERROR: The variable '" + this.meta.parentDatabank.name + ":" + this.name + "' is not an array-timeseries.");
                    G.Writeln("           Indexer used: [" + txt.Substring(0, txt.Length - 2) + "]", Color.Red);
                    G.Writeln("           You may use '" + this.name + " = series(" + keys.Length + ");' to create it,", Color.Red);
                    G.Writeln("           perhaps with 'CREATE " + this.name + ";' first.", Color.Red);
                    throw new GekkoException();
                }

                IVariable iv = null;
                if (this.dimensions != keys.Length)
                {
                    G.Writeln("*** ERROR: " + keys.Length + " dimensional index used on " + this.dimensions + "-dimensional array-timeseries " + G.GetNameAndFreqPretty(this.name));
                    throw new GekkoException();
                }
                this.dimensionsStorage.TryGetValue(new MapMultidimItem(keys), out iv);

                if (iv == null)
                {
                    if (!isLhs)
                    {

                        //on the RHS
                        
                        if (smpl != null && smpl.command == GekkoSmplCommand.Unfold)
                        {
                            //print
                            if (Program.options.series_array_print_missing == ESeriesMissing.Error)
                            {
                                FindArraySeriesHelper2(keys);  //error
                            }
                            else if (Program.options.series_array_print_missing == ESeriesMissing.M)
                            {
                                rv = new Series(ESeriesType.Timeless, this.freq, null);
                                ((Series)rv).SetTimelessData(double.NaN);
                                ((Series)rv).isNotFoundArraySubSeries = ESeriesMissing.M;
                            }
                            else if (Program.options.series_array_print_missing == ESeriesMissing.Zero)
                            {
                                rv = new Series(ESeriesType.Timeless, this.freq, null);
                                ((Series)rv).SetTimelessData(0d);
                                ((Series)rv).isNotFoundArraySubSeries = ESeriesMissing.Zero;
                            }
                            else if (Program.options.series_array_print_missing == ESeriesMissing.Skip)
                            {
                                rv = new Series(ESeriesType.Timeless, this.freq, null);
                                ((Series)rv).SetTimelessData(0d);  //must be 0 for .isNotFound to work
                                ((Series)rv).isNotFoundArraySubSeries = ESeriesMissing.Skip;
                            }
                            else throw new GekkoException();
                        }
                        else //GekkoSmplCommand.Sum but also others, like GetIVariableFromString()
                        {
                            //sum and others, non-print

                            //we start checking out settings (may be null). These origin from FindIVariableFromString(), will be null in normal expressions etc.

                            if (settings?.create == O.ECreatePossibilities.NoneReturnNull)
                            {
                                rv = null;  //just return null
                            }
                            else if (settings?.create == O.ECreatePossibilities.Can || settings?.create == O.ECreatePossibilities.Must)
                            {
                                Series ts = new Series(ESeriesType.Normal, this.freq, Globals.seriesArraySubName + Globals.freqIndicator + G.GetFreq(this.freq));
                                this.dimensionsStorage.AddIVariableWithOverwrite(new MapMultidimItem(keys), ts);
                                rv = ts;
                            }
                            else if (Program.options.series_array_calc_missing == ESeriesMissing.Error)
                            {
                                FindArraySeriesHelper2(keys);  //error
                            }
                            else if (Program.options.series_array_calc_missing == ESeriesMissing.M)
                            {
                                rv = new Series(ESeriesType.Timeless, this.freq, null);
                                ((Series)rv).SetTimelessData(double.NaN);
                            }
                            else if (Program.options.series_array_calc_missing == ESeriesMissing.Zero)
                            {
                                rv = new Series(ESeriesType.Timeless, this.freq, null);
                                ((Series)rv).SetTimelessData(0d);
                            }
                            else if (Program.options.series_array_calc_missing == ESeriesMissing.Skip)
                            {
                                G.Writeln2("*** ERROR: Please use 'OPTION series array calc missing = zero' instead of 'skip'");
                                throw new GekkoException();
                            }
                            else throw new GekkoException();
                        }

                    }
                    else
                    {
                        //lhs variable like the 'a' element in x[a], where x[a] is on LHS

                        if (Program.options.databank_create_auto || this.name.StartsWith("xx", StringComparison.OrdinalIgnoreCase))
                        {
                            //good
                        }
                        else
                        {
                            //#07549843254
                            G.Writeln2("*** ERROR: Cannot auto-create array-series element " + this.GetNameWithoutCurrentFreq(true) + "[" + G.GetListWithCommas(keys) + "].");
                            G.Writeln("           You may change the settings with the following option:", Color.Red);
                            G.Writeln("           OPTION databank create auto = yes;", Color.Red);
                            throw new GekkoException();
                        }

                        if (rhsIsTimeless)
                        {
                            rv = new Series(ESeriesType.Timeless, this.freq, Globals.seriesArraySubName + Globals.freqIndicator + G.GetFreq(this.freq), double.NaN);                            
                        }
                        else
                        {
                            rv = new Series(ESeriesType.Normal, this.freq, Globals.seriesArraySubName + Globals.freqIndicator + G.GetFreq(this.freq));
                        }                        
                        MapMultidimItem mmi = new MapMultidimItem(keys, this);
                        this.dimensionsStorage.AddIVariableWithOverwrite(mmi, rv);
                    }
                }
                else
                {
                    if (settings?.create == O.ECreatePossibilities.Must)
                    {
                        //creates a brand new                        
                        Series ts = new Series(ESeriesType.Normal, this.freq, Globals.seriesArraySubName + Globals.freqIndicator + G.GetFreq(this.freq));
                        this.dimensionsStorage.AddIVariableWithOverwrite(new MapMultidimItem(keys), ts);
                        rv = ts;
                    }
                    else
                    {
                        rv = iv as Series;
                    }
                    if (rv == null)
                    {
                        //should not be possible
                        G.Writeln2("*** ERROR: Array-timeseries element is non-series.");
                        throw new GekkoException();
                    }
                }
            }

            if (Globals.precedents != null && rv != null)
            {
                Series rv_series = rv as Series;
                if (rv_series.type != ESeriesType.ArraySuper)
                {                    
                    //TODO: name may probably be null, for instance in x[#i] where the #i is not present, and a timeless series with NaN or 0 is returned
                    Program.AddToPrecedents(this.GetParentDatabank(), rv_series.GetName());                    
                }
            }

            return rv;
        }

        private void FindArraySeriesHelper2(string[] keys)
        {
            List<string> warnings = new List<string>();

            string txt = null;
            foreach (string ss in keys)
            {
                if (ss.Length != ss.Trim().Length)
                {
                    warnings.Add("Please note that the element '" + ss + "' contains blanks at start or end of string");
                }
                txt += "'" + ss + "', ";
            }
            G.Writeln2("*** ERROR: The arrayseries " + G.GetNameAndFreqPretty(this.name) + " did not contain this element:");
            G.Writeln("           [" + txt.Substring(0, txt.Length - 2) + "]", Color.Red);
            foreach (string warning in warnings)
            {
                G.Writeln("+++ NOTE: " + warning);
            }
            G.Writeln("+++ NOTE: You may ignore such errors with 'OPTION series array print missing = ... ;' and 'OPTION series array calc missing = ... ;'");
            throw new GekkoException();
        }

        public static bool IsLagOrLead(int i)
        {
            return i > -100 && i < 100;
        }



        //public IVariable Indexer(GekkoSmpl smpl, IVariablesFilterRange index)
        //{
        //    G.Writeln2("Ts error 8");
        //    return null;
        //}

        //public IVariable Indexer(GekkoSmpl smpl, IVariablesFilterRange index1, IVariablesFilterRange index2)
        //{
        //    G.Writeln2("Ts error 9");
        //    return null;
        //}

        //public IVariable Indexer(GekkoSmpl smpl, IVariable index, IVariablesFilterRange indexRange)
        //{
        //    G.Writeln2("Ts error 10");
        //    return null;
        //}

        //public IVariable Indexer(GekkoSmpl smpl, IVariablesFilterRange indexRange, IVariable index)
        //{
        //    G.Writeln2("Ts error 11");
        //    return null;
        //}

        public void InjectAdd(GekkoSmpl smpl, IVariable y)
        {            
            if (this.Type() == EVariableType.Series && y.Type() == EVariableType.Series)
            {
                Series y_series = y as Series;
                if (Globals.bugfix_speedup && this.type != ESeriesType.Timeless && y_series.type != ESeriesType.Timeless)
                {
                    GekkoTime window1 = smpl.t0;
                    GekkoTime window2 = smpl.t3;

                    int ib1 = this.ResizeDataArray(window1);
                    int ib2 = this.ResizeDataArray(window2);

                    int ic1 = y_series.ResizeDataArray(window1);
                    int ic2 = y_series.ResizeDataArray(window2);
                                        
                    double[] arrayb = this.data.GetDataArray_ONLY_INTERNAL_USE();
                    double[] arrayc = y_series.data.GetDataArray_ONLY_INTERNAL_USE();

                    bool b = MissingZero(y_series);
                    for (int i = 0; i < GekkoTime.Observations(window1, window2); i++)
                    {
                        double d = arrayc[i + ic1];
                        if (b && G.isNumericalError(d)) d = 0d;
                        arrayb[i + ib1] += d;  //what if lhs is NaN?
                    }
                }
                else
                {
                    foreach (GekkoTime t in smpl.Iterate03())
                    {
                        this.SetData(t, ((Series)this).GetData(smpl, t) + ((Series)y).GetData(smpl, t));
                    }
                }
            }            
            else if (this.Type() == EVariableType.Series && y.Type() == EVariableType.Val)
            {
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    this.SetData(t, ((Series)this).GetData(smpl, t) + ((ScalarVal)y).val);
                }
            }
            else
            {
                G.Writeln("*** ERROR: Variables are of wrong type for summation");
                throw new GekkoException();
            }
            return;
        }

        public double GetValOLD(GekkoSmpl smpl)
        {
            G.Writeln2("Ts error 13");
            return double.NaN;
        }

        public double GetVal(GekkoTime t)
        {
            return this.GetDataSimple(t);
        }

        public double ConvertToVal()
        {
            G.Writeln2("*** ERROR: Cannot extract a val from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            G.Writeln2("*** ERROR: Cannot convert series to string (series name: '" + this.GetName() + "')");
            throw new GekkoException();
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            G.Writeln2("*** ERROR: Cannot convert series to date (series name: '" + this.GetName() + "')");
            throw new GekkoException();
        }

        public List<IVariable> ConvertToList()
        {
            G.Writeln2("*** ERROR: Cannot convert series to list (series name: '" + this.GetName() + "')");
            throw new GekkoException();
        }

        public EVariableType Type()
        {
            return EVariableType.Series;
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, O.Assignment options, params IVariable[] indexes)
        {
            if (this.type == ESeriesType.ArraySuper)
            {
                //Will fail with an error if not all indexes are of STRING type     
                bool rhsIsTimeless = false;
                if (rhsExpression.Type() == EVariableType.Series && (rhsExpression as Series).type == ESeriesType.Timeless) rhsIsTimeless = true;
                IVariable iv = this.FindArraySeries(smpl, indexes, true, rhsIsTimeless, null);  //if not found, it will be created (since we are on the lhs) and inherit the timeless status from this timeseries.
                Series ts = iv as Series;
                if (ts == null)
                {
                    //Probably not possible, sum() and unfold are on the RHS
                    G.Writeln2("*** ERROR: indexer on LHS on a null object");
                    throw new GekkoException();
                }
                O.LookupHelperLeftside(smpl, ts, rhsExpression, EVariableType.Var, options);                
            }
            else
            {
                if (indexes.Length == 1 && indexes[0].Type() == EVariableType.Val)
                {
                    int i = O.ConvertToInt(indexes[0]);
                    if (IsLagOrLead(i))
                    {
                        G.Writeln2("*** ERROR: You cannot use lags or lead on left-hand side of an expression");
                        throw new GekkoException();
                    }
                    else
                    {
                        if (this.freq == EFreq.A || this.freq == EFreq.U)
                        {
                            double d = rhsExpression.ConvertToVal();  //will fail with an error unless VAL or 1x1 matrix
                            GekkoTime t = new GekkoTime(this.freq, i, 1);
                            this.SetData(t, d);
                            if (Program.options.series_failsafe)
                            {
                                //only for debugging                        
                                O.ReportSeriesMissingValue(this, t, t);
                            }
                        }
                        else
                        {
                            G.Writeln2("*** ERROR: You cannot []-index a " + G.GetFreqString(this.freq) + " SERIES with [" + i + "]");
                            throw new GekkoException();
                        }
                    }
                }
                else if (indexes.Length == 1 && indexes[0].Type() == EVariableType.Date)
                {
                    double d = rhsExpression.ConvertToVal();  //will fail with an error unless VAL or 1x1 matrix                
                    GekkoTime t = ((ScalarDate)(indexes[0])).date;
                    this.SetData(t, d);  //will fail with an error if freqs do not match
                    if (Program.options.series_failsafe)
                    {
                        //only for debugging                        
                        O.ReportSeriesMissingValue(this, t, t);
                    }
                }
                else
                {                    
                    G.Writeln2("*** ERROR: A normal series " + this.GetNameAndFreqPretty(true) + " on the left-hand side must be []-indexed with date or val");
                    throw new GekkoException();
                }
            }
        }

        public void SetDirty(bool b1)
        {
            if (this.type == ESeriesType.Light)
            {
                G.Writeln2("*** ERROR: Light series cannot be set dirty");
                throw new GekkoException();
            }
            else
            {
                if (this.meta != null) this.meta.SetDirty(b1);
            }
        }

        //public void SetGhost(bool b2)
        //{
        //    if (meta != null) this.meta.SetGhost(b2);
        //}

        public int GetAnchorPeriodPositionInArray()
        {
            //this.data is not null when this is called
            //BEWARE: Be careful when using .dataOffsetLag! #772439872435
            return this.data.anchorPeriodPositionInArray + this.dataOffsetLag;  //.dataOffset changed from 0 to -1 is same as x[-1]
        }

        public bool IsDirty()
        {
            if (meta == null) return false; //not used for light Series
            return this.meta.IsDirty();
        }

        //public bool IsLight()
        //{
        //    return this.meta == null;
        //}

        //public bool IsArrayTimeseries()
        //{
        //    return this.dimensions > 0;
        //}

        /// <summary>
        /// Creates a clone of the Series, copying all fields. Used for copying databanks in RAM.
        /// </summary>
        /// <returns>The cloned Series object.</returns>
        public IVariable DeepClone(GekkoSmplSimple truncate)
        {
            //Always make sure new fields are remembered in the DeepClone() method

            //.isNotFoundArraySub... field is not cloned
            //the .isDirty and .parentDatabank fields are not cloned

            Series tsCopy = new Series(this.freq, this.name);  //this will create t½he .meta object - the .data object is always there

            tsCopy.type = this.type;

            //BEWARE: Be careful when using .dataOffsetLag! #772439872435
            tsCopy.dataOffsetLag = this.dataOffsetLag;  //probably 0 in all cases

            //.data field is always there
            if (this.data.GetDataArray_ONLY_INTERNAL_USE() == null)
            {
                tsCopy.data.SetDataarray_ONLY_INTERNAL_USE(null);
            }
            else
            {
                tsCopy.data.SetDataarray_ONLY_INTERNAL_USE(new double[this.data.GetDataArray_ONLY_INTERNAL_USE().Length]);
                System.Array.Copy(this.data.GetDataArray_ONLY_INTERNAL_USE(), tsCopy.data.GetDataArray_ONLY_INTERNAL_USE(), this.data.GetDataArray_ONLY_INTERNAL_USE().Length);
            }
            tsCopy.data.anchorPeriod = this.data.anchorPeriod;
            tsCopy.data.anchorPeriodPositionInArray = this.data.anchorPeriodPositionInArray;  //!!! DO NOT USE ANY .dataLag here, it is dealt with somewhere else

            if (this.type == ESeriesType.ArraySuper)
            {
                //Clone the array-subseries
                tsCopy.dimensions = this.dimensions;
                tsCopy.dimensionsStorage = new MapMultidim();
                foreach (KeyValuePair<MapMultidimItem, IVariable> kvp in this.dimensionsStorage.storage)
                {
                    MapMultidimItem item = kvp.Key.Clone();
                    item.parent = tsCopy;  //must be re-pointed
                    Series subseries = kvp.Value.DeepClone(truncate) as Series;
                    subseries.mmi = item; //the sub-ser
                    tsCopy.dimensionsStorage.storage.Add(item, subseries);
                }                
            }

            if (this.type != ESeriesType.Light)
            {
                if (this.meta != null)
                {
                    tsCopy.meta.firstPeriodPositionInArray = this.meta.firstPeriodPositionInArray;
                    tsCopy.meta.lastPeriodPositionInArray = this.meta.lastPeriodPositionInArray;
                    if (this.meta.label != null) tsCopy.meta.label = this.meta.label;
                    if (this.meta.source != null) tsCopy.meta.source = this.meta.source;
                    if (this.meta.units != null) tsCopy.meta.source = this.meta.units;
                    if (this.meta.stamp != null) tsCopy.meta.stamp = this.meta.stamp;
                    if (this.meta.domains != null)
                    {
                        tsCopy.meta.domains = new string[this.meta.domains.Length];
                        this.meta.domains.CopyTo(tsCopy.meta.domains, 0);
                    }
                    tsCopy.meta.fix = this.meta.fix; //will also get copied for array-series
                    if (this.meta.fixedNormal != null)
                    {
                        //will also get copied for array-series
                        tsCopy.meta.fixedNormal = new GekkoTimeSpans();
                        foreach (GekkoTimeSpan gts in this.meta.fixedNormal.data)
                        {
                            tsCopy.meta.fixedNormal.data.Add(new GekkoTimeSpan(gts.tStart, gts.tEnd));
                        }
                    }
                }
                if (this.mmi != null) tsCopy.mmi = this.mmi;  //only for array sub-series  
            }

            if (truncate != null)
            {
                tsCopy.Truncate(truncate.t1, truncate.t2);  //somewhat slack, since truncation is AFTER a deep clone...
            }

            return tsCopy;
        }

        public void DeepTrim()
        {             
            if (this.type == ESeriesType.ArraySuper)
            {                
                foreach (KeyValuePair<MapMultidimItem, IVariable> kvp in this.dimensionsStorage.storage)
                {
                    kvp.Value.DeepTrim();
                }
            }
            else
            {
                this.Trim();
            }            
        }

        public void DeepCleanup()
        {
            if (this.type == ESeriesType.ArraySuper)
            {
                //#parentpointer
                foreach (KeyValuePair<MapMultidimItem, IVariable> kvp in this.dimensionsStorage.storage)
                {
                    Series subSeries = kvp.Value as Series;
                    if (subSeries != null)
                    {
                        //best to keep these pointers out of protobuf                        
                        ConnectArraysSeriesWithSubSeries(this, subSeries, kvp.Key);
                    }
                }
            }
        }

        private static void ConnectArraysSeriesWithSubSeries(Series arraySeries, Series subSeries, MapMultidimItem mmi)
        {
            mmi.parent = arraySeries;  //The mmi item points to the array-series
            subSeries.mmi = mmi; //the sub-series points to the mmi, this way we can get from the sub-series all the way up to the array-series.                        
        }
    }    

        [ProtoContract]
    public class SeriesDataInformation
    {
        [ProtoMember(1, IsPacked = true)]  //a bit faster, and a bit smaller file (also when zipped) 
        //BEWARE: Be careful about .dataOffsetLag when using the array! #772439872435
        private double[] dataArray = null;  //BEWARE: if altering directly, make sure that .protect in the databank is not set!!

        [ProtoMember(2)]
        public GekkoTime anchorPeriod = GekkoTime.tNull;

        [ProtoMember(3)]
        //Do not access directly, use GetAnchorPeriodPositionInArray(), so the .lagOffset is included
        public int anchorPeriodPositionInArray = -123454321;

        public double[] GetDataArray_ONLY_INTERNAL_USE()
        {
            return this.dataArray;
        }

        public void SetDataarray_ONLY_INTERNAL_USE(double[] x)
        {
            this.dataArray = x;
        }

        public SeriesDataInformation DeepClone()
        {
            SeriesDataInformation rv = new SeriesDataInformation();
            rv.dataArray = this.dataArray.Clone() as double[];
            rv.anchorPeriodPositionInArray = this.anchorPeriodPositionInArray;
            rv.anchorPeriod = this.anchorPeriod;  //immutable, so safe to point to
            return rv;
        }

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

    

}
