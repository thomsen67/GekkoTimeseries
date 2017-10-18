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
    public enum ETimeSeriesType
    {
        TimeSeriesLight
    }

    /// <summary>
    /// The TimeSeries class is a class designed for storing and retrieving timeseries (vectors/arrays of consecutive time data) 
    /// in a fast and reliable way. Timeseries can be of different frequencies (for instance annual, quarterly or monthly). 
    /// The internal representation of the data is an auto-resizing double[] array for speed and compactness.
    /// </summary>
    /// <remarks>
    /// The TimeSeries class is typically used in conjunction with a Databank, which can be thought of as a container that
    /// stores the individual timeseries by their string names. Several databanks may be open at the same time (in RAM).
    /// </remarks>
    /// <example>
    /// A stand-alone TimeSeries may be created and filled with data like this:
    /// <code>
    /// TimeSeries ts = new TimeSeries(EFreq.Quarterly, "gdp");
    /// GekkoTime t1 = new GekkoTime(EFreq.Quarterly, 2000, 1);
    /// GekkoTime t2 = new GekkoTime(EFreq.Quarterly, 2002, 4);
    /// ts.SetData(t1.Add(-1), 323490d);
    /// foreach (GekkoTime t in new GekkoTimeIterator(t1, t2))
    /// {
    ///     double lagValue = ts.GetData(t.Add(-1));
    ///     ts.SetData(t, 1.01d * lagValue);
    /// }    
    /// </code>
    /// The code first creates a new TimeSeries 'ts' with quarterly frequency and the name 'gdp' (this name is used if the
    /// timeseries is later put into a databank). Next, start and end periods are defined (2000q1 and 2002q4), and the period
    /// before the start period has its value set to 323490 (that is, for the quarter 1999q4). Next, all the
    /// quarters are looped via the iterator in the next line (that is, t = 2000q1, 2000q2, ... , 2002q4 = 12 observations
    /// in all). Inside the loop, lagValue is given the value of the lagged observations (for t = 2000q1 that will be the
    /// value of the timeseries in 1999q4). This value is augmented with 1% and set for the current observation. This way,
    /// the 'gdp' timeseries will obtain a quarterly growth rate of 1% for the period 2000q1-2002q4.
    /// </example>
    /// <example>
    /// A TimeSeries object can be put into a Databank object and retrieved again by name (builds upon the previous example):    
    /// <code>    
    /// Databanks databanks = new Databanks();
    /// databanks.AddDatabank(new Databank("Work");    
    /// databanks.GetDatabank("Work").AddVariable(ts);
    /// TimeSeries ts1 = databanks.GetDatabank("Work").GetVariable("gdp");
    /// </code>
    /// The 'Work' databank will contain the 'gdp' time series (the name indicated when the timeseries was created), 
    /// and 'ts' and 'ts1' will point to the same TimeSeries object. 
    /// You may consult the Databanks and Databank classes for more info on storing timeseries.
    /// </example>    
    /// <seealso cref="Databank"/>
    /// <seealso cref="Databanks"/>
    [ProtoContract]
    public class TimeSeries : IVariable
    {
        [ProtoMember(1)]
        public TimeSeriesMetaInformation meta = null;
        ///// <summary>
        ///// Indicates the frequency of the TimeSeries.
        ///// </summary>
        [ProtoMember(2)]
        public EFreq freq;
        /// <summary>
        /// The name of the variable. In a databank, this name corresponds to the key that the TimeSeries is stored under,
        /// including frequency (for instance x!q for x with quarterly freq).        
        /// </summary>
        [ProtoMember(3)]
        public string name;
        /// <summary>
        /// The array containing the time series data. This array is initialized with NaN values, and the array may resize
        /// itself if necessary to store a particular observation.
        /// </summary>
        [ProtoMember(4, IsPacked = true)]  //a bit faster, and a bit smaller file (also when zipped)        
        public double[] dataArray;  //BEWARE: if altering directly, make sure that .protect in the databank is not set!!
        [ProtoMember(5)]
        /// <summary>
        /// The 'super' period (year) corresponding to the anchor date.
        /// </summary>
        /// 
        public GekkoTime anchorPeriod = Globals.tNull;
        /// <summary>
        /// The index corresponding to the anchor date.
        /// </summary>
        [ProtoMember(6)]
        public int anchorPeriodPositionInArray;
        [ProtoMember(7)]
        private bool isTimeless = false; //a timeless variable is like a ScalarVal (VAL). A timeless variable puts the value in dataArray[0]        
        [ProtoMember(8)]
        public MapMultidim storage = null;  //only active if it is an array-timeseries
        [ProtoMember(9)]
        public int storageDim = 0;  //default is 0 which is same as normal timeseries, also used in IsArrayTimeseries()

        private TimeSeries()
        {
            //This is ONLY because protobuf-net needs it! 
            //Empty timeseries should not be created that way.            
        }

        public TimeSeries(ETimeSeriesType type, GekkoSmpl smpl)
        {
            // ------------------------------
            //Constructing a TimeSeriesLight
            //type is just a decorator (not used), so that it is easier to 
            //see when a light timeseries is created.
            // ------------------------------
            this.freq = smpl.t0.freq;  //same as for t1, t2 or t3
            this.name = null; //light
            this.meta = null; //light
            int n = smpl.Observations03();
            this.dataArray = new double[n];  //we make the array as compact as possible --> faster
            InitializeDataArray(this.dataArray);
            this.anchorPeriod = smpl.t0;            
            this.anchorPeriodPositionInArray = 0;
        }

        /// <summary>
        /// Constructor that creates a new TimeSeries object with a particular frequency and variable name.
        /// </summary>
        /// <param name="frequency">The frequency of the timeseries</param>
        /// <param name="variableName">The variable name of the timeseries</param>
        public TimeSeries(EFreq frequency, string variableName)
        {            
            this.freq = frequency;
            this.name = variableName;  //Note: the variableName does contain a '!'. If the name is null, it is a light TimeSeries.
            if (this.name != null) this.meta = new TimeSeriesMetaInformation();  //do not create this object if this.name = null, that is, a light TimeSeries.
        }

        public void SetZero(GekkoSmpl smpl)
        {
            foreach (GekkoTime t in smpl.Iterate03())
            {
                this.SetData(t, 0d);
            }
        }

        public void Truncate(ReadDatesHelper dates)
        {
            //Also see #345632473
            if (dates == null) return;
            if (this.freq == EFreq.Annual)
            {
                this.Truncate(dates.t1Annual, dates.t2Annual);
            }
            else if (this.freq == EFreq.Quarterly)
            {
                this.Truncate(dates.t1Quarterly, dates.t2Quarterly);
            }
            else if (this.freq == EFreq.Monthly)
            {
                this.Truncate(dates.t1Monthly, dates.t2Monthly);
            }
            else
            {
                G.Writeln2("***: Freq error");
                throw new GekkoException();
            }
        }

        /// <summary>
        /// Truncates the TimeSeries object, so that the starting period
        /// and ending period are as given. Beware: this will usually mean that data is deleted. Used to truncate
        /// a databank to a particular time period. Note: You may wish to use Trim() after a Truncate(). Note: only works for annual timeseries.
        /// </summary>
        /// <param name="start">The start period.</param>
        /// <param name="end">The end period.</param>
        /// <exception cref="GekkoException">
        /// </exception>
        public void Truncate(GekkoTime start, GekkoTime end)
        {
            if (this.IsTimeless()) return;
            if (this.meta.parentDatabank != null && this.meta.parentDatabank.protect) Program.ProtectError("You cannot truncate a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
            int indexStart = this.GetArrayIndex(start);
            int indexEnd = this.GetArrayIndex(end);

            int newFirst = Math.Max(this.meta.firstPeriodPositionInArray, indexStart);
            int newLast = Math.Min(this.meta.lastPeriodPositionInArray, indexEnd);

            if (newFirst > newLast)
            {
                //the truncate window is completely before or after the data window
                //wipe all the data, and set the sample to 1 length
                for (int i = 0; i < this.dataArray.Length; i++)
                {
                    this.dataArray[i] = double.NaN;
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
                    this.dataArray[i] = double.NaN;
                }
                for (int i = this.meta.lastPeriodPositionInArray + 1; i < this.dataArray.Length; i++)
                {
                    this.dataArray[i] = double.NaN;
                }
            }                        
            this.SetDirty(true);
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
            //DimensionCheck();
            if (this.dataArray == null) return;
            if (this.IsNullPeriod()) return;  //could actually trim this, but oh well
            if (!(this.meta.firstPeriodPositionInArray == 0 && this.meta.lastPeriodPositionInArray == this.dataArray.Length - 1))  //already trimmed                
            {
                int size = this.meta.lastPeriodPositionInArray - this.meta.firstPeriodPositionInArray + 1;
                double[] temp = new double[size];
                Array.Copy(this.dataArray, this.meta.firstPeriodPositionInArray, temp, 0, size);
                int first = this.meta.firstPeriodPositionInArray;
                //Correct these pointers accordingly
                this.anchorPeriodPositionInArray += -first;
                this.meta.firstPeriodPositionInArray += -first; // --> 0
                this.meta.lastPeriodPositionInArray += -first;  // --> size-1
                this.dataArray = temp;  //point to this array
            }
        }

        /// <summary>
        /// Gets the timeseries value corresponding to the given period.
        /// </summary>
        /// <param name="t">The period.</param>
        /// <returns>The value (double.NaN if missing)</returns>
        /// <exception cref="GekkoException">Exception if frequency of timeseries and period do not match.</exception>
        public double GetData(GekkoSmpl smpl, GekkoTime t)
        {            
            if (this.freq != t.freq)
            {                
                FreqError(t);             
            }
            if (this.dataArray == null)
            {
                //If no data has been added to the timeseries, NaN will always be returned.
                if (this.IsArrayTimeseries())
                {
                    G.Writeln2("*** ERROR: The variable '" + this.name + "' is an array-timeseries,");
                    G.Writeln("           but is used as a normal timeseries here (without []-indexer)", Color.Red);
                    Program.ArrayTimeseriesTip(this.name);
                    throw new GekkoException();
                }
                else
                {
                    return double.NaN;
                }
            }
            if (this.isTimeless)
            {
                return this.dataArray[0];
            }
            else
            {
                int index = GetArrayIndex(t);
                if (index < 0 || index >= this.dataArray.Length)
                {
                    if (this.Type(ETimeSeriesType.TimeSeriesLight))
                    {
                        int ii = index;
                        if (index >= 0)
                        {
                            ii = index - (this.dataArray.Length - 1);
                        }
                        if (smpl != null) smpl.gekkoError = new GekkoError(ii);  //GetData() can be called with smpl=null, where out-of-window is not signalled back. Typically because we take the timeseries directly from a databank, so they are not light.
                    }
                    return double.NaN;  //out of bounds, we return a missing value (NaN)
                }
                else
                {
                    return this.dataArray[index];
                }
            }
        }

        private void FreqError(GekkoTime t)
        {
            G.Writeln2("*** ERROR: Frequency mismatch: " + G.GetFreqString(this.freq) + " versus " + G.GetFreqString(t.freq));
            throw new GekkoException();
        }


        public void SetTimelessData(double value)
        {
            if (!this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #100");
                throw new GekkoException();
            }
            if (this.meta != null && this.meta.parentDatabank != null && this.meta.parentDatabank.protect) Program.ProtectError("You cannot change an observation in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
                        
            if (this.dataArray == null)
            {
                this.dataArray = new double[1];
            }
            this.dataArray[0] = value;
        }

        public double GetTimelessData()
        {
            if (!this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #1009");
                throw new GekkoException();
            }
            if (this.dataArray == null)
            {
                return double.NaN;
            }
            return this.dataArray[0];
        }

        /// <summary>
        /// This sets the observation (period) to the given value.
        /// </summary>
        /// <param name="t">The period.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="GekkoException">Exception if frequency of timeseries and period do not match.</exception>
        public void SetData(GekkoTime t, double value)
        {
            if (this.isTimeless)
            {
                //Should not normally be used.
                //But this may be called from for instance DECOMP, calling it with a time period
                //Normally timeless variables should be called via the SetData(double value) method
                this.dataArray[0] = value;
            }
            if (this.meta != null && this.meta.parentDatabank != null && this.meta.parentDatabank.protect) Program.ProtectError("You cannot change an observation in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
            
            if (this.freq != t.freq)
            {
                //See comment to GetData()
                FreqError(t);                
            }
            if (this.dataArray == null)
            {
                InitDataArray(t);
            }

            if (this.isTimeless)
            {
                this.dataArray[0] = value;             
            }
            else
            {
                //Get the array index corresponding to the period. If this index is out of array bounds, the array will
                //be resized (1.5 times larger).
                int index = ResizeDataArray(t, true);
                this.dataArray[index] = value;
                //Start and end date for observations are adjusted.
                //for the first obs put into a new timeseries, both the if's should trigger.
                if (this.meta != null)
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
                this.SetDirty(true);
            }
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
        public double[] GetDataSequence(out int index1, out int index2, GekkoTime gt1, GekkoTime gt2, bool setStartEndPeriods)
        {
            //NB NB NB NB NB
            //NB NB NB NB NB
            //NB NB NB NB NB   BEWARE: the array returned is a pointer to the REAL datacontainer for the timeseries. So do not alter the array unless you are ACTUALLY altering the timeseries (for instance UPD, GENR etc.)
            //NB NB NB NB NB
            //NB NB NB NB NB
            //      It might be called half overlapped like this:
            //            +++++++++++++++++++++++
            //      ---------------
            //      if so, the array is resized before it is returned.
            //      if the array is null, it will be created when calling this method.
            //
            //Also beware that if the array returned is touched afterwards, the timeseries will be dirty. This only happens in the 
            //simulation code, though. See #98726527!

            //DimensionCheck();

            if (this.isTimeless)
            {
                int n = GekkoTime.Observations(gt1, gt2);
                double[] numbers = new double[n];
                for (int i = 0; i < n; i++) numbers[i] = this.dataArray[0];
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

            if (this.dataArray == null)
            {
                InitDataArray(gt1);
            };

            index1 = GetArrayIndex(gt1);
            index2 = GetArrayIndex(gt2);

            if (index1 < 0 || index1 >= this.dataArray.Length || index2 < 0 || index2 >= this.dataArray.Length)
            {
                index1 = ResizeDataArray(gt1);
                index2 = ResizeDataArray(gt2);  //this would never change index1
            }

            if (setStartEndPeriods)  //only relevant if the returned arrays is actually tampered with, which is normally NOT the case (only for a[,] and b[] array stuff in simulation)
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
            return this.dataArray;
        }

        /// <summary>
        /// Overload with setStartEndPeriods = false.
        /// </summary>
        /// <param name="index1">The array index corresponding to the start of the period</param>
        /// <param name="index2">The array index corresponding to the end of the period</param>
        /// <param name="per1">The start of the period.</param>
        /// <param name="per2">The end of the period.</param>
        /// <returns></returns>
        public double[] GetDataSequence(out int index1, out int index2, GekkoTime per1, GekkoTime per2)
        {
            if (this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #2");
                throw new GekkoException();
            }
            return GetDataSequence(out index1, out index2, per1, per2, false);
        }

        /// <summary>
        /// For the given timeseries, this sets the data of the given period to array to the values given in the input array. An offset may be used.
        /// </summary>
        /// <param name="gt1">Start of the time period.</param>
        /// <param name="gt2">End of the time period</param>
        /// <param name="input">The input.</param>
        /// <param name="inputOffset">The input offset.</param>
        /// <exception cref="GekkoException">Exception if frequency of timeseries and periods differ.</exception>
        public void SetDataSequence(GekkoTime gt1, GekkoTime gt2, double[] input, int inputOffset)
        {
            if (this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #3");
                throw new GekkoException();
            }
            if (this.meta.parentDatabank != null && this.meta.parentDatabank.protect) Program.ProtectError("You cannot change observations in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
            
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

            if (this.dataArray == null)
            {
                //slack: init could be more precise regarding size, for non-annual frequencies.
                GekkoTime gtTemp = new GekkoTime(gt1.freq, (gt1.super + gt2.super) / 2, 1);
                InitDataArray(gtTemp);  //assumes subperiod 1
            }
            int index1 = -12345;
            int index2 = -12345;
            index1 = GetArrayIndex(gt1);
            index2 = GetArrayIndex(gt2);
            if (index1 < 0 || index1 >= this.dataArray.Length || index2 < 0 || index2 >= this.dataArray.Length)
            {
                index1 = ResizeDataArray(gt1);
                index2 = ResizeDataArray(gt2); //this would never change index1, since slots are added at the end                            
            }
            System.Array.Copy(input, inputOffset, this.dataArray, index1, index2 - index1 + 1);
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

        public bool IsTimeless()
        {
            return this.isTimeless;
        }

        public void SetTimeless()
        {
            this.isTimeless = true;
        }

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
            if (this.isTimeless)
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
            if (this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #5");
                throw new GekkoException();
            }
            return GetPeriod(this.meta.lastPeriodPositionInArray);
        }

        public GekkoTime GetRealDataPeriodFirst()
        {
            //Takes some time for large non-trimmed arrays, but is more precise than GetPeriodFirst()
            GekkoTime rv = Globals.tNull;
            if (this.isTimeless)
            {
                //do nothing
            }
            else
            {
                for (int i = 0; i < this.dataArray.Length; i++)
                {
                    if (!G.isNumericalError(this.dataArray[i]))
                    {
                        rv = GetPeriod(i);
                        break;
                    }
                }
            }
            return rv;            
        }

        public GekkoTime GetRealDataPeriodLast()
        {
            //Takes some time for large non-trimmed arrays, but is more precise than GetPeriodLast()
            GekkoTime rv = Globals.tNull;
            if (this.isTimeless)
            {
                //do nothing
            }
            else
            {
                for (int i = this.dataArray.Length - 1; i >= 0; i--)
                {
                    if (!G.isNumericalError(this.dataArray[i]))
                    {
                        rv = GetPeriod(i);
                        break;
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
            if (this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #7");
                throw new GekkoException();
            }
            //The inverse method is GetArrayIndex()
            //Should maybe be private method? But then how to unit-test?
            //see also AddToPeriod()
            //DimensionCheck();
            int subPeriods = 1;
            if (this.freq == EFreq.Quarterly) subPeriods = 4;
            else if (this.freq == EFreq.Monthly) subPeriods = 12;
            else if (this.freq == EFreq.Undated) subPeriods = 1;

            //Calculates the period by means of using the anchor. Uses integer division, so there is an
            //implicit modulo calculation here.
            int sub1 = this.anchorPeriod.sub + (indexInDataArray - anchorPeriodPositionInArray);
            int addPer = (sub1 - 1) / subPeriods;
            int addSub = (indexInDataArray - anchorPeriodPositionInArray) - subPeriods * addPer;

            int resultSuperPer = this.anchorPeriod.super + addPer;
            int resultSubPer = this.anchorPeriod.sub + addSub;

            //This code below fixes a bug (1.4 suffers from it: only affects non-annual timeseries), bug fixed in 1.5.8
            if (resultSubPer < 1)  //this may happen, probaby because of "/" on integer not behaving as expected
            {
                resultSuperPer -= 1;
                resultSubPer += subPeriods;
            }
            GekkoTime t = new GekkoTime(this.freq, resultSuperPer, resultSubPer);
            return t;
        }
         
        // -----------------------------------------------------------------------------
        // ----------------- private methods -------------------------------------------
        // -----------------------------------------------------------------------------

        private void InitializeDataArray(double[] dataArray)
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
        private int GetArrayIndex(GekkoTime gt)
        {
            int rv = FromGekkoTimeToArrayIndex(gt, new GekkoTime(this.freq, this.anchorPeriod.super, this.anchorPeriod.sub), this.anchorPeriodPositionInArray);
            return rv;
        }

        public static GekkoTime FromArrayIndexToGekkoTime(int i, GekkoTime anchorPeriod, int anchorPeriodPositionInArray)
        {
            int dif = i - anchorPeriodPositionInArray;
            GekkoTime rv = anchorPeriod.Add(dif);
            return rv;
        }

        public static int FromGekkoTimeToArrayIndex(GekkoTime gt, GekkoTime anchorPeriod, int anchorPeriodPositionInArray)
        {
            //this.anchorPeriod.sub is always 1 at the moment, and will always be 1 for Annual.
            //but we cannot count on anchorSubPeriod being 1 forever (for instance for daily obs)   
            int rv = -12345;
            if (anchorPeriod.freq == EFreq.Annual)
            {
                //Special treatment in order to make it fast.
                //undated freq could return fast in the same way as this??
                rv = anchorPeriodPositionInArray + gt.super - anchorPeriod.super;
            }
            else
            {
                //Non-annual                
                int subPeriods = 1;
                if (anchorPeriod.freq == EFreq.Quarterly) subPeriods = 4;
                else if (anchorPeriod.freq == EFreq.Monthly) subPeriods = 12;
                else if (anchorPeriod.freq == EFreq.Undated) subPeriods = 1;
                //For quarterly data for instance, each super period amounts to 4 observations. Therefore the multiplication.
                int offset = subPeriods * (gt.super - anchorPeriod.super) + (gt.sub - anchorPeriod.sub);
                int index = anchorPeriodPositionInArray + offset;
                rv = index;
            }

            return rv;
        }

        //public static int FromGekkoTimeToArrayIndex(GekkoTime gt, EFreq freqEnum,int anchorPeriodPositionInArray, int anchorSuperPeriod, int anchorSubPeriod)
        //{            
        //    //this.anchorPeriod.sub is always 1 at the moment, and will always be 1 for Annual.
        //    //but we cannot count on anchorSubPeriod being 1 forever (for instance for daily obs)   
        //    int rv = -12345;
        //    if (freqEnum == EFreq.Annual)
        //    {
        //        //Special treatment in order to make it fast.
        //        //undated freq could return fast in the same way as this??
        //        rv = anchorPeriodPositionInArray + gt.super - anchorSuperPeriod;
        //    }
        //    else
        //    {
        //        //Non-annual                
        //        int subPeriods = 1;
        //        if (freqEnum == EFreq.Quarterly) subPeriods = 4;
        //        else if (freqEnum == EFreq.Monthly) subPeriods = 12;
        //        else if (freqEnum == EFreq.Undated) subPeriods = 1;
        //        //For quarterly data for instance, each super period amounts to 4 observations. Therefore the multiplication.
        //        int offset = subPeriods * (gt.super - anchorSuperPeriod) + (gt.sub - anchorSubPeriod);
        //        int index = anchorPeriodPositionInArray + offset;
        //        rv = index;
        //    }

        //    return rv;
        //}

        private int ResizeDataArray(GekkoTime gt)
        {
            return ResizeDataArray(gt, true);
        }

        private int ResizeDataArray(GekkoTime gt, bool adjustStartEndDates)
        {
            int index = GetArrayIndex(gt);
            while (index < 0 || index >= this.dataArray.Length)
            {
                //Resize data array
                //Keeps on going until the array is large enough.
                double n = Math.Max(this.dataArray.Length, 4);  //the length could be 1 (or maybe even 0), so we translate 0, 1, 2, 3 into 4 which will become 6 with 1.5 times expandRate.
                double[] newDataArray = new double[(int)(n * Globals.defaultExpandRateForDataArrays)];
                InitializeDataArray(newDataArray);
                if (index >= this.dataArray.Length)
                {
                    //new periods after end
                    System.Array.Copy(this.dataArray, newDataArray, this.dataArray.Length);
                }
                else
                {
                    //new periods added before start
                    int diffSize = newDataArray.Length - this.dataArray.Length;
                    System.Array.Copy(this.dataArray, 0, newDataArray, diffSize, this.dataArray.Length);
                    this.anchorPeriodPositionInArray += diffSize;
                    if (adjustStartEndDates)  //only for setting data
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
                this.dataArray = newDataArray;
                index = GetArrayIndex(gt);
            }
            return index;
        }

        private void InitDataArray(GekkoTime t)
        {
            if (this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless error #10");
                throw new GekkoException();
            }
            else
            {
                //The anchor is set in the middle of the array, and the anchor date is set to gt.
                this.dataArray = new double[Globals.defaultPeriodsWhenCreatingTimeSeries];
                this.anchorPeriodPositionInArray = Globals.defaultPeriodsWhenCreatingTimeSeries / 2;  //possible to simulate 100 years forwards, and have data 100 years back.
                InitializeDataArray(this.dataArray);  //may fill it with NaN's
                                                      //the following two will always be fixed to what they
                                                      //were for the very first observation entering the double[] array (unless the array is resized).
                this.anchorPeriod = t;
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
                hash += ((ScalarString)indexes[i])._string2;
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

        public IVariable Add(GekkoSmpl smpl, IVariable x)
        {
            IVariable rv = null;            

            int n = GekkoTime.Observations(smpl.t0, smpl.t3);
            double[] data = new double[n];

            if (x.Type() == EVariableType.Series)
            {                
                TimeSeries xx = x as TimeSeries;
                TimeSeries tsl = new TimeSeries(ETimeSeriesType.TimeSeriesLight, smpl);

                //LIGHTFIXME: speedup with arrays
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    tsl.SetData(t, this.GetData(smpl, t) + xx.GetData(smpl, t));
                }

                rv = tsl;
            }
            else if (x.Type() == EVariableType.Val)
            {
                TimeSeries tsl = new TimeSeries(ETimeSeriesType.TimeSeriesLight, smpl);
                ScalarVal xx = x as ScalarVal;

                //LIGHTFIXME: speedup with arrays
                foreach (GekkoTime gt in smpl.Iterate03())
                {
                    tsl.SetData(gt, this.GetData(smpl, gt) + xx.val);
                }

                rv = tsl;
            }
            return rv;
        }

        public IVariable Subtract(GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("Ts error 2");
            return null;
        }

        public IVariable Multiply(GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("Ts error 3");
            return null;
        }

        public IVariable Divide(GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("Ts error 4");
            return null;
        }

        public IVariable Power(GekkoSmpl smpl, IVariable x)
        {
            G.Writeln2("Ts error 5");
            return null;
        }

        public IVariable Negate(GekkoSmpl smpl)
        {
            TimeSeries ts = new TimeSeries(ETimeSeriesType.TimeSeriesLight, smpl);
            foreach (GekkoTime t in smpl.Iterate03())
            {
                ts.SetData(t, -this.GetData(smpl, t));
            }
            return ts;
        }    

        public IVariable Indexer(GekkoSmpl smpl, params IVariable[] indexes)
        {
            IVariable rv = null;

            if (indexes.Length == 1 && indexes[0].Type() == EVariableType.Val)
            {
                int i = O.ConvertToInt(indexes[0]);

                //TODO: Broken lags!!

                if (IsLagOrLead(i))
                {
                    //must be a lag
                    if (this.Type(ETimeSeriesType.TimeSeriesLight))
                    {
                        //just move the offset!
                        //this object is not used in other places, and will soon be garbage collected anyway
                        this.anchorPeriodPositionInArray += i;
                        rv = this;
                    }
                    else
                    {
                        //cannot offset, since this object lives in a databank, so that would
                        //yield bad side-effects.
                        TimeSeries ts = new TimeSeries(ETimeSeriesType.TimeSeriesLight, smpl);
                        foreach (GekkoTime t in smpl.Iterate03())
                        {
                            ts.SetData(t, this.GetData(smpl, t.Add(i)));
                        }
                        rv = ts;
                    }                    
                }
                else
                {
                    if (this.freq == EFreq.Annual || this.freq == EFreq.Undated)
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
            }
            else if (indexes.Length == 1 && indexes[0].Type() == EVariableType.Date)
            {
                double d = this.GetData(smpl, ((ScalarDate)indexes[0]).date);
                rv = new ScalarVal(d);
            }
            else
            {
                //Not x[2] or x[2020q1]
                rv = FindArrayTimeSeries(indexes, false);
            }

            return rv;
        }

        private bool Type(ETimeSeriesType type)
        {
            //type is not used here, just a decorator
            return this.name == null;  //then this.meta will also be null, but we only test .name
        }

        private TimeSeries FindArrayTimeSeries(IVariable[] indexes, bool isLhs)
        {
            if (indexes.Length == 0)
            {
                G.Writeln2("*** ERROR: Indexer has 0 length");
                throw new GekkoException();
            }
            TimeSeries ts = null;
            int stringCount = 0;
            foreach (IVariable iv in indexes)
            {
                if (iv.Type() == EVariableType.String)
                {
                    stringCount++;
                }
            }
            if (indexes.Length == stringCount)
            {
                
                string[] keys = new string[indexes.Length];
                for (int i = 0; i < indexes.Length; i++)
                {
                    ScalarString ss = indexes[i] as ScalarString;
                    keys[i] = ss._string2;
                }               

                if (this.storage == null)
                {
                    string txt = null; foreach (string ss in keys) txt += "'" + ss + "', ";
                    G.Writeln2("*** ERROR: The variable '" + this.name + "' is not an array-timeseries.");
                    G.Writeln("           Indexer used: [" + txt.Substring(0, txt.Length - 2) + "]", Color.Red);
                    G.Writeln("           You may use '" + this.name + " = series(" + keys.Length + ");' to create it,", Color.Red);
                    G.Writeln("           perhaps with 'CREATE " + this.name + ";' first.", Color.Red);
                    throw new GekkoException();
                }

                IVariable iv = null;                
                this.storage.TryGetValue(new MapMultidimItem(keys), out iv);

                if (iv == null)
                {
                    if (!isLhs)
                    {
                        string txt = null; foreach (string ss in keys) txt += "'" + ss + "', ";
                        G.Writeln2("*** ERROR: The series '" + this.name + "' did not contain this element:");
                        G.Writeln2("*** ERROR: [" + txt.Substring(0, txt.Length - 2) + "]");
                        throw new GekkoException();
                    }
                    else
                    {
                        ts = new TimeSeries(this.freq, "[[array-timeseries]]");
                        this.storage.AddIVariableWithOverwrite(new MapMultidimItem(keys), ts);
                    }
                }
                else
                {
                    ts = iv as TimeSeries;
                    if (ts == null)
                    {
                        G.Writeln2("*** ERROR: Array-timeseries element is non-series.");
                        throw new GekkoException();
                    }
                }                   
                
            }
            else
            {
                //FAIL
                string s = null;
                foreach (IVariable iv in indexes)
                {
                    s += iv.Type().ToString() + ", ";
                }
                G.Writeln2("*** ERROR: Timeseries []-index with these argument types: " + s.Substring(0, s.Length - (", ").Length));
                throw new GekkoException();
            }

            return ts;
        }

        private TimeSeries FindArrayTimeSeriesOLDDELETE(IVariable[] indexes, bool isLhs)
        {
            TimeSeries ts = null;
            int stringCount = 0;
            foreach (IVariable iv in indexes)
            {
                if (iv.Type() == EVariableType.String)
                {
                    stringCount++;
                }
            }
            if (indexes.Length == stringCount)
            {
                string s = G.RemoveFreqIndicator(this.name);
                if (true)
                {
                    string hash = GetHashCodeFromIvariables(indexes);
                    string varname = s + Globals.symbolTurtle + hash + Globals.freqIndicator + G.GetFreq(this.freq);
                    ts = this.meta.parentDatabank.GetIVariable(varname) as TimeSeries;  //should not be able to return null, since no-sigil name is timeseries                    
                    if (ts == null)
                    {
                        if (!isLhs)
                        {
                            G.Writeln2("*** ERROR: Could not find " + G.PrettifyTimeseriesHash(varname, true, false));
                            throw new GekkoException();
                        }
                        else
                        {
                            ts = new TimeSeries(this.freq, varname);
                            this.meta.parentDatabank.AddIVariable(ts);
                        }
                    }
                }
            }
            else
            {
                string s = null;
                foreach (IVariable iv in indexes)
                {
                    s += iv.Type().ToString() + ", ";
                }
                G.Writeln2("*** ERROR: Timeseries []-index with these argument types: " + s.Substring(0, s.Length - (", ").Length));
                throw new GekkoException();
            }

            return ts;
        }

        private static bool IsLagOrLead(int i)
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

        public void InjectAdd(GekkoSmpl smpl, IVariable x, IVariable y)
        {


            




            if (x.Type() == EVariableType.Series && y.Type() == EVariableType.Series)
            {
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    this.SetData(t, ((TimeSeries)x).GetData(smpl, t) + ((TimeSeries)y).GetData(smpl, t));
                }
            }
            else if (x.Type() == EVariableType.Val && y.Type() == EVariableType.Series)
            {
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    this.SetData(t, ((ScalarVal)x).val + ((TimeSeries)y).GetData(smpl, t));
                }
            }
            else if (x.Type() == EVariableType.Series && y.Type() == EVariableType.Val)
            {
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    this.SetData(t, ((TimeSeries)x).GetData(smpl, t) + ((ScalarVal)y).val);
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
            return this.GetData(null, t);
        }

        public double ConvertToVal()
        {
            G.Writeln2("*** ERROR: Cannot extract a VAL from " + G.GetTypeString(this) + " type");
            throw new GekkoException();
        }

        public string ConvertToString()
        {
            G.Writeln2("Ts error 14");
            return null;
        }

        public GekkoTime ConvertToDate(O.GetDateChoices c)
        {
            G.Writeln2("Ts error 15");
            return Globals.tNull;
        }

        public List<IVariable> ConvertToList()
        {
            G.Writeln2("Ts error 16");
            return null;
        }

        public EVariableType Type()
        {
            return EVariableType.Series;
        }

        public void IndexerSetData(GekkoSmpl smpl, IVariable rhsExpression, params IVariable[] indexes)
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
                    if (this.freq == EFreq.Annual || this.freq == EFreq.Undated)
                    {
                        double d = rhsExpression.ConvertToVal();  //will fail with an error unless VAL or 1x1 matrix
                        GekkoTime t = new GekkoTime(this.freq, i, 1);
                        this.SetData(t, d);
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
                this.SetData(((ScalarDate)(indexes[0])).date, d);  //will fail with an error if freqs do not match
            }
            else 
            {
                //Will fail with an error if not all indexes are of STRING type
                TimeSeries ts = FindArrayTimeSeries(indexes, true);
                foreach (GekkoTime t in smpl.Iterate03())
                {
                    ts.SetData(t, rhsExpression.GetVal(t));  //will fail if expression is wrong type
                }
            }
        }

        public void SetDirty(bool b1)
        {
            if (meta != null) this.meta.SetDirty(b1);
        }

        //public void SetGhost(bool b2)
        //{
        //    if (meta != null) this.meta.SetGhost(b2);
        //}

        public bool IsDirty()
        {
            if (meta == null) return false; //not used for light TimeSeries
            return this.meta.IsDirty();
        }

        public bool IsArrayTimeseries()
        {
            return this.storageDim > 0;
        }

        /// <summary>
        /// Creates a clone of the TimeSeries, copying all fields. Used for copying databanks in RAM.
        /// </summary>
        /// <returns>The cloned TimeSeries object.</returns>
        public IVariable DeepClone()
        {            
            //Always make sure new fields are remembered in the DeepClone() method
            TimeSeries tsCopy = new TimeSeries(this.freq, this.name);
            if (this.dataArray == null)
            {
                tsCopy.dataArray = null;
            }
            else
            {
                tsCopy.dataArray = new double[this.dataArray.Length];
                System.Array.Copy(this.dataArray, tsCopy.dataArray, this.dataArray.Length);
            }
            tsCopy.anchorPeriod = this.anchorPeriod;
            tsCopy.anchorPeriodPositionInArray = this.anchorPeriodPositionInArray;
            tsCopy.isTimeless = this.isTimeless;
            tsCopy.storage = this.storage;
            tsCopy.storageDim = this.storageDim;

            if (this.meta != null)
            {
                tsCopy.meta = new TimeSeriesMetaInformation();
                tsCopy.meta.firstPeriodPositionInArray = this.meta.firstPeriodPositionInArray;
                tsCopy.meta.lastPeriodPositionInArray = this.meta.lastPeriodPositionInArray;
                if (this.meta.label != null) tsCopy.meta.label = this.meta.label;
                if (this.meta.source != null) tsCopy.meta.source = this.meta.source;
                if (this.meta.stamp != null) tsCopy.meta.stamp = this.meta.stamp;
                //tsCopy.SetGhost(this.IsArrayTimeseries());                
            }
            return tsCopy;
        }

        public void DeepTrim()
        {
            //Handle sub-series!!! #987539875
            this.Trim();
        }

    }


    [ProtoContract]
    public class TimeSeriesMetaInformation
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
        /// <summary>
        /// The time of the last last change in the timeseries
        /// </summary>
        /// 
        //[ProtoMember(6)]
        //private bool isGhost = false; //A ghost variable x is a placeholder for x['a', 'b'] for example. This x variable should not be used for anything.
        [ProtoMember(7)]        
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
