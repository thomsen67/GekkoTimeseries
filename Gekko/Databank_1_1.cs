/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;
using Gekko;
using ProtoBuf;

namespace Gekko
{
    [ProtoContract]
    public class TimeSeries_1_1
    {
        /// <summary>
        /// Indicates the frequency of the TimeSeries as a string ('a', 'q', 'm', 'u'). 
        /// May become obsolete, please use freqEnum instead.
        /// </summary>
        [ProtoMember(1)]
        public string frequency;
        /// <summary>
        /// Indicates the frequency of the TimeSeries.
        /// </summary>
        [ProtoMember(2)]
        public EFreq freqEnum;
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
        public Databank_1_1 parentDatabank = null;  //do not keep this in protobuf

        private TimeSeries_1_1()
        {
            //This is ONLY because protobuf-net needs it! 
            //Empty timeseries should not be created that way.
        }

        /// <summary>
        /// Constructor that creates a new TimeSeries object with a particular frequency and variable name.
        /// </summary>
        /// <param name="frequency">The frequency of the timeseries</param>
        /// <param name="variableName">The variable name of the timeseries</param>
        public TimeSeries_1_1(EFreq frequency, string variableName)
        {
            this.freqEnum = frequency;
            this.frequency = G.ConvertFreq(frequency);
            this.variableName = variableName;
        }

