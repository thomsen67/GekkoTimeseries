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

namespace Gekko
{
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
        /// <summary>
        /// Indicates the frequency of the TimeSeries as a string ('a', 'q', 'm', 'u'). 
        /// May become obsolete, please use freqEnum instead.
        /// </summary>
        //[ProtoMember(1)]
        //public string frequency;
        ///// <summary>
        ///// Indicates the frequency of the TimeSeries.
        ///// </summary>
        [ProtoMember(2)]
        public EFreq freq;
        /// <summary>
        /// The name of the variable. In a databank, this name corresponds to the key that the TimeSeries is stored under,
        /// except that non-annual frequencies have a '%q' (quarterly), '%m' (monthly) etc. appended to the key, in order
        /// to be able to store the same variable name with different frequencies in the same databank.
        /// </summary>
        [ProtoMember(3)]
        public string variableName;
        /// <summary>
        /// The array containing the time series data. This array is initialized with NaN values, and the array may resize
        /// itself if necessary to store a particular observation.
        /// </summary>
        [ProtoMember(4, IsPacked = true)]  //a bit faster, and a bit smaller file (also when zipped)        
        public double[] dataArray;  //BEWARE: if altering directly, make sure that .protect in the databank is not set!!
        /// <summary>
        /// The 'super' period (year) corresponding to the anchor date.
        /// </summary>
        [ProtoMember(5)]
        public int anchorSuperPeriod;
        /// <summary>
        /// The 'sub' period (quarter, month, etc.) corresponding to the anchor date.
        /// </summary>
        [ProtoMember(6)]
        public int anchorSubPeriod;
        /// <summary>
        /// The index corresponding to the anchor date.
        /// </summary>
        [ProtoMember(7)]
        public int anchorPeriodPositionInArray;
        /// <summary>
        /// The label of the timeseries (meta-data), for instance 'GDP in current prices'.
        /// </summary>
        [ProtoMember(8)]
        public string label;
        /// <summary>
        /// The source of the timeseries (meta-data), for instance 'Statistics Denmark, National Accounts'.
        /// </summary>
        [ProtoMember(9)]
        public string source;
        /// <summary>
        /// First data in timeseries: points to the index in the data array.
        /// </summary>        
        [ProtoMember(10)]
        public int firstPeriodPositionInArray = int.MaxValue;
        /// <summary>
        /// Last data in timeseries: points to the index in the data array.
        /// </summary>        
        [ProtoMember(11)]
        public int lastPeriodPositionInArray = int.MinValue;
        /// <summary>
        /// This field is only used when CLOSEing an OPEN bank, to see if
        /// the bank needs to be rewritten. Should not be put into protobuffer.
        /// </summary>        
        [ProtoMember(12)]
        public string stamp;
        /// <summary>
        /// The time of the last last change in the timeseries
        /// </summary>
        /// 

        [ProtoMember(13)]
        private bool isGhost = false; //A ghost variable x is a placeholder for x['a', 'b'] for example. This x variable should not be used for anything.

        [ProtoMember(14)]
        private bool isTimeless = false; //a timeless variable is like a ScalarVal (VAL). A timeless variable puts the value in dataArray[0]
        
        private bool isDirty = false;  //do not keep this in protobuf
        public Databank parentDatabank = null;  //do not keep this in protobuf

        private TimeSeries()
        {
            //This is ONLY because protobuf-net needs it! 
            //Empty timeseries should not be created that way.
        }

        /// <summary>
        /// Constructor that creates a new TimeSeries object with a particular frequency and variable name.
        /// </summary>
        /// <param name="frequency">The frequency of the timeseries</param>
        /// <param name="variableName">The variable name of the timeseries</param>
        public TimeSeries(EFreq frequency, string variableName)
        {
            this.freq = frequency;            
            this.variableName = variableName;
        }