        /// <summary>
        /// Creates a clone of the TimeSeries, copying all fields. Used for copying databanks in RAM.
        /// </summary>
        /// <returns>The cloned TimeSeries object.</returns>
        public TimeSeries_1_1 Clone()
        {
            //DimensionCheck();
            //Always make sure new fields are remembered in the Clone() method
            TimeSeries_1_1 tsCopy = new TimeSeries_1_1(this.freqEnum, this.variableName);
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
            if (this.freqEnum != t.freq)
            {
                //t.freq will almost always correspond to the frequency setting in Gekko, that is, Program.options.freq.
                //When getting the timeseries, a "%q" or "%m" is appended to the name, because that is the way other frequencies
                //are stored in the Dictionary. (Note that these '%' have nothing to do with scalar variables!).
                //So if Program.options.freq is quarterly, 'fy' will point to 'fy%q', and that
                //timeseries should have .freqEnum = EFreq.Q. This is basically what the above IF tests. It is for
                //safety, and might be omitted at some point.
                new Error("Freq mismatch");
                //throw new GekkoException();
            }
            if (this.dataArray == null)
            {
                //If no data has been added to the timeseries, NaN will always be returned.
                if (this.IsGhost())
                {
                    new Error("The variable '" + this.variableName + "' is an array-timeseries, but is used as a normal timeseries here (without []-indexer)." + Program.ArrayTimeseriesTip(this.variableName));                    
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
                new Error("Timeless variable error #100");
                //throw new GekkoException();
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

            if (this.freqEnum != t.freq)
            {
                //See comment to GetData()
                new Error("Freq mismatch");
                //throw new GekkoException();
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

            if (this.freqEnum != gt1.freq || gt1.freq != gt2.freq)
            {
                //This check: better safe than sorry!
                //See comment to GetData()
                new Error("Freq mismatch");
                //throw new GekkoException();
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
                new Error("Timeless variable error #2");
                //throw new GekkoException();
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
                new Error("Timeless variable error #3");
                //throw new GekkoException();
            }
            if (this.parentDatabank != null && this.parentDatabank.protect) Program.ProtectError("You cannot change observations in a timeseries residing in a non-editable databank, see OPEN<edit> or UNLOCK");
            //Program.ErrorIfDatabanksSwapped(this);
            if (this.freqEnum != gt1.freq || gt1.freq != gt2.freq)
            {
                //This check: better safe than sorry!
                //See comment to GetData()
                new Error("Freq mismatch");
                //throw new GekkoException();
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
                new Error("Timeless variable error #4");
                //throw new GekkoException();
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
                new Error("Timeless variable error #5");
                //throw new GekkoException();
            }
            return GetPeriod(this.lastPeriodPositionInArray);
        }

        public GekkoTime GetRealDataPeriodFirst()
        {
            if (this.isTimeless)
            {
                new Error("Timeless variable error #6");
                //throw new GekkoException();
            }
            //Could be sped up by means of looping through dataaraay with GetDataSequence, but oh well...
            //returns tNull if all missing
            //DimensionCheck();
            GekkoTime realStart = GekkoTime.tNull;
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
                new Error("Timeless variable error #7");
                //throw new GekkoException();
            }
            //Could be sped up by means of looping through dataaraay with GetDataSequence, but oh well...
            //returns tNull if all missing
            //DimensionCheck();
            GekkoTime realEnd = GekkoTime.tNull;
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
                new Error("Timeless variable error #7");
                //throw new GekkoException();
            }
            //The inverse method is GetArrayIndex()
            //Should maybe be private method? But then how to unit-test?
            //see also AddToPeriod()
            //DimensionCheck();
            int subPeriods = 1;
            if (this.freqEnum == EFreq.Q) subPeriods = 4;
            else if (this.freqEnum == EFreq.M) subPeriods = 12;
            else if (this.freqEnum == EFreq.U) subPeriods = 1;

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
            GekkoTime t = new GekkoTime(this.freqEnum, resultSuperPer, resultSubPer);
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

        private int GetArrayIndex(GekkoTime gt)
        {
            //this.anchorSubPeriod is always 1 at the moment, and will always be 1 for Annual.
            //but we cannot count on anchorSubPeriod being 1 forever (for instance for daily obs)            
            if (this.freqEnum == EFreq.A)
            {
                //Special treatment in order to make it fast.
                //undated freq could return fast in the same way as this??
                return this.anchorPeriodPositionInArray + gt.super - this.anchorSuperPeriod;
            }
            else
            {
                //Non-annual                
                int subPeriods = 1;
                if (this.freqEnum == EFreq.Q) subPeriods = 4;
                else if (this.freqEnum == EFreq.M) subPeriods = 12;
                else if (this.freqEnum == EFreq.U) subPeriods = 1;
                //For quarterly data for instance, each super period amounts to 4 observations. Therefore the multiplication.
                int offset = subPeriods * (gt.super - this.anchorSuperPeriod) + (gt.sub - this.anchorSubPeriod);
                int index = this.anchorPeriodPositionInArray + offset;
                return index;
            }
        }

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
                new Error("Timeless error #10");
                //throw new GekkoException();
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
                    new Error("Expected [] indexer element #" + (i + 1) + " to be STRING");
                    //throw new GekkoException();
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

    }



    [ProtoContract]
    public class Databank_1_1
    {

        //Note the .isDirty field, so methods that change anything must set isDirty = true!
        //Remember new fields in Clear() method and also in G.CloneDatabank()        
        [ProtoMember(1)]
        public GekkoDictionary<string, TimeSeries_1_1> storage;
        public string aliasName = null;
        private string fileNameWithPath = null;  //will be constructed when reading: do not protobuf it        


        public string FileNameWithPath
        {
            //?????????
            //?????????
            //????????? Could this be avoided by means of testing boolean open == true/false, in Program.OpenOrRead()?
            //?????????
            //?????????
            get
            {
                return this.fileNameWithPath;
            }
            set
            {
                if (G.Equal(this.aliasName, Globals.Work) || G.Equal(this.aliasName, Globals.Ref))
                {
                    this.fileNameWithPath = value;  //overwrite filename with latest bank read or merged into Work/Ref
                }
                else
                {
                    //If the bank is not Work or Ref, it must have been opened with OPEN
                    //If there is no filename, put it in. But if there is a filename already, always keep it.
                    //  This may happen in the IMPORT here: OPEN<edit>bank; IMPORT<xlsx>data;
                    //  An IMPORT or READ statement should not alter the filename.
                    if (this.fileNameWithPath == null) this.fileNameWithPath = value;
                    else
                    {
                        //do nothing, keep the first filename encountered. This is the filename that the OPEN databank
                        //is tied to, and that it will be trying to write to when the bank is closed.
                    }
                }
            }
        }
        public bool save = true;  //Don't use protobuffer on this field.
        public int yearStart = -12345;  //only set when reading a bank, not afterwards if timeseries change. Not meant for making loops etc. or critical, only static information about the bank        
        public int yearEnd = -12345;  //only set when reading a bank, not afterwards if timeseries change. Not meant for making loops etc. or critical, only static information about the bank        
        public string info1 = null; //must be taken from DatabankInfo.xml, don't use protobuffer        
        public string date = null; //must be taken from DatabankInfo.xml, don't use protobuffer
        public bool isDirty = false;  //used to see if en OPEN databank must be re-written. Don't use protobuffer on this field.
        public bool protect = false;  //used to set an OPEN databank as protected. Don't use protobuffer on this field.
        //public GekkoDictionary<string, string> tmptmpVars;
        public Program.ReadInfo readInfo = null; //contains info from reading the file, among other things info from the XML file. NOTE: do not store it in protobuf!
        public string fileHash = null; //do not store this in protobuf

        private Databank_1_1()
        {
            //This is ONLY because protobuf-net needs it
            this.storage = new GekkoDictionary<string, TimeSeries_1_1>(StringComparer.OrdinalIgnoreCase);
        }

        public Databank_1_1(string aliasName)
        {
            this.storage = new GekkoDictionary<string, TimeSeries_1_1>(StringComparer.OrdinalIgnoreCase);
            this.aliasName = aliasName;
            //this.aliasNameOriginal = aliasName;
        }

        public void Clear()
        {
            if (this.protect) Program.ProtectError("You cannot clear a non-editable databank, see OPEN<edit> or UNLOCK");
            //aliasName = null; --> keep that name when clearing
            //fileNameWithPath = null;  --> keep that name when clearing
            yearStart = -12345;
            yearEnd = -12345;
            info1 = null;
            date = null;
            this.storage.Clear();
            //this.fileNameWithPath = null;  --> NO! This would be ok regarding READ, but not regarding OPEN<edit/first> for instance --> we need a bank to write back to!
            //this.readInfo = null;  //must be ok to remove, just contains stuff for printing --> but let us keep it for ultra safety for now
            this.isDirty = true;
        }

        public bool ContainsVariable(string variable)
        {
            return ContainsVariable(true, variable);
        }
        public bool ContainsVariable(bool freqAddToName, string variable)
        {
            if (freqAddToName) variable = AddFreqAtEndOfVariableName(variable);
            return this.storage.ContainsKey(variable);
        }


        public static string AddFreqAtEndOfVariableName(string var)  //used most of the time, uses global freq
        {
            return AddFreqAtEndOfVariableName(var, Program.options.freq);
        }

        public static string AddFreqAtEndOfVariableName(string var, string freq)
        {
            return AddFreqAtEndOfVariableName(var, G.ConvertFreq(freq));
        }

        public static string AddFreqAtEndOfVariableName(string var, EFreq freq)
        {
            if (freq == EFreq.A) return var;
            string var2 = var;
            if (freq == EFreq.Q || freq == EFreq.M || freq == EFreq.U)
            {
                if (var2.EndsWith(Globals.freqIndicator + "q") || var2.EndsWith(Globals.freqIndicator + "m") || var2.EndsWith(Globals.freqIndicator + "u"))
                {
                    //this is just a safety measure, to be deleted sometime
                    new Error("strange behavior regarding freq indicator");
                    //throw new GekkoException();
                }
                else
                {
                    if (freq == EFreq.Q)
                    {
                        var2 = var + Globals.freqIndicator + "q";
                    }
                    else if (freq == EFreq.M)
                    {
                        var2 = var + Globals.freqIndicator + "m";
                    }
                    else if (freq == EFreq.U)
                    {
                        var2 = var + Globals.freqIndicator + "u";
                    }
                    else
                    {
                        new Error("#745387463");
                    }
                    //for instance fy%q for fY in quarters
                }
            }
            else
            {
                new Error("Internal error #74389642");
                //throw new GekkoException();
            }
            //nothing done for "a" type
            return var2;
        }



        public void RemoveVariable(string variable)
        {
            RemoveVariable(true, variable);
        }

        public void RemoveVariable(EFreq eFreq, string variable)
        {
            if (this.protect) Program.ProtectError("You cannot remove a timeseries in a non-editable databank, see OPEN<edit> or UNLOCK");
            variable = AddFreqAtEndOfVariableName(variable, eFreq);
            if (ContainsVariable(false, variable))  //do not add freq at the end (has just been added)
            {
                this.storage.Remove(variable);
            }
            this.isDirty = true;
            return;
        }

        public void RemoveVariable(bool freqAddToName, string variable)
        {
            if (this.protect) Program.ProtectError("You cannot remove a timeseries in a non-editable databank, see OPEN<edit> or UNLOCK");
            if (freqAddToName) variable = AddFreqAtEndOfVariableName(variable);
            if (ContainsVariable(false, variable))  //do not add freq at the end (has just been added)
            {
                this.storage.Remove(variable);
            }
            this.isDirty = true;
            return;
        }

        //Generic method, not for outside use!
        private void RemoveVariable(bool freqAddToName, string freq, string variable)
        {
            if (this.protect) Program.ProtectError("You cannot remove a timeseries in a non-editable databank, see OPEN<edit> or UNLOCK");
            if (freqAddToName) variable = AddFreqAtEndOfVariableName(variable, freq);
            if (ContainsVariable(false, variable))  //do not add freq at the end (has just been added)
            {
                this.storage.Remove(variable);
            }
            this.isDirty = true;
            return;
        }

        public void Trim()
        {
            //Used to save some RAM, or just before serializing the databank via protobuf-net.
            DateTime t0 = DateTime.Now;
            foreach (TimeSeries_1_1 ts in this.storage.Values)
            {
                ts.Trim();
            }
            G.WritelnGray("TRIM: " + G.Seconds(t0));
            //This does not change the databank, so this.hasBeenChanged is not touched!!
        }

        public void AddVariable(TimeSeries_1_1 ts)
        {
            AddVariable(true, null, ts, true);
        }

        public void AddVariable(TimeSeries_1_1 ts, bool variableNameCheck)
        {
            AddVariable(true, null, ts, variableNameCheck);
        }

        public void AddVariable(string frequency, TimeSeries_1_1 ts)
        {
            AddVariable(false, frequency, ts, true);
        }

        public void AddVariable(string frequency, TimeSeries_1_1 ts, bool variableNameCheck)
        {
            AddVariable(false, frequency, ts, variableNameCheck);
        }

        //generic method, not for outside use
        private void AddVariable(bool freqAddToName, string frequency, TimeSeries_1_1 ts, bool variableNameCheck)
        {
            if (this.protect) Program.ProtectError("You cannot add a timeseries to a non-editable databank, see OPEN<edit> or UNLOCK");
            string variable = ts.variableName;
            if (variableNameCheck && !G.IsSimpleToken(variable))  //also checks for null and "" and '¤'
            {
                G.Writeln2("*** ERROR in databank: the name '" + variable + "' is not a simple variable name");
                throw new GekkoException();
            }
            if (freqAddToName) variable = AddFreqAtEndOfVariableName(variable);
            else variable = AddFreqAtEndOfVariableName(variable, frequency);
            this.storage.Add(variable, ts);
            ts.parentDatabank = this;
            this.isDirty = true;
        }

        public TimeSeries_1_1 GetVariable(string variable)
        {
            return GetVariable(true, variable);
        }

        public TimeSeries_1_1 GetVariable(bool freqAddToName, string variable)
        {
            if (freqAddToName) variable = AddFreqAtEndOfVariableName(variable);
            TimeSeries_1_1 x = null; this.storage.TryGetValue(variable, out x);
            return x;
        }

        public TimeSeries_1_1 GetVariable(EFreq eFreq, string variable)
        {
            if (eFreq != EFreq.A) variable = AddFreqAtEndOfVariableName(variable, eFreq);  //we do this IF here because it is speed critical code. Else a new string object will be created.
            TimeSeries_1_1 x = null; this.storage.TryGetValue(variable, out x);
            return x;
        }
    }


    class Utilities_1_1
    {
        public static TimeSeries_1_1 FindOrCreateTimeSeriesInDataBank(Databank_1_1 databank, string varName, EFreq frequency)
        {
            //This auto-creates timeseries for use when reading for example tsd or PCIM files
            //Has an overload used for UPD statements etc.
            TimeSeries_1_1 ts = null;
            string varName2 = Databank_1_1.AddFreqAtEndOfVariableName(varName, frequency);

            if (!databank.ContainsVariable(false, varName2))  //a little bit slack, but not much if databank is empty to start with
            {
                ts = new TimeSeries_1_1(frequency, varName);
                databank.AddVariable(ts);
            }
            else
            {
                ts = databank.GetVariable(false, varName2);  //false: do not add options.freq at the end!
            }
            if (!G.Equal(varName, ts.variableName))
            {
                new Error("In findOrCreateTimeSeriesInDataBank(), name");  //safety, can be deleted for speed sometime                
            }
            if (!(frequency == ts.freqEnum))
            {
                new Error("In findOrCreateTimeSeriesInDataBank(), freq");  //safety, can be deleted for speed sometime                
            }
            return ts;
        }

        public static void ReadAllTsdRecords(string file, bool merge, bool isTsdx, Databank_1_1 databank, ref int NaNCounter, Program.ReadInfo readInfo)
        {
            int smallWarnings = 0;
            int emptyWarnings = 0;
            if (!merge)
            {
                databank.Clear();
            }

            if (Globals.threadIsInProcessOfAborting) throw new GekkoException();
            double[] tempArray = new double[100000]; //we don't expect series with more than 100000 obs.
            int counter = 0;
            using (FileStream fs = Program.WaitForFileStream(file, null, Program.GekkoFileReadOrWrite.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                //file should not contain æøå, so no need to use GetTextFromFileWithWait()

                string line = null;
                int nextState = 1;
                string varName = null;
                string frequency = null;
                EFreq freq = EFreq.A;

                DateTime t0 = DateTime.Now;
                DateTime t1 = DateTime.Now;
                DateTime t2 = DateTime.Now;
                int ii = 0;
                int d1min = int.MaxValue;
                int d2max = int.MinValue;
                int d1 = 0, d1sub = 0, d2 = 0, d2sub = 0;
                int countdata = 0;
                int obs = 0;
                int obsLeft = 0;
                //int datalines = 0;
                TimeSeries_1_1 ts = null;

                while ((line = sr.ReadLine()) != null)
                {
                    line = line.Replace("\0", " ");
                    string lineTrim = line.Trim();
                    if (lineTrim == "") continue;

                    if (nextState == 1)
                    {
                        varName = lineTrim;
                        nextState = 2;
                    }

                    else if (nextState == 2)
                    {
                        //read expression
                        string expr = line.Substring(0, 32).Trim();

                        //read stamp
                        string stamp = line.Substring(32, 8).Trim();
                        if (stamp.Length > 0)
                        {
                            try
                            {
                                // #80927435209843
                                int i1 = int.Parse(stamp.Substring(0, 2)); //month
                                int i2 = int.Parse(stamp.Substring(3, 2)); //day
                                int i3 = int.Parse(stamp.Substring(6, 2)); //year
                                if (i3 > 80) i3 = 1900 + i3;  //will work until 2080!
                                else i3 = 2000 + i3;
                                stamp = stamp.Substring(3, 2) + "-" + stamp.Substring(0, 2) + "-" + i3;
                            }
                            catch { };

                        }

                        //read date
                        int iiStart = 37;
                        string date1 = line.Substring(iiStart + 7, 4);
                        string date1sub = line.Substring(iiStart + 11, 2);
                        string date2 = line.Substring(iiStart + 15, 4);
                        string date2sub = line.Substring(iiStart + 19, 2);
                        try
                        {
                            d1 = int.Parse(date1);
                        }
                        catch
                        {
                            new Error("" + varName + ": could not parse '" + date1 + "' as an int (start year)");
                            //throw new GekkoException();
                        }
                        try
                        {
                            d1sub = int.Parse(date1sub);
                        }
                        catch
                        {
                            new Error("" + varName + ": could not parse '" + date1sub + "' as an int (start sub-period)");
                            //throw new GekkoException();
                        }
                        try
                        {
                            d2 = int.Parse(date2);
                        }
                        catch
                        {
                            new Error("" + varName + ": could not parse '" + date2 + "' as an int (end year)");
                            //throw new GekkoException();
                        }
                        try
                        {
                            d2sub = int.Parse(date2sub);
                        }
                        catch
                        {
                            new Error("" + varName + ": could not parse '" + date2sub + "' as an int (end sub-period)");
                            //throw new GekkoException();
                        }
                        frequency = line.Substring(iiStart + 23, 1).ToLower(); //a or q or m

                        d1min = G.GekkoMin(d1, d1min);  //finding min and max years
                        d2max = G.GekkoMax(d2, d2max);

                        //preparing nextState = 3
                        {

                            bool ok = G.IsSimpleToken(varName);
                            string label = null;
                            if (!ok)
                            {
                                if (varName.Length >= 17)
                                {
                                    //Of this type, where the first 16 chars is the name, and the rest is the label
                                    //gdp2            GDP in version 2, mia. DKK
                                    string v1 = varName.Substring(0, 16).Trim();
                                    string v2 = varName.Substring(16, varName.Length - 16).Trim();

                                    if (!G.IsSimpleToken(v1))
                                    {
                                        new Error("Tsd read: the following name is malformed: " + v1 + ". The name should contain letters, digits or underscore only (it seems there is a label starting in position 17, this is ok).");                                    
                                    }

                                    varName = v1;
                                    label = v2;
                                }
                                else
                                {
                                    new Error("Tsd read: the following name is malformed: " + varName + ". The name should contain letters, digits or underscore only");                                    
                                }
                            }

                            countdata = 0;
                            freq = G.ConvertFreq(frequency);
                            obs = GekkoTime.Observations(new GekkoTime(freq, d1, d1sub), new GekkoTime(freq, d2, d2sub));
                            obsLeft = obs;
                            ts = null;
                            if (Program.IsNonsenseVariableName(varName))
                            {
                                emptyWarnings++;
                                ts = new TimeSeries_1_1(freq, varName);  //completely phoney, will not live after exit of this method: just so that we can continue
                            }
                            else
                            {
                                ts = FindOrCreateTimeSeriesInDataBank(databank, varName, freq);
                            }
                            if (label != null && label != "") ts.label = label;
                            if (expr != null && expr != "") ts.source = expr;
                            if (stamp != null && stamp != "") ts.stamp = stamp;
                            //datalines = 0;
                            nextState = 3;
                        }

                    }
                    else if (nextState == 3)
                    {
                        int n = Math.Min(5, obsLeft);
                        for (int i5 = 0; i5 < n; i5++)
                        {
                            double ss = double.NaN;
                            bool success = false;
                            int width = 0;
                            if (isTsdx)
                            {
                                width = 21;
                            }
                            else
                            {
                                width = 15;
                            }
                            success = G.TryParseIntoDouble(line.Substring(ii + i5 * width, width), out ss);
                            if (!success)
                            {
                                string toParse = line.Substring(ii + i5 * width, width).Trim();
                                if (G.Equal(toParse, "NaN") || G.Equal(toParse, "-NaN"))
                                {
                                    ss = 1e+15;  //signals missing value
                                    NaNCounter++;
                                }
                                else
                                {
                                    new Error("" + varName + ": could not parse '" + toParse + "' as a number");
                                    //sr.Close();
                                    //throw new GekkoException();
                                }
                            }

                            if (ss == 1e+15)
                            {
                                ss = double.NaN;
                            }
                            else
                            {
                                if (Math.Abs(ss) < 1e-37 && ss != 0d)
                                {
                                    smallWarnings++;
                                    ss = 0d;  //numbers smaller than this become imprecise when imported from AREMOS
                                              //AREMOS sets all numbers > 1e+15 to 1e+15 when exporting, so no need to do this for large numbers
                                }
                            }
                            tempArray[countdata] = ss;
                            countdata++;
                            obsLeft--;
                        }

                        if (obsLeft == 0)
                        {
                            GekkoTime gt1 = new GekkoTime(freq, d1, d1sub);
                            GekkoTime gt2 = new GekkoTime(freq, d2, d2sub);

                            int offset = 0;
                            
                            int nob = GekkoTime.Observations(gt1, gt2);
                            if (nob > 0)
                            {
                                ts.SetDataSequence(gt1, gt2, tempArray, offset);
                                ts.Trim(); //to save ram                                                             
                            }
                            counter++;
                            nextState = 1;
                        }
                    }
                }  //end of readline from file
                readInfo.startPerInFile = d1min;
                readInfo.endPerInFile = d2max;
                readInfo.variables = counter;
                if (emptyWarnings > 0) new Warning(emptyWarnings + " variables with empty string as name in .tsd file (skipped)");
                if (smallWarnings > 0) new Warning(smallWarnings + " numbers numerically smaller than 1.0e-37 were set to 0");

            }
        }

        public static void ReadGbkOld_1_1(string databankName, string version, ReadOpenMulbkHelper oRead, Program.ReadInfo readInfo, ref string file, ref Databank_1_1 databank, string originalFilePath, string originalFilePathPretty, ref string tsdxFile, ref string tempTsdxPath, ref int NaNCounter)
        {
            //handles databank versions 1.0 and 1.1

            readInfo.fileName = originalFilePath; readInfo.fileNamePretty = originalFilePathPretty;  //TODO: is the last right?

            int type = -12345;
            string file2 = null;
            string[] array1 = Directory.GetFiles(tempTsdxPath);
            foreach (string s in array1)
            {
                string ext = Path.GetExtension(s);
                string s2 = Path.GetFileName(s);
                if (G.Equal(ext, ".xml")) continue;
                if (G.Equal(ext, ".tsd"))
                {
                    type = 2;
                    file2 = s;
                    break;
                }
                if (G.Equal(s2, Globals.protobufFileName) || G.Equal(s2, Globals.protobufFileName2) || G.Equal(s2, Program.options.databank_file_gbk_internal))
                {
                    type = 1;
                    file2 = s;
                    break;
                }
            }

            if (type == -12345)
            {
                new Error("Could not find data storage file inside zipped databank file. Troubleshooting, try this page: " + Globals.databankformatUrl);
                
                //throw new GekkoException();
            }

            if (type == 1)
            {
                using (FileStream fs = Program.WaitForFileStream(file2, null, Program.GekkoFileReadOrWrite.Read))
                {

                    //Databank_1_1 temp = null;
                    ////May take a little time to create: so use static serializer if doing serialize on a lot of small objects
                    //RuntimeTypeModel serializer = TypeModel.Create();
                    //serializer.UseImplicitZeroDefaults = false;  //otherwise an int that has default constructor value -12345 but is set to 0 will reappear as a -12345 (instead of 0). For int, 0 is default, false for bools etc.
                    try
                    {
                        DateTime dt3 = DateTime.Now;
                        databank = Serializer.Deserialize<Databank_1_1>(fs);
                        readInfo.variables = databank.storage.Count;
                        G.WritelnGray("Protobuf deserialize took: " + G.Seconds(dt3));
                    }
                    catch (Exception e)
                    {
                        new Error("Unexpected technical error when reading " + Globals.extensionDatabank + " databank in version 1.1 format (protobuffers). Message: " + e.Message + ". Troubleshooting, try this page: " + Globals.databankformatUrl + ".");                        
                    }
                    

                }  //end of using
            }

            else

            {
                databank = new Databank_1_1(databankName);
                //string xx1 = file.Replace("Is_a_protobuffer_file", "") + Path.GetFileNameWithoutExtension(originalFilePath) + ".tsd";
                //string xx2 = file.Replace("Is_a_protobuffer_file", "") + "databank.tsd";
                ReadAllTsdRecords(file2, oRead.Merge, true, databank, ref NaNCounter, readInfo);
                readInfo.nanCounter = NaNCounter;
            }

            int maxYearInProtobufFile = int.MinValue;
            int minYearInProtobufFile = int.MaxValue;
            int emptyWarnings = 0;
            foreach (TimeSeries_1_1 tsTemp in databank.storage.Values)  //for each timeseries in temp (deserialized) databank 
            {
                bool isGhost = tsTemp.IsGhost();
                //looping through each timeseries to find databank start and end year (and to merge variables if we are merging)

                if (Program.IsNonsenseVariableName(tsTemp.variableName))
                {
                    emptyWarnings++;
                    continue;
                }

                GekkoTime first = GekkoTime.tNull;
                GekkoTime last = GekkoTime.tNull;

                if (!tsTemp.IsTimeless())
                {
                    first = tsTemp.GetPeriodFirst();
                    last = tsTemp.GetPeriodLast();
                }

                if (!isGhost)
                {
                    maxYearInProtobufFile = G.GekkoMax(maxYearInProtobufFile, last.super);
                    minYearInProtobufFile = G.GekkoMin(minYearInProtobufFile, first.super);
                }

            }
            if (emptyWarnings > 0) new Warning(emptyWarnings + " variables with empty string as name in ." + Globals.extensionDatabank + " file (skipped)");

            readInfo.startPerInFile = minYearInProtobufFile;
            readInfo.endPerInFile = maxYearInProtobufFile;
        }
    }
}