        /// <summary>
        /// Creates a clone of the TimeSeries, copying all fields. Used for copying databanks in RAM.
        /// </summary>
        /// <returns>The cloned TimeSeries object.</returns>
        public TimeSeries Clone()
        {
            //DimensionCheck();
            //Always make sure new fields are remembered in the Clone() method
            TimeSeries tsCopy = new TimeSeries(this.freq, this.variableName);
            if (this.dataArray == null)
            {
                tsCopy.dataArray = null;
            }
            else
            {
                tsCopy.dataArray = new double[this.dataArray.Length];
                System.Array.Copy(this.dataArray, tsCopy.dataArray, this.dataArray.Length);
            }
            tsCopy.anchorSuperPeriod = this.anchorSuperPeriod;
            tsCopy.anchorSubPeriod = this.anchorSubPeriod;
            tsCopy.anchorPeriodPositionInArray = this.anchorPeriodPositionInArray;
            tsCopy.firstPeriodPositionInArray = this.firstPeriodPositionInArray;
            tsCopy.lastPeriodPositionInArray = this.lastPeriodPositionInArray;
            if (this.label != null) tsCopy.label = string.Copy(this.label);  //using string.Copy() probably not be necessary, but we use it for extra safety
            if (this.source != null) tsCopy.source = string.Copy(this.source); //using string.Copy() probably not be necessary, but we use it for extra safety                        
            if (this.stamp != null) tsCopy.stamp = string.Copy(this.stamp); //using string.Copy() probably not be necessary, but we use it for extra safety                        
            tsCopy.isGhost = this.isGhost;
            tsCopy.isTimeless = this.isTimeless;            
            return tsCopy;
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
            //DimensionCheck();
            if (this.parentDatabank != null && this.parentDatabank.protect) Program.ProtectError("You cannot truncate a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
            int indexStart = this.GetArrayIndex(start);
            int indexEnd = this.GetArrayIndex(end);

            int newFirst = Math.Max(this.firstPeriodPositionInArray, indexStart);
            int newLast = Math.Min(this.lastPeriodPositionInArray, indexEnd);

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
                this.firstPeriodPositionInArray = newFirst;
                this.lastPeriodPositionInArray = newLast;

                //NOTE: after this, the anchor position may be outside first/lastPeriodPositionInArray. 
                //But this should not be a problem: it is only a hook
                //that translates a date into an index and vice versa.

                //When Truncate() is used for writing databanks, the timeseries
                //will be Trim()'ed anyway, so these missings will disappear. But since the method
                //could be used for other purposes later on, we set the values to missings explicitly
                //The time loss is very small, and usually WRITE is not time-truncated anyway.

                for (int i = 0; i < this.firstPeriodPositionInArray; i++)
                {
                    this.dataArray[i] = double.NaN;
                }
                for (int i = this.lastPeriodPositionInArray + 1; i < this.dataArray.Length; i++)
                {
                    this.dataArray[i] = double.NaN;
                }
            }                        
            this.SetDirty(true);
        }

        private void SetNullPeriod()
        {
            this.firstPeriodPositionInArray = Globals.firstPeriodPositionInArrayNull;
            this.lastPeriodPositionInArray = Globals.lastPeriodPositionInArrayNull;
        }

        public bool IsNullPeriod()
        {
            return this.firstPeriodPositionInArray == Globals.firstPeriodPositionInArrayNull && this.lastPeriodPositionInArray == Globals.lastPeriodPositionInArrayNull;
        }

        /// <summary>
        /// Puts a date stamp into the timeseries, see also .isDirty
        /// </summary>
        public void Stamp()
        {
            //See also #80927435209843
            this.stamp = Globals.dateStamp;
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
            if (!(this.firstPeriodPositionInArray == 0 && this.lastPeriodPositionInArray == this.dataArray.Length - 1))  //already trimmed                
            {
                int size = this.lastPeriodPositionInArray - this.firstPeriodPositionInArray + 1;
                double[] temp = new double[size];
                Array.Copy(this.dataArray, this.firstPeriodPositionInArray, temp, 0, size);
                int first = this.firstPeriodPositionInArray;
                //Correct these pointers accordingly
                this.anchorPeriodPositionInArray += -first;
                this.firstPeriodPositionInArray += -first; // --> 0
                this.lastPeriodPositionInArray += -first;  // --> size-1
                this.dataArray = temp;  //point to this array
            }
        }

        /// <summary>
        /// Gets the timeseries value corresponding to the given period.
        /// </summary>
        /// <param name="t">The period.</param>
        /// <returns>The value (double.NaN if missing)</returns>
        /// <exception cref="GekkoException">Exception if frequency of timeseries and period do not match.</exception>
        public double GetData(GekkoTime t)
        {
            //DimensionCheck();
            if (this.freq != t.freq)
            {
                //t.freq will almost always correspond to the frequency setting in Gekko, that is, Program.options.freq.
                //When getting the timeseries, a "%q" or "%m" is appended to the name, because that is the way other frequencies
                //are stored in the Dictionary. (Note that these '%' have nothing to do with scalar variables!).
                //So if Program.options.freq is quarterly, 'fy' will point to 'fy%q', and that
                //timeseries should have .freqEnum = EFreq.Quarterly. This is basically what the above IF tests. It is for
                //safety, and might be omitted at some point.
                G.Writeln2("*** ERROR: Freq mismatch");
                throw new GekkoException();
            }
            if (this.dataArray == null)
            {
                //If no data has been added to the timeseries, NaN will always be returned.
                if (this.IsGhost())
                {
                    G.Writeln2("*** ERROR: The variable '" + this.variableName + "' is an array-timeseries,");
                    G.Writeln("           but is used as a normal timeseries here (without []-indexer)", Color.Red);
                    Program.ArrayTimeseriesTip(this.variableName);
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
                    return double.NaN;  //out of bounds, we return a missing value (NaN)
                }
                else
                {
                    return this.dataArray[index];
                }
            }
        }

       

        public void SetTimelessData(double value)
        {
            if (!this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #100");
                throw new GekkoException();
            }
            if (this.parentDatabank != null && this.parentDatabank.protect) Program.ProtectError("You cannot change an observation in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
                        
            if (this.dataArray == null)
            {
                this.dataArray = new double[1];
            }
            this.dataArray[0] = value;
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
            if (this.parentDatabank != null && this.parentDatabank.protect) Program.ProtectError("You cannot change an observation in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
            
            if (this.freq != t.freq)
            {
                //See comment to GetData()
                G.Writeln2("*** ERROR: Freq mismatch");
                throw new GekkoException();
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
                if (index > this.lastPeriodPositionInArray)
                {
                    this.lastPeriodPositionInArray = index;
                }
                if (index < this.firstPeriodPositionInArray)
                {
                    this.firstPeriodPositionInArray = index;
                }
                this.SetDirtyGhost(true, false);
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

            if (this.freq != gt1.freq || gt1.freq != gt2.freq)
            {
                //This check: better safe than sorry!
                //See comment to GetData()
                G.Writeln2("*** ERROR: Freq mismatch");
                throw new GekkoException();
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
                if (index2 > this.lastPeriodPositionInArray)
                {
                    this.lastPeriodPositionInArray = index2;
                }
                if (index1 < this.firstPeriodPositionInArray)
                {
                    this.firstPeriodPositionInArray = index1;
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
            if (this.parentDatabank != null && this.parentDatabank.protect) Program.ProtectError("You cannot change observations in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
            //Program.ErrorIfDatabanksSwapped(this);
            if (this.freq != gt1.freq || gt1.freq != gt2.freq)
            {
                //This check: better safe than sorry!
                //See comment to GetData()
                G.Writeln2("*** ERROR: Freq mismatch");
                throw new GekkoException();
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
            if (index2 > this.lastPeriodPositionInArray)
            {
                this.lastPeriodPositionInArray = index2;
            }
            if (index1 < this.firstPeriodPositionInArray)
            {
                this.firstPeriodPositionInArray = index1;
            }
            this.SetDirtyGhost(true, false);

        }

        public void SetDirtyGhost(bool b1, bool b2)
        {
            this.isDirty = b1;
            this.isGhost = b2;
        }

        public void SetDirty(bool b1)
        {
            this.isDirty = b1;            
        }

        public void SetGhost(bool b2)
        {         
            this.isGhost = b2;
        }

        public bool IsDirty()
        {
            return this.isDirty;
        }

        public bool IsGhost()
        {
            return this.isGhost;
        }

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
            return GetPeriod(this.firstPeriodPositionInArray);
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
            return GetPeriod(this.lastPeriodPositionInArray);
        }

        public GekkoTime GetRealDataPeriodFirst()
        {
            if (this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #6");
                throw new GekkoException();
            }
            //Could be sped up by means of looping through dataaraay with GetDataSequence, but oh well...
            //returns tNull if all missing
            //DimensionCheck();
            GekkoTime realStart = Globals.tNull;
            foreach (GekkoTime dt in new GekkoTimeIterator(GetPeriodFirst(), GetPeriodLast()))
            {
                if (!G.isNumericalError(this.GetData(dt)))
                {
                    //a real number, not missing or infinite
                    realStart = dt;
                    break;
                }
            }
            return realStart;
        }

        public GekkoTime GetRealDataPeriodLast()
        {
            if (this.isTimeless)
            {
                G.Writeln2("*** ERROR: Timeless variable error #7");
                throw new GekkoException();
            }
            //Could be sped up by means of looping through dataaraay with GetDataSequence, but oh well...
            //returns tNull if all missing
            //DimensionCheck();
            GekkoTime realEnd = Globals.tNull;
            foreach (GekkoTime dt in new GekkoTimeIteratorBackwards(GetPeriodLast(), GetPeriodFirst()))
            {
                if (!G.isNumericalError(this.GetData(dt)))
                {
                    //a real number, not missing or infinite
                    realEnd = dt;
                    break;
                }
            }
            return realEnd;
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
            int sub1 = this.anchorSubPeriod + (indexInDataArray - anchorPeriodPositionInArray);
            int addPer = (sub1 - 1) / subPeriods;
            int addSub = (indexInDataArray - anchorPeriodPositionInArray) - subPeriods * addPer;

            int resultSuperPer = this.anchorSuperPeriod + addPer;
            int resultSubPer = this.anchorSubPeriod + addSub;

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
            int rv = FromGekkoTimeToArrayIndex(gt, new GekkoTime(this.freq, this.anchorSuperPeriod, this.anchorSubPeriod), this.anchorPeriodPositionInArray);
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
            //this.anchorSubPeriod is always 1 at the moment, and will always be 1 for Annual.
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
        //    //this.anchorSubPeriod is always 1 at the moment, and will always be 1 for Annual.
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
                        if (this.firstPeriodPositionInArray != Globals.firstPeriodPositionInArrayNull)
                        {
                            this.firstPeriodPositionInArray += diffSize;
                        }
                        if (this.lastPeriodPositionInArray != Globals.lastPeriodPositionInArrayNull)
                        {
                            this.lastPeriodPositionInArray += diffSize;
                        }
                    }
                }
                this.dataArray = newDataArray;
                index = GetArrayIndex(gt);
            }
            return index;
        }

        private void InitDataArray(GekkoTime gt)
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
                this.anchorSuperPeriod = gt.super;
                this.anchorSubPeriod = gt.sub;
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

            int n = GekkoTime.Observations(smpl.t1, smpl.t2);
            double[] data = new double[n];            

            if (x.Type() == EVariableType.TimeSeries)
            {
                TimeSeriesLight tsl = new TimeSeriesLight();

                TimeSeries xx = x as TimeSeries;
                if (xx != null)
                {
                    int counter = 0;
                    foreach (GekkoTime gt in new GekkoTimeIterator(smpl.t1, smpl.t2))
                    {
                        data[counter] = this.GetData(gt) + xx.GetData(gt);
                        counter++;
                    }
                }
                else
                {
                    TimeSeriesLight xx2 = x as TimeSeriesLight;
                    int counter = 0;
                    foreach (GekkoTime gt in new GekkoTimeIterator(smpl.t1, smpl.t2))
                    {
                        data[counter] = this.GetData(gt) + xx2.GetData(gt);
                        counter++;
                    }
                }
                tsl.storage = data;
                tsl.anchorPeriod = smpl.t1;
                tsl.anchorPeriodPositionInArray = 0;
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
            G.Writeln2("Ts error 6");
            return null;
        }

        public IVariable Indexer(GekkoSmpl smpl, bool isLhs, params IVariable[] index)
        {
            G.Writeln2("Ts error 7");
            return null;
        }

        public IVariable Indexer(GekkoSmpl smpl, IVariablesFilterRange index)
        {
            G.Writeln2("Ts error 8");
            return null;
        }

        public IVariable Indexer(GekkoSmpl smpl, IVariablesFilterRange index1, IVariablesFilterRange index2)
        {
            G.Writeln2("Ts error 9");
            return null;
        }

        public IVariable Indexer(GekkoSmpl smpl, IVariable index, IVariablesFilterRange indexRange)
        {
            G.Writeln2("Ts error 10");
            return null;
        }

        public IVariable Indexer(GekkoSmpl smpl, IVariablesFilterRange indexRange, IVariable index)
        {
            G.Writeln2("Ts error 11");
            return null;
        }

        public void InjectAdd(GekkoSmpl smpl, IVariable x, IVariable y)
        {
            G.Writeln2("Ts error 12");
            return;
        }

        public double GetVal(GekkoSmpl smpl)
        {
            G.Writeln2("Ts error 13");
            return double.NaN;
        }

        public string GetString()
        {
            G.Writeln2("Ts error 14");
            return null;
        }

        public GekkoTime GetDate(O.GetDateChoices c)
        {
            G.Writeln2("Ts error 15");
            return Globals.tNull;
        }

        public List<IVariable> GetList()
        {
            G.Writeln2("Ts error 16");
            return null;
        }

        public EVariableType Type()
        {
            return EVariableType.TimeSeries;
        }

    }

    //[ProtoContract]
    //public class Dim
    //{
    //    public Dim()
    //    {
    //        //This is ONLY because protobuf-net needs it
    //        this.timeSeriesArray = new GekkoDictionary<string, TimeSeries>(StringComparer.OrdinalIgnoreCase);
    //    }

    //    [ProtoMember(1)]
    //    public GekkoDictionary<string, TimeSeries> timeSeriesArray;
    //}

    //public class GekkoTimeSeriesIterator : IEnumerable<TimeSeries>
    //{
    //    private TimeSeries _ts;

    //    public GekkoTimeSeriesIterator(TimeSeries ts)
    //    {
    //        _ts = ts;
    //    }

    //    public IEnumerator<TimeSeries> GetEnumerator()
    //    {
    //        if (_ts.dimensions == 0)
    //        {
    //            yield return _ts;
    //        }
    //        else
    //        {
    //            foreach (TimeSeries ts in _ts.dim.timeSeriesArray.Values)
    //            {
    //                yield return ts;
    //            }
    //        }
    //    }

    //    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    //    {
    //        G.Writeln("*** ERROR: iterator problem");
    //        throw new GekkoException();
    //    }
    //}


}
